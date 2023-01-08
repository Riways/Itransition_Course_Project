using Dropbox.Api;
using Dropbox.Api.Files;
using Dropbox.Api.Sharing;
using totten_romatoes.Shared.Models;
using totten_romatoes.Shared;
using System.Net.Http.Headers;
using Duende.IdentityServer.EntityFramework.Entities;
using NuGet.Protocol;

namespace totten_romatoes.Server.Services
{
    public interface IDropboxService
    {
        public Task<string> UploadImageToDropbox(ImageModel imageToUpload);
    }

    public class DropboxService : IDropboxService
    {
        private DropboxClient _dropBoxClient;
        private readonly IConfiguration _appConfig;
        private static string _dropboxAccessToken = "";
        private static DateTime _expireDate = DateTime.UtcNow.AddHours(-1);

        public DropboxService(IConfiguration appConfig)
        {
            _appConfig = appConfig;
        }

        private void RefreshDropboxTokenIfNecessary()
        {
            if (_expireDate <= DateTime.UtcNow)
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var authenticationString = $"{_appConfig["dropbox_app_key"]}:{_appConfig["dropbox_secret"]}";
                    var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                    var response = httpClient.PostAsync($"{Constants.DROPBOX_URL}?grant_type=refresh_token&refresh_token={_appConfig["dropbox_refresh_token"]}", null).GetAwaiter().GetResult();
                    var wrappedToken = response.Content.ReadFromJsonAsync<DropboxTokenWrap>().GetAwaiter().GetResult();
                    _dropboxAccessToken = wrappedToken.access_token;
                    _dropBoxClient = new DropboxClient(_dropboxAccessToken);
                    _expireDate = _expireDate.AddHours(3);
                }
            }
        }

        public async Task<string> UploadImageToDropbox(ImageModel imageToUpload)
        {
            RefreshDropboxTokenIfNecessary();
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

        record DropboxTokenWrap(string? access_token, string? token_type, int? expires_in);
    }
}
