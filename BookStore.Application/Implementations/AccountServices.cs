using BookStore.Application.Bases;
using BookStore.Application.Features;
using BookStore.Domain.DTOs.AccountDTOs;
using BookStore.Domain.DTOs.CustomerDTOs;
using BookStore.Domain.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStore.Application.Implementations
{
    public class AccountServices : ResponseHandler, IAccountServices
    {
        SignInManager<IdentityUser> _signin;
        UserManager<IdentityUser> _manager;
        private readonly JwtSettings _jwtSettings;


        public AccountServices(SignInManager<IdentityUser> signin, UserManager<IdentityUser> manager, JwtSettings jwtSettings)
        {
            _signin = signin;
            _manager = manager;
            _jwtSettings = jwtSettings;

        }

        #region Change Password
        public async Task<Response<string>> ChangePassword(ChangePasswordDTO pass)
        {
            var user = await _manager.FindByIdAsync(pass.id);
            if (user == null)
                return BadRequest<string>("User not found");

            var result = await _manager.ChangePasswordAsync(user, pass.oldpassword, pass.newpassword);
            if (result.Succeeded)
                return Success<string>("Password changed successfully");

            return BadRequest<string>("Failed to change password");
        }
        #endregion

        #region Login
        public async Task<Response<JwtAuthResult>> Login(LoginDTO logindata)
        {
            //Check if user is exist or not
            var user = await _manager.FindByNameAsync(logindata.username);
            //Return The UserName Not Found
            if (user == null) return BadRequest<JwtAuthResult>("UserNameIsNotExist");
            //try To Sign in 
            var signInResult = await _signin.CheckPasswordSignInAsync(user, logindata.password, false);
            //if Failed Return Passord is wrong
            if (!signInResult.Succeeded) return BadRequest<JwtAuthResult>("PasswordNotCorrect");

            //Generate Token
            var (jwtToken, accessToken) = await GenerateJwtToken(user);
            var response = new JwtAuthResult();

            response.AccessToken = accessToken;
            //return Token 
            return Success(response);



        }

        #region Generate JwtToken
        private async Task<(JwtSecurityToken, string)> GenerateJwtToken(IdentityUser user)
        {
            var claims = await GetClaims(user);
            var jwtToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwtSettings.AccessTokenExpireDate),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)), SecurityAlgorithms.HmacSha256Signature));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return (jwtToken, accessToken);
        }
        #endregion

        #region Get All Claims
        public async Task<List<Claim>> GetClaims(IdentityUser user)
        {
            var roles = await _manager.GetRolesAsync(user);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(nameof(UserClaimModel.PhoneNumber), user.PhoneNumber),
                new Claim(nameof(UserClaimModel.Id), user.Id.ToString())
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var userClaims = await _manager.GetClaimsAsync(user);
            claims.AddRange(userClaims);
            return claims;
        }

        #endregion

        #endregion

        #region Logout
        public async Task<Response<string>> logout()
        {
            if (!_signin.Context.User.Identity.IsAuthenticated)
                return BadRequest<string>("No user is currently logged in");

            await _signin.SignOutAsync();
            return Success<string>("Logout successful");
        }
        #endregion


    }
}

