using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProdActivity.Installer
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();
        public void Execute(object parameter) => _execute();
    }

    public class WizardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private int _currentStep = 0;
        public int CurrentStep
        {
            get => _currentStep;
            set { _currentStep = value; OnPropertyChanged(nameof(CurrentStep)); OnPropertyChanged(nameof(IsStep1)); OnPropertyChanged(nameof(IsStep2)); OnPropertyChanged(nameof(IsStep3)); }
        }

        public bool IsStep1 => CurrentStep == 0;
        public bool IsStep2 => CurrentStep == 1;
        public bool IsStep3 => CurrentStep == 2;

        private string _installPath;
        public string InstallPath
        {
            get => _installPath;
            set { _installPath = value; OnPropertyChanged(nameof(InstallPath)); CheckIsInstalled(); }
        }

        private string _statusMessage = "Ready to install.";
        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(nameof(StatusMessage)); }
        }

        private int _progress = 0;
        public int Progress
        {
            get => _progress;
            set { _progress = value; OnPropertyChanged(nameof(Progress)); }
        }

        private bool _isInstalled = false;
        public bool IsInstalled
        {
            get => _isInstalled;
            set { _isInstalled = value; OnPropertyChanged(nameof(IsInstalled)); OnPropertyChanged(nameof(IsNotInstalled)); }
        }

        public bool IsNotInstalled => !IsInstalled;

        public ICommand NextCommand { get; }
        public ICommand InstallCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand UninstallCommand { get; }
        public ICommand CloseCommand { get; }

        public WizardViewModel()
        {
            InstallPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ProdActivity");
            CheckIsInstalled();
            
            NextCommand = new RelayCommand(() => CurrentStep++);
            CloseCommand = new RelayCommand(() => Application.Current.Shutdown());
            InstallCommand = new RelayCommand(async () => await PerformInstallation(false));
            UpdateCommand = new RelayCommand(async () => await PerformInstallation(true));
            UninstallCommand = new RelayCommand(async () => await PerformUninstallation());
        }

        private void CheckIsInstalled()
        {
            IsInstalled = File.Exists(Path.Combine(InstallPath, "ProdActivity.UI.exe"));
        }

        private async Task PerformUninstallation()
        {
            CurrentStep = 2;
            Progress = 10;
            StatusMessage = "Preparing uninstallation...";

            try
            {
                await Task.Delay(500);
                if (Directory.Exists(InstallPath))
                {
                    Directory.Delete(InstallPath, true);
                }

                try
                {
                    string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ProdActivity.lnk");
                    if (File.Exists(shortcutPath)) File.Delete(shortcutPath);
                }
                catch { }

                Progress = 100;
                StatusMessage = "Uninstallation completed successfully!";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Uninstallation failed: {ex.Message}";
            }
        }

        private async Task PerformInstallation(bool isUpdate)
        {
            CurrentStep = 2; // Move to progress screen
            Progress = 10;
            StatusMessage = isUpdate ? "Preparing update..." : "Preparing installation...";

            try
            {
                await Task.Delay(500); // Simulate UI update

                string payloadDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Payload");
                if (!Directory.Exists(payloadDir))
                {
                    StatusMessage = "Error: Payload directory missing. Ensure you run build_installer.ps1.";
                    return;
                }

                if (!Directory.Exists(InstallPath))
                {
                    Directory.CreateDirectory(InstallPath);
                }

                var files = Directory.GetFiles(payloadDir, "*.*", SearchOption.AllDirectories);
                int totalFiles = files.Length;
                int copied = 0;

                foreach (var file in files)
                {
                    string relativePath = file.Substring(payloadDir.Length + 1);
                    string targetFile = Path.Combine(InstallPath, relativePath);

                    string targetDir = Path.GetDirectoryName(targetFile);
                    if (!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);

                    File.Copy(file, targetFile, true);
                    copied++;
                    
                    Progress = 10 + (int)((copied / (float)totalFiles) * 80);
                    StatusMessage = $"Copying: {Path.GetFileName(file)}";
                    await Task.Delay(10); // Artificial delay to show progress visually
                }

                Progress = 100;
                StatusMessage = isUpdate ? "Update completed successfully!" : "Installation completed successfully!";

                // Create Desktop Shortcut (Simplified via WScript)
                CreateShortcut();
            }
            catch (Exception ex)
            {
                StatusMessage = (isUpdate ? "Update failed: " : "Installation failed: ") + ex.Message;
            }
        }

        private void CreateShortcut()
        {
            try
            {
                Type wshShellType = Type.GetTypeFromProgID("WScript.Shell");
                dynamic shell = Activator.CreateInstance(wshShellType);
                string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ProdActivity.lnk");
                dynamic shortcut = shell.CreateShortcut(shortcutPath);
                shortcut.TargetPath = Path.Combine(InstallPath, "ProdActivity.UI.exe");
                shortcut.WorkingDirectory = InstallPath;
                shortcut.Save();
            }
            catch { /* Ignore shortcut errors */ }
        }
    }
}
