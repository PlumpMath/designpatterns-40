﻿using kipschieten.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private static int max_chickens = 10;
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
        public List<Unit> _units;
        private GameCanvas _playGrid;
        
        public bool GameOver = false;

        public Manager(GameCanvas playGrid)
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
            _units      = new List<Unit>();

            // random amount chickens
            int startAmountChickens = _random.Next(4);
            for (int x = 0; x < startAmountChickens; x++)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // set location
                    // random based on x and y bounds
                    double xPos, yPos;
                    xPos = (double)_random.Next(_xMinBounds, _xMaxBounds);
                    yPos = (double)_random.Next(_yMinBounds, _yMaxBounds);
                    _units.Add(UnitFactory.CreateUnit(UnitEnum.Chicken, xPos, yPos));
                });
            }

            //random amount of trees
            //int startAmountTrees = _random.Next(2);
            //for (int t = 0; t < startAmountChickens; t++)
            //{
            //    Application.Current.Dispatcher.Invoke(() =>
            //    {
            //        // set location
            //        // random based on x and y bounds
            //        double xPos, yPos;
            //        xPos = (double)_random.Next(_xMinBounds, _xMaxBounds);
            //        yPos = (double)_random.Next(_yMinBounds, _yMaxBounds);
            //        _units.Add(new Tree(xPos, yPos));
            //    });
            //}
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

            for (int i = _units.Count-1; i >= 0; i--)
            {
                foreach(KeyValuePair<double, double> coords in clickedLocations)
                {
                    if (_units[i].GetType() == typeof(Chicken) || _units[i].GetType() == typeof(Cow))
                    {
                        PropertyInfo pInfo = _units[i].GetType().GetProperty("isShot");
                        if (coords.Key >= _units[i].LeftPosition && coords.Key <= _units[i].LeftPosition + 50 &&
                        coords.Value >= _units[i].TopPosition && coords.Value <= _units[i].TopPosition + 50)
                        {
                            pInfo.SetValue(_units[i], true);
                        }
                    }
                }

                if (_units[i].TopPosition >= _yMaxBounds - 40 || _units[i].TopPosition <= 0 ||
                    _units[i].LeftPosition >= _xMaxBounds - 40 || _units[i].LeftPosition <= 0)
                    _units.Remove(_units[i]);
            }

            foreach (Unit unit in _units)
            {
                if (unit.CanMove)
                {
                    MethodInfo mInfo = unit.GetType().GetMethod("Move");
                    mInfo.Invoke(unit, null);
                }
            }

            int randomNum = _random.Next(35);
            if (randomNum % 2 == 0 && _units.Count < max_chickens)
            {
                // set location
                double xPos, yPos;
                xPos = (double)_random.Next(_xMinBounds, _xMaxBounds);
                yPos = (double)_random.Next(_yMinBounds, _yMaxBounds);

                UnitEnum unitType = (UnitEnum)_random.Next(1, 3);
                Unit unit = UnitFactory.CreateUnit(unitType, xPos, yPos);
                _units.Add(unit);
            }
        }

        private void updatePlayer()
        {
            foreach (Unit unit in _units)
            {
                PropertyInfo pInfo = unit.GetType().GetProperty("isShot");
                if ((bool)pInfo.GetValue(unit, null))
                {
                    _player.addPoint();
                }
            }

            if (_player.score == 3)
                GameOver = true;
        }

        private void updateRender()
        {
            try
            {
                for (int i = _units.Count - 1; i >= 0; i--)
                {
                    if (_units[i].CanBeShot && (_units[i].GetType() == typeof(Chicken) || _units[i].GetType() == typeof(Cow)))
                    {
                        PropertyInfo pInfo = _units[i].GetType().GetProperty("isShot");
                        if ((bool)pInfo.GetValue(_units[i], null))
                        {
                            _units.Remove(_units[i]);
                            continue;
                        }
                    }
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    _playGrid.DrawUnits(_units);
                });
            }
            catch (Exception e)
            {
                Environment.Exit(0);
            }
            
        }
    }
}
