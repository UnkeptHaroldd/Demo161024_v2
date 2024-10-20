using APIDemo161024.DTOS;
using Microsoft.AspNetCore.Identity;

namespace APIDemo161024.Repositories
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUp(SignUpModel model);

        public Task<string> SignIn(SignInModel model);
    }
}
