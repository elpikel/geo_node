using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TagLife.Models.Api;

namespace TagLife.Services
{
    public class ApiService
    {
        private static HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Place>> GetPlaces()
        {
            var places = await new HttpClient().GetAsync(@"https://young-thicket-35712.herokuapp.com/places");

            // todo: one day change it to throw correct exception
            if (!places.IsSuccessStatusCode)
            {
                throw new HttpRequestException();
            }

            var readAsStringAsync = await places.Content.ReadAsStringAsync();

            // todo: serialization exception
            return JsonConvert.DeserializeObject<List<Place>>(readAsStringAsync);
        }

        public async Task<List<Note>> GetNotes()
        {
            var notes = await _httpClient.GetAsync(@"https://young-thicket-35712.herokuapp.com/notes");

            // todo: one day change it to throw correct exception
            if (!notes.IsSuccessStatusCode)
            {
                throw new HttpRequestException();
            }

            var readAsStringAsync = await notes.Content.ReadAsStringAsync();

            // todo: serialization exception
            return JsonConvert.DeserializeObject<List<Note>>(readAsStringAsync);
        }

        public async Task SendNote(InputNote inputNote)
        {
            dynamic container = new
            {
                note = inputNote
            };

            var serializedPlace = JsonConvert.SerializeObject(container);

            var httpResponseMessage = await
                _httpClient.PostAsync("https://young-thicket-35712.herokuapp.com/notes",
                    new StringContent(serializedPlace,Encoding.UTF8,"application/json"));

            // todo: remember to catch no internet exceptions!
            if (httpResponseMessage.StatusCode != HttpStatusCode.Created)
            {
                throw new NotImplementedException(httpResponseMessage.StatusCode.ToString());
            }
        }
    }
}