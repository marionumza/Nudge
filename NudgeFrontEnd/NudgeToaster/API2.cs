using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Xaml.Controls;
using Google.Apis.Util.Store;

namespace NudgeToaster
{

    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Prediction.v1_6;
    class API2
    {


        public float Prob { get; set; }
        public String ModelsJson { get; set; }
        public String PredictJson { get; set; }

        private TextBox textBoxOutput;
        public API2(TextBox textBoxOutput)
        {
            this.textBoxOutput = textBoxOutput;
            textBoxOutput.Text = "";
        }


        /// <summary>
        /// Appends the given string to the on-screen log, and the debug console.
        /// </summary>
        /// <param name="output">string to be appended</param>
        public void output(string output)
        {
            textBoxOutput.Text = textBoxOutput.Text + output + Environment.NewLine;
            Debug.WriteLine(output);
        }


        public async Task TestAPI()
        {
            // Makes a call to the Userinfo endpoint, and prints the results.
            output("Making API Call to Model...");


            output("API Works! ");
        }

        public async Task TestPredict()
        {
            // Makes a call to the Userinfo endpoint, and prints the results.
            output("Making API Call to Model...");


            output("API Works! ");
        }

        public async Task authGoogleCloud()
        {
            UserCredential credential;
            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                //credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                //    GoogleClientSecrets.Load(stream).Secrets,
                //    new[] { PredictionService.Scope.Prediction },
                //    "user", CancellationToken.None);
            }

        }



        ///==================== API Methods


        public class Inputs
        {
            public float TimeSpan { get; set; }
            public float HashedForegroundApp { get; set; }
            public float ActiveMouseTime { get; set; }
            public float ActiveKeyboardTime { get; set; }

        }

    }
}
