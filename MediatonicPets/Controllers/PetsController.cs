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

        /// <summary>
        /// Retrieves information about all existing Pets
        /// </summary> 
        [Produces("application/json")]
        [HttpGet]
        public ActionResult<List<Pet>> Get() =>
            _petService.Get();


        /// <summary>
        /// Retrieves information about a specific Pet 
        /// </summary>
        /// <param name="id"></param>
        [Produces("application/json")]
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

        /// <summary>
        /// Creates a specific Pet with a Type and an Owner ID.
        /// </summary>
        /// <param name="ownerID"></param> 
        /// <param name="type"></param> 
        [Produces("application/json")]
        [HttpPost("{ownerID:length(24)}/{type}")]
        public ActionResult<Pet> Create(string ownerID, string type)
        {
            Pet newPet = _petService.Create(ownerID, type);
            if (newPet == null)
            {
                return BadRequest();
            }
            return CreatedAtRoute("GetPet", new { id = newPet.Id.ToString() }, newPet);
        }

        /// <summary>
        /// Updates a specific Pet.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newPet"></param> 
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

        /// <summary>
        /// Deletes a specific Pet.
        /// </summary>
        /// <param name="id"></param> 
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

        /// <summary>
        /// Strokes a specific Pet - Increases Happiness.
        /// </summary>
        /// <param name="id"></param> 
        [Produces("application/json")]
        [HttpPut("{id:length(24)}/stroke")]
        public IActionResult Stroke(string id)
        {
            var pet = _petService.Get(id);

            if (pet == null)
            {
                return NotFound();
            }
            
            Pet resultPet = _petService.Stroke(id);
            return Ok(resultPet);
        }

        /// <summary>
        /// Feeds a specific Pet - Decreases Hungriness.
        /// </summary>
        /// <param name="id"></param> 
        [Produces("application/json")]
        [HttpPut("{id:length(24)}/feed")]
        public IActionResult Feed(string id)
        {
            var pet = _petService.Get(id);

            if (pet == null)
            {
                return NotFound();
            }
            
            Pet resultPet = _petService.Feed(id);

            return Ok(resultPet);
        }
        

    }
}
