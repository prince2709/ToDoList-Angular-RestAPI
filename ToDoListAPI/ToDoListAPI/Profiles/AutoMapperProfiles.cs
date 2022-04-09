using AutoMapper;
using ToDoList.Contracts.DataModels;
using ToDoListAPI.DomainModels;

namespace ToDoListAPI.Profiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserDomainModel>().ReverseMap();
            CreateMap<ToDoTask, ToDoTaskDomainModel>()
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate.ToString("MM/dd/yyyy")))
                .ForMember(dest => dest.CompletedDate, opt => opt.MapFrom(src => src.CompletedDate.Value.ToString("MM/dd/yyyy")));
            CreateMap<ToDoTaskDomainModel, ToDoTask>()
                 .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate))
                .ForMember(dest => dest.CompletedDate, opt => opt.MapFrom(src => (string.IsNullOrEmpty(src.CompletedDate) ? null : src.CompletedDate)));


        }
    }
}
