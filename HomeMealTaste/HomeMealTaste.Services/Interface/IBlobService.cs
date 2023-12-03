using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Interface
{
    public interface IBlobService
    {
        Task<string> UploadQuestImgAndReturnImgPathAsync(IFormFile file, string containerName);
        Task<string> UploadQuestImgAndReturnImgPathAsync111(string file, string containerName);
    }
}
