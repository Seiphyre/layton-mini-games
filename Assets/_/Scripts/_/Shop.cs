using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Shop
{
    public List<PieceDefinition> InitialItems { get; private set; }
    public List<BoardElement> InitialPurchases { get; private set; }



    public ObservableCollection<PieceDefinition> Items { get; private set; }
    public ObservableCollection<BoardElement> Purchases { get; private set; }



    // -------------------------------------------------

    public void Initialize(IEnumerable<PieceDefinition> items, IEnumerable<BoardElement> purchases)
    {
        InitialItems = new List<PieceDefinition>(items);
        Items = new ObservableCollection<PieceDefinition>(items);

        InitialPurchases = new List<BoardElement>(purchases);
        Purchases = new ObservableCollection<BoardElement>(purchases);
    }

    public void Reinitialize()
    {
        Items = new ObservableCollection<PieceDefinition>(InitialItems);
        Purchases = new ObservableCollection<BoardElement>(InitialPurchases);
    }



    // -------------------------------------------------

    public void Purchase(PieceDefinition item, float x, float y)
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
