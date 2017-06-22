using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Azure.Devices;
using TweetSharp;

namespace BeaconWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        static ServiceClient serviceClient;
        static string connectionString = "{REMOVED}"; //IOT HUB OWNER CONNECTION STRING
        public override void Run()
        {
            Trace.TraceInformation("BeaconWorker is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }
        private static async Task CheckAmberAlerts()
        {
            //Check @AmberAlert Twitter for new alerts
            string consumerKey = "NuJqfBKYAOGrGrq9VQgUAboQe";

            string consumerSecret = "124ZakFjwjOzlhSrZfvyIwjwWCZ6HXSqo8bj9Gm3FBSyho63Iu";

            string accessToken = "877373799343169536-EGA7XYg94hNmkLFoYQki2qbL2Z5XPZM";

            string accessSecret = "tRjjrVF6VPPCi6wkD2fcy9GDiYAzmWrKzuOKOxM08BWVJ";

            // Obtain keys by registering your app on https://dev.twitter.com/apps or https://apps.twitter.com/

            
            var service = new TwitterService(consumerKey, consumerSecret);
            
            service.AuthenticateWith(accessToken, accessSecret);

            TwitterStatus result = service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions() { ScreenName = "AmberAlert", Count = 1 }).First();
            var sendAmberAlert= new CloudToDeviceMethod("amber_alert") { ResponseTimeout = TimeSpan.FromSeconds(30) };
            var clearAmberAlert = new CloudToDeviceMethod("clear_amber") { ResponseTimeout = TimeSpan.FromSeconds(30) };
            //Determine if this a cancelled alert or not
            //Twitter text can mess with Json - ignore reTweets
            CloudToDeviceMethodResult response = new CloudToDeviceMethodResult();
            if (result.TextDecoded.Contains("cancelled"))
            {
                //clearAmberAlert.SetPayloadJson("'{}'");
                response = await serviceClient.InvokeDeviceMethodAsync("BeaconDisplay", clearAmberAlert);
            }
            else if(result.RetweetedStatus.Author.ScreenName != "AmberAlert")
            {
                //Handle ReTweets here
            }
            else if (result.Author.ScreenName == "AmberAlert")
            {
                var jsonPayload = "'{\"title\":\"Amber Alert!\",\"details\":\"" + result.TextDecoded.ToString() + "\"}'";
                sendAmberAlert.SetPayloadJson(jsonPayload);
                response = await serviceClient.InvokeDeviceMethodAsync("BeaconDisplay", sendAmberAlert);
            }
            
            
            // SEND TO DISPLAY BEACONS
            
            
            Trace.TraceInformation("Response status: {0}, payload:", response.Status);
            Trace.TraceInformation(response.GetPayloadAsJson());
        }
        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("BeaconWorker has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("BeaconWorker is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("BeaconWorker has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Checking Latest Amber Alert Tweets from @AmberAlert");
                CheckAmberAlerts().Wait();
                await Task.Delay(50000);
            }
        }
    }
}
