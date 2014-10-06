using kipschieten.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace kipschieten.Model
{
    class Manager
    {
        private const int max_chickens = 10;
        private readonly Random _random = new Random();

        // Grid bounds
        private readonly int _xMinBounds;
        private readonly int _xMaxBounds;
        private readonly int _yMinBounds;
        private readonly int _yMaxBounds;

        //click thing
        private readonly MouseCapture _mouseCapture;

        private Player _player;
        public List<Unit> _units;
        private readonly GameCanvas _playGrid;
        
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
            _units      = new List<Unit>();

            // random amount chickens
            int startAmountUnits = _random.Next(4);
            for (int x = 0; x < startAmountUnits; x++)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // set location
                    // random based on x and y bounds
                    var xPos = (double)_random.Next(_xMinBounds, _xMaxBounds);
                    var yPos = (double)_random.Next(_yMinBounds, _yMaxBounds);

                    // Set random unit on screen
                    var unitType = (UnitEnum)_random.Next(1, 3);
                    Unit unit = UnitFactory.CreateUnit(unitType, xPos, yPos);
                    _units.Add(unit);
                });
            }

            // set trees
            int startAmountTrees = 5;
            for(int t = 0; t < startAmountTrees; t++) {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // set location
                    // random based on x and y bounds
                    var xPos = (double)_random.Next(_xMinBounds, _xMaxBounds);
                    var yPos = (double)_random.Next(_yMinBounds, _yMaxBounds);

                    // Set random unit on screen
                    Unit unit = UnitFactory.CreateUnit(UnitEnum.Tree, xPos, yPos);
                    _units.Add(unit);
                });
            }
        }

        public void Update()
        {
            updateUnits();
            updatePlayer();
            updateRender();
        }

        private void updateUnits()
        {
            Dictionary<double, double> clickedLocations = _mouseCapture.getClicks();

            for (int i = _units.Count-1; i >= 0; i--)
            {
                foreach (KeyValuePair<double, double> coords in clickedLocations.Where(coords => _units[i] is MovingUnit))
                {
                    ((MovingUnit) _units[i]).SetIsShot(coords);
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

                    for (int i = 0; i < _units.Count - 1; i++)
                    {
                        if (_units[i].GetType() == typeof(Tree))
                        {

                            //For left bounds
                            if ((unit.LeftPosition + 50 >= _units[i].LeftPosition && unit.LeftPosition + 50 <= _units[i].LeftPosition + 50) &&
                                (unit.TopPosition + 50 >= _units[i].TopPosition && unit.TopPosition <= _units[i].TopPosition)
                               )
                            {
                                MethodInfo methodInfo = unit.GetType().GetMethod("ChangePosition");
                                methodInfo.Invoke(unit, null);
                            }

                            if((unit.LeftPosition <= _units[i].LeftPosition + 50 && unit.LeftPosition + 50 >= _units[i].LeftPosition) && 
                               (unit.TopPosition + 50 >= _units[i].TopPosition && unit.TopPosition <= _units[i].TopPosition)
                              )
                            {
                                MethodInfo methodInfo = unit.GetType().GetMethod("ChangePosition");
                                methodInfo.Invoke(unit, null);
                            }
                        }
                    }
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
                // check for spawn on a tree
                foreach (var unit1 in _units.OfType<Tree>().Where(unit1 => xPos >= unit1.LeftPosition && xPos <= (unit1.LeftPosition + 50) && yPos >= unit1.TopPosition &&
                                                                                          yPos <= (unit1.TopPosition + 50)))
                {
                    unit = UnitFactory.CreateUnit(unitType, xPos + 60, yPos + 60);
                }
                _units.Add(unit);
            }
        }

        private void updatePlayer()
        {
            foreach (MovingUnit unit in _units.OfType<MovingUnit>().Where(unit => unit.isShot))
            {
                _player.addPoint();
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
                    if (_units[i].CanBeShot && _units[i] is MovingUnit)
                    {
                        PropertyInfo pInfo = _units[i].GetType().GetProperty("isShot");
                        if ((bool)pInfo.GetValue(_units[i], null))
                        {
                            _units.Remove(_units[i]);
                            continue;
                        }
                    }
                }

                Application.Current.Dispatcher.Invoke(() => _playGrid.DrawUnits(_units));
            }
            catch (Exception e)
            {
                Environment.Exit(0);
            }
            
        }
    }
}
