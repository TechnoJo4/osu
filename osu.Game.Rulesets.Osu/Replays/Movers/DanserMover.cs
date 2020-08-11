// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Osu.Configuration;
using osu.Game.Rulesets.Osu.Objects;
using osuTK;
using static osu.Game.Rulesets.Osu.Replays.Movers.MoverUtilExtensions;

namespace osu.Game.Rulesets.Osu.Replays.Movers
{
    public class DanserMover : BaseDanceMover
    {
        private readonly float mult;
        private readonly float offsetMult;
        private float offset => MathF.PI * offsetMult;

        private Vector2 p1;
        private Vector2 p2;

        private bool firstPoint = true;
        private float invert = 1;
        private float lastAngle;
        private Vector2 lastPoint;

        public DanserMover()
        {
            var c = OsuRulesetConfigManager.Instance;
            mult = c.Get<float>(OsuRulesetSetting.JumpMulti);
            offsetMult = c.Get<float>(OsuRulesetSetting.AngleOffset);
        }

        public override void OnObjChange()
        {
            if (firstPoint)
            {
                lastAngle = MathF.Atan2(-StartY, -StartX);
                firstPoint = false;
            }

            var dst = mult * Vector2.Distance(StartPos, EndPos);

            var s1 = Start as Slider;
            var s2 = End as Slider;

            var newAngle = offset * invert;

            if (s1 != null && s2 != null)
            {
                invert *= -1;
                p1 = V2FromRad(s1.GetEndAngle(), dst) + StartPos;
                p2 = V2FromRad(s2.GetStartAngle(), dst) + EndPos;
            }
            else if (s1 != null)
            {
                invert *= -1;

                if (DurationF > 1)
                    lastAngle = StartPos.AngleRV(EndPos) - newAngle;
                else
                    lastAngle = s1.GetEndAngle() + MathF.PI;

                p1 = V2FromRad(s1.GetEndAngle(), dst) + StartPos;
                p2 = V2FromRad(lastAngle, dst) + EndPos;
            }
            else if (s2 != null)
            {
                if (Duration > 1) lastAngle += MathF.PI;
                p1 = V2FromRad(lastAngle, dst) + StartPos;
                p2 = V2FromRad(s2.GetStartAngle(), dst) + EndPos;
            }
            else
            {
                float angle;

                if (Duration > 1 && AngleBetween(StartPos, lastPoint, EndPos) >= offset)
                {
                    invert *= -1;
                    angle = StartPos.AngleRV(EndPos) - offset * invert;
                }
                else angle = lastAngle;

                p1 = V2FromRad(lastAngle + MathF.PI, dst) + StartPos;
                p2 = V2FromRad(angle, dst) + EndPos;
                if (dst > 2) lastAngle = angle;
            }

            lastPoint = StartPos;
        }

        public override Vector2 Update(double time)
        {
            var t = T(time);
            var ct = 1 - t;

            return new Vector2(
                ct * ct * ct * StartX
              + 3 * ct * ct * t * p1.X
              + 3 * ct * t * t * p2.X
              + t * t * t * EndX,
                ct * ct * ct * StartY
              + 3 * ct * ct * t * p1.Y
              + 3 * ct * t * t * p2.Y
              + t * t * t * EndY
            );
        }
    }
}
