using System.Linq;
using System.Web.Http;
using Logik;

namespace WebApplication3.Controllers.Api
{
    public class PostViewModel
    {
        public string Text { get; set; }
        public string ProfileReceiverId { get; set; }
    }

    public class PostsController : ApiBaseController
    {
        

        [HttpPost]
        public void Create(PostViewModel posts)
        {

            var user = GetUser();
            var profile = db.Profiles.Single(x => x.Id.ToString() == posts.ProfileReceiverId);

            var messages = new Messages
            {
                Text = posts.Text,
                ProfileAuthor = user.Profile,
                ProfileReceiver = profile,
            };

            db.Messages.Add(messages);
            db.SaveChanges();
        }

    }
}