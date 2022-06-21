using AutoMapper;
using ImagX_API.Contracts;
using ImagX_API.DTOs.InComing;
using ImagX_API.DTOs.OutGoing;
using ImagX_API.Entities;
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
    public class ChatController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ChatController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpGet("UserChats/{id}")]
        public async Task<ActionResult<ChatResponseDto>> GetUserChats(string id)
        {
            var chats = await _unitOfWork.Chats.GetUserChats(id);
            if (chats is null)
                return NotFound(new { Success = false, Message = "Could not pull chats" });

            return Ok(_mapper.Map<ICollection<ChatResponseDto>>(chats));
        }
        [HttpGet("ChatOfTwoParties")]
        public async Task<ActionResult<ChatResponseDto>> GetUserChats(TwoPartiesChatDto model)
        {
            var chat = await _unitOfWork.Chats.GetTwoPartChat(model.partyAId, model.partyBId);
            if (chat is null)
                return NotFound(new { Success = false, Message = "Could not pull chats" });

            return Ok(_mapper.Map<ChatResponseDto>(chat));
        }

        [HttpPost]
        public async Task<ActionResult<ChatResponseDto>> Add(ChatInputDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid Requst Body" });

            var exists = await _unitOfWork.Chats.GetTwoPartChatExists(model.PartyAId, model.PartyBId);
            if(exists)
                return BadRequest(new { Success = false, Message = "Chat already Exist" });


            var partyA = await _unitOfWork.Users.GetById(model.PartyAId);
            var partyB = await _unitOfWork.Users.GetById(model.PartyBId);

            if(partyA is null || partyB is null)
                return BadRequest(new { Success = false, Message = "Invalid Requst Body" });


            var chat = new Chat() 
            { 
              PartyA = partyA,
              PartyB = partyB,
              PartyAId = partyA.Id,
              PartyBId = partyB.Id 
            };

            var check = await _unitOfWork.Chats.Add(chat);
            if (check is null)
                return NotFound(new { Success = false, Message = "Could nut process request" });

            return Ok(_mapper.Map<ChatResponseDto>(check));

            
        }

        [HttpDelete("{chatId}")]
        public async Task<ActionResult> Remove(int chatId)
        {
            var check = await _unitOfWork.Chats.Remove(chatId);

            if(!check)
                return NotFound(new { Success = false, Message = "Could nut process request" });

            return NoContent();
        }
    }
}
