using AutoMapper;
using ImagX_API.Contracts;
using ImagX_API.DTOs.InComing;
using ImagX_API.DTOs.OutGoing;
using ImagX_API.Entities;
using ImagX_API.Hubs;
using ImagX_API.Options;
using ImagX_API.Services;
using ImagX_API.Services.HelperClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
        private readonly IHubContext<ChatHub> _chatContext;
        private readonly IHubContext<NotificationHub> _notificationContext;
        private readonly JwtSettings _jwtSettings;

        public AccountController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager , EmailService emailservice, IHubContext<ChatHub> chatContext, IHubContext<NotificationHub> notificationContext, JwtSettings jwtSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _emailservice = emailservice;
            _chatContext = chatContext;
            _notificationContext = notificationContext;
            _jwtSettings = jwtSettings;
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
            var mail = new EmailModel { Title = "Welcome Message", Receipient = model.Email , Body = $"<h3>Dear {model.UserName}, welcome to <p>ImagX</a>!</h3><br />May the photo force be with you!" };
            try
            {
                await _emailservice.SendMail(mail);
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, newuser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, newuser.Email),
                    new Claim("id", newuser.Id)
                    }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key) , SecurityAlgorithms.HmacSha256Signature)
            };

            var gentoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(gentoken);

            if (token.Length < 1 || token is null)
                return NotFound(new { success = false, Message = "could not process request" });

            return Ok(new { Success = true, Message = "request completed", Token = token, User = _mapper.Map<UserResponseDto>(newuser) });
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

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id)
                    }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var gentoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(gentoken);

            return Ok(new { Success = true, Message = " Login processed ", Token = token, User = _mapper.Map<UserResponseDto>(user) });


        }

        [HttpPost]
        [Route("PasswordReset")]
        public async Task<IActionResult> SendPasswordResetLink([FromQuery] string email)
        {
            var user = await  _userManager.FindByEmailAsync(email);

         
            if (user is null)
                return BadRequest(new { Success = false, Message = "No user with that email" });
            

            // generate reset token
            var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;


            // generate random token
            Random generator = new Random();
            string emailToken = generator.Next(0, 1000000).ToString("D6");

            var resetToken = new ResetToken
            {
                EmailToken = emailToken,
                ResetPasswordToken = token
            };

            var check = await _unitOfWork.ResetTokens.Add(resetToken);

            if (check is null)
                return NotFound(new { Success = false, Message = "Could not process request" });

            var emailModel = new EmailModel()
            {
                Receipient = email,
                Title = "IMAGX:  A LINK TO RESET YOUR PASSWORD",
                Body = $"Dear {user.UserName} this is your reset token {emailToken}"
            };

            await _emailservice.SendMail(emailModel);

            return Ok(new { Success = true, Message = "an email has been sent successfully" });
        }

        [HttpPost]
        [Route("PasswordReset/Confirm")]
        public async Task<IActionResult> PasswordResetConfirm(ResetPasswordConfirmationDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "bad request body" });
            var tokenModel = await _unitOfWork.ResetTokens.GetByToken(model.Token);
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (tokenModel is null)
                return BadRequest(new { Success = false, Error = "wrong token inputed" });

            if(user is null)
                return BadRequest(new { Success = false, Error = "wrong email inputed" });


            try
            {
                await _userManager.ResetPasswordAsync(user, tokenModel.ResetPasswordToken, model.NewPassword);
                return Ok(new { Success = true , Message = "Password reset successfully"});
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            
        }
    }
}
