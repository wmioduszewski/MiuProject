using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics.Dynamics;

namespace RaceGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        World world;
        public Car car1;
        public Car car2;

        List<DrawableObject> objects = new List<DrawableObject>();

        Texture2D carTexture1, carTexture2, coneTexture, tireTexture, blockTexture;
        public static Texture2D lineTexture;

        Texture2D trackTex, trackStartTex, turnTex, grassTex;

        public static SpriteFont Font1;

        List<Camera> cameras = new List<Camera>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            GraphicsDevice.RasterizerState = new RasterizerState()
            {
                ScissorTestEnable = true
            };

            /*graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ToggleFullScreen();*/
            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            carTexture1 = Content.Load<Texture2D>("car1");
            carTexture2 = Content.Load<Texture2D>("car2");
            lineTexture = Content.Load<Texture2D>("line");
            coneTexture = Content.Load<Texture2D>("cone");
            tireTexture = Content.Load<Texture2D>("tire");
            blockTexture = Content.Load<Texture2D>("block");
            trackTex = Content.Load<Texture2D>("track");
            trackStartTex = Content.Load<Texture2D>("trackStart");
            turnTex = Content.Load<Texture2D>("turn");
            grassTex = Content.Load<Texture2D>("grass");

            world = new World(Vector2.Zero);
            
            car1 = new Car(world, carTexture1, new Vector2(0.1f, 0.1f), 1);
            car1.Restitution = 0.1f;
            car1.Rotation = (float)Math.PI / 2;
            car1.Position = new Vector2(440, 1450);
            car1.level = 0.1f;
            objects.Add(car1);

            car2 = new Car(world, carTexture2, new Vector2(0.1f, 0.1f), 1);
            car2.Restitution = 0.1f;
            car2.Rotation = (float)Math.PI / 2;
            car2.Position = new Vector2(360, 1450);
            //car2.Position = new Vector2(0, 0);
            car2.level = 0.1f;
            objects.Add(car2);

            const int N = 5;
            int dist = 200 / N;
            List<Vector2> conesPos = new List<Vector2>();
            for(int i = 0; i < N; i++)
                for(int j = 0; j < N - i; j++)
                    if (i != 0 || j != 0)
                        conesPos.Add(new Vector2(500 + dist * i, 500 + dist * j));

            for (int i = 0; i < N; i++)
                for (int j = 0; j < N - i; j++)
                    if (i != 0 || j != 0)
                        conesPos.Add(new Vector2(2100 - dist * i, 500 + dist * j));

            for (int i = 0; i < N; i++)
                for (int j = 0; j < N - i; j++)
                    if (i != 0 || j != 0)
                        conesPos.Add(new Vector2(500 + dist * i, 2500 - dist * j));

            for (int i = 0; i < N; i++)
                for (int j = 0; j < N - i; j++)
                    if (i != 0 || j != 0)
                        conesPos.Add(new Vector2(2100 - dist * i, 2500 - dist * j));

            foreach(Vector2 v in conesPos)
            {
                PhysicObj obj = new PhysicObj(world, coneTexture, new Vector2(0.2f, 0.2f), 0.8f);
                obj.Position = v;
                obj.Restitution = 0.1f;
                obj.body.LinearDamping = 5.0f;
                obj.level = 0.9f;
                objects.Add(obj);
            }

            List<Vector2> tiresPos = new List<Vector2>();

            dist = 50;
            int minX = 250;
            int maxX = 2350;
            int minY = 250;
            int maxY = 2750;

            for(int x = minX; x <= maxX; x+=dist)
            {
                tiresPos.Add(new Vector2(x, minY));
                tiresPos.Add(new Vector2(x, maxY));
            }

            for (int y = minY + dist; y < maxY; y += dist)
            {
                tiresPos.Add(new Vector2(minX, y));
                tiresPos.Add(new Vector2(maxX, y));
            }

            foreach (Vector2 v in tiresPos)
            {
                PhysicObj obj = new PhysicObj(world, tireTexture, new Vector2(0.5f, 0.5f), 1);
                obj.Position = v;
                obj.Restitution = 0.8f;
                obj.body.LinearDamping = 8.0f;
                obj.level = 0.9f;
                objects.Add(obj);
            }

            /*tires = new PhysicObj[10];
            for (int i = 1; i <= tires.Length; i++)
            {
                tires[i - 1] = new PhysicObj(world, tireTexture, new Vector2(0.5f, 0.5f), 1);
                tires[i - 1].Position = new Vector2(75 * i, 100);
                tires[i - 1].Restitution = 0.8f;
                tires[i - 1].body.LinearDamping = 8.0f;
                tires[i - 1].level = 0.9f;
                objects.Add(tires[i - 1]);
            }*/

            /*for (int i = 0; i < 100; i++)
            {
                PhysicObj obj = new PhysicObj(world, blockTexture, new Vector2(10, 10), 1);
                obj.Position = new Vector2(-100 - i * 100, -100);
                obj.body.BodyType = BodyType.Static;
                obj.body.Restitution = 0.0f;
                objects.Add(obj);
            }*/
            {
                PhysicObj obj = new PhysicObj(world, blockTexture, new Vector2(14, 18), 0.1f);
                obj.Position = new Vector2(1300, 1500);
                obj.body.BodyType = BodyType.Static;
                obj.body.Restitution = 0.0f;
                objects.Add(obj);
            }

            var track = new int[][] {
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 5, 1, 1, 1, 1, 1, 9, 1, 1, 1, 1, 8, 0, 0},
            new int[] {0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0},
            new int[] {0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0},
            new int[] {0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0},
            new int[] {0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0},
            new int[] {0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0},
            new int[] {0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0},
            new int[] {0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0},
            new int[] {0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0},
            new int[] {0, 0, 6, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 7, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            };

            for(int i = 0; i < track.Length; i++)
                for (int j = 0; j < track[i].Length; j++)
                {
                    DrawableObject obj = null;
                    if (track[i][j] == 0)
                    {
                        continue;
                        obj = new DrawableObject(grassTex, 0);
                    }
                    else if (track[i][j] <= 4)
                    {
                        obj = new DrawableObject(trackTex, (track[i][j] - 1) * MathHelper.PiOver2);
                    }
                    else if (track[i][j] <= 8)
                    {
                        obj = new DrawableObject(turnTex, (track[i][j] - 5) * MathHelper.PiOver2);
                    }
                    else if (track[i][j] <= 12)
                    {
                        obj = new DrawableObject(trackStartTex, (track[i][j] - 9) * MathHelper.PiOver2);
                    }

                    obj.Position = new Vector2(i * 200, j * 200);
                    obj.scale = new Vector2(2);

                    objects.Add(obj);
                }

            List<Car> cars = null;

            /*
            List<Car> cars = new List<Car>();
            cars.Add(car1);
            cars.Add(car2);
            cameras.Add(new Camera(cars, graphics, GraphicsDevice, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight / 2)));

            cars = new List<Car>();
            cars.Add(car1);
            cameras.Add(new Camera(cars, graphics, GraphicsDevice, new Rectangle(0, graphics.PreferredBackBufferHeight / 2, graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight/2)));

            cars = new List<Car>();
            cars.Add(car2);
            cameras.Add(new Camera(cars, graphics, GraphicsDevice, new Rectangle(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2, graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight/2)));
            */

            cars = new List<Car>();
            cars.Add(car1);
            cameras.Add(new Camera(cars, graphics, GraphicsDevice, new Rectangle(0, 0, graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight)));

            cars = new List<Car>();
            cars.Add(car2);
            cameras.Add(new Camera(cars, graphics, GraphicsDevice, new Rectangle(graphics.PreferredBackBufferWidth / 2, 0, graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight)));

            Font1 = Content.Load<SpriteFont>("Font1");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState keyboard = Keyboard.GetState();
            car2.inputState.acceleration = keyboard.IsKeyDown(Keys.Up) ? 1 : 0;
            car2.inputState.breakVal = keyboard.IsKeyDown(Keys.Down) ? 1 : 0;
            car2.inputState.steer = keyboard.IsKeyDown(Keys.Left) ? -1 : 0;
            car2.inputState.steer += keyboard.IsKeyDown(Keys.Right) ? 1 : 0;
            if (keyboard.IsKeyDown(Keys.Escape))
                this.Exit();

            car1.update(gameTime.ElapsedGameTime);
            car2.update(gameTime.ElapsedGameTime);
            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);

            foreach(Camera camera in cameras)
                camera.Update(gameTime.ElapsedGameTime);
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(0.3f, 0.3f, 0.3f));

            foreach (Camera camera in cameras)
                camera.Draw(spriteBatch, objects);

            base.Draw(gameTime);
        }
    }
}
