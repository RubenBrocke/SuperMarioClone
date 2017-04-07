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
        //Properties
        public Vector2 Position { get; private set; }
        private float Rotation { get; set; }
        private float Zoom { get; set; }
        private Vector2 Origin { get; set; }

        //Private fields
        private Viewport _viewport;
        private CameraState _state;
        private float _cameraSpeed;
        private Vector2 _prevPos;
        private int _cameraMargin;
        private int _veritcalCameraMargin;

        //Enumeration
        public enum CameraState
        {
            RightAlign,
            LeftAlign,
            Follow,
        }

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
            _cameraMargin = 25;
            _veritcalCameraMargin = 464;
        }

        /// <summary>
        /// Centers camera on the given position
        /// </summary>
        /// <param name="focusPos">Position camera should focus on</param>
        public void LookAt(Vector2 focusPos)
        {
            _cameraSpeed = (float)Math.Sqrt(Math.Abs(focusPos.X - Position.X - (_viewport.Width / 2)));

            //Switch states
            switch (_state)
            {
                case CameraState.RightAlign:
                    if (focusPos.X - _viewport.Width / 2 + _cameraMargin > Global.Instance.MainGame.currentLevel.Width - 289)
                    {
                        _state = CameraState.Follow;
                    }
                    break;
                case CameraState.LeftAlign:
                    if (focusPos.X - _viewport.Width / 2  - _cameraMargin > -289)
                    {
                        _state = CameraState.Follow;
                    }
                    break;
                case CameraState.Follow:
                    if (focusPos.X - _viewport.Width / 2 < -289)
                    {
                        _state = CameraState.LeftAlign;
                    }
                    else if (focusPos.X - _viewport.Width / 2 > Global.Instance.MainGame.currentLevel.Width - 289)
                    {
                        _state = CameraState.RightAlign;
                    }
                    break;
            }

            float newX = Position.X;
            float newY = Position.Y;

            switch (_state)
            {
                case CameraState.RightAlign:
                    newX = Global.Instance.MainGame.currentLevel.Width - 289;
                    break;
                case CameraState.LeftAlign:
                    newX = -289;
                    break;
                case CameraState.Follow:
                    if (Position.X < focusPos.X - _viewport.Width / 2 - _cameraSpeed)
                    {
                        newX = Position.X + _cameraSpeed;
                    }
                    else if (Position.X > focusPos.X - _viewport.Width / 2 + _cameraSpeed)
                    {
                        newX = Position.X - _cameraSpeed;
                    }
                    else
                    {
                        newX = focusPos.X - _viewport.Width / 2;
                    }
                    Origin = new Vector2(_viewport.Width / 2, _viewport.Height);
                    break;               
            }

            //Check y to see if the camera has to be moved upwards
            if (focusPos.Y < _veritcalCameraMargin)
            {
                newY = focusPos.Y - 566;
            }
            else
            {
                newY = -100;
            }

            Position = new Vector2(newX, newY);
            _prevPos = focusPos;
            Console.WriteLine(focusPos.Y);
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
