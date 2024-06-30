using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBling : MonoBehaviour
{
    [SerializeField] Text _txt;
    Color _color;
    bool _faded;

    private void Start()
    {
        _color = _txt.color;
        _faded = false;
        InvokeRepeating("Bling", 0, 0.01f);
    }

    void Bling()
    {
        if (!_faded)
        {
            _txt.color = new Color(_color.r, _color.g, _color.b, _txt.color.a - 0.01f);
            if(_txt.color.a <= 0)
                _faded = true;
        }
        else
        {
            _txt.color = new Color(_color.r, _color.g, _color.b, _txt.color.a + 0.01f);
            if (_txt.color.a >= 1)
                _faded = false;
        }
    }
}
