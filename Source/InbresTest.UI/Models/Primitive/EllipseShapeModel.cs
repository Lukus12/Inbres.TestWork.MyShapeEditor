using Avalonia;
using Avalonia.Media;

namespace InbresTest.Models.Primitive;

public class EllipseShapeModel: PrimitiveShapeModel
{
    public override Geometry Geometry => 
        new EllipseGeometry(new Rect(0, 0, Width, Height));
}