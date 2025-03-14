using AutoMapper;
using MiBlog.DTOs;
using MiBlog.Entities;

namespace MiBlog.Mapper
{
    public class MapperClass:Profile
    {
        public MapperClass()
        {
            #region Usuario
            CreateMap<UsuarioDTO,SesionDTO>().ReverseMap();
            CreateMap<Usuario,UsuarioDTO>().ReverseMap();
            CreateMap<Usuario, SesionDTO>().ReverseMap();
            #endregion

        }
    }
}
