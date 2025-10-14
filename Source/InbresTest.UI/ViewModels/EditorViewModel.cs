using Avalonia;
using Avalonia.Media;
using InbresTest.Models;
using InbresTest.Models.Curves;
using InbresTest.Models.Primitive;
using ReactiveUI;
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
    
    //параметры безье
    [Reactive] private CreationMode _currentCreationMode = CreationMode.None;
    [Reactive] private BezierSquareShapeModel? _temporaryBezier;
    
    // состояния кривой
    public enum CreationMode
    {
        None,
        AwaitingStartPoint,
        AwaitingControlPoint,
        AwaitingEndPoint
    }
    
    
    // команды
    
    [ReactiveCommand]
    private void CanvasClick(Point point)
    {
        if (CurrentCreationMode != CreationMode.None)
        {
            HandleBezierCreationClick(point);
        }
        else
        {
            Deselect();
            ClickX = point.X;
            ClickY = point.Y;
            HasClick = true;
            System.Diagnostics.Debug.WriteLine($"Regular Click set at: {point.X}, {point.Y}");
        }
    }
    
    [ReactiveCommand]
    private void StartBezierCreation()
    {
        if (CurrentCreationMode != CreationMode.None) return;
    
        Deselect();
        CurrentCreationMode = CreationMode.AwaitingStartPoint;
        
        TemporaryBezier = null; 
    }
    
    private void HandleBezierCreationClick(Point point)
    {
        switch (CurrentCreationMode)
        {
            case CreationMode.AwaitingStartPoint:
                TemporaryBezier = new BezierSquareShapeModel
                {
                    X = point.X,
                    Y = point.Y,
                    StartPoint = new Point(0, 0),
                    IsBeingPlaced = true 
                };
                
                TemporaryBezier.IsSelected = true; 
                Shapes.Add(TemporaryBezier);
                
                CurrentCreationMode = CreationMode.AwaitingControlPoint;
                break;

            case CreationMode.AwaitingControlPoint:
                if (TemporaryBezier == null)
                {
                    CurrentCreationMode = CreationMode.None;
                    return;
                }
                
                TemporaryBezier.ControlPoint = new Point(point.X - TemporaryBezier.X, point.Y - TemporaryBezier.Y);
                
                // отрисовывка линии от A до B
                TemporaryBezier.UpdateGeometry();
                
                CurrentCreationMode = CreationMode.AwaitingEndPoint;
                
                break;

            case CreationMode.AwaitingEndPoint:
                if (TemporaryBezier == null) return;
                
                TemporaryBezier.EndPoint = new Point(point.X - TemporaryBezier.X, point.Y - TemporaryBezier.Y);
                
                // отрисовка полной кривой
                TemporaryBezier.UpdateGeometry(); 
                
                TemporaryBezier.IsBeingPlaced = false; 
                TemporaryBezier.IsSelected = false; 
                
                TemporaryBezier = null;
                CurrentCreationMode = CreationMode.None;
                System.Diagnostics.Debug.WriteLine("Bezier Creation Complete.");
                
                break;
        }
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
        
        if (CurrentCreationMode != CreationMode.None) return;
        
        Deselect();
        shape.IsSelected = true;
        HasSelectedShape =  shape;
        
        if (shape is BezierSquareShapeModel bezier)
        {
            bezier.IsBeingPlaced = false;
        }
        
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
