using Microsoft.AspNetCore.Mvc;
using webapi.Interfaces;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IBlobStorageService _blobService;

        public FileController(IBlobStorageService blobService)
        {
            _blobService = blobService ?? throw new ArgumentNullException();
        }

        [HttpPost, Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromQuery] string email)
        {
            if (EmailValidator.IsValidEmail(email))
            {
                if (file != null && file.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                {
                    await _blobService.UploadFileToBlobStorage(file, email);
                    return Ok();
                }
                else
                {
                    return BadRequest("Invalid file type. Allowed type DOCX.");
                }
            }
            return BadRequest("Invalid email address");
        }
    }
}
