using Logik;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace WebApplication3.Controllers
{
    public class HomeController : BaseController
    {

        public PartialViewResult Singlar()
        {
            try
            {
                var AllaSinglar = db.Profiles.Take(10).Where(x => x.AllowSearch == true && x.Inactivate == false).ToList();
                var userId = User.Identity.GetUserId();
                if (!String.IsNullOrEmpty(userId))
                {
                    var user = GetUser();
                    var profile = GetProfileUser(user.Profile.Id); 
                    AllaSinglar.Remove(profile);
                }


                return PartialView(AllaSinglar);
            }
            catch (Exception)
            {

                return PartialView();
            }

        }

        [Authorize]
        public ActionResult GetProfile(int id)
        {
            try
            {
                var user = GetUser();
                var currentProfile = GetProfileUser(user.Profile.Id); 
                if (currentProfile.Id == id)
                {
                    return RedirectToAction("Index", "Home");
                }
                var profile = GetProfileUser(id); 
                var friends = db.FriendsRequests.Where(x => x.ToProfile.Id == profile.Id && x.FromProfile.Id == currentProfile.Id && x.Status == "Confirmed" || x.ToProfile.Id == currentProfile.Id && x.FromProfile.Id == profile.Id && x.Status == "Confirmed").ToList();
                var ProfileAndFriend = new ProfileAndFriend
                {
                    Profile = profile
                };
                if (friends.Count == 0)
                {
                    ProfileAndFriend.FriendOrNot = false;
                }
                else
                {
                    ProfileAndFriend.FriendOrNot = true;
                }
                SaveProfileVisited(profile);
                return View(ProfileAndFriend);
            }
            catch (Exception)
            {
                return RedirectToAction("CatchError", "Base");
            }

        }

        public void SaveProfileVisited(Profiles visitedProfile)
        {
            try
            {
                var user = GetUser();
                var currentProfile = GetProfileUser(user.Profile.Id);
                var topVisitors = db.TopVisited.Where(x => x.Profile.Id == visitedProfile.Id).ToList();


                bool match = false;
                bool stop = false;
                if (topVisitors.Count < 5)
                {
                    foreach (var item in topVisitors)
                    {
                        if (item.Visitor.Id == currentProfile.Id)
                        {
                            item.DateVisited = DateTime.Now;
                            match = true;
                            stop = true;
                        }
                    }
                    if (!match)
                    {
                        var visitor = new TopVisited
                        {
                            Profile = visitedProfile,
                            Visitor = currentProfile,
                            DateVisited = DateTime.Now

                        };
                        stop = true;
                        db.TopVisited.Add(visitor);
                    }
                }

                if (!stop)
                {
                    foreach (var item in topVisitors)
                    {
                        if (item.Visitor.Id == currentProfile.Id)
                        {
                            item.DateVisited = DateTime.Now;
                            match = true;
                        }
                    }
                    if (!match)
                    {
                        DateTime? dateTime = DateTime.Now;
                        TopVisited visitor = null;
                        foreach (var item in topVisitors)
                        {
                            if (item.DateVisited < dateTime)
                            {
                                dateTime = item.DateVisited;
                                visitor = item;
                            }
                        }
                        db.TopVisited.Remove(visitor);
                        var newVisitor = new TopVisited
                        {
                            Profile = visitedProfile,
                            Visitor = currentProfile,
                            DateVisited = DateTime.Now
                        };
                        db.TopVisited.Add(newVisitor);
                    }
                }

                db.SaveChanges();
            }
            catch (Exception)
            {

            }

        }

        public ActionResult Index()
        {
            try
            {
                var loggedIn = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
                if (loggedIn)
                {
                    var userName = User.Identity.Name;
                    var user = db.Users.Include(x => x.Profile).Single(x => x.UserName == userName);
                    var profile = user.Profile;
                    return View(profile);
                }
                else return RedirectToAction("Login", "Account");
            }
            catch (Exception)
            {

                return RedirectToAction("CatchError", "Base");
            }

        }
    }
}