using ChatApp_RealTime.Data;
using ChatApp_RealTime.Hubs;
using ChatApp_RealTime.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ChatApp_RealTime
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            // Database
            // Lấy từ biến môi trường "DATABASE_URL"
            string? connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

            // Nếu ở máy local chưa có biến môi trường thì lấy từ appsettings.json
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            }

            builder.Services.AddDbContext<ChatDbContext>(options =>
            {
                // Lấy thông tin từ các biến môi trường Railway cung cấp
                var host = Environment.GetEnvironmentVariable("MYSQLHOST");
                var port = Environment.GetEnvironmentVariable("MYSQLPORT");
                var user = Environment.GetEnvironmentVariable("MYSQLUSER");
                var password = Environment.GetEnvironmentVariable("MYSQLPASSWORD");
                var database = Environment.GetEnvironmentVariable("");

                // Ghép thành chuỗi kết nối (Connection String)
                string connectionString = $"Server={host};Port={port};Database={database};Uid={user};Pwd={password};";

                // Debug log (xóa dòng này sau khi chạy thành công)
                Console.WriteLine("DEBUG Connection String: " + connectionString);

                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            // SignalR
            builder.Services.AddSignalR();

            // Authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login";
                    options.LogoutPath = "/Logout";
                    options.AccessDeniedPath = "/Login";
                    options.ExpireTimeSpan = TimeSpan.FromDays(7);
                    options.SlidingExpiration = true;
                });

            // Application services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IChatService, ChatService>();

            var app = builder.Build();

            // Auto-migrate the database on startup
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
                db.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages().WithStaticAssets();
            app.MapHub<ChatHub>("/chatHub");

            app.Run();
        }
    }
}
