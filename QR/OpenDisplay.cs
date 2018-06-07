using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using QR.Drawing.Util;

namespace QR.Drawing.Open
{
    class OpenDisplay : GameWindow
    {
        /// <summary>
        /// Texture to display.
        /// </summary>
        public OpenTexture Texture { get; set; }
        Vector2 TextureDisplaySize { get; set; }

        public OpenDisplay(Bitmap bmp) : this(bmp, bmp.Width, bmp.Height, Default.WINDOW_TITLE) { }
        public OpenDisplay(Bitmap bmp, int window_width, int window_height)
            : this(bmp, window_width, window_height, Default.WINDOW_TITLE)
        { }
        public OpenDisplay(Bitmap bmp, int window_width, int window_height, string title) : base(window_width, window_height)
        {
            Title = title;
            GL.Enable(EnableCap.Texture2D);

            Texture = new OpenTexture(bmp);
            UpdateTextureDisplaySize();
            UpdateWindowLocation();
        }

        //Override Methods
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Default.WINDOW_BG);
            GL.BindTexture(TextureTarget.Texture2D, Texture.ID);
            DisplayImage();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateTextureDisplaySize();
        }

        //Private Methods
        private void DisplayImage()
        {
            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0, 0);
            GL.Vertex2(-TextureDisplaySize.X, TextureDisplaySize.Y);

            GL.TexCoord2(1, 0);
            GL.Vertex2(TextureDisplaySize.X, TextureDisplaySize.Y);

            GL.TexCoord2(1, 1);
            GL.Vertex2(TextureDisplaySize.X, -TextureDisplaySize.Y);

            GL.TexCoord2(0, 1);
            GL.Vertex2(-TextureDisplaySize.X, -TextureDisplaySize.Y);

            GL.End();

            SwapBuffers();
        }

        /// <summary>
        /// Update texture display size parameter to guarantee the texture could display completely and max in the window.
        /// </summary>
        private void UpdateTextureDisplaySize()
        {
            float factor_x = 0f;
            float factor_y = 0f;
            float window_aspect_ratio = (float)Width / (float)Height;
            float texture_aspect_ratio = (float)Texture.SourceImage.Width / (float)Texture.SourceImage.Height;

            if (window_aspect_ratio > texture_aspect_ratio)
            {
                factor_y = 1;
                factor_x = (float)Texture.SourceImage.Width * (float)Height / (float)Texture.SourceImage.Height / Width;
            }
            else if (window_aspect_ratio < texture_aspect_ratio)
            {
                factor_x = 1;
                factor_y = (float)Texture.SourceImage.Height * (float)Width / (float)Texture.SourceImage.Width / Height;
            }
            else
            {
                factor_x = factor_y = 1;
            }

            TextureDisplaySize = new Vector2(factor_x, factor_y);
        }

        private void UpdateWindowLocation()
        {
            Location = new Point(400, 400);
        }
    }
}
