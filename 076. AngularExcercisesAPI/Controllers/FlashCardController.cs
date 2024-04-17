using _076.AngularExcercisesAPI.Database.Entities;
using _076.AngularExcercisesAPI.Models;
using _076.AngularExcercisesAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace _076._AngularExcercisesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlashCardController : ControllerBase
    {
        private readonly IFlashCardService _service;

        public FlashCardController(IFlashCardService service)
        {
            _service = service;
        }
        [HttpGet]
        public ActionResult<List<FlashCard>> GetFlashCards()
        {
            var flashcards = _service.GetFlashCards();
            return Ok(flashcards);
        }
        [HttpGet("{id}")]
        public ActionResult<FlashCard> GetFlashCard([FromRoute] int id)
        {
            FlashCard flashCard = _service.GetFlashCard(id);

            return Ok(flashCard);
        }
        [HttpGet("polish")]
        public ActionResult<FlashCard> GetPolishFlashCard([FromQuery] string word)
        {
            FlashCard flashcard = _service.GetPolishFlashCard(word);
            return Ok(flashcard);
        }
        [HttpGet("english")]
        public ActionResult<FlashCard> GetEnglishFlashCard([FromQuery] string word)
        {
            FlashCard flashcard = _service.GetEnglishFlashCard(word);
            return Ok(flashcard);
        }
        [HttpPost]
        public ActionResult AddWord([FromBody] FlashCardDto dto)
        {
            _service.AddWord(dto);
            return Ok("Słowo dodane pomyślnie");
        }
        [HttpPut("{id}")]
        public ActionResult UpdateWord([FromRoute] int id, [FromBody] FlashCardDto dto)
        {
            _service.UpdateWord(id, dto);
            return Ok("Słowo zaaktualizowane pomyślnie");
        }
        [HttpDelete("{id}")]
        public ActionResult RemoveWord([FromRoute] int id)
        {
            _service.RemoveWord(id);
            return NoContent();
        }
    }
}
