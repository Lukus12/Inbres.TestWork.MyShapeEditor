using Avalonia;
using Avalonia.Media;
using ReactiveUI.SourceGenerators;

namespace InbresTest.Models.Curves;

public class BezierSquareShapeModel: ShapeBaseModel
{
    [Reactive] public Point StartPoint { get; set; } = new Point(0, 0);
    [Reactive] public Point ControlPoint { get; set; } = new Point(100, 150);
    [Reactive] public Point EndPoint { get; set; } = new Point(200, 0);
    
    //public override bool IsResizable => false;

    public override Geometry Geometry
    {
        get
        {
            var pathFigure = new PathFigure { StartPoint = StartPoint };
            pathFigure.Segments?.Add(new QuadraticBezierSegment
            {
                Point1 = ControlPoint,
                Point2 = EndPoint
            });
            var pathGeometry = new PathGeometry();
            pathGeometry.Figures?.Add(pathFigure);
            return pathGeometry;
        }
    }
}