#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace OneStopShop
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SelectLevelButtonView))]
    public sealed class SelectLevelButtonViewPreview : MonoBehaviour
    {
        [SerializeField] private Sprite thumbnail;
        [SerializeField] private int levelNumber = 1;
        [SerializeField] private string levelName = "{level_name}";

        private SelectLevelButtonView _view;
        private bool _isDirty = false;



        private void Awake()
        {
            _view = GetComponent<SelectLevelButtonView>();

            //if (IsDrivenByScreenPreview())
            //    Debug.LogWarning("SelectLevelButtonView disabled when driven by SelectLevelView.");
        }

        private void Update()
        {
            if (_isDirty)
            {
                Bind();
                _isDirty = false;
            }
        }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            Bind();
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

            Unbind();
        }

        private void OnValidate()
        {
            _isDirty = true;
        }



        // --

        private void Bind()
        {
            if (Application.isPlaying)
                return;

            if (_view == null)
                return;

            if (IsDrivenByScreenPreview())
                return;

            _view.Bind(levelNumber, levelName, thumbnail);
        }

        private void Unbind()
        {
            if (Application.isPlaying)
                return;

            if (_view == null)
                return;

            if (IsDrivenByScreenPreview())
                return;

            _view.Unbind();
        }

        // --

        private bool IsDrivenByScreenPreview()
        {
            return GetComponentInParent<SelectLevelView>() != null
                   /*&& !IsInPrefabMode()*/;
        }

        private bool IsInPrefabMode()
        {
            return PrefabStageUtility.GetCurrentPrefabStage() != null;
        }

        // --

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.ExitingEditMode:
                    // Clear preview before runtime starts
                    Unbind();
                    break;

                case PlayModeStateChange.EnteredEditMode:
                    // Restore preview after play mode
                    Bind();
                    break;
            }
        }
    }
}

#endif
