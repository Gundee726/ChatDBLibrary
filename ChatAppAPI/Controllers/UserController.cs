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
        public async Task<ActionResult<IEnumerable<User>>> GetUsers([FromQuery] int currentUserId)
        {
            var users = await _context.Users
            .Where(u => u.User_id != currentUserId) // Exclude current user
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
}
