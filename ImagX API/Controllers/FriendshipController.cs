using AutoMapper;
using ImagX_API.Contracts;
using ImagX_API.DTOs.InComing;
using ImagX_API.DTOs.OutGoing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FriendshipController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("UserBuddyList/{id}")]
        public async Task<ActionResult<ICollection<UserResponseDto>>> GetUsersFriendList(string id)
        {
            var friendList = await _unitOfWork.Friendships.GetFriendsOfUser(id);
            if (friendList is null)
            {
                return NotFound(new { Success = false, Message = "Unable to retrieve friend list" });
            }

            return Ok(_mapper.Map<ICollection<UserResponseDto>>(friendList));
        }

        [HttpDelete("RemoveAFriend")]
        public async Task<ActionResult> RemoveFriend(RemoveFriendDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Mesaage = "innvalid request body" });
            

           var check =  await _unitOfWork.Friendships.RemoveFriendOfUser(model.InitiatorId, model.ReceiverId);
            if (!check)
                return NotFound(new { Success = false, Message = "unable to process request" });

            return NoContent();
        }
    }
}
