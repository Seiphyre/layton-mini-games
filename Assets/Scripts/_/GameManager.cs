using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelData LevelInfo;
    [SerializeField] private Shop Shop = new Shop();

    [SerializeField] private ShopItemList ItemList;
    //[SerializeField] private ListView OverviewList;

    [SerializeField] private BoardView Board;


    // --------------------------------------------------------

    private IEnumerator Start()
    {
        Shop.Initialize(LevelInfo.Items, LevelInfo.Purchases);

        // --

        ItemList.SetList(Shop.Items);

        //ItemList.ClearBindings();
        //ItemList.BindProperty<ShopItem, ShopItem, ShopItemView, ShopItem>((x) => x, (x) => x.Value);

        // --

        //OverviewList.Source = Shop.Items;

        //OverviewList.ClearBindings();
        //OverviewList.BindProperty<ShopItem, ShopItem, ShopItemView, ShopItem>((x) => x, (x) => x.Value);

        //if (OverviewList.HasSelection)
        //{
        //    OverviewList.Selection.SelectionMode = SelectionMode.MultiSelection;
        //    OverviewList.Selection.Select(Shop.Items.Take(ItemList.Pagination.ItemPerPage));
        //}

        foreach(var item in Shop.Purchases)
        {
            var piece = Board.CreatePiece(item.Item);

            Board.MovePiece(piece, new Vector2Int(item.X, item.Y));

            if (piece.IsDraggable)
                Destroy(piece.GetComponent<Draggable>());
        }

        // --

        yield return null;



        //yield return new WaitForSeconds(1);

        //if (OverviewList.HasSelection)
        //{
        //    OverviewList.Selection.SelectionMode = SelectionMode.SingleSelection;
        //    OverviewList.Selection.Select(Shop.Items.FirstOrDefault());
        //}

        //yield return new WaitForSeconds(1);

        //if (OverviewList.HasSelection)
        //{
        //    OverviewList.Selection.SelectionMode = SelectionMode.SingleSelection;
        //    OverviewList.Selection.Select(Shop.Items.Take(3));
        //}

        //yield return new WaitForSeconds(1);

        //if (OverviewList.HasSelection)
        //{
        //    OverviewList.Selection.SelectionMode = SelectionMode.MultiSelection;
        //}

        //yield return new WaitForSeconds(1);

        //if (OverviewList.HasSelection)
        //{
        //    OverviewList.Selection.SelectionMode = SelectionMode.SingleSelection;
        //}





        //yield return new WaitForSeconds(1);

        //Shop.Purchase(Shop.Items.First(), 1, 1);

        //yield return new WaitForSeconds(2);

        //Shop.Purchase(Shop.Items.First(), 1, 1);

        //yield return new WaitForSeconds(1);

        //Shop.Purchase(Shop.Items.First(), 1, 1);

        //yield return new WaitForSeconds(1);

        //Shop.ReturnPurchase(Shop.Purchases.First());

        //yield return new WaitForSeconds(1);

        //Shop.ReturnPurchase(Shop.Purchases.First());

        //yield return new WaitForSeconds(1);

        //ItemList.Pagination.ItemPerPage = 5;

        //yield return new WaitForSeconds(1);

        //ItemList.Pagination.ItemPerPage = 7;
    }
}
