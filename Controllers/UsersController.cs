using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediatonicPets.Models;
using MediatonicPets.Services;

namespace MediatonicPets.Controllers
{
    public class UsersController : ControllerBase
    {
        
        private readonly ILogger<UsersController> _logger;

        private readonly UserService _userService;

        public UsersController(ILogger<UsersController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }


        [HttpGet]
        public ActionResult<List<User>> Get() =>
            _userService.Get();

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

        [HttpPost("{id:length(24)}")]
        public ActionResult<User> Create(User user)
        {
            User newUser = _userService.Create(user);
            if (newUser == null)
            {
                return BadRequest();
            }
            return CreatedAtRoute("GetPet", new { id = newUser.Id.ToString() }, newUser);
        }

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