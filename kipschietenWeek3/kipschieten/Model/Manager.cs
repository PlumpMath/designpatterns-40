using kipschieten.Levels;
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
        private const int MaxChickens = 10;
        private readonly Random _random = new Random();

        // Grid bounds
        private readonly int _xMinBounds;
        private readonly int _xMaxBounds;
        private readonly int _yMinBounds;
        private readonly int _yMaxBounds;

        //click thing
        private readonly MouseCapture _mouseCapture;

        private Player _player;
        private List<MovingUnit> _movingUnits;
        private List<Unit> _nonMovingUnits; 
        private readonly GameCanvas _playGrid;
        
        public bool GameOver = false;
        private LevelEnum _currentLevelEnum;
        private Level _currentLevel;
        private int _unitsHit;

        public Manager(GameCanvas playGrid)
        {
            _playGrid       = playGrid;
            _mouseCapture   = new MouseCapture(_playGrid);

            _xMaxBounds = (int)_playGrid.ActualWidth - 60;
            _xMinBounds = 10;

            _yMaxBounds = (int)_playGrid.ActualHeight - 60;
            _yMinBounds = 10;

            _currentLevelEnum = LevelEnum.LevelOne;
            _currentLevel = LevelFactory.CreateLevel(_currentLevelEnum);

            initialize();
            initLevel();
        }

        private void initialize()
        {
            _player = new Player();
        }

        private void nextLevel()
        {
            _currentLevelEnum++;
            if (_currentLevelEnum == LevelEnum.GameOver)
            {
                GameOver = true;
                return;
            }

            _currentLevel = LevelFactory.CreateLevel(_currentLevelEnum);
            initLevel();
        }

        private void initLevel()
        {
            _unitsHit = 0;
            _nonMovingUnits = new List<Unit>();
            _movingUnits = new List<MovingUnit>();

            // set trees
            for (int t = 0; t < _currentLevel.NonMovingUnitAmount; t++)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // set location
                    // random based on x and y bounds
                    var xPos = (double)_random.Next(_xMinBounds, _xMaxBounds);
                    var yPos = (double)_random.Next(_yMinBounds, _yMaxBounds);

                    // Set random unit on screen
                    Unit unit = UnitFactory.CreateUnit(UnitEnum.Tree, xPos, yPos);
                    _nonMovingUnits.Add(unit);
                });
            }

            // random amount MovingUnits
            int startAmountUnits = _random.Next(5);
            for (int x = 0; x < startAmountUnits; x++)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MovingUnit unit = null;
                    bool collides = true;
                    while (collides)
                    {
                        // set location
                        var xPos = _random.Next(_xMinBounds, _xMaxBounds);
                        var yPos = _random.Next(_yMinBounds, _yMaxBounds);

                        var unitType = (UnitEnum)_random.Next(1, 3);
                        unit = (MovingUnit)UnitFactory.CreateUnit(unitType, xPos, yPos);

                        if (
                            !_nonMovingUnits.Any(
                                unit1 =>
                                    xPos >= unit1.LeftPosition && xPos <= (unit1.LeftPosition + 50) &&
                                    yPos >= unit1.TopPosition &&
                                    yPos <= (unit1.TopPosition + 50)))
                            collides = false;
                    }

                    unit.SetSteps(_currentLevel.MinSpeed, _currentLevel.MaxSpeed);
                    _movingUnits.Add(unit);
                });
            }
        }

        public void Update()
        {
            updateUnits();
            updatePlayer();
            updateRender();
            
            if (_unitsHit == _currentLevel.UnitsToHit)
                nextLevel();
        }

        private void updateUnits()
        {
            Dictionary<double, double> clickedLocations = _mouseCapture.getClicks();

            foreach (MovingUnit unit in _movingUnits)
            {
                foreach (KeyValuePair<double, double> coords in clickedLocations)
                {
                    unit.SetIsShot(coords);
                }
                unit.IsOutOfBounds(_yMaxBounds, _xMaxBounds);

                bool wasCollide = false;
                unit.MoveX();

                foreach (Unit nonMovingUnit in _nonMovingUnits)
                {
                   wasCollide = unit.CheckCollisionX(nonMovingUnit);
                }

                unit.MoveY();

                if (!wasCollide)
                {
                    foreach (Unit nonMovingUnit in _nonMovingUnits)
                    {
                        unit.CheckCollisionY(nonMovingUnit);
                    }
                }
            }

            MovingUnit newMovingUnit = null;
            // Add MovingUnit
            int randomNum = _random.Next(35);
            if (randomNum%2 == 0 && _movingUnits.Count < _currentLevel.MovingUnitAmount)
            {
                bool collides = true;
                while (collides)
                {
                    // set location
                    var xPos = _random.Next(_xMinBounds, _xMaxBounds);
                    var yPos = _random.Next(_yMinBounds, _yMaxBounds);

                    var unitType = (UnitEnum) _random.Next(1, 3);
                    newMovingUnit = (MovingUnit) UnitFactory.CreateUnit(unitType, xPos, yPos);

                    if (
                        !_nonMovingUnits.Any(
                            unit1 =>
                                xPos >= unit1.LeftPosition && xPos <= (unit1.LeftPosition + 50) &&
                                yPos >= unit1.TopPosition &&
                                yPos <= (unit1.TopPosition + 50)))
                        collides = false;
                }

                newMovingUnit.SetSteps(_currentLevel.MinSpeed, _currentLevel.MaxSpeed);
                _movingUnits.Add(newMovingUnit);
            }
        }

        private void updatePlayer()
        {
            foreach (MovingUnit unit in _movingUnits.Where(unit => unit.isShot))
            {
                _player.addPoint();
                _unitsHit++;
            }
        }

        private void updateRender()
        {
            try
            {
                for (int mUnit = _movingUnits.Count - 1; mUnit > 0; mUnit--)
                {
                    var movingUnit = _movingUnits[mUnit];
                    if (movingUnit.isShot)
                        _movingUnits.Remove(movingUnit);
                }

                Application.Current.Dispatcher.Invoke(() => _playGrid.DrawMovingUnits(_movingUnits));
                Application.Current.Dispatcher.Invoke(() => _playGrid.DrawUnits(_nonMovingUnits));
            }
            catch (Exception e)
            {
                Environment.Exit(0);
            }
            
        }
    }
}
