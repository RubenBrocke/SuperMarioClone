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
        private Type MysteryObject { get; set; }
        private ContentManager _cm;
        private bool _hasBeenUsed = false;
        private int _spriteImageIndex = 0;
        private Timer _timer;

        public MysteryBlock(int _x, int _y, Level lvl, ContentManager cm, Type MysteryObject) : base()
        {
            //TODO: add animation later
            this.MysteryObject = MysteryObject;
            Position = new Vector2(_x, _y);
            CurrentLevel = lvl;
            _cm = cm;
            Sprite = _cm.Load<Texture2D>("MysteryBlockSheet");
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, 16, 16); // TODO: numbers represent pixels, change magic number

            _timer = new Timer(ChangeSpriteIndex);
            _timer.Change(0, 120);
        }

        public void Eject(Mario mario, float vY, float Y)
        {
            if (vY < 0 && !_hasBeenUsed && Y > Hitbox.Bottom)
            {
                if (MysteryObject == typeof(Coin))
                {
                    Coin c = (Coin)Activator.CreateInstance(MysteryObject, (int)Position.X, (int)Position.Y - Hitbox.Height, true, CurrentLevel, _cm);                    
                    c.AddCoin(mario);
                    CurrentLevel.ToAddGameObject(c);
                }
                if (MysteryObject == typeof(Mushroom))
                {
                    Mushroom m = (Mushroom)Activator.CreateInstance(MysteryObject, (int)Position.X, (int)Position.Y - Hitbox.Height, CurrentLevel, _cm);
                    CurrentLevel.ToAddGameObject(m);
                }
                if (MysteryObject == typeof(LevelReader))
                {
                    LevelReader _lr = (LevelReader)Activator.CreateInstance(MysteryObject, _cm);
                    MainGame.currentLevel = _lr.ReadLevel(1);
                }
                _hasBeenUsed = true;
            } 
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture: Sprite, position: Position, sourceRectangle: new Rectangle(16 * _spriteImageIndex, 0, 16, 16));
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
