using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

using Remidy.Models;

namespace Remidy.Data
{
    public class HandTypeRepository
    {
        private bool _hasBeenInitialized = false;
        private readonly ILogger _logger;

        public HandTypeRepository(ILogger<HandTypeRepository> logger)
        {
            _logger = logger;
        }

        private async Task Init()
        {
            if (_hasBeenInitialized)
                return;

            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            try
            {
                var createTableCmd = connection.CreateCommand();
                createTableCmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS HandType (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL
                );";
                await createTableCmd.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating HandType table");
                throw;
            }

            _hasBeenInitialized = true;
        }

        public async Task<List<HandType>> ListAsync()
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = "SELECT * FROM HandType";

            var items = new List<HandType>();

            await using var reader = await selectCmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                items.Add(new HandType
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1)
                });
            }

            return items;
        }

        public async Task<HandType?> GetAsync(int id)
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = "SELECT * FROM HandType WHERE Id = @id";
            selectCmd.Parameters.AddWithValue("@id", id);

            await using var reader = await selectCmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new HandType
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1)
                };
            }

            return null;
        }

        public async Task<int> SaveItemAsync(HandType item)
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var saveCmd = connection.CreateCommand();
            if (item.Id == 0)
            {
                saveCmd.CommandText = @"
                INSERT INTO HandType (Title)
                VALUES (@Title);
                SELECT last_insert_rowid();";
            }
            else
            {
                saveCmd.CommandText = @"
                UPDATE HandType SET Title = @Title
                WHERE Id = @Id";
                saveCmd.Parameters.AddWithValue("@Id", item.Id);
            }

            saveCmd.Parameters.AddWithValue("@Title", item.Title);

            var result = await saveCmd.ExecuteScalarAsync();
            if (item.Id == 0)
            {
                item.Id = Convert.ToInt32(result);
            }

            return item.Id;
        }

        public async Task<int> DeleteItemAsync(HandType item)
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var deleteCmd = connection.CreateCommand();
            deleteCmd.CommandText = "DELETE FROM HandType WHERE Id = @id";
            deleteCmd.Parameters.AddWithValue("@id", item.Id);

            return await deleteCmd.ExecuteNonQueryAsync();
        }

        public async Task DropTableAsync()
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var dropTableCmd = connection.CreateCommand();
            dropTableCmd.CommandText = "DROP TABLE IF EXISTS HandType";

            await dropTableCmd.ExecuteNonQueryAsync();
            _hasBeenInitialized = false;
        }
    }

}
