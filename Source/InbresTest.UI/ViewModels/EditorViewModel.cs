

using Avalonia;
using InbresTest.Models;
using ReactiveUI.SourceGenerators;

namespace InbresTest.ViewModels;

using System.Collections.ObjectModel;

public partial class EditorViewModel : ViewModelBase
{
    //координаты клика
    [Reactive] private double _clickX;
    [Reactive] private double _clickY;
    
    // проверка клика
    [Reactive] private bool _hasClick;
    
    
    // коллекция фигур
    public ObservableCollection<ShapeBaseModel> Shapes { get; set; } = new();
    
    // команды
    [ReactiveCommand]
    private void CanvasClick(Point point)
    {
        System.Diagnostics.Debug.WriteLine($"OnCanvasClick called! X={point.X}, Y={point.Y}");
        ClickX = point.X;
        ClickY = point.Y;
        HasClick = true;
        System.Diagnostics.Debug.WriteLine($"HasClick set to true");
    }

    [ReactiveCommand]
    private void AddRectangle()
    {
        if (!HasClick)
        {
            System.Diagnostics.Debug.WriteLine("No click position!");
            return;
        };
        
        Shapes.Add(new RectangleShapeModel { X = ClickX, Y = ClickY });
        
        System.Diagnostics.Debug.WriteLine($"Added rect at ({ClickX}, {ClickY})");
        
        HasClick = false;
    }
    
    [ReactiveCommand]
    private void AddEllipse()
    {
        if (!HasClick) return;
        
        Shapes.Add(new EllipseShapeModel { X = ClickX, Y = ClickY });
        
        System.Diagnostics.Debug.WriteLine($"Added ellipse at ({ClickX}, {ClickY})");
        
        HasClick = false;
    }

    [ReactiveCommand]
    private void DeleteSelectedShape()
    {
        
    }

}
