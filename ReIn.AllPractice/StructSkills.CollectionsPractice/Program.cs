using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StructSkills.CollectionsPractice
{
    public enum GridType
    {
        PortalGrid,
        BridgeGrid,
        ExtensionGrid,
        ExtensionInterfaceGrid
    }

    public interface IFrameGrid
    {
        int Id { get; set; }

        string Title { get; set; }

        double Width { get; set; }

        double Height { get; set; }

        double Spacing { get; set; }

        double Distance { get; set; }
    }

    public interface IFrameGridDefinition
    {
        int Id { get; set; }

        GridType Type { get; set; }

        IFrameGrid FrameGrid { get; set; }
    }

    public class FrameGrid : IFrameGrid
    {
        private double _spacing;

        public event Action DependentPropertyChanged;

        public int Id { get; set; }

        public string Title { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public double Spacing { get => _spacing; set { _spacing = value; RaiseDependentPropertyChanged(); } }

        public double Distance { get; set; }

        public IFrameGrid PreviousFrameGrid { get; private set; }

        private void RaiseDependentPropertyChanged()
        {
            DependentPropertyChanged?.Invoke();
        }

    }

    public class PortalFrameGrid : FrameGrid, IFrameGrid
    {
        public PortalFrameGrid()
        {
            Title = "Portal Frame Grid";
        }
    }

    public class BridgeFrameGrid : FrameGrid, IFrameGrid
    {
        public BridgeFrameGrid()
        {
            Title = "Bridge Frame Grid";
        }
    }

    public class ExtensionFrameGrid : FrameGrid, IFrameGrid
    {
        public ExtensionFrameGrid()
        {
            Title = "Extension Frame Grid";
        }
    }

    public class ExtensionInterfaceFrameGrid : FrameGrid, IFrameGrid
    {
        public ExtensionInterfaceFrameGrid()
        {
            Title = "Extension Interface Frame Grid";
        }
    }


    public class FrameGridDefinition : IFrameGridDefinition
    {
        private GridType type;


        public FrameGridDefinition(IFrameGridDefinition previousFrameGridDefinition = null)
            : this(previousFrameGridDefinition, GridType.BridgeGrid)
        {

        }


        public FrameGridDefinition(IFrameGridDefinition previousFrameGridDefinition, GridType gridType)
        {
            PreviousFrameGridDefinition = previousFrameGridDefinition;
            Type = gridType;
        }

        public int Id { get; set; }

        public GridType Type { get => type; set { type = value; UpdateFrameGrid(); } }

        public IFrameGridDefinition PreviousFrameGridDefinition { get; set; }

        public IFrameGrid FrameGrid { get; set; }

        #region Private Helpers

        private void UpdateFrameGrid()
        {
            IFrameGrid newFrameGrid;


            //switch (type)
            //{
            //    case GridType.PortalGrid:
            //        newFrameGrid = new PortalFrameGrid();
            //        break;
            //    case GridType.BridgeGrid:
            //        newFrameGrid = new BridgeFrameGrid();
            //        break;
            //    case GridType.ExtensionGrid:
            //        newFrameGrid = new ExtensionFrameGrid();
            //        break;
            //    case GridType.ExtensionInterfaceGrid:
            //        newFrameGrid = new ExtensionInterfaceFrameGrid();
            //        break;
            //    default:
            //        newFrameGrid = new PortalFrameGrid();
            //        break;
            //}

            //if (FrameGrid != null)
            //{
            //    var currentFrameGrid = FrameGrid;
            //    newFrameGrid.Id = currentFrameGrid.Id;
            //}

            //FrameGrid = newFrameGrid;

            switch (type)
            {
                case GridType.PortalGrid:
                    newFrameGrid = FrameGridMapper.Map<PortalFrameGrid>(FrameGrid);
                    break;
                case GridType.BridgeGrid:
                    newFrameGrid = FrameGridMapper.Map<BridgeFrameGrid>(FrameGrid);
                    break;
                case GridType.ExtensionGrid:
                    newFrameGrid = FrameGridMapper.Map<ExtensionFrameGrid>(FrameGrid);
                    break;
                case GridType.ExtensionInterfaceGrid:
                    newFrameGrid = FrameGridMapper.Map<ExtensionInterfaceFrameGrid>(FrameGrid);
                    break;
                default:
                    newFrameGrid = FrameGridMapper.Map<BridgeFrameGrid>(FrameGrid);
                    break;
            }

            FrameGrid = newFrameGrid;

        }

        #endregion

    }

    public class FrameGridMapper
    {
        public static IFrameGrid Map<TTarget>(IFrameGrid source) where TTarget : IFrameGrid, new()
        {
            TTarget target = new TTarget();

            if (source == null)
            {
                return target;
            }

            target.Id = source.Id;
            target.Spacing = source.Spacing;
            target.Height = source.Height;
            target.Width = source.Width;

            return target;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ///PeopleCollectionWorkout();

            var firstGridDefinition = new FrameGridDefinition() { Type = GridType.PortalGrid };
            var secondGridDefinition = new FrameGridDefinition(firstGridDefinition) { Type = GridType.BridgeGrid };
            var thirdGridDefinition = new FrameGridDefinition(secondGridDefinition) { Type = GridType.BridgeGrid };
            var fourthGridDefinition = new FrameGridDefinition(thirdGridDefinition) { Type = GridType.ExtensionGrid };


            ObservableFrameGridDefinitionCollection frameGridDefinitions = new ObservableFrameGridDefinitionCollection()
            {
                firstGridDefinition,
                secondGridDefinition,
                thirdGridDefinition,
                fourthGridDefinition
            };

            var counter = 1.0;
            foreach (IFrameGridDefinition fgd in frameGridDefinitions)
            {
                fgd.FrameGrid.Height = counter * 2.8;
                fgd.FrameGrid.Spacing = counter * 2.3;
                fgd.FrameGrid.Width = counter * 2.0;
                counter += 0.1;
            }

            foreach (var item in frameGridDefinitions)
            {
                ///item.FrameGrid.Distance = 
            }

            LinkedList<IFrameGridDefinition> frameGridDefinitions1 = new LinkedList<IFrameGridDefinition>();

            frameGridDefinitions1.AddFirst(firstGridDefinition);
            frameGridDefinitions1.AddLast(secondGridDefinition);
            frameGridDefinitions1.AddLast(thirdGridDefinition);

            

            firstGridDefinition.Type = GridType.BridgeGrid;

            _ = frameGridDefinitions.Remove(secondGridDefinition);

        }

        #region People Collection Workout
        private static void PeopleCollectionWorkout()
        {
            People people = new People()
            {
                new Person(),
                new Person(),
                new Person(),
                new Person(),
            };
            people.Add(new Person());
            people.Add(new Person());

            PrintPeople(people);

            Console.WriteLine($"{Environment.NewLine}Before removing any items{Environment.NewLine}");

            people.Remove(people[3]);

            Console.WriteLine($"{Environment.NewLine}Removed 4th item{Environment.NewLine}");

            PrintPeople(people);

            people.RemoveAt(3);

            Console.WriteLine($"{Environment.NewLine}Removed item at index 3{Environment.NewLine}");

            PrintPeople(people);

            Console.WriteLine($"{Environment.NewLine}Removed last item{Environment.NewLine}");

            people.Remove(people.Last());

            PrintPeople(people);


            Console.ReadLine();
        }

        private static void PrintPeople(People people)
        {
            for (int i = 0; i < people.Count; i++)
            {
                Console.WriteLine($"Iterator: {i} and Id: {people[i].Id}");
            }
        }
        #endregion
    }

    #region Person Class
    class Person
    {
        public int Id { get; internal set; }

        public string Name { get; set; }

    }
    #endregion

    #region List with Added Functionality
    internal class People : List<Person>
    {
        public new void Add(Person person)
        {
            base.Add(person);
            person.Id = Count;
        }

        public new void Remove(Person person)
        {
            _ = base.Remove(person);

            RenumberIds();
        }

        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            RenumberIds();
        }

        private void RenumberIds()
        {
            if (Count >= 1)
            {
                for (int i = 0; i < Count; i++)
                {
                    this[i].Id = i + 1;
                }
            }
        }
    }
    #endregion

    public class ObservableFrameGridDefinitionCollection : ObservableCollection<IFrameGridDefinition>
    {
        public ObservableFrameGridDefinitionCollection()
        {
            CollectionChanged += OnFrameGridDefinitionsCollectionChanged;
        }

        private void OnFrameGridDefinitionsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CalculateProperties();
        }

        public new void Add(IFrameGridDefinition frameGridDefinition)
        {
            base.Add(frameGridDefinition);
            GenerateIds(frameGridDefinition);
        }

        private void CalculateProperties()
        {
            if (Count > 1)
            {
                IFrameGridDefinition previousItem = this[Count - 2];
                IFrameGridDefinition currentItem = this.LastOrDefault();

                if (previousItem != null)
                {
                    currentItem.FrameGrid.Distance = previousItem.FrameGrid.Distance + currentItem.FrameGrid.Spacing;
                }
            }
        }

        public new bool Remove(IFrameGridDefinition frameGridDefinition)
        {
            bool removed = base.Remove(frameGridDefinition);

            if (removed && Count >= 1)
            {
                UpdatedIds();
            }

            return removed;
        }

        private void UpdatedIds()
        {
            for (int i = 0; i < Count; i++)
            {
                int id = i + 1;

                this[i].Id = id;
                this[i].FrameGrid.Id = id;
            }
        }

        private void GenerateIds(IFrameGridDefinition frameGridDefinition)
        {
            frameGridDefinition.FrameGrid.Id = Count;
            frameGridDefinition.Id = Count;
        }
    }

}
