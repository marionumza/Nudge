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
        BackgroundTaskDeferral _deferral;
        public NudgeCycle()
        {
            hash = Guid.NewGuid().GetHashCode();
        }


        public override int GetHashCode()
        {
            return hash;
        }

        static int defaultCycle = 2 * 60;
        static NudgeStore nudgeStore = new NudgeStore();
        static NudgeCycle nudgeCycle = new NudgeCycle();
        private int hash;
        private IBackgroundTaskInstance taskInstance;
        private int loadCount;

        public static NudgeCycle getCycleObj()
        {
            return nudgeCycle;
        }

        // We have to write to a file because when we want to get the Cycle from the MainPage 
        // the NudgeCycle object is diffrent from that of the one called within the BackgroundTasks where we set the cycle.
        // Therefore, to get the current cycle, we load from the json file, and to set the cycle we write to the json file.
        public async void loadCycle()
        {
            if (loadCount > 0)
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

                // START: async
                if (taskInstance != null)
                    _deferral = taskInstance.GetDeferral();

                StorageFile jsonFile = await storageFolder.CreateFileAsync("json.txt", CreationCollisionOption.OpenIfExists);
                String jsonString = await FileIO.ReadTextAsync(jsonFile);

                // END: async
                if (_deferral != null)
                    _deferral.Complete();

                Debug.WriteLine("Read from: " + storageFolder.Path);
                if (jsonString.Length > 0) // Loading file
                {
                    Debug.WriteLine("Read JSON " + jsonString);
                    nudgeStore = (NudgeStore)JsonConvert.DeserializeObject<NudgeStore>(jsonString);
                    if (getCycle() <= 0) setCycle(defaultCycle);
                }
                loadCount++;
                return;
            }
            loadCount++;
            setCycle(defaultCycle);
        }

        // We need to loadCycle() before we can use this to get the current Cycle
        public int getCycle()
        {
            Debug.WriteLine("Get Cycle " + nudgeStore.Cycle + " @ " + NudgeCycle.getCycleObj().GetHashCode());

            return nudgeStore.Cycle;
        }

        // Here we write the current cycle
        public async void setCycle(int i)
        {
            nudgeStore.Cycle = i;

            // START: async
            if (taskInstance != null)
                _deferral = taskInstance.GetDeferral();

            // Write cycle to json
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile jsonFile = await storageFolder.CreateFileAsync("json.txt", Windows.Storage.CreationCollisionOption.OpenIfExists);

            String jsonString = JsonConvert.SerializeObject(nudgeStore);
            Debug.WriteLine("Write JSON: " + jsonString);

            await FileIO.WriteTextAsync(jsonFile, jsonString);

            // END: async
            if (_deferral != null)
                _deferral.Complete();

            Debug.WriteLine("Done writing");

        }

        public void setTaskInstance(IBackgroundTaskInstance taskInstance)
        {
            this.taskInstance = taskInstance;
        }
    }


}
