using System.ComponentModel.DataAnnotations;


namespace Logik
{

    public class Messages
    {
        public int Id { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Text { get; set; }
        public virtual Profiles ProfileAuthor { get; set; }
        public virtual Profiles ProfileReceiver { get; set; }
        public string Filename { get; set; }
        public string ContentType { get; set; }
        public byte[] File { get; set; }
    }
}
