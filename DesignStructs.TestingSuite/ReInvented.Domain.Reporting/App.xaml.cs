using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

using ReInvented.DataAccess;
using ReInvented.DataAccess.Services;
using ReInvented.Domain.Reporting.Models;
using ReInvented.StaadPro.Interactivity.Models;

namespace ReInvented.Domain.Reporting
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ///string filePath = @"C:\Users\masanams\Desktop\Desktop\Code\foundation-load-data-content.js";

            ///JsonDataSerializer<FoundationLoadData> serializer = new JsonDataSerializer<FoundationLoadData>();


            ///File.WriteAllText(filePath, $"FoundationLoadData = {serializer.Serialize(foundationLoadData, JsonSerializerSettingsProvider.MinifiedSettings())}");

        }
    }
}