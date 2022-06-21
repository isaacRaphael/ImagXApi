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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public PostController(IUnitOfWork unitOfWork, IImageService imageService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<ICollection<PostResponseDto>>> GetAll()
        {
            var posts = await _unitOfWork.Posts.GetAll();
            if (posts is null)
                return NotFound(new { Success = false, Message = "Coudnt access posts" });

            return Ok(_mapper.Map<ICollection<PostResponseDto>>(posts));

        }

        [HttpGet]
        [Route("Chronological")]
        public async Task<ActionResult<ICollection<PostResponseDto>>> GetAllChronological()
        {
            var posts = await _unitOfWork.Posts.GetAll();
            var chronoLogical = posts.OrderByDescending(x => x.Created).ToList();
            if (posts is null)
                return NotFound(new { Success = false, Message = "Coudnt access posts" });

            return Ok(_mapper.Map<ICollection<PostResponseDto>>(chronoLogical));
        }

        [HttpGet("Paginated")]
        public async Task<ActionResult<ICollection<PostResponseDto>>> GetPaginated([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var posts = await _unitOfWork.Posts.GetPaginated(page, size);
            if (posts is null)
                return NotFound(new { Success = false, Message = "Coudnt access posts" });

            return Ok(_mapper.Map<ICollection<PostResponseDto>>(posts));

        }

        [HttpGet("Paginated&Chronological")]
        public async Task<ActionResult<ICollection<PostResponseDto>>> GetPaginatedandChronological([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var posts = await _unitOfWork.Posts.GetPaginated(page,size);
            var chronoLogical = posts.OrderByDescending(x => x.Created).ToList();
            if (posts is null)
                return NotFound(new { Success = false, Message = "Coudnt access posts" });

            return Ok(_mapper.Map<ICollection<PostResponseDto>>(chronoLogical));

        }

        [HttpGet("PopularPostPAginated")]
        public async Task<ActionResult<ICollection<PostResponseDto>>> GetPopularPaginated([FromQuery] int page = 1, [FromQuery] int size = 10, string metric = "Likes")
        {
            var posts = await _unitOfWork.Posts.GetPaginated(page, size);
            var chronoLogical = posts.OrderByDescending(x => _unitOfWork.Likes.LikesOfAPost(x.Id).Result.Count).ToList();
            if (chronoLogical is null)
                return NotFound(new { Success = false, Message = "Coudnt access posts" });

            return Ok(_mapper.Map<ICollection<PostResponseDto>>(chronoLogical));

        }

        [HttpGet("BuddyPosts{userId}")]
        public async Task<ActionResult<ICollection<PostResponseDto>>> GetBuddyPosts(string userId)
        {
            var posts = await _unitOfWork.Posts.GetPostsOfBuddies(userId);
            if (posts is null)
                return NotFound(new { Success = false, Message = "Coudnt access posts" });

            return Ok(_mapper.Map<ICollection<PostResponseDto>>(posts));

        }

        [HttpGet("OfUser/{id}")]
        public async Task<ActionResult<ICollection<PostResponseDto>>> GetUsersPost(string id)
        {
            var posts = await _unitOfWork.Posts.GetPostByUser(id);

            if (posts is null)
                return NotFound(new { Success = false, Message = "Coudnt access posts from this user" });

            return Ok(_mapper.Map<ICollection<PostResponseDto>>(posts));
        }

        

        [HttpPost]
        public async Task<ActionResult> AddPost([FromForm] AddPostDto objfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Success = false, Message = "invalid request payload" });
            }
            string imgUrl = "";
            if(objfile.Media is not null)
            {
                var filePath = Path.GetTempFileName();
                using (var stream = System.IO.File.Create(filePath))
                {
                    objfile.Media.CopyTo(stream);

                }
                var result = await _imageService.AddImage(filePath);


                if (result is not null && result.Length > 0)
                    imgUrl = result;
            }
            var poster = await  _unitOfWork.Users.GetById(objfile.UserId);
            var post = new Post
            {
                AppUser = poster,
                Caption = objfile.Caption,
                Created = DateTime.UtcNow,
                AppUserId = poster.Id,
                ImageUri = imgUrl,
            };

            var mainResult = _unitOfWork.Posts.Add(post);
            if (mainResult is null)
                return  StatusCode(501 , new { Success = false, Message = "Action not carried out"});

            return Ok(new { Success = true, Message = "Post Created" });

        }

        [HttpGet("Likes/{postId}")]
        public async Task<ActionResult<ICollection<LikeResponseDto>>> GetPostLikes(int postId)
        {
            var Likes = await _unitOfWork.Likes.LikesOfAPost(postId);
            if (Likes is null)
                return BadRequest();


            return Ok(_mapper.Map<ICollection<LikeResponseDto>>(Likes));

        }

        [HttpGet("LikeCount/{postId}")]
        public async Task<ActionResult> GetPostLikeCount(int postId)
        {
            var Likes = await _unitOfWork.Likes.LikesOfAPost(postId);
            if (Likes is null)
                return BadRequest();


            return Ok(new {Success = true, Count = Likes.Count});

        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdatePost([FromBody] JsonPatchDocument entity, [FromRoute] int id)
        {
            var patch = await _unitOfWork.Posts.Update(entity, id);
            if (!patch)
                return NotFound(new { Success = false, Message = "No such post" });

            return Ok(new { Success = true, Message = "Updated post" });

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletPost(int id)
        {
            var check = await _unitOfWork.Posts.Remove(id);

            if (!check)
                return NotFound(new { Success = false, Message = "No Post with such Id" });

            return Ok(new { Success = false, Message = "Post Deleted Successfully" });
        }

    }
}
