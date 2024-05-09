using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum HurdleType
{
    Plus, Minus, Multiple
}

public class StageHurdle : MonoBehaviour
{
    [SerializeField] StageHurdle _pair;
    [Space(20)]
    [SerializeField] HurdleType _hurdleType;
    [SerializeField] int _value;
    [SerializeField] TextMeshProUGUI _hurdleText;

    public bool CanEnter { get { return _canEnter; } set { _canEnter = value; } }

    public HurdleType HurdleType => _hurdleType;

    public int Value => _value;

    bool _canEnter = true;

    bool  _moveSW = false;
    float _move = 0f;

    void Start()
    {
        string text = string.Empty;

        switch (_hurdleType)
        {
            case HurdleType.Plus : 
                text += "+";
                break;
            case HurdleType.Minus : 
                text += "-";
                break;
            case HurdleType.Multiple : 
                text += "X";
                break;
        }

        text += _value.ToString();

        _hurdleText.text = text;
    }

    void Update()
    {
        _hurdleText.transform.position = transform.position + new Vector3(0, 0, -0.01f);

        if (_hurdleType == HurdleType.Minus)
        {
            if (_moveSW)
            {
                _move = -1f;
                if (transform.position.x < -0.55f)
                    _moveSW = false;
            }
            else
            {
                _move = 1f;
                if (transform.position.x > 0.55f)
                    _moveSW = true;
            }

            transform.position = new Vector3(transform.position.x + _move * Time.deltaTime, transform.position.y, transform.position.z);
        }
    }

    public void Collide()
    {
        CanEnter = false;

        if (_pair != null)
        {
            _pair.CanEnter = false;
        }

        gameObject.SetActive(false);
        _hurdleText.gameObject.SetActive(false);
    }

}