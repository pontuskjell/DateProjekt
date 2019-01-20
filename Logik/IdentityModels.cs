using Logik.Languages;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Logik
{
    
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "Birth date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> BirthDate { get; set; }

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


        public virtual Profiles Profile { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {

            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("ApplicationDbContext", throwIfV1Schema: false)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Profiles>().HasMany(x => x.Message).WithRequired(x => x.ProfileReceiver);
            modelBuilder.Entity<Profiles>().HasMany(x => x.FriendRequests).WithRequired(x => x.ToProfile);
            modelBuilder.Entity<Profiles>().HasMany(x => x.TopVisitors).WithRequired(x => x.Profile);
            modelBuilder.Entity<Interests>().HasMany(x => x.Profiles).WithMany(x => x.Interests).Map(xx => { xx.MapLeftKey("InterestId"); xx.MapRightKey("ProfileId"); xx.ToTable("InterestsProfiles"); });
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>()
                        .HasOptional(s => s.Profile)
                        .WithRequired(ad => ad.User);
        }
        //Tabeller databas
        public DbSet<Profiles> Profiles { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<Interests> Interests { get; set; }
        public DbSet<FriendRequests> FriendsRequests { get; set; }
        public DbSet<TopVisited> TopVisited { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}