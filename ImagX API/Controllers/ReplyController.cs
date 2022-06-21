using AutoMapper;
using ImagX_API.Contracts;
using ImagX_API.DTOs.InComing;
using ImagX_API.DTOs.OutGoing;
using ImagX_API.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
    public class ReplyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReplyController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("CommentsReply/{commentId}")]
        public async Task<ActionResult<ICollection<ReplyResponseDto>>> GetCommentReplies(int commentId)
        {
            var replies = await _unitOfWork.Replies.GetCommentReplies(commentId);

            if (replies is null)
                return NotFound(new { Success = false, Message = "No replies matches the comment id" });

            return Ok(_mapper.Map<ICollection<ReplyResponseDto>>(replies));
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddReplyDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "incomplete request body" });

            var comment = await _unitOfWork.Comments.GetById(model.CommentId);
            if (comment is null)
                return BadRequest(new { Success = false, Message = "no such comment exists" });

            var replier = await _unitOfWork.Users.GetById(model.ReplierId);
            if (replier is null)
                return BadRequest(new { Success = false, Message = "nos such user exists" });

            var reply = new Reply
            {
                AppUser = replier,
                PayLoad = model.PayLoad,
                AppUserId = replier.Id,
                Comment = comment,
                CommentId = comment.Id
            };

            var result = await _unitOfWork.Replies.Add(reply);
            if (result is null)
            
                return BadRequest(new { Success = false, Message = "Reply was not created" });

            var newNotification = new Notification
            {
                RecipientID = comment.AppUserId,
                SenderID = replier.Id,
                ActionType = "Reply",
                Created = DateTime.UtcNow,
                ActionId = result.Id
            };

            await _unitOfWork.Notifications.Add(newNotification);
            return Ok(new { Success = true, Message = "Reply created successfully" });
        }


        [HttpDelete("replyId")]
        public async Task<ActionResult> Remove(int replyId)
        {
            var result = await _unitOfWork.Replies.Remove(replyId);
            if (!result)
                return NotFound(new { Success = false, Message = "could'nt perform delete" });

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateComment([FromBody] JsonPatchDocument entity, [FromRoute] int id)
        {
            var patch = await _unitOfWork.Replies.Update(entity, id);
            if (!patch)
                return NotFound(new { Success = false, Message = "No such reply" });

            return Ok(new { Success = true, Message = "Updated reply" });

        }
    }
}

