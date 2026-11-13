using System.Numerics;

public struct Photon
{
    public double Speed {get;} = 299792458.0;
    public double WaveLength {get; set;}
    public Double2 Position {get; private set;}
    public Double2 Direction {get;}

    // Constractor
    public Photon (double waveLength, Double2 position, Double2 direction)
    {
        WaveLength = waveLength;
        Position = position;
        Direction = direction;
    }

    // Send Position to the renderer
    public Vector2 RenderPos (VectorInt2 graphicsOrigin) => (Vector2) Position + graphicsOrigin;

    // Create next photon & assign new pos to it
    public void Move (double deltaTime)
    {
        double Distance = Speed * deltaTime;
        Position += Direction * Distance;
    }
}
