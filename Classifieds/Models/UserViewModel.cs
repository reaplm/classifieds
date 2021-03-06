using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Classifieds.Web.Models
{
    public class UserViewModel
    {

        public long ID { set; get; }

        [Display(Name = "Activated")]
        public int? Activated { set; get; }
        public String ActivationCode { set; get; }
        public int? Notified { set; get; }

        [Required(ErrorMessage = "Please enter email address")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public String Email { set; get; }

        public String Password { set; get; }

        [Display(Name = "Last Login")]
        public DateTime? LastLogin { set; get; }

        [Display(Name = "Registered")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public DateTime RegDate { set; get; }
        public DateTime? PasswordExpiry { set; get; }

        public int IsVerified { set; get; }
        public string VerificationToken { set; get; }

        public virtual UserDetailViewModel UserDetail{set;get;}
        public List<LikeViewModel> Likes { set; get; }
        public virtual List<RoleViewModel> Roles { set; get; }
        public virtual  List<NotificationViewModel> Notifications { set; get; }

        [Display(Name ="Roles")]
        public string RoleList
        {
            get
            {
                return Roles == null ? null : string.Join(",", Roles.Select(x => x.Name).ToList());
            }
        }

        public int Days
        {
            get
            {
                var today = DateTime.Now;
                TimeSpan days = today - RegDate;
                return days.Days;
            }
        }
    }
}
