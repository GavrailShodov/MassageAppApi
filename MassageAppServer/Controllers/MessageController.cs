using Microsoft.AspNetCore.Mvc;

namespace MassageAppServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class MessageController : Controller
    {
        private static readonly List<Message> Messages = new List<Message>();

        [HttpGet]
        public ActionResult<IEnumerable<Message>> GetMessages()
        {
            return Ok(Messages);
        }

        [HttpPost]
        public ActionResult SendMessage([FromBody] Message message)
        {
            if (string.IsNullOrEmpty(message.Owner) || string.IsNullOrEmpty(message.Text))
            {
                return BadRequest("Username and Content are required.");
            }

            Messages.Add(message);
            return Ok(new { status = "Message sent" });
        }
    }
}
