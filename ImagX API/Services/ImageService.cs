using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ImagX_API.Contracts;
using ImagX_API.Services.HelperClasses;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Services
{
    public class ImageService : IImageService
    {
        private readonly CloudinaryObj _options;

        public ImageService(IOptions<CloudinaryObj> options)
        {
            _options = options.Value;
        }

        public async Task<string> AddImage(string path)
        {
            var myAccount = new Account { ApiKey = _options.Key, ApiSecret = _options.Secret, Cloud = _options.Name };
            Cloudinary _cloudinary = new(myAccount);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(path)
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.Url.AbsoluteUri;

        }

        public async Task<string> AddWithCompression(string path, int compressionIndex)
        {
            var myAccount = new Account { ApiKey = _options.Key, ApiSecret = _options.Secret, Cloud = _options.Name };
            Cloudinary _cloudinary = new(myAccount);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(path),
                Transformation = new Transformation().Quality(compressionIndex)
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.Url.AbsoluteUri;
        }

        public async Task<string> AddWithDimensions(string path, int width, int height)
        {
            var myAccount = new Account { ApiKey = _options.Key, ApiSecret = _options.Secret, Cloud = _options.Name };
            Cloudinary _cloudinary = new(myAccount);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(path),
                Transformation = new Transformation().Width(width).Height(height)
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.Url.AbsoluteUri;
        }

        public  async Task<string> AddWithFilter(string path, string filter)
        {
            var myAccount = new Account { ApiKey = _options.Key, ApiSecret = _options.Secret, Cloud = _options.Name };
            Cloudinary _cloudinary = new(myAccount);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(path),
                Transformation = new Transformation().Effect($"art:{filter}")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.Url.AbsoluteUri;
        }

        public async Task<string> ChangeFormat(string path, string format)
        {
            var myAccount = new Account { ApiKey = _options.Key, ApiSecret = _options.Secret, Cloud = _options.Name };
            Cloudinary _cloudinary = new(myAccount);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(path),
                
            };

            uploadParams.Format = format;

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.Url.AbsoluteUri;
        }
    }
}
