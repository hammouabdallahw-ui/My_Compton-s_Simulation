using System.Numerics;

public struct VectorInt2
{
    public int X { get; }
    public int Y { get; }
    public VectorInt2(int width, int height)
    {
        X = width;
        Y = height;
    }

    // multiplication
    public static Vector2 operator * (VectorInt2 vectorInt2, Vector2 vector2) => new Vector2(vectorInt2.X * vector2.X, vectorInt2.Y * vector2.Y);
    public static Vector2 operator *(Vector2 vector2, VectorInt2 vectorInt2) => vectorInt2 * vector2;

    // Addition
    public static Vector2 operator + (VectorInt2 vectorInt2, Vector2 vector2) => new Vector2(vectorInt2.X + vector2.X, vectorInt2.Y + vector2.Y);
    public static Vector2 operator +(Vector2 vector2, VectorInt2 vectorInt2) => vectorInt2 + vector2;

    // convertion
    public static implicit operator Vector2 (VectorInt2 vectorInt2) => new Vector2(vectorInt2.X, vectorInt2.Y);
}