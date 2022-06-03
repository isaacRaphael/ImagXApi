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
    }
}
