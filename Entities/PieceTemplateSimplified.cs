using System;

namespace TempleGardens
{
    [Serializable]
    public struct PieceTemplateSimplified
    {
        public bool Unlocked { get; private set;}
        public bool Used { get; private set;}

        public PieceTemplateSimplified(bool used, bool unlocked)
            :this()
        {
            Unlocked = unlocked;
            Used = used;
        }
    }
}

