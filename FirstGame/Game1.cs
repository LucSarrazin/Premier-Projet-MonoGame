using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FirstGame;

public class Game1 : Game
{
    private Texture2D ballTexture;
    private float ballVitesse;
    private Vector2 ballPosition;
    

    private Texture2D _playerTexture;
    private float _PlayerVitesse;
    private Vector2 _PlayerPosition;

    private string nameUser;
    private int score = 0;

    private SoundEffect _soundEffect;
    
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    Random r = new Random();
    private int xPos;

    private Rectangle ballRectangle;
    private Rectangle playerRectangle;

    private SpriteFont _font;
    private Vector2 _fontPos;
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
        Window.AllowAltF4 = true;
        Window.IsBorderless = true;

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _PlayerPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
        _PlayerVitesse = 300f;
        
        
        ballPosition = new Vector2(400, -150);
        ballVitesse = 300f;

        nameUser = Environment.UserName;
        Console.Write(nameUser);
        

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("MyMenuFont");
        Viewport viewport = _graphics.GraphicsDevice.Viewport;

        // TODO: use this.Content to load your game content here
        ballTexture = Content.Load<Texture2D>("ball");
        _playerTexture = Content.Load<Texture2D>("panier");
        _soundEffect = Content.Load<SoundEffect>("tx0_fire1");
        _fontPos = new Vector2(150, 25);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        ballRectangle = new Rectangle(
            (int)ballPosition.X,
            (int)ballPosition.Y,
            ballTexture.Width,
            ballTexture.Height
        );

        playerRectangle = new Rectangle(
            (int)_PlayerPosition.X,
            (int)_PlayerPosition.Y,
            80,
            50
        );
        
        float updatedPlayerSpeed = _PlayerVitesse * (float)gameTime.ElapsedGameTime.TotalSeconds;
        float updatedBallSpeed = ballVitesse * (float)gameTime.ElapsedGameTime.TotalSeconds;

        var getState = Keyboard.GetState();

        if (getState.IsKeyDown(Keys.Down))
        {
            _PlayerPosition.Y += updatedPlayerSpeed;
        } 
        if (getState.IsKeyDown(Keys.Up))
        {
            _PlayerPosition.Y -= updatedPlayerSpeed;
        } 
        if (getState.IsKeyDown(Keys.Right))
        {
            _PlayerPosition.X += updatedPlayerSpeed;
        } 
        if (getState.IsKeyDown(Keys.Left))
        {
            _PlayerPosition.X -= updatedPlayerSpeed;
        }

        if (getState.IsKeyDown(Keys.Space))
        { 
            _soundEffect.Play();
        }
        
        ballPosition.Y += updatedBallSpeed;
        
        if (ballPosition.Y > _graphics.PreferredBackBufferHeight + 150)
        {
            ballPosition.Y = -_graphics.PreferredBackBufferHeight + 150;
            xPos = r.Next(0, 800);
            ballPosition.X = xPos;
            Console.WriteLine($"Les cordonnées de la balle sont : {ballPosition}");
            Console.WriteLine($"La balle à toucher le bord !");
            score--;
        }
        if (ballRectangle.Intersects(playerRectangle))
        {
            xPos = r.Next(0, 800);
            ballPosition.X = xPos;
            ballPosition.Y= -150;
        
            _soundEffect.Play();
            Console.WriteLine($"La balle à toucher le joueur !");
            score++;

        }

        if (_PlayerPosition.X > _graphics.PreferredBackBufferWidth)
        {
            _PlayerPosition.X = _graphics.PreferredBackBufferWidth;
        }
        if (_PlayerPosition.X < -_graphics.PreferredBackBufferWidth + 800)
        {
            _PlayerPosition.X = -_graphics.PreferredBackBufferWidth + 800;
        }
        
        if (_PlayerPosition.Y > _graphics.PreferredBackBufferHeight)
        {
            _PlayerPosition.Y = _graphics.PreferredBackBufferHeight;
        }
        if (_PlayerPosition.Y < -_graphics.PreferredBackBufferHeight + 850)
        {
            _PlayerPosition.Y = -_graphics.PreferredBackBufferHeight + 850;
        }
        
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        
        _spriteBatch.Begin();
        
        _spriteBatch.Draw(
            ballTexture,
            ballPosition,
            null,
            Color.White,
            0f,
            new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
            0.5f, 
            SpriteEffects.None,
            0f
        );
        _spriteBatch.Draw(
            _playerTexture,
            _PlayerPosition,
            null,
            Color.White,
            0f,
            new Vector2(_playerTexture.Width / 2, _playerTexture.Height / 2),
            0.1f,
            SpriteEffects.None,
            0f
        );
        
        // Draw Hello World
        string output = $"Le score de {nameUser} est de : {score}";

        // Find the center of the string
        Vector2 FontOrigin = _font.MeasureString(output) / 2;
        // Draw the string
        _spriteBatch.DrawString(_font, output, _fontPos, Color.LightGreen,
            0, FontOrigin, 1.5f, SpriteEffects.None, 0.5f);

        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}