using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Contracts
{
    public interface IImageService
    {
        Task<string> AddImage(string path);
        Task<string> AddWithDimensions(string path, int width, int height);
        Task<string> AddWithFilter(string path, string filter);
        Task<string> AddWithCompression(string path, int compressionIndex);
        Task<string> ChangeFormat(string path, string format);

    }
}
