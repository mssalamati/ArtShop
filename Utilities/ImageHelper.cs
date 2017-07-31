using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utilities
{
    public static class ImageHelper
    {
        public static byte[] CropImage(byte[] content, int x, int y, int width, int height)
        {
            using (MemoryStream stream = new MemoryStream(content))
            {
                return CropImage(stream, x, y, width, height);
            }
        }

        public static byte[] CropImage(Stream content, int x, int y, int width, int height)
        {
            //Parsing stream to bitmap
            using (Bitmap sourceBitmap = new Bitmap(content))
            {
                //Get new dimensions
                double sourceWidth = Convert.ToDouble(sourceBitmap.Size.Width);
                double sourceHeight = Convert.ToDouble(sourceBitmap.Size.Height);
                Rectangle cropRect = new Rectangle(x, y, width, height);

                //Creating new bitmap with valid dimensions
                using (Bitmap newBitMap = new Bitmap(cropRect.Width, cropRect.Height))
                {
                    using (Graphics g = Graphics.FromImage(newBitMap))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        g.CompositingQuality = CompositingQuality.HighQuality;

                        g.DrawImage(sourceBitmap, new Rectangle(0, 0, newBitMap.Width, newBitMap.Height), cropRect, GraphicsUnit.Pixel);

                        return GetBitmapBytes(newBitMap);
                    }
                }
            }
        }

        public static byte[] GetBitmapBytes(Bitmap source)
        {
            //Settings to increase quality of the image
            ImageCodecInfo codec = ImageCodecInfo.GetImageEncoders()[4];
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);

            //Temporary stream to save the bitmap
            using (MemoryStream tmpStream = new MemoryStream())
            {
                source.Save(tmpStream, codec, parameters);

                //Get image bytes from temporary stream
                byte[] result = new byte[tmpStream.Length];
                tmpStream.Seek(0, SeekOrigin.Begin);
                tmpStream.Read(result, 0, (int)tmpStream.Length);

                return result;
            }
        }

        public class CropResult
        {
            public string SqureFullPath;
            public string WideFullPath;
            public bool ResultStatus;
            public string Error;
        }

        public static CropResult Crop(HttpServerUtilityBase server, string RowPath, float SQ_x, float SQ_y, float SQ_width, float SQ_height, float WD_x, float WD_y, float WD_width, float WD_height)
        {
            string sq_path = "Upload/products/Squre";
            string wide_path = "Upload/products/Wide";
            string act_sqpath = server.MapPath("~/" + sq_path);
            string act_widepath = server.MapPath("~/" + wide_path);
            string Path = server.MapPath("~/" + RowPath);
            if (!File.Exists(Path))
                return new CropResult() { Error = "File Not Exist", ResultStatus = false };
            string filename = System.IO.Path.GetFileName(Path);
            //create directory if need
            if (!FileHelper.CreateFolderIfNeeded(act_sqpath))
                return new CropResult() { Error = "can not create directory", ResultStatus = false };
            if (!FileHelper.CreateFolderIfNeeded(act_widepath))
                return new CropResult() { Error = "can not create directory", ResultStatus = false };

            string act_sqpath_with_name = System.IO.Path.Combine(act_sqpath, filename);
            string act_widepath_with_name = System.IO.Path.Combine(act_widepath, filename);

            try
            {
                Image sq, wide;
                using (Image image = Image.FromFile(Path))
                {
                    sq = ResizeImage(image, SQ_x, SQ_y, SQ_width, SQ_height);
                    wide = ResizeImage(image, WD_x, WD_y, WD_width, WD_height);
                }
                SaveJpeg(act_sqpath_with_name, sq);
                SaveJpeg(act_widepath_with_name, wide);

                return new CropResult()
                {
                    SqureFullPath = string.Concat("/", sq_path, "/", filename),
                    WideFullPath = string.Concat("/", wide_path, "/", filename),
                    ResultStatus = true
                };
            }
            catch (Exception ex)
            {
                return new CropResult() { Error = ex.ToString(), ResultStatus = false };
            }
        }

        public static SaveImageResult Saveimage(HttpServerUtilityBase server, HttpPostedFileBase file, string path, saveImageMode mode = saveImageMode.Not)
        {
            if (file != null && file.ContentLength != 0)
            {
                var img = Image.FromStream(file.InputStream, true, true);
                if (mode == saveImageMode.Squre && img.Width != img.Height)
                    return new SaveImageResult() { Error = "this is not squre picture", FullPath = "", ResultStatus = false };
                if (mode == saveImageMode.wide && img.Width <= img.Height)
                    return new SaveImageResult() { Error = "this is not wide picture", FullPath = "", ResultStatus = false };
                if (mode == saveImageMode.portrait && img.Width >= img.Height)
                    return new SaveImageResult() { Error = "this is not portrait picture", FullPath = "", ResultStatus = false };

                string tempFolderPath = server.MapPath("~/" + path);
                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        file.SaveAs(Path.Combine(tempFolderPath, filename));
                        string filePath = string.Concat(path, "/", filename);
                        return new SaveImageResult() { Error = "", FullPath = filePath, ResultStatus = true, Width = img.Width, Height = img.Height };
                    }
                    catch (Exception ex)
                    {
                        return new SaveImageResult() { Error = ex.ToString(), FullPath = "", ResultStatus = false };
                    }
                }
                else
                    return new SaveImageResult() { Error = "can not create directory", FullPath = "", ResultStatus = false };
            }
            else
                return new SaveImageResult() { Error = "file is null", FullPath = "", ResultStatus = false };
        }

        public enum saveImageMode { Not, Squre, portrait, wide }
        public class SaveImageResult
        {
            public string FullPath;
            public string Error;
            public bool ResultStatus;
            public int Width;
            public int Height;
        }

        public static Image ResizeImage(Image image, float x, float y, float newWidth, float newHeight)
        {
            var res = new Bitmap(image);
            res = res.Clone(new Rectangle((int)x, (int)y, (int)newWidth, (int)newHeight), res.PixelFormat);
            //using (var graphic = Graphics.FromImage(res))
            //{
            //    graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //    graphic.SmoothingMode = SmoothingMode.HighQuality;
            //    graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            //    graphic.CompositingQuality = CompositingQuality.HighQuality;
            //    graphic.DrawImage(image, x, y, newWidth, newHeight);
            //}
            return res;
        }

        public static SaveImageResult saveThumb(HttpServerUtilityBase server, string Orginalpath, string thumbPath)
        {
            string tempFolderPath = server.MapPath("~" + thumbPath);

            try
            {
                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    using (Image image = Image.FromFile(server.MapPath("~" + Orginalpath)))
                    {
                        var qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50L);
                        var jpegCodec = GetEncoderInfo("image/jpeg");
                        var encoderParams = new EncoderParameters(1);
                        encoderParams.Param[0] = qualityParam;
                        image.Save(tempFolderPath + "/" + Path.GetFileName(Orginalpath), jpegCodec, encoderParams);
                        return new SaveImageResult() { ResultStatus = true, FullPath = thumbPath + "/" + Path.GetFileName(Orginalpath) };
                    }
                }
                else
                    return new SaveImageResult() { ResultStatus = false, Error = "can not create folder for thumb" };
            }
            catch (Exception ex)
            {
                return new SaveImageResult() { ResultStatus = false, Error = ex.ToString() };
            }
        }

        public static void SaveJpeg(string path, Image img)
        {
            var qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
            var jpegCodec = GetEncoderInfo("image/jpeg");
            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            img.Save(path, jpegCodec, encoderParams);
        }
        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            return ImageCodecInfo.GetImageEncoders().FirstOrDefault(t => t.MimeType == mimeType);
        }
    }
}
