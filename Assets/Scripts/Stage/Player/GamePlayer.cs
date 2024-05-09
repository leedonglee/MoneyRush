using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoneyRush.Stage
{
    public enum PlayerState
    {
        None, Start, Finish
    }

    public enum ScoreType
    {
        Coin, Plus, Minus, Multiple
    }

    public class GamePlayer : PlayerBase
    {
        [SerializeField] InputHandler _inputHandler;
        [Space(20)]
        [SerializeField] Camera _camera;
        [Space(20)]
        [SerializeField] Transform _worldPointL;
        [SerializeField] Transform _worldPointR;
        [Space(20)]
        [SerializeField] PlayerPivot _playerPivot;
        [SerializeField] PlayerCoins _playerCoins;
        [Space(20)]
        [SerializeField] Transform _goalLine;

        PlayerState _playerState;

        // 스크린 좌표 최대값
        float _pointMaxX;

        // Touch Moved 상태 유지 막기
        bool _isTouchBegan;
        // Input 현재값
        float _currentPointX;

        // 시작 스피드(1f -> 4f)
        float _startSpeed = 1f;

        // 점수
        int _score = 1;

        void Start()
        {
            Vector3 pointL = _camera.WorldToScreenPoint(_worldPointL.position);
            Vector3 pointR = _camera.WorldToScreenPoint(_worldPointR.position);
            _pointMaxX = pointR.x - pointL.x;

            _inputHandler.OnInputEvent += InputCallback;
            _playerPivot.OnCollisionEvent += SetScore;
        }

        void Update()
        {
            if (_playerState == PlayerState.Finish)
            {
                return;
            }

            if (_playerState == PlayerState.Start)
            {
                if (_startSpeed < 4f)
                {
                    _startSpeed += Time.deltaTime;
                }

                // 앞으로 이동
                float speed = _startSpeed * Time.deltaTime;
                _pivot.transform.position = new Vector3(_pivot.transform.position.x, _pivot.transform.position.y, _pivot.transform.position.z + speed);

                // 카메라 이동
                _camera.transform.position = new Vector3(_camera.transform.position.x, _camera.transform.position.y, _pivot.transform.position.z - 4.5f);
            }

            if (_pivot.transform.position.z > _goalLine.position.z)
            {
                _playerState = PlayerState.Finish;
                _playerPivot.Stop();
                _controller.Notify(NotificationType.Finish);
            }
        }

        void InputCallback(Vector2 point, TouchPhase touchPhase)
        {
            if (_playerState == PlayerState.Finish)
            {
                return;
            }

            if (touchPhase == TouchPhase.Moved)
            {
                if (!_isTouchBegan)
                    return;

                // 좌우 이동
                float pointDifference = point.x - _currentPointX;
                float normalizedX = pointDifference / _pointMaxX;
                float x = _pivot.transform.position.x + normalizedX;
                x = Mathf.Clamp(x, -0.8f, 0.8f);
                _pivot.transform.position = new Vector3(x, _pivot.transform.position.y, _pivot.transform.position.z);

                _currentPointX = point.x;
            }
            else if (touchPhase == TouchPhase.Ended)
            {
                if (!_isTouchBegan)
                    return;

                _isTouchBegan = false;
            }
            else if (touchPhase == TouchPhase.Began)
            {
                if (_playerState == PlayerState.None)
                {
                    _playerState = PlayerState.Start;
                    _controller.Notify(NotificationType.Start);
                }

                _isTouchBegan = true;
                _currentPointX = point.x;
            }
        }

        void SetScore(ScoreType scoreType, int value)
        {
            switch (scoreType)
            {
                case ScoreType.Coin :
                case ScoreType.Plus :
                    _score += value;
                    break;
                case ScoreType.Minus :
                    _score -= value;
                    break;
                case ScoreType.Multiple :
                    _score *= value;
                    break;
            }

            _playerCoins.SetCoin(_score);
            _controller.Score(_score);
        }

    }
}

