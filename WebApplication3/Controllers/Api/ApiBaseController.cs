using Logik;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Http;

namespace WebApplication3.Controllers.Api
{
    public class ApiBaseController : ApiController
    {
        protected ApplicationDbContext db = new ApplicationDbContext();
        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
        [Authorize]
        protected ApplicationUser GetUser()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.Single(x => x.Id == userId);
                return user;
            }

            catch (Exception)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
                  
            }           
        }
    }
}