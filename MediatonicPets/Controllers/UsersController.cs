using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediatonicPets.Models;
using MediatonicPets.Services;

namespace MediatonicPets.Controllers
{    
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        
        private readonly ILogger<UsersController> _logger;

        private readonly UserService _userService;

        public UsersController(ILogger<UsersController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// Retrieves a list of all existing Users
        /// </summary>
        [Produces("application/json")]
        [HttpGet]
        public ActionResult<List<User>> Get() =>
            _userService.Get();

        /// <summary>
        /// Retrieves information about a specific User.
        /// </summary>
        /// <param name="id"></param> 
        [Produces("application/json")]
        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public ActionResult<User> Get(string id)
        {
            var user = _userService.Get(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        /// <summary>
        /// Creates a new User.
        /// </summary>
        [Produces("application/json")]
        [HttpPost]
        public ActionResult<User> Create()
        {
            User newUser = _userService.Create(new User());
            if (newUser == null)
            {
                return BadRequest();
            }
            return CreatedAtRoute("GetUser", new { id = newUser.Id.ToString() }, newUser);
        }

        /// <summary>
        /// Deletes a specific User.
        /// </summary>
        /// <param name="id"></param>    
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            _userService.Remove(user.Id);

            return NoContent();
        }
    }
}