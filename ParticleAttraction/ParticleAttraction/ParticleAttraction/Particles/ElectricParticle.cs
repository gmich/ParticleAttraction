#region Declarations

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace ParticleAttraction
{
    public abstract class ElectricParticle
    {

        #region Declarations

        Vector2 velocity;
        Vector2 location;
        float timePassed;

        #endregion

        #region Properties

        #region Physics Properties

        public Vector2 Location
        {
            get
            {
                return location;
            }
            private set
            {
                location = value;

                if (this.IsOutOfScreenWidth)
                {
                    Vector2 normal = new Vector2(1, 0);
                    Vector2.Reflect(ref velocity, ref normal, out velocity);
                }
                else if (this.IsOutOfScreenHeight)
                {
                    Vector2 normal = new Vector2(0, 1);
                    Vector2.Reflect(ref velocity, ref normal, out velocity);
                }
            }
        }

        public Vector2 Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = new Vector2(MathHelper.Clamp(value.X, -300, 300), MathHelper.Clamp(value.Y, -300, 300));
            }
        }

        #endregion

        #region Drawing Properties

        Rectangle DestinationRectangle
        {
            get
            {
                return new Rectangle((int)Location.X, (int)Location.Y, Size, Size);
            }
        }

        int Size
        {
            get
            {
                return 2;
            }
        }

        Color Color
        {
            get;
            set;
        }

        Texture2D Texture
        {
            get;
            set;
        }

        #endregion

        public bool IsOutOfScreenWidth
        {
            get
            {
                return (Location.X <= 0 || Location.X >= 800);
            }
        }

        public bool IsOutOfScreenHeight
        {
            get
            {
                return (Location.Y <= 0 || Location.Y >= 800);
            }
        }
        #endregion

        #region Constructor

        public ElectricParticle(Texture2D Texture, Color Color, Vector2 Location)
        {
            this.Texture = Texture;
            this.Location = Location;
            Velocity = Vector2.Zero;
            timePassed = 0.0f;
            this.Color = Color;
        }

        #endregion

        #region Determine Direction

        public abstract void ApplyPhysics(Vector2 otherLocation,float attraction);

        #endregion

        #region Reduce Velocity

        public void ReduceVelocity(float timeToReduce)
        {
            if (timePassed > timeToReduce)
            {
                if (Velocity.X > 0)
                    Velocity -= new Vector2(1, 0);
                else if (Velocity.X < 0)
                    Velocity += new Vector2(1, 0);

                if (Velocity.Y > 0)
                    Velocity -= new Vector2(0, 1);
                else if (Velocity.Y < 0)
                    Velocity += new Vector2(0, 1);

                timePassed = 0.0f;
            }

        }
        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timePassed += elapsed;

            this.Location += Velocity * elapsed;
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, DestinationRectangle, null, Color * 0.1f, 0.0f, Vector2.Zero, SpriteEffects.None, 0.9f);
        }

        #endregion

    }
}
