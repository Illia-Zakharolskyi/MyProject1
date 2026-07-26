using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

namespace Project.TowerDefense.Runtime.UI
{
    public class GameUIManager : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private GameObject _gamePanel;
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private GameObject _pauseExitPanel;
        [SerializeField] private TMP_Text _pauseCountDownText;
        [SerializeField] private string _mainMenuSceneName;
        [SerializeField] private string _gameSceneName;
        [SerializeField] private GameEvents _events;
        [SerializeField] private GameObject _towerBuildingPanel;
        [SerializeField] private GameObject _towerUpgradeContentPanel;
        [SerializeField] private AudioClip _backgroundMusic;
        [SerializeField] private GlobalEvents _globalEvents;
        [SerializeField] private TMP_Text _playerHealthText;
        [SerializeField] private TMP_Text _playerGoldText;
        [SerializeField] private TMP_Text _playerDiamondText;
        [SerializeField] private TMP_Text _waveCountText;
        [SerializeField] private TMP_Text _nextWaveTimeText;

        private void OnEnable()
        {
            _events.OnGameOver += OnGameOver;
            //_events.OnTowerUpgradePanel += OnTowerUpgradePanel;
            _events.OnMessageRequired += HandleMessage;
            _globalEvents.InvokePlayMusic(_backgroundMusic);
        }

        private void OnDisable()
        {
            _events.OnGameOver -= OnGameOver;
            //_events.OnTowerUpgradePanel -= OnTowerUpgradePanel;
            _events.OnMessageRequired -= HandleMessage;
            _globalEvents.InvokeStopMusic();
        }

        /*void Start()
        {
            _gamePanel.SetActive(true);
            _pausePanel.SetActive(false);
            _gameOverPanel.SetActive(false);
        }*/

        public void OnGame()
        {
            Time.timeScale = 1.0f;
            _gamePanel.SetActive(true);
            _pausePanel.SetActive(false);
            _towerBuildingPanel.SetActive(false);
        }

        public void OnPause()
        {
            Time.timeScale = 0.0f;
            _pausePanel.SetActive(true);
            _gamePanel.SetActive(false);
        }

        public void OnPauseExit()
        {
            _pausePanel.SetActive(false);
            _pauseExitPanel.SetActive(true);
            StartCoroutine(CountDownCoroutine());
        }
        private IEnumerator CountDownCoroutine()
        {
            float countDownTime = 3;

            while (countDownTime > 0)
            {
                _pauseCountDownText.text = countDownTime.ToString("F1");
                yield return new WaitForSecondsRealtime(0.1f);
                countDownTime -= 0.1f;
            }

            _pauseCountDownText.text = "GO!";
            yield return new WaitForSecondsRealtime(0.5f);

            _pauseExitPanel.SetActive(false);
            _gamePanel.SetActive(true);
            Time.timeScale = 1.0f;
        }

        public void OnGameOver()
        {
            Time.timeScale = 0.0f;
            _gameOverPanel.SetActive(true);
            _pausePanel.SetActive(false);
            _gamePanel.SetActive(false);
        }

        public void OnExit()
        {
            SceneManager.LoadScene(_mainMenuSceneName);
        }

        public void OnRestart()
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(_gameSceneName);
        }

        public void OnTowerBuilding()
        {
            _towerBuildingPanel.SetActive(true);
            _gamePanel.SetActive(false);
        }

        public void OnTowerChoosen()
        {
            _gamePanel.SetActive(true);
            _towerBuildingPanel.SetActive(false);
        }

        /*public void OnTowerUpgradePanel(bool isOpen)
        {
            if (!isOpen)
            {
                OnTowerUpgradePanelClose();
                return;
            }
            else
            {
                OnTowerUpgradePanelOpen();
            }
        }

        private void OnTowerUpgradePanelOpen()
        {
            _gamePanel.SetActive(false);
            _towerUpgradeContentPanel.SetActive(true);
        }

        private void OnTowerUpgradePanelClose()
        {
            _gamePanel.SetActive(true);
            _towerUpgradeContentPanel.SetActive(false);
        }*/

        private void HandleMessage(MessageData messageData, MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.PlayerHealth:
                    UpdatePlayerHealthText(messageData);
                    break;
                case MessageType.PlayerGold:
                    UpdatePlayerGoldText(messageData);
                    break;
                case MessageType.PlayerDiamond:
                    UpdatePlayerDiamondText(messageData);
                    break;
                case MessageType.WaveCount:
                    UpdateWaveCountText(messageData);
                    break;
                case MessageType.NextWaveTime:
                    UpdateNextWaveTimeText(messageData);
                    break;
                default:
                    Debug.LogWarning($"Unhandled: {this.name} on {gameObject.name}");
                    break;
            }
        }

        private void UpdatePlayerHealthText(MessageData newHealth)
        {
            _playerHealthText.text = NumberFormatter.ReturnFormatted(newHealth.DoubleValue, 0);
        }

        private void UpdatePlayerGoldText(MessageData newGold)
        {
            _playerGoldText.text = NumberFormatter.ReturnFormatted(newGold.DoubleValue, 0);
        }

        private void UpdatePlayerDiamondText(MessageData newDiamond)
        {
            _playerDiamondText.text = NumberFormatter.ReturnFormatted(newDiamond.DoubleValue, 0);
        }

        private void UpdateWaveCountText(MessageData newWave)
        {
            if (newWave.StringValue != null)
            {
                _waveCountText.text = newWave.StringValue;
                return;
            }
            _waveCountText.text = NumberFormatter.ReturnFormatted(newWave.UintValue, 0);
        }

        private void UpdateNextWaveTimeText(MessageData newTime)
        {
            _nextWaveTimeText.text = $"{newTime.DoubleValue:F0}s";
        }
    }

    public static class NumberFormatter
    {
        private static string[] _suffices = {
            "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No",
            "Dc", "Ud", "Dd", "Td", "Qad", "Qid", "Sxd", "Spd", "Ocd", "Nod",
            "Vg", "Uvg", "Dvg", "Tvg", "Qavg", "Qivg", "Sxvg", "Spvg", "Ocvg", "Novg"
        };

        public static string ReturnFormatted(double number, int decimalPlaces)
        {
            if (double.IsNaN(number) || double.IsInfinity(number)) return "0";

            if (number < 1000)
            {
                return $"{number:F0}";
            }

            double absValue = Math.Abs(number);

            int suffixIndex = (int)(Math.Log10(absValue) / 3);
            suffixIndex = Math.Min(suffixIndex, _suffices.Length - 1);

            double shortValue = number / Math.Pow(1000, suffixIndex);
            double roundedValue = Math.Round(shortValue, decimalPlaces);

            return $"{roundedValue.ToString($"0.{new string('#', decimalPlaces)}")} {_suffices[suffixIndex]}";
        }
    }
}
