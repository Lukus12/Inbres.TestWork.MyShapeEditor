using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using InbresTest.Models;

namespace InbresTest.Views.Shape;

public partial class BezierSquareShapeView : UserControl
{
    public BezierSquareShapeView()
    {
        InitializeComponent();
        if (DataContext is ShapeBaseModel vm)
        {
            Canvas.SetLeft(this, vm.X);
            Canvas.SetTop(this, vm.Y);
        }
    }
}