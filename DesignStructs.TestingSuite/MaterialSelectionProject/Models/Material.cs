using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using ReInvented.Sections.Domain.Enums;
using ReInvented.Sections.Domain.Models;
using ReInvented.Sections.Domain.Repositories;
using ReInvented.Shared.Models;
using ReInvented.Shared.Services;
using ReInvented.Shared.Stores;

namespace MaterialSelectionProject.Models
{
    /// <summary>
    /// Main class that captures all the information related to a section profile and material data.
    /// </summary>
    public class Material : ErrorsEnabledPropertyStore
    {

        #region Default Constructor

        public Material()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        [JsonProperty(Order = 0)]
        public string SelectedCountry
        {
            get => Get<string>();
            set
            {
                Set(value);
                RaisePropertyChanged(nameof(ShapeGroups));
                SelectedShapeGroup = ShapeGroups.FirstOrDefault();
            }
        }

        [JsonProperty(Order = 1)]
        public string ShapeGroup
        {
            get => SelectedShapeGroup.ToString();
            set
            {
                Set(value);
                SelectedShapeGroup = (MaterialShapeGroup)Enum.Parse(typeof(MaterialShapeGroup), value);
            }
        }

        //[JsonConverter(typeof(MaterialTableConverter))]
        [JsonProperty(Order = 2)]
        public MaterialTable SelectedMaterialTable
        {
            get => Get<MaterialTable>();
            set
            {
                Set(value);
                SelectedMaterialGrade = SelectedMaterialTable?.Grades?.FirstOrDefault();
            }
        }

        [JsonProperty(Order = 3)]
        public string MaterialGrade
        {
            get => SelectedMaterialGrade?.Designation;
            set
            {
                Set(value);
                SelectedMaterialGrade = SelectedMaterialTable.Grades.FirstOrDefault(g => g.Designation == value);
            }
        }

        [JsonIgnore]
        public MaterialGrade SelectedMaterialGrade
        {
            get => Get<MaterialGrade>();
            set
            {
                Set(value);
                RaiseMultiplePropertiesChanged(nameof(MaterialProperties), nameof(MaterialIsSelected));
            }
        }

        [JsonIgnore]
        public MaterialShapeGroup SelectedShapeGroup
        {
            get => Get<MaterialShapeGroup>();
            set
            {
                Set(value);
                FilterTables();
            }
        }

        #endregion

        #region Public Read-only Properties

        [JsonIgnore]
        public MaterialsLibrary MaterialsLibrary
        {
            get => Get<MaterialsLibrary>();
            private set
            {
                Set(value);
                SelectedMaterialTable = value.Tables.FirstOrDefault();
            }
        }

        [JsonIgnore]
        public IEnumerable<MaterialTable> FilteredTables { get => Get<IEnumerable<MaterialTable>>(); private set => Set(value); }

        [JsonIgnore]
        public IEnumerable<string> Countries => MaterialsLibrary.Tables.Select(t => t.Country).ToHashSet();

        [JsonIgnore]
        public IEnumerable<MaterialShapeGroup> ShapeGroups => MaterialsLibrary.Tables.Where(t => t.Country == SelectedCountry).Select(t => t.Group).ToHashSet();

        [JsonIgnore]
        public IEnumerable<PropertyData> MaterialProperties => SelectedMaterialGrade != null ? PropertyListService.GetList(SelectedMaterialGrade) : null;

        [JsonIgnore]
        public bool MaterialIsSelected => SelectedMaterialGrade != null;

        #endregion

        #region Private Helper Methods

        private void Initialize()
        {
            MaterialsLibrary = MaterialsRepository.Instance.GetMaterialsLibrary();
            SelectedCountry = Countries.FirstOrDefault();
            SelectedShapeGroup = ShapeGroups.FirstOrDefault();
        }

        private void FilterTables()
        {
            FilteredTables = MaterialsLibrary.Tables.Where(t => t.Country == SelectedCountry && t.Group == SelectedShapeGroup);

            if (FilteredTables?.Count() == 0)
            {
                FilteredTables = MaterialsLibrary.Tables.Distinct().Where(t => t.Group == SelectedShapeGroup);
            }


            UpdateMaterialSelections();
        }

        /// (TODO): These methods are introduced newly. Without these methods, the application was working fine except the material selection
        ///       comboboxes were not getting auto populating. In case of an issue, delete these methods and uncomment UpdateMaterialSelections()
        ///       method in the above method.
        private void UpdateMaterialSelections()
        {
            SelectedMaterialTable = FilteredTables?.FirstOrDefault();
            OnSelectedMaterialTableChanged();
        }

        private void OnSelectedMaterialTableChanged()
        {

        }

        #endregion
    }
}
