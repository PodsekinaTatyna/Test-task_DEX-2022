using Microsoft.AspNetCore.Mvc;
using Models;
using ModelsDb;
using Services;
using Services.Filters;


namespace BulletinBoardAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private UserService _service { get; set; }

        private BulletinBoardContext _context { get; set; }

        public UserController()
        {
            _service = new UserService();
            _context = new BulletinBoardContext();
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetFilteredUsers([FromQuery] UserFilter filter)
        {
            var filteredUsers = await _service.GetFilteredUsers(filter);

            if (filteredUsers.Count != 0)
            {
                return filteredUsers;
            }

            return new NotFoundResult();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromQuery] User user)
        {
            if (_context.Users.FirstOrDefault(u => u.Id == user.Id) != null)
                return new BadRequestObjectResult(new KeyNotFoundException("Такой клиент есть в базе данных"));

            await _service.AddUserAsync(user);
            return Ok();
                   
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromQuery] User user)
        {
            if (_context.Users.FirstOrDefault(u => u.Id == user.Id) == null)
                return new BadRequestObjectResult(new KeyNotFoundException("Такого клиента нет в базе данных"));

            await _service.DeleteUserAsync(user);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromQuery] User user)
        {
            if (_context.Users.FirstOrDefault(u => u.Id == user.Id) == null)
                return new BadRequestObjectResult(new KeyNotFoundException("Такого клиента нет в базе данных"));

            await _service.UpdateUserAsync(user);
            return Ok();
        }

       
    }
}
