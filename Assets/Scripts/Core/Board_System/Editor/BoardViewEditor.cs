using Codice.CM.Client.Differences;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(BoardView))]
public class BoardViewEditor : Editor
{
    private BoardView _board;
    private BoardView Board
    {
        get
        {
            if (_board == null)
                _board = (BoardView)target;

            return _board;
        }
    }



    // ------------------------------------------

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // --

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Generate"))
        {
            GameObject board = Board.CreateBoard();

            Undo.RegisterCreatedObjectUndo(board, "Created board");

            SaveBoard();
        }

        if (GUILayout.Button("Clear"))
        {
            Board.DestroyBoard();

            SaveBoard();
        }

        //Undo.RegisterCreatedObjectUndo(grid, "Created Grid");
        //Undo.RecordObject(this, "Created Grid");
        //EditorSceneManager.MarkSceneDirty(grid.scene);

        EditorGUILayout.EndHorizontal();
    }

    private void SaveBoard()
    {
        EditorUtility.SetDirty(Board);
        EditorSceneManager.MarkSceneDirty(Board.gameObject.scene);
    }
}
