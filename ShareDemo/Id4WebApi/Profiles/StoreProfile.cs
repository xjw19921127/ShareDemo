using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Id4WebApi
{
    public class StoreProfile : Profile
    {
        public StoreProfile()
        {
            CreateMap<BasStore, StoreDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Pid))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Tilte));
        }
    }
}
