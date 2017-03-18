using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NudgeToaster
{
    class API
    {
        
        public float Prob { get; set; }
        public String ModelsJson { get; set; }
        static HttpClient client = new HttpClient();

        public void RunAPI()
        {
            client.BaseAddress = new Uri("https://ml.googleapis.com");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Console.WriteLine(GetModelsAsync("v1/projects/nudge-161903/models"));

        }


        async Task<String> GetModelsAsync(string path)
        {
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                ModelsJson = await response.Content.ReadAsAsync<String>();
            }
            return ModelsJson;
        }





        async Task<float> GetPredictionAsync(string path)
        {

            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                Prob = await response.Content.ReadAsAsync<float>();
            }
            return Prob;
        }
    }
}
