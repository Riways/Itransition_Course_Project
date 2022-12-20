using Dropbox.Api;
using Dropbox.Api.Files;
using Dropbox.Api.Sharing;
using System.Text;
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
        private readonly string DROPBOX_PATH = "/romatoes/";
        private string DROPBOX_ACCESS_TOKEN = "hidden";
        private DropboxClient _dropBoxClient;

        public DropboxService()
        {
            _dropBoxClient = new DropboxClient(DROPBOX_ACCESS_TOKEN);
        }

        public async Task<string> UploadImageToDropbox(ImageModel imageToUpload)
        {
            string pathToFileOnDropbox = $"{DROPBOX_PATH}{imageToUpload.ImageName}";
            string imageUrl;
            using (var mem = new MemoryStream(imageToUpload.ImageData))
            {
                var uploadedFile = await _dropBoxClient.Files.UploadAsync(
                    pathToFileOnDropbox,
                        WriteMode.Overwrite.Instance,
                        body: mem);
                PathLinkMetadata linkData  = await _dropBoxClient.Sharing.CreateSharedLinkAsync(pathToFileOnDropbox);
                imageUrl = linkData.Url;
            }
            return imageUrl;
        }
    }
}
