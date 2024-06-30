using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
	SpriteRenderer _sr;
    [SerializeField] bool hasChangedColor = false;

    // Start is called before the first frame update
    void Start()
	{
		_sr = GetComponent<SpriteRenderer>();
		_sr.color = GameManager.instance._stairColor;
	}
    public void Init()
    {
		if(_sr != null)
			_sr.color = GameManager.instance._stairColor;
    }

    // Update is called once per frame
    void Update()
	{
        if (GameManager.instance.gameState != GameManager.GAME_STATE.PLAY)
        {
            return;
        }
        if (PlayerController.instance.isMove)
        {
            if (!hasChangedColor)
            {
                UpColor();
                hasChangedColor = true;
            }
        }
        else
        {
            hasChangedColor = false;
        }
    }

	public void UpColor()
	{
		StartCoroutine(UpColorSecond(0));
	}

	IEnumerator UpColorSecond(int time)
	{
		yield return new WaitForSeconds(0.01f);
		Color defColor = _sr.color;

		Color color = new Color(defColor.r + 0.5f / 255f, defColor.g + 0.5f / 255f, defColor.b + 0.5f / 255f);
		_sr.color = color;

		if (time < 40)
		{
			StartCoroutine(UpColorSecond(time + 1));
		}
	}
}
