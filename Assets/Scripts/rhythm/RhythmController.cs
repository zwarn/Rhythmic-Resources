using System;
using System.Collections.Generic;
using resource;
using UnityEngine;
using Zenject;

namespace rhythm
{
    public class RhythmController : MonoBehaviour
    {
        public float bpm = 100;

        public int missTolerance = 2500;
        public int badTolerance = 1000;
        public int goodTolerance = 330;
        public int perfectTolerance = 100;
        public Rhythm currentRhythm;

        private int _time;
        private bool _isPlaying = false;
        private int _nextNote = 0;
        private int _barCount = 0;

        private RhythmResult _recentBeat = null;
        private Dictionary<TimingResult, int> _resultCounter = new Dictionary<TimingResult, int>();

        [Inject] private ResourceController _resourceController;
        [Inject] private AudioController _audioController;

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

                var currentBar = _time / currentRhythm.barDuration;
                if (_barCount != currentBar)
                {
                    _barCount = currentBar;
                    HandleProduction();
                    Debug.Log($"Finished {_barCount}");
                }
            }
        }

        private void HandleProduction()
        {
            _resourceController.Gain(currentRhythm.production);
        }

        private void PlayNote()
        {
            AudioSource.PlayClipAtPoint(currentRhythm.sound, Vector3.zero);
        }

        public int[] GetRelativeNotes()
        {
            int barDuration = currentRhythm.barDuration;
            int absoluteFrom = _time + (-missTolerance * 2);
            int absoluteTo = _time + barDuration;

            List<int> notes = new List<int>();

            int fromBar = absoluteFrom / barDuration;
            int toBar = (absoluteTo / barDuration) + 1;

            for (int currentBar = fromBar; currentBar < toBar; currentBar++)
            {
                int timebase = currentBar * barDuration;
                foreach (var note in currentRhythm.notes)
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

        private void AnnounceMiss()
        {
            // Stop();
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
            int barDuration = currentRhythm.barDuration;
            var currentBar = _barCount;

            while (true)
            {
                int timebase = currentBar * barDuration;
                foreach (var note in currentRhythm.notes)
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
            _audioController.Stop();
        }

        public void Play()
        {
            _time = 0;
            _barCount = 0;
            _isPlaying = true;
            FindNextNote();
            _recentBeat = null;
            ResetResultCounter();
            _audioController.Play(currentRhythm);
        }

        public bool IsPlaying()
        {
            return _isPlaying;
        }

        public RhythmResult GetRecentResult()
        {
            return _recentBeat;
        }
    }
}