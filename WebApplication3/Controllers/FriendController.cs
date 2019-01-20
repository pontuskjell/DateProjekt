using Logik;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication3.Controllers
{
    public class FriendController : BaseController
    {
        [Authorize]
        public ActionResult AddFriend(string id)
        {
            try
            {
                var user = GetUser();

                var SenderProfile = GetProfileUser(user.Profile.Id); 
                var RecieverProfile = db.Profiles.Single(x => x.Id.ToString() == id);

                var earlierRequests = db.FriendsRequests.Where(x => x.ToProfile.Id == RecieverProfile.Id && x.FromProfile.Id == SenderProfile.Id && x.Status == "Waiting" || x.ToProfile.Id == SenderProfile.Id && x.FromProfile.Id == RecieverProfile.Id && x.Status == "Waiting").ToList();
                if (earlierRequests.Count == 0)
                {
                    var request = new FriendRequests
                    {
                        FromProfile = SenderProfile,
                        ToProfile = RecieverProfile
                    };
                    db.FriendsRequests.Add(request);
                    db.SaveChanges();
                }
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }
            catch (Exception)
            {

                return RedirectToAction("CatchError", "Base");
            }


        }
        [Authorize]
        public ActionResult OrderContacts()
        {
            try
            {
                var user = GetUser();
                var profile = GetProfileUser(user.Profile.Id); 
                var friends = db.FriendsRequests.Where(x => x.Status == "Confirmed" && x.ToProfile.Id == profile.Id || x.FromProfile.Id == profile.Id && x.Status == "Confirmed").ToList();
                var friendProfiles = new List<Profiles>();
                foreach (var item in friends)
                {
                    if (item.ToProfile.Id == profile.Id && item.FromProfile.Inactivate == false)
                    {
                        friendProfiles.Add(item.FromProfile);
                    }
                    else
                    {
                        if (item.FromProfile.Id == profile.Id && item.ToProfile.Inactivate == false)
                        {
                            friendProfiles.Add(item.ToProfile);
                        }
                    }
                }
                var kampsport = db.Interests.Single(x => x.Name == "Kampsport");
                var kampsportLista = friendProfiles.Where(x => x.Interests.Contains(kampsport)).ToList();
                var hockey = db.Interests.Single(x => x.Name == "Hockey");
                var hockeyLista = friendProfiles.Where(x => x.Interests.Contains(hockey)).ToList();
                var golf = db.Interests.Single(x => x.Name == "Golf");
                var golfLista = friendProfiles.Where(x => x.Interests.Contains(golf)).ToList();
                var fotboll = db.Interests.Single(x => x.Name == "Fotboll");
                var fotbollsLista = friendProfiles.Where(x => x.Interests.Contains(fotboll)).ToList();

                var orderContacts = new OrderContacts();
                orderContacts.Fotboll = fotbollsLista;
                orderContacts.Golf = golfLista;
                orderContacts.Hockey = hockeyLista;
                orderContacts.Kampsport = kampsportLista;
                return View(orderContacts);
            }
            catch (Exception)
            {

                return RedirectToAction("CatchError", "Base");
            }


        }
        [Authorize]
        public ActionResult FriendRequests()
        {
            try
            {
                var user = GetUser();
                var profile = GetProfileUser(user.Profile.Id); 

                var requests = db.FriendsRequests.Where(x => x.ToProfile.Id == profile.Id && x.Status == "Waiting").ToList();
                var senders = new List<Profiles>();
                foreach (var item in requests)
                {
                    senders.Add(item.FromProfile);
                }

                var userAndProfiles = new List<UserAndProfiles>();
                foreach (var item in senders)
                {
                    var userAndProfile = new UserAndProfiles();
                    userAndProfile.Profile = item;
                    userAndProfile.User = db.Users.Single(x => x.Profile.Id == item.Id);
                    userAndProfiles.Add(userAndProfile);
                }


                return View(userAndProfiles);
            }
            catch (Exception)
            {

                return RedirectToAction("CatchError", "Base");
            }


        }
        [Authorize]
        public ActionResult ConfirmFriend(string id)
        {
            try
            {
                var user = GetUser();
                var profile = GetProfileUser(user.Profile.Id); 
                var sender = db.Profiles.Single(x => x.Id.ToString() == id);
                var id1 = profile.Id;
                var id2 = sender.Id;
                var friendRequest = db.FriendsRequests.Single(x => x.ToProfile.Id == profile.Id && x.FromProfile.Id == sender.Id);
                friendRequest.Status = "Confirmed";
                db.SaveChanges();
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }
            catch (Exception)
            {

                return RedirectToAction("CatchError", "Base");
            }


        }
        [Authorize]
        public ActionResult DeclineFriend(string id)
        {
            try
            {
                var user = GetUser();
                var profile = GetProfileUser(user.Profile.Id); 
                var sender = db.Profiles.Single(x => x.Id.ToString() == id);
                var friendRequest = db.FriendsRequests.Single(x => x.ToProfile.Id == profile.Id && x.FromProfile.Id == sender.Id && x.Status == "Waiting");
                friendRequest.Status = "Declined";
                db.SaveChanges();
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }
            catch (Exception)
            {

                return RedirectToAction("CatchError", "Base");
            }


        }
        [Authorize]
        public PartialViewResult Contacts()
        {
            try
            {
                var user = GetUser();
                var profile = GetProfileUser(user.Profile.Id); 
                var id = profile.Id;
                var friends = db.FriendsRequests.Where(x => x.Status == "Confirmed" && x.ToProfile.Id == profile.Id || x.FromProfile.Id == profile.Id && x.Status == "Confirmed").ToList();
                var friendProfiles = new List<Profiles>();
                foreach (var item in friends)
                {
                    if (item.ToProfile.Id == profile.Id && item.FromProfile.Inactivate == false)
                    {
                        friendProfiles.Add(item.FromProfile);
                    }
                    else
                    {
                        if (item.FromProfile.Id == profile.Id && item.ToProfile.Inactivate == false)
                        {
                            friendProfiles.Add(item.ToProfile);
                        }
                    }
                }
                return PartialView(friendProfiles);
            }
            catch (Exception)
            {

                return PartialView();
            }


        }
        [Authorize]
        public PartialViewResult RequestNumbers()
        {
            try
            {
                var user = GetUser();
                var profile = GetProfileUser(user.Profile.Id); 
                var requests = db.FriendsRequests.Where(x => x.ToProfile.Id == profile.Id && x.Status == "Waiting").ToList().Count;
                var text = new MessageIndex();
                if (requests > 0)
                {
                    text.Text = $"Friend Requests({requests})";
                }
                else
                {
                    text.Text = "Friend Requests";
                }
                return PartialView(text);
            }
            catch (Exception)
            {
                return PartialView();
            }


        }
    }
}
