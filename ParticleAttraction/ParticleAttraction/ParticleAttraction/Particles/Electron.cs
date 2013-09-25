using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleAttraction
{
    public class Electron : ElectricParticle
    {

        #region Constructor 
        
        public Electron(Texture2D Texture, Color Color, Vector2 Location)
            : base(Texture, Color, Location)
        { }

        #endregion

        public override void ApplyPhysics(Vector2 otherLocation, float Attraction)
        {            
            if (Location != otherLocation)
            {
                Vector2 direction = (Location - otherLocation);
                Velocity-= direction * ((float)(Attraction * (10) / Math.Pow((double)Vector2.Distance(Location, otherLocation), 2)));
            }
        }
    }
}
