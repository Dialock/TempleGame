
using Microsoft.Xna.Framework;


namespace TempleGardens
{
    public class WindGage
    {
        public float CurrentRotation { get; set; }
        public float RealRotation { get; set; }

        public Vector2 Position { get; private set; }
        public Vector2 Origin { get; private set; }

        public bool WindChange { get; set;}

        public WindGage(float rot)
        {
            WindChange = true;

            RealRotation = 0;

            Position = new Vector2(1000, 32);
            Origin = new Vector2(24, 24);
        }

        public void Update()
        {
            if (WindChange)
            {
                if (!Extensions.NearlyEqual(CurrentRotation, RealRotation))
                {
                    if (CurrentRotation < RealRotation)
                        CurrentRotation += 0.01f;
                    else if (CurrentRotation > RealRotation)
                        CurrentRotation -= 0.01f;
                    else
                        WindChange = !WindChange;

                }
            }
        }
    }
}
