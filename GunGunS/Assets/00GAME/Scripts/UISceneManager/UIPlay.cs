using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIPlay : MonoBehaviour
{
    [SerializeField] Text _comboTxt;
    [SerializeField] Image _killMarkImg;
    [SerializeField] Text _scoreTxt;
    [SerializeField] Text _coinTxt;
    [SerializeField] Slider _timeBar;

    [SerializeField] Image _flashImage;
    [SerializeField] float _flashDuration;

    // Start is called before the first frame update
    void Start()
    {
        Observer.instance.AddListener(CONSTANTS.UIPLAY_UPDATESCORE, UpdateScoreTxt);
        Observer.instance.AddListener(CONSTANTS.UIPLAY_UPDATECOIN, UpdateCoinTxt);
        Observer.instance.AddListener(CONSTANTS.UIPLAY_UPDATETIME, UpdateTimeTxt);
        Observer.instance.AddListener(CONSTANTS.UIPLAY_COMBOTXT, ShowComboTxt);
        Observer.instance.AddListener(CONSTANTS.UIPLAY_HEADSHOT, ShowHeadShot);
    }

    // Update is called once per frame
    void Update()
    {
        Observer.instance.Notify(CONSTANTS.UIPLAY_UPDATESCORE, null);
        Observer.instance.Notify(CONSTANTS.UIPLAY_UPDATECOIN, null);
        Observer.instance.Notify(CONSTANTS.UIPLAY_UPDATETIME, null);
        UpdateDificult();
    }

    void UpdateDificult()
    {
        if (GameManager.instance.score > 200)
        {
            _timeBar.maxValue = 5;
        }
        else if (GameManager.instance.score > 150)
        {
            _timeBar.maxValue = 6;
        }
        else if (GameManager.instance.score > 100)
        {
            _timeBar.maxValue = 7;
        }
        else if (GameManager.instance.score > 50)
        {
            _timeBar.maxValue = 8;
        }
        else if (GameManager.instance.score > 20)
        {
            _timeBar.maxValue = 9;
        }
        else
        {
            _timeBar.maxValue = 10;
        }
    }

    void ShowComboTxt(object obj)
    {
        if (GameManager.instance.combo - 1 < 4)
        {
            _comboTxt.color = Color.black;
            _killMarkImg.color = Color.black;
            _comboTxt.text = "HEADSHOT !\nCOMBO x" + (GameManager.instance.combo - 1);
        }
        else if (GameManager.instance.combo - 1 < 7)
        {
            _comboTxt.color = new Color(229 / 255f, 37 / 255f, 84 / 255f);
            _killMarkImg.color = new Color(229 / 255f, 37 / 255f, 84 / 255f);
            _comboTxt.text = "CRAZY AIM !!\nCOMBO x" + (GameManager.instance.combo - 1);
        }
        else
        {
            _comboTxt.color = new Color(192 / 255f, 24 / 255f, 63 / 255f);
            _killMarkImg.color = new Color(192 / 255f, 24 / 255f, 63 / 255f); ;
            _comboTxt.text = "THE AIM GOD !!!\nCOMBO x" + (GameManager.instance.combo - 1);
        }

        _comboTxt.gameObject.SetActive(true);
        _comboTxt.color = new Color(_comboTxt.color.r, _comboTxt.color.g, _comboTxt.color.b, 1);
        _killMarkImg.gameObject.SetActive(true);
        _killMarkImg.color = new Color(_comboTxt.color.r, _comboTxt.color.g, _comboTxt.color.b, 1);
        StopAllCoroutines();
        StartCoroutine(FadeTextToZeroAlpha(_comboTxt));
        StartCoroutine(FadeImageToZeroAlpha(_killMarkImg));
    }



    void UpdateCoinTxt(object obj)
    {
        _coinTxt.text = GameManager.instance.money.ToString();
    }

    void UpdateScoreTxt(object obj)
    {
        _scoreTxt.text = GameManager.instance.score.ToString();
    }

    void UpdateTimeTxt(object obj)
    {
        _timeBar.value = GameManager.instance.time;
        //UpdateTextColor(remainingTime);
    }

    /*void UpdateTextColor(int remainingTime)
	{
		float t = 1;
		if (remainingTime > 7)
		{
			t = (remainingTime - 7f) / 3f;
			_timeTxt.color = Color.Lerp(Color.yellow, Color.green, t);
		}
		else if (remainingTime > 4)
		{
			_timeTxt.color = Color.yellow;
		}
		else
		{
			t = remainingTime / 4f; 
			_timeTxt.color = Color.Lerp(Color.red, Color.yellow, t);
		}
	}*/
    public IEnumerator FadeTextToZeroAlpha(Text i)
    {
        if (i.color.a == 1f)
        {
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(0.05f);
        i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - 0.05f);

        if (i.color.a <= 0)
        {
            i.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(FadeTextToZeroAlpha(i));
        }
    }
    public IEnumerator FadeImageToZeroAlpha(Image i)
    {
        if (i.color.a == 1f)
        {
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(0.05f);
        i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - 0.05f);

        if (i.color.a <= 0)
        {
            i.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(FadeImageToZeroAlpha(i));
        }
    }

    public void ShowHeadShot(object obj)
    {
        StartCoroutine(Flash(_flashDuration, 0));
    }

    public IEnumerator Flash(float dur, int run)
    {
        if (run == 0)
        {
            _flashImage.gameObject.SetActive(true);
            _flashImage.color = new Color(_flashImage.color.r, _flashImage.color.g, _flashImage.color.b, 1);

        }
        float splitdur = dur / 10;
        yield return new WaitForSeconds(splitdur);
        _flashImage.color = new Color(_flashImage.color.r, _flashImage.color.g, _flashImage.color.b, _flashImage.color.a - (255f / 10) / 255f);
        if (run < 9)
        {
            StartCoroutine(Flash(dur, run + 1));
        }
        else
        {
            _flashImage.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        Observer.instance.RemoveListener(CONSTANTS.UIPLAY_UPDATESCORE, UpdateScoreTxt);
        Observer.instance.RemoveListener(CONSTANTS.UIPLAY_UPDATECOIN, UpdateCoinTxt);
        Observer.instance.RemoveListener(CONSTANTS.UIPLAY_UPDATETIME, UpdateTimeTxt);
        Observer.instance.RemoveListener(CONSTANTS.UIPLAY_COMBOTXT, ShowComboTxt);
        Observer.instance.RemoveListener(CONSTANTS.UIPLAY_HEADSHOT, ShowHeadShot);
    }
}