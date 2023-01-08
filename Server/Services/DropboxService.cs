using Dropbox.Api;
using Dropbox.Api.Files;
using Dropbox.Api.Sharing;
using System.Net.Http.Headers;
using totten_romatoes.Shared;
using totten_romatoes.Shared.Models;

namespace totten_romatoes.Server.Services
{
    public interface IDropboxService
    {
        public Task<string> UploadImageToDropbox(ImageModel imageToUpload);
    }

    public class DropboxService : IDropboxService
    {
        private DropboxClient? _dropBoxClient;
        private readonly IConfiguration _appConfig;
        private static string _dropboxAccessToken = "";
        private static DateTime _expireDate = DateTime.UtcNow.AddHours(-1);

        public DropboxService(IConfiguration appConfig)
        {
            _appConfig = appConfig;
        }

        private void RefreshDropboxTokenIfNecessary()
        {
            if (_expireDate <= DateTime.UtcNow || _dropBoxClient == null)
            {
                using HttpClient httpClient = new();
                string authenticationString = $"{_appConfig["dropbox_app_key"]}:{_appConfig["dropbox_secret"]}";
                string base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                HttpResponseMessage response = httpClient.PostAsync($"{Constants.DROPBOX_URL}?grant_type=refresh_token&refresh_token={_appConfig["dropbox_refresh_token"]}", null).GetAwaiter().GetResult();
                DropboxTokenWrap? wrappedToken = response.Content.ReadFromJsonAsync<DropboxTokenWrap>().GetAwaiter().GetResult();
                _dropboxAccessToken = wrappedToken?.access_token ?? string.Empty;
                _dropBoxClient = new DropboxClient(_dropboxAccessToken);
                _expireDate = _expireDate.AddHours(3);
            }
        }

        public async Task<string> UploadImageToDropbox(ImageModel imageToUpload)
        {
            if (imageToUpload.ImageData == null)
            {
                throw new ArgumentException("No data to upload, fill imageData field");
            }
            RefreshDropboxTokenIfNecessary();
            string pathToFileOnDropbox = $"{Constants.DROPBOX_PATH}{imageToUpload.ImageName}";
            string imageUrl;
            using (MemoryStream mem = new(imageToUpload.ImageData))
            {
                FileMetadata uploadedFile = await _dropBoxClient!.Files.UploadAsync(
                        pathToFileOnDropbox,
                        WriteMode.Overwrite.Instance,
                        body: mem);
                SharedLinkMetadata linkData = await _dropBoxClient.Sharing.CreateSharedLinkWithSettingsAsync(pathToFileOnDropbox, new SharedLinkSettings(allowDownload: true));
                imageUrl = linkData.Url;
            }
            imageUrl = imageUrl.Remove(imageUrl.Length - 4, 4) + "raw=1";
            return imageUrl;
        }

        private record DropboxTokenWrap(string? access_token, string? token_type, int? expires_in);
    }
}
