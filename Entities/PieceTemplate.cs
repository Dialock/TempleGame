using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TempleGardens
{
    public enum FlowerTypes { Undefined = 0, Orchid, Rose, Anemone, Begonia, Gazania, Tulip, Petunia, Poppy, Rhododendron };

    [Serializable]
    public class PieceTemplate : ICloneable
    {

        public static FlowerTypes[] Flowers = new FlowerTypes[]
        {
            FlowerTypes.Orchid,
            FlowerTypes.Rose,
            FlowerTypes.Anemone, 
            FlowerTypes.Begonia,
            FlowerTypes.Gazania,
            FlowerTypes.Tulip,
            FlowerTypes.Petunia,
            FlowerTypes.Poppy,
            FlowerTypes.Rhododendron
        };

        public static List<Color> ColorFamilies;

        public ColorPair Colors { get; private set; }
        public byte[,] Shape { get; private set; }
        public FlowerTypes FlowerType { get; private set; }
        public bool Unlocked { get; set; }
        public bool IsDark { get; private set; }

        public bool Used { get; set; }

        public static void Init(string colorType)
        {
            ColorFamilies = new List<Color>(7);

            for (var i = 0; i < 7; i++)
                ColorFamilies.Add(Color.White);

            SetColorFamilies(colorType);
        }

        public PieceTemplate() { }

        public PieceTemplate(ColorPair colors, byte[,] shape, FlowerTypes flower, bool isDark)
        {
            Colors = colors;

            Shape = shape;
            FlowerType = flower;

            if (Colors.Color1 == Colors.Color2)
                Unlocked = true;
            else
                Unlocked = false;

            IsDark = isDark;

            Used = false;
        }

        public override string ToString()
        {

            if (Colors.Color1 == Colors.Color2)
                return "Flower:  " + FlowerType.ToString() + " SAME Color";
            else
                return "Flower:  " + FlowerType.ToString() + " NOT SAME Color";
        }

        public static void SetColorFamilies(string familyName)
        {


            switch (familyName)
            {
                case "Standard":
                default:

                    ColorFamilies[0] = Color.FromNonPremultiplied(153, 7, 0, 255); // Red
                    ColorFamilies[1] = Color.AntiqueWhite; // white
                    ColorFamilies[2] = Color.FromNonPremultiplied(255, 255, 0, 255); // yellow
                    ColorFamilies[3] = Color.FromNonPremultiplied(255, 128, 128, 255); // pink
                    ColorFamilies[4] = Color.FromNonPremultiplied(128, 179, 255, 255); // light blue
                    ColorFamilies[5] = Color.FromNonPremultiplied(31, 20, 184, 255); // dark blue
                    ColorFamilies[6] = Color.FromNonPremultiplied(94, 0, 153, 255); // violet

                    break;
                case "Grey":

                    ColorFamilies[0] = Color.FromNonPremultiplied(36, 36, 36, 255); 
                    ColorFamilies[1] = Color.FromNonPremultiplied(72, 72, 72, 255); 
                    ColorFamilies[2] = Color.FromNonPremultiplied(108, 108, 108, 255); 
                    ColorFamilies[3] = Color.FromNonPremultiplied(144, 144, 144, 255); 
                    ColorFamilies[4] = Color.FromNonPremultiplied(180, 180, 180, 255); 
                    ColorFamilies[5] = Color.FromNonPremultiplied(216, 216, 216, 255); 
                    ColorFamilies[6] = Color.FromNonPremultiplied(252, 252, 252, 255); 

                    break;
                case "Bold" :

                    ColorFamilies[0] = Color.FromNonPremultiplied(255, 51, 0, 255); 
                    ColorFamilies[1] = Color.FromNonPremultiplied(255, 255, 0, 255); 
                    ColorFamilies[2] = Color.FromNonPremultiplied(51, 255, 0, 255); 
                    ColorFamilies[3] = Color.FromNonPremultiplied(0, 255, 255, 255); 
                    ColorFamilies[4] = Color.FromNonPremultiplied(0, 0, 204, 255); 
                    ColorFamilies[5] = Color.FromNonPremultiplied(255, 0, 255, 255); 
                    ColorFamilies[6] = Color.FromNonPremultiplied(0, 153, 255, 255); 

                    break;
                case "Pastels":

                    ColorFamilies[0] = Color.FromNonPremultiplied(255, 128, 204, 255); 
                    ColorFamilies[1] = Color.FromNonPremultiplied(153, 128, 255, 255); 
                    ColorFamilies[2] = Color.FromNonPremultiplied(128, 179, 255, 255); 
                    ColorFamilies[3] = Color.FromNonPremultiplied(128, 255, 179, 255); 
                    ColorFamilies[4] = Color.FromNonPremultiplied(229, 255, 128, 255); 
                    ColorFamilies[5] = Color.FromNonPremultiplied(255, 204, 128, 255); 
                    ColorFamilies[6] = Color.FromNonPremultiplied(255, 128, 128, 255); 

                    break;
                case "Mystery":

                    var same = false;

                    do
                    {
                        for (var i = 0; i < 7; i++)
                            ColorFamilies[i] = Color.FromNonPremultiplied(MasterRandom.FRandom.Next(1, 255), MasterRandom.FRandom.Next(1, 255), MasterRandom.FRandom.Next(1, 255), 255);

                        var noDupes = ColorFamilies.Distinct().Count();

                        if (noDupes == ColorFamilies.Count())
                            same = false;
                        else
                            same = true;


                    } while (same);
                    break;
            }

        }

        public object Clone()
        {
            PieceTemplate newPiece = (PieceTemplate)this.MemberwiseClone();
            return newPiece;
        }
    }
}
