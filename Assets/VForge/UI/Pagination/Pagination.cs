using System;

using UnityEngine;
using UnityEngine.Events;



public class Pagination : MonoBehaviour
{
    [field: SerializeField] public int ItemsPerPage { get; private set; } = 5;

    [Space]

    public UnityEvent onPageChanged = new UnityEvent();

    // --

    public int TotalItems { get; private set; } = 0;
    public int CurrentPage { get; private set; } = 0;



    // --------------------------------------------------------

    public void SetItemsPerPage(int value)
    {
        value = Mathf.Clamp(value, 0, TotalItems);

        if (ItemsPerPage != value)
        {
            ItemsPerPage = value;

            ClampCurrentPage();

            onPageChanged?.Invoke();
        }
    }



    public void SetTotalItems(int value, bool notify = true)
    {
        value = Mathf.Max(0, value);

        if (TotalItems != value)
        {
            TotalItems = value;

            ClampCurrentPage();

            if (notify)
                onPageChanged?.Invoke();
        }
    }



    // --------------------------------------------------------

    public void SetPage(int page)
    {
        int max = PageCount() - 1;
        page = Mathf.Clamp(page, 0, max);

        if (page != CurrentPage)
        {
            CurrentPage = page;

            onPageChanged?.Invoke();
        }
    }

    public void Next() => SetPage(CurrentPage + 1);
    public void Prev() => SetPage(CurrentPage - 1);
    public void First() => SetPage(0);
    public void Last() => SetPage(PageCount() - 1);



    // --------------------------------------------------------

    public (int start, int end) GetVisibleRange(bool allowEmptyItemOnLastPage = false)
    {
        int start = CurrentPage * ItemsPerPage;
        int end = allowEmptyItemOnLastPage ? start + ItemsPerPage : Mathf.Min(start + ItemsPerPage, TotalItems);

        return (start, end);
    }
    public int PageCount()
    {
        if (TotalItems <= 0)
            return 1;

        return Mathf.CeilToInt(TotalItems / (float)ItemsPerPage);
    }

    public bool IsLastPage => CurrentPage == PageCount() - 1;

    public bool IsFirstPage => CurrentPage == 0;



    // --------------------------------------------------------

    private void ClampCurrentPage()
    {
        if (CurrentPage >= PageCount()) 
            CurrentPage = PageCount() - 1;

        if (CurrentPage < 0) 
            CurrentPage = 0;
    }
}
