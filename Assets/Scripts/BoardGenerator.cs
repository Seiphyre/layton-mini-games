//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.CompilerServices;
//using Unity.VisualScripting;
//using UnityEditor.SceneManagement;
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.WSA;

//public class BoardGenerator
//{
//    [ContextMenu("Generate")]
//    private void GenerateBoard(BoardData boardData)
//    {
//        if (boardData != null)
//        {
//            DestroyBoard();

//            // --

//            VerticalLayoutGroup gridLayout = gameObject.AddComponent<VerticalLayoutGroup>();
//            gridLayout.padding = new RectOffset(WallSize / 2, WallSize / 2, WallSize / 2, WallSize / 2);
//            gridLayout.childAlignment = TextAnchor.UpperLeft;
//            gridLayout.reverseArrangement = false;

//            gridLayout.childControlWidth = true;
//            gridLayout.childControlHeight = true;
//            gridLayout.childScaleWidth = false;
//            gridLayout.childScaleHeight = false;
//            gridLayout.childForceExpandWidth = false;
//            gridLayout.childForceExpandHeight = false;

//            for (int y = 0; y < Data.Size.y; y++)
//            {
//                GameObject row = new GameObject();

//                row.name = $"Row {y}";
//                row.transform.SetParent(this.transform);

//                // --

//                HorizontalLayoutGroup rowLayout = row.AddComponent<HorizontalLayoutGroup>();

//                rowLayout.padding = new RectOffset(0, 0, 0, 0);
//                rowLayout.childAlignment = TextAnchor.UpperLeft;
//                rowLayout.reverseArrangement = false;

//                rowLayout.childControlWidth = true;
//                rowLayout.childControlHeight = true;
//                rowLayout.childScaleWidth = false;
//                rowLayout.childScaleHeight = false;
//                rowLayout.childForceExpandWidth = false;
//                rowLayout.childForceExpandHeight = false;

//                Undo.RegisterCreatedObjectUndo(row, "Created board row");
//                EditorSceneManager.MarkSceneDirty(row.scene);

//                for (int x = 0; x < Data.Size.x; x++)
//                {
//                    VisualTile tile = Instantiate(TileTemplate, row.transform);

//                    tile.name = $"Tile {x}:{y}";

//                    LayoutElement tileLayoutElement = tile.GetComponent<LayoutElement>();

//                    if (tileLayoutElement == null)
//                        tileLayoutElement = tile.AddComponent<LayoutElement>();

//                    tileLayoutElement.ignoreLayout = false;
//                    tileLayoutElement.minWidth = TileSize;
//                    tileLayoutElement.minHeight = TileSize;
//                    tileLayoutElement.preferredWidth = TileSize;
//                    tileLayoutElement.preferredHeight = TileSize;
//                    tileLayoutElement.flexibleWidth = 0;
//                    tileLayoutElement.flexibleHeight = 0;

//                    tile.Value = Data.GetTile(x, y);

//                    tile.Refresh();

//                    Undo.RegisterCreatedObjectUndo(tile.gameObject, "Created board tile");
//                    EditorSceneManager.MarkSceneDirty(tile.gameObject.scene);
//                }
//            }
//        }
//    }

//    [ContextMenu("Destroy")]
//    public void DestroyBoard()
//    {
//        Transform[] children = GetComponentsInChildren<Transform>();
//        foreach (Transform child in children)
//        {
//            if (child == transform) continue;

//            if (child != null)
//                DestroyImmediate(child.gameObject);
//        }

//        LayoutGroup oldLayout = GetComponent<LayoutGroup>();

//        if (oldLayout != null)
//            DestroyImmediate(oldLayout);
//    }
//}
