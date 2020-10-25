// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Osu.Configuration;
using osu.Game.Rulesets.UI;

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
                    Current = config.GetBindable<bool>(OsuRulesetSetting.SnakingInSliders)
                },
                new SettingsCheckbox
                {
                    LabelText = "Snaking out sliders",
                    Current = config.GetBindable<bool>(OsuRulesetSetting.SnakingOutSliders)
                },
                new SettingsCheckbox
                {
                    LabelText = "Cursor trail",
                    Current = config.GetBindable<bool>(OsuRulesetSetting.ShowCursorTrail)
                },
                new SettingsCheckbox
                {
                    LabelText = "Cursor trail additive blending",
                    Current = config.GetBindable<bool>(OsuRulesetSetting.CursorTrailAdditive)
                },
                new SettingsCheckbox
                {
                    LabelText = "Cursor trail combo coloring",
                    Current = config.GetBindable<bool>(OsuRulesetSetting.CursorTrailCombo)
                },
                new SettingsCheckbox
                {
                    LabelText = "Cursor trail hue shifting",
                    Current = config.GetBindable<bool>(OsuRulesetSetting.CursorTrailHueShift)
                },
                new SettingsSlider<float, MultiplierSlider>
                {
                    LabelText = "Cursor trail density",
                    Current = config.GetBindable<float>(OsuRulesetSetting.CursorTrailDensity)
                },
                new SettingsSlider<float, FramerateSlider>
                {
                    LabelText = "Autoplay framerate",
                    Current = config.GetBindable<float>(OsuRulesetSetting.ReplayFramerate)
                },
                new SettingsEnumDropdown<OsuDanceMover>
                {
                    LabelText = "Dance mover",
                    Current = config.GetBindable<OsuDanceMover>(OsuRulesetSetting.DanceMover)
                },
                new SettingsSlider<float, AngleSlider>
                {
                    LabelText = "Angle Offset",
                    Current = config.GetBindable<float>(OsuRulesetSetting.AngleOffset),
                    KeyboardStep = 1f / 18f
                },
                new SettingsSlider<float, MultiplierSlider>
                {
                    LabelText = "Flower: Jump Multiplier",
                    Current = config.GetBindable<float>(OsuRulesetSetting.FlowerJumpMulti),
                    KeyboardStep = 1f / 6f
                },
                new SettingsSlider<float, MultiplierSlider>
                {
                    LabelText = "Momentum: Jump Multiplier",
                    Current = config.GetBindable<float>(OsuRulesetSetting.MomentumJumpMulti),
                    KeyboardStep = 1f / 6f
                },
                new SettingsSlider<float, MultiplierSlider>
                {
                    LabelText = "Momentum: Next Jump Multiplier",
                    Current = config.GetBindable<float>(OsuRulesetSetting.NextJumpMulti),
                    KeyboardStep = 1f / 12f
                },
                new SettingsSlider<float, MultiplierSlider>
                {
                    LabelText = "Momentum: Next Multiplier",
                    Current = config.GetBindable<float>(OsuRulesetSetting.NextMulti),
                },
                new SettingsCheckbox
                {
                    LabelText = "Bounce off borders",
                    Current = config.GetBindable<bool>(OsuRulesetSetting.BorderBounce)
                },
                new SettingsEnumDropdown<PlayfieldBorderStyle>
                {
                    LabelText = "Playfield border style",
                    Current = config.GetBindable<PlayfieldBorderStyle>(OsuRulesetSetting.PlayfieldBorderStyle),
                },
            };
        }

        private class MultiplierSlider : OsuSliderBar<float>
        {
            public override string TooltipText => Current.Value.ToString("N3") + "x";
        }

        private class AngleSlider : OsuSliderBar<float>
        {
            public override string TooltipText => (Current.Value * 180).ToString("N2") + "deg";
        }

        private class FramerateSlider : OsuSliderBar<float>
        {
            public override string TooltipText => Current.Value.ToString("N0") + "fps";
        }
    }
}
