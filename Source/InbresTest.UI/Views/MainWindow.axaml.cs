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
    }
}