using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OneStopShop
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private Transform buttonsRoot1;
        [SerializeField] private Transform buttonsRoot2;
        [SerializeField] private LevelButton buttonprefab;

        [SerializeField] private GameConfig gameConfig;

        private List<LevelButton> levelButtons = new();



        private void OnEnable()
        {
            int i = 1;
            foreach (var levelData in gameConfig.Levels)
            {
                var levelButton = Instantiate(buttonprefab, (i <= 3) ? buttonsRoot1 : buttonsRoot2);

                levelButton.Initialize(levelData);
                levelButton.OnCick += OnClick;

                levelButtons.Add(levelButton);
                i++;
            }
        }

        private void OnDisable()
        {
            foreach (var levelButton in levelButtons)
            {
                levelButton.OnCick -= OnClick;
                Destroy(levelButton.gameObject);
            }

            levelButtons.Clear();
        }

        private void OnClick(LevelButton levelButton)
        {
            gameConfig.StartLevel = gameConfig.Levels.IndexOf(levelButton.LevelData);
            SceneManager.LoadScene("GameScene");
        }
    }
}
