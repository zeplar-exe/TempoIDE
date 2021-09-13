using System;
using System.Net.Http;
using System.Threading.Tasks;
using Jammo.HTTP;
using Newtonsoft.Json.Linq;
using TempoIDE.UserControls.Editors;

namespace TempoIDE.Core.Static
{
    public static class SourceCodeHelper
    {
        public static SourceViewer FromGithub()
        { // TODO: infinite hang
            var editor = new SourceViewer();
            var url = "https://github.com";

            var owner = "zeplar-exe"; // use DotNetFoundation user
            var repo = "TempoIDE";
            var request = $"/repos/{owner}/{repo}/git/commits/";
            
            var client = new JHttpClient(new Url(url));
            client.NavigateRelative(new RelativePath(request));
            
            var treeData = JObject.Parse(client.Get().Content.ReadAsStringAsync().Result);

            client = new JHttpClient(new Url(url));
            client.NavigateRelative(new RelativePath($"/repos/{owner}/{repo}/git/trees/{treeData["tree"]["sha"]}"));

            var tree = JObject.Parse(client.Get().Content.ReadAsStringAsync().Result);
            
            Console.WriteLine(tree);

            return editor;
        }
    }

    public static class HttpHelper
    {
        public static HttpConnection Connect(string url)
        {
            return new HttpConnection(url);
        }
    }

    public class HttpConnection
    {
        private readonly string clientUrl;

        internal HttpConnection(string url)
        {
            clientUrl = url;
        }

        public async Task<JObject> RequestAsync(string request)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(clientUrl + request);
            
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException)
            {
                return null;
            }
            
            return JObject.Parse(await response.Content.ReadAsStringAsync());
        }
    }
}