namespace CetTodoApp.Data;

public class TodoItem
{
    public int Id {get; set;}
    public string? Title {get; set;}
    public DateTime DueDate { get; set; }
    public DateTime CreatedDate {get; set;}
    public bool IsComplete { get; set; }

    public TodoItem()
    {
        CreatedDate = DateTime.Now;
        IsComplete = false;
        Title = "";
    }
    
}