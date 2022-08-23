using Microsoft.AspNetCore.WebUtilities;

namespace MultimediaStream.Interface
{
    public interface IStreamFileUploadService
    {
        Task<bool> UploadFile(MultipartReader reader, MultipartSection section, string name);
    }
}
