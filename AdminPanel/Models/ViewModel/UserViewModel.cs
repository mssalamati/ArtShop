using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminPanel.Models.ViewModel
{
    public class UserViewModel
    {
        public string id { get; set; }

        [Required]
        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "پست الکترونیک را وارد کنید")]
        [EmailAddress(ErrorMessage = ("فرمت پست الکترونیک  اشتیاه"))]
        [Display(Name = "پست الکترونیک")]
        public string Email { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public string RegisterDate { get; set; }
    }
}