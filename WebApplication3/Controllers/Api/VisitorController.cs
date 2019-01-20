using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Http;


namespace WebApplication3.Controllers.Api
{
    public class Visitors
    {
        public string Name { get; set; }
    }

    public class VisitorController : ApiBaseController
    {
        
        [System.Web.Http.HttpGet]
        [System.Web.Http.Authorize]
        public List<Visitors> VisitorList()
        {
            var user = GetUser();

            return db.TopVisited.Include(x => x.Profile).Include(x => x.Profile.User).Where(x => x.Profile.User.Id == user.Id).OrderByDescending(x => x.DateVisited).Select(visitor => new Visitors
            {

                Name = visitor.Visitor.User.Name

            }).ToList();
        }
    }
}