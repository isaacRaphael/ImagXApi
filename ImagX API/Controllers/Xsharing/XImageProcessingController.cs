using ImagX_API.Contracts;
using ImagX_API.DTOs.InComing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Controllers.Xsharing
{
    [Route("api/[controller]")]
    [ApiController]
    public class XImageProcessingController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public XImageProcessingController(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        [HttpGet("PlaceHolderImage")]
        public async Task<ActionResult> GetPlaceHolder([FromHeader] string key)
        {
            var exists = await _unitOfWork.Keys.Exists(key);
            if (!exists)
                return BadRequest(new { Success = false, Message = "invalid api key" });

            var imagePath = Directory.GetFiles("./Controllers/Xsharing/ImageStore");
            var media = Path.GetFileName(imagePath[0]);



            var result = await _imageService.AddImage(imagePath[0]);

            if (result is null || result.Length < 1)
                return NotFound(new { Success = false, Message = "Could'nt process request" });


            return Ok(new { Success = true, Message = "processed successfully", PlaceholderUrl = result });

        }

        [HttpGet("SpecifyPlaceHolderDimension")]
        public async Task<ActionResult> GetPlaceHolderByDimension([FromQuery] int width, [FromQuery] int height, [FromHeader] string key)
        {
            var exists = await _unitOfWork.Keys.Exists(key);
            if (!exists)
                return BadRequest(new { Success = false, Message = "invalid api key" });

            var imagePath = Directory.GetFiles("~/ImageStore");
            var media = Path.GetFileName(imagePath[0]);

            var result = await _imageService.AddWithDimensions(media, width, height);

            if (result is null || result.Length < 1)
                return NotFound(new { Success = false, Message = "Could'nt process request" });


            return Ok(new { Success = true, Message = "processed successfully", PlaceholderUrl = result });
        }

        [HttpGet("GetFilterNames")]
        public async Task<ActionResult> GetPossibleFilters([FromHeader] string key)
        {
            var exists = await _unitOfWork.Keys.Exists(key);
            if (!exists)
                return BadRequest(new { Success = false, Message = "invalid api key" });

            var filters = new List<string> { "al_dente", "athena", "audrey", "aurora", "daguerre", "eucalyptus", "fes", "frost", "hairspray",
            "hokusai", "incognito", "linen", "peacock", "primavera", "quartz", "red_rock", "refresh", "sizzle", "sonnet", "ukulele", "zorro"};

            return Ok(filters);

        }

        [HttpPost("AddFilter")]
        public async Task<ActionResult> AddFilter([FromHeader] string key, [FromForm] AddfilterDto model)
        {
            var exists = await _unitOfWork.Keys.Exists(key);
            if (!exists)
                return BadRequest(new { Success = false, Message = "invalid api key" });

            var filePath = Path.GetTempFileName();
            using (var stream = System.IO.File.Create(filePath))
            {
                model.Media.CopyTo(stream);
            }
            var result = await _imageService.AddWithFilter(filePath, model.Filter);

            if (result is null || result.Length < 1)
                return NotFound(new { Success = false, Message = "Could'nt process request" });

            return Ok(new { Success = true, Message = "processed successfully", PlaceholderUrl = result });
        }


        [HttpPost("CompressImage")]
        public async Task<ActionResult> AddCompression([FromHeader] string key, [FromForm]CompressImageDto model)
        {
            var exists = await _unitOfWork.Keys.Exists(key);
            if (!exists)
                return BadRequest(new { Success = false, Message = "invalid api key" });

            var filePath = Path.GetTempFileName();
            using (var stream = System.IO.File.Create(filePath))
            {
                model.Media.CopyTo(stream);
            }
            var result = await _imageService.AddWithCompression(filePath, model.CompressionIndex);
            if (result is null || result.Length < 1)
                return NotFound(new { Success = false, Message = "Could'nt process request" });

            return Ok(new { Success = true, Message = "processed successfully", PlaceholderUrl = result });
        }

        [HttpPost("ConvertFormat")]
        public async Task<ActionResult> ConvertImage([FromHeader] string key, [FromForm] ConvertImageDto model)
        {
            var exists = await _unitOfWork.Keys.Exists(key);
            if (!exists)
                return BadRequest(new { Success = false, Message = "invalid api key" });

            var possibleFormats = new List<string> { "jpg", "png", "gif", "bmp", "tiff", "ico", "pdf", "eps", "psd", "svg", "webp", "jxr", "wdp" };
            if (!possibleFormats.Contains(model.Format.ToLower()))
                return BadRequest(new { Success = false, Message = "unsupported format" });

            var filePath = Path.GetTempFileName();
            using (var stream = System.IO.File.Create(filePath))
            {
                model.Media.CopyTo(stream);
            }
            var result = await _imageService.ChangeFormat(filePath, model.Format);
            if (result is null || result.Length < 1)
                return NotFound(new { Success = false, Message = "Could'nt process request" });

            return Ok(new { Success = true, Message = "processed successfully", PlaceholderUrl = result });
        }
    }
}
