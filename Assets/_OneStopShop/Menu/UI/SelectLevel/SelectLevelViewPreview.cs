#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace OneStopShop
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SelectLevelView))]
    public sealed class SelectLevelViewPreview : MonoBehaviour
    {
        [SerializeField] private List<LevelDefinition> levels;

        private SelectLevelView _view;
        private bool _isDirty = false;



        private void Awake()
        {
            _view = GetComponent<SelectLevelView>();

            if (IsInPrefabMode())
                Debug.LogWarning("SelectLevelViewPreview disabled in prefab mode."); // Because it modify the prefab hierarchy
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

            if (IsInPrefabMode())
                return;

            _view.Bind(levels, MarkAsPreviewObject);
        }

        private void Unbind()
        {
            if (Application.isPlaying)
                return;

            if (_view == null)
                return;

            // Avoid unsafe prefab asset mutation
            if (IsInPrefabMode())
                return;

            _view.Unbind();
        }


        // --

        private void MarkAsPreviewObject(GameObject go)
        {
            go.name += " (Preview)";
            go.hideFlags |= HideFlags.DontSaveInEditor;
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
