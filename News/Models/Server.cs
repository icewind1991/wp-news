using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;

namespace News.Models
{
    public class Server
    {
        public HttpClient Client;
        public Folder[] Folders { get; set; }

        public Server(string baseUrl, string user, string password)
        {
            var credentials = new NetworkCredential(user, password);
            var handler = new HttpClientHandler { Credentials = credentials };
            Client = new HttpClient(handler);
            Debug.WriteLine(baseUrl + "/index.php?/apps/news/api/v1-2/");
            Client.BaseAddress = new Uri(baseUrl + "/index.php/apps/news/api/v1-2/");
        }

        async public Task<Folder[]> getFoldersAsync()
        {
            HttpResponseMessage response = await Client.GetAsync("folders?format=json");
            string data = await response.Content.ReadAsStringAsync();
            OCSFolderData result = await JsonConvert.DeserializeObjectAsync<OCSFolderData>(data);
            return result.folders;
        }

        async public Task<Item[]> getFolderItemsAsync(Folder folder, int offset = 0)
        {
            return await getItemsAsync(1, folder.id, offset);
        }

        async public Task<Item[]> getAllItemsAsync(int offset = 0)
        {
            return await getItemsAsync(3, 0, offset);
        }

        async public Task<Item[]> getItemsAsync(int type, int id, int offset = 0)
        {
            HttpResponseMessage response = await Client.GetAsync("items?format=json&batchSize=20&offset=" + offset + "&getRead=true&type=" + type + "&id=" + id);
            string data = await response.Content.ReadAsStringAsync();
            OCSItemData result = await JsonConvert.DeserializeObjectAsync<OCSItemData>(data);
            return result.items;
        }

        async public void markAsRead(int itemId)
        {
            System.Diagnostics.Debug.WriteLine("markRead");
            await Client.PutAsync("items/" + itemId + "/read", new StringContent(""));
        }

        async public void markAsUnRead(int itemId)
        {
            System.Diagnostics.Debug.WriteLine("markUnRead");
            await Client.PutAsync("items/" + itemId + "/unread", new StringContent(""));
        }


    }
}
