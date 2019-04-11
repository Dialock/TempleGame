#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace TempleGardens
{
    public class ControlManager : List<Control>
    {
        public event EventHandler FocusChanged;

        int selectedControl = 0;

        public static SpriteFont SpriteFont { get; set; }

        #region Constructors

        public ControlManager(SpriteFont font)
            : base()
        {
            SpriteFont = font;
        }

        public ControlManager(int capacity, SpriteFont font)
            : base(capacity)
        {
            SpriteFont = font;
        }

        public ControlManager(IEnumerable<Control> collection, SpriteFont font)
            : base(collection)
        {
            SpriteFont = font;
        }

        #endregion

        #region Methods

        public void Update(GameTime gameTime)
        {
            if (Count == 0)
                return;

            foreach (Control c in this)
            {
                if (c.Enabled)
                    c.Update(gameTime);

                if (c.HasFocus)
                    c.HandleInput();
            }

//            if (InputHandler.KeyReleased(Keys.Up))
//                PreviousControl();
//
//            if (InputHandler.KeyReleased(Keys.Down))
//                NextControl();

        }


        //public void PreDraw()
        //{
        //    foreach (Control c in this)
        //    {
        //        if (c is TextBox)
        //            c.PreDraw();
        //    }
        //}

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Control c in this)
            {
                if (c.Visible)
                    c.Draw(spriteBatch);
            }
        }

        #endregion

        public void NextControl()
        {
            if (Count == 0)
                return;

            int currentControl = selectedControl;

            this[selectedControl].HasFocus = false;

            do
            {
                selectedControl++;

                if (selectedControl == Count)
                    selectedControl = 0;

                if (this[selectedControl].TabStop && this[selectedControl].Enabled)
                {
                    if (FocusChanged != null)
                        FocusChanged(this[selectedControl], null);
                    break;
                }
            } while (currentControl != selectedControl);

            this[selectedControl].HasFocus = true;
        }

        public void PreviousControl()
        {
            if (Count == 0)
                return;

            var currentControl = selectedControl;

            this[selectedControl].HasFocus = false;

            do
            {
                selectedControl--;

                if (selectedControl < 0)
                    selectedControl = Count - 1;

                if (this[selectedControl].TabStop && this[selectedControl].Enabled)
                {
                    if (FocusChanged != null)
                        FocusChanged(this[selectedControl], null);
                    break;
                }
            } while (currentControl != selectedControl);

            this[selectedControl].HasFocus = true;
        }
    }
}
