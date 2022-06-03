using AutoMapper;
using ImagX_API.Contracts;
using ImagX_API.DTOs.InComing;
using ImagX_API.DTOs.OutGoing;
using ImagX_API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuddyRequestController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BuddyRequestController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
            var request = _mapper.Map<BuddyRequest>(model);

            var done = await  _unitOfWork.Buddies.AddBuddy(request);
            if (!done)
                return StatusCode(501, new { Success = false, Message = "Error while handling add buddyRequest" });
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
