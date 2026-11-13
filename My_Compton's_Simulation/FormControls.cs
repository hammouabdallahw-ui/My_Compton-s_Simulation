using static Raylib_cs.Raylib;
using Raylib_cs;
using System.Numerics;
using System.Text;

public struct Button
{
    Vector2 BodySize;
    Vector2 BodyPos;
    byte[] ValueInBytes { get; }
    public Rectangle Body { get; private set; }

    // constructor
    public Button(Vector2 canvasSize, Vector2 canvasPos, string text, int count)
    {
        ValueInBytes = Encoding.UTF8.GetBytes(text + '\0');

        BodySize = new Vector2(
            canvasSize.X - canvasSize.X / 120,
            canvasSize.Y / 7.5f
        );
        BodyPos = new Vector2(
            canvasPos.X + (canvasSize.X - BodySize.X) / 2,
            ((count + 1) * 25) + (BodySize.Y * count)
        );

        Body = new Rectangle(
            BodyPos,
            BodySize
        );
    }

    public void Draw()
    {
        // ______ Draw Button Body ______
        DrawRectangleRec(Body, Color.LightGray);

        // ______ Draw Button Text ______
        Font font = GetFontDefault();
        float fontSize = 35.0f;
        unsafe
        {
            fixed (byte* ValuePointer = ValueInBytes)
            {
                sbyte* ValueInSbytes = (sbyte*)ValuePointer;
                Vector2 textSize = MeasureTextEx(font, ValueInSbytes, fontSize, 2.0f);
                if (textSize.X > BodySize.X - 30)
                {
                    fontSize *= (BodySize.X - 30) / textSize.X;
                }
                DrawText(ValueInSbytes, (int)BodyPos.X + 10, (int)(BodyPos.Y + (BodySize.Y / 2) - (fontSize / 2)), (int)fontSize, Color.Black);
            }
        }
    }

    public bool IsPressed()
    {
        if (!IsMouseButtonPressed(MouseButton.Left)) return false;
        else
        {
            return CheckCollisionPointRec(
                GetMousePosition(),
                Body
            );
        }
    }
}

public struct InputField
{
    // ______ Body ______
    Rectangle Body;
    Vector2 BodySize;
    Vector2 BodyPos;

    // ______ Title ______
    string Title;

    // ______ Value ______
    public string Value;
    StringBuilder stringBuilder = new();

    // ______ enteraction ______
    public bool haveBeenPressed { get; private set; }

    // ______ Constructor ______
    public InputField(Vector2 canvasSize, Vector2 canvasPos, string title, string value, int count)
    {
        Value = value;
        Title = title;

        BodySize = new Vector2(
            canvasSize.X - canvasSize.X / 120,
            canvasSize.Y / 7.5f
        );
        BodyPos = new Vector2(
            canvasPos.X + (canvasSize.X - BodySize.X) / 2,
            ((count + 1) * 25) + (BodySize.Y * count)
        );

        Body = new Rectangle(
            BodyPos,
            BodySize
        );
    }

    public void Draw()
    {
        // ______ Draw Button Body ______
        DrawRectangleRec(Body, Color.LightGray);

        // ______ Draw Button Text ______
        Font font = GetFontDefault();
        float TitleFontSize = 18.0f;
        byte[]? TitleInBytes;
        float ValueFontSize = 35.0f;
        byte[]? ValueInBytes;
        unsafe
        {
            ValueInBytes = Encoding.UTF8.GetBytes(Value);
            TitleInBytes = Encoding.UTF8.GetBytes(Title);

            // ______ Draw Value ______
            fixed (byte* ValuePointer = ValueInBytes)
            {
                sbyte* ValueInSbytes = (sbyte*)ValuePointer;

                Vector2 textSize = MeasureTextEx(font, ValueInSbytes, ValueFontSize, 2.0f);
                if (textSize.X > BodySize.X - 30) ValueFontSize *= (BodySize.X - 30) / textSize.X;

                DrawText(ValueInSbytes, (int)BodyPos.X + 10, (int)(BodyPos.Y + (BodySize.Y / 2) - (ValueFontSize / 2)), (int)ValueFontSize, Color.Black);
            }
            // ______ Draw Title ______
            fixed (byte* TitlePointer = TitleInBytes)
            {
                sbyte* TitleInSbytes = (sbyte*)TitlePointer;

                Vector2 textSize = MeasureTextEx(font, TitleInSbytes, TitleFontSize, 2.0f);
                if (textSize.X > BodySize.X - 30) TitleFontSize *= (BodySize.X - 30) / textSize.X;

                DrawText(TitleInSbytes, (int)BodyPos.X + 5, (int)(BodyPos.Y - TitleFontSize - 2), (int)TitleFontSize, Color.Black);
            }
        }
    }

    public bool IsPressed()
    {
        bool isLeftClicked = IsMouseButtonPressed(MouseButton.Left);
        bool isHovering = CheckCollisionPointRec(GetMousePosition(), Body);

        if (isLeftClicked && isHovering)
        {
            haveBeenPressed = true;
            return true;
        }
        else if ((isLeftClicked && !isHovering) || IsKeyPressed(KeyboardKey.Enter))
        {
            haveBeenPressed = false;
            return false;
        }
        else return false;
    }

    public double EnterInputs(double distanceScaler)
    {
        // ______ Variables ______
        double valueInDouble;
        int key = GetCharPressed();
        int maxCharacters = 10;
        bool isNumber = key >= '0' && key <= '9';
        bool isDecimal = key == '.';


        // ______ Entering Input Nums ______
        if ((isNumber || isDecimal) && stringBuilder.Length <= maxCharacters)
        {
            if (!(isDecimal && stringBuilder.ToString().Contains('.')))
            {
                stringBuilder.Append((char)key);
            }
        }
        // ______ Enable Deleting ______
        if (IsKeyPressed(KeyboardKey.Backspace) && stringBuilder.Length > 0) stringBuilder.Length--;

        // ______ Assign Graphics Value ______
        this.Value = stringBuilder.ToString();

        // ______ Assign Physics Value ______
        if (stringBuilder.Length == 0 || stringBuilder.ToString() == ".") valueInDouble = 0.0;
        else
        {
            double.TryParse(stringBuilder.ToString(), out valueInDouble);
            valueInDouble *= distanceScaler;
        }
        return valueInDouble;

    }
}

public struct CheckBox
{
    Rectangle Body;
    Vector2 BodySize;
    Vector2 BodyPos;
    public string Title;
    StringBuilder stringBuilder = new();
    public bool wasPressed { get; private set; } = false;

    // Constructor
    public CheckBox(Vector2 canvasSize, Vector2 canvasPos, string title, int count)
    {
        Title = title;

        BodySize = new Vector2(
            canvasSize.X / 12,
            canvasSize.X / 12
        );
        BodyPos = new Vector2(
            canvasPos.X + BodySize.X,
            ((count + 1) * 25) + (count * canvasSize.Y / 7.5f)
        );

        Body = new Rectangle(
            BodyPos,
            BodySize
        );
    }

    public void Draw()
    {
        // ______ Draw CheckBox Body ______
        DrawRectangleLinesEx(Body, 5, Color.LightGray);
        if (WasPressed()) DrawRectangleRec(Body, Color.LightGray);

        // ______ Draw CheckBox Text ______
        Font font = GetFontDefault();
        float fontSize = 18.0f;
        byte[]? TitleInBytes;
        unsafe
        {
            TitleInBytes = Encoding.UTF8.GetBytes(Title);
            fixed (byte* TitlePointer = TitleInBytes)
            {
                sbyte* TitleInSbytes = (sbyte*)TitlePointer;
                DrawText(TitleInSbytes, (int)(BodyPos.X + BodySize.X + 10), (int)(BodyPos.Y + (BodySize.Y / 2) - (fontSize / 2)), (int)fontSize, Color.Black);
            }
        }
    }

    public bool WasPressed()
    {
        bool isLeftClicked = IsMouseButtonPressed(MouseButton.Left);
        bool isHovering = CheckCollisionPointRec(GetMousePosition(), Body);

        if (isLeftClicked && isHovering) wasPressed = !wasPressed;
        return wasPressed;
    }
}