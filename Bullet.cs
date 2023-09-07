using System;
using SplashKitSDK;

namespace RobotDodge;

public class Bullet
{
    private Bitmap _bulletBitmap;
    public double X { get; set; }
    public double Y { get; set; }
    public Vector2D Velocity { get; set; }

    public Bullet(double _X, double _Y)
    {
        _bulletBitmap = new Bitmap("bullet", "bullet.png");

        X = _X;
        Y = _Y;

        const double SPEED = 10;
        Point2D fromPt = new Point2D()
        {
            X = X,
            Y = Y
        };

        Point2D toPt = SplashKit.MousePosition();

        Vector2D dir;
        dir = SplashKit.UnitVector(SplashKit.VectorPointToPoint(fromPt, toPt));

        Velocity = SplashKit.VectorMultiply(dir, SPEED);
    }
    public void Update()
    {
        X = X + Velocity.X;
        Y = Y + Velocity.Y;
    }
    public void Draw()
    {
        SplashKit.DrawBitmap(_bulletBitmap, X, Y);
    }

    public bool CollidedWith(Robot other)
    {
        return _bulletBitmap.CircleCollision(X, Y, other.CollisionCircle);
    }
}