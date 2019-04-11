namespace TempleGardens
{
    public class PieceCell
    {
        public bool ShapeCellPresent { get;  set; }
        public bool ShapeStart { get;  set; }

        public PieceCell(int x, int y)
        {
            ShapeCellPresent = false;
            ShapeStart = false;
        }

    }
}
