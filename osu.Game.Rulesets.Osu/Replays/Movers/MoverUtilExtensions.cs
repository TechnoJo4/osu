// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Osu.Objects;
using osuTK;

namespace osu.Game.Rulesets.Osu.Replays.Movers
{
    public static class MoverUtilExtensions
    {
        public static float AngleRV(this Vector2 v1, Vector2 v2) => MathF.Atan2(v1.Y - v2.Y, v1.X - v2.X);
        public static Vector2 V2FromRad(float rad, float radius) => new Vector2(MathF.Cos(rad) * radius, MathF.Sin(rad) * radius);

        public static float GetEndAngle(this Slider s) => s.GetAngle();
        public static float GetStartAngle(this Slider s) => s.GetAngle(true);

        public static float GetAngle(this Slider s, bool start = false) =>
            (start ? s.StackedPosition : s.StackedEndPosition).AngleRV(s.StackedPositionAt(
                start ? 1 / s.Duration : (s.Duration - 1) / s.Duration
            ));

        public static float AngleBetween(Vector2 centre, Vector2 v1, Vector2 v2)
        {
            var a = Vector2.Distance(centre, v1);
            var b = Vector2.Distance(centre, v2);
            var c = Vector2.Distance(v1, v2);
            return MathF.Acos((a * a + b * b - c * c) / (2 * a * b));
        }
    }
}
