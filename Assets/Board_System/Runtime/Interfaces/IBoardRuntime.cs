namespace BoardSystem
{
    public interface IBoardRuntime : IBoardReadOnly
    {
        bool TryAddPiece(int x, int y);
        bool TryMovePiece(Piece piece, int newX, int newY);
        bool TryRemovePiece(int x, int y);
    }
}
