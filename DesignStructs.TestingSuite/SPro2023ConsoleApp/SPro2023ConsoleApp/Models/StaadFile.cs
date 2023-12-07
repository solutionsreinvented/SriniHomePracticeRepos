using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using OpenSTAADUI;

using ReInvented.Shared;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Extensions;

using SPro2023ConsoleApp.Enums;

namespace SPro2023ConsoleApp.Models
{
    public class StaadFile
    {
        #region Parameterized Constructor

        public StaadFile(string path, StructureType type = StructureType.Space)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                throw new ArgumentException($"The {nameof(path)} provided is invalid. It is either null or empty or doesn't exist. Please provide a valid file path");
            }

            Type = type;
            Path = path;

            Content = File.ReadAllLines(Path).ToList();
        }

        #endregion

        #region Private Properties

        private StructureType Type { get; set; }

        private string Path { get; set; }

        #endregion

        #region Public Properties

        public List<string> Content { get; set; }

        #endregion

        #region Public Functions

        public StaadFile AddCopyRight(List<string> copyrightContent)
        {
            return AddBeforeLineContaining(Type.GetDescription(), copyrightContent);
        }

        public StaadFile RemoveFilePath()
        {
            string structureTypeText = Type.GetDescription();

            Content[Content.FindIndex(l => l.Contains(structureTypeText))] = structureTypeText;

            return this;
        }

        public StaadFile SwitchBeamIncidence(List<int> beamsIds)
        {
            /// TODO: Implementation pending
            return this;
        }

        public StaadFile SwitchBeamIncidence(int beamId)
        {
            /// TODO: Implementation pending
            return this;
        }

        public StaadFile SwitchPlateIncidence(List<int> platesIds)
        {
            /// TODO: Implementation pending
            return this;
        }

        public StaadFile SwitchPlateIncidence(int plateId)
        {
            /// TODO: Implementation pending
            return this;
        }

        public StaadFile SwitchPlateOrientation(OSGeometryUI geometry, int plateId)
        {
            Plate plate = geometry.GetPlate(plateId);

            string currentIncidence = string.Empty;
            string switchedIncidence = string.Empty;

            if (plate.D == null)
            {
                currentIncidence = $"{plate.Id} {plate.A.Id} {plate.B.Id} {plate.C.Id};";
                switchedIncidence = $"{plate.Id} {plate.A.Id} {plate.C.Id} {plate.B.Id};";
            }
            else
            {
                currentIncidence = $"{plate.Id} {plate.A.Id} {plate.B.Id} {plate.C.Id} {plate.D.Id};";
                switchedIncidence = $"{plate.Id} {plate.A.Id} {plate.D.Id} {plate.C.Id} {plate.B.Id};";
            }

            _ = Content.FirstOrDefault(l => l.Contains(currentIncidence)).Replace(currentIncidence, switchedIncidence);

            return this;
        }

        public StaadFile FixTriangularPlatesIncidences(List<Plate> plates)
        {
            if (plates.All(p => p.A == p.D))
            {
                plates.ForEach(p => FixFourNodedTriangularPlate(p));
            }

            return this;
        }

        public StaadFile FixFourNodedTriangularPlate(Plate plate)
        {
            if (plate.A == plate.D)
            {
                string incidenceText = $"{plate.Id} {plate.A.Id} {plate.B.Id} {plate.C.Id} {plate.D};";
                string correctedText = $"{plate.Id} {plate.A.Id} {plate.B.Id} {plate.C.Id};";

                _ = Content.FirstOrDefault(l => l.Contains(incidenceText)).Replace(incidenceText, correctedText);
            }

            return this;
        }

        public StaadFile AddBeforeLineContaining(string searchText, List<string> lines)
        {
            int searchTextIndex = Content.FindIndex(l => l.Contains(searchText));

            if (searchTextIndex != -1)
            {
                Content.InsertRange(searchTextIndex, lines);
            }

            return this;
        }

        public StaadFile AddAfterLineContaining(string searchText, List<string> lines)
        {
            int searchTextIndex = Content.FindIndex(l => l.Contains(searchText));

            if (searchTextIndex != -1)
            {
                Content.InsertRange(searchTextIndex + 1, lines);
            }

            return this;
        }

        public StaadFile AddBeforeLineContaining(string searchText, string line)
        {
            return AddBeforeLineContaining(searchText, new List<string>() { line });

        }

        public StaadFile AddAfterLineContaining(string searchText, string line)
        {
            return AddAfterLineContaining(searchText, new List<string>() { line });
        }

        #endregion

    }
}
