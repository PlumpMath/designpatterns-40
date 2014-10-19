using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kipschieten.Levels;
using kipschieten.View;

namespace kipschieten.Model
{
    static class LevelFactory
    {
        private static LevelEnum _currentLevelEnum;
        public static bool GameOver = false;
        private static GameCanvas _playGrid;
        private static Player _player;

        public static Level CurrentLevel { get; set; }

        public static Level CreateLevel(LevelEnum level, GameCanvas playGrid, Player player)
        {
            _playGrid = playGrid;
            _player = player;
            string className = "kipschieten.Levels." + level.ToString();

            Type t = Type.GetType(className);
            return Activator.CreateInstance(t, _playGrid, _player) as Level;
        }

        public static void NextLevel()
        {
            _currentLevelEnum++;
            if (_currentLevelEnum == LevelEnum.GameOver)
            {
                GameOver = true;
                return;
            }

            CurrentLevel = LevelFactory.CreateLevel(_currentLevelEnum,_playGrid,_player);
            CurrentLevel.InitLevel();
        }
    }
}
