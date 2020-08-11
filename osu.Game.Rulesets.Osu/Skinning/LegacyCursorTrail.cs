// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Osu.Configuration;
using osu.Game.Rulesets.Osu.UI.Cursor;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Osu.Skinning
{
    public class LegacyCursorTrail : CursorTrail
    {
        private const double disjoint_trail_time_separation = 1000 / 60.0;

        private bool disjointTrail;
        private double lastTrailTime;
        private readonly Bindable<bool> additiveTrail = new Bindable<bool>(true);

        public LegacyCursorTrail()
        {
            additiveTrail.BindValueChanged(v => Blending = v.NewValue ? BlendingParameters.Additive : BlendingParameters.Mixture, true);
        }

        [BackgroundDependencyLoader]
        private void load(ISkinSource skin, OsuRulesetConfigManager config)
        {
            Texture = skin.GetTexture("cursortrail");
            disjointTrail = skin.GetTexture("cursormiddle") == null;

            config.BindWith(OsuRulesetSetting.CursorTrailAdditive, additiveTrail);

            if (Texture != null)
            {
                // stable "magic ratio". see OsuPlayfieldAdjustmentContainer for full explanation.
                Texture.ScaleAdjust *= 1.6f;
            }
        }

        protected override double FadeDuration => disjointTrail ? 150 : 500;

        protected override bool InterpolateMovements => !disjointTrail;

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            if (!disjointTrail)
                return base.OnMouseMove(e);

            if (Time.Current - lastTrailTime >= disjoint_trail_time_separation)
            {
                lastTrailTime = Time.Current;
                return base.OnMouseMove(e);
            }

            return false;
        }
    }
}
