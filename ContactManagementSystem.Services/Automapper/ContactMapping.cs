using AutoMapper;
using ContactManagementSystem.DTOs;
using ContactManagementSystem.Repositories.Models;

namespace ContactManagementSystem.Services.Automapper
{
    public class ContactMapping : Profile
    {
        public ContactMapping()
        {
            CreateMap<ContactRequestDTO, ContactManagement>();

            CreateMap<UpdateContactDTO, ContactManagement>().ReverseMap();

            CreateMap<ContactManagement, ContactResponseDTO>();
        }
    }
}