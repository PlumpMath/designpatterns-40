using kipschieten.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace kipschieten.Model
{
    class Manager
    {
        private static int max_chickens = 6;
        private Random _random = new Random();
        private int _xGridSize;
        private int _yGridSize;
        private int _xMinBounds;
        private int _xMaxBounds;
        private int _yMinBounds;
        private int _yMaxBounds;
        private int count = 0;

        private Player _player;
        private List<Chicken> _chickens;
        private Grid _playGrid;
        private static String imgChickens = @"C:\Users\stefan\Documents\SourceTree\DesignPatterns\kipschieten\kipschieten\View\kip.png";
        public bool GameOver = false;

        public Manager(Grid playGrid)
        {
            _playGrid = playGrid;

            _yGridSize = (int)_playGrid.ActualHeight;
            _xGridSize = (int)_playGrid.ActualWidth;

            _xMaxBounds = _xGridSize - 100;
            _xMinBounds = 100;

            _yMaxBounds = _yGridSize - 100;
            _yMinBounds = 100;

            initialize();
        }

        private void initialize()
        {
            _player = new Player();
            _chickens = new List<Chicken>();

            // random amount chickens
            int startAmount = _random.Next(4);
            for (int x = 0; x < startAmount; x++)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Chicken chicken = new Chicken();

                    // set location
                    // random based on x and y bounds
                    double xPos, yPos;
                    xPos = (double)_random.Next(_xMinBounds, _xMaxBounds);
                    yPos = (double)_random.Next(_yMinBounds, _yMaxBounds);
                    chicken.setLocation(xPos, yPos);

                    _chickens.Add(chicken);
                    _playGrid.Children.Add(chicken);
                });
                
            }
        }

        public void Update()
        {
            updatePlayer();
            updateChickens();
            updateRender();
        }

        private void updateChickens()
        {
            foreach(Chicken chicken in _chickens)
            {
                
                if (chicken.isShot || chicken.yLocation > _yGridSize || chicken.xLocation > _xGridSize)
                    _chickens.Remove(chicken);

                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                   chicken.Move();
                   //chicken.Margin = new Thickness(300, 300, 200, 100);
                }));
            }

            int randomNum = _random.Next(10);
            if (randomNum % 2 == 0 && _chickens.Count < max_chickens)
            {
                Chicken chicken = new Chicken();

                // set location
                double xPos, yPos;
                xPos = (double)_random.Next(_xMinBounds, _xMaxBounds);
                yPos = (double)_random.Next(_yMinBounds, _yMaxBounds);
                chicken.setLocation(xPos, yPos);

                _chickens.Add(chicken);
            }
        }

        private void updatePlayer()
        {
            foreach(Chicken chicken in _chickens)
            {
                if (chicken.isShot)
                    _player.addPoint();
            }

            if (_player.score == 20)
            {
                GameOver = true;
            }

        }

        private void updateRender()
        {
            // somethings needs to be changed with the dispatchers
            // the clear works fine but adding children throws error

            Application.Current.Dispatcher.Invoke(() => {
                _playGrid.Children.Clear();
            });
            Thread.Sleep(10);
            foreach(Chicken chicken in _chickens)
            {
                Application.Current.Dispatcher.Invoke(() => {
                    var img = new Image
                    {
                        Source =
                            new BitmapImage(
                            new Uri(imgChickens))
                    };
                    img.Width = 120;
                    _playGrid.Children.Add(img);
                });
                
            }
        }
    }
}
