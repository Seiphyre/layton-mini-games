using UnityEditorInternal;

namespace BoardSystem
{
    public interface IBoard : IBoardRuntime
    {
        bool TryAddTile(int x, int y);
        bool TryRemoveTile(int x, int y);

        bool TryAddWall(EdgeAxis axis, int x, int y);
        bool TryRemoveWall(EdgeAxis axis, int x, int y);
    }
}
