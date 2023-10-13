using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Domain.TankAndSupportStructure.Common.Interfaces;
using ReInvented.Domain.TankAndSupportStructure.Models;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.ProjectSetup.Models
{
    public class Project : ErrorsEnabledPropertyStore, IProject
    {
        #region Default Constructor

        public Project()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        public IThickenerInput Input { get => Get<IThickenerInput>(); private set => Set(value); }

        public ISettings Settings { get => Get<Settings>(); private set => Set(value); }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            Input = new ThickenerInput();
            Settings = new Settings();
        }

        #endregion
    }
}
