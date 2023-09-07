using System;
using SplashKitSDK;

namespace RobotDodge;

public abstract class Robot
{
    public double X { get; set; }
    public double Y { get; set; }
    public Color MainColor { get; set; }
    public Vector2D Velocity { get; set; }
    public int Width
    {
        get { return 50; }
    }
    public int Height
    {
        get { return 50; }
    }
    public Circle CollisionCircle
    {
        get { return SplashKit.CircleAt(X + (Width / 2), Y + (Height / 2), 20); } //circle is in middle of robot
    }

    public Robot(Window gameWindow, Player player)
    {
        MainColor = Color.RandomRGB(200);

        if (SplashKit.Rnd() < 0.5)
        {
            X = SplashKit.Rnd(gameWindow.Width);

            if (SplashKit.Rnd() < 0.5) //50% of time robot spawns above or below screen
            {
                Y = -Height; //above
            }
            else
            {
                Y = gameWindow.Height; //below
            }
        }
        else
        {
            Y = SplashKit.Rnd(gameWindow.Height);

            if (SplashKit.Rnd() > 0.5) //50% of time robot spawns outside screen (left/right)
            {
                X = -Width; //left
            }
            else
            {
                X = gameWindow.Width; //right
            }
        }

        // velocity below
        const int SPEED = 4;

        Point2D fromPt = new Point2D()
        {
            X = X,
            Y = Y
        };

        Point2D toPt = new Point2D()
        {
            X = player.X,
            Y = player.Y
        };

        Vector2D dir;
        dir = SplashKit.UnitVector(SplashKit.VectorPointToPoint(fromPt, toPt));

        Velocity = SplashKit.VectorMultiply(dir, SPEED);
    }

    public abstract void Draw();

    public void Update()
    {
        X = X + Velocity.X;
        Y = Y + Velocity.Y;
    }

    public bool isOffscreen(Window screen)
    {
        return
        (
        X < -Width ||
        X > screen.Width ||
        Y < -Height ||
        Y > screen.Height
        );
    }

}

public class Boxy : Robot
{
    public override void Draw()
    {
        double leftX, rightX, eyeY, mouthY;
        leftX = X + 12;
        rightX = X + 27;
        eyeY = Y + 10;
        mouthY = Y + 30;

        SplashKit.FillRectangle(Color.Gray, X, Y, 50, 50);
        SplashKit.FillRectangle(MainColor, leftX, eyeY, 10, 10);
        SplashKit.FillRectangle(MainColor, rightX, eyeY, 10, 10);
        SplashKit.FillRectangle(MainColor, leftX, mouthY, 25, 10);
        SplashKit.FillRectangle(MainColor, leftX + 2, mouthY + 2, 21, 6);
    }
    public Boxy(Window gameWindow, Player player) : base(gameWindow, player)
    { }
}

public class Roundy : Robot
{
    public override void Draw()
    {
        double leftX, midX, rightX, eyeY, midY, mouthY;
        leftX = X + 17;
        midX = X + 25;
        rightX = X + 33;
        eyeY = Y + 20;
        midY = Y + 25;
        mouthY = Y + 35;

        SplashKit.FillCircle(Color.DarkGray, midX, midY, 25);
        SplashKit.DrawCircle(Color.Gray, midX, midY, 25);
        SplashKit.FillCircle(MainColor, leftX, eyeY, 5);
        SplashKit.FillCircle(MainColor, rightX, eyeY, 5);
        SplashKit.FillEllipse(Color.Gray, X, eyeY, 50, 30);
        SplashKit.DrawLine(Color.Black, X, mouthY, X + 50, Y + 35);
    }
    public Roundy(Window gameWindow, Player player) : base(gameWindow, player)
    { }
}

public class Pointy : Robot
{
    public override void Draw() //if i ever feel like fixing this, triangle is upside down, so collision cirlce is outside of the triangle.
    {
        double leftX, topX, rightX, leftY, topY, rightY, eyeY;
        leftX = X + 5;
        topX = X + 40;
        rightX = X + 75;
        leftY = Y + 30;
        topY = Y + 80;
        rightY = Y + 30;
        eyeY = Y + 40;

        SplashKit.FillTriangle(Color.Gray, leftX, leftY, topX, topY, rightX, rightY);
        SplashKit.FillCircle(MainColor, leftX+20, eyeY, 5);
        SplashKit.FillCircle(MainColor, rightX-20, eyeY, 5);
    }
    public Pointy(Window gameWindow, Player player) : base(gameWindow, player)
    { }
}