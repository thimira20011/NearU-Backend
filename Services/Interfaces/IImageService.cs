namespace NearU_Backend_Revised.Services.Interfaces
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file, string folder); //IFormFile .net type for file coming from HTTP req
        //take file from HTTP req and return URL 
    }
}