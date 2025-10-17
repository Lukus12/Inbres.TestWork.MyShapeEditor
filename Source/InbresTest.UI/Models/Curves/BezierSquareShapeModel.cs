using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using InbresTest.Models.Serialization;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace InbresTest.Models.Curves;

public partial class BezierSquareShapeModel: ShapeBaseModel
{
    [Reactive] private Point _startPoint;
    [Reactive] private ObservableCollection<Point> _controlPoint  = new();
    [Reactive] private ObservableCollection<Point> _endPoint = new();
    
    private double _width = 100;
    private double _height = 200;
    public override double Width
    {
        get => _width;
        set => this.RaiseAndSetIfChanged(ref _width, value);
    }

    public override double Height
    {
        get => _height;
        set => this.RaiseAndSetIfChanged(ref _height, value);
    }
    

    // свойство для отслеживания, что фигура в процессе размещения
    [Reactive] private bool _isBeingPlaced = true;
    
    public override Geometry Geometry
    {
        get
        {
            var pathGeometry = new PathGeometry();
            
            if (ControlPoint.Count != EndPoint.Count)
            {
                return pathGeometry; 
            }
            
            var pathFigure = new PathFigure
            {
                StartPoint = StartPoint,
                IsClosed = false
            };
            

            if (IsBeingPlaced)
            {

                for (int i = 0; i < ControlPoint.Count; i++)
                {
                    pathFigure.Segments?.Add(new QuadraticBezierSegment
                    {
                        Point1 = ControlPoint[i],
                        Point2 = EndPoint[i]
                    });
                }
            }
            else
            {
                pathFigure = new PathFigure
                {
                    StartPoint = StartPoint,
                    IsClosed = false
                };
                for (int i = 0; i < ControlPoint.Count; i++)
                {
                    pathFigure.Segments?.Add(new QuadraticBezierSegment
                    {
                        Point1 = ControlPoint[i],
                        Point2 = EndPoint[i]
                    });
                }
            }
            
            pathGeometry.Figures?.Add(pathFigure);
            return pathGeometry;
        }
    }
    
    
     public override void ResizeShape(string type, Point delta)
    {
        Point internalDelta = new Point(0, 0);
        switch (type)
        {
            case "TopCenter":
                internalDelta = new Point(0, delta.Y);
                
                ControlPoint[0] = new Point(
                    ControlPoint[0].X + internalDelta.X,
                    ControlPoint[0].Y + internalDelta.Y
                );
                
                break;
            
            case "LeftCenter":
                internalDelta = new Point(delta.X, 0);
                
                ControlPoint[0] = new Point(
                    ControlPoint[0].X + internalDelta.X,
                    ControlPoint[0].Y + internalDelta.Y
                );
                
                break;
            
            case "TopLeft":
                internalDelta = delta;
                ControlPoint[0] = new Point(
                    ControlPoint[0].X + internalDelta.X,
                    ControlPoint[0].Y + internalDelta.Y
                );
                break;
        }
        
        RecalculateControlPoints();
        
        UpdateGeometry(); 
    }

    public void AddCalculationControlPoint()
    {
        if(EndPoint.Count > 1)
            ControlPoint.Add( new Point(
                2 * EndPoint[^2].X - ControlPoint[^1].X,
                2 * EndPoint[^2].Y - ControlPoint[^1].Y
            ));
    }
    
    public void RecalculateControlPoints()
    {
        if (ControlPoint.Count == 0 || EndPoint.Count == 0)
        {
            return;
        }
        
        int maxIndex = Math.Min(ControlPoint.Count, EndPoint.Count);
        
        for (int i = 1; i < maxIndex; i++)
        {
            ControlPoint[i] = new Point(
                2 * EndPoint[i - 1].X - ControlPoint[i - 1].X,
                2 * EndPoint[i - 1].Y - ControlPoint[i - 1].Y
            );
        }
    }
    
    public void UpdateGeometry()
    {
        ((IReactiveObject)this).RaisePropertyChanged(nameof(Geometry));
    }
    
    
    public override BezierShapeData CreateSerializationData()
    {
        return new BezierShapeData
        {
            X = X,
            Y = Y,
            Width = Width,
            Height = Height,
            TypeDiscriminator = GetType().Name,
            
            StartPoint = new PointDto(StartPoint),
            ControlPoints = ControlPoint.Select(p => new PointDto(p)).ToList(),
            EndPoints = EndPoint.Select(p => new PointDto(p)).ToList(),
        };
    }
    
    public override void RestoreFromData(ShapeData data)
    {
        if (data is not BezierShapeData bezierData || data.TypeDiscriminator != nameof(BezierSquareShapeModel))
        {
            return;
        }
        
        X = bezierData.X;
        Y = bezierData.Y;
        Width = bezierData.Width;
        Height = bezierData.Height;
        
        ControlPoint.Clear();
        foreach (var dto in bezierData.ControlPoints)
        {
            ControlPoint.Add(dto.ToAvaloniaPoint()); 
        }

        EndPoint.Clear();
        foreach (var dto in bezierData.EndPoints)
        {
            EndPoint.Add(dto.ToAvaloniaPoint());
        }
    
        System.Diagnostics.Debug.WriteLine($"ControlPoints count: {ControlPoint.Count}");
        System.Diagnostics.Debug.WriteLine($"EndPoints count: {EndPoint.Count}");

        for (int i = 0; i < EndPoint.Count; i++)
        {
            System.Diagnostics.Debug.WriteLine($"EndPoint: {EndPoint[i]}");
        }
        
        UpdateGeometry(); 
    }
    
}