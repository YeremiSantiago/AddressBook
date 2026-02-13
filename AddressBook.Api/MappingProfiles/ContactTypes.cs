using AutoMapper;
using AddressBook.Api.DTOs;
using AddressBook.Api.Models;
using AddressBook.Api.DTOs.ContactTypeDTO;

namespace AddressBook.Api.MappingProfiles
{
    public class ContactTypes : Profile
    {
        public ContactTypes()
        {
            // Entidad a DTO
            CreateMap<ContactType, ContactTypeReadDTO>();

            // DTO a Entidad
            CreateMap<ContactTypeRequestDTO, ContactType>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Contacts, opt => opt.Ignore());
        }
    }
}