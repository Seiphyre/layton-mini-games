using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Shop
{
    public List<ShopItem> InitialItems { get; private set; }
    public List<BoardElement> InitialPurchases { get; private set; }



    public ObservableCollection<ShopItem> Items { get; private set; }
    public ObservableCollection<BoardElement> Purchases { get; private set; }



    // -------------------------------------------------

    public void Initialize(IEnumerable<ShopItem> items, IEnumerable<BoardElement> purchases)
    {
        InitialItems = new List<ShopItem>(items);
        Items = new ObservableCollection<ShopItem>(items);

        InitialPurchases = new List<BoardElement>(purchases);
        Purchases = new ObservableCollection<BoardElement>(purchases);
    }

    public void Reinitialize()
    {
        Items = new ObservableCollection<ShopItem>(InitialItems);
        Purchases = new ObservableCollection<BoardElement>(InitialPurchases);
    }



    // -------------------------------------------------

    public void Purchase(ShopItem item, float x, float y)
    {
        Debug.Log("Purchase");

        //var purchase = new BoardPiece(item, x, y);

        //Purchases.Add(purchase);

        //// --

        //Items.Remove(item);
    }

    public void ReturnPurchase(BoardElement purchase)
    {
        Debug.Log("Return Purchase");

        //Items.Add(purchase.Item);

        //// --

        //Purchases.Remove(purchase);
    }
}
