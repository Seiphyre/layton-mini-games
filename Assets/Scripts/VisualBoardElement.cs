using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualBoardElement : VisualElement
{
    [SerializeField] private VisualElement VisualElement;

    //private BoardElement _boardElement;

    //public BoardElement BoardElement
    //{
    //    get
    //    {
    //        if (_boardElement == null)
    //            _boardElement = GetComponent<BoardElement>();

    //        return _boardElement;
    //    }
    //}

    public BoardView Board { get; set; }

    public Vector2Int Position { get; set; }
    public Vector2Int Size { get; set; }

    protected override void OnValueChanged()
    {
        if (VisualElement != null)
            VisualElement.Value = Value;

        base.OnValueChanged();
    }
}
