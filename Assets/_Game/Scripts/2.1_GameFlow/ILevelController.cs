using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneStopShop
{
    public interface ILevelController
    {
        void ResetLevel();
        bool HasNextLevel();
        void LoadNextLevel();

        LevelData CurrentLevel { get; }
    }
}
