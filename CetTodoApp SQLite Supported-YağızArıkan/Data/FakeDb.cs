namespace CetTodoApp.Data;

public static class FakeDb
{
    public static List<TodoItem> Data = new List<TodoItem>();

    public static void AddToDo(TodoItem item )
    {
        Data.Add(item);
    }
    
    public static void AddToDo(string title, DateTime dueDate )
    {

        TodoItem item = new TodoItem();
            item.Title = title;
            item.DueDate = dueDate;
            
        Data.Add(item);
    }

    public static void ChageCompletionStatus(TodoItem item)
    {
        item.IsComplete = !item.IsComplete;
    }
}