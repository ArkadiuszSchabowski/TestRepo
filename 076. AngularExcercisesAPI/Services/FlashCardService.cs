using _076.AngularExcercisesAPI.Database;
using _076.AngularExcercisesAPI.Database.Entities;
using _076.AngularExcercisesAPI.Exceptions;
using _076.AngularExcercisesAPI.Models;
using AutoMapper;

namespace _076.AngularExcercisesAPI.Services
{
    public interface IFlashCardService
    {
        List<FlashCard> GetFlashCards();
        FlashCard GetFlashCard(int id);
        FlashCard GetPolishFlashCard(string word);
        FlashCard GetEnglishFlashCard(string word);
        void AddWord(FlashCardDto dto);
        void UpdateWord(int id, FlashCardDto dto);
        void RemoveWord(int id);
    }
    public class FlashCardService : IFlashCardService
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public FlashCardService(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public List<FlashCard> GetFlashCards()
        {
            return _context.FlashCards.ToList();
        }
        public FlashCard GetFlashCard(int id)
        {
            if(id < 0)
            {
                throw new BadRequestException("Wartość id musi być większa od zero");
            }
            var flashCard = _context.FlashCards.Find(id);

            if(flashCard == null)
            {
                throw new NotFoundException("Nie znaleziono słowo o podanym id");
            }

            return flashCard;
        }
        public FlashCard GetPolishFlashCard(string word)
        {
            if(word.Length < 2 || word.Length > 25)
            {
                throw new BadRequestException("Szukane słowo musi mieścić się w przedziale od 2 do 25 znaków");
            }
            var flashCard = _context.FlashCards.FirstOrDefault(x => x.PolishWord == word);

            if (flashCard == null)
            {
                throw new NotFoundException("Nie znaleziono takiego polskiego słowa");
            }

            return flashCard;
        }
        public FlashCard GetEnglishFlashCard(string word)
        {
            if (word.Length < 2 || word.Length > 25)
            {
                throw new BadRequestException("Szukane słowo musi mieścić się w przedziale od 2 do 25 znaków");
            }
            var flashCard = _context.FlashCards.FirstOrDefault(x => x.EnglishWord == word);

            if (flashCard == null)
            {
                throw new NotFoundException("Nie znaleziono takiego angielskiego słowa");
            }
            return flashCard;
        }

        public void AddWord(FlashCardDto dto)
        {
            var newWordDto = new FlashCard()
            {
                PolishWord = dto.PolishWord.ToUpper(),
                EnglishWord = dto.EnglishWord.ToUpper()
            };

            var polishFlashCard = _context.FlashCards.FirstOrDefault(x => x.PolishWord == newWordDto.PolishWord);
            var englishFlashCard = _context.FlashCards.FirstOrDefault(x => x.EnglishWord == newWordDto.EnglishWord);

            if (polishFlashCard != null)
            {
                throw new ConflictException("W słowniku istnieje już takie polskie słowo");
            }
            if (englishFlashCard != null)
            {
                throw new ConflictException("W słowniku istnieje już takie angielskie słowo");
            }
            var newWord = _mapper.Map<FlashCard>(newWordDto);
            _context.FlashCards.Add(newWord);
            _context.SaveChanges();
        }

        public void UpdateWord(int id, FlashCardDto dto)
        {
            var word = _context.FlashCards.Find(id);

            if(word == null)
            {
                throw new NotFoundException("Nie znaleziono słowa o podanym id");
            }

            _mapper.Map(dto, word);
            _context.SaveChanges();
        }

        public void RemoveWord(int id)
        {
            var word = _context.FlashCards.Find(id);

            if (word == null)
            {
                throw new NotFoundException("Nie znaleziono słowa o podanym id");
            }
            _context.FlashCards.Remove(word);
            _context.SaveChanges();
        }
    }

}
