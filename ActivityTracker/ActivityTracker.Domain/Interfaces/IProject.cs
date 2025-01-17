using ProdActivity.Domain.Enums;

using System;
using System.Collections.ObjectModel;

namespace ProdActivity.Domain.Interfaces
{
    public interface IProject
    {
        event Action<IProject> ProjectCodeChanged;

        ProjectType Type { get; }

        string Code { get; }

        string Name { get; }

        ObservableCollection<IActivity> Activities { get; set; }
    }
}