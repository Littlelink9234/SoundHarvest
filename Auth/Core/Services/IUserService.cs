using Auth.Core.Models;
using Auth.Core.Services.Communication;

namespace Auth.Core.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateUserAsync(User user, params ApplicationRole[] userRoles);
        Task<User> FindByEmailAsync(string email);
    }
}
