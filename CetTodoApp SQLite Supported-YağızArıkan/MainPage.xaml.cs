using System.Globalization;
using CetTodoApp.Data;

namespace CetTodoApp;

public class DateToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime date)
        {
            return date.Date < DateTime.Today ? Colors.Red : Colors.Black;
        }
        return Colors.Black;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public partial class MainPage : ContentPage
{
    TodoDatabase database;

    public MainPage()
    {
        InitializeComponent();
        database = new TodoDatabase();
        Task.Run(async () => await RefreshListView());
    }

    private async void AddButton_OnClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Title.Text))
        {
            await DisplayAlert("Error", "Title cannot be empty", "OK");
            return;
        }

        if (DueDate.Date < DateTime.Today)
        {
            await DisplayAlert("Error", "Due date cannot be in the past", "OK");
            return;
        }

        var newItem = new TodoItem
        {
            Title = Title.Text,
            DueDate = DueDate.Date,
            IsComplete = false,
            CreatedDate = DateTime.Now
        };

        await database.SaveItemAsync(newItem);

        Title.Text = string.Empty;
        DueDate.Date = DateTime.Now;
        
        await RefreshListView();
    }

    private async Task RefreshListView()
    {
        var items = await database.GetFilteredItemsAsync();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            TasksListView.ItemsSource = null;
            TasksListView.ItemsSource = items;
        });
    }

    private async void TasksListView_OnItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {
        var item = e.SelectedItem as TodoItem;
        if (item != null)
        {
            item.IsComplete = !item.IsComplete;
            await database.SaveItemAsync(item);
            await RefreshListView();
        }
    }
}