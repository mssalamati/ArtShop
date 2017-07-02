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
                        string filePath = string.Concat("/", path, "/", filename);
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
    }
}
