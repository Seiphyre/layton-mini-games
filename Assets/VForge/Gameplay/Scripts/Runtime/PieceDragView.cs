using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VForge.BoardPieces.Definitions;

public class PieceDragView : MonoBehaviour
{
    [SerializeField] private Image _background;



    public void Initialize(PieceDefinition pieceDefinition)
    {
        _background.color = pieceDefinition.Style.Color;
    }
}
