using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Shop
{
    public List<PieceData> InitialItems { get; private set; }
    public List<BoardElement> InitialPurchases { get; private set; }



    public ObservableCollection<PieceData> Items { get; private set; }
    public ObservableCollection<BoardElement> Purchases { get; private set; }



    // -------------------------------------------------

    public void Initialize(IEnumerable<PieceData> items, IEnumerable<BoardElement> purchases)
    {
        InitialItems = new List<PieceData>(items);
        Items = new ObservableCollection<PieceData>(items);

        InitialPurchases = new List<BoardElement>(purchases);
        Purchases = new ObservableCollection<BoardElement>(purchases);
    }

    public void Reinitialize()
    {
        Items = new ObservableCollection<PieceData>(InitialItems);
        Purchases = new ObservableCollection<BoardElement>(InitialPurchases);
    }



    // -------------------------------------------------

    public void Purchase(PieceData item, float x, float y)
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
