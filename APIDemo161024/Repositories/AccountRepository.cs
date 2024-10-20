using APIDemo161024.DTOS;
using APIDemo161024.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIDemo161024.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IConfiguration configuration;

        public AccountRepository(UserManager<User> userManager,
            SignInManager<User> signInManager, IConfiguration configuration) 
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        public async Task<string> SignIn(SignInModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (!result.Succeeded)
            {
                return string.Empty;
            }

            var authClaims = new List<Claim> 
            { 
              new Claim(ClaimTypes.Email, model.Email),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid()
              .ToString()),
            };

            var authenKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));

            var token = new JwtSecurityToken
            (
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new Microsoft.IdentityModel.Tokens.
                SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256Signature)

            ); 

            return new JwtSecurityTokenHandler().WriteToken(token); 
        }

        public async Task<IdentityResult> SignUp(SignUpModel model)
        {
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email
            };

            return await userManager.CreateAsync(user, model.Password);
        }
    }
}
