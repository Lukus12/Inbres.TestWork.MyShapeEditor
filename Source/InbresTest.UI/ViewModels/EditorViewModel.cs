using Avalonia;
using InbresTest.Models;
using InbresTest.Models.Curves;
using InbresTest.Models.Primitive;
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
    private void AddSquareBezier()
    {
        if (!HasClick) return;
        
        var newCurve = new BezierSquareShapeModel 
        { 
            X = ClickX, 
            Y = ClickY,
            // Установим начальные значения, чтобы она была видна
            StartPoint = new Point(0, 25),
            ControlPoint = new Point(50, 100),
            EndPoint = new Point(100, 25),
        };
        
        Shapes.Add(newCurve);
        
        HasClick = false;
    }

    [ReactiveCommand]
    private void AddRectangle()
    {
        if (!HasClick)
        {
            System.Diagnostics.Debug.WriteLine("No click position!");
            return;
        }
        
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
        
        HasSelectedShape.MoveTo(delta);
       
    }

    [ReactiveCommand]
    private void ResizedShape(object[] args)
    {
        if(HasSelectedShape == null) return;
        if(args[0] is not string type || args[1] is not Point delta) return;
        
        HasSelectedShape.ResizeShape(type, delta);
    }

    [ReactiveCommand]
    private void ChangeColor()
    {
        if(HasSelectedShape == null) return;
        HasSelectedShape.ChangeColor();
    }
}
