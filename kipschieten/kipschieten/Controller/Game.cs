using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace kipschieten.Controller
{
    class Game
    {
        private Boolean _running = false;
        private MainWindow _mainWindow;
        private DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public Game()
        {
            _mainWindow = new MainWindow();


            _mainWindow.Show();
            Run();
        }



        public void Run()
        {
            long beforeTime, timeDiff, sleepTime, frameTime;
            frameTime = 1 / 60;

            beforeTime = (long)(DateTime.UtcNow - _epoch).TotalMilliseconds;
            _running = true;
            while (_running)
            {



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

        }
    }
}
