using AutoMapper;
using EventRegistrationSystem.Areas.Identity.Models;
using EventRegistrationSystem.Models.Events;
using EventRegistrationSystem.ViewModels.Events;
using EventRegistrationSystem.ViewModels.Users;

namespace EventRegistrationSystem.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Event, EventViewModel>()
                .ForMember(dest => dest.EventImageFileName, opt => opt.MapFrom((src, dest) => src.EventImages.FirstOrDefault()?.FileName ?? "default_logo.png"));

            CreateMap<Event, EventDetailsViewModel>()
                .ForMember(dest => dest.EventImageFileNames, opt => opt.MapFrom(src => src.EventImages.Select(ei => ei.FileName)))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.EventCategory.Name))
                .ForMember(dest => dest.ApplicantNum, opt => opt.MapFrom(src => src.EventEnrollments.Count))
                .ForMember(dest => dest.Organization, opt => opt.MapFrom(src => src.Creator));

            CreateMap<Event, EventCreateFormViewModel>()
                .ReverseMap();

            CreateMap<Event, EventEditFormViewModel>()
                .ReverseMap();

            CreateMap<OrganizationUser, OrganizationViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.OrganizationName));

            CreateMap<OrganizationUser, OrganizationDetailsViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.OrganizationName));

            AllowNullCollections = true;
        }
    }
}
