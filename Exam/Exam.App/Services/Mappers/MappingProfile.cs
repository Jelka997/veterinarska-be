using AutoMapper;
using Exam.App.Domain;
using Exam.App.Services.Dtos;

namespace Exam.App.Services.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, ProfileDto>();
            CreateMap<CreatePatientDto, Patient>().ReverseMap();
            CreateMap<UpdatePatientDto, Patient>().ReverseMap();
            CreateMap<Patient, PatientPreviewDto>()
                .ForMember(dest => dest.OwnerFullName,
                opt => opt.MapFrom(src => src.Owner.User.Name + " " + src.Owner.User.Surname))
                .ForMember(dest => dest.VetFullName,
                opt => opt.MapFrom(src => src.Vet.User.Name + " " + src.Vet.User.Surname))
                .ForMember(dest => dest.AnimalSpecie,
                opt => opt.MapFrom(src => src.AnimalSpecie.Name));
            CreateMap<Vet, VetPreviewDto>()
                .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => src.User.Name + " " + src.User.Surname))
                .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.User.Email));
        }
    }
}
