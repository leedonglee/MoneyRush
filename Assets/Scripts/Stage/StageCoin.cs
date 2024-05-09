using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCoin : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(90f * Time.deltaTime, 0, 0));
    }

}