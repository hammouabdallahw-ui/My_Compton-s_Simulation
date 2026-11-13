using System.Numerics;

public struct Electron
{
    public double Mass {get;} = 9.109E-31;
    public Double2 Position {get; private set;}
    public Double2 Velocity {get;}

    // Constructor
    public Electron (Double2 position, Double2 velocity)
    {
        Position = position;
        Velocity = velocity;
    }

    // Send Position to runderer
    public Vector2 RenderPos(VectorInt2 graphicsOrigin) => (Vector2) Position + graphicsOrigin;

    public void Move(double deltaTime)
    {
        Position += Velocity * deltaTime;
    }
}