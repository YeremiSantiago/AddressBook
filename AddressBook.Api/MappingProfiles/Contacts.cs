using AutoMapper;

namespace AddressBook.Api.MappingProfiles
{
    public class Contacts: Profile
    {
        public Contacts()
        {
            //Entidad a DTO
            CreateMap<Models.Contact, DTOs.ContactReadDTO>()
                .ForMember(dest => dest.ContactTypeName, src => src.MapFrom(o => o.ContactType.Name)); 

            // --> / <--
            //DTO a Entidad
            CreateMap<DTOs.ContactRequestDTO, Models.Contact>()
                .ForMember(dest => dest.Id, src => src.Ignore());
        }
    }
}
