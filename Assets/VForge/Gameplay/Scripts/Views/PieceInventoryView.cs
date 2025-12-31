using UnityEngine;
using VForge.BoardPieces.Definitions;
using VForge.Inventories;
using VForge.Inventories.UI;

namespace VForge.Gameplay
{
    /// <summary>
    /// Inventory view for board pieces.
    /// Inspector-safe concrete implementation.
    /// </summary>
    public sealed class PieceInventoryView : InventoryView<PieceDefinition>
    {
        [SerializeField]
        private PieceInventoryItemView _itemViewPrefab;

        [SerializeField]
        private Transform _contentRoot;




        protected override InventoryItemView<PieceDefinition> CreateItemView()
        {
            var view = Instantiate(_itemViewPrefab);

            view.transform.SetParent(_contentRoot, false);
            view.transform.SetAsLastSibling();

            return view;
        }
    }
}
