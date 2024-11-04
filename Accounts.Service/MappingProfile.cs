using AutoMapper;
using Core.Entities;
using Util.DTO;

namespace Accounts.Service {
    public class MappingProfile : Profile {

        public MappingProfile() {

            CreateMap<CuentaDTO, Cuenta>().ReverseMap();
            CreateMap<MovimientoDTO, Movimiento>().ReverseMap();

        }
    }
}
