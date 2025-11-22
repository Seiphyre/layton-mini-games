using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public static class ICollectionExtensions
{
    public static int AddAll<T>(this ICollection<T> collection, IEnumerable<T> itemsToAdd)
    {
        foreach (var itemToAdd in itemsToAdd)
        {
            collection.Add(itemToAdd);
        }

        return itemsToAdd.Count();
    }

    public static int RemoveAll<T>(this ICollection<T> collection)
    {
        var itemsToRemove = collection.ToList();

        foreach (var itemToRemove in itemsToRemove)
        {
            collection.Remove(itemToRemove);
        }

        return itemsToRemove.Count;
    }

    public static int RemoveAll<T>(this ICollection<T> collection, IEnumerable<T> itemsToRemove)
    {
        foreach (var itemToRemove in itemsToRemove)
        {
            collection.Remove(itemToRemove);
        }

        return itemsToRemove.Count();
    }

    public static int RemoveAll<T>(this ICollection<T> collection, Func<T, bool> condition)
    {
        var itemsToRemove = collection.Where(condition).ToList();

        foreach (var itemToRemove in itemsToRemove)
        {
            collection.Remove(itemToRemove);
        }

        return itemsToRemove.Count;
    }
}
