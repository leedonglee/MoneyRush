using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoneyRush.Stage
{
    public class PlayerPivot : MonoBehaviour
    {
        public event Action<ScoreType, int> OnCollisionEvent;

        public void Stop()
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                Destroy(rigidbody);
            }
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.TryGetComponent(out StageCoin stageCoin))
            {
                Destroy(stageCoin.gameObject);
                OnCollisionEvent?.Invoke(ScoreType.Coin, 1);
            }

            if (collider.TryGetComponent(out StageHurdle stageHurdle))
            {
                if (stageHurdle.CanEnter)
                {
                    stageHurdle.Collide();

                    HurdleType hurdleType = stageHurdle.HurdleType;
                    int value = stageHurdle.Value;

                    ScoreType scoreType = ScoreType.Coin;

                    switch (hurdleType)
                    {
                        case HurdleType.Plus :
                            scoreType = ScoreType.Plus;
                            break;
                        case HurdleType.Minus :
                            scoreType = ScoreType.Minus;
                            break;
                        case HurdleType.Multiple :
                            scoreType = ScoreType.Multiple;
                            break;
                    }

                    OnCollisionEvent?.Invoke(scoreType, value);
                }
            }
        }
    }
}

