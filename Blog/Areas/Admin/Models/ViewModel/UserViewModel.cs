using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Areas.Admin.Models.ViewModel
{
    public class UserViewModel
    {
        public string id { get; set; }

        [Required]
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "نام کاربری")]
        public string Username { get; set; }

        [Required(ErrorMessage = "پست الکترونیک را وارد کنید")]
        [EmailAddress(ErrorMessage = ("فرمت پست الکترونیک  اشتیاه"))]
        [Display(Name = "پست الکترونیک")]
        public string Email { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public string RegisterDate { get; set; }
    }
}