using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SuperMarioClone
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game
    {
        private Mario _mario;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private LevelReader _levelReader;
        public Level currentLevel;
        private GraphicalUserInterface _graphicalUserInterface;
        public Camera camera;
        
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
            Global.Instance.MainGame = this;
            _levelReader = new LevelReader(Content);
            currentLevel = _levelReader.ReadLevel(0);
            _mario = new Mario(1, 1, currentLevel, Content);
            currentLevel.ToAddGameObject(_mario);
            camera = new Camera(GraphicsDevice.Viewport);
            _graphicalUserInterface = new GraphicalUserInterface(_mario, Content);
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
            camera.LookAt(_mario.Position);
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            currentLevel.UpdateLevel();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(transformMatrix: camera.GetMatrix(), samplerState: SamplerState.PointClamp);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            currentLevel.DrawLevel(_spriteBatch, GraphicsDevice.Viewport);
            _spriteBatch.End();
            _graphicalUserInterface.Draw(_spriteBatch);
            base.Draw(gameTime);
        }

        public void ChangeCurrentLevel(Level level)
        {
            currentLevel = level;
            _mario.ChangeCurrentLevel(currentLevel);
        }
    }
}
