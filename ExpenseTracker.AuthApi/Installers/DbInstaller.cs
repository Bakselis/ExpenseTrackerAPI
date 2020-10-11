using ExpenseTracker.AuthApi.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.AuthApi.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
//            var connection = @"Server=db-server;Database=ExpenseTrackerUser;User=sa;Password=Tomas1234;";
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));
            
//            services.AddDbContext<DataContext>(options =>
//                options.UseSqlServer(connection));
            
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataContext>();

//            services.AddScoped<IExpenseService, ExpenseService>();
        }
    }
}