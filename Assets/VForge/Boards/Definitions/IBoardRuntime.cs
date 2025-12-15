using VForge.Boards.Runtime;

namespace VForge.Boards.Definitions
{
    public interface IBoardRuntime : IBoardReadOnly
    {
        bool TryAddPiece(int x, int y);
        bool TryMovePiece(BPiece piece, int newX, int newY);
        bool TryRemovePiece(int x, int y);
    }
}
