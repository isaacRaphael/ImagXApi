using ImagX_API.Contracts;
using ImagX_API.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Controllers.Xsharing
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ApiKeyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public ApiKeyController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult> RetrieveApiKey(string userId)
        {
            var user = await _unitOfWork.Users.GetById(userId);

            if (user is null)
                return BadRequest(new { Success = false, Message = "cant retieve user" });

            var presentKey = await _unitOfWork.Keys.GetAll();
            var p = presentKey.FirstOrDefault(k => k.AppUserId == userId);

            if (p is not null)
                return Ok(p);

            var key = new SharingKey { AppUserId = user.Id };
            var result = await _unitOfWork.Keys.Add(key);

            if (result is null)
                return NotFound(new { Success = false, Message = "could not comple process" });

            return Ok(result);
        }

    }
}
