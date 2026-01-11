using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VForge.Gameplay;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Transform buttonsRoot;
    [SerializeField] private LevelButton buttonprefab;

    [SerializeField] private GameConfig gameConfig;

    private List<LevelButton> levelButtons = new ();



    private void OnEnable()
    {
        foreach (var levelData in gameConfig.Levels)
        {
            var levelButton = Instantiate(buttonprefab, buttonsRoot);

            levelButton.Initialize(levelData);
            levelButton.OnCick += OnClick;

            levelButtons.Add(levelButton);
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
