using AutoMapper;
using HabVilla_CouponAPI.Models.DTO;
using HabVilla_CouponAPI.Models;

namespace HabVilla_CouponAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Coupon, CouponCreateDTO>().ReverseMap();
            CreateMap<Coupon, CouponDTO>().ReverseMap();
        }
    }
}
