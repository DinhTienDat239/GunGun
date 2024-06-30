using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpin : MonoBehaviour
{
	[SerializeField] Text _coinTxt;
	[SerializeField] GameObject _rewardBox;
    [SerializeField] Image _rewardImage;
    [SerializeField] Text _rewardText;
    [SerializeField] Text _coinWarningTxt;

    // Start is called before the first frame update
    void Start()
    {
        Observer.instance.AddListener(CONSTANTS.UISPIN, ShowRewardBox);
        Observer.instance.AddListener(CONSTANTS.UISPIN_NOMONEY, ShowCoinWarning);
		Observer.instance.AddListener(CONSTANTS.UISPIN_UPDATECOIN, UpdateCoinTxt);
	}

    // Update is called once per frame
    void Update()
    {
		Observer.instance.Notify(CONSTANTS.UISPIN_UPDATECOIN, null);
	}

	public void BackBtn()
	{
        if(!SpinController.instance._inRotate)
		GameManager.instance.ChangeState(GameManager.GAME_STATE.MENU);
	}

    public void ShowRewardBox(object data)
    {
        int i = (int)data;
        _rewardBox.SetActive(true);
        _rewardImage.sprite = SpinController.instance.gameObject.transform.GetChild(i).GetComponent<Image>().sprite;
        if(i == 0 || i == 7 || i == 5 || i == 3)
        {
            _rewardText.text = "CONGRACT! YOU HAVE EARNED \n" + SpinController.instance.gameObject.transform.GetChild(i).GetComponentInChildren<Text>().text + " COINS";
        }
        if(i == 1 || i == 6)
        {
            _rewardText.text = "OH NO! GOOD LUCK NEXT TIME GUNNER :(";
        }
        
    }

    public void ShowCoinWarning(object data)
    {
        _coinWarningTxt.gameObject.SetActive(true);
    }

    public void CloseRewardBox()
    {
        _rewardBox.SetActive(false);
    }

	void UpdateCoinTxt(object obj)
	{
		_coinTxt.text = GameManager.instance.money.ToString();
	}

	private void OnDestroy()
    {
        Observer.instance.RemoveListener(CONSTANTS.UISPIN, ShowRewardBox);
        Observer.instance.RemoveListener(CONSTANTS.UISPIN_NOMONEY, ShowCoinWarning);
		Observer.instance.RemoveListener(CONSTANTS.UISPIN_UPDATECOIN, UpdateCoinTxt);
	}
}
