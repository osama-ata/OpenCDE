using System;

namespace Dangl.OpenCDE.Shared.Models.Controllers.Documents
{
    /// <summary>
    /// A URL the client can PUT a document's raw bytes to, and how long it stays valid.
    /// Previously an Azure Blob Storage SAS link; now always a link back to this
    /// server's own local upload endpoint, since storage is local disk.
    /// </summary>
    public class DocumentUploadLink
    {
        public string UploadLink { get; set; }

        public DateTimeOffset ValidUntil { get; set; }
    }
}
