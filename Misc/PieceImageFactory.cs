#region Using statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TempleGardens
{
    public class PieceImageFactory
    {
        public static readonly int Size = 16;
        private static GraphicsDevice _device;

        public static void Init(GraphicsDevice device)
        {
            _device = device;
        }

        public static Texture2D BuildImage(byte[,] shape)
        {
            Texture2D iconTexture = new Texture2D(
                _device, shape.GetLength(0) * Size, shape.GetLength(1) * Size, false, SurfaceFormat.Color);
            Color[] color = new Color[(shape.GetLength(0) * Size) * (shape.GetLength(1) * Size)];

            for (var x = 0; x < shape.GetLength(0); x++)
                for (var y = 0; y < shape.GetLength(1); y++)
                {
                    for (var u = x * Size; u < x * Size + Size; u++)
                        for (var v = y * Size; v < y * Size + Size; v++)
                        {

                            if (shape[x, y] == 1)
                                color[v * (shape.GetLength(0) * Size) + u] = Color.FromNonPremultiplied(0, 255, 0, 255);
                            else if (shape[x, y] > 1)
                                color[v * (shape.GetLength(0) * Size) + u] = Color.FromNonPremultiplied(255, 0, 0, 255);
                            else
                                color[v * (shape.GetLength(0) * Size) + u] = Color.Transparent;
                        }
                }

            iconTexture.SetData(color);

            return iconTexture;
        }
    }
}
