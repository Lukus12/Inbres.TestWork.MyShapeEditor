

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
    
    // дейсвтия с фигурой
    [Reactive] private ShapeBaseModel? _hasSelectedShape;
    
    // коллекция фигур
    public ObservableCollection<ShapeBaseModel> Shapes { get; set; } = new();
    
    // команды
    [ReactiveCommand]
    private void CanvasClick(Point point)
    {
        Deselect();
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
        if(HasSelectedShape!=null) Shapes.Remove(HasSelectedShape);
    }

    [ReactiveCommand]
    private void SelectedShape(ShapeBaseModel shape)
    {
        Deselect();
        shape.IsSelected = true;
        HasSelectedShape =  shape;
        
        System.Diagnostics.Debug.WriteLine($"Shape set to Selected");
    }

    private void Deselect()
    {
        if(HasSelectedShape!=null) HasSelectedShape.IsSelected = false;
        HasSelectedShape = null;
    }

    [ReactiveCommand]
    private void MovedShape(Point delta)
    {
        if(HasSelectedShape == null) return;
        
        if (HasSelectedShape.X >= 0 && HasSelectedShape.Y >= 0)
        {
            HasSelectedShape.X += delta.X;
            HasSelectedShape.Y += delta.Y;
        }

        if (HasSelectedShape.X < 0) HasSelectedShape.X = 0;
        if (HasSelectedShape.Y < 0) HasSelectedShape.Y = 0;
    }
}
