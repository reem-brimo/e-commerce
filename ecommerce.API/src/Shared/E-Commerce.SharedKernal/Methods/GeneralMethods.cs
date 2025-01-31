using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace E_Commerce.SharedKernal.Methods
{
    public class GeneralMethods
    {
        #region Properties and constructors

        private readonly IHostingEnvironment _hostingEnvironment;

        public GeneralMethods(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        #endregion

        #region Methods

        public string AddFile(IFormFile file, string path)
        {
            string directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, path);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            Guid fileName = Guid.NewGuid();
            string fileExtension = Path.GetExtension(file.FileName);
            string physicalFileName = $"{fileName}{fileExtension}";

            string physicalFilePath = Path.Combine
                (_hostingEnvironment.WebRootPath, path, physicalFileName);
            string checksum = null;
            using (FileStream fileStream = new FileStream(physicalFilePath, FileMode.Create))
            using (HashAlgorithm checksumAlgorithm = new SHA1Managed())
            {
                file.CopyTo(fileStream);
                checksum = GetChecksum(fileStream, checksumAlgorithm);
            }
            return Path.Combine(path, physicalFileName); ;
        }

        public bool RemoveFile(string path)
        {
            try
            {
                string pathToFile = Path.Combine(_hostingEnvironment.WebRootPath, path);
                if (File.Exists(pathToFile))
                    File.Delete(pathToFile);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string GetChecksum(Stream stream, HashAlgorithm checksumAlgorithm)
        {
            return BitConverter.ToString(checksumAlgorithm.ComputeHash(stream)).Replace("-", string.Empty);
        }
        #endregion
    }
}
