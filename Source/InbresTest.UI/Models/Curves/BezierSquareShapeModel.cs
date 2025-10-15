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
    [Reactive] private List<Point> _endPoint = new();
    
    
    // свойство для отслеживания, что фигура в процессе размещения
    [Reactive] private bool _isBeingPlaced = true;
    
    public override Geometry Geometry
    {
        get
        {
            var pathGeometry = new PathGeometry();
            var pathFigure = new PathFigure
            {
                StartPoint = StartPoint,
                IsClosed = false
            };
            

            if (IsBeingPlaced)
            {
                if (ControlPoint == null) return pathGeometry;

                for (int i = 0; i < ControlPoint.Count; i++)
                {
                    pathFigure.Segments?.Add(new QuadraticBezierSegment
                    {
                        Point1 = ControlPoint![i],
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
                for (int i = 0; i < ControlPoint!.Count; i++)
                {
                    pathFigure.Segments?.Add(new QuadraticBezierSegment
                    {
                        Point1 = ControlPoint![i],
                        Point2 = EndPoint[i]
                    });
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