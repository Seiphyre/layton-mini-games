using UnityEditorInternal;

namespace BoardSystem
{
    public class Wall
    {
        public EdgeAxis Axis { get; }
        public int X { get; }
        public int Y { get; }



        // --------------------------------
        // Constructors
        // --------------------------------

        internal Wall(EdgeAxis axis, int x, int y)
        {
            this.Axis = axis;
            this.X = x;
            this.Y = y;
        }

        internal Wall(Wall other)
        {
            this.Axis = other.Axis;
            this.X = other.X;
            this.Y = other.Y;
        }



        // --------------------------------
        // Utils
        // --------------------------------

        internal Wall Clone() => new Wall(this);

        internal WallData ToSaveData()
        {
            return new WallData(Axis, X, Y);
        }
    }
}
