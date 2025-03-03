using System;
using Microsoft.Xna.Framework;

namespace Triangulation;

public class Shapes
{
    public static VertexStructure EquilateralTriangle(Vector2 center, float sideLength)
    {
        float height = sideLength * MathF.Sqrt(3) / 2f;
        return VertexStructure.FromList(
            [
                center + new Vector2(0, -height * 2 / 3f),
                center + new Vector2(-sideLength / 2f, height / 3f),
                center + new Vector2(sideLength / 2f, height / 3f),
            ]
        );
    }

    public static float TriangleArea(Vector2 a, Vector2 b, Vector2 c)
    {
        return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
    }

    /// <summary>
    /// Checks if the point c is on the left side of the line segment between a and b.
    /// </summary>
    /// <param name="a">the start of the line segment</param>
    /// <param name="b">the end of the line segment</param>
    /// <param name="c">the point to check if it is on the left side of the line segment</param>
    /// <returns>true if c is on the left side of the line segment, false otherwise</returns>
    public static bool IsLeft(Vector2 a, Vector2 b, Vector2 c)
    {
        return TriangleArea(a, b, c) > 0;
    }

    /// <summary>
    /// Checks if the point c is on the left side of the line segment between a and b.
    /// </summary>
    /// <param name="a">the start of the line segment</param>
    /// <param name="b">the end of the line segment</param>
    /// <param name="c">the point to check if it is on the left side of the line segment</param>
    /// <returns>true if c is on the left side of the line segment, false otherwise</returns>
    public static bool IsLeftOrOn(Vector2 a, Vector2 b, Vector2 c)
    {
        return TriangleArea(a, b, c) >= 0;
    }

    public static bool Collinear(Vector2 a, Vector2 b, Vector2 c)
    {
        return TriangleArea(a, b, c) == 0;
    }

    /// <summary>
    /// Checks if the point c is between the points a and b.
    /// </summary>
    /// <param name="a">the start of the line segment</param>
    /// <param name="b">the end of the line segment</param>
    /// <param name="c">the point to check if it is between a and b</param>
    /// <returns>true if c is between a and b, false otherwise</returns>
    public static bool Between(Vector2 a, Vector2 b, Vector2 c)
    {
        if (!Collinear(a, b, c))
        {
            return false;
        }

        if (a.X != b.X)
        {
            return a.X <= c.X && c.X <= b.X || a.X >= c.X && c.X >= b.X;
        }
        else
        {
            return a.Y <= c.Y && c.Y <= b.Y || a.Y >= c.Y && c.Y >= b.Y;
        }
    }

    /// <summary>
    /// Checks if the line segment between a and b intersects with the line segment between c and d.
    /// </summary>
    /// <param name="a">the start of the first line segment</param>
    /// <param name="b">the end of the first line segment</param>
    /// <param name="c">the start of the second line segment</param>
    /// <param name="d">the end of the second line segment</param>
    /// <returns>true if the line segments intersect, false otherwise</returns>
    public static bool Intersects(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        if (IntersectsProperly(a, b, c, d))
        {
            return true;
        }

        // if c or d lies on the line segment between a and b, then the line segments intersect
        // if a or b lies on the line segment between c and d, then the line segments intersect
        return Between(a, b, c) || Between(a, b, d) || Between(c, d, a) || Between(c, d, b);
    }

    /// <summary>
    /// Checks if the line segment between a and b intersects with the line segment between c and d.
    /// </summary>
    /// <param name="a">the start of the first line segment</param>
    /// <param name="b">the end of the first line segment</param>
    /// <param name="c">the start of the second line segment</param>
    /// <param name="d">the end of the second line segment</param>
    /// <returns>true if the line segments intersect, false otherwise</returns>
    public static bool IntersectsProperly(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        if (Collinear(a, b, c) || Collinear(a, b, d) || Collinear(c, d, a) || Collinear(c, d, b))
        {
            return false;
        }

        return IsLeft(a, b, c) != IsLeft(a, b, d) && IsLeft(c, d, a) != IsLeft(c, d, b);
    }

    public static bool InAngle(Vector2 a, Vector2 b, Vector2 c, Vector2 x)
    {
        if (AngleIsConvex(a, b, c)) {
            return Shapes.IsLeft(b, x, a) && Shapes.IsLeft(x, b, c);
        }

        return !(Shapes.IsLeftOrOn(b, x, c) && Shapes.IsLeftOrOn(x, b, a));
    }

    public static bool AngleIsConvex(Vector2 a, Vector2 b, Vector2 c)
    {
        return Shapes.IsLeftOrOn(a, b, c);
    }
}
