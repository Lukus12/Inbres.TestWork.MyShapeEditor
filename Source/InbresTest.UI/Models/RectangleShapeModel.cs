using Avalonia;
using Avalonia.Media;

namespace InbresTest.Models;

public class RectangleShapeModel : ShapeBaseModel
{
    public override Geometry Geometry =>
        new RectangleGeometry(new Rect(0, 0, Width, Height));
}