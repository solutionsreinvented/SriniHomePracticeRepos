using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using ReInvented.StaadPro.Interop.Entities;

namespace ReInvented.Shared.Geometry
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var node = new Node(1, 0, 0, 0);
            var nodes = new List<Node>();
            nodes.Add(node);
            nodes.Add(node);
        }
    }
}
