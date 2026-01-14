using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneStopShop
{
    public class SelectLevelView : MonoBehaviour
    {
        [SerializeField] private Transform contentRoot;
        [SerializeField] private SelectLevelButtonView buttonPrefab;

        // --

        private readonly List<SelectLevelButtonView> _buttonViews = new();

        // --

        public event Action<LevelDefinition> LevelSelected;



        // ----------------------------------------------------------
        // View lifecycle
        // ----------------------------------------------------------

        public void Bind(IEnumerable<LevelDefinition> levels, Action<GameObject> onButtonCreated = null)
        {
            Unbind();

            if (levels == null)
                return;

            int levelNumber = 1;
            foreach (var level in levels)
            {
                if (level == null)
                    continue;

                var button = Instantiate(buttonPrefab, contentRoot);
                button.name = level.Name;
                onButtonCreated?.Invoke(button.gameObject);

                button.Bind(levelNumber, level.Name, level.Thumbail, () => OnButtonClicked(level));

                _buttonViews.Add(button);

                levelNumber++;
            }
        }

        public void Unbind()
        {
            foreach (var button in _buttonViews)
            {
                if (button == null)
                    continue;

                button.Unbind();
                DestroyImmediate(button.gameObject);
            }

            _buttonViews.Clear();
        }



        // ----------------------------------------------------------
        // Event Handlers
        // ----------------------------------------------------------

        private void OnButtonClicked(LevelDefinition level)
        {
            LevelSelected?.Invoke(level);
        }
    }
}
