using _076.AngularExcercisesAPI.Database.Entities;
using _076.AngularExcercisesAPI.Models;
using AutoMapper;

namespace _076.AngularExcercisesAPI
{
    public class FlashCardMappingProfile : Profile
    {
        public FlashCardMappingProfile()
        {
            CreateMap<FlashCard, FlashCardDto>();
            CreateMap<FlashCardDto, FlashCard>();
        }
    }
}
