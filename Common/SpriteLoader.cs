#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace TempleGardens
{
	public static class SpriteLoader
    {
        public static Texture2D IosMainSheet { get; private set; }
        public static Texture2D IosTextSheet { get; private set; }
        public static Texture2D IosSecondarySheet { get; private set; }
        public static Texture2D IosEnglish { get; private set; }


		private static ContentManager Content;

		public static  void Init(Game game)
        {
			Content = new ContentManager(game.Services, "Content");

            IosMainSheet = GetCalledBackground("iosMainSheet");
            IosTextSheet = GetCalledBackground("iosTextMain");
            IosSecondarySheet = GetCalledBackground("iosMiscSheet");
            IosEnglish = GetCalledBackground("iosEnglish");

        }

		public static Texture2D GetCalledShape(string shapeName)
        {
			return Content.Load<Texture2D>(@"Data\" + shapeName);
        }

		public static SpriteFont GetCalledFont(string fontName)
        {
			return Content.Load<SpriteFont>(@"Fonts\" + fontName);
        }

		public static Texture2D GetCalledBackground(string backgroundName)
        {
			return Content.Load<Texture2D>(@"Textures\" + backgroundName);
        }
    }
}
