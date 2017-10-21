using ImageServer.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace ImageServer.Controllers
{
    public class UploadController : Controller
    {
        //server info
        public JsonResult info()
        {
            string ip = Request.UserHostAddress;
            var validIP = ConfigurationManager.AppSettings["validIP"];
            var rootFolder = ConfigurationManager.AppSettings["rootFolder"];
            var ImageServerName = ConfigurationManager.AppSettings["ImageServerName"];
            var mainDomain = ConfigurationManager.AppSettings["mainDomain"];
            var maxStoreage = long.Parse(ConfigurationManager.AppSettings["maxStoreage"]);
            var path = Server.MapPath("~" + rootFolder);
            var length = GetDirectorySize(path + "/Equal");
            length += GetDirectorySize(path + "/Orginal");
            length += GetDirectorySize(path + "/Squre");
            length += GetDirectorySize(path + "/Wide");
            return Json(new
            {
                diskUsage = length + 1,
                maxStoreage = maxStoreage,
                mainDomain = mainDomain,
                Name = ImageServerName,
                SSAllowPicture = true || length < maxStoreage,
                IP = "valid"
            }, JsonRequestBehavior.AllowGet);
        }

        //get directory size
        private long GetDirectorySize(string p)
        {
            string[] a = Directory.GetFiles(p, "*.*");
            long b = 0;
            foreach (string name in a)
            {
                FileInfo info = new FileInfo(name);
                b += info.Length;
            }
            return b;
        }

        //remove on demand
        public JsonResult rm()
        {
            string ip = Request.UserHostAddress;

            return Json("" + ip, JsonRequestBehavior.AllowGet);
        }

        //false if not exist true if exist and dont remove later
        public JsonResult verify()
        {
            string ip = Request.UserHostAddress;

            return Json("" + ip, JsonRequestBehavior.AllowGet);
        }

        //check file exist or not
        public JsonResult exist(string path)
        {
            string ip = Request.UserHostAddress;

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //resize picture and save
        [HttpPost]
        public ActionResult Resize(ResizeViewModel model)
        {
            model = model.square_width == 0 || model.wide_width == 0 || model.square_height == 0 || model.wide_height == 0 ? new ResizeViewModel()
            {
                image = Request["image"],
                wide_x = float.Parse(Request["wide_x"].Replace(".", "/")),
                wide_height = float.Parse(Request["wide_height"].Replace(".", "/")),
                wide_width = float.Parse(Request["wide_width"].Replace(".", "/")),
                wide_y = float.Parse(Request["wide_y"].Replace(".", "/")),
                square_height = float.Parse(Request["square_height"].Replace(".", "/")),
                square_width = float.Parse(Request["square_width"].Replace(".", "/")),
                square_x = float.Parse(Request["square_x"].Replace(".", "/")),
                square_y = float.Parse(Request["square_y"].Replace(".", "/")),
            } : model;

            try
            {
                var ImageServerName = ConfigurationManager.AppSettings["ImageServerName"];
                var mainDomain = ConfigurationManager.AppSettings["mainDomain"];
                Uri mainuri = new Uri("https://" + ImageServerName + "." + mainDomain);
                Uri imageuri = new Uri(model.image);
                string image = mainuri.MakeRelativeUri(imageuri).ToString();
                var result = ImageHelper.Crop(Server, image, model.square_x, model.square_y, model.square_width, model.square_height, model.wide_x, model.wide_y, model.wide_width, model.wide_height);
                if (result.ResultStatus)
                {
                    return Json(new
                    {
                        result = true,
                        SqureFullPath = "https://" + ImageServerName + "." + mainDomain + result.SqureFullPath,
                        WideFullPath = "https://" + ImageServerName + "." + mainDomain + result.WideFullPath
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, error = result.Error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        //upload
        [HttpPost]
        public ActionResult upload(HttpPostedFileBase file)
        {
            try
            {
                var rootFolder = ConfigurationManager.AppSettings["rootFolder"];
                var ImageServerName = ConfigurationManager.AppSettings["ImageServerName"];
                var mainDomain = ConfigurationManager.AppSettings["mainDomain"];
                string OrginalFolderName = rootFolder + "/Orginal";
                string thumbPath = rootFolder + "/Equal";
                var result = ImageHelper.Saveimage(Server, file, OrginalFolderName, ImageHelper.saveImageMode.Not);
                if (!result.ResultStatus)
                    return Json(new { result = false, data = result.Error });
                var saveResult = ImageHelper.saveThumb(Server, result.FullPath, thumbPath);
                if (!saveResult.ResultStatus)
                    return Json(new { result = false, data = saveResult.Error }, JsonRequestBehavior.AllowGet);
                return Json(new
                {
                    result = true,
                    data = "https://" + ImageServerName + "." + mainDomain + saveResult.FullPath,
                    width = result.Width,
                    height = result.Height
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, data = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public class base64img { public string raw { get; set; } }
        [HttpPost]
        public ActionResult uploadBase64(base64img model)
        {
            try
            {
                string file = model.raw;
                var rootFolder = ConfigurationManager.AppSettings["rootFolder"];
                var ImageServerName = ConfigurationManager.AppSettings["ImageServerName"];
                var mainDomain = ConfigurationManager.AppSettings["mainDomain"];
                string OrginalFolderName = rootFolder + "/Orginal";
                string thumbPath = rootFolder + "/Equal";
                var result = ImageHelper.Saveimage(Server, file, OrginalFolderName, ImageHelper.saveImageMode.Not);

                if (!result.ResultStatus)
                    return Json(new { result = false, data = result.Error });
                var saveResult = ImageHelper.saveThumb(Server, result.FullPath, thumbPath);
                if (!saveResult.ResultStatus)
                    return Json(new { result = false, data = saveResult.Error }, JsonRequestBehavior.AllowGet);
                return Json(new
                {
                    result = true,
                    data = "https://" + ImageServerName + "." + mainDomain + saveResult.FullPath,
                    width = result.Width,
                    height = result.Height
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, data = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}