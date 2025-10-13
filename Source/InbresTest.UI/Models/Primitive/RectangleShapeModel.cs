using Avalonia;
using Avalonia.Media;

namespace InbresTest.Models.Primitive;

public class RectangleShapeModel : PrimitiveShapeModel
{
    public override Geometry Geometry  =>
         new RectangleGeometry(new Rect(0, 0, Width, Height));
}