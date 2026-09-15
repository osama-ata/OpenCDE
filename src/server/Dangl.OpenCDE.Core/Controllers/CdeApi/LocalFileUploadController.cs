using Dangl.Data.Shared;
using Dangl.OpenCDE.Data.IO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Dangl.OpenCDE.Core.Controllers.CdeApi
{
    /// <summary>
    /// Local-disk storage stand-in for the direct-to-storage PUT link that other openCDE
    /// deployments hand out as an Azure Blob SAS url (see
    /// <see cref="Dangl.OpenCDE.Data.Repository.DocumentsRepository"/> and
    /// <see cref="Dangl.OpenCDE.Data.Repository.OpenCdeDocumentSelectionRepository"/>,
    /// which both build links pointing here instead of to Azure). The file record itself
    /// must already exist (created via the document upload preparation flow) -- this
    /// endpoint only accepts the raw bytes for it.
    /// </summary>
    [Route("api/local-file-storage")]
    public class LocalFileUploadController : CdeAppControllerBase
    {
        private readonly ICdeAppFileHandler _cdeAppFileHandler;

        public LocalFileUploadController(ICdeAppFileHandler cdeAppFileHandler)
        {
            _cdeAppFileHandler = cdeAppFileHandler;
        }

        [HttpPut("{fileId}")]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> UploadFileContentAsync(Guid fileId)
        {
            // The request body is a forward-only Kestrel stream; buffer it so the file
            // manager can seek/read Length as it would for a regular uploaded file.
            await using var buffered = new MemoryStream();
            await Request.Body.CopyToAsync(buffered);
            buffered.Position = 0;

            var result = await _cdeAppFileHandler.WriteExistingFileContentAsync(fileId, buffered);
            if (!result.IsSuccess)
            {
                return BadRequest(new ApiError(result.ErrorMessage));
            }

            return NoContent();
        }
    }
}
