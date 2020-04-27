using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Id4WebApi
{
    public class ServiceOrderProfile : Profile
    {
        public ServiceOrderProfile()
        {
            CreateMap<MinServiceOrder, ServiceOrderDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Pid));

            CreateMap<MinInvoiceTitle, InvoiceTitleDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Pid))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name));
        }
    }
}
