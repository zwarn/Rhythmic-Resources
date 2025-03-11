using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace rhythm
{
    public class RhythmUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject notePrefab;
        [SerializeField] private Transform noteParent;
        [SerializeField] private Transform lineParent;
        [SerializeField] private GameObject smallLinePrefab;
        [SerializeField] private GameObject bigLinePrefab;
        [SerializeField] private Image feedbackMarker;
        [SerializeField] private Transform shadowParent;
        [SerializeField] private TMP_Text hotkeyLabel;

        [SerializeField] private List<ResultConfig> resultConfig = new List<ResultConfig>();
        [SerializeField] private RhythmHandler rhythmHandler;

        private List<GameObject> _noteObjects = new List<GameObject>();
        private List<GameObject> _smallLines = new List<GameObject>();
        private List<GameObject> _bigLines = new List<GameObject>();
        private GameObject _shadow;

        public void Start()
        {
            _shadow = Instantiate(notePrefab, shadowParent);
            _shadow.SetActive(false);
        }

        public void Update()
        {
            HandleBarObject(rhythmHandler.GetRelativeNotes(), _noteObjects, notePrefab, noteParent);
            HandleBarObject(rhythmHandler.GetRelativeSmallLines(), _smallLines, smallLinePrefab, lineParent);
            HandleBarObject(rhythmHandler.GetRelativeBigLines(), _bigLines, bigLinePrefab, lineParent);
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

        public void SetHotKey(String key)
        {
            hotkeyLabel.text = key;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick();
        }


        private void OnClick()
        {
            rhythmHandler.Click();
        }

        private void HandleBarObject(int[] positionsToDisplay, List<GameObject> gameObjectPool, GameObject prefab,
            Transform parent)
        {
            while (gameObjectPool.Count < positionsToDisplay.Length)
            {
                gameObjectPool.Add(Instantiate(prefab, parent));
            }

            for (var i = 0; i < gameObjectPool.Count; i++)
            {
                GameObject barObject = gameObjectPool[i];
                if (i < positionsToDisplay.Length)
                {
                    barObject.SetActive(true);
                    MoveNoteToPosition(barObject.GetComponent<RectTransform>(), positionsToDisplay[i]);
                }
                else
                {
                    barObject.SetActive(false);
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