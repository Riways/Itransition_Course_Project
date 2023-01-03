using Dropbox.Api;
using Dropbox.Api.Files;
using Dropbox.Api.Sharing;
using totten_romatoes.Shared.Models;
using totten_romatoes.Shared;

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
            _dropBoxClient = new DropboxClient(_appConfig["dropboxAccessToken"]);
        }

        public async Task<string> UploadImageToDropbox(ImageModel imageToUpload)
        {
            string pathToFileOnDropbox = $"{Constants.DROPBOX_PATH}{imageToUpload.ImageName}";
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
