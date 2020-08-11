// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Osu.Configuration;

namespace osu.Game.Rulesets.Osu.UI
{
    public class OsuSettingsSubsection : RulesetSettingsSubsection
    {
        protected override string Header => "osu!";

        public OsuSettingsSubsection(Ruleset ruleset)
            : base(ruleset)
        {
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            var config = (OsuRulesetConfigManager)Config;

            Children = new Drawable[]
            {
                new SettingsCheckbox
                {
                    LabelText = "Snaking in sliders",
                    Bindable = config.GetBindable<bool>(OsuRulesetSetting.SnakingInSliders)
                },
                new SettingsCheckbox
                {
                    LabelText = "Snaking out sliders",
                    Bindable = config.GetBindable<bool>(OsuRulesetSetting.SnakingOutSliders)
                },
                new SettingsCheckbox
                {
                    LabelText = "Cursor trail",
                    Bindable = config.GetBindable<bool>(OsuRulesetSetting.ShowCursorTrail)
                },
                new SettingsCheckbox
                {
                    LabelText = "Cursor trail additive blending",
                    Bindable = config.GetBindable<bool>(OsuRulesetSetting.CursorTrailAdditive)
                },
                new SettingsSlider<float, DensitySlider>
                {
                    LabelText = "Cursor trail density",
                    Bindable = config.GetBindable<float>(OsuRulesetSetting.CursorTrailDensity)
                },
                new SettingsEnumDropdown<OsuDanceMover>
                {
                    LabelText = "Dance mover",
                    Bindable = config.GetBindable<OsuDanceMover>(OsuRulesetSetting.DanceMover)
                }
            };
        }

        private class DensitySlider : OsuSliderBar<float>
        {
            public override string TooltipText => Current.Value.ToString("N2") + "x";
        }
    }
}
