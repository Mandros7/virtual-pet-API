using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediatonicPets.Models;
using MediatonicPets.Services;

namespace MediatonicPets.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController : ControllerBase
    {
        private readonly PetService _petService;

        private readonly ILogger<PetsController> _logger;

        public PetsController(ILogger<PetsController> logger, PetService petService)
        {
            _logger = logger;
            _petService = petService;
        }

        [HttpGet]
        public ActionResult<List<Pet>> Get() =>
            _petService.Get();

        [HttpGet("{id:length(24)}", Name = "GetPet")]
        public ActionResult<Pet> Get(string id)
        {
            var pet = _petService.Get(id);
            if (pet == null)
            {
                return NotFound();
            }
            return pet;
        }

        [HttpPost("{id:length(24)}/{type}")]
        public ActionResult<Pet> Create(string id, string type)
        {
            Pet newPet = _petService.Create(id, type);
            if (newPet == null)
            {
                return BadRequest();
            }
            return CreatedAtRoute("GetPet", new { id = newPet.Id.ToString() }, newPet);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Pet newPet)
        {
            var pet = _petService.Get(id);

            if (pet == null)
            {
                return NotFound();
            }

            _petService.Update(id, newPet);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var pet = _petService.Get(id);

            if (pet == null)
            {
                return NotFound();
            }

            _petService.Remove(pet.Id);

            return NoContent();
        }


        [HttpPut("{id:length(24)}/stroke")]
        public IActionResult Stroke(string id)
        {
            var pet = _petService.Get(id);

            if (pet == null)
            {
                return NotFound();
            }
            
            _petService.Stroke(id);

            return NoContent();
        }

        [HttpPut("{id:length(24)}/feed")]
        public IActionResult Feed(string id)
        {
            var pet = _petService.Get(id);

            if (pet == null)
            {
                return NotFound();
            }
            
            _petService.Feed(id);

            return NoContent();
        }
        

    }
}
