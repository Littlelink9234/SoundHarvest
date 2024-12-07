using Auth.Core.Models;
using Auth.Core.Repositories;
using Auth.Core.Security.Hashing;
using Auth.Core.Services;
using Auth.Core.Services.Communication;
using Microsoft.AspNetCore.Identity;

namespace Auth.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<CreateUserResponse> CreateUserAsync(User user, params ApplicationRole[] userRoles)
        {
            var existingUser = await _userRepository.FindByEmailAsync(user.Email);

            if (existingUser == null)
            {
                return new CreateUserResponse(false, "Email already in use.", null);
            }



            //user.Password = _passwordHasher.HashPassword(user.Password);
            await _userRepository.AddAsync(user, userRoles);
            await _unitOfWork.CompleteAsync();

            return new CreateUserResponse(true, null, user);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userRepository.FindByEmailAsync(email);
        }

        public bool ValidatePassword(User user, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
