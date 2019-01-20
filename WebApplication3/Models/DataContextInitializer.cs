
using Logik;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;


namespace WebApplication3
{
    public class DataContextInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {

        protected override void Seed(ApplicationDbContext context)
        {
            //    var store = new UserStore<ApplicationUser>(context);
            //    var userManager = new ApplicationUserManager(store);

            //    //IdentityUser/USER
            //    var user = new ApplicationUser { UserName = "Jesper@gmail.com", Email = "Jesper@gmail.com", Name = "Aleksandar Smiljanic", City = "Örebro", Gender = "Male", BirthDate = DateTime.Now };
            //    userManager.CreateAsync(user, "#1245Hej").Wait();
            //    var user2 = new ApplicationUser { UserName = "Pontus@gmail.com", Email = "Pontus@gmail.com", Name = "Aleksandar Smiljanic", City = "Örebro", Gender = "Male", BirthDate = DateTime.Now };
            //    userManager.CreateAsync(user2, "#1245Hej").Wait();

            //    //Profiles
            //    var Profil1 = new Profiles { Description = "Trevlig, Snygg, fit kille som älskar att träna" };
            //    var Profil2 = new Profiles { Description = "Trevlig, Snygg" };

            //    user.Profile = Profil1;
            //    user2.Profile = Profil2;

            //    context.Profiles.Add(Profil1);
            //    context.Profiles.Add(Profil2);

            var fotboll = new Interests
        {
            Name = "Fotboll"
        };
        var Golf = new Interests
        {
            Name = "Golf"
        };
        var Kampsport = new Interests
        {
            Name = "KampSport"
        };
        var Hockey = new Interests
        {
            Name = "Hockey"
        };
            context.Interests.Add(fotboll);
            context.Interests.Add(Golf);
            context.Interests.Add(Kampsport);
            context.Interests.Add(Hockey);

        //    //TopVisited
        //    context.TopVisited.Add(new TopVisited { Visitor = Profil1, Profile = Profil2, DateVisited = DateTime.Now });
        //    context.TopVisited.Add(new TopVisited { Visitor = Profil2, Profile = Profil1, DateVisited = DateTime.Now });

            base.Seed(context);
    }
}
}