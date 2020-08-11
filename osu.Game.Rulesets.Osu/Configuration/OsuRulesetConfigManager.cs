// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Configuration;
using osu.Game.Rulesets.Configuration;

namespace osu.Game.Rulesets.Osu.Configuration
{
    public class OsuRulesetConfigManager : RulesetConfigManager<OsuRulesetSetting>
    {
        public OsuRulesetConfigManager(SettingsStore settings, RulesetInfo ruleset, int? variant = null)
            : base(settings, ruleset, variant)
        {
        }

        protected override void InitialiseDefaults()
        {
            base.InitialiseDefaults();
            Set(OsuRulesetSetting.SnakingInSliders, true);
            Set(OsuRulesetSetting.SnakingOutSliders, true);
            Set(OsuRulesetSetting.ShowCursorTrail, true);
            Set(OsuRulesetSetting.CursorTrailAdditive, true);
            Set(OsuRulesetSetting.CursorTrailDensity, 2.5f, 0.1f, 10.0f, 0.05f);
            Set(OsuRulesetSetting.DanceMover, OsuDanceMover.Linear);
        }
    }

    public enum OsuDanceMover
    {
        Linear,
        HalfCircle,
        AngleOffset
    }

    public enum OsuRulesetSetting
    {
        SnakingInSliders,
        SnakingOutSliders,
        ShowCursorTrail,
        CursorTrailAdditive,
        CursorTrailDensity,
        DanceMover
    }
}
