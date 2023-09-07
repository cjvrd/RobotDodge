using System;
using SplashKitSDK;

namespace RobotDodge;

public class Player
{
    private Bitmap _playerBitmap;

    public double X { get; set; }
    public double Y { get; set; }
    public bool Quit { get; set; }
    public int Width
    {
        get { return _playerBitmap.Width; }
    }
    public int Height
    {
        get { return _playerBitmap.Height; }
    }
    //lives below
    public int numberOfLives = 5;
    //bullet
    public Bullet? bullet = null;

    public Player(Window gameWindow)
    {
        _playerBitmap = new Bitmap("Player", "Player.png");
        X = (gameWindow.Width) / 2 - (_playerBitmap.Width) / 2; //player spawns in middle of screen
        Y = (gameWindow.Height) / 2 - (_playerBitmap.Height) / 2;
        Quit = false;
    }
    public void Draw()
    {
        _playerBitmap.Draw(X, Y);
        //lives
        Bitmap _lifeBitmap = new Bitmap("Life", "life.png");
        _lifeBitmap.Draw(0, 0);
        SplashKit.DrawText($"x {numberOfLives}", Color.Black, 35, 15);
    }
    public void HandleInput()
    {
        double speed = 5;

        if (SplashKit.KeyDown(KeyCode.WKey))
        {
            Y = Y - speed;
        }
        if (SplashKit.KeyDown(KeyCode.SKey))
        {
            Y = Y + speed;
        }
        if (SplashKit.KeyDown(KeyCode.AKey))
        {
            X = X - speed;
        }
        if (SplashKit.KeyDown(KeyCode.DKey))
        {
            X = X + speed;
        }
        if (SplashKit.KeyDown(KeyCode.EscapeKey))
        {
            Quit = true;
        }
        if (SplashKit.MouseClicked(MouseButton.LeftButton)) //shoot bullet
        {
            bullet = new Bullet(X, Y);
            bullet.Draw();
        }
    }
    public void StayOnWindow(Window gameWindow)
    {
        if (X > gameWindow.Width - _playerBitmap.Width)
        {
            X = gameWindow.Width - _playerBitmap.Width;
        }
        if (X < 0)
        {
            X = 0;
        }
        if (Y > gameWindow.Height - _playerBitmap.Height)
        {
            Y = gameWindow.Height - _playerBitmap.Height;
        }
        if (Y < 0)
        {
            Y = 0;
        }
    }

    public bool CollidedWith(Robot other)
    {
        return _playerBitmap.CircleCollision(X, Y, other.CollisionCircle);
    }

    public void RemoveBullet()
    {
        bullet = null;
    }
}