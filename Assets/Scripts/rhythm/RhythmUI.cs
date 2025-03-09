using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace rhythm
{
    public class RhythmUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject notePrefab;
        [SerializeField] private Transform noteParent;
        [SerializeField] private Image feedbackMarker;
        [SerializeField] private Transform shadowParent;

        [SerializeField] private List<ResultConfig> resultConfig = new List<ResultConfig>();
        [SerializeField] private RhythmHandler rhythmHandler;

        private List<GameObject> _noteObjects = new List<GameObject>();
        private GameObject _shadow;

        public void Start()
        {
            _shadow = Instantiate(notePrefab, shadowParent);
            _shadow.SetActive(false);
        }

        public void Update()
        {
            HandleNotes();
            HandleRecent();
            HandleShadow();
        }

        private void HandleShadow()
        {
            var recentResult = rhythmHandler.GetRecentResult();
            if (recentResult != null)
            {
                _shadow.SetActive(true);
                _shadow.GetComponent<Image>().color = ColorForQuality(recentResult.Quality);
                MoveNoteToPosition(_shadow.GetComponent<RectTransform>(), recentResult.Offset);
            }
            else
            {
                _shadow.SetActive(false);
            }
        }

        private void HandleRecent()
        {
            var recentResult = rhythmHandler.GetRecentResult();

            if (recentResult != null)
            {
                var color = ColorForQuality(recentResult.Quality);
                feedbackMarker.color = color;
            }
        }

        private Color ColorForQuality(TimingResult quality)
        {
            var color = resultConfig.Find(config => config.result == quality).color;
            return color;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick();
        }

        private void OnClick()
        {
            if (rhythmHandler.IsPlaying())
            {
                rhythmHandler.Hit();
            }
            else
            {
                rhythmHandler.RequestPlayEvent();
            }
        }

        private void HandleNotes()
        {
            var notesToDisplay = rhythmHandler.GetRelativeNotes();
            while (_noteObjects.Count < notesToDisplay.Length)
            {
                _noteObjects.Add(Instantiate(notePrefab, noteParent));
            }

            for (var i = 0; i < _noteObjects.Count; i++)
            {
                GameObject noteObject = _noteObjects[i];
                if (i < notesToDisplay.Length)
                {
                    noteObject.SetActive(true);
                    MoveNoteToPosition(noteObject.GetComponent<RectTransform>(), notesToDisplay[i]);
                }
                else
                {
                    noteObject.SetActive(false);
                }
            }
        }

        private void MoveNoteToPosition(RectTransform rectTransform, int offset)
        {
            rectTransform.anchoredPosition = new Vector2(offset / 100f, 0);
        }
    }

    [Serializable]
    public struct ResultConfig
    {
        public TimingResult result;
        public Color color;
    }
}