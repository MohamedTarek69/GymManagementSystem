using GymManagementBLL.MappingProfiles;
using GymManagementBLL.Services.AttachmentService.Class;
using GymManagementBLL.Services.AttachmentService.Interface;
using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Data.DataSeed;
using GymManagementDAL.Entities;
using GymManagementDAL.Repository.Classes;
using GymManagementDAL.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagementPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                //options.UseSqlServer("Server=.;Database=GymManagementDB;Trusted_Connection=true;TrustServerCertificate=true");

                //options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["DefualtConnection"]);
                //options.UseSqlServer(builder.Configuration["ConnectionStrings:DefualtConnection"]);
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefualtConnection"));

            });

            //builder.Services.AddScoped<GenericRepository<Member>, GenericRepository<Member>>();
            //builder.Services.AddScoped<GenericRepository<Trainer>, GenericRepository<Trainer>>();
            //builder.Services.AddScoped<GenericRepository<Plan>, GenericRepository<Plan>>();
            //builder.Services.AddScoped(typeof(GenericRepository<>), typeof(GenericRepository<>));
            //builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(IGenericRepository<>));
            //builder.Services.AddScoped<IPlanRepository, PlanRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); 
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddScoped<IMemberShipRepository, MemberShipRepository>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddAutoMapper(X => X.AddProfile(new MappingProfile()));
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.AddScoped<IMemberShipService, MemberShipService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<GymDbContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });
            builder.Services.AddScoped<IAccountService , AccountService>();


            //builder.Services.AddIdentityCore<ApplicationUser>
            //    ().AddEntityFrameworkStores<GymDbContext>();

            var app = builder.Build();

            #region Data Seed
            using var Scoped = app.Services.CreateScope();
            var dbContext = Scoped.ServiceProvider.GetRequiredService<GymDbContext>();
            var roleManager = Scoped.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = Scoped.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var PendingMigrations = dbContext.Database.GetPendingMigrations();

            if (PendingMigrations?.Any() ?? false)
            {
                dbContext.Database.Migrate();
            }


            GymDbContextDataSeeding.SeedData(dbContext);
            IdentityDbContextSeeding.SeedData(roleManager, userManager);

            #endregion


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id:int?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
