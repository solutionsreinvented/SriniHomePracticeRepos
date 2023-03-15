using OpenSTAADUI;

using ReInvented.StaadPro.InteractivityVB.Services;

namespace Practice.DesignParameters
{
    public static class DesignParametersProcessor
    {
        public static void AssignParameters()
        {
            OpenStaadProvider.Instantiate();
            OpenSTAAD openStaad = OpenStaadProvider.OpenStaad;

            IOSCommandsUI commands = OpenStaadProvider.OSCommands;

            object code = 1001;
            object intValues = new int[1] { 2 };
            object stringValues = new string[1] { "4.0" };
            object assignList = new int[1] { 16177 };

            object commandNo = 9240;
            object floatValues = new double[2] { 0.85, 0.6 };
            commands.CreateSteelDesignCommand(code, commandNo, intValues, floatValues, stringValues, assignList);

            commandNo = 9250;
            floatValues = new double[1] { 0.50 };
            commands.CreateSteelDesignCommand(code, commandNo, intValues, floatValues, stringValues, assignList);

            openStaad.SaveModel(true);
        }
    }


}
