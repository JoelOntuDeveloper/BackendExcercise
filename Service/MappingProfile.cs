using AutoMapper;
using Core.Entities;
using Util.DTO;

namespace Service {
    public class MappingProfile:Profile 
    {
        public MappingProfile()
        {

            CreateMap<ClienteDTO, Persona>()
                .ForMember(persona => persona.Identificacion, opt => opt.MapFrom(dto => dto.Identificacion))
                .ForMember(persona => persona.Nombre, opt => opt.MapFrom(dto => dto.Nombre))
                .ForMember(persona => persona.Genero, opt => opt.MapFrom(dto => dto.Genero))
                .ForMember(persona => persona.Edad, opt => opt.MapFrom(dto => dto.Edad))
                .ForMember(persona => persona.Direccion, opt => opt.MapFrom(dto => dto.Direccion))
                .ForMember(persona => persona.Telefono, opt => opt.MapFrom(dto => dto.Telefono))
                .ReverseMap();

            CreateMap<ClienteDTO, Cliente>()
                .ForMember(cliente => cliente.ClienteId, opt => opt.MapFrom(dto => dto.ClienteId))
                .ForMember(cliente => cliente.Contrasenia, opt => opt.MapFrom(dto => dto.Contrasenia))
                .ForMember(cliente => cliente.Estado, opt => opt.MapFrom(dto => dto.Estado))
                .ReverseMap();

            CreateMap<CuentaDTO, Cuenta>().ReverseMap();

            CreateMap<MovimientoDTO, Movimiento>() .ReverseMap();
        }
    }
}
