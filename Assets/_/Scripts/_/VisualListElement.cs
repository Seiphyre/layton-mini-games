using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VisualListElement : VisualElement
{
    [Space]

    [SerializeField] protected GameObject m_ContentPanel;
    [SerializeField] protected VisualElement m_ItemPanel;

    [Space]

    [SerializeField] protected List<GameObject> m_disabledObjectsOnDrag;

    private bool _dragged = false;


    // ---------------------------------------------

    public override void Refresh()
    {
        base.Refresh();

        if (m_disabledObjectsOnDrag != null)
        {
            foreach (var obj in m_disabledObjectsOnDrag)
                obj.SetActive(!_dragged);
        }

        //if (m_ContentPanel != null)
        //    m_ContentPanel.SetActive(Value != null);
    }

    protected virtual void OnEnable()
    {
        if (IsDraggable)
        {
            //Draggable.onDragStarted += Draggable_DragStarted;
            //Draggable.onDragEnded += Draggable_DragEnded;
        }
    }

    protected virtual void OnDisable()
    {
        if (IsDraggable)
        {
            //Draggable.onDragStarted -= Draggable_DragStarted;
            //Draggable.onDragEnded -= Draggable_DragEnded;
        }
    }

    private void Draggable_DragEnded(Draggable arg1)
    {
        _dragged = false;
        _shouldRefresh = true;
    }

    private void Draggable_DragStarted(Draggable arg1)
    {
        _dragged = true;
        _shouldRefresh = true;
    }

    //protected override void OnValueChanged()
    //{
    //    if (m_ItemPanel != null)
    //        m_ItemPanel.Value = Value;

    //    base.OnValueChanged();
    //}
}
