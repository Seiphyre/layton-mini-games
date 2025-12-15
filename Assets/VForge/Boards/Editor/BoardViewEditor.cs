using UnityEditor;
using VForge.Boards;
using UnityEngine;
using VForge.Boards.Views;

[CustomEditor(typeof(BoardView))]
public class BoardViewEditor : Editor
{
    private void OnSceneGUI()
    {
        var view = (BoardView)target;

        if (view.IsDirty)
        {
            view.Rebuild();
            view.ClearDirty();
        }
    }

    public override void OnInspectorGUI()
    {
        BoardView view = (BoardView)target;

        EditorGUI.BeginChangeCheck();

        DrawDefaultInspector();

        if (EditorGUI.EndChangeCheck())
        {
            // Values changed → rebuild immediately
            if (!Application.isPlaying)
            {
                view.Rebuild();
                EditorUtility.SetDirty(view);
            }
        }

        // --

        GUILayout.Space(10);

        // Disable button if no BoardData assigned
        EditorGUI.BeginDisabledGroup(view.BoardData == null);
        if (GUILayout.Button("Refresh", GUILayout.Height(30)))
        {
            // Ensure safe Rebuild with undo support
            Undo.RecordObject(view, "BoardView Refresh");

            // Force refresh
            view.Rebuild();

            EditorUtility.SetDirty(view);
            SceneView.RepaintAll();
        }
        EditorGUI.EndDisabledGroup();
    }
}
