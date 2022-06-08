using AutoMapper;
using ImagX_API.Contracts;
using ImagX_API.DTOs.InComing;
using ImagX_API.DTOs.OutGoing;
using ImagX_API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet("PostComments/{postId}")]
        public async Task<ActionResult<ICollection<CommentResponseDto>>> GetPostComments(int postId)
        {
            var result = await _unitOfWork.Comments.GetCommentsOfPost(postId);
            if (result is null)
                return NotFound(new { Success = false, Message = "post not found" });

            return Ok(_mapper.Map<ICollection<CommentResponseDto>>(result));
        }
        [HttpPost]
        public async Task<ActionResult> Add(AddCommentDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "incomplete request body" });

            var post = await _unitOfWork.Posts.GetById(model.PostId);
            if (post is null)
                return BadRequest(new { Success = false, Message = "no such post exists" });

            var commenter = await _unitOfWork.Users.GetById(model.CommenterID);
            if (commenter is null)
                return BadRequest(new { Success = false, Message = "nos such user exists" });

            var comment = new Comment
            {
                AppUser = commenter,
                PayLoad = model.PayLoad,
                AppUserId = commenter.Id,
                Post = post,
                PostId = post.Id
            };

            var result = await _unitOfWork.Comments.Add(comment);
            if (result is null)
            {
                return BadRequest(new { Success = false, Message = "comment was not created" });
            }

            return Ok(new { Success = true, Message = "Comment created successfully" });
        }


        [HttpDelete("commentId")]
        public async Task<ActionResult> Remove (int commentId)
        {
            var result = await _unitOfWork.Comments.Remove(commentId);
            if (!result)
                return NotFound(new { Success = false , Message = "could'nt perform delete"});

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateComment([FromBody] JsonPatchDocument entity, [FromRoute] int id)
        {
            var patch = await _unitOfWork.Comments.Update(entity, id);
            if (!patch)
                return NotFound(new { Success = false, Message = "No such comment" });

            return Ok(new { Success = true, Message = "Updated Comment" });

        }
    }
}
