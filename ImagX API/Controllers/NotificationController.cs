using AutoMapper;
using ImagX_API.Contracts;
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
    public class NotificationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("UserNotification/{id}")]
        public async Task<ActionResult<ICollection<NotificationResponseDto>>> GetUserNotifications(string id)
        {
            var notifications = await _unitOfWork.Notifications.GetUserNotifications(id);
            if (notifications is null)
                return NotFound(new { Success = false, Message = "Couldnt retrieve notifications" });

            return Ok(_mapper.Map<ICollection<NotificationResponseDto>>(notifications));
        }

        [HttpDelete("{notificationId}")]
        public async Task<ActionResult> Remove(int notificationId)
        {
            var check = await _unitOfWork.Notifications.Remove(notificationId);

            if (!check)
                return NotFound(new { Success = false, Message = "Could not process request" });

            return NoContent();
        }
    }
}
