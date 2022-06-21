using AutoMapper;
using ImagX_API.Contracts;
using ImagX_API.DTOs.InComing;
using ImagX_API.DTOs.OutGoing;
using ImagX_API.Entities;
using ImagX_API.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class BuddyRequestController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public BuddyRequestController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<BuddyRequestDto>> GetAll()
        {
            var brs = await _unitOfWork.Buddies.GetAll();
            if (brs is null)
                return NotFound();
            return Ok(brs);
        }



        [HttpPost]
        public async Task<ActionResult> AddBuddies(BuddyRequestDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid Request Body" });

            var hasCeck = await _unitOfWork.Users.Hasfriend(model.SenderId, model.RecipientId);
            if (hasCeck)
                return BadRequest(new { Success = false, Message = "Frienship already exists" });

            var request = _mapper.Map<BuddyRequest>(model);

            var done = await _unitOfWork.Buddies.Add(request);
            if (done is null)
                return NotFound( new { Success = false, Message = "Error while handling add buddyRequest" });

            var newNotification = new Notification
            {
                RecipientID = done.RecipientId,
                SenderID = done.SenderId,
                ActionType = "BuddyRequest",
                Created = DateTime.UtcNow,
                ActionId = done.Id
            };

            await _unitOfWork.Notifications.Add(newNotification);

            return Ok(new { Success = true, Message = "BuddyRequest sent Succesfully", PayLoad = request });

        }

        [HttpPost("ByUserName")]
        public async Task<ActionResult> AddBuddiesByUserName([FromQuery]string senderUsername , [FromQuery]string receipientUsername)
        {
            var a = await _userManager.FindByNameAsync(senderUsername);
            var b = await _userManager.FindByNameAsync(receipientUsername);

            if (a is null || b is null)
                return BadRequest(new { Success = false, Message = "one or more invalid usernames" });

            var hasCheck = await _unitOfWork.Users.Hasfriend(a.Id, b.Id);
            if(hasCheck)
                return BadRequest(new { Success = false, Message = "Frienship already exists" });

            var request = new BuddyRequest { SenderId = a.Id, RecipientId = b.Id };

            var done = await _unitOfWork.Buddies.Add(request);
            if (done is null)
                return NotFound( new { Success = false, Message = "Error while handling add buddyRequest" });

            var newNotification = new Notification
            {
                RecipientID = done.RecipientId,
                SenderID = done.SenderId,
                ActionType = "BuddyRequest",
                Created = DateTime.UtcNow,
                ActionId = done.Id
            };

            await _unitOfWork.Notifications.Add(newNotification);

            return Ok(new { Success = true, Message = "BuddyRequest sent Succesfully", PayLoad = request });


        }

        [HttpPost("ConfirmRequest/{id}")]
        public async Task<ActionResult> ConfirmRequest(int id)
        {
            var check = await _unitOfWork.Buddies.ConfirmBuddyRequest(id);
            if (!check)
                return NotFound(new { Success = false, Message = "No buddy request exist with that id" });

            return Ok(new { Success = true, Message = "Request Confirmed" });
        }


    }
}
