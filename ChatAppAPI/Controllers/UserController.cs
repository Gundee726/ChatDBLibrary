using ChatDBLibrary.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserWithLastActive>>> GetUsers([FromQuery] int currentUserId)
        {
            var users = await _context.Users
                .Join(_context.LastActiveDates,
                      u => u.User_id,
                      l => l.User_id,
                      (u, l) => new UserWithLastActive
                      {
                          User_id = u.User_id,
                          Username = u.Username,
                          Avatar = u.Avatar,
                          LastActiveDate = l.LastActiveDate 
                      })
                .Where(u => u.User_id != currentUserId)
                .Where(u => _context.chathistory.Any(m =>
                    (m.Sender_id == currentUserId && m.Receiver_id == u.User_id) ||
                    (m.Receiver_id == currentUserId && m.Sender_id == u.User_id)))
                .ToListAsync();

            if (!users.Any())
            {
                return NotFound("No users found.");
            }

            return Ok(users);
        }
    }

    public class UserWithLastActive
    {
        public int User_id { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public DateTime LastActiveDate { get; set; }
    }
}
