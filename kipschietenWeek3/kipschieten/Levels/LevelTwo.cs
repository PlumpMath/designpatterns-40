using kipschieten.Model;
using kipschieten.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace kipschieten.Levels
{
    class LevelTwo : Level
    {
        private List<MovingUnit> _movingUnits;
        private List<Unit> _nonMovingUnits;
        private int _unitsHit;
        private readonly Random _random = new Random();

        // Grid bounds
        private int _xMinBounds;
        private int _xMaxBounds;
        private int _yMinBounds;
        private int _yMaxBounds;

        private MouseCapture _mouseCapture;
        private readonly GameCanvas _playGrid;
        private readonly Player _player;

        public LevelTwo(GameCanvas playGrid, Player player)
        {
            _playGrid = playGrid;
            _player = player;
            UnitsToHit = 7;
            MovingUnitAmount = 7;
            NonMovingUnitAmount = 10;
            MaxSpeed = 6;
            MinSpeed = 3;
        }

         public override void InitLevel()
        {
            _mouseCapture = new MouseCapture(_playGrid);

            _xMaxBounds = (int)_playGrid.ActualWidth - 60;
            _xMinBounds = 10;

            _yMaxBounds = (int)_playGrid.ActualHeight - 60;
            _yMinBounds = 10;

            _unitsHit = 0;
            _nonMovingUnits = new List<Unit>();
            _movingUnits = new List<MovingUnit>();

            // set trees
            for (int t = 0; t < this.NonMovingUnitAmount; t++)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // set location
                    // random based on x and y bounds
                    var xPos = (double)_random.Next(_xMinBounds, _xMaxBounds);
                    var yPos = (double)_random.Next(_yMinBounds, _yMaxBounds);

                    // Set stone on screen
                    Unit unit = UnitFactory.CreateUnit(UnitEnum.Stone, xPos, yPos);
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

                        var unitType = (UnitEnum)_random.Next(2, 4);
                        unit = (MovingUnit)UnitFactory.CreateUnit(unitType, xPos, yPos);

                        if (
                            !_nonMovingUnits.Any(
                                unit1 =>
                                    xPos >= unit1.LeftPosition && xPos <= (unit1.LeftPosition + 50) &&
                                    yPos >= unit1.TopPosition &&
                                    yPos <= (unit1.TopPosition + 50)))
                            collides = false;
                    }

                    unit.SetSteps(this.MinSpeed, this.MaxSpeed);
                    _movingUnits.Add(unit);
                });
            }
        }

        public override void Update()
        {
            UpdateUnits();
            UpdatePlayer();
            UpdateRender();

            if (_unitsHit == this.UnitsToHit)
                NextLevel();
        }

        public void UpdateUnits()
        {
            Dictionary<double, double> clickedLocations = MouseCapture.getClicks();

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
            if (randomNum % 2 == 0 && _movingUnits.Count < this.MovingUnitAmount)
            {
                bool collides = true;
                while (collides)
                {
                    // set location
                    var xPos = _random.Next(_xMinBounds, _xMaxBounds);
                    var yPos = _random.Next(_yMinBounds, _yMaxBounds);

                    var unitType = (UnitEnum)_random.Next(2, 4);
                    newMovingUnit = (MovingUnit)UnitFactory.CreateUnit(unitType, xPos, yPos);

                    if (
                        !_nonMovingUnits.Any(
                            unit1 =>
                                xPos >= unit1.LeftPosition && xPos <= (unit1.LeftPosition + 50) &&
                                yPos >= unit1.TopPosition &&
                                yPos <= (unit1.TopPosition + 50)))
                        collides = false;
                }

                newMovingUnit.SetSteps(this.MinSpeed, this.MaxSpeed);
                _movingUnits.Add(newMovingUnit);
            }
        }

        public void UpdatePlayer()
        {
            foreach (MovingUnit unit in _movingUnits.Where(unit => unit.isShot))
            {
                _player.addPoint();
                _unitsHit++;
            }
        }

        public void UpdateRender()
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
