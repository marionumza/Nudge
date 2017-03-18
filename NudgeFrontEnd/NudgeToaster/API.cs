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
        
        public float prob { get; set; }
        static HttpClient client = new HttpClient();

        public void RunAPI()
        {
            // New code:
            client.BaseAddress = new Uri("https://ml.googleapis.com/v1/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Console.WriteLine(GetProductAsync("{parent=nudge-161903/*}/models"));
        }

        async Task<float> GetProductAsync(string path)
        {
      
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                prob = await response.Content.ReadAsAsync<float>();
            }
            return prob;
        }
    }
}
