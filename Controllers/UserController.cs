using guesser_api.Dto;
using guesser_api.Interfaces;
using guesser_api.Repositories;
using Microsoft.AspNetCore.Mvc;
using server.Models;

namespace guesser_api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserInterface _userRepository;
        public UserController(IUserInterface userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            var forwardedIp = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            var ipAddress = forwardedIp ?? HttpContext.Connection.RemoteIpAddress?.ToString();

            user.Ip = ipAddress;

            User newUser = await _userRepository.Create(user);

            return Ok(newUser);
        }
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrent()
        {
            var forwardedIp = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            var ipAddress = forwardedIp ?? HttpContext.Connection.RemoteIpAddress?.ToString();

            User user = await _userRepository.GetCurrent(ipAddress);

            return Ok(user);
        }
        [HttpPost("update_score")]
        public async Task<IActionResult> UpdateScore([FromBody] UpdateScoreDto updateScoreDto)
        {
            User user = await _userRepository.UpdateScore(updateScoreDto);
            return Ok(user);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUser()
        {
            return Ok(await _userRepository.GetUsers());
        }
    }
}