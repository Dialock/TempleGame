using System;
using Microsoft.Xna.Framework;

[Serializable]
public struct ColorPair
{
    public Color Color1 { get; private set;}
    public Color Color2 { get; private set;}

    public ColorPair(Color color1, Color color2)
        :this()
    {
        Color1 = color1;
        Color2 = color2;
    }
}