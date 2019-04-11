#region Using Statments

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using System.Linq;
#endregion

namespace TempleGardens
{
	public class InputHandler : GameComponent
    {
        //static GamePadState gp;
        //static GamePadState lgp;

        //public static readonly Buttons[] _anyButtons = new Buttons[]
        //{
        //    Buttons.A, Buttons.B, Buttons.Back, Buttons.DPadDown, Buttons.DPadLeft, Buttons.LeftShoulder
        //};

        #region Keyboard Field Region

//        static KeyboardState keyboardState;
//        static KeyboardState lastKeyboardState;
//
        #endregion

        #region Mouse Field Region

//        static MouseState mouseState;
//        static MouseState lastMouseState;
//


        public static TouchCollection TouchPanelState;
//		static TouchCollection lastTouchState;



        #endregion

        #region Initiliaze

        public InputHandler(Game game)
            : base(game)
        {
//            keyboardState = Keyboard.GetState();

//            mouseState = Mouse.GetState();

            //if (GamePad.GetState(PlayerIndex.One).IsConnected)
            //    gp = GamePad.GetState(PlayerIndex.One);

             TouchPanelState = TouchPanel.GetState ();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {
//            lastKeyboardState = keyboardState;
//            keyboardState = Keyboard.GetState();
//
//            lastMouseState = mouseState;
//            mouseState = Mouse.GetState();

            //lgp = gp;
            //gp = GamePad.GetState(PlayerIndex.One);

//			lastTouchState = touchPanelState;
            TouchPanelState = TouchPanel.GetState ();

            base.Update(gameTime);
        }

        #endregion

        #region Keyboard Function Region

//        public static bool KeyReleased(Keys key)
//        {
//            return keyboardState.IsKeyUp(key) &&
//                lastKeyboardState.IsKeyDown(key);
//        }
//
//        public static bool KeyPressed(Keys key)
//        {
//            return keyboardState.IsKeyDown(key) &&
//                lastKeyboardState.IsKeyUp(key);
//        }
//
//        public static bool KeyDown(Keys key)
//        {
//            return keyboardState.IsKeyDown(key);
//        }

        #endregion

		#region Touch Region



        public static bool IsScreenBeingTouched()
        {
            if (TouchPanelState.Count >= 1) {
                var touch = TouchPanelState [0];

                if (touch.State == TouchLocationState.Pressed || touch.State == TouchLocationState.Moved) {
                    return true;
                }
            }

            return false;
        }

        public static bool FingerRaised()
        {
            if (TouchPanelState.Count >= 1) {
                var touch = TouchPanelState [0];

                if (touch.State == TouchLocationState.Released) {

                    return true;
                }
            }

            return false;
        }

        public static Vector2 TouchVector() {
         {
                if (TouchPanelState.Count >= 1) {

                    var touch = TouchPanelState [0];

                    return touch.Position;
                } else
                    return Vector2.Zero;
            }
        }
        public static Vector2 TouchVectorScaled
        {
            get { return Vector2.Transform(TouchVector(), Matrix.Invert(ResolutionHelper.ScaleMatrix)); }
        }



		#endregion

        #region Mouse Function Region

//        public static void SetMouseCursor(int x, int y)
//        {
//            Mouse.SetPosition(x, y);
//            lastMouseState = Mouse.GetState();
//        }
//
//        public static bool LeftButtonReleased()
//        {
//            return mouseState.LeftButton == ButtonState.Released &&
//                lastMouseState.LeftButton == ButtonState.Pressed;
//        }
//
//        public static bool RightButtonReleased()
//        {
//            return mouseState.RightButton == ButtonState.Released &&
//                lastMouseState.RightButton == ButtonState.Pressed;
//        }
//
//        public static bool LeftButtonPressed()
//        {
//            return mouseState.LeftButton == ButtonState.Pressed &&
//                lastMouseState.LeftButton == ButtonState.Released;
//        }
//
//        public static bool RightButtonPressed()
//        {
//            return mouseState.RightButton == ButtonState.Pressed &&
//                lastMouseState.RightButton == ButtonState.Released;
//        }
//
//        public static bool LeftButtonDown()
//        {
//            return mouseState.LeftButton == ButtonState.Pressed;
//        }
//
//        public static bool RightButtonDown()
//        {
//            return mouseState.RightButton == ButtonState.Pressed;
//        }
//
//        public static Point MousePoint
//        {
//            get { return new Point(mouseState.X, mouseState.Y); }
//        }
//
//        public static Vector2 MouseVector
//        {
//            get { return new Vector2(mouseState.X, mouseState.Y); }
//        }
//
//        public static Vector2 LastMouseVector
//        {
//            get { return new Vector2(lastMouseState.X, lastMouseState.Y); }
//        }
//
//        public static Rectangle MouseRectangle
//        {
//            get { return new Rectangle((int)mouseState.X, (int)mouseState.Y, 1, 1); }
//        }
//
//        public static Vector2 MousePositionScaled
//        {
//            get { return Vector2.Transform(MouseVector, Matrix.Invert(ResolutionHelper.ScaleMatrix)); }
//        }
//
//        public static float Y()
//        {
//            return (float)mouseState.Y;
//        }
//
//        public static float X()
//        {
//            return (float)mouseState.X;
//        }
//
//        public static float ScrollWheelValue()
//        {
//            return (float)mouseState.ScrollWheelValue;
//        }
//
//        public static float DeltaY()
//        {
//            return (float)(lastMouseState.Y - mouseState.Y);
//        }
//
//        public static float DeltaX()
//        {
//            return (float)(lastMouseState.X - mouseState.X);
//        }
//
//        public static float DeltaScrollWheelValue()
//        {
//            return (float)(lastMouseState.ScrollWheelValue - mouseState.ScrollWheelValue);
//        }
//
//        #endregion
//
//        #region General Method Region
//
//        public static void Flush()
//        {
//            lastKeyboardState = keyboardState;
//        }
//
//        #endregion
//
//        #region Keyboard Property Region
//
//        public static KeyboardState KeyboardState
//        {
//            get { return keyboardState; }
//        }
//
//        public static KeyboardState LastKeyboardState
//        {
//            get { return lastKeyboardState; }
//        }
//
//        #endregion
//
//        #region Mouse Properties
//
//        public static MouseState MouseState
//        {
//            get { return mouseState; }
//            set { mouseState = value; }
//        }
//
//        public static MouseState LastMouseState
//        {
//            get { return lastMouseState; }
//        }
//
        #endregion

        #region Key Convertion
//        public static char ConvertKeyToChar(Keys key, bool shift)
//        {
//
//
//            switch (key)
//            {
//                case Keys.Space: return ' ';
//
//                // Escape Sequences
//                case Keys.Enter: return '\n';
//                case Keys.Tab: return '\t';
//
//                // Upper Numerics
//                case Keys.D0: return shift ? ')' : '0';
//                case Keys.D1: return shift ? '!' : '1';
//                case Keys.D2: return shift ? '@' : '2';
//                case Keys.D3: return shift ? '#' : '3';
//                case Keys.D4: return shift ? '$' : '4';
//                case Keys.D5: return shift ? '%' : '5';
//                case Keys.D6: return shift ? '^' : '6';
//                case Keys.D7: return shift ? '&' : '7';
//                case Keys.D8: return shift ? '*' : '8';
//                case Keys.D9: return shift ? '(' : '9';
//
//                // Numpad 
//                case Keys.NumPad0: return '0';
//                case Keys.NumPad1: return '1';
//                case Keys.NumPad2: return '2';
//                case Keys.NumPad3: return '3';
//                case Keys.NumPad4: return '4';
//                case Keys.NumPad5: return '5';
//                case Keys.NumPad6: return '6';
//                case Keys.NumPad7: return '7';
//                case Keys.NumPad8: return '8';
//                case Keys.NumPad9: return '9';
//                case Keys.Add: return '+';
//                case Keys.Subtract: return '-';
//                case Keys.Multiply: return '*';
//                case Keys.Divide: return '/';
//                case Keys.Decimal: return '.';
//
//                // Alphabet 
//                case Keys.A: return shift ? 'A' : 'a';
//                case Keys.B: return shift ? 'B' : 'b';
//                case Keys.C: return shift ? 'C' : 'c';
//                case Keys.D: return shift ? 'D' : 'd';
//                case Keys.E: return shift ? 'E' : 'e';
//                case Keys.F: return shift ? 'F' : 'f';
//                case Keys.G: return shift ? 'G' : 'g';
//                case Keys.H: return shift ? 'H' : 'h';
//                case Keys.I: return shift ? 'I' : 'i';
//                case Keys.J: return shift ? 'J' : 'j';
//                case Keys.K: return shift ? 'K' : 'k';
//                case Keys.L: return shift ? 'L' : 'l';
//                case Keys.M: return shift ? 'M' : 'm';
//                case Keys.N: return shift ? 'N' : 'n';
//                case Keys.O: return shift ? 'O' : 'o';
//                case Keys.P: return shift ? 'P' : 'p';
//                case Keys.Q: return shift ? 'Q' : 'q';
//                case Keys.R: return shift ? 'R' : 'r';
//                case Keys.S: return shift ? 'S' : 's';
//                case Keys.T: return shift ? 'T' : 't';
//                case Keys.U: return shift ? 'U' : 'u';
//                case Keys.V: return shift ? 'V' : 'v';
//                case Keys.W: return shift ? 'W' : 'w';
//                case Keys.X: return shift ? 'X' : 'x';
//                case Keys.Y: return shift ? 'Y' : 'y';
//                case Keys.Z: return shift ? 'Z' : 'z';
//
//                // Oem 
//                case Keys.OemOpenBrackets: return shift ? '{' : '[';
//                case Keys.OemCloseBrackets: return shift ? '}' : ']';
//                case Keys.OemComma: return shift ? '<' : ',';
//                case Keys.OemPeriod: return shift ? '>' : '.';
//                case Keys.OemMinus: return shift ? '_' : '-';
//                case Keys.OemPlus: return shift ? '+' : '=';
//                case Keys.OemQuestion: return shift ? '?' : '/';
//                case Keys.OemSemicolon: return shift ? ':' : ';';
//                case Keys.OemQuotes: return shift ? '\'' : '"';
//                case Keys.OemPipe: return shift ? '|' : '\\';
//                case Keys.OemTilde: return shift ? '~' : '`';
//            }

//            return '?';
	
        }

        #endregion


        //public static bool ButtonReleased(Buttons button)
        //{
        //    return gp.IsButtonUp(button) &&
        //        lgp.IsButtonDown(button);
        //}

        //public static bool ButtonPressed(Buttons button)
        //{
        //    return gp.IsButtonDown(button) &&
        //        lgp.IsButtonUp(button);
        //}

        //public static bool ButtonDown(Buttons button)
        //{
        //    return gp.IsButtonDown(button);
        //}

    
}
