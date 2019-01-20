using Logik;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace WebApplication3.Controllers
{
    public class PartnersController : BaseController
    {
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                var user = GetUser();
                var profile = GetProfileUser(user.Profile.Id); 
                var profileInterests = profile.Interests.ToList();
                if (profileInterests.Count == 0)
                {
                    return RedirectToAction("noMatches", "Partners");
                }
                else
                {
                    var people = db.Profiles.Include(x => x.User).Where(x => x.AllowSearch == true && x.Inactivate == false && !(x.Id == profile.Id)).
                        Select(userProfile => new UserAndProfiles { User = userProfile.User, Profile = userProfile }).ToList();
                    foreach (var item in profileInterests)
                    {
                        foreach (var x in people.ToList())
                        {
                            if (!x.Profile.Interests.Contains(item))
                                people.Remove(x);
                        }
                    }
                    if (people.Count == 0)
                    {
                        return RedirectToAction("noMatches", "Partners");
                    }

                    return View(people);
                }
            }
            catch (Exception)
            {

                return RedirectToAction("CatchError", "Base");
            }

        }

        public ActionResult noMatches()
        {
            return View();
        }
        [Authorize]
        public ActionResult Match(int id, string url)
        {
            try
            {
                var user = GetUser();
                var profile = GetProfileUser(user.Profile.Id); 
                var profileInterests = profile.Interests.ToList();
                var matchingProfile = db.Profiles.Single(x => x.Id == id);
                decimal procent = 0;
                foreach (var item in profileInterests)
                {
                    if (matchingProfile.Interests.Contains(item))
                    {
                        procent += 0.25m;
                    }
                }
                procent = procent * 100;
                var matchProcent = new Match
                {
                    procent = Convert.ToInt32(procent),
                    url = url
                };
                return View(matchProcent);
            }
            catch (Exception)
            {

                return RedirectToAction("CatchError", "Base");
            }

        }
    }
}