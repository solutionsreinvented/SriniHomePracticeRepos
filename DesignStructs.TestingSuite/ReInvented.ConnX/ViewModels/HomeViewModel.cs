using HelixToolkit.Wpf;
using ReInvented.ConnX.Extensions;
using ReInvented.Sections.Domain.Models;
using ReInvented.Sections.Domain.Repositories;
using ReInvented.Shared.Stores;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace ReInvented.ConnX.ViewModels
{
    public class HomeViewModel : ValidatablePropertyStore
    {
        public HomeViewModel()
        {
            Library = SectionsRepository.Instance.GetSectionsLibrary();
            ColumnDatabase = Library.Databases.FirstOrDefault();
            ColumnLength = 1.0;
        }

        public SectionsLibrary Library { get => Get<SectionsLibrary>(); private set => Set(value); }

        public Database ColumnDatabase { get => Get<Database>(); set { Set(value); ColumnShape = ColumnDatabase?.SectionShapes.FirstOrDefault(); } }

        public SectionShape ColumnShape { get => Get<SectionShape>(); set { Set(value); ColumnClassification = ColumnShape?.Classifications.FirstOrDefault(); } }

        public Classification ColumnClassification { get => Get<Classification>(); set { Set(value); Column = ColumnClassification?.Sections.FirstOrDefault() as RolledSectionHShape; } }

        public RolledSectionHShape Column
        {
            get => Get<RolledSectionHShape>();
            set
            {
                Set(value);
                if (Column != null)
                {
                    RenderColumn();
                }
            }
        }

        public Model3D ColumnModel { get => Get<Model3D>(); set => Set(value); }

        public double ColumnLength { get => Get<double>(); set { Set(value); RenderColumn(); } }

        private void RenderColumn()
        {
            if (Column != null && ColumnLength != 0.0)
            {
                Model3DGroup modelGroup = new Model3DGroup();
                MeshBuilder builder = new MeshBuilder();

                // Retrieve profile outline using the service
                List<Point> profile = Column.GetSectionProfile();


                Vector3D extrVector = new Vector3D(0, 0, ColumnLength);
                Point3D sP = new Point3D(0, 0, 0);
                Point3D eP = new Point3D(0, 0, ColumnLength);

                // Extrude the profile along the column's length
                //builder.AddExtrudedGeometry(profile, extrVector, sP, eP);
                //builder.AddBox(new Point3D(0, 0, 0), 100, 200, 300);
                builder.AddCone(sP, extrVector, 150, 250, 300, false, false, 28);
                builder.AddNode(new Point3D(0, 0, 0), extrVector, new Point(0, 0));
                // Apply material
                DiffuseMaterial material = new DiffuseMaterial(new SolidColorBrush(Colors.SteelBlue));
                GeometryModel3D geometry = new GeometryModel3D(builder.ToMesh(), material);

                modelGroup.Children.Add(geometry);


                ColumnModel = modelGroup;
            }
        }

    }
}
