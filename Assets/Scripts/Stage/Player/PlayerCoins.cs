using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoneyRush.Stage
{
    public class PlayerCoins : MonoBehaviour
    {
        [SerializeField] Transform _coinTransform;
        [SerializeField] GameObject _tempCoin;

        public void SetCoin(int score)
        {
            Vector3 vec = Vector3.zero;
            vec.y = _tempCoin.transform.position.y;

            float currentZ = 0f;
            int breakIndex = 1; // 1 -> 4 (+3) -> 9 (+5) -> 16 (+7)
            int breakValue = 3; // break마다 2씩 증가

            float coinPositionX = 0.15f;
            float coinPositionZ = 0.15f;

            for (int i = 1; i < score; i++)
            {
                GameObject coin;

                if (i >= _coinTransform.childCount)
                {
                    coin = Instantiate(_tempCoin, _coinTransform);
                }
                else
                {
                    coin = _coinTransform.GetChild(i).gameObject;
                }

                if (i == breakIndex)
                {
                    currentZ -= 0.3f;
                    breakIndex += breakValue;

                    if (breakValue < 7)
                    {
                        breakValue += 2;
                    }

                    coinPositionX = 0.15f;
                    coinPositionZ = 0.15f;

                    vec.x = 0f;
                    vec.z = currentZ;
                }
                else
                {
                    vec.x = coinPositionX;
                    vec.z = currentZ + coinPositionZ;

                    coinPositionX = coinPositionX > 0f ? -coinPositionX : Mathf.Abs(coinPositionX); // 양수면 음수, 음수면 양수

                    if (coinPositionX > 0f)
                    {
                        coinPositionX += 0.15f;
                        coinPositionZ += 0.15f;
                    }
                }

                coin.transform.localPosition = vec;
                coin.SetActive(true);
            }

            for (int i = score; i < _coinTransform.childCount; i++)
            {
                if (_coinTransform.GetChild(i) != null)
                {
                    _coinTransform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

    }

}
