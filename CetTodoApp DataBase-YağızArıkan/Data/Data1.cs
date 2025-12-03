using SQLite;

namespace CetTodoApp.Data;

public class TodoDatabase
{
    SQLiteAsyncConnection Database;

    public TodoDatabase()
    {
    }

    async Task Init()
    {
        if (Database is not null)
            return;

        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyTodo.db");
        
        Database = new SQLiteAsyncConnection(databasePath);
        
        await Database.CreateTableAsync<TodoItem>();
    }

    public async Task<List<TodoItem>> GetFilteredItemsAsync()
    {
        await Init();
        
        var allItems = await Database.Table<TodoItem>().ToListAsync();
        
        return allItems.Where(x => !x.IsComplete || 
                                   (x.IsComplete && x.DueDate > DateTime.Now.AddDays(-1)))
            .ToList();
    }

    public async Task<int> SaveItemAsync(TodoItem item)
    {
        await Init();
        if (item.Id != 0)
        {
            return await Database.UpdateAsync(item);
        }
        else
        {
            return await Database.InsertAsync(item);
        }
    }

    public async Task<int> DeleteItemAsync(TodoItem item)
    {
        await Init();
        return await Database.DeleteAsync(item);
    }
}