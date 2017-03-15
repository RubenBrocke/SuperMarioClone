using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioClone
{
    public class Camera
    {
        private Viewport _viewport;
        private Vector2 Position { get; set; }
        private float Rotation { get; set; }
        private float Zoom { get; set; }
        private Vector2 Origin { get; set; }

        public Camera(Viewport viewport)
        {
            this._viewport = viewport;

            Position = Vector2.Zero;
            Rotation = 0;
            Zoom = 1;
            Origin = new Vector2(this._viewport.Width / 2f, this._viewport.Height / 2f);
        }

        public void LookAt(Vector2 _pos)
        {
            Zoom = 3.6f;
            Position = new Vector2(Math.Max(-289, _pos.X - _viewport.Width / 2), 0);
            Origin = new Vector2(_viewport.Width / 2, _viewport.Height);
        }

        public Matrix GetMatrix()
        {
            return
                Matrix.CreateTranslation(new Vector3(-Position, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }
    }
}
