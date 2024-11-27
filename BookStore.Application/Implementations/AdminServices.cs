using BookStore.Application.Bases;
using BookStore.Application.Features;
using BookStore.Domain.Classes;
using BookStore.Domain.DTOs.AdminDTOs;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Application.Implementations
{
    public class AdminServices : ResponseHandler, IAdminServices
    {
        public UserManager<IdentityUser> _userManager;
        public AdminServices(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;

        }
        #region Register User
        public async Task<Response<string>> RegisterAdmin(RegisterAdminDTO user)
        {
            // Validate email
            if (await _userManager.FindByEmailAsync(user.email) != null)
            {
                return NotFound<string>("Email is already registered.");
            }

            // Validate username
            if (await _userManager.FindByNameAsync(user.username) != null)
            {
                return BadRequest<string>("Username is already taken.");
            }

            // Map DTO to Employee entity
            Admin newEmployee = new Admin()
            {
                Email = user.email,
                UserName = user.username,
                PhoneNumber = user.phonenumber,
            };

            // Create the user
            IdentityResult creationResult = await _userManager.CreateAsync(newEmployee, user.password);
            if (!creationResult.Succeeded)
            {
                var errors = string.Join(", ", creationResult.Errors.Select(e => e.Description));
                return BadRequest<string>($"Admin creation failed: {errors}");
            }

            // Assign role to the user
            var roleResult = await _userManager.AddToRoleAsync(newEmployee, "admin");
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                return BadRequest<string>($"Failed to assign role: {errors}");
            }

            return Created<string>("Admin registration successful.");
        }



        #endregion
    }
}
