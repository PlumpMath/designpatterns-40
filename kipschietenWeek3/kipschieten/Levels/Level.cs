using kipschieten.Model;
using kipschieten.View;

namespace kipschieten.Levels
{
    abstract class Level
    {
        public int MovingUnitAmount { get; set; }

        public int NonMovingUnitAmount { get; set; }

        public int MaxSpeed { get; set; }

        public int MinSpeed { get; set; }

        public int UnitsToHit { get; set; }

        public virtual void Update()
        {
            
        }

        public void NextLevel()
        {
            LevelFactory.NextLevel();
        }

        public virtual void InitLevel()
        {
            
        }
    }
}
