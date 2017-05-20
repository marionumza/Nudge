using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using BackgroundTasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BackgroundTasks
{
    public sealed class NudgeCycle
    {

        static int defaultCycle = 6;
        static NudgeStore nudgeStore = new NudgeStore();
        static NudgeCycle nudgeCycle = new NudgeCycle();

        public static NudgeCycle getCycleObj()
        {
            return nudgeCycle;
        }

        public async void loadCycle()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile jsonFile = await storageFolder.CreateFileAsync("json.txt", Windows.Storage.CreationCollisionOption.OpenIfExists);
            String jsonString = await FileIO.ReadTextAsync(jsonFile);

            if (jsonString.Length > 0) // Loading file
            {
                Debug.WriteLine("File Text " + jsonString);
                nudgeStore = (NudgeStore)JsonConvert.DeserializeObject<NudgeStore>(jsonString);
            }
            else // Creating file for first time
            {
                
                setCycle(defaultCycle);
            }
        }

        public int getCycle()
        {
            Debug.WriteLine("Get Cycle " + nudgeStore.Cycle);

            return nudgeStore.Cycle;
        }

        public async void setCycle(int i)
        {
            nudgeStore.Cycle = i;

            // Create sample file; replace if exists.
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile jsonFile = await storageFolder.CreateFileAsync("json.txt", Windows.Storage.CreationCollisionOption.OpenIfExists);
            String jsonString = JsonConvert.SerializeObject(nudgeStore);
            Debug.WriteLine("File Write " + jsonString);

            await FileIO.WriteTextAsync(jsonFile, jsonString);
        }
    }


}
