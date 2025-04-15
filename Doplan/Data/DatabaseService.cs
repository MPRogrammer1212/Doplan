using Doplan.Model;
using SQLite;

namespace Doplan.Data;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _connection;
    public DatabaseService()
    {
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, Constants.DatabaseFileName);
        _connection = new SQLiteAsyncConnection(databasePath);

        _connection.CreateTableAsync<TaskModel>().Wait();
        _connection.CreateTableAsync<TermModel>().Wait();
        _connection.CreateTableAsync<NoteModel>().Wait();

    }

    public SQLiteAsyncConnection GetConnection() => _connection;
}
