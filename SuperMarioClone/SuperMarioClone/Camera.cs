﻿using System;
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
        private Vector2 _position { get; set; }
        private float _rotation { get; set; }
        private float _zoom { get; set; }
        private Vector2 _origin { get; set; }

        public Camera(Viewport _viewport)
        {
            this._viewport = _viewport;

            _position = Vector2.Zero;
            _rotation = 0;
            _zoom = 1;
            _origin = new Vector2(this._viewport.Width / 2f, this._viewport.Height / 2f);
        }

        public void LookAt(Vector2 _pos)
        {
            _zoom = 3.6f;
            _position = new Vector2(Math.Max(-289, _pos.X - _viewport.Width / 2), 0);
            _origin = new Vector2(_viewport.Width / 2, _viewport.Height);
        }

        public Matrix GetMatrix()
        {
            return
                Matrix.CreateTranslation(new Vector3(-_position, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(-_origin, 0.0f)) *
                Matrix.CreateRotationZ(_rotation) *
                Matrix.CreateScale(_zoom, _zoom, 1) *
                Matrix.CreateTranslation(new Vector3(_origin, 0.0f));
        }
    }
}
