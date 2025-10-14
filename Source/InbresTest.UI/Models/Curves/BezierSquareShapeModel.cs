using System;
using Avalonia;
using Avalonia.Media;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace InbresTest.Models.Curves;

public class BezierSquareShapeModel: ShapeBaseModel
{
    [Reactive] public Point StartPoint { get; set; } = new Point(0, 0);
    [Reactive] public Point ControlPoint { get; set; } = new Point(50, 100); // Пример смещения
    [Reactive] public Point EndPoint { get; set; } = new Point(100, 0); 

    
    
    // свойство для отслеживания, что фигура в процессе размещения
    [Reactive] public bool IsBeingPlaced { get; set; } = true;
    
    public override Geometry Geometry
    {
        get
        {
            var pathGeometry = new PathGeometry();
            var pathFigure = new PathFigure { StartPoint = StartPoint };
            pathFigure.IsClosed = false;

            if (IsBeingPlaced)
            {
                // 1. AwaitingStartPoint
                if (ControlPoint.X == 50 && ControlPoint.Y == 100 && EndPoint.X == 100 && EndPoint.Y == 0)
                {
                     // Рисуем только начальную точку A.
                     // Возвращаем пустую геометрию, пока нет контрольной точки, чтобы избежать артефактов
                     return pathGeometry;
                }
                
                // 2. AwaitingControlPoint
                if (EndPoint.X == 100 && EndPoint.Y == 0) 
                {
                    // Рисуем линию от StartPoint до ControlPoint
                    pathFigure.Segments?.Add(new LineSegment { Point = ControlPoint });
                }
                
                // 3. AwaitingEndPoint 
                else 
                {
                    pathFigure.Segments?.Add(new QuadraticBezierSegment
                    {
                        Point1 = ControlPoint,
                        Point2 = EndPoint
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