using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Your_Finance_Escort.Services.AuthAPI.Models.Dto;
using Your_Finance_Escort.Services.AuthAPI.Service.IService;

namespace Your_Finance_Escort.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ResponseDto _response = new ResponseDto();
        private readonly IUserService _userService;
        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var errorMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
            return Ok(_response);
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var result = await _authService.Login(model);
            if (result.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Invalid credentials.";
                return BadRequest(_response);
            }
            _response.Result = result;
            return Ok(_response);
        }
        [AllowAnonymous]
        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)
        {
            var success = await _authService.AssignRole(model.Email, model.Role);
            if (!success)
            {
                _response.IsSuccess = false;
                _response.Message = "Role assignment failed.";
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _authService.GetAllUsersAsync();
                _response.Result = users;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = "Error retrieving users: " + ex.Message;
                return StatusCode(500, _response);
            }
        }


        [HttpGet("user/{userId}")]

        public async Task<IActionResult> GetUserById(string userId)
        {
            try
            {
                var userDto = await _authService.GetUserByIdAsync(userId);

                if (userDto == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "User not found.";
                    return NotFound(_response);
                }

                _response.Result = userDto;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = "Error retrieving user: " + ex.Message;
                return StatusCode(500, _response);
            }
        }

        [HttpPut("user/{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UserDto updatedUser)
        {
            try
            {
                // Check if the user exists
                var existingUser = await _authService.GetUserByIdAsync(userId);
                if (existingUser == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "User not found.";
                    return NotFound(_response);
                }

                // Update the user details
                var success = await _authService.UpdateUserAsync(userId, updatedUser);
                if (!success)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Failed to update user.";
                    return BadRequest(_response);
                }

                _response.IsSuccess = true;
                _response.Message = "User updated successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = "Error updating user: " + ex.Message;
                return StatusCode(500, _response);
            }
        }
    }
}
