using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VForge.BoardPieces.Definitions;
using VForge.BoardPieces.Views;

public class PieceDragView : MonoBehaviour
{
    [SerializeField] private PieceDefinitionView definitionView;
    [SerializeField] private float blockSize = 64;



    public void Initialize(PieceDefinition pieceDefinition)
    {
        definitionView.Initialize(pieceDefinition, blockSize);
    }
}
