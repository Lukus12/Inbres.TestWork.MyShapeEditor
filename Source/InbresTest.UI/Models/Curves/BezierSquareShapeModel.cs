using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace InbresTest.Models.Curves;

public partial class BezierSquareShapeModel: ShapeBaseModel
{
    [Reactive] private Point _startPoint;
    [Reactive] private List<Point>? _controlPoint = new(); 
    [Reactive] private Point _endPoint;
    
    // свойство для отслеживания, что фигура в процессе размещения
    [Reactive] private bool _isBeingPlaced = true;
    
    public override Geometry Geometry
{
    get
    {
        var pathGeometry = new PathGeometry();
        var pathFigure = new PathFigure { StartPoint = StartPoint };
        pathFigure.IsClosed = false;

        if (ControlPoint == null) 
        {
            return pathGeometry;
        }
        
        int pointCount = ControlPoint.Count;

        if (IsBeingPlaced)
        {

            if (pointCount == 0) return pathGeometry;

            for (int i = 0; i < pointCount; i++)
            {
                pathFigure.Segments?.Add(new LineSegment { Point = ControlPoint[i] });
            }
            
        }
        else
        {
            for (int i = 0; i < pointCount; i += 2)
            {
                if (i + 1 < pointCount)
                {
                    Point control = ControlPoint[i];     // CP_n
                    Point end = ControlPoint[i + 1];     // P_n

                    pathFigure.Segments?.Add(new QuadraticBezierSegment
                    {
                        Point1 = control,
                        Point2 = end
                    });
                    
                }
                else
                {
                    pathFigure.Segments?.Add(new LineSegment { Point = ControlPoint[i] });
                }
            }
            
        }

        pathGeometry.Figures?.Add(pathFigure);
        return pathGeometry;
    }
}

    public void UpdateGeometry()
    {
        ((IReactiveObject)this).RaisePropertyChanged(nameof(Geometry));
        ((IReactiveObject)this).RaisePropertyChanged(nameof(Width));
        ((IReactiveObject)this).RaisePropertyChanged(nameof(Height));
        ((IReactiveObject)this).RaisePropertyChanged(nameof(IsBeingPlaced));
    }
}