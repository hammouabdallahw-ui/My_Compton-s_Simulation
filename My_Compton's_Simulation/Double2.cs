using System.Numerics;

public struct Double2
{
    double doubleX { get; }
    double doubleY { get; }

    // Constructor
    public Double2(double DoubleX, double DoubleY)
    {
        doubleX = DoubleX;
        doubleY = DoubleY;
    }

    // operations
    public static Double2 operator *(Double2 d1, double d2) => new Double2(d1.doubleX * d2, d1.doubleY * d2);

    public static Double2 operator /(Double2 d1, double d2) => new Double2(d1.doubleX / d2, d1.doubleY / d2);

    public static Double2 operator +(Double2 d1, Double2 d2) => new Double2(d1.doubleX + d2.doubleX, d1.doubleY + d2.doubleY);

    // convertions
    public static explicit operator Vector2(Double2 double2) => new Vector2((float)double2.doubleX, (float)double2.doubleY);

    // propreities
    public static double Distance (Double2 d1, Double2 d2)
    {
        double a, o, h;
        a = d1.doubleX - d2.doubleX;
        o = d1.doubleY - d2.doubleY;
        h = Math.Sqrt(a * a - o * o);
        return h;
    }
}