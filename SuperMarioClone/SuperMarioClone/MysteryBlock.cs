using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SuperMarioClone
{
    class MysteryBlock : Solid
    {
        private Type _mysteryObject { get; set; }
        private ContentManager _cm;
        private bool _hasBeenUsed = false;
        private int _spriteImageIndex = 0;
        private Timer _timer;

        public MysteryBlock(int _x, int _y, Level lvl, ContentManager cm, Type MysteryObject) : base()
        {
            //TODO: add animation later
            _mysteryObject = MysteryObject;
            position = new Vector2(_x, _y);
            currentLevel = lvl;
            _cm = cm;
            sprite = _cm.Load<Texture2D>("MysteryBlockSheet");
            hitbox = new Rectangle((int)position.X, (int)position.Y, 16, 16); // TODO: numbers represent pixels, change magic number

            _timer = new Timer(ChangeSpriteIndex);
            _timer.Change(0, 120);
        }

        public void Eject(Mario mario, float vY)
        {
            if (vY < 0 && !_hasBeenUsed)
            {
                GameObject o = (GameObject)Activator.CreateInstance(_mysteryObject, (int)position.X, (int)position.Y - hitbox.Height, true, currentLevel, _cm);
                currentLevel.ToAddGameObject(o);
                _hasBeenUsed = true;
                if (o.GetType() == typeof(Coin))
                {
                    object state;
                    Coin c = (Coin)o;
                    c.AddCoin(mario);
                }
            } 
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture: sprite, position: position, sourceRectangle: new Rectangle(16 * _spriteImageIndex, 0, 16, 16));
        }

        private void ChangeSpriteIndex(object state)
        {
            if (_spriteImageIndex < 3)
            {
                _spriteImageIndex++;
            }
            else
            {
                _spriteImageIndex = 0;
            }
            if (_hasBeenUsed)
            {
                _spriteImageIndex = 4;
            }
        }
    }
}
