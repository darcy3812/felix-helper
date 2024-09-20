using FluentFTP;
using Microsoft.Extensions.Options;
using System.IO;

namespace FelixHelper.Services
{
    public class FtpService
    {
        private readonly FtpClient _client;
        private readonly HostingConfiguration _hostingConfig;

        public FtpService(IOptions<HostingConfiguration> hostingConfig)
        {
            _hostingConfig = hostingConfig.Value;
            _client = new FtpClient(_hostingConfig.FtpPath, _hostingConfig.Login, _hostingConfig.Password);
        }

        public bool DownloadFile(Stream stream)
        {
            _client.AutoConnect();
            return _client.DownloadStream(stream, _hostingConfig.FilePath);
        }

        public bool UploadFile(Stream stream)
        {
            _client.AutoConnect();
            var status = _client.UploadStream(stream, _hostingConfig.FilePath);

            return status == FtpStatus.Success;
        }
    }
}
