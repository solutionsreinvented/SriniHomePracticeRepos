using System.IO;
using System.Text;
using RakeMechanism.Geometry.Domain;

namespace RakeMechanism.Geometry.Export
{
    public static class StaadFileExporter
    {
        public static void ExportToStd(CentralStructureGenerator generator, string filePath)
        {
            var sb = new StringBuilder();

            // Header
            sb.AppendLine("STAAD SPACE");
            sb.AppendLine("START JOB INFORMATION");
            sb.AppendLine("JOB NAME Central Structure Auto Generated");
            sb.AppendLine("END JOB INFORMATION");
            sb.AppendLine("INPUT WIDTH 79");
            sb.AppendLine("UNIT METER KN"); // Assuming input is in meters

            // Joint Coordinates
            sb.AppendLine("JOINT COORDINATES");
            foreach (var node in generator.AllNodes)
            {
                sb.AppendLine($"{node.Id} {node.X:F4} {node.Y:F4} {node.Z:F4}");
            }

            // Member Incidences
            if (generator.AllBeams.Count > 0)
            {
                sb.AppendLine("MEMBER INCIDENCES");
                foreach (var beam in generator.AllBeams)
                {
                    sb.AppendLine($"{beam.Id} {beam.StartNode.Id} {beam.EndNode.Id}");
                }
            }

            // Element Incidences (Plates)
            if (generator.AllPlates.Count > 0)
            {
                sb.AppendLine("ELEMENT INCIDENCES");
                foreach (var plate in generator.AllPlates)
                {
                    // 4-noded quad plates vs 3-noded triangular
                    if (plate.D != null && plate.D != plate.A)
                    {
                        sb.AppendLine($"{plate.Id} {plate.A.Id} {plate.B.Id} {plate.C.Id} {plate.D.Id}");
                    }
                    else // 3-noded triangular
                    {
                        sb.AppendLine($"{plate.Id} {plate.A.Id} {plate.B.Id} {plate.C.Id}");
                    }
                }
            }

            // Finish
            sb.AppendLine("FINISH");

            File.WriteAllText(filePath, sb.ToString());
        }
    }
}
