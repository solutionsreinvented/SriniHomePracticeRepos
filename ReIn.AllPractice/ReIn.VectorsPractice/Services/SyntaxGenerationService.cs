using System;
using System.Collections.Generic;

using ReIn.VectorsPractice.Interfaces;

namespace ReIn.VectorsPractice.Services
{
    public static class SyntaxGenerationService
    {
        public static string NodeIncidencesFrom(List<IReferenceGrid> grids, int lastUsedNodeNumber = 1)
        {
            string syntax = $"JOINT COORDINATES{Environment.NewLine}";

            int nodeId = lastUsedNodeNumber;

            for (int i = 0; i < grids.Count; i++)
            {
                grids[i].A.Id = nodeId++;
                grids[i].B.Id = nodeId++;
                grids[i].C.Id = nodeId++;
                grids[i].D.Id = nodeId++;

                syntax += $"{grids[i].A.Id} {grids[i].A.X} {grids[i].A.Y} {grids[i].A.Z};{Environment.NewLine}";
                syntax += $"{grids[i].B.Id} {grids[i].B.X} {grids[i].B.Y} {grids[i].B.Z};{Environment.NewLine}";
                syntax += $"{grids[i].C.Id} {grids[i].C.X} {grids[i].C.Y} {grids[i].C.Z};{Environment.NewLine}";
                syntax += $"{grids[i].D.Id} {grids[i].D.X} {grids[i].D.Y} {grids[i].D.Z};{Environment.NewLine}";
            }

            return syntax;
        }

        public static string MemberIncidencesFrom(List<IReferenceGrid> grids, int lastUsedMemberNumber = 1)
        {
            string syntax = $"MEMBER INCIDENCES{Environment.NewLine}";

            int memberId = lastUsedMemberNumber;


            for (int i = 0; i < grids.Count; i++)
            {
                syntax += $"{memberId++} {grids[i].A.Id} {grids[i].B.Id};{Environment.NewLine}";
                syntax += $"{memberId++} {grids[i].C.Id} {grids[i].D.Id};{Environment.NewLine}";
                syntax += $"{memberId++} {grids[i].A.Id} {grids[i].C.Id};{Environment.NewLine}";
                syntax += $"{memberId++} {grids[i].B.Id} {grids[i].D.Id};{Environment.NewLine}";
                if (i >= 1)
                {
                    syntax += $"{memberId++} {grids[i - 1].A.Id} {grids[i].A.Id};{Environment.NewLine}";
                    syntax += $"{memberId++} {grids[i - 1].B.Id} {grids[i].B.Id};{Environment.NewLine}";
                    syntax += $"{memberId++} {grids[i - 1].C.Id} {grids[i].C.Id};{Environment.NewLine}";
                    syntax += $"{memberId++} {grids[i - 1].D.Id} {grids[i].D.Id};{Environment.NewLine}";
                }
            }

            return syntax;
        }

    }
}
