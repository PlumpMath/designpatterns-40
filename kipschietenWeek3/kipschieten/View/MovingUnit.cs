using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;

namespace kipschieten.View
{
    abstract class MovingUnit : Unit
    {
        private bool _isShot = false;
        public bool isShot { get { return _isShot; } set { _isShot = value; } }

        protected double XStep = 0;
        protected double YStep = 0;

        public void SetSteps(int minSpeed, int maxSpeed)
        {
            var random = new Random();

            while (XStep >= -(minSpeed) && XStep <= minSpeed)
                XStep = random.Next(-(maxSpeed), maxSpeed);
            while (YStep >= -(minSpeed) && YStep <= (minSpeed))
                YStep = random.Next(-(maxSpeed), maxSpeed);
        }

        public void MoveX()
        {
            // margin for location in grid
            LeftPosition += XStep;
            
        }

        public void MoveY()
        {
            TopPosition += YStep;
        }

        public void ChangeYPosition()
        {
            YStep = -YStep;
            MoveX();
        }

        public void ChangeXPosition()
        {
            XStep = -XStep;
            MoveX();
        }

        public void SetIsShot(KeyValuePair<double, double> coords)
        {
            if (coords.Key >= LeftPosition && coords.Key <= LeftPosition + Width &&
                coords.Value >= TopPosition && coords.Value <= TopPosition + Height)
                _isShot = true;
        }

        public bool IsOutOfBounds(double yMaxBounds, double xMaxBounds)
        {
            if (TopPosition <= 0 || TopPosition >= yMaxBounds+10)
                ChangeYPosition();

            if (LeftPosition <= 0 || LeftPosition >= xMaxBounds+10)
                ChangeXPosition();
            return false;
        }

        public bool CheckCollisionX(Unit nonMovingUnit)
        {
            // check if collision left
            if ((LeftPosition + 50) >= nonMovingUnit.LeftPosition &&
                (LeftPosition + 50) <= (nonMovingUnit.LeftPosition + 10)
                && ((TopPosition >= nonMovingUnit.TopPosition && TopPosition <= nonMovingUnit.TopPosition + 50)
                    ||
                    (TopPosition + 50 <= nonMovingUnit.TopPosition + 50 && TopPosition + 50 >= nonMovingUnit.TopPosition)))
            {
                ChangeXPosition();
                return true;
            }

            // check if collision right
            if (LeftPosition <= nonMovingUnit.LeftPosition + 50 && LeftPosition >= nonMovingUnit.LeftPosition + 40
                && ((TopPosition >= nonMovingUnit.TopPosition && TopPosition <= nonMovingUnit.TopPosition + 50)
                    ||
                    (TopPosition + 50 <= nonMovingUnit.TopPosition + 50 && TopPosition + 50 >= nonMovingUnit.TopPosition)))
            {
                ChangeXPosition();
                return true;
            }
            return false;
        }

        public bool CheckCollisionY(Unit nonMovingUnit)
        {
            // check collision top
            if ((TopPosition + 50) >= nonMovingUnit.TopPosition && (TopPosition + 50) <= nonMovingUnit.TopPosition + 10
                && ((LeftPosition >= nonMovingUnit.LeftPosition && LeftPosition <= nonMovingUnit.LeftPosition + 50)
                    ||
                    (LeftPosition + 50 >= nonMovingUnit.LeftPosition &&
                     LeftPosition + 50 <= nonMovingUnit.LeftPosition + 50)))
            {
                ChangeYPosition();
                return true;
            }

            // Check collision bottom
            if (TopPosition <= (nonMovingUnit.TopPosition + 50) && TopPosition >= nonMovingUnit.TopPosition + 40
                && ((LeftPosition >= nonMovingUnit.LeftPosition && LeftPosition <= nonMovingUnit.LeftPosition + 50)
                    ||
                    (LeftPosition + 50 >= nonMovingUnit.LeftPosition &&
                     LeftPosition + 50 <= nonMovingUnit.LeftPosition + 50)))
            {
                ChangeYPosition();
                return true;
            }

            return false;
        }
    }
}
