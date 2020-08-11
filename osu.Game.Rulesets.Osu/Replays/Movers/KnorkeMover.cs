// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Osu.Configuration;
using osu.Game.Rulesets.Osu.Objects;
using osuTK;
using static osu.Game.Rulesets.Osu.Replays.Movers.MoverUtilExtensions;

namespace osu.Game.Rulesets.Osu.Replays.Movers
{
    public class KnorkeMover : BaseDanceMover
    {
        private readonly float mult;
        private readonly float offsetMult;
        private float offset => MathF.PI * offsetMult;

        private Vector2 p1;
        private Vector2 p2;

        private bool firstPoint = true;
        private float lastAngle;

        public KnorkeMover()
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

            var newAngle =
                Start is Slider s
                    ? s.GetEndAngle()
                    : lastAngle + offset;

            if (newAngle > MathF.PI * 2f) newAngle -= MathF.PI * 2f;

            p1 = StartPos + V2FromRad(newAngle, dst);

            if (End is Slider e)
            {
                var angle = e.GetStartAngle();
                p2 = EndPos + V2FromRad(angle, dst);
            }
            else p2 = p1;

            if (Duration > 1) lastAngle = p1.AngleRV(End.StackedPosition);
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
