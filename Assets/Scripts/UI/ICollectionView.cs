using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public interface ICollectionView
{
    public ICollection Source { get; }
    public IEnumerable<VisualElement> Items { get; }

    public event Action<object, NotifyCollectionChangedEventArgs> SourceChanged;
    public event Action<object, NotifyCollectionChangedEventArgs> ItemsChanged;
}
