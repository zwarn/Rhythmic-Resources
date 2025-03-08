using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace rhythm
{
    public class RhythmUI : MonoBehaviour
    {
        public GameObject notePrefab;
        public Transform noteParent;
        public Image feedbackMarker;
        public Transform shadowParent;

        public List<ResultConfig> ResultConfig = new List<ResultConfig>();

        [Inject] private RhythmController _rhythmController;
        private List<GameObject> _noteObjects = new List<GameObject>();
        private GameObject shadow;

        public void Start()
        {
            shadow = Instantiate(notePrefab, shadowParent);
            shadow.SetActive(false);
        }

        public void Update()
        {
            HandleNotes();
            HandleRecent();
            HandleInput();
            HandleShadow();
        }

        private void HandleShadow()
        {
            var recentResult = _rhythmController.GetRecentResult();
            if (recentResult != null)
            {
                shadow.SetActive(true);
                shadow.GetComponent<Image>().color = ColorForQuality(recentResult.Quality);
                MoveNoteToPosition(shadow.GetComponent<RectTransform>(), recentResult.Offset);
            }
            else
            {
                shadow.SetActive(false);
            }
        }

        private void HandleRecent()
        {
            var recentResult = _rhythmController.GetRecentResult();

            if (recentResult != null)
            {
                var color = ColorForQuality(recentResult.Quality);
                feedbackMarker.color = color;
            }
        }

        private Color ColorForQuality(TimingResult quality)
        {
            var color = ResultConfig.Find(config => config.result == quality).color;
            return color;
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_rhythmController.IsPlaying())
                {
                    _rhythmController.Hit();
                }
                else
                {
                    _rhythmController.Play();
                }
            }
        }

        private void HandleNotes()
        {
            var notesToDisplay = _rhythmController.GetRelativeNotes();
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