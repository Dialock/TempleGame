//////////////////////////////////////////////////////////////////////////
////License:  The MIT License (MIT)
////Copyright (c) 2010 David Amador (http://www.david-amador.com)
////
////Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
////
////The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
////
////THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//////////////////////////////////////////////////////////////////////////

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TempleGardens
{
    public static class ResolutionHelper
    {
        private static GraphicsDeviceManager _deviceManager = null;

        private static int _width = 1280;
        private static int _height = 720;
        private static int _vWidth = 1280;
        private static int _vHeight = 720;
        public static Matrix ScaleMatrix { get; private set; }
        private static bool _fullscreen = false;
        private static bool _dirtyMatrix = true;

        /// <summary>
        /// Initialize Class
        /// </summary>
        /// <param name="device">Graphics Device</param>
        /// <param name="width">default width</param>
        /// <param name="height">default height</param>
        /// <param name="vWidth">target width</param>
        /// <param name="vHeight">target height</param>
        /// <param name="fullscreen">is fullscreen</param>
        public static void Init(GraphicsDeviceManager device, int width, int height, int vWidth, int vHeight, bool fullscreen)
        {
            _width = width;
            _height = height;
            _vWidth = vWidth;
            _vHeight = vHeight;
            _fullscreen = fullscreen;

            _deviceManager = device;

            _dirtyMatrix = true;

            ApplyResolutionSettings();
        }

        public static Matrix GetTransformationMatrix()
        {
            if (_dirtyMatrix)
                RecreateScaleMatrix();

            return ScaleMatrix;
        }

        public static void SetResolution(int width, int height, bool fullscreen)
        {
            _width = width;
            _height = height;

            _fullscreen = fullscreen;

            ApplyResolutionSettings();
        }

        public static void SetVirtualResolution(int width, int height)
        {
            _vWidth = width;
            _vHeight = height;

            _dirtyMatrix = true;
        }

        public static void UpdateResolutionSettings(int width, int height, int vWidth, int vHeight, bool fullscreen)
        {
            _width = width;
            _height = height;
            _vWidth = vWidth;
            _vHeight = vHeight;

            _fullscreen = fullscreen;

            if (!_fullscreen)
            {
                if ((_width <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width) &&
                    (_height <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
                {
                    _deviceManager.PreferredBackBufferWidth = _width;
                    _deviceManager.PreferredBackBufferHeight = _height;
                    _deviceManager.IsFullScreen = _fullscreen;
                    _deviceManager.ApplyChanges();
                }
            }
            else
            {
                foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                {
                    if ((dm.Width == _width) && (dm.Height == _height))
                    {
                        _deviceManager.PreferredBackBufferWidth = _width;
                        _deviceManager.PreferredBackBufferHeight = _height;
                        _deviceManager.IsFullScreen = _fullscreen;
                        _deviceManager.ApplyChanges();
                    }
                }
            }

            _dirtyMatrix = true;

            _width = _deviceManager.PreferredBackBufferWidth;
            _height = _deviceManager.PreferredBackBufferHeight;
        }
        
        private static void ApplyResolutionSettings()
        {
            if (!_fullscreen)
            {
                if ((_width <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width) &&
                    (_height <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
                {
                    _deviceManager.PreferredBackBufferWidth = _width;
                    _deviceManager.PreferredBackBufferHeight = _height;
                    _deviceManager.IsFullScreen = _fullscreen;
                }
            }
            else
            {
                foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                {
                    if ((dm.Width == _width) && (dm.Height == _height))
                    {
                        _deviceManager.PreferredBackBufferWidth = _width;
                        _deviceManager.PreferredBackBufferHeight = _height;
                        _deviceManager.IsFullScreen = _fullscreen;
                    }
                }
            }

            _dirtyMatrix = true;

            _width = _deviceManager.PreferredBackBufferWidth;
            _height = _deviceManager.PreferredBackBufferHeight;


            _deviceManager.ApplyChanges();
        }

        public static void BeginDraw(Color targetColor)
        {
            FullViewport();

            _deviceManager.GraphicsDevice.Clear(Color.Black);

            ResetViewport();

            _deviceManager.GraphicsDevice.Clear(targetColor);

        }

        private static void RecreateScaleMatrix()
        {
            _dirtyMatrix = false;
            ScaleMatrix = Matrix.CreateScale(
                (float)_deviceManager.GraphicsDevice.Viewport.Width / _vWidth,
                (float)_deviceManager.GraphicsDevice.Viewport.Width / _vWidth,
                1f);
        }

        public static void FullViewport()
        {
            Viewport vp = new Viewport();
            vp.X = vp.Y = 0;
            vp.Width = _width;
            vp.Height = _height;
            _deviceManager.GraphicsDevice.Viewport = vp;
        }

        public static float GetVirtualAspectRation()
        {
            return (float)_vWidth / (float)_vHeight;
        }

        public static void ResetViewport()
        {

            float targetAspectRation = GetVirtualAspectRation();

            int width = _deviceManager.PreferredBackBufferWidth;
            int height = (int)(width / targetAspectRation + .5f);
            bool changed = false;

            if (height > _deviceManager.PreferredBackBufferHeight)
            {
                height = _deviceManager.PreferredBackBufferHeight;

                width = (int)(height * targetAspectRation + .5f);
                changed = true;
            }

            Viewport viewport = new Viewport();

            viewport.X = (_deviceManager.PreferredBackBufferWidth / 2) - (width / 2);
            viewport.Y = (_deviceManager.PreferredBackBufferHeight / 2) - (height / 2);
            viewport.Width = width;
            viewport.Height = height;
            viewport.MinDepth = 0;
            viewport.MaxDepth = 1;


            if (changed)
            {
                _dirtyMatrix = true;
            }

            _deviceManager.GraphicsDevice.Viewport = viewport;
        }
    }
}
