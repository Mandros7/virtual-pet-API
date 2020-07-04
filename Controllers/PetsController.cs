using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace mediatonic_pets.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetController : ControllerBase
    {
        private readonly ILogger<PetController> _logger;

        public PetController(ILogger<PetController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Pet> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Pet
            {
                lastUpdate = DateTime.Now.AddDays(-1*index),
                hungriness = rng.Next(-100, 100),
                happiness = rng.Next(-100, 100),
            })
            .ToArray();
        }
    }
}
