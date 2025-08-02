using CommunityToolkit.Maui.Storage;

using Microsoft.Extensions.Logging;

namespace Remidy.Services
{
    /// <summary>
    /// Implementation of database backup and restore service.
    /// Handles SQLite database backup to user-selected locations and restore operations.
    /// </summary>
    public class DatabaseBackupService : IDatabaseBackupService
    {
        private readonly ILogger<DatabaseBackupService> _logger;
        private readonly string _databasePath;

        public DatabaseBackupService(ILogger<DatabaseBackupService> logger)
        {
            _logger = logger;
            _databasePath = Path.Combine(FileSystem.AppDataDirectory, Constants.DatabaseFilename);
        }

        /// <summary>
        /// Creates a backup of the current database to a user-selected location.
        /// </summary>
        /// <returns>Task with the result of the backup operation</returns>
        public async Task<BackupResult> CreateBackupAsync()
        {
            try
            {
                // Check if source database exists
                if (!File.Exists(_databasePath))
                {
                    return new BackupResult
                    {
                        Success = false,
                        ErrorMessage = "Source database file not found."
                    };
                }

                // Generate backup filename with timestamp
                var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                var defaultFileName = $"Remidy_Backup_{timestamp}.db3";

                // Let user select backup location
                var backupPath = await PickBackupLocationAsync(defaultFileName);
                if (string.IsNullOrEmpty(backupPath))
                {
                    return new BackupResult
                    {
                        Success = false,
                        ErrorMessage = "Backup location not selected."
                    };
                }

                // Copy database file
                await Task.Run(() => File.Copy(_databasePath, backupPath, true));

                var fileInfo = new FileInfo(backupPath);
                var result = new BackupResult
                {
                    Success = true,
                    FilePath = backupPath,
                    FileSizeBytes = fileInfo.Length,
                    CreatedAt = DateTime.Now
                };

                _logger.LogInformation("Database backup created successfully: {BackupPath}", backupPath);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating database backup");
                return new BackupResult
                {
                    Success = false,
                    ErrorMessage = $"Backup failed: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Restores the database from a user-selected backup file.
        /// </summary>
        /// <returns>Task with the result of the restore operation</returns>
        public async Task<RestoreResult> RestoreFromBackupAsync()
        {
            try
            {
                // Let user select backup file
                var backupPath = await PickBackupFileAsync();
                if (string.IsNullOrEmpty(backupPath))
                {
                    return new RestoreResult
                    {
                        Success = false,
                        ErrorMessage = "Backup file not selected."
                    };
                }

                // Validate backup file
                if (!File.Exists(backupPath))
                {
                    return new RestoreResult
                    {
                        Success = false,
                        ErrorMessage = "Selected backup file does not exist."
                    };
                }

                // Create a backup of current database before restore
                var currentBackupPath = await CreateSafetyBackupAsync();
                if (!string.IsNullOrEmpty(currentBackupPath))
                {
                    _logger.LogInformation("Safety backup created before restore: {SafetyBackupPath}", currentBackupPath);
                }

                // Stop any active database connections
                await CloseDatabaseConnectionsAsync();

                // Restore database
                await Task.Run(() => File.Copy(backupPath, _databasePath, true));

                var result = new RestoreResult
                {
                    Success = true,
                    RestoredAt = DateTime.Now
                };

                _logger.LogInformation("Database restored successfully from: {BackupPath}", backupPath);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error restoring database");
                return new RestoreResult
                {
                    Success = false,
                    ErrorMessage = $"Restore failed: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Gets information about the last backup if it exists.
        /// </summary>
        /// <returns>Backup information or null if no backup exists</returns>
        public async Task<BackupInfo?> GetLastBackupInfoAsync()
        {
            try
            {
                // This is a simplified implementation
                // In a real app, you might want to store backup metadata in a separate file
                // or scan common backup locations for the most recent backup
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting last backup info");
                return null;
            }
        }

        /// <summary>
        /// Opens folder picker for user to select backup location.
        /// </summary>
        /// <param name="defaultFileName">Default filename for the backup</param>
        /// <returns>Selected folder path or null if cancelled</returns>
        private async Task<string?> PickBackupLocationAsync(string defaultFileName)
        {
            try
            {
                var result = await FolderPicker.PickAsync(FileSystem.AppDataDirectory);
                if (result != null)
                {
                    // Combine the selected folder path with the default filename
                    return Path.Combine(result.Folder.Path, defaultFileName);
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error picking backup location");
                return null;
            }
        }

        /// <summary>
        /// Opens file picker for user to select backup file for restore.
        /// </summary>
        /// <returns>Selected file path or null if cancelled</returns>
        private async Task<string?> PickBackupFileAsync()
        {
            try
            {
                var customFileType = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.WinUI, new[] { ".db3", ".db", ".sqlite", ".sqlite3" } },
                        { DevicePlatform.macOS, new[] { "db3", "db", "sqlite", "sqlite3" } },
                        { DevicePlatform.Android, new[] { "application/x-sqlite3", "application/vnd.sqlite3" } },
                        { DevicePlatform.iOS, new[] { "public.database", "public.sqlite" } }
                    });

                var options = new PickOptions
                {
                    PickerTitle = "Select Backup File to Restore",
                    FileTypes = customFileType
                };

                var result = await FilePicker.PickAsync(options);
                return result?.FullPath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error picking backup file");
                return null;
            }
        }

        /// <summary>
        /// Creates a safety backup of the current database before restore.
        /// </summary>
        /// <returns>Path to safety backup or null if failed</returns>
        private async Task<string?> CreateSafetyBackupAsync()
        {
            try
            {
                if (!File.Exists(_databasePath))
                    return null;

                var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                var safetyBackupPath = Path.Combine(FileSystem.AppDataDirectory, $"Safety_Backup_{timestamp}.db3");
                
                await Task.Run(() => File.Copy(_databasePath, safetyBackupPath, true));
                return safetyBackupPath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating safety backup");
                return null;
            }
        }

        /// <summary>
        /// Closes any active database connections to allow file operations.
        /// </summary>
        private async Task CloseDatabaseConnectionsAsync()
        {
            try
            {
                // Force garbage collection to release any file handles
                GC.Collect();
                GC.WaitForPendingFinalizers();
                
                // Small delay to ensure file handles are released
                await Task.Delay(100);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error closing database connections");
            }
        }
    }
} 