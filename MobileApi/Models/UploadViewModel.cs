using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace MobileApi.Models
{
    public class UploadViewModel
    {
        public int id { get; set; }
        [Required]
        public string LanguageCode { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public int categoryId { get; set; }
        [Required]
        public int createDate { get; set; }
        [Required]
        public bool isforsale { get; set; }
        [Required]
        public string mediums { get; set; }
        [Required]
        public int[] materials { get; set; }
        [Required]
        public string styles { get; set; }
        [Required]
        public string keywords { get; set; }
        [Required]
        public float Height { get; set; }
        [Required]
        public float Width { get; set; }
        [Required]
        public float Depth { get; set; }
        public float weight { get; set; }
        [Required]
        public string Title { get; set; }
        public int Limitform { get; set; }
        public int LimitOf { get; set; }
        [Required]
        public string Description { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public int Country { get; set; }
        public string Region { get; set; }
        public string Zipcode { get; set; }
        public string Phonenumber { get; set; }
        public decimal Price { get; set; }
        [Required]
        public sizeMV SqrResizeRect { get; set; }
        [Required]
        public sizeMV WideResizeRect { get; set; }  
        public HttpPostedFileBase Image { get; set; }
        [Required]
        public string imageUpload { get; set; }
        public Productpackage Packaging { get; set; }
        public ProductStatus Status { get; set; }
    }


    public class sizeMV
    {
        [Required]
        public int x { get; set; }
        [Required]
        public int y { get; set; }
        [Required]
        public int width { get; set; }
        [Required]
        public int height { get; set; }

    }

    public class upoadNowResult
    {
        public int id { get; set; }
        public string error { get; set; }
        public bool success { get; set; }
        public upoadNowResult()
        {

        }
        public upoadNowResult(int id, string error, bool success)
        {
            this.id = id;
            this.error = error;
            this.success = success;
        }
    }
}