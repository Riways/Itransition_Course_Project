using Dropbox.Api;
using Dropbox.Api.Files;
using Dropbox.Api.Sharing;
using Dropbox.Api.TeamLog;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using totten_romatoes.Shared.Models;
using static Dropbox.Api.Files.SearchMatchType;

namespace totten_romatoes.Server.Services
{
    public interface IDropboxService
    {
        public Task<string> UploadImageToDropbox(ImageModel imageToUpload);
    }

    public class DropboxService : IDropboxService
    {
        private readonly DropboxClient _dropBoxClient;
        private readonly IConfiguration _appConfig;

        public DropboxService(IConfiguration appConfig)
        {
            _appConfig = appConfig;
            _dropBoxClient = new DropboxClient(_appConfig["DropboxAccessToken"]);
        }

        public async Task<string> UploadImageToDropbox(ImageModel imageToUpload)
        {
            string pathToFileOnDropbox = $"{ServerConstants.DROPBOX_PATH}{imageToUpload.ImageName}";
            string imageUrl;
            using (var mem = new MemoryStream(imageToUpload.ImageData))
            {
                var uploadedFile = await _dropBoxClient.Files.UploadAsync(
                        pathToFileOnDropbox,
                        WriteMode.Overwrite.Instance,
                        body: mem);
                SharedLinkMetadata linkData  = await _dropBoxClient.Sharing.CreateSharedLinkWithSettingsAsync(pathToFileOnDropbox, new SharedLinkSettings(allowDownload:true));
                imageUrl = linkData.Url;
            }
            imageUrl = imageUrl.Remove(imageUrl.Length - 4, 4) + "raw=1";
            return imageUrl;
        }
    }
}
