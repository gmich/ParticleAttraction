using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Threading;
using System.Threading.Tasks;

namespace ParticleAttraction
{
    using Input;

    public class ParticleManager
    {
        #region Declarations

        List<ElectricParticle> electricParticles;
        float attraction;
        float deaccelerationAmount;
        bool applyDeacceleration;
        bool showText;

        #endregion

        #region Constructor

        public ParticleManager(Texture2D particleTexture, SpriteFont font)
        {
            this.Font = font;
            electricParticles = new List<ElectricParticle>();
            this.ParticleTexture = particleTexture;
            this.ResetValues();
        }

        #endregion

        #region Properties

        SpriteFont Font
        {
            get;
            set;
        }

        Texture2D ParticleTexture
        {
            get;
            set;
        }

        float Attraction
        {
            get
            {
                return attraction;
            }
            set
            {
                attraction = MathHelper.Max(value, 0);
            }
        }

        float Deacceleration
        {
            get
            {
                return deaccelerationAmount;
            }
            set
            {
                deaccelerationAmount = MathHelper.Max(0.001f, value);
            }
        }

        #endregion

        #region Add Particle Methods

        void AddElectron(Vector2 location)
        {
            electricParticles.Add(new Electron(ParticleTexture,Color.White,location));
        }

        #endregion

        #region Add Random Particles

        public void AddRandomParticles(int quantity)
        {
            Random rand = new Random();
            for (int i = 0; i < quantity; i++)
            {
                Vector2 location = new Vector2(rand.Next(10,790),rand.Next(10,790));
                    AddElectron(location);
            }
        }

        #endregion

        #region Private Helper Methods

        //TODO: find better looking colors
        Color GetRandomColor()
        {
            Random rand = new Random();

            int randomNum = rand.Next(0, 4);

            switch (randomNum)
            {
                case 1:
                    return Color.Yellow;
                case 2:
                    return Color.LightGoldenrodYellow;
                case 3:
                    return Color.LightYellow;
                case 4:
                    return Color.YellowGreen;
                default:
                    return Color.White;
            }
        }

        void ResetValues()
        {
            Attraction = 160f;
            applyDeacceleration = false;
            Deacceleration = 0.002f;
            electricParticles.Clear();
            AddRandomParticles(55000);
            showText = true;
        }

        #endregion

        #region Set Constants and Variables

        void SetConstantsAndVariables()
        {
            if (InputManager.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
            {
                Attraction -= 1f;
            }
            else if (InputManager.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Q))
            {
                Attraction += 1f; ;
            }

            if (InputManager.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.W))
            {
                Deacceleration += 0.001f;
            }
            else if (InputManager.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.S))
            {
                Deacceleration -= 0.001f;
            }

            if (InputManager.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                applyDeacceleration = !applyDeacceleration;
            }

            if (InputManager.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.F1))
            {
                showText = !showText;
            }

            if (InputManager.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.R))
            {
                this.ResetValues();
            }

        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {

            if (InputManager.LeftButtonIsClicked())
            {
                Parallel.ForEach<ElectricParticle>(electricParticles, particle =>
                    {
                        particle.ApplyPhysics(InputManager.MousePosition, Attraction);
                    });
            }
            else if (InputManager.RightButtonIsClicked())
            {
                Parallel.ForEach<ElectricParticle>(electricParticles, particle =>
                    {
                        particle.ApplyPhysics(InputManager.MousePosition, -Attraction);
                    });
            }


            SetConstantsAndVariables();

            Parallel.ForEach<ElectricParticle>(electricParticles, particle =>
            {
                particle.Update(gameTime);
                if (applyDeacceleration)
                    particle.ReduceVelocity(Deacceleration);
            });

        }

        #endregion

        public void Draw(SpriteBatch spriteBatch)
        {
            if (showText)
            {
                spriteBatch.DrawString(Font, "Attraction <Q,A>       : " + Attraction, new Vector2(5, 5), Color.White);
                spriteBatch.DrawString(Font, "Apply Deacceleration <SPACE>    : " + applyDeacceleration, new Vector2(5, 20), Color.White);
                spriteBatch.DrawString(Font, "Deacceleration Delay(sec) <W,S> : " + Deacceleration, new Vector2(5, 35), Color.White);
                spriteBatch.DrawString(Font, "Toggle Text <F1>", new Vector2(5, 55), Color.White);
                spriteBatch.DrawString(Font, "Reset <R>", new Vector2(5, 70), Color.White);
            }

            foreach (ElectricParticle particle in electricParticles)
                particle.Draw(spriteBatch);
        }
    }
}
