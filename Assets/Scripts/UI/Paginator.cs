using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

[RequireComponent(typeof(ICollectionView))]
public class Paginator : MonoBehaviour
{
    [Header("References"), Space]

    [SerializeField] private Button m_nextPageBtn;
    [SerializeField] private Button m_prevPageBtn;



    [Header("Parameters"), Space]

    [SerializeField] private int m_itemPerPage = 5;
    public int ItemPerPage
    {
        get { return m_itemPerPage; }
        set
        {
            var prevValue = m_itemPerPage;

            m_itemPerPage = value;

            if (value != prevValue)
                OnItemPerPageChanged(this);
        }
    }

    public event Action<object> ItemPerPageChanged;



    [SerializeField] private int m_currentPage = 0;
    public int CurrentPage
    {
        get { return m_currentPage; }
        set
        {
            var prevValue = m_currentPage;
            var newValue = Mathf.Clamp(value, FirstPage, LastPage);

            m_currentPage = newValue;

            if (newValue != prevValue)
                OnCurrentPageChanged(this);
        }
    }

    public event Action<object> CurrentPageChanged;

    // --

    private ICollectionView _collection;
    public ICollectionView Collection
    {
        get
        {
            if (_collection == null)
                _collection = GetComponent<ICollectionView>();

            return _collection;
        }
    }

    // --

    public ICollection<object> PageItems
    {
        get
        {
            if (Collection == null || Collection.Source == null)
                return null;

            return Collection.Source.OfType<object>().Skip(CurrentPage * ItemPerPage).Take(ItemPerPage).ToList();
        }
    }

    // --

    private bool _shouldRefresh = false;



    // ----------------------------------------

    private void OnEnable()
    {
        if (Collection != null)
            Collection.SourceChanged += Collection_SourceChanged;

        // --

        if (m_prevPageBtn != null)
            m_prevPageBtn.onClick.AddListener(PrevPage_OnClick);

        if (m_nextPageBtn != null)
            m_nextPageBtn.onClick.AddListener(NextPage_OnClick);

        // --

        _shouldRefresh = true;
    }

    private void OnDisable()
    {
        if (Collection != null)
            Collection.SourceChanged -= Collection_SourceChanged;

        // --

        if (m_prevPageBtn != null)
            m_prevPageBtn.onClick.RemoveListener(PrevPage_OnClick);

        if (m_nextPageBtn != null)
            m_nextPageBtn.onClick.RemoveListener(NextPage_OnClick);
    }

    private void Update()
    {
        if (_shouldRefresh)
            Refresh();
    }


    // ----------------------------------------

    public void Refresh()
    {
        if (m_prevPageBtn != null)
        {
            m_prevPageBtn.interactable = !IsFirstPage;
        }

        if (m_nextPageBtn != null)
        {
            m_nextPageBtn.interactable = !IsLastPage;
        }

        _shouldRefresh = false;
    }



    // ----------------------------------------

    public int FirstPage
    {
        get { return 0; }
    }

    public bool IsFirstPage
    {
        get { return CurrentPage == FirstPage; }
    }

    // --

    public int LastPage
    {
        get
        {
            if (Collection == null || Collection.Source == null || ItemPerPage <= 0)
                return FirstPage;

            return Mathf.CeilToInt(Collection.Source.Count / (float)ItemPerPage) - 1;
        }
    }

    public bool IsLastPage
    {
        get { return CurrentPage == LastPage; }
    }



    // ----------------------------------------

    private void PrevPage_OnClick()
    {
        CurrentPage--;
    }

    private void NextPage_OnClick()
    {
        CurrentPage++;
    }

    // --

    private void OnItemPerPageChanged(object sender)
    {
        _shouldRefresh = true;

        ItemPerPageChanged?.Invoke(sender);
    }

    private void OnCurrentPageChanged(object sender)
    {
        _shouldRefresh = true;

        CurrentPageChanged?.Invoke(sender);
    }

    // --

    private void Collection_SourceChanged(object obj, NotifyCollectionChangedEventArgs e)
    {
        _shouldRefresh = true;
    }
}
