using Dangl.AspNetCore.FileHandling;
using Dangl.Data.Shared;
using Dangl.OpenCDE.Data.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Dangl.OpenCDE.Data.IO
{
    public interface ICdeAppFileHandler
    {
        /// <summary>
        /// This will also return a successful result even if the file could not be persisted to storage, only
        /// the metadata. It will only return a failure if an exception itself happens that was not caught inside
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="fileName"></param>
        /// <param name="container"></param>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        Task<RepositoryResult<Guid>> SaveFileAsync(Stream fileStream, string fileName, string container, string mimeType);

        Task<RepositoryResult<FileResultContainer>> GetFileByIdAsync(Guid fileId);

        /// <summary>
        /// Writes the content stream for a file record that was already created via
        /// <see cref="Repository.IDocumentsRepository.PrepareDocumentUploadAsync"/>, and
        /// marks it as available in storage. Used by the local upload endpoint that the
        /// openCDE upload session flow directs clients to PUT their bytes to.
        /// </summary>
        Task<RepositoryResult> WriteExistingFileContentAsync(Guid fileId, Stream fileStream);

        Task<RepositoryResult> DeleteFileAsync(Guid fileId);

        Task<bool> CheckIfFileExistsInStorageAsync(Guid fileId);

        Task<CdeAppFileMimeType> GetDbMimeTypeAsync(string mimeType);
    }
}
