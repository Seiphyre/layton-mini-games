using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaginationControls : UIElement
{
    [SerializeField] private Pagination pagination;

    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;



    // ----------------------------------------

    private void Awake()
    {
        ComponentUtils.AssignIfNull(this, ref pagination);

        if (prevButton != null) 
            prevButton.onClick.AddListener(pagination.Prev);

        if (nextButton != null) 
            nextButton.onClick.AddListener(pagination.Next);
    }

    private void OnEnable()
    {
        if (pagination != null) 
            pagination.onPageChanged.AddListener(RefreshButtonsStates);

        RefreshButtonsStates();
    }

    private void OnDisable()
    {
        if (pagination != null) 
            pagination.onPageChanged.RemoveListener(RefreshButtonsStates);
    }

    private void OnDestroy()
    {
        if (prevButton != null) 
            prevButton.onClick.RemoveListener(pagination.Prev);

        if (nextButton != null) 
            nextButton.onClick.RemoveListener(pagination.Next);
    }



    // ----------------------------------------

    private void RefreshButtonsStates()
    {
        if (pagination == null) return;

        int currentPage = pagination.CurrentPage;
        int lastPage = pagination.PageCount() - 1;

        if (prevButton != null)
            prevButton.interactable = currentPage > 0;

        if (nextButton != null)
            nextButton.interactable = currentPage < lastPage;
    }
}
