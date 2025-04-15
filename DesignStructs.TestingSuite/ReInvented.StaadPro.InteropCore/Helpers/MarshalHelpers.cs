using System;
using System.Runtime.InteropServices;

namespace ReInvented.StaadPro.InteropCore.Helpers
{
    public class MarshalHelpers
    {
        #region DLL Imports

        [DllImport("ole32")]
        private static extern int CLSIDFromProgIDEx([MarshalAs(UnmanagedType.LPWStr)] string lpszProgID, out Guid lpclsid);

        [DllImport("oleaut32")]
        private static extern int GetActiveObject([MarshalAs(UnmanagedType.LPStruct)] Guid rclsid, IntPtr pvReserved, [MarshalAs(UnmanagedType.IUnknown)] out object ppunk);

        #endregion

        public static object GetActiveObject(string progId, bool throwOnError = false)
        {
            if (progId == null)
                throw new ArgumentNullException(nameof(progId));

            int hResult = CLSIDFromProgIDEx(progId, out var clsid);

            if (hResult < 0)
            {
                if (throwOnError)
                    Marshal.ThrowExceptionForHR(hResult);

                return null;
            }

            hResult = GetActiveObject(clsid, IntPtr.Zero, out var obj);

            if (hResult < 0)
            {
                if (throwOnError)
                    Marshal.ThrowExceptionForHR(hResult);

                return null;
            }

            return obj;
        }
    }
}
