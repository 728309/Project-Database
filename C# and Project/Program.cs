using C__and_Project.Repositories;

namespace C__and_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllersWithViews();

            // Register repositories (Dependency Injection)
            builder.Services.AddSingleton<IUsersRepository, UsersRepository>(); // Users Repository
            builder.Services.AddSingleton<IRoomRepository, RoomRepository>(); // Rooms Repository
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<ILecturerRepository, LecturerRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IDrinkRepository, DrinkRepository>();
            builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
            builder.Services.AddScoped<ISupervisorRepository, SupervisorRepository>();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts(); // Enables HTTP Strict Transport Security (HSTS) for security
            }

            app.UseHttpsRedirection(); // Enforce HTTPS
            app.UseStaticFiles(); // Enable serving static files (CSS, JS, images)

            app.UseRouting(); // Enable routing

            app.UseAuthorization(); // Enable authorization middleware

            // Configure default route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}