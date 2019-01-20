using Logik.Languages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Logik
{
    public class Profiles
    {
        private void SetDefaultPicture()
        {
            var webClient = new WebClient();
            byte[] imageBytes = webClient.DownloadData("https://x1.xingassets.com/assets/frontend_minified/img/users/nobody_m.original.jpg");
            Picture = imageBytes;
        }

        public Profiles()
        {
            Interests = new HashSet<Interests>();
            SetDefaultPicture();
        }

        public int Id { get; set; }
        [Display(Name = nameof(Resources.Description), ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources),
           ErrorMessageResourceName = nameof(Resources.Required))]
        public string Description { get; set; }
        public bool AllowSearch { get; set; } = true;
        public bool Inactivate { get; set; } = false;
        public byte[] Picture { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Interests> Interests { get; set; }
        public virtual ICollection<FriendRequests> FriendRequests { get; set; }
        public virtual ICollection<Messages> Message { get; set; }
        public virtual ICollection<TopVisited> TopVisitors { get; set; }
    }
}