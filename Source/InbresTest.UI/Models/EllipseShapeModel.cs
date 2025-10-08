using Avalonia;
using Avalonia.Media;

namespace InbresTest.Models;

public class EllipseShapeModel: ShapeBaseModel
{
    public override Geometry Geometry => 
        new EllipseGeometry(new Rect(0, 0, Width, Height));
}