using System.Collections.Generic;

using OpenSTAADUI;

using ReInvented.StaadPro.Interop.Enums;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.Interop.Interfaces;
using ReInvented.StaadPro.InteropCore.Models;

namespace ReInvented.StaadPro.InteropCore.Extensions
{
    public static class OSCoreDesignExtensions
    {
        #region Fluent Functions

        public static OSCoreDesign AssignDesignCommands(this OSCoreDesign design, int designBriefId, IEnumerable<IDesignCommand> designCommands)
        {
            design.ComObject.AssignDesignCommands(designBriefId, designCommands);
            return design;
        }

        public static OSCoreDesign AssignDesignParameters(this OSCoreDesign design, StaadDesignCode designCode, IEnumerable<IDesignParameter> designParameters)
        {
            design.ComObject.AssignDesignParameters((DesignCodes)designCode, designParameters);
            return design;
        }

        public static OSCoreDesign AssignDesignParameters(this OSCoreDesign design, int designBriefId, IEnumerable<IDesignParameter> designParameters)
        {
            design.ComObject.AssignDesignParameters(designBriefId, designParameters);
            return design;
        }

        public static OSCoreDesign AssignDesignParameter(OSCoreDesign design, int designBriefId, IDesignParameter designParameter)
        {
            OSDesignExtensions.AssignDesignParameter(design.ComObject, designBriefId, designParameter);
            return design;
        }

        public static OSCoreDesign AssignDesignCommand(OSCoreDesign design, int designBriefId, IDesignCommand designCommand)
        {
            OSDesignExtensions.AssignDesignCommand(design.ComObject, designBriefId, designCommand);
            return design;
        }

        #endregion
    }
}
