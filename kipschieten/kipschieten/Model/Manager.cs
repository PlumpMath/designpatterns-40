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

        // Grid bounds
        private int _xMinBounds;
        private int _xMaxBounds;
        private int _yMinBounds;
        private int _yMaxBounds;

        //click thing
        private MouseCapture _mouseCapture;

        private Player _player;
        private List<Chicken> _chickens;
        private Grid _playGrid;
        
        public bool GameOver = false;

        private double clickedX, clickedY;

        public Manager(Grid playGrid)
        {
            _playGrid       = playGrid;
            _mouseCapture   = new MouseCapture(_playGrid);

            _xMaxBounds = (int)_playGrid.ActualWidth - 10;
            _xMinBounds = 10;

            _yMaxBounds = (int)_playGrid.ActualHeight - 10;
            _yMinBounds = 10;

            initialize();
        }

        private void initialize()
        {
            _player     = new Player();
            _chickens   = new List<Chicken>();

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
                    chicken.setLocation(xPos, -yPos);

                    _chickens.Add(chicken);
                });
            }
        }

        public void Update()
        {
            updateChickens();
            updatePlayer();
            updateRender();
        }

        private void updateChickens()
        {
            Dictionary<double, double> clickedLocations = _mouseCapture.getClicks();

            for (int i = _chickens.Count-1; i >= 0; i--)
            {
                foreach(KeyValuePair<double, double> coords in clickedLocations)
                {
                    if (coords.Key >= _chickens[i].margin.Left && coords.Key <= _chickens[i].margin.Left + 50 &&
                        coords.Value >= _chickens[i].margin.Top && coords.Value <= _chickens[i].margin.Top + 50)
                    {
                        _chickens[i].isShot = true;
                    }
                }

                if (_chickens[i].margin.Top  >= _yMaxBounds - 40 || _chickens[i].margin.Top  <= 0 ||
                    _chickens[i].margin.Left >= _xMaxBounds - 40 || _chickens[i].margin.Left <= 0)
                    _chickens.Remove(_chickens[i]);
            }

            foreach (Chicken chicken in _chickens)
            {
                chicken.Move();
            }

            int randomNum = _random.Next(35);
            if (randomNum % 2 == 0 && _chickens.Count < max_chickens)
            {
                Chicken chicken = new Chicken();

                // set location
                double xPos, yPos;
                xPos = (double)_random.Next(_xMinBounds, _xMaxBounds);
                yPos = (double)_random.Next(_yMinBounds, _yMaxBounds);
                chicken.setLocation(xPos,yPos);

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

            if (_player.score == 3)
                GameOver = true;
        }

        private void updateRender()
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _playGrid.Children.Clear();
                });

                for (int i = _chickens.Count - 1; i >= 0; i--)
                {
                    if (_chickens[i].isShot)
                    {
                        _chickens.Remove(_chickens[i]);
                        continue;
                    }

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        // Add the chicken
                        var img = new Image
                        {
                            Source =
                                new BitmapImage(
                                new Uri(_chickens[i].imgChickens)),
                            Margin = _chickens[i].margin,
                            Width = 50,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment   = VerticalAlignment.Top
                        };
                        _playGrid.Children.Add(img);
                    });

                }
            }
            catch (Exception e)
            {
                Environment.Exit(0);
            }
            
        }
    }
}
