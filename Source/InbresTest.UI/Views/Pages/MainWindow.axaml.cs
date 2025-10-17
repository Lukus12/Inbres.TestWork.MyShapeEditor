using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using InbresTest.ViewModels;

namespace InbresTest.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var editorVM = new EditorViewModel();
        DataContext = editorVM;

        if (DataContext is EditorViewModel viewModel)
        {
            // Подписка на событие закрытия окна
            Closing += async (sender, e) => await OnClosing(viewModel, e);
        }
    }
    private async Task OnClosing(EditorViewModel viewModel, WindowClosingEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("Application closing. Saving data...");
        await viewModel.SaveDataShapeCommand.Execute(); 
    }
}