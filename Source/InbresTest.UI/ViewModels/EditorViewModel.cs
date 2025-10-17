using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Avalonia;
using InbresTest.Models;
using InbresTest.Models.Curves;
using InbresTest.Models.Primitive;
using InbresTest.Models.Serialization;
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
    
    // действия с фигурой
    [Reactive] private ShapeBaseModel? _hasSelectedShape;
    
    // коллекция фигур
    public ObservableCollection<ShapeBaseModel> Shapes { get; set; } = new();
    
    //параметры безье
    [Reactive] private CreationMode _currentCreationMode = CreationMode.None;
    [Reactive] private BezierSquareShapeModel? _temporaryBezier;
    [Reactive] private bool _isEnding;
    [Reactive] private bool _isClickControlPoint = true;
    
    // состояния кривой
    public enum CreationMode
    {
        None,
        AwaitingStartPoint,
        AwaitingControlPoint
    }
    
    public EditorViewModel()
    {
        LoadDataShapeCommand.Execute().Subscribe();
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

                if (IsClickControlPoint)
                {
                    TemporaryBezier.ControlPoint.Add(new Point(point.X - TemporaryBezier.X, point.Y - TemporaryBezier.Y));
                    IsClickControlPoint = false;
                    return;
                }

                if (IsEnding)
                {
                    TemporaryBezier.IsBeingPlaced = false; 
                    
                    // отрисовка полной кривой
                    TemporaryBezier.UpdateGeometry(); 
                    
                    TemporaryBezier.IsSelected = false; 
                    
                    IsEnding = false;
                    TemporaryBezier = null;
                    CurrentCreationMode = CreationMode.None;
                    IsClickControlPoint = true;
                    System.Diagnostics.Debug.WriteLine("Bezier Creation Complete.");
                    
                    //CurrentCreationMode = CreationMode.AwaitingEndPoint;
                    break;
                }
                
                TemporaryBezier.EndPoint.Add(new Point(point.X - TemporaryBezier.X, point.Y - TemporaryBezier.Y));

                TemporaryBezier.AddCalculationControlPoint();
                
                
                
                // отрисовывка линии
                TemporaryBezier.UpdateGeometry();
                
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
        if(HasSelectedShape != null) HasSelectedShape.IsSelected = false;
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

    private string _filePath = "../shapeData.json";
    
    [ReactiveCommand]
    private void SaveDataShape()
    {
        List<ShapeData> jsonShapes = new();

        for (int i = 0; i < Shapes.Count; i++)
        {
            jsonShapes.Add(Shapes[i].CreateSerializationData());
        }

        string jsonData = JsonSerializer.Serialize(
            jsonShapes,
            ShapeJsonContext.Default.ListShapeData
            );
        
        File.WriteAllText(_filePath, jsonData);
    }

    [ReactiveCommand]
    private void LoadDataShape()
    {
        string json = File.ReadAllText(_filePath);
        var deserializedData = JsonSerializer.Deserialize(json, ShapeJsonContext.Default.ListShapeData);
        ShapeBaseModel shape;
        
        foreach (ShapeData shapeData in deserializedData)
        {
            switch (shapeData.TypeDiscriminator)
            {
                case "RectangleShapeModel":
                    shape = new RectangleShapeModel();
                    break;
                
                case "EllipseShapeModel":
                    shape = new EllipseShapeModel();
                    break;
                
                case "BezierSquareShapeModel":
                    shape = new BezierSquareShapeModel();
                    break;
                default:
                    shape = null;
                    return;
            }

            shape.RestoreFromData(shapeData);
            Shapes.Add(shape);
        }
    }
}
