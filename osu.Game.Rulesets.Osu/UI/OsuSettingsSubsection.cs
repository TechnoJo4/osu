﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
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
                new SettingsCheckbox
                {
                    LabelText = "Cursor trail combo coloring",
                    Bindable = config.GetBindable<bool>(OsuRulesetSetting.CursorTrailCombo)
                },
                new SettingsCheckbox
                {
                    LabelText = "Cursor trail hue shifting",
                    Bindable = config.GetBindable<bool>(OsuRulesetSetting.CursorTrailHueShift)
                },
                new SettingsSlider<float, MultiplierSlider>
                {
                    LabelText = "Cursor trail density",
                    Bindable = config.GetBindable<float>(OsuRulesetSetting.CursorTrailDensity)
                },
                new SettingsSlider<float, FramerateSlider>
                {
                    LabelText = "Autoplay framerate",
                    Bindable = config.GetBindable<float>(OsuRulesetSetting.ReplayFramerate)
                },
                new SettingsEnumDropdown<OsuDanceMover>
                {
                    LabelText = "Dance mover",
                    Bindable = config.GetBindable<OsuDanceMover>(OsuRulesetSetting.DanceMover)
                },
                new SettingsSlider<float, AngleSlider>
                {
                    LabelText = "Angle Offset",
                    Bindable = config.GetBindable<float>(OsuRulesetSetting.AngleOffset),
                    KeyboardStep = 1f / 18f
                },
                new SettingsSlider<float, MultiplierSlider>
                {
                    LabelText = "Jump Multiplier",
                    Bindable = config.GetBindable<float>(OsuRulesetSetting.JumpMulti),
                    KeyboardStep = 1f / 6f
                },
                new SettingsSlider<float, MultiplierSlider>
                {
                    LabelText = "Next Jump Multiplier",
                    Bindable = config.GetBindable<float>(OsuRulesetSetting.NextJumpMulti),
                    KeyboardStep = 1f / 12f
                },
                new SettingsSlider<float, MultiplierSlider>
                {
                    LabelText = "Next Multiplier",
                    Bindable = config.GetBindable<float>(OsuRulesetSetting.NextMulti),
                },
                new SettingsCheckbox
                {
                    LabelText = "Bounce off borders",
                    Bindable = config.GetBindable<bool>(OsuRulesetSetting.BorderBounce)
                }
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
