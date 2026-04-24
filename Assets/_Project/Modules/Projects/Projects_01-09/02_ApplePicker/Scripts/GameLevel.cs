using UnityEngine;
using TMPro;
using System.Linq;
using System;

namespace Projects.ApplePicker
{
    public class GameLevel : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TMP_Text bestScoreText;
        [SerializeField] private TMP_Text sessionScoreText;

        private StorageManager _storageManager;
        private SaveData _saveData;
        private int score = 0;

        private void Awake()
        {
            _storageManager = new StorageManager();
            _saveData = _storageManager.Load();

            var bestScore = _saveData.entries.Find(e => e.key == "bestScore");

            if (bestScore == null)
                bestScoreText.text = "Best Score: 0";
            else
                bestScoreText.text = $"Best Score: {bestScore.value}";

            sessionScoreText.text = "Score: 0";
        }

        public void UpdateScore()
        {

            score++;
            sessionScoreText.text = $"Score: {score}";
        }

        private void OnDestroy()
        {
            DataEntry bestEntry = _saveData.entries.FirstOrDefault(e => e.key == "bestScore");

            if (bestEntry == null)
            {
                _storageManager.SetValue<int>("bestScore", score);
                return;
            }

            if (!int.TryParse(bestEntry.value, out int bestScore))
                return;

            if (score > bestScore)
            {
                _storageManager.SetValue<int>("bestScore", score);
            }
        }
    }
}
