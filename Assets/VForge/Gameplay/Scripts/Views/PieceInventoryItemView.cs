using UnityEngine;
using UnityEngine.UI;
using VForge.BoardPieces.Definitions;
using VForge.BoardPieces.Views;
using VForge.Inventories;
using VForge.Inventories.UI;

namespace VForge.Gameplay
{
    /// <summary>
    /// Visual representation of a single piece inventory item.
    /// </summary>
    public sealed class PieceInventoryItemView : InventoryItemView<PieceDefinition>
    {
        [SerializeField] private PieceDefinitionView definitionView;
        [SerializeField] private float blockSize = 64;



        public void Initialize(PieceDefinition pieceDefinition)
        {
            definitionView.Initialize(pieceDefinition, blockSize);
        }




        // --

        protected override void OnBind()
        {
            definitionView.Initialize(TypedItem.Data, blockSize);
        }

        protected override void OnUnbind()
        {
            // Clear visuals
        }
    }
}
