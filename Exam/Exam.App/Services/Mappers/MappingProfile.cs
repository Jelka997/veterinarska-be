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
                opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.Username,
                opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<Examination, ExaminationDto>().ReverseMap();
            CreateMap<Examination, ExaminationPreviewForVetDto>()
                .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.Pet.Name))
                .ForMember(dest => dest.AnimalSpecie,
                opt => opt.MapFrom(src => src.Pet.AnimalSpecie.Name))
                .ForMember(dest => dest.Age,
                opt => opt.MapFrom(src => DateTime.Now.Year - src.Pet.DateOfBirth.Year));

            CreateMap<Patient, PatientPreviewForVetDto>()
                .ForMember(dest => dest.AnimalSpecie,
                opt => opt.MapFrom(src => src.AnimalSpecie.Name));
            CreateMap<Vet, VetByIdDto>()
                 .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => src.User.Name + " " + src.User.Surname))
                .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.User.Email));
            CreateMap<ExamReport, ExamReportDto>().ReverseMap();
        }
    }
}
