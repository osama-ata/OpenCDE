using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dangl.OpenCDE.Shared.Models.Controllers.Documents
{
    public class DocumentContentSasUploadResultGet
    {
        [Required]
        public DocumentUploadLink SasUploadLink { get; set; }

        [Required]
        public List<DocumentContentSasUploadResultHeaderGet> CustomHeaders { get; set; }
    }
}
