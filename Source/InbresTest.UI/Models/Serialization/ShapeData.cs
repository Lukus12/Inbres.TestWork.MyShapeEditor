using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InbresTest.Models.Serialization;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(List<ShapeData>))]
[JsonSerializable(typeof(PrimitiveShapeData))]
[JsonSerializable(typeof(BezierShapeData))]
public partial class ShapeJsonContext : JsonSerializerContext
{
}

[JsonDerivedType(typeof(PrimitiveShapeData), typeDiscriminator: "primitive")]
[JsonDerivedType(typeof(BezierShapeData), typeDiscriminator: "bezier")]
public abstract class ShapeData
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public string? Fill { get; set; }
    public string TypeDiscriminator { get; set; }
}

public class PrimitiveShapeData : ShapeData
{
}


public class BezierShapeData : ShapeData
{
    public PointDto StartPoint { get; set; }
    public List<PointDto> ControlPoints { get; set; } = new();
    public List<PointDto> EndPoints { get; set; } = new();
}