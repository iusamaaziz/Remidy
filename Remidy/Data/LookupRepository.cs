using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace Remidy.Data
{
    public class LookupRepository<T> where T : ILookup
    {
        private string _lookupName => typeof(T).Name;
        private bool _hasBeenInitialized = false;
        private readonly ILogger _logger;

        public LookupRepository(ILogger<HandTypeRepository> logger)
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
                createTableCmd.CommandText = @$"
                CREATE TABLE IF NOT EXISTS {_lookupName} (
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

        public async Task<List<T>> ListAsync()
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = $"SELECT * FROM {_lookupName}";

            var items = new List<T>();

            await using var reader = await selectCmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var instance = Activator.CreateInstance<T>();
                (instance as ILookup).Id = reader.GetInt32(0);
                (instance as ILookup).Title = reader.GetString(1);

                items.Add(instance);
            }

            return items;
        }

        public async Task<T?> GetAsync(int id)
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = $"SELECT * FROM {_lookupName} WHERE Id = @id";
            selectCmd.Parameters.AddWithValue("@id", id);

            await using var reader = await selectCmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var instance = Activator.CreateInstance<T>();
                (instance as ILookup).Id = reader.GetInt32(0);
                (instance as ILookup).Title = reader.GetString(1);

                return instance;
            }

            return default(T);
        }

        public async Task<int> SaveItemAsync(T item)
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var saveCmd = connection.CreateCommand();
            if (item.Id == 0)
            {
                saveCmd.CommandText = @$"
                INSERT INTO {_lookupName} (Title)
                VALUES (@Title);
                SELECT last_insert_rowid();";
            }
            else
            {
                saveCmd.CommandText = @$"
                UPDATE {_lookupName} SET Title = @Title
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

        public async Task<int> DeleteItemAsync(T item)
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var deleteCmd = connection.CreateCommand();
            deleteCmd.CommandText = $"DELETE FROM {_lookupName} WHERE Id = @id";
            deleteCmd.Parameters.AddWithValue("@id", item.Id);

            return await deleteCmd.ExecuteNonQueryAsync();
        }

        public async Task DropTableAsync()
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var dropTableCmd = connection.CreateCommand();
            dropTableCmd.CommandText = $"DROP TABLE IF EXISTS {_lookupName}";

            await dropTableCmd.ExecuteNonQueryAsync();
            _hasBeenInitialized = false;
        }
    }
}
