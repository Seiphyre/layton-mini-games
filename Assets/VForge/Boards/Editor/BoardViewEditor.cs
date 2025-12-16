using UnityEditor;
using VForge.Boards;
using UnityEngine;
using VForge.Boards.Views;

[CustomEditor(typeof(BoardView))]
public class BoardViewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var view = (BoardView)target;

        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        if (EditorGUI.EndChangeCheck())
        {
            if (!Application.isPlaying)
            {
                Undo.RecordObject(view, "BoardView Change");
                view.Rebuild();
                EditorUtility.SetDirty(view);
            }
        }

        GUILayout.Space(10);

        using (new EditorGUI.DisabledScope(view == null))
        {
            if (GUILayout.Button("Rebuild Board"))
            {
                Undo.RecordObject(view, "BoardView Rebuild");
                view.Rebuild();
                EditorUtility.SetDirty(view);
            }
        }
    }
}
