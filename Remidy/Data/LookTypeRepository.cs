using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

using Remidy.Models;

namespace Remidy.Data
{
    public class LookTypeRepository
    {
        private bool _hasBeenInitialized = false;
        private readonly ILogger _logger;

        public LookTypeRepository(ILogger<LookTypeRepository> logger)
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
                CREATE TABLE IF NOT EXISTS LookType (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL
                );";
                await createTableCmd.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating LookType table");
                throw;
            }

            _hasBeenInitialized = true;
        }

        public async Task<List<LookType>> ListAsync()
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = "SELECT * FROM LookType";

            var items = new List<LookType>();

            await using var reader = await selectCmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                items.Add(new LookType
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1)
                });
            }

            return items;
        }

        public async Task<LookType?> GetAsync(int id)
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = "SELECT * FROM LookType WHERE Id = @id";
            selectCmd.Parameters.AddWithValue("@id", id);

            await using var reader = await selectCmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new LookType
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1)
                };
            }

            return null;
        }

        public async Task<int> SaveItemAsync(LookType item)
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var saveCmd = connection.CreateCommand();
            if (item.Id == 0)
            {
                saveCmd.CommandText = @"
                INSERT INTO LookType (Title)
                VALUES (@Title);
                SELECT last_insert_rowid();";
            }
            else
            {
                saveCmd.CommandText = @"
                UPDATE LookType SET Title = @Title
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

        public async Task<int> DeleteItemAsync(LookType item)
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var deleteCmd = connection.CreateCommand();
            deleteCmd.CommandText = "DELETE FROM LookType WHERE Id = @id";
            deleteCmd.Parameters.AddWithValue("@id", item.Id);

            return await deleteCmd.ExecuteNonQueryAsync();
        }

        public async Task DropTableAsync()
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var dropTableCmd = connection.CreateCommand();
            dropTableCmd.CommandText = "DROP TABLE IF EXISTS LookType";

            await dropTableCmd.ExecuteNonQueryAsync();
            _hasBeenInitialized = false;
        }
    }

}
