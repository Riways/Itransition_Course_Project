using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using totten_romatoes.Server.Services;
using totten_romatoes.Shared.Models;

namespace totten_romatoes.Server.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("chunk")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IEnumerable<ApplicationUser>> GetChunkOfUsers([FromQuery] int chunkNumber)
        {

            return await _userService.GetChunkOfUsers(chunkNumber);
        }

        [HttpGet("amount")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<int> GetAmountOfUsers()
        {
            return await _userService.GetAmountOfUsers();
        }

        [HttpDelete]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteMultipleReviews([FromQuery] List<string> ids)
        {
            await _userService.DeleteMuiltipleUsers(ids);
            return NoContent();
        }
    }
}
