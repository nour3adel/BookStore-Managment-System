using BookStore.Application.Bases;
using BookStore.Application.Features;
using BookStore.Domain.Classes;
using BookStore.Domain.DTOs.CustomerDTOs;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Application.Implementations
{
    public class CustomerServices : ResponseHandler, ICustomerServices
    {
        public UserManager<IdentityUser> _userManager;
        private readonly IValidator<RegisterCustomerDTO> _registerCustomerValidator;
        public CustomerServices(UserManager<IdentityUser> userManager, IValidator<RegisterCustomerDTO> registerCustomerValidator)
        {
            _userManager = userManager;
            _registerCustomerValidator = registerCustomerValidator;

        }
        #region Register Customer
        public async Task<Response<string>> RegisterUser(RegisterCustomerDTO user)
        {
            // Validate the DTO
            var validationResult = await _registerCustomerValidator.ValidateAsync(user);
            if (!validationResult.IsValid)
            {

                var error = validationResult.Errors.First().ErrorMessage;
                return BadRequest<string>(error);
            }

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
            Customer newEmployee = new Customer()
            {
                Email = user.email,
                UserName = user.username,
                fullname = user.fullname,
                address = user.address,
                PhoneNumber = user.phonenumber,
            };

            // Create the user
            IdentityResult creationResult = await _userManager.CreateAsync(newEmployee, user.password);
            if (!creationResult.Succeeded)
            {
                var errors = string.Join(", ", creationResult.Errors.Select(e => e.Description));
                return BadRequest<string>($"{errors}");


            }

            // Assign role to the user
            var roleResult = await _userManager.AddToRoleAsync(newEmployee, "customer");
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                return BadRequest<string>($"Failed to assign role: {errors}");
            }

            return Created<string>("Customer registration successful.");
        }



        #endregion

        #region Edit User

        public async Task<Response<string>> EditUser(EditCutomerDTO userDto)
        {
            // Validate input DTO
            if (userDto == null)
                return BadRequest<string>("Invalid user data.");

            // Validate Customer ID
            Customer customer = (Customer)await _userManager.FindByIdAsync(userDto.id);
            if (customer == null)
                return NotFound<string>("User not found.");

            // Update properties
            customer.Email = userDto.email?.Trim();
            customer.PhoneNumber = userDto.phonenumber?.Trim();
            customer.UserName = userDto.username?.Trim();
            customer.fullname = userDto.fullname?.Trim();
            customer.address = userDto.address?.Trim();

            // Attempt to update the user
            var result = await _userManager.UpdateAsync(customer);

            if (result.Succeeded)
            {
                return Updated<string>("Customer data updated successfully.");
            }

            // Aggregate errors and return detailed response
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return BadRequest<string>($"Failed to update customer: {errors}");
        }


        #endregion

        #region Get All Customers
        public async Task<Response<IEnumerable<SelectCustomerDTO>>> GetAllUsers()
        {
            // Retrieve all users in the "customer" role
            var users = await _userManager.GetUsersInRoleAsync("customer");
            if (users == null || !users.Any())
                return NotFound<IEnumerable<SelectCustomerDTO>>("No customers found.");

            // Map users to DTOs using LINQ
            var customerDTOs = users
                .OfType<Customer>() // Filter only `Customer` objects
                .Select(user => new SelectCustomerDTO
                {
                    id = user.Id,
                    fullname = user.fullname,
                    address = user.address,
                    username = user.UserName,
                    email = user.Email,
                    phonenumber = user.PhoneNumber
                })
                .ToList();

            // Return the mapped DTOs
            return Success<IEnumerable<SelectCustomerDTO>>(customerDTOs);
        }
        #endregion

        #region Get Customer By ID
        public async Task<Response<SelectCustomerDTO>> GetCustomerByID(string id)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest<SelectCustomerDTO>("Customer ID cannot be null or empty.");

            // Retrieve customer by ID
            var user = await _userManager.FindByIdAsync(id) as Customer;
            if (user == null)
                return NotFound<SelectCustomerDTO>($"No customer found with ID = {id}");

            // Map user to DTO
            var customerDto = new SelectCustomerDTO
            {
                id = user.Id,
                fullname = user.fullname,
                address = user.address,
                username = user.UserName,
                email = user.Email,
                phonenumber = user.PhoneNumber
            };

            // Return the mapped DTO
            return Success<SelectCustomerDTO>(customerDto);
        }


        #endregion

    }
}
