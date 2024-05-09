using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace MoneyRush.UI
{
    public class GameUI : UIBase
    {
        [Header("Tutorial")]
        [SerializeField] CanvasGroup _tutorial;
        [SerializeField] RectTransform _tutorialFinger;

        [Header("Player Score")]
        [SerializeField] CanvasGroup _playerScore;
        [SerializeField] RectTransform _playerScoreBox;
        [SerializeField] TextMeshProUGUI _playerScoreText;

        [Header("Finish Popup")]
        [SerializeField] GameObject _finish;
        [SerializeField] TextMeshProUGUI _scoreText;
        [SerializeField] Button _btnRestart;

        // Common
        bool _isStarted = false;

        // Tutorial
        bool  _fingerMoveRight = false;
        float _fingerPositionX = 0f;

        void Start()
        {
            _btnRestart.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(0);
            });
        }

        void Update()
        {
            // Tutorial
            if (_tutorial.gameObject.activeSelf)
            {
                _fingerPositionX = _fingerMoveRight ? _fingerPositionX + 80f * Time.deltaTime : _fingerPositionX - 80f * Time.deltaTime;

                if (_fingerPositionX < -80f)
                {
                    _fingerMoveRight = true;
                }
                else if (_fingerPositionX > 80f)
                {
                    _fingerMoveRight = false;
                }

                _tutorialFinger.anchoredPosition = new Vector2(_fingerPositionX, _tutorialFinger.anchoredPosition.y);

                if (_isStarted)
                {
                    _tutorial.alpha -= Time.deltaTime;
                }

                if (_tutorial.alpha < 0f)
                {
                    _tutorial.gameObject.SetActive(false);
                }
            }

            // Rotate Box
            float posX = _playerPivot.position.x * 250f;
            posX = Mathf.Clamp(posX, -200f, 200f);
            _playerScoreBox.anchoredPosition = new Vector2(posX, 0);

            float rotZ = _playerPivot.position.x * 12.5f;
            rotZ = -Mathf.Clamp(rotZ, -10f, 10f);
            _playerScoreBox.rotation = Quaternion.Euler(0, 0, rotZ);
        }

        public override void GameStart()
        {
            _isStarted = true;
        }

        public override void GameFinish()
        {
            _scoreText.text = _playerScoreText.text + "!";
            _finish.SetActive(true);
        }

        public override void SetScore(int score)
        {
            string formattedScore = string.Format("$" + "{0:F2}", score);
            _playerScoreText.text = formattedScore;
        }
    }
}


