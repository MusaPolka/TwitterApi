using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;

namespace Client.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _TwitterApiScope = string.Empty;
        private readonly string _TwitterApiBaseAddress = string.Empty;
        private readonly ITokenAcquisition _tokenAcquisition;

        public UserController(ITokenAcquisition tokenAcquisition, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _tokenAcquisition = tokenAcquisition;
            _TwitterApiScope = configuration["TwitterApi:TwitterApiScope"];
            _TwitterApiBaseAddress = configuration["TwitterApi:TwitterApiBaseAddress"];
        }
        [AuthorizeForScopes(Scopes = new[] { "https://akvelontwitterapi.onmicrosoft.com/b26d7a5b-7d4b-4342-93ad-afffd8d8ae8e/Files.Read" })]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();

            await AddHeaders(client);

            var response = await client.GetAsync($"{ _TwitterApiBaseAddress}/api/user/GetCurrentUser");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserViewModel>(content);

                return View(user);
            }

            throw new HttpRequestException($"Invalid status code {response.StatusCode}.");
        }

        private async Task AddHeaders(HttpClient client)
        {
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { _TwitterApiScope });
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

    }
}
