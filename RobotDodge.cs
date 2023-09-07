using System;
using SplashKitSDK;
using System.Collections.Generic;

namespace RobotDodge;

public class RobotDodge
{
    private Player _player;
    private Window _gameWindow;
    private List<Robot> _robots = new List<Robot>();
    //timer and score below
    private SplashKitSDK.Timer score { get; set; }

    public bool Quit
    {
        get { return _player.Quit; }
    }

    public RobotDodge(Window gameWindow)
    {
        _gameWindow = gameWindow;

        Player player = new Player(gameWindow);
        _player = player;

        //score
        score = new SplashKitSDK.Timer("Score");
        score.Start();
    }

    public void HandleInput()
    {
        _player.HandleInput();
        _player.StayOnWindow(_gameWindow);
    }

    public void Draw()
    {
        _gameWindow.Clear(Color.White);

        foreach (Robot robot in _robots)
        {
            robot.Draw();
        }

        if (_player.bullet != null)
        {
            _player.bullet.Draw();
        }

        _player.Draw();

        //score
        SplashKit.DrawText($"SCORE: {score.Ticks / 1000}", Color.Black, 720, 15);

        _gameWindow.Refresh(60);
    }

    public void Update()
    {
        foreach (Robot robot in _robots)
        {
            robot.Update();
        }

        if (SplashKit.Rnd() < 0.05) //5% probabliity of robot spawning
        {
            _robots.Add(RandomRobot());
        }

        if (_player.bullet != null)
        {
            _player.bullet.Update();
        }
        LoseLife(); //this has to be above checkcollisions because it checks from the same list, check collisions removes the robot from the list if it collides so if its below then it cant check collision on the same robot again.
        CheckCollisions();
    }

    public Robot RandomRobot()
    {
        if (SplashKit.Rnd() < 0.33)
        {
            Robot robot = new Boxy(_gameWindow, _player);
            return robot;
        }
        if (SplashKit.Rnd() < 0.67)
        {
            Robot robot = new Roundy(_gameWindow, _player);
            return robot;
        }
        else
        {
            Robot robot = new Pointy(_gameWindow, _player);
            return robot;
        }
    }

    private void CheckCollisions()
    {
        List<Robot> removeRobots = new List<Robot>();
        foreach (Robot robot in _robots)
        {
            if (_player.CollidedWith(robot) || robot.isOffscreen(_gameWindow))
            {
                removeRobots.Add(robot);
            }
            if (_player.bullet != null && _player.bullet.CollidedWith(robot))
            {
                removeRobots.Add(robot);
                _player.RemoveBullet();
            }
        }
        foreach (Robot robot in removeRobots)
        {
            _robots.Remove(robot);
        }
    }

    private void LoseLife() //only works if lostlife method is on top of check collisions method, because it runs over the same list, if the robot gets removed then it can't check another collision
    {
        foreach (Robot r in _robots)
        {
            if (_player.CollidedWith(r))
            {
                _player.numberOfLives -= 1;
            }
        }
        if (_player.numberOfLives < 1)
        {
            _player.Quit = true;
        }
    }
    public void GameOver()
    {
        SplashKit.FillRectangle(Color.Red, 0, 0, _gameWindow.Width, _gameWindow.Height);
        SplashKit.DrawText("GAME OVER", Color.White, 350, 250);
        SplashKit.DrawText($"Your score was {score.Ticks / 1000}", Color.White, 320, 300);
        _gameWindow.Refresh(60);
        SplashKit.Delay(5000);
    }
}