using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OneStopShop
{
    public class MenuStartup : MonoBehaviour
    {
        [SerializeField] private SelectLevelView selectLevelView;

        [SerializeField] private AppConfig gameConfig;



        private void Start()
        {
            selectLevelView.Bind(gameConfig.Levels);

            selectLevelView.LevelSelected += SelectLevelView_LevelSelected;
        }

        private void SelectLevelView_LevelSelected(LevelDefinition levelDefinition)
        {
            gameConfig.StartLevel = gameConfig.Levels.IndexOf(levelDefinition);
            SceneManager.LoadScene("GameScene");
        }
    }
}
