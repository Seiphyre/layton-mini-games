using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneStopShop
{
    public class SceneNavigator : ISceneNavigator
    {
        private readonly UnitySceneLoader _loader;
        private readonly Dictionary<SceneId, string> _sceneMap;

        public SceneNavigator(UnitySceneLoader loader)
        {
            _loader = loader;

            _sceneMap = new Dictionary<SceneId, string>
            {
                { SceneId.Menu, "MenuScene" },
                { SceneId.Game, "GameScene" }
            };
        }

        public void GoTo(SceneId scene)
        {
            if (!_sceneMap.TryGetValue(scene, out var sceneName))
            {
                throw new KeyNotFoundException($"Scene not registered: {scene}");
            }

            _loader.Load(sceneName);
        }
    }
}
