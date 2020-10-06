// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Colour;
using static System.MathF;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Osu.Skinning
{
    public static class DanceSkinExtensions
    {
        public static (float, float, float) HueShiftConstants(float shift)
        {
            shift *= 2 * PI;
            var cs = Cos(shift);
            var cs3 = (1f - cs) / 3f;
            var ss3 = Sqrt(1f / 3f) * Sin(shift);
            return (cs + cs3, cs3 + ss3, cs3 - ss3);
        }

        public static Color4 HueShift(this Color4 c, float shift)
        {
            var (x, y, z) = HueShiftConstants(shift);

            return new Color4(
                c.R * x + c.G * y + c.B * z,
                c.R * y + c.G * x + c.B * z,
                c.R * z + c.G * y + c.B * x,
                c.A
            );
        }

        public static ColourInfo HueShift(this ColourInfo c, float shift)
        {
            var i = new ColourInfo { TopLeft = c.TopLeft.Linear.HueShift(shift) };

            if (c.TopLeft.Linear == c.TopRight.Linear)
                i.TopRight = i.TopLeft;
            else
                i.TopRight = c.TopRight.Linear.HueShift(shift);

            if (c.TopLeft.Linear == c.BottomLeft.Linear)
                i.BottomLeft = i.TopLeft;
            else
                i.BottomLeft = c.BottomLeft.Linear.HueShift(shift);

            if (c.TopLeft.Linear == c.BottomRight.Linear)
                i.BottomRight = i.TopLeft;
            else
                i.BottomRight = c.BottomRight.Linear.HueShift(shift);

            return i;
        }
    }
}
