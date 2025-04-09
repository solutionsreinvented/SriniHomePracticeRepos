using HelixToolkit.Wpf;

using ReInvented.ConnX.Extensions;
using ReInvented.Sections.Domain.Models;
using ReInvented.Sections.Domain.Repositories;
using ReInvented.Shared.Stores;

using System;
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
            ColumnLength = 1000.0;
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

                // Create the 3D mesh by extrusion
                MeshGeometry3D mesh = ExtrudeProfile(profile.Take(profile.Count).ToList(), ColumnLength);

                DiffuseMaterial material = new DiffuseMaterial(new SolidColorBrush(Colors.SteelBlue));

                GeometryModel3D geometry = new GeometryModel3D(mesh, material) { BackMaterial = material };

                modelGroup.Children.Add(geometry);

                ColumnModel = modelGroup;
            }
        }

        private MeshGeometry3D ExtrudeProfile(List<Point> profile, double depth)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            int n = profile.Count;

            // Top and bottom polygons
            for (int i = 0; i < n; i++)
            {
                Point pt = profile[i];
                mesh.Positions.Add(new Point3D(pt.X, pt.Y, 0));       // bottom
                mesh.Positions.Add(new Point3D(pt.X, pt.Y, depth));   // top
            }

            //// Add side quads
            //for (int i = 0; i < n; i++)
            //{
            //    int i1 = i * 2;
            //    int i2 = (i + 1) % n * 2;
                
            //    mesh.TriangleIndices.Add(i1);
            //    mesh.TriangleIndices.Add(i2);
            //    mesh.TriangleIndices.Add(i2 + 1);

            //    mesh.TriangleIndices.Add(i1);
            //    mesh.TriangleIndices.Add(i2 + 1);
            //    mesh.TriangleIndices.Add(i1 + 1);
            //}

            //// Cap bottom face
            //for (int i = 0; i < n; i++)
            //{
            //    mesh.TriangleIndices.Add(0);
            //    mesh.TriangleIndices.Add(i * 2);
            //    mesh.TriangleIndices.Add((i + 1) * 2);
            //}



            //// Cap top face
            //for (int i = 1; i < n - 1; i++)
            //{
            //    mesh.TriangleIndices.Add(1);
            //    mesh.TriangleIndices.Add((i + 1) * 2 + 1);
            //    mesh.TriangleIndices.Add(i * 2 + 1);
            //}

            return mesh;
        }
    }
}
