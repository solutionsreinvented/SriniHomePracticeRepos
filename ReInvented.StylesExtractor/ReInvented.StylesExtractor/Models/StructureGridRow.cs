using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReInvented.StylesExtractor.Models
{
    internal sealed class StructureGridRow
    {
        public int SlNo { get; set; }

        public string DbName { get; set; }

        public string ProfileShape { get; set; }

        public string ProfileType { get; set; }

        public string ProfileName { get; set; }

        public StructureGridRow()
        {

        }

        public StructureGridRow(int slNo, string dbName, string profileShape, string profileType, string profileName)
        {
            SlNo = slNo;
            DbName = dbName;
            ProfileShape = profileShape;
            ProfileType = profileType;
            ProfileName = profileName;
        }

    }


}
