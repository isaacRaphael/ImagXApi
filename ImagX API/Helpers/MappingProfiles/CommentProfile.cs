using AutoMapper;
using ImagX_API.DTOs.OutGoing;
using ImagX_API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Helpers.MappingProfiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentResponseDto>().ForMember(d => d.CommenterID, opt => opt.MapFrom(s => s.AppUserId));
        }
    }
}
