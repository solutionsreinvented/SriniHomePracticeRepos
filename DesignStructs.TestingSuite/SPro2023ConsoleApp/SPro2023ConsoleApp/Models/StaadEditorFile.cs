using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

using OpenSTAADUI;

using ReInvented.Shared;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Enums;
using ReInvented.StaadPro.Interactivity.Extensions;

using SPro2023ConsoleApp.Enums;

namespace SPro2023ConsoleApp.Models
{
    public enum StaadEditorCommands
    {
        [Description("JOINT COORDINATES")]
        JointCoordinates,
        [Description("MEMBER INCIDENCES")]
        MemberIncidences,
        [Description("ELEMENT INCIDENCES")]
        ElementIncidences,
    }

    public class StaadEditorSettings
    {
        public StaadEditorSettings()
        {

        }

        public int LineWidth { get; set; }

        public string IncidenceSeparator { get; set; } = ";";

        public StructureType StructureType { get; set; } = StructureType.Space;

        public static StaadEditorSettings GetDefaultSettings()
        {
            return new StaadEditorSettings()
            {
                LineWidth = 79,
                StructureType = StructureType.Space
            };
        }
    }

    public class StaadEditorFile
    {
        #region Parameterized Constructor

        public StaadEditorFile(string path, StaadEditorSettings editorSettings)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                throw new ArgumentException($"The {nameof(path)} provided is invalid. It is either null or empty or doesn't exist. Please provide a valid file path");
            }

            EditorSettings = editorSettings ?? throw new ArgumentException($"The {nameof(editorSettings)} provided is invalid.");

            Path = path;

            Content = File.ReadAllLines(Path).ToList();
        }

        #endregion

        #region Private Properties

        private StaadEditorSettings EditorSettings { get; set; }

        private string Path { get; set; }

        #endregion

        #region Public Properties

        public List<string> Content { get; set; }

        #endregion

        #region Public Functions

        public StaadEditorFile AddCopyRight(List<string> copyrightContent)
        {
            return AddBeforeLineContaining(EditorSettings.StructureType.GetDescription(), copyrightContent);
        }

        public StaadEditorFile RemoveFilePath()
        {
            string structureTypeText = EditorSettings.StructureType.GetDescription();

            Content[Content.FindIndex(l => l.Contains(structureTypeText))] = structureTypeText;

            return this;
        }

        public StaadEditorFile SwitchBeamIncidence(List<int> beamsIds)
        {
            /// TODO: Implementation pending
            return this;
        }

        public StaadEditorFile SwitchBeamIncidence(int beamId)
        {
            /// TODO: Implementation pending
            return this;
        }

        public StaadEditorFile SwitchPlateIncidence(List<int> platesIds)
        {
            /// TODO: Implementation pending
            return this;
        }

        public StaadEditorFile SwitchPlateIncidence(int plateId)
        {
            /// TODO: Implementation pending
            return this;
        }

        public StaadEditorFile FormatEntityIncidences(EntityType entityType)
        {
            (int StartIndex, int EndIndex) = GetEntityIncidencesRange(this, entityType);
            IEnumerable<string> incidenceContent = GetEntityIncidencesContent(this, entityType);

            List<string> formattedContent = FormatIncidenceText(string.Join(" ", incidenceContent), EditorSettings.LineWidth, EditorSettings.IncidenceSeparator);

            Content.RemoveRange(StartIndex + 1, EndIndex - StartIndex);
            Content.InsertRange(StartIndex + 1, formattedContent);

            return this;
        }

        public StaadEditorFile FixSyntaxForTriangularPlates()
        {
            (int StartIndex, int EndIndex) = GetEntityIncidencesRange(this, EntityType.Plates);
            IEnumerable<string> incidenceContent = GetEntityIncidencesContent(this, EntityType.Plates);
            List<string> rectifiedIncidenceContent = new List<string>();

            foreach (var incidenceLine in incidenceContent)
            {
                rectifiedIncidenceContent.Add(RectifyPlateIncidences(incidenceLine));
            }

            List<string> formattedContent = FormatIncidenceText(string.Join(" ", rectifiedIncidenceContent), EditorSettings.LineWidth, EditorSettings.IncidenceSeparator);

            Content.RemoveRange(StartIndex + 1, EndIndex - StartIndex);
            Content.InsertRange(StartIndex + 1, formattedContent);

            return this;
        }

        public StaadEditorFile FixTriangularPlatesIncidences(List<Plate> plates)
        {
            if (plates.All(p => p.A == p.D))
            {
                plates.ForEach(p => FixFourNodedTriangularPlate(p));
            }

            return this;
        }

        public StaadEditorFile SwitchPlateOrientation(OSGeometryUI geometry, int plateId)
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

        public StaadEditorFile FixFourNodedTriangularPlate(Plate plate)
        {
            if (plate.A == plate.D)
            {
                string incidenceText = $"{plate.Id} {plate.A.Id} {plate.B.Id} {plate.C.Id} {plate.D.Id};";
                string correctedText = $"{plate.Id} {plate.A.Id} {plate.B.Id} {plate.C.Id};";

                _ = Content.FirstOrDefault(l => l.Contains(incidenceText)).Replace(incidenceText, correctedText);
            }

            return this;
        }

        public StaadEditorFile AddBeforeLineContaining(string searchText, List<string> lines)
        {
            int searchTextIndex = Content.FindIndex(l => l.Contains(searchText));

            if (searchTextIndex != -1)
            {
                Content.InsertRange(searchTextIndex, lines);
            }

            return this;
        }

        public StaadEditorFile AddAfterLineContaining(string searchText, List<string> lines)
        {
            int searchTextIndex = Content.FindIndex(l => l.Contains(searchText));

            if (searchTextIndex != -1)
            {
                Content.InsertRange(searchTextIndex + 1, lines);
            }

            return this;
        }

        public StaadEditorFile AddBeforeLineContaining(string searchText, string line)
        {
            return AddBeforeLineContaining(searchText, new List<string>() { line });

        }

        public StaadEditorFile AddAfterLineContaining(string searchText, string line)
        {
            return AddAfterLineContaining(searchText, new List<string>() { line });
        }

        #endregion

        #region Static Functions

        public static List<string> FormatIncidenceText(string incidenceText, int lineMaxLength, string separator = ";")
        {
            List<string> result = new List<string>();

            while (incidenceText.Length > 0)
            {
                string lineOfMaxLength = incidenceText.Length >= lineMaxLength ? incidenceText.Substring(0, lineMaxLength) : incidenceText;

                int splitIndex = lineOfMaxLength.LastIndexOf(separator);
                string line = lineOfMaxLength.Substring(0, splitIndex + 1);

                result.Add(line.Trim());

                incidenceText = incidenceText.Substring(splitIndex + 1);
            }

            return result;
        }

        public static (int StartIndex, int EndIndex) GetEntityIncidencesRange(StaadEditorFile editorFile, EntityType entityType)
        {
            var entitySearchText = string.Empty;

            if (entityType == EntityType.Nodes)
            {
                entitySearchText = StaadEditorCommands.JointCoordinates.GetDescription();
            }
            else if (entityType == EntityType.Beams)
            {
                entitySearchText = StaadEditorCommands.MemberIncidences.GetDescription();
            }
            else
            {
                entitySearchText = StaadEditorCommands.ElementIncidences.GetDescription();
            }

            int startIndex = editorFile.Content.FindIndex(l => l.Contains(entitySearchText));
            int endIndex = startIndex + editorFile.Content.Skip(startIndex + 1).ToList().FindIndex(l => !l.Contains(";"));

            return (startIndex, endIndex);
        }

        public static List<string> GetEntityIncidencesContent(StaadEditorFile editorFile, EntityType entityType)
        {
            (int StartIndex, int EndIndex) = GetEntityIncidencesRange(editorFile, entityType);
            IEnumerable<string> incidenceContent = editorFile.Content.Skip(StartIndex + 1).Take(EndIndex - StartIndex);

            return incidenceContent.ToList();
        }


        public static string RectifyPlateIncidences(string incidenceLine)
        {
            string[] incidences = incidenceLine.Split(';');

            List<string> rectifiedIncidences = new List<string>();

            foreach (var incidence in incidences)
            {
                string[] incidenceArray = incidence.Trim().Split(' ');

                if (incidenceArray.Count() > 0 && incidenceArray[0] != string.Empty)
                {
                    string currentPlateIncidence;

                    if (incidenceArray.Length > 4 && incidenceArray[1].Trim() != incidenceArray[4].Trim())
                    {
                        currentPlateIncidence = $"{incidenceArray[0]} {incidenceArray[1]} {incidenceArray[2]} {incidenceArray[3]} {incidenceArray[4]};";
                    }
                    else
                    {
                        currentPlateIncidence = $"{incidenceArray[0]} {incidenceArray[1]} {incidenceArray[2]} {incidenceArray[3]};";
                    }

                    rectifiedIncidences.Add(currentPlateIncidence);
                }
            }

            return string.Join(" ", rectifiedIncidences);
        }


        #endregion
    }
}
