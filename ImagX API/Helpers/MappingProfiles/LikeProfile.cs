using AutoMapper;
using ImagX_API.DTOs.OutGoing;
using ImagX_API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Helpers.MappingProfiles
{
    public class LikeProfile : Profile
    {
        public LikeProfile()
        {
            CreateMap<Like, LikeResponseDto>().ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                                                .ForMember(d => d.LikerId, opt => opt.MapFrom(s => s.AppUserId))
                                                .ForMember(d => d.PostId, opt => opt.MapFrom(s => s.PostId));
        }
    }
}
