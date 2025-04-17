using System;

using ReInvented.StaadPro.Interop.Enums;
using ReInvented.StaadPro.Interop.Models;

namespace ReInvented.StaadPro.InteropCore.Models
{
    public class OpenStaadCoreWrapper
    {
        #region Parameterized Constructor

        public OpenStaadCoreWrapper(OpenStaadWrapper wrapper)
        {
            Initialize(wrapper);
        }

        #endregion

        #region Readonly Properties

        public OpenStaadWrapper OpenStaadWrapper { get; private set; }

        public bool IsDedicated { get; private set; }

        public OSCoreRoot Root { get; private set; }

        public OSCoreGeometry Geometry { get; private set; }

        public OSCoreLoad Load { get; private set; }

        public OSCoreOutput Output { get; private set; }

        public OSCoreProperty Property { get; private set; }

        public OSCoreSupport Support { get; private set; }

        public OSCoreDesign Design { get; private set; }

        public OSCoreCommands Commands { get; private set; }

        public OSCoreView View { get; private set; }

        public OSCoreTable Table { get; private set; }

        public ApplicationVersion StaadVersion { get; private set; }

        public ApplicationEdition StaadEdition { get; private set; }

        public IntPtr StaadApplicationWindowHandle { get; private set; }

        #endregion

        #region Public Functions

        public void Dispose()
        {
            Root = null;
            Commands = null;
            Design = null;
            Geometry = null;
            Load = null;
            Output = null;
            Property = null;
            Support = null;
            View = null;
            Table = null;

            OpenStaadWrapper.Dispose();
        }

        #endregion

        #region Private Helpers

        private void Initialize(OpenStaadWrapper wrapper)
        {
            OpenStaadWrapper = wrapper;
            IsDedicated = wrapper != null && wrapper.IsDedicated;

            Root = new OSCoreRoot(wrapper.OpenStaad);
            Geometry = new OSCoreGeometry(wrapper.Geometry);
            Load = new OSCoreLoad(wrapper.Load);
            Output = new OSCoreOutput(wrapper.Output);
            Property = new OSCoreProperty(wrapper.Property);
            Support = new OSCoreSupport(wrapper.Support);
            Design = new OSCoreDesign(wrapper.Design);
            Commands = new OSCoreCommands(wrapper.Commands);
            View = new OSCoreView(wrapper.View);
            Table = new OSCoreTable(wrapper.Table);

            StaadVersion = wrapper.StaadVersion;
            StaadEdition = wrapper.StaadEdition;
            StaadApplicationWindowHandle = wrapper.StaadApplicationWindowHandle;
        }

        #endregion
    }
}
