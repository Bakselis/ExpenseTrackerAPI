using System.Threading.Tasks;
using ExpenseTracker.AuthApi.Domain;
using ExpenseTracker.Domain;

namespace ExpenseTracker.AuthApi.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password);
        
        Task<AuthenticationResult> LoginAsync(string email, string password);
        
        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
    }
}