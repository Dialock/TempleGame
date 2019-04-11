#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TempleGardens
{
    public abstract class Control
    {
        #region Properties

        public byte ID { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 Position { get; set; }
        public bool HasFocus { get; set; }
        public bool Enabled { get; set; }
        public bool Visible { get; set; }
        public bool TabStop { get; set; }
        public SpriteFont Font { get; set; }
        public Color Color { get; set; }
        public float DrawLayer { get; set; }

        #endregion

        #region events

        public event EventHandler Selected;

        #endregion

        public Control()
        {
            Color = Color.White;
            Enabled = true;
            Font = ControlManager.SpriteFont;
            Visible = true;
            DrawLayer = 0.0f;
        }

        #region Abstracted XNA Regions

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        //public abstract void PreDraw();
        public abstract void HandleInput();

        #endregion

        protected virtual void OnSelected(EventArgs e)
        {
            if (Selected != null)
                Selected(this, e);
        }
    }
}
