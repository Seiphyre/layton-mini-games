using UnityEngine;
using VForge.BoardPieces.Runtime;

namespace VForge.Gameplay
{
    public static class PieceBoardInitializer
    {
        public static void LoadStartingPieces(PieceBoard board, PieceDataSet dataSet)
        {
            if (board == null || dataSet == null)
                return;

            foreach (var data in dataSet.Pieces)
            {
                if (!data.HasStartingPosition)
                    continue;

                var result = board.TryPlace(
                    data.Definition,
                    data.StartingPosition,
                    data.Locked,
                    out var piece);

                if (!result.Success)
                {
                    Debug.LogError($"Failed to place starting piece {data.Id}: {result.Reason}");
                    continue;
                }
            }
        }
    }
}
