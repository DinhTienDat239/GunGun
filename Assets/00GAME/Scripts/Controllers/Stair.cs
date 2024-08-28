using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Stair : MonoBehaviour
{
    [SerializeField] public float height;
    [SerializeField] public Color _stairColor;
    [SerializeField] public Color _bgColor;
    [SerializeField] bool hasChangedColor = false;
    [SerializeField] bool changedColor = false;

    [SerializeField] public List<SpriteRenderer> _stairs;
    [SerializeField] public List<Sprite> _decorations;
    [SerializeField] public SpriteRenderer _bg;
    [SerializeField] public SpriteRenderer _decorObj;

    void Start()
    {
        height = GetHeight();
    }

    void Update()
    {
        if (GameManager.instance.gameState != GameManager.GAME_STATE.PLAY)
        {
            return;
        }

        if (PlayerController.instance.isMove && !hasChangedColor && SpawnController.instance._canUpColor)
        {
            hasChangedColor = true;
            changedColor = false;
            UpColor();
        }

        if (!PlayerController.instance.isMove && hasChangedColor && changedColor)
        {
            hasChangedColor = false;
        }

        if (PlayerController.instance.transform.position.y - this.transform.position.y > 8)
            this.gameObject.SetActive(false);
    }

    public float GetHeight()
    {
        int count = 0;
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            if (this.gameObject.transform.GetChild(i).CompareTag("Step"))
            {
                count++;
            }
        }
        float stepHeight = this.gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.y;

        return count * stepHeight;
    }

    public void Init(Stair belowStair)
    {
        if(Random.Range(0,5) == 0)
        {
            _decorObj.sprite = null;
        }
        else
        {
            _decorObj.sprite = _decorations[Random.Range(0,_decorations.Count)];
            _decorObj.color = belowStair._bg.color;
        }
        hasChangedColor = false;
        Color defColor = belowStair._bg.color;
        Color bgColor = new Color(defColor.r - 20f / 255f, defColor.g - 20f / 255f, defColor.b - 20f / 255f);
        SetColor(defColor, bgColor);
    }

    public void Init()
    {
        _decorObj.sprite = null;
        hasChangedColor = false;
        Color defColor = GameManager.instance._stairColor;
        Color bgColor = new Color(defColor.r - 20f / 255f, defColor.g - 20f / 255f, defColor.b - 20f / 255f);
        SetColor(defColor, bgColor);
    }

    public void UpColor()
    {
        StartCoroutine(UpColorSecond(0));
    }

    IEnumerator UpColorSecond(int time)
    {
        yield return new WaitForSeconds(0.01f);
        Color defSColor = _stairColor;
        Color defBGColor = _bgColor;

        Color sColor = new Color(defSColor.r + 0.5f / 255f, defSColor.g + 0.5f / 255f, defSColor.b + 0.5f / 255f);
        Color bgColor = new Color(defBGColor.r + 0.5f / 255f, defBGColor.g + 0.5f / 255f, defBGColor.b + 0.5f / 255f);
        SetColor(sColor, bgColor);

        if (time < 39)
        {
            StartCoroutine(UpColorSecond(time + 1));
        }
        else
        {
            changedColor = true;
            SpawnController.instance._canUpColor = false;
        }
    }

    public void SetColor(Color stairColor, Color bgColor)
    {
        if (stairColor == _stairColor && bgColor == _bgColor)
        {
            return;
        }
        foreach (SpriteRenderer rb in _stairs)
        {
            rb.color = stairColor;
        }
        _bg.color = bgColor;

        _stairColor = stairColor;
        _decorObj.color = stairColor;
        _bgColor = bgColor;
    }
}