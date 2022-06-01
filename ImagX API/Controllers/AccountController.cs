using AutoMapper;
using ImagX_API.Contracts;
using ImagX_API.DTOs.InComing;
using ImagX_API.DTOs.OutGoing;
using ImagX_API.Entities;
using ImagX_API.Services;
using ImagX_API.Services.HelperClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailService _emailservice;

        public AccountController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager , EmailService emailservice)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _emailservice = emailservice;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetById(string id)
        {
            var user = await _unitOfWork.Users.GetById(id);
            if (user is null)
                return NotFound(new { Success = false, Message = "user does not exist" });


            return Ok(_mapper.Map<UserResponseDto>(user));
        }

        [HttpGet("ByEmail")]
        public async Task<ActionResult<UserResponseDto>> GetByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return NotFound(new { Success = false, Message = "User not found" });

            return Ok(_mapper.Map<UserResponseDto>(user));
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(UserRegistrationDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Bad Request Body" });

            var emailCheck = await _userManager.FindByEmailAsync(model.Email);

            if (emailCheck is not null)
                return BadRequest(new { Success = false, Message = "User Already Exists" });

            var newuser = _mapper.Map<AppUser>(model);

            var check =  await _userManager.CreateAsync(newuser, model.Password);
            var ErrorList = new List<string>();
            if (!check.Succeeded)
            {
                check.Errors.ToList().ForEach(error => ErrorList.Add(error.Description));
                return BadRequest(new { Success = false, Message = string.Join(", ", ErrorList)});
            }
            var mail = new EmailModel { Title = "Welcome Message", Receipient = model.Email };
            await _emailservice.SendMail(mail);


            return CreatedAtAction(nameof(GetById), new { Id = newuser.Id }, _mapper.Map<UserResponseDto>(newuser));
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserResponseDto>> Login(UserLoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
                return BadRequest(new { Success = false, Message = "User does not exist" });

            var check = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!check)
                return BadRequest(new { Success = false, Message = "User does not exist" });

            return Ok(_mapper.Map<UserResponseDto>(user));
        }
    }
}
