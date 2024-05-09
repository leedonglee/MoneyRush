using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MoneyRush
{
    public enum NotificationType
    {
        Start, Finish
    }

    public interface IController
    {
        public void Notify(NotificationType notificationType);

        public void Score(int score);
    }

    public abstract class PlayerBase : MonoBehaviour
    {
        [SerializeField] protected Transform _pivot;

        public Transform Pivot { get { return _pivot; } }

        protected IController _controller;

        public void Init(IController controller)
        {
            _controller = controller;
        }
    }

    public abstract class UIBase : MonoBehaviour
    {
        protected Transform _playerPivot;

        public abstract void GameStart();

        public abstract void GameFinish();

        public abstract void SetScore(int score);

        public void Init(Transform pivot)
        {
            _playerPivot = pivot;
        }
    }

    public class GameController : MonoBehaviour, IController
    {
        [SerializeField] PlayerBase _gamePlayer;
        [SerializeField] UIBase _gameUI;
        [Space(20)]
        [SerializeField] CanvasScaler[] _canvasScalers;

        // Ratio
        private const float RATIO_WIDTH  = 9f;
        private const float RATIO_HEIGHT = 16f;

        void Start()
        {
            _gamePlayer.Init(this);
            _gameUI.Init(_gamePlayer.Pivot);

            // Aspect Ratio
            float width  = Screen.width;
            float height = Screen.height;
            bool isTabletRatio = width * RATIO_HEIGHT / height > RATIO_WIDTH;

            if (isTabletRatio)
            {
                for (int i = 0; i < _canvasScalers.Length; i++)
                {
                    _canvasScalers[i].matchWidthOrHeight = 1f;
                }
            }
        }

        public void Notify(NotificationType notificationType)
        {
            switch (notificationType)
            {
                case NotificationType.Start:
                    _gameUI.GameStart();
                    break;
                case NotificationType.Finish:
                    _gameUI.GameFinish();
                    break;
            }
        }

        public void Score(int score)
        {
            _gameUI.SetScore(score);
        }

    }

}
