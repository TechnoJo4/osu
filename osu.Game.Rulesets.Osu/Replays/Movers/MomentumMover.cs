// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Osu.Configuration;
using osu.Game.Rulesets.Osu.Objects;
using osuTK;
using static osu.Game.Rulesets.Osu.Replays.Movers.MoverUtilExtensions;

namespace osu.Game.Rulesets.Osu.Replays.Movers
{
    public class MomentumMover : BaseDanceMover
    {
        private readonly float jmult;
        private readonly float nmult;
        private readonly float njmult;

        private Vector2 p1;
        private Vector2 p2;
        private Vector2 last;

        public MomentumMover()
        {
            var c = OsuRulesetConfigManager.Instance;
            jmult = c.Get<float>(OsuRulesetSetting.JumpMulti);
            nmult = c.Get<float>(OsuRulesetSetting.NextMulti);
            njmult = c.Get<float>(OsuRulesetSetting.NextJumpMulti);
        }

        public override void OnObjChange()
        {
            var s = Start as Slider;
            var dst = Vector2.Distance(StartPos, EndPos);

            var a2 =
                End is Slider e
                    ? e.GetStartAngle()
                    : ObjectIndex + 2 < Beatmap.HitObjects.Count
                        ? EndPos.AngleRV(Next.StackedPosition)
                        : (s?.GetEndAngle() ?? StartPos.AngleRV(last))
                        + MathF.PI;

            var a1 =
                s != null
                    ? s.GetEndAngle()
                    : ObjectIndex == 0
                        ? a2 + MathF.PI
                        : StartPos.AngleRV(last);

            p1 = V2FromRad(a1, dst * jmult) + StartPos;

            p2 = (1 - nmult) * (V2FromRad(a1, dst * njmult) + StartPos)
               + nmult * (V2FromRad(a2, dst * njmult) + EndPos);

            if (!(End is Slider) && StartPos != EndPos) last = p2;
        }

        public override Vector2 Update(double time)
        {
            var t = T(time);
            var r = 1 - t;

            return new Vector2(
                r * r * r * StartX
              + r * r * t * p1.X * 3
              + r * t * t * p2.X * 3
              + t * t * t * EndX,
                r * r * r * StartY
              + r * r * t * p1.Y * 3
              + r * t * t * p2.Y * 3
              + t * t * t * EndY
            );
        }
    }
}
