using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Resources;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using BackgroundTasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BackgroundTasks
{
    public sealed class NudgeCycle
    {

        //        static int cycle = 1000 * 60 * 15;
        static int cycle = 6;
        static NudgeCycle nudgeCycle = new NudgeCycle();

        public int getCycleNum()
        {
            return cycle;
        }

        public async void loadCycle()
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile = await storageFolder.CreateFileAsync("json.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);

            StreamReader sr = new System.IO.StreamReader(new FileStream(sampleFile.Path, FileMode.Open));
            JsonTextReader reader = new JsonTextReader(sr);
            nudgeCycle = JsonConvert.DeserializeObject<NudgeCycle>(reader.ToString());

        }

        public static int getCycle()
        {

            Debug.WriteLine("Get Cycle " + cycle);

            return nudgeCycle.getCycleNum();
        }

        public static void setCycle(int i)
        {

            cycle = i;

            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(new FileStream(@"C:\\Users\\Sammy\\Documents\\json.txt", FileMode.Create)))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, nudgeCycle);
                // {"ExpiryDate":new Date(1230375600000),"Price":0}
            }


        }
    }


}
