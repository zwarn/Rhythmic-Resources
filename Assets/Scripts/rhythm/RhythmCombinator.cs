using System;
using System.Collections.Generic;
using System.Linq;
using helper;
using recipes;
using UnityEngine;
using Zenject;

namespace rhythm
{
    public class RhythmCombinator : MonoBehaviour
    {
        public event Action<Dictionary<TimingResult, int>> OnBarCompleted;
        public event Action OnRequestPlay;

        [SerializeField] private RhythmHandler handlerPrefab;
        [SerializeField] private Transform handlerParent;
        [SerializeField] private List<KeyCode> hotkeys;

        private RecipeSO _recipe;

        private int _receivedBars = 0;
        private int _exceptedBars = 0;

        private readonly List<RhythmHandler> _handlers = new();
        private Dictionary<TimingResult, int> _timingResult = new();

        [Inject] private DiContainer _container;

        public void SetRecipe(RecipeSO recipe)
        {
            _recipe = recipe;
            UpdateHandler();
            Reset();

            var bars = _recipe.rhythms.Select(rhythm => rhythm.barDuration / 10000).ToList();
            _exceptedBars = LeastCommonMultipleHelper.FindSmallestCommonMultiple(bars);
        }

        private void Update()
        {
            for (int i = 0; i < NumberOfTracks(); i++)
            {
                if (Input.GetKeyDown(hotkeys[i]))
                {
                    _handlers[i].Click();
                }
            }
        }

        public void Stop()
        {
            _handlers.ForEach(handler => handler.Stop());
            Reset();
        }

        public void Play()
        {
            _handlers.ForEach(handler => handler.Play());
            Reset();
        }

        private void UpdateHandler()
        {
            _handlers.ForEach(HideTrack);

            for (int i = 0; i < NumberOfTracks(); i++)
            {
                if (_handlers.Count <= i)
                {
                    var handler = _container.InstantiatePrefab(handlerPrefab, handlerParent)
                        .GetComponent<RhythmHandler>();
                    _handlers.Add(handler);
                    handler.SetHotKey(hotkeys[i].ToString());
                }

                _handlers[i].ChangeCurrentRhythm(_recipe.rhythms[i]);
                ShowTrack(_handlers[i]);
            }
        }

        private int NumberOfTracks()
        {
            return _recipe.rhythms.Count;
        }

        private void ShowTrack(RhythmHandler handler)
        {
            handler.gameObject.SetActive(true);
            handler.OnBarCompleted += ReceiveBarComplete;
            handler.OnRequestPlay += RequestPlayEvent;
        }

        private void HideTrack(RhythmHandler handler)
        {
            handler.gameObject.SetActive(false);
            handler.OnBarCompleted -= ReceiveBarComplete;
            handler.OnRequestPlay -= RequestPlayEvent;
        }

        private void ReceiveBarComplete(Dictionary<TimingResult, int> result)
        {
            AggregateTimingResults(result);

            if (_receivedBars >= _exceptedBars)
            {
                BarCompletedEvent(_timingResult);
                Reset();
            }
        }

        private void AggregateTimingResults(Dictionary<TimingResult, int> result)
        {
            _timingResult = _timingResult.Concat(result).GroupBy(kvp => kvp.Key)
                .ToDictionary(g => g.Key, g => g.Sum(kvp => kvp.Value));
            _receivedBars++;
        }

        private void Reset()
        {
            _receivedBars = 0;
            _timingResult.Clear();
        }

        private void RequestPlayEvent()
        {
            OnRequestPlay?.Invoke();
        }

        private void BarCompletedEvent(Dictionary<TimingResult, int> timingResult)
        {
            OnBarCompleted?.Invoke(timingResult);
        }
    }
}