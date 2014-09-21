using kipschieten.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace kipschieten.View
{
    class Game
    {
        private static double FPS = 30;

        private Boolean _running = false;
        private MainWindow _mainWindow;
        private DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private Manager _gameManager;

        public Game()
        {
            _mainWindow = new MainWindow();
            _mainWindow.Show();

            startGame(_mainWindow.PlayCanvas);
        }

        private void startGame(GameCanvas playGrid)
        {
            _gameManager = new Manager(playGrid);
            
            // new thread for game stuffs 
            Thread gameThread = new Thread(Run);
            gameThread.SetApartmentState(ApartmentState.STA);
            gameThread.Start();
        }

        public void Run()
        {
            double beforeTime, timeDiff, sleepTime, frameTime;
            frameTime = (1 / FPS) * 1000;

            beforeTime = (long)(DateTime.UtcNow - _epoch).TotalMilliseconds;
            _running = true;
            while (_running)
            {
                // Update from gamemanager
                _gameManager.Update();

                // Check gameover
                if (_gameManager.GameOver)
                {
                    GameOver();
                }

                timeDiff = (long)((DateTime.UtcNow - _epoch).TotalMilliseconds - beforeTime);
                sleepTime = frameTime - timeDiff;

                if (sleepTime <= 0)
                    sleepTime = 5;

                try
                {
                    Thread.Sleep(new TimeSpan(0,0,0,0,(int)sleepTime));
                }
                catch (ThreadInterruptedException e) { }

                beforeTime = (long)(DateTime.UtcNow - _epoch).TotalMilliseconds;
            }
        }

        public void GameOver()
        {
            MessageBox.Show("You shot 3 chickens, you win!");
            Environment.Exit(0);
        }
    }
}
