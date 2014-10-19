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
        private Player _player;
        private readonly Level _currentLevel;

        public Manager(GameCanvas playGrid)
        {
            const LevelEnum currentLevelEnum = LevelEnum.LevelOne;
            _player = new Player();
            LevelFactory.CurrentLevel = LevelFactory.CreateLevel(currentLevelEnum, playGrid, _player);
            LevelFactory.CurrentLevel.InitLevel();
        }

        public void Update()
        {
            LevelFactory.CurrentLevel.Update();
        }
    }
}
