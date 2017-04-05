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
        private CameraState _state;
        private float _cameraSpeed;
        private Vector2 _prevPos;

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
            _state = CameraState.Follow;
            _cameraSpeed = 10;
            _prevPos = new Vector2(0, 0);
        }

        /// <summary>
        /// Centers camera on the given position
        /// </summary>
        /// <param name="_pos">Position camera should focus on</param>
        public void LookAt(Vector2 _pos)
        {
            _cameraSpeed = (float)Math.Sqrt(_pos.X - Position.X - (_viewport.Width / 2));

            //Switch states
            switch (_state)
            {
                case CameraState.RightAlign:
                    if (_pos.X - _viewport.Width / 2 + 50 > Global.Instance.MainGame.currentLevel.Width - 289)
                    {
                        _state = CameraState.Follow;
                        Console.WriteLine("SWITCHED TO FOLLOW");
                    }
                    break;
                case CameraState.LeftAlign:
                    if (_pos.X - _viewport.Width / 2  - 50 > -289)
                    {
                        _state = CameraState.Follow;
                        Console.WriteLine("SWITCHED TO FOLLOW");
                    }
                    break;
                case CameraState.Follow:
                    if (_pos.X - _viewport.Width / 2 < -289)
                    {
                        _state = CameraState.LeftAlign;
                        Console.WriteLine("SWITCHED TO LEFT ALIGN");
                    }
                    else if (_pos.X - _viewport.Width / 2 > Global.Instance.MainGame.currentLevel.Width - 289)
                    {
                        _state = CameraState.RightAlign;
                        Console.WriteLine("SWITCHED TO RIGHT ALIGN");
                    }
                    break;
            }

            switch (_state)
            {
                case CameraState.RightAlign:
                    Position = new Vector2(Global.Instance.MainGame.currentLevel.Width - 289, -100);
                    Origin = new Vector2(_viewport.Width / 2, _viewport.Height);
                    break;
                case CameraState.LeftAlign:
                    Position = new Vector2(-289, -100);
                    Origin = new Vector2(_viewport.Width / 2, _viewport.Height);
                    break;
                case CameraState.Follow:
                    if (Position.X < _pos.X - _viewport.Width / 2 - _cameraSpeed)
                    {
                        Position = new Vector2(Position.X + _cameraSpeed, -100);
                        Console.WriteLine("LEFT OF MARIO" + _cameraSpeed);
                    }
                    else if (Position.X > _pos.X - _viewport.Width / 2 + _cameraSpeed)
                    {
                        Position = new Vector2(Position.X - _cameraSpeed, -100);
                        Console.WriteLine("RIGHT OF MARIO" + _cameraSpeed);
                    }
                    else
                    {
                        Position = new Vector2(_pos.X - _viewport.Width / 2, -100); //Y = _pos.Y - 534 to follow player sort of
                        Console.WriteLine("ON MARIO");
                    }
                    Origin = new Vector2(_viewport.Width / 2, _viewport.Height);
                    break;               
            }

            _prevPos = _pos;
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

    public enum CameraState
    {
        RightAlign,
        LeftAlign,
        Follow,
    }
}
