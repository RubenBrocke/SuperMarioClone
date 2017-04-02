using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioClone
{
    //TODO: Make the camara work like in SMW
    public class Camera
    {
        private Viewport _viewport;
        private Vector2 Position { get; set; }
        private float Rotation { get; set; }
        private float Zoom { get; set; }
        private Vector2 Origin { get; set; }

        /// <summary>
        /// Constructor sets the values of different properties and variables
        /// </summary>
        /// <param name="viewport">The viewport of the current screen</param>
        public Camera(Viewport viewport)
        {
            _viewport = viewport;

            Position = Vector2.Zero;
            Rotation = 0;
            Zoom = 3.6f;
            Origin = new Vector2(this._viewport.Width / 2f, this._viewport.Height / 2f);
        }

        /// <summary>
        /// Centers camera on the given position
        /// </summary>
        /// <param name="_pos">Position camera should focus on</param>
        public void LookAt(Vector2 _pos)
        {
            Position = new Vector2(Math.Max(-289, _pos.X - _viewport.Width / 2), -100); //Y = _pos.Y - 534 to follow player sort of
            Origin = new Vector2(_viewport.Width / 2, _viewport.Height);
        }

        /// <summary>
        /// Returns the Camera Matrix used to transform the SpriteBatch
        /// </summary>
        /// <returns>Camera Matrix</returns>
        public Matrix GetMatrix()
        {
            return
                Matrix.CreateTranslation(new Vector3(-Position, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }

        /// <summary>
        /// Returns a Rectangle with the same bounds as the Camera
        /// </summary>
        /// <returns>Rectangle with the same bounds as the Camera</returns>
        public Rectangle GetBounds()
        {
            return new Rectangle((int)Position.X + 289, (int)Position.Y + 500, (int)(_viewport.Width / Zoom), (int)(_viewport.Height / Zoom));
        }
    }
}
