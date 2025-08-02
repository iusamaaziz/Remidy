namespace Remidy.Services
{
    /// <summary>
    /// Service interface for database backup and restore operations.
    /// Provides functionality to backup the SQLite database to a user-selected location
    /// and restore from a previously created backup.
    /// </summary>
    public interface IDatabaseBackupService
    {
        /// <summary>
        /// Creates a backup of the current database to a user-selected location.
        /// </summary>
        /// <returns>Task with the result of the backup operation</returns>
        Task<BackupResult> CreateBackupAsync();

        /// <summary>
        /// Restores the database from a user-selected backup file.
        /// </summary>
        /// <returns>Task with the result of the restore operation</returns>
        Task<RestoreResult> RestoreFromBackupAsync();

        /// <summary>
        /// Gets information about the last backup if it exists.
        /// </summary>
        /// <returns>Backup information or null if no backup exists</returns>
        Task<BackupInfo?> GetLastBackupInfoAsync();
    }

    /// <summary>
    /// Result of a backup operation.
    /// </summary>
    public class BackupResult
    {
        public bool Success { get; set; }
        public string? FilePath { get; set; }
        public string? ErrorMessage { get; set; }
        public long FileSizeBytes { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// Result of a restore operation.
    /// </summary>
    public class RestoreResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime RestoredAt { get; set; }
    }

    /// <summary>
    /// Information about a backup file.
    /// </summary>
    public class BackupInfo
    {
        public string FilePath { get; set; } = string.Empty;
        public long FileSizeBytes { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FileName => Path.GetFileName(FilePath);
        public string FileSizeFormatted => FormatFileSize(FileSizeBytes);

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
    }
} 