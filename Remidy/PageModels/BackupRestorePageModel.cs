using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Remidy.Data;
using Remidy.Services;

namespace Remidy.PageModels
{
    /// <summary>
    /// ViewModel for the Database Backup and Restore Page.
    /// Manages database backup creation, restoration, and status display.
    /// Provides user-friendly interface for critical database operations.
    /// </summary>
    /// <remarks>
    /// Core Functionality:
    /// - Create database backups to user-selected locations
    /// - Restore database from backup files with safety measures
    /// - Display real-time operation status and results
    /// - Show database location and backup information
    /// - Handle user confirmations for destructive operations
    /// 
    /// Safety Features:
    /// - Automatic safety backup before restore operations
    /// - Clear warnings about data loss during restore
    /// - Validation of backup files before restoration
    /// - User confirmation dialogs for critical operations
    /// 
    /// User Experience:
    /// - Loading indicators during operations
    /// - Clear status messages and error handling
    /// - Informative UI with helpful tips and guidance
    /// - Responsive design with proper state management
    /// </remarks>
    public partial class BackupRestorePageModel : ObservableObject
    {
        private readonly IDatabaseBackupService _backupService;

        public BackupRestorePageModel(IDatabaseBackupService backupService)
        {
            _backupService = backupService;
            LoadDatabaseInfo();
        }

        #region Observable Properties

        /// <summary>
        /// Indicates if the page is currently busy with an operation.
        /// Used to show loading indicators and disable UI controls.
        /// </summary>
        [ObservableProperty]
        private bool _isBusy;

        /// <summary>
        /// Inverse of IsBusy for enabling/disabling UI controls.
        /// </summary>
        public bool IsNotBusy => !IsBusy;

        /// <summary>
        /// Current status message to display to the user.
        /// Shows operation results, errors, or informational messages.
        /// </summary>
        [ObservableProperty]
        private string _statusMessage = string.Empty;

        /// <summary>
        /// Color for the status message based on operation result.
        /// Green for success, red for errors, blue for information.
        /// </summary>
        [ObservableProperty]
        private Color _statusColor = Colors.Black;

        /// <summary>
        /// Indicates if there is a status message to display.
        /// Used to show/hide the status section.
        /// </summary>
        public bool HasStatusMessage => !string.IsNullOrEmpty(StatusMessage);

        /// <summary>
        /// Information about the last backup operation if available.
        /// </summary>
        [ObservableProperty]
        private BackupInfo? _lastBackupInfo;

        /// <summary>
        /// Indicates if backup information is available to display.
        /// </summary>
        public bool HasBackupInfo => LastBackupInfo != null;

        /// <summary>
        /// Formatted text displaying backup information.
        /// Shows file name, size, and creation date.
        /// </summary>
        public string BackupInfoText
        {
            get
            {
                if (LastBackupInfo == null) return string.Empty;
                return $"File: {LastBackupInfo.FileName}\nSize: {LastBackupInfo.FileSizeFormatted}\nCreated: {LastBackupInfo.CreatedAt:g}";
            }
        }

        /// <summary>
        /// Current database file location for display purposes.
        /// Shows users where their data is stored.
        /// </summary>
        [ObservableProperty]
        private string _databaseLocation = string.Empty;

        #endregion

        #region Commands

        /// <summary>
        /// Command to create a new database backup.
        /// Opens file picker for user to select backup location.
        /// </summary>
        [RelayCommand]
        private async Task CreateBackup()
        {
            try
            {
                IsBusy = true;
                ClearStatus();

                var result = await _backupService.CreateBackupAsync();

                if (result.Success)
                {
                    SetStatus($"Backup created successfully!\nFile: {result.FilePath}\nSize: {FormatFileSize(result.FileSizeBytes)}", Colors.Green);
                    LastBackupInfo = new BackupInfo
                    {
                        FilePath = result.FilePath!,
                        FileSizeBytes = result.FileSizeBytes,
                        CreatedAt = result.CreatedAt
                    };
                    OnPropertyChanged(nameof(BackupInfoText));
                    OnPropertyChanged(nameof(HasBackupInfo));
                }
                else
                {
                    SetStatus($"Backup failed: {result.ErrorMessage}", Colors.Red);
                }
            }
            catch (Exception ex)
            {
                SetStatus($"Unexpected error: {ex.Message}", Colors.Red);
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Command to restore database from a backup file.
        /// Includes safety measures and user confirmation.
        /// </summary>
        [RelayCommand]
        private async Task RestoreDatabase()
        {
            try
            {
                // Show confirmation dialog
                var confirmed = await Application.Current!.MainPage!.DisplayAlert(
                    "Confirm Restore",
                    "This will replace your current database with the backup. All current data will be lost. Are you sure you want to continue?",
                    "Yes, Restore",
                    "Cancel");

                if (!confirmed) return;

                IsBusy = true;
                ClearStatus();

                var result = await _backupService.RestoreFromBackupAsync();

                if (result.Success)
                {
                    SetStatus("Database restored successfully! The application will restart to apply changes.", Colors.Green);
                    
                    // Show restart confirmation
                    var restart = await Application.Current.MainPage!.DisplayAlert(
                        "Restart Required",
                        "Database has been restored successfully. The application needs to restart to apply the changes. Restart now?",
                        "Restart Now",
                        "Later");

                    if (restart)
                    {
                        // Restart the application
                        Application.Current.Quit();
                    }
                }
                else
                {
                    SetStatus($"Restore failed: {result.ErrorMessage}", Colors.Red);
                }
            }
            catch (Exception ex)
            {
                SetStatus($"Unexpected error: {ex.Message}", Colors.Red);
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads database information for display.
        /// Shows users where their data is stored.
        /// </summary>
        private void LoadDatabaseInfo()
        {
            try
            {
                var dbPath = Constants.DatabasePath.Replace("Data Source=", "");
                DatabaseLocation = dbPath;
            }
            catch (Exception ex)
            {
                DatabaseLocation = "Unable to determine database location";
            }
        }

        /// <summary>
        /// Clears the current status message.
        /// </summary>
        private void ClearStatus()
        {
            StatusMessage = string.Empty;
            StatusColor = Colors.Black;
            OnPropertyChanged(nameof(HasStatusMessage));
        }

        /// <summary>
        /// Sets the status message and color.
        /// </summary>
        /// <param name="message">Status message to display</param>
        /// <param name="color">Color for the status message</param>
        private void SetStatus(string message, Color color)
        {
            StatusMessage = message;
            StatusColor = color;
            OnPropertyChanged(nameof(HasStatusMessage));
        }

        /// <summary>
        /// Formats file size in human-readable format.
        /// </summary>
        /// <param name="bytes">File size in bytes</param>
        /// <returns>Formatted file size string</returns>
        private static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        #endregion
    }
} 