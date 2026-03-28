using Imagekit;
using Imagekit.Sdk;
using Microsoft.Extensions.Options;
using NearU_Backend_Revised.Configuration;
using NearU_Backend_Revised.Services.Interfaces;
using NearU_Backend_Revised.Models;

namespace NearU_Backend_Revised.Services
{
    public class ImageService : IImageService
    {
        private readonly ImageKitSetting _settings;

        public ImageService(IOptions<ImageKitSetting> settings) //IOptions<ImageKitSetting> reads from appsetting auto
        {
            _settings = settings.Value;
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folder)
        {
            using var memoryStream = new MemoryStream(); //convert file to raw bytes

            await file.CopyToAsync(memoryStream); //copy file contents to memory stream

            var fileBytes = memoryStream.ToArray(); //convert memory stream to byte array

            //create imagekit client
            var imageKit = new ImagekitClient(
                _settings.PublicKey,
                _settings.PrivateKey,
                _settings.UrlEndpoint
                );

            var fileName = $"{Guid.NewGuid()}_{Path.GetExtension(file.FileName)}"; //generate unique file name

            var uploadRequest = new FileCreateRequest
            {
                file = fileBytes, //real file bytes
                fileName = fileName, //filename in imagekit
                folder = folder //folder in imagekit (which folder foodshop or menuitem)
            };

            var result = await imageKit.UploadAsync(uploadRequest); //upload file to imagekit

            return result.url; //return the URL of the uploaded image
        }
    }
}