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
    public MainPage()
    {
        InitializeComponent();
        FakeDb.AddToDo("Test1" ,DateTime.Now.AddDays(-1));
        FakeDb.AddToDo("Test2" ,DateTime.Now.AddDays(1));
        FakeDb.AddToDo("Test3" ,DateTime.Now);
        RefreshListView();
        ;
    }

    private void AddButton_OnClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Title.Text))
        {
            DisplayAlert("Error", "Title cannot be empty", "OK");
            return;
        }

        if (DueDate.Date < DateTime.Today)
        {
            DisplayAlert("Error", "Due date cannot be in the past", "OK");
            return;
        }

        FakeDb.AddToDo(Title.Text, DueDate.Date);
        Title.Text = string.Empty;
        DueDate.Date=DateTime.Now;
        RefreshListView();
    }

    private void RefreshListView()
    {
        TasksListView.ItemsSource = null;
        TasksListView.ItemsSource = FakeDb.Data.Where(x => !x.IsComplete ||
                                                           (x.IsComplete && x.DueDate > DateTime.Now.AddDays(-1)))
            .ToList();
    }

    private void TasksListView_OnItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {
        var item = e.SelectedItem as TodoItem;
        if (item != null)
        {
            FakeDb.ChageCompletionStatus(item);
            RefreshListView();
        }
    }
}