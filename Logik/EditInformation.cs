using Logik.Languages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Logik
{
    public class EditInformation
    {
        [Display(Name = nameof(Resources.Name), ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources),
          ErrorMessageResourceName = nameof(Resources.Required))]
        [StringLength(60, MinimumLength = 3, ErrorMessageResourceType = typeof(Resources),
                  ErrorMessageResourceName = nameof(Resources.Length))]
        public string Name { get; set; }
        [Display(Name = nameof(Resources.City), ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources),
     ErrorMessageResourceName = nameof(Resources.Required))]
        [StringLength(60, MinimumLength = 3, ErrorMessageResourceType = typeof(Resources),
             ErrorMessageResourceName = nameof(Resources.Length))]
        public string City { get; set; }
        [Display(Name = nameof(Resources.Gender), ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources),
    ErrorMessageResourceName = nameof(Resources.Required))]
        public string Gender { get; set; }

        public IEnumerable<SelectListItem> genders { get; set; }
    }
}
