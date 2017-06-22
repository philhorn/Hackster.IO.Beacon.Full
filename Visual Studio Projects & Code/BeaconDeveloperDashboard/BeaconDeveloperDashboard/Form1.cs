using BeaconDeveloperDashboard.DataModels;
using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeaconDeveloperDashboard
{
    public partial class Form1 : Form
    {
        static string connectionString = "{REMOVED}"; //IOT HUB OWNER CONNECTION STRING
        static ServiceClient serviceClient;
        static RegistryManager registryManager;

        

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_LoadAsync(object sender, EventArgs e)
        {
            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
            IEnumerable<Device> x = await registryManager.GetDevicesAsync(100);
            lstDevices.DataSource = x;
            lstDevices.DisplayMember = "Id";
            lstDevices.ValueMember = "Id";
            lstDisplays.DataSource = x;
            lstDisplays.DisplayMember = "Id";
            lstDisplays.ValueMember = "Id";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InvokeRemoteCommand("amber_alert", lstDevices.SelectedValue.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InvokeRemoteCommand("weather_alert", lstDevices.SelectedValue.ToString());
        }

        private async void InvokeRemoteCommand(string command, string device)
        {
            txtLogbox.AppendText(string.Format("Sending command: {0} to {1}", command, device) + Environment.NewLine);
            var remoteCommand = new CloudToDeviceMethod(command) { ResponseTimeout = TimeSpan.FromSeconds(30) };
            CloudToDeviceMethodResult response = new CloudToDeviceMethodResult();
            try
            {
                response = await serviceClient.InvokeDeviceMethodAsync(lstDevices.SelectedValue.ToString(), remoteCommand);
            }
            catch (Exception ex)
            {

                txtLogbox.AppendText(ex.Message + Environment.NewLine);
            }
            
            txtLogbox.AppendText(response.GetPayloadAsJson() + Environment.NewLine);
        }
        private async void SendFakeAmberAlert(string device, AmberAlertMSG payload)
        {
            string command = "amber_alert";
            string output = JsonConvert.SerializeObject(payload);
            txtLogbox.AppendText("This will attempt to activate LightBeacons via the DisplayBeacon as a proxy." + Environment.NewLine);
            
            var remoteCommand = new CloudToDeviceMethod(command) { ResponseTimeout = TimeSpan.FromSeconds(30) };
            remoteCommand.SetPayloadJson(output);
            CloudToDeviceMethodResult response = new CloudToDeviceMethodResult();
            try
            {
                response = await serviceClient.InvokeDeviceMethodAsync(lstDevices.SelectedValue.ToString(), remoteCommand);
            }
            catch (Exception ex)
            {

                txtLogbox.AppendText(ex.Message + Environment.NewLine);
            }

            txtLogbox.AppendText(response.GetPayloadAsJson() + Environment.NewLine);
        }

        private async void SendFakeWeatherAlert(string device, string payload)
        {
            string command = "weather_alert";
            string output = JsonConvert.SerializeObject(payload);
            txtLogbox.AppendText("This will attempt to activate LightBeacons via the DisplayBeacon as a proxy." + Environment.NewLine);

            var remoteCommand = new CloudToDeviceMethod(command) { ResponseTimeout = TimeSpan.FromSeconds(30) };
            remoteCommand.SetPayloadJson(output);
            CloudToDeviceMethodResult response = new CloudToDeviceMethodResult();
            try
            {
                response = await serviceClient.InvokeDeviceMethodAsync(lstDevices.SelectedValue.ToString(), remoteCommand);
            }
            catch (Exception ex)
            {

                txtLogbox.AppendText(ex.Message + Environment.NewLine);
            }

            txtLogbox.AppendText(response.GetPayloadAsJson() + Environment.NewLine);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            InvokeRemoteCommand("all_clear", lstDevices.SelectedValue.ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            InvokeRemoteCommand("new_tail_color", lstDevices.SelectedValue.ToString());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AmberAlertMSG testPayload = new AmberAlertMSG();
            SendFakeAmberAlert(lstDevices.SelectedValue.ToString(), testPayload);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Payload is just a zip code for the weather check. 
            
            SendFakeWeatherAlert(lstDevices.SelectedValue.ToString(), txtZipCode.Text);
        }
    }
}
