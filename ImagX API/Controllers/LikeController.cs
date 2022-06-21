using ImagX_API.Contracts;
using ImagX_API.DTOs.InComing;
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
    public class LikeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LikeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult> AddLike(AddLikeDto addLikeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "incorrect request details" });

            var post = await _unitOfWork.Posts.GetById(addLikeDto.PostId);
            var poster = await _unitOfWork.Users.GetById(addLikeDto.UserId);

            if (post is null || poster is null)
                return NotFound(new { Success = false, Message = "No such post or poster" });

            var hasLiked = await _unitOfWork.Likes.UserHasLiked(poster.Id, post.Id);
            if (hasLiked)
                return BadRequest(new { Success = false, Message = "This User has already liked" });

            var like = new Like { AppUser = poster, AppUserId = poster.Id, Post = post, PostId = post.Id };

            var result = await _unitOfWork.Likes.Add(like);
            if (result is null)
                return NotFound(new { Success = false, Message = "error adding like" });

             var newNotification = new Notification
            {
                RecipientID = post.AppUserId,
                SenderID = poster.Id,
                ActionType = "Like",
                Created = DateTime.UtcNow,
                ActionId = result.Id
            };
            await _unitOfWork.Notifications.Add(newNotification);

            return Ok(new { Success = true, Message = "Like added success fully" });

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await _unitOfWork.Likes.Exists(id);

            if (!exists)
                return NotFound(new { Success = false, Message = "no like matches id" });

          var done =  await _unitOfWork.Likes.Remove(id);
            if (!done)
                return NotFound(new { Success = false, Message = "like delete unsuccessful" });

            return Ok(new { Success = true, Message = "deleted successfully" });
        }


    }
}
