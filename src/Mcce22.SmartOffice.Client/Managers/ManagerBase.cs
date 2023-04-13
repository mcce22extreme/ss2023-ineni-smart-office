using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Mcce22.SmartOffice.Client.Models;
using Newtonsoft.Json;

namespace Mcce22.SmartOffice.Client.Managers
{
    public abstract class ManagerBase<T> where T : IModel
    {
        protected static HttpClient HttpClient = new HttpClient();

        protected string BaseUrl { get; }

        public ManagerBase(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public async Task<T[]> GetList()
        {
            var json = await HttpClient.GetStringAsync(BaseUrl);

            var entries = JsonConvert.DeserializeObject<T[]>(json);

            return entries;
        }

        public async Task<T> Save(T model)
        {
            if (model.Id == 0)
            {
                var response = await HttpClient.PostAsJsonAsync(BaseUrl, model);

                await EnsureSuccessStatusCode(response);

                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                var response = await HttpClient.PutAsJsonAsync($"{BaseUrl}/{model.Id}", model);

                await EnsureSuccessStatusCode(response);

                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task Delete(int id)
        {
            await EnsureSuccessStatusCode(await HttpClient.DeleteAsync($"{BaseUrl}/{id}"));
        }

        private async Task EnsureSuccessStatusCode(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
