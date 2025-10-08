using Avalonia;
using Avalonia.Media;
using ReactiveUI.SourceGenerators;

namespace InbresTest.Models;

public class BezierSquareShapeModel: ShapeBaseModel
{
    [Reactive] public Point StartPoint { get; set; } = new Point(0, 0);
    [Reactive] public Point ControlPoint1 { get; set; } = new Point(50, 100);
    [Reactive] public Point ControlPoint2 { get; set; } = new Point(150, 100);
    [Reactive] public Point EndPoint { get; set; } = new Point(200, 0);

    public override Geometry Geometry
    {
        get
        {
            var pathFigure = new PathFigure { StartPoint = StartPoint };
            pathFigure.Segments.Add(new BezierSegment 
            { 
                Point1 = ControlPoint1, 
                Point2 = ControlPoint2, 
                Point3 = EndPoint 
            });
            
            var pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);
            return pathGeometry;
        }
    }
}