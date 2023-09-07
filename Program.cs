using System;
using SplashKitSDK;

namespace RobotDodge
{
    public class Program
    {
        public static void Main()
        {
            Window gameWindow = new Window("Robot Dodge", 800, 600);
            RobotDodge robotDodge = new RobotDodge(gameWindow);
            do
            {
                SplashKit.ProcessEvents();
                robotDodge.HandleInput();
                robotDodge.Update();
                robotDodge.Draw();
            } 
            while (!robotDodge.Quit && !gameWindow.CloseRequested);
            robotDodge.GameOver();
            gameWindow.Close();
        }  
    }
}