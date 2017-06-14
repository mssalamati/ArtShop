using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class ValueController : BaseController
    {
        public JsonResult GetCategories()
        {
            var list = db.Categories.Select(x => 
            new { id = x.Id, name = x.Translations.FirstOrDefault(y => y.languageId == "en").Name });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMaterials()
        {
            var list = db.Materials.Select(x =>
            new { id = x.Id, name = x.Translations.FirstOrDefault(y => y.languageId == "en").Name });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMediums()
        {
            var list = db.Mediums.Select(x =>
            new { id = x.Id, name = x.Translations.FirstOrDefault(y => y.languageId == "en").Name });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStyles()
        {
            var list = db.Styles.Select(x =>
            new { id = x.Id, name = x.Translations.FirstOrDefault(y => y.languageId == "en").Name });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubjects()
        {
            var list = db.Subjects.Select(x =>
            new { id = x.Id, name = x.Translations.FirstOrDefault(y => y.languageId == "en").Name });
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}