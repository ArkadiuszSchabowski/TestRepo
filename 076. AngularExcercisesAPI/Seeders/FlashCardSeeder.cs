using _076.AngularExcercisesAPI.Database;
using _076.AngularExcercisesAPI.Database.Entities;

namespace _076.AngularExcercisesAPI.Server.Seeders
{
    public interface IFlashCardSeeder
    {
        void AddStartupFlashCards();
    }
    public class FlashCardSeeder : IFlashCardSeeder
    {
        private readonly MyDbContext _context;

        public FlashCardSeeder(MyDbContext context)
        {
            _context = context;
        }
        public void AddStartupFlashCards()
        {
            if (!_context.FlashCards.Any())
            {
            var flashcards = new List<FlashCard> {
                new FlashCard
                {
                    PolishWord = "ŚWINKA MORSKA",
                    EnglishWord = "GUINEA PIG"
                }, new FlashCard
                {
                    PolishWord = "SAMOLOT",
                    EnglishWord = "AIRPLANE"
                },
                new FlashCard {
                PolishWord = "KSIĄŻKA",
                EnglishWord = "BOOK"
                } 
            };
            _context.FlashCards.AddRange(flashcards);
            _context.SaveChanges();
            }
        }
    }
}
