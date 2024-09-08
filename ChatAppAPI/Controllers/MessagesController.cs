using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using ChatDBLibrary.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ChatAppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages([FromQuery] int sender_id, [FromQuery] int receiver_id)
        {
            var messages = await _context.chathistory
                .Where(m => (m.Sender_id == sender_id && m.Receiver_id == receiver_id) ||
                            (m.Sender_id == receiver_id && m.Receiver_id == sender_id))
                .ToListAsync();

            if (!messages.Any())
            {
                return NotFound("No messages found.");
            }

            return Ok(messages);
        }


        [HttpPost]
        public async Task<ActionResult> PostMessage([FromBody] Messages message)
        {
            if (message == null)
            {
                return BadRequest("Message cannot be null.");
            }

            _context.chathistory.Add(message);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
