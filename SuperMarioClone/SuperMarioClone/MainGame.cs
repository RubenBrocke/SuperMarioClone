﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Windows.Forms;

namespace SuperMarioClone
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game
    {
        public Mario mario;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private LevelReader _levelReader;
        public Level currentLevel;
        private GraphicalUserInterface _graphicalUserInterface;
        public Camera camera;
        private Texture2D _background;
        public Sound sound;
        public bool gameOver;
        
        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = 700;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            gameOver = false;
            sound = new Sound(Content);
            Global.Instance.MainGame = this;
            _levelReader = new LevelReader(Content);
            currentLevel = _levelReader.ReadLevel(0);
            mario = new Mario(0, 32, currentLevel, Content);
            currentLevel.ToAddGameObject(mario);
            camera = new Camera(GraphicsDevice.Viewport);
            _graphicalUserInterface = new GraphicalUserInterface(mario, Content);
            System.Windows.Forms.Form form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(this.Window.Handle);
            form.Location = new System.Drawing.Point(Screen.PrimaryScreen.Bounds.Width / 2 - GraphicsDevice.Viewport.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - GraphicsDevice.Viewport.Height / 2);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _background = Content.Load<Texture2D>("BackGround");       
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!gameOver)
            {
                sound.CheckInput();
                camera.LookAt(mario.Position);
                if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                    Exit();
                currentLevel.UpdateLevel();
                base.Update(gameTime); 
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {            
            if (!gameOver)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                _spriteBatch.Draw(_background, new Rectangle(-(int)camera.Position.X - GraphicsDevice.Viewport.Width / 2, - 800, 2000, 1600), Color.White);
                _spriteBatch.Draw(_background, new Rectangle(-(int)camera.Position.X - GraphicsDevice.Viewport.Width / 2 + 2000, -800, 2000, 1600), Color.White);
                _spriteBatch.End();
                _spriteBatch.Begin(transformMatrix: camera.GetMatrix(), samplerState: SamplerState.PointClamp);
                currentLevel.DrawLevel(_spriteBatch);
                _spriteBatch.End();
                _graphicalUserInterface.Draw(_spriteBatch);
                base.Draw(gameTime);
            }
            else
            {
                _spriteBatch.Begin();
                GraphicsDevice.Clear(Color.Black);
                _graphicalUserInterface.DrawBorderedText(_spriteBatch, "Game Over", Color.Black, Color.Red, new Vector2(GraphicsDevice.Viewport.Width / 2f, GraphicsDevice.Viewport.Height / 2f), true);
                _spriteBatch.End();
            }
        }

        public void ChangeCurrentLevel(Level level)
        {
            currentLevel.DisableTimer();
            currentLevel = level;
            mario.ChangeCurrentLevel(currentLevel);
        }
    }
}
