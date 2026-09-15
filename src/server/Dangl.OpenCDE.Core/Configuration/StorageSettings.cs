using Dangl.OpenCDE.Shared.Configuration;

namespace Dangl.OpenCDE.Core.Configuration
{
    public class StorageSettings
    {
        /// <summary>
        /// Directory on local disk where uploaded documents are stored, via
        /// <see cref="Dangl.AspNetCore.FileHandling.DiskFileManager"/>.
        /// </summary>
        public string LocalDiskBasePath { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(LocalDiskBasePath))
            {
                throw new InvalidConfigurationException($"{nameof(LocalDiskBasePath)} missing.");
            }
        }
    }
}
