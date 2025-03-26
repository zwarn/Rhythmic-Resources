using System;
using System.Collections.Generic;
using UnityEngine;

namespace rhythm
{
    public class RhythmHandler : MonoBehaviour
    {
        public event Action<Dictionary<TimingResult, int>> OnBarCompleted;
        public event Action OnRequestPlay;

        public float bpm = 100;

        [SerializeField] private int missTolerance = 2500;
        [SerializeField] private int badTolerance = 1000;
        [SerializeField] private int goodTolerance = 330;
        [SerializeField] private int perfectTolerance = 100;
        [SerializeField] private RhythmSO currentRhythmSO;
        [SerializeField] private RhythmUI ui;
        [SerializeField] private RhythmPlayer rhythmPlayer;

        private int _time;
        private bool _isPlaying = false;
        private int _nextNote = 0;
        private int _barCount = 0;

        private RhythmResult _recentBeat = null;
        private Dictionary<TimingResult, int> _resultCounter = new Dictionary<TimingResult, int>();


        public void Update()
        {
            if (_isPlaying)
            {
                int delta = (int)(Time.deltaTime * 10000 * bpm / 60);
                _time += delta;

                if (_nextNote + badTolerance < _time)
                {
                    AnnounceMiss();
                }

                var currentBar = _time / currentRhythmSO.barDuration;
                if (_barCount != currentBar)
                {
                    _barCount = currentBar;
                    BarCompletedEvent(_resultCounter);
                    ResetResultCounter();
                }
            }
        }

        private void OnEnable()
        {
            _recentBeat = null;
        }

        private void OnDisable()
        {
            _recentBeat = null;
        }


        public int[] GetRelativeSmallLines()
        {
            return GetRelativePositionOfPeriodicLines(10000);
        }

        public int[] GetRelativeBigLines()
        {
            return GetRelativePositionOfPeriodicLines(currentRhythmSO.barDuration);
        }

        private int[] GetRelativePositionOfPeriodicLines(int frequency)
        {
            int barDuration = currentRhythmSO.barDuration;
            int absoluteFrom = _time;
            int absoluteTo = _time + 2 * barDuration;

            List<int> lines = new List<int>();
            int timebase = (absoluteFrom / barDuration) * barDuration;

            for (int t = 0; t <= absoluteTo - absoluteFrom; t += frequency)
            {
                var timing = timebase + t - _time;
                if (timing > 0 && timing <= barDuration)
                {
                    lines.Add(timebase + t - _time);
                }
            }

            return lines.ToArray();
        }

        public int[] GetRelativeNotes()
        {
            int barDuration = currentRhythmSO.barDuration;
            int absoluteFrom = _time + (-missTolerance * 2);
            int absoluteTo = _time + barDuration;

            List<int> notes = new List<int>();

            int fromBar = absoluteFrom / barDuration;
            int toBar = (absoluteTo / barDuration) + 1;

            for (int currentBar = fromBar; currentBar < toBar; currentBar++)
            {
                int timebase = currentBar * barDuration;
                foreach (var note in currentRhythmSO.notes)
                {
                    int timing = timebase + note;
                    if (timing >= absoluteFrom && timing <= absoluteTo && timing >= _nextNote)
                    {
                        notes.Add(timing - _time);
                    }
                }
            }

            return notes.ToArray();
        }

        public void Hit()
        {
            var distance = Mathf.Abs(_nextNote - _time);

            if (distance <= perfectTolerance)
            {
                AnnounceHit(TimingResult.Perfect);
            }
            else if (distance <= goodTolerance)
            {
                AnnounceHit(TimingResult.Good);
            }
            else if (distance <= badTolerance)
            {
                AnnounceHit(TimingResult.Bad);
            }
            else if (distance <= missTolerance)
            {
                AnnounceMiss();
            }
        }

        public void SetHotKey(String hotkey)
        {
            ui.SetHotKey(hotkey);
        }

        public void ChangeCurrentRhythm(RhythmSO rhythm)
        {
            Stop();
            currentRhythmSO = rhythm;
        }

        private void AnnounceMiss()
        {
            _recentBeat = new RhythmResult(_time, _nextNote - _time, TimingResult.Miss);

            FindNextNote();
            _resultCounter[TimingResult.Miss] += 1;
        }

        private void AnnounceHit(TimingResult result)
        {
            _recentBeat = new RhythmResult(_time, _nextNote - _time, result);

            FindNextNote();
            _resultCounter[result] += 1;
        }

        private void FindNextNote()
        {
            int barDuration = currentRhythmSO.barDuration;
            var currentBar = _barCount;

            while (true)
            {
                int timebase = currentBar * barDuration;
                foreach (var note in currentRhythmSO.notes)
                {
                    int timing = timebase + note;
                    if (timing >= _time && timing > _nextNote)
                    {
                        _nextNote = timing;
                        return;
                    }
                }

                currentBar++;
            }
        }

        private void ResetResultCounter()
        {
            _resultCounter = new Dictionary<TimingResult, int>();
            _resultCounter.Add(TimingResult.Bad, 0);
            _resultCounter.Add(TimingResult.Good, 0);
            _resultCounter.Add(TimingResult.Perfect, 0);
            _resultCounter.Add(TimingResult.Miss, 0);
        }

        public int GetTime()
        {
            return _time;
        }

        public void Stop()
        {
            _time = 0;
            _isPlaying = false;
            _nextNote = 0;
            _barCount = 0;
            rhythmPlayer.Stop();
        }

        public void Play()
        {
            _time = 0;
            _barCount = 0;
            _isPlaying = true;
            FindNextNote();
            _recentBeat = null;
            ResetResultCounter();
            rhythmPlayer.Play(currentRhythmSO);
        }

        public bool IsPlaying()
        {
            return _isPlaying;
        }

        public RhythmResult GetRecentResult()
        {
            return _recentBeat;
        }

        public void Click()
        {
            if (IsPlaying())
            {
                Hit();
            }
            else
            {
                RequestPlayEvent();
            }
        }

        public void RequestPlayEvent()
        {
            OnRequestPlay?.Invoke();
        }

        public void BarCompletedEvent(Dictionary<TimingResult, int> timingResult)
        {
            OnBarCompleted?.Invoke(timingResult);
        }
    }
}