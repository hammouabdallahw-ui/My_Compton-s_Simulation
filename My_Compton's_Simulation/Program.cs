using static Raylib_cs.Raylib;
using Raylib_cs;
using System.Numerics;


class Program
{
    // ______ Defining Variables ______
    const double plankConst = 6.62606896E-34;
    static double timeScaler = 7.5E-7;
    static double distanceScaler = 1E-12;

    static VectorInt2 graphicsOrigin = new VectorInt2(160, 450);

    // ______ Graphics Variables ______
    static bool simulationStarted;
    static VectorInt2 windowSize = new VectorInt2(1600, 900);
    static Vector2 canvasSize = new Vector2(320, 900);
    static Vector2 canvasPos = new Vector2(1280, 0);

    // ______ UI Variables ______
    static Button startSimulationButton = new Button(canvasSize, canvasPos, "Start simulation", 0);
    static Button resetSimulationButton = new Button(canvasSize, canvasPos, "Reset simulation", 1);
    static InputField wavelengthInputField = new InputField(canvasSize, canvasPos, "waveLength", "Set the Wavelength of your photon", 2);
    static InputField thetaInputField = new InputField(canvasSize, canvasPos, "angle", "Set the angle of scattering", 3);

    static void Main(string[] arg)
    {
        // ______ Defining Variables ______
        double creticalDist = 2;

        double theta = Math.PI / 4;


        Photon photon = new Photon(1.5E-12, new Double2(0, 0), new Double2(1, 0));
        Electron electron = new Electron(new Double2(windowSize.X / 1.75f, 0), new Double2(0, 0));


        // ______ Initializing Raylib Window ______
        SetConfigFlags(ConfigFlags.ResizableWindow);

        InitWindow(windowSize.X, windowSize.Y, "Compron's Simulation");
        SetTargetFPS(60);

        // ______ Start Simulation Loop ______
        while (!WindowShouldClose())
        {
            // Fix Resizeing Window
            if (IsWindowResized())
            {
                windowSize = new VectorInt2(GetScreenWidth(), GetScreenHeight());

                graphicsOrigin = new VectorInt2(windowSize.X / 10, windowSize.Y / 2);
                canvasSize = new Vector2(windowSize.X / 5, windowSize.Y);
                canvasPos = new Vector2(windowSize.X - canvasSize.X, 0);

                startSimulationButton = new Button(canvasSize, canvasPos, "Start simulation", 0);
                resetSimulationButton = new Button(canvasSize, canvasPos, "Reset simulation", 1);
                wavelengthInputField = new InputField(canvasSize, canvasPos, "waveLength", "Set the Wavelength of your photon", 2);
                thetaInputField = new InputField(canvasSize, canvasPos, "angle", "Set the angle of scattering", 3);

            }


            // ______ Begin Logic ______
            if (simulationStarted)
            {
                double dt = GetFrameTime() * timeScaler;

                photon.Move(dt);
                electron.Move(dt);

                if (Double2.Distance(photon.Position, electron.Position) < creticalDist) (photon, electron) = Collision(photon, electron, theta);

            }
            //  ____ Input System ____
            if (startSimulationButton.IsPressed())
            {
                simulationStarted = !simulationStarted;
                if (!simulationStarted) startSimulationButton = new Button(canvasSize, canvasPos, "Resume simulation", 0);
                else if (simulationStarted) startSimulationButton = new Button(canvasSize, canvasPos, "Pause simulation", 0);
            }

            if (resetSimulationButton.IsPressed())
            {
                photon = new Photon(1.5E-12, new Double2(0, 0), new Double2(1, 0));
                electron = new Electron(new Double2(windowSize.X / 1.55f, 0), new Double2(0, 0));
            }

            if (thetaInputField.haveBeenPressed) theta = thetaInputField.EnterInputs(Math.PI / 180);
            if (!thetaInputField.IsPressed()
                && thetaInputField.Value != "Set the angle of scattering"
                && !thetaInputField.Value.Contains(" °")
            ) thetaInputField.Value += " °";

            if (wavelengthInputField.haveBeenPressed) photon.WaveLength = wavelengthInputField.EnterInputs(distanceScaler);
            if (!wavelengthInputField.IsPressed()
                && wavelengthInputField.Value != "Set the Wavelength of your photon"
                && !wavelengthInputField.Value.Contains(" pm")
            ) wavelengthInputField.Value += " pm";
            //
            // ______ End Logic ______



            // ______ Begin Drawing ______
            BeginDrawing();
            ClearBackground(Color.Black);

            DrawCircleV(photon.RenderPos(graphicsOrigin), 6, Color.Yellow);
            DrawCircleV(electron.RenderPos(graphicsOrigin), 6, Color.Red);


            // Draw the input system
            DrawRectangleV(canvasPos, canvasSize, Color.Gray);
            startSimulationButton.Draw();
            resetSimulationButton.Draw();
            wavelengthInputField.Draw();
            thetaInputField.Draw();
            //

            EndDrawing();
            // ______ End Drawing ______
        }

        CloseWindow();
    }

    private static (Photon newPhoton, Electron newElectron) Collision(Photon p, Electron e, double theta)
    {
        // Compton's low
        // Calculate the new energy for the photon 
        double oldphotonEnergy = plankConst * p.Speed / p.WaveLength;
        double newPhotonWaveLength = p.WaveLength + ((1 - Math.Cos(theta)) * plankConst / (e.Mass * p.Speed));
        double newphotonEnergy = plankConst * p.Speed / newPhotonWaveLength;

        // Calculate the new Velocity for the electron

        // Calculate the electron's scattering angle (psi)
        double psi = Math.Atan2(-p.WaveLength * Math.Sin(theta), newPhotonWaveLength - p.WaveLength * Math.Cos(theta));

        // Calculate Lorenz Factor \(\gamma \)
        // \(\gamma \) = 1 + E_lost / (m_e * c^2)
        double LorenzFactor = 1 + (oldphotonEnergy - newphotonEnergy) / (e.Mass * Math.Pow(p.Speed, 2));

        // Calcuate scattered Electron's Velocity v
        // v = c * sqrt(1 - (1 / \(\gamma \)^2))
        double VelocityMagnitude = p.Speed * Math.Sqrt(1 - (1 / Math.Pow(LorenzFactor, 2)));
        Double2 newElectronVelocity = new Double2(
            VelocityMagnitude * Math.Cos(psi),
            VelocityMagnitude * Math.Sin(psi)
        );

        Double2 newPhotonDirection = new Double2(
            Math.Cos(theta),
            Math.Sin(theta)
        );

        return (
            new Photon(newPhotonWaveLength, p.Position, newPhotonDirection),
            new Electron(e.Position, newElectronVelocity)
        );
    }

}

// ______ Done ______
// last thing I did is making button body relative to the canvas size wich is also relative to the windowSize
// I need to insure that this future is working... tomorrow ان شاء الله

// I'm tring to make an input field that allow user to input a number
// The problem is the value of the "photon.WaveLength" variable does not change

// I finally got the Input field to work, with a room for improvements (like proffitional cursor & color changing when hovering)
// Now I shall proceed to make input fields for other parameters like theta