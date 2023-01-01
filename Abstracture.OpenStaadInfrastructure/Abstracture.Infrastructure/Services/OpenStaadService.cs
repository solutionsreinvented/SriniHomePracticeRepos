using Microsoft.VisualBasic;

using OpenSTAADUI;
using System.Runtime.InteropServices;

namespace Abstracture.Infrastructure.Services
{
    public static class OpenStaadService
    {
        public static OpenSTAAD Instance() => Interaction.GetObject(null, "StaadPro.OpenSTAAD") as OpenSTAAD;

        public static OpenSTAAD GetInstance() => Marshal.GetActiveObject("StaadPro.OpenSTAAD") as OpenSTAAD;
    }



}
