// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Replays;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Osu.Beatmaps;
using osu.Game.Rulesets.Osu.Objects;
using osu.Game.Rulesets.Osu.Replays.Movers;
using osu.Game.Rulesets.Osu.Replays.Movers.Sliders;
using osu.Game.Rulesets.Osu.Replays.Movers.Spinners;
using osuTK;

namespace osu.Game.Rulesets.Osu.Replays
{
    public class OsuDanceAutoGenerator : OsuAutoGeneratorBase
    {
        public new OsuBeatmap Beatmap => (OsuBeatmap)base.Beatmap;
        private readonly BaseDanceMover mover;
        private readonly BaseDanceObjectMover<Slider> sliderMover;
        private readonly BaseDanceObjectMover<Spinner> spinnerMover;

        private bool tapRight;
        private const double frame_time = 1000.0 / 144.0;

        private OsuAction action()
        {
            tapRight = !tapRight;
            return tapRight ? OsuAction.LeftButton : OsuAction.RightButton;
        }

        public OsuDanceAutoGenerator(IBeatmap beatmap)
            : base(beatmap)
        {
            mover = new KnorkeMover();
            sliderMover = new SimpleSliderMover();
            spinnerMover = new SimpleSpinnerMover();
        }

        private void objectGenerate(OsuHitObject o)
        {
            switch (o)
            {
                case Slider s:
                    sliderMover.Object = s;

                    var tap = action();
                    AddFrameToReplay(new OsuReplayFrame(s.StartTime, s.StackedPosition, tap));

                    for (double t = s.StartTime + frame_time; t < s.EndTime; t += frame_time)
                        AddFrameToReplay(new OsuReplayFrame(t, sliderMover.Update(t), tap));

                    break;

                case Spinner s:
                    spinnerMover.Object = s;

                    tap = action();
                    AddFrameToReplay(new OsuReplayFrame(s.StartTime, s.StackedPosition, tap));

                    for (double t = s.StartTime + frame_time; t < s.EndTime; t += frame_time)
                        AddFrameToReplay(new OsuReplayFrame(t, spinnerMover.Update(t), tap));

                    break;

                case HitCircle c:
                    AddFrameToReplay(new OsuReplayFrame(c.StartTime, c.StackedPosition, action()));
                    break;

                default: return;
            }
        }

        public override Replay Generate()
        {
            var s = Beatmap.HitObjects[0];
            AddFrameToReplay(new OsuReplayFrame(-100000, s.Position));
            AddFrameToReplay(new OsuReplayFrame(s.StartTime - 1500, s.Position));
            AddFrameToReplay(new OsuReplayFrame(s.StartTime - 1500, s.Position));

            for (int i = 1; i < Beatmap.HitObjects.Count; i++)
            {
                var e = Beatmap.HitObjects[i];
                objectGenerate(s);

                mover.Start = s;
                mover.End = e;
                mover.OnObjChange();

                for (double t = s.GetEndTime() + frame_time; t < e.StartTime; t += frame_time)
                    AddFrameToReplay(new OsuReplayFrame(t, mover.Update(t)));

                s = e;
            }

            objectGenerate(Beatmap.HitObjects[^1]);

            return Replay;
        }
    }

    public abstract class BaseDanceMover
    {
        protected double StartTime => Start.GetEndTime();
        protected double EndTime => End.StartTime;
        protected double Duration => EndTime - StartTime;
        protected float DurationF => (float)Duration;

        protected Vector2 StartPos => Start.StackedEndPosition;
        protected Vector2 EndPos => End.StackedPosition;
        protected float StartX => StartPos.X;
        protected float StartY => StartPos.Y;
        protected float EndX => EndPos.X;
        protected float EndY => EndPos.Y;

        protected float T(double time) => (float)((time - StartTime) / Duration);

        public OsuHitObject Start { set; protected get; }
        public OsuHitObject End { set; protected get; }
        public virtual void OnObjChange() { }
        public abstract Vector2 Update(double time);
    }

    public abstract class BaseDanceObjectMover<TObject>
        where TObject : OsuHitObject, IHasDuration
    {
        protected double StartTime => Object.StartTime;
        protected double Duration => Object.Duration;

        protected float T(double time) => (float)((time - StartTime) / Duration);

        public TObject Object { set; protected get; }
        public abstract Vector2 Update(double time);
    }
}
