using System;

namespace TempleGardens
{
    [Serializable]
    public class Achievements
    {
        public string Name { get; private set; }
        public bool Awarded { get; private set; }
        public int TrophieValue { get; private set; }
        public string Description { get; private set; }

        public Achievements(string name, string description, int value)
        {
            Name = name;
            Awarded = false;
            TrophieValue = value;
            Description = description;
        }

        public void Award()
        {
            Awarded = true;
        }
    }
}
