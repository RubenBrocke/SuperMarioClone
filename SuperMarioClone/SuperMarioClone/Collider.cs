using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace SuperMarioClone
{
    public class Collider
    {
        private Action<Tangible> _whenHit;
        private Rectangle _hitbox;
        private bool _isSolid;
        private CollisionDirection _collDirection;
        private Tangible _collisionObject;

        public enum CollisionDirection
        {
            Top,
            Bottom,
            Left,
            Right,
            None            
        }

        public Collider(int x, int y, int width, int height, Tangible collisionObject, bool isSolid, Action<Tangible> whenHit)
        {
            _hitbox = new Rectangle(x, y, width, height);
            _whenHit = whenHit;
            _isSolid = isSolid;
            _collisionObject = collisionObject;
        }

        public bool CheckCollision(int x, int y, out CollisionDirection collDirection)
        {
            bool returnBool = false;
            collDirection = CollisionDirection.None; 
            foreach (Tangible t in MainGame.currentLevel.GameObjects)
            {
                Rectangle testRect = new Rectangle(t.collider._hitbox.X - x, t.collider._hitbox.Y - y, t.collider._hitbox.Width, t.collider._hitbox.Height);
                if (_hitbox.Intersects(testRect) && t != _collisionObject)
                {
                    //Check if solid
                    if (_isSolid)
                    {
                        returnBool = true;
                    }
                    //Check if above
                    if (_hitbox.Bottom < t.collider._hitbox.Top)
                    {
                        collDirection = CollisionDirection.Top;
                    }
                    //Check below
                    else if (_hitbox.Top > t.collider._hitbox.Bottom)
                    {
                        collDirection = CollisionDirection.Bottom;
                    }
                    //Check left
                    else if (_hitbox.Right < t.collider._hitbox.Left)
                    {
                        collDirection = CollisionDirection.Left;
                    }
                    else if (_hitbox.Left > t.collider._hitbox.Right)
                    {
                        collDirection = CollisionDirection.Right;
                    }
                    _whenHit(t);
                }
            }
            return returnBool;
        }
    }
}
