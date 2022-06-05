using ReInvented.StylesExtractor.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReInvented.StylesExtractor.Services
{
    internal class FakeDataService
    {
        public static IList<StructureGridRow> GenerateRows()
        {
            return new List<StructureGridRow>()
            {
                new StructureGridRow(){ SlNo = 1, DbName = "Indian", ProfileShape = "I", ProfileType = "MB", ProfileName = "ISMB200"},
                new StructureGridRow(){ SlNo = 2, DbName = "Indian", ProfileShape = "I", ProfileType = "MB", ProfileName = "ISMB250"},
                new StructureGridRow(){ SlNo = 3, DbName = "Indian", ProfileShape = "I", ProfileType = "MB", ProfileName = "ISMB300"},
                new StructureGridRow(){ SlNo = 4, DbName = "Indian", ProfileShape = "I", ProfileType = "MB", ProfileName = "ISMB350"},
                new StructureGridRow(){ SlNo = 5, DbName = "Indian", ProfileShape = "I", ProfileType = "MB", ProfileName = "ISMB400"}
            };
        }
    }
}
