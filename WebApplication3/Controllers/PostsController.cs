using Logik;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication3.Controllers
{
    public class PostsController : BaseController
    {
        [Authorize]
        public ActionResult Index(string id)
        {
            try
            {
                var user = GetUser();
                var profile = GetProfileUser(user.Profile.Id); 

                var posts = db.Messages.Include(x => x.ProfileAuthor.User).Where(x => x.ProfileReceiver.Id.ToString() == id).OrderByDescending(x => x.Id).ToList();
                return View(new PostIndexViewModel { Id = id, Posts = posts, Profile = profile });
            }
            catch (Exception)
            {

                return RedirectToAction("CatchError", "Base");
            }


        }
        [Authorize]
        public ActionResult Create(int id)
        {
            try
            {
                var profile = GetProfileUser(id); 
                var messages = new Messages
                {
                    ProfileReceiver = profile
                };


                return View(messages);
            }
            catch (Exception)
            {

                return RedirectToAction("CatchError", "Base");
            }

        }
        [Authorize]
        public ActionResult Delete(int id)
        {
            try
            {
                var idt = db.Messages.Where(m => m.Id == id);
                db.Messages.RemoveRange(idt);
                db.SaveChanges();
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }
            catch (Exception)
            {

                return RedirectToAction("CatchError", "Base");
            }

        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(Messages messages, int id, HttpPostedFileBase upload)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(messages);
                }
                var userName = GetUser();

                var user = db.Profiles.Single(x => x.Id == userName.Profile.Id);

                messages.ProfileAuthor = user;

                var toUser = db.Profiles.Single(x => x.Id == id);
                messages.ProfileReceiver = toUser;

                db.Messages.Add(messages);


                if (upload != null && upload.ContentLength > 0)
                {
                    messages.Filename = upload.FileName;
                    messages.ContentType = upload.ContentType;

                    using (var reader = new BinaryReader(upload.InputStream))
                    {
                        messages.File = reader.ReadBytes(upload.ContentLength);
                    }
                }

                db.SaveChanges();


                return RedirectToAction("GetProfile", "Home", new { id });
            }
            catch (Exception)
            {

                return RedirectToAction("CatchError", "Base");
            }

        }
        [Authorize]
        public ActionResult Image(int id)
        {
            try
            {
                var post = db.Messages.Single(x => x.Id == id);
                return File(post.File, post.ContentType);
            }
            catch (Exception)
            {
                return RedirectToAction("CatchError", "Base");
            }

        }
    }

    public class PostIndexViewModel
    {
        public string Id { get; set; }
        public Profiles Profile { get; set; }
        public ICollection<Messages> Posts { get; set; }
    }
}