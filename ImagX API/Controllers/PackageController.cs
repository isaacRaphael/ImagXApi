using ImagX_API.Contracts;
using ImagX_API.DTOs.InComing;
using ImagX_API.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class PackageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IImageService _imageService;

        public PackageController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _imageService = imageService;
        }

        [HttpPost]
        public async Task<ActionResult> SendPackage(SendPackageDto model)
        {
            var a = new List<AppUser>();

            foreach(var item in model.Usernames)
            {
                var toadd = await _userManager.FindByNameAsync(item);
                if (toadd is null)
                    return BadRequest(new { Success = false, Message = "an incorrent username is present" });

                a.Add(toadd);
            }
            var imageUrl = "";

            if (model.Media is not null)
            {
                var filePath = Path.GetTempFileName();
                using (var stream = System.IO.File.Create(filePath))
                {
                    model.Media.CopyTo(stream);

                }
                var result = await _imageService.AddImage(filePath);


                if (result is not null && result.Length > 0)
                    imageUrl = result;
            }

            var resultlist = new List<Package>();
            foreach (var item in a)
            {
                var package = new Package { senderId = model.senderId, Caption = model.Caption, Image = imageUrl, receiverId = item.Id };
                var t = await _unitOfWork.Packages.Add(package);
                resultlist.Add(t);
            }

            if (resultlist.Count != a.Count)
                return NotFound(new { Success = false, Message = "problem handling request of one user" });

            return Ok(new { Success = true, Message = "sent successfully" });
        }

        [HttpGet("UserPackages/{userId}")]
        public async Task<ActionResult> RetrieveUserPackages(string userId)
        {
            var packages = await _unitOfWork.Packages.RetrieveUserPackages(userId);
            if (packages is null)
                return BadRequest(new { Success = true, Message = "cannot complete retrieve operation" });

            return Ok(packages);
        }

    }
}
