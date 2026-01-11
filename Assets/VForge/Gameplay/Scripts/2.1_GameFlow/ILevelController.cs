using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VForge.Gameplay
{
    public interface ILevelController
    {
        void ResetLevel();
        bool HasNextLevel();
        void LoadNextLevel();

        LevelData CurrentLevel { get; }
    }
}
