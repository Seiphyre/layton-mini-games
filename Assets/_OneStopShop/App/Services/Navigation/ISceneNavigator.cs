using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneStopShop
{
    public interface ISceneNavigator
    {
        void GoTo(SceneId scene);
    }
}