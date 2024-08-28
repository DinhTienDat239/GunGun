using UnityEngine;
using UnityEngine.UI;

public class UISpin : MonoBehaviour
{
	[SerializeField] Text _coinTxt;
	[SerializeField] GameObject _rewardBox;
	[SerializeField] Image _rewardImage;
	[SerializeField] Text _rewardText;
	[SerializeField] Text _coinWarningTxt;
	[SerializeField] Text _watchedVidTxt;

	// Start is called before the first frame update
	void Start()
	{
		Observer.instance.AddListener(CONSTANTS.UISPIN, ShowRewardBox);
		Observer.instance.AddListener(CONSTANTS.UISPIN_RBWGI, ShowRewardBoxWithGunItem);
		Observer.instance.AddListener(CONSTANTS.UISPIN_RBWSI, ShowRewardBoxWithSkinItem);
		Observer.instance.AddListener(CONSTANTS.UISPIN_NOMONEY, ShowCoinWarning);
		Observer.instance.AddListener(CONSTANTS.UISPIN_UPDATECOIN, UpdateCoinTxt);
        _watchedVidTxt.gameObject.SetActive(false);
    }

	// Update is called once per frame
	void Update()
	{
		Observer.instance.Notify(CONSTANTS.UISPIN_UPDATECOIN, null);
	}

	public void BackBtn()
	{
		if (!SpinController.instance._inRotate)
			GameManager.instance.ChangeState(GameManager.GAME_STATE.MENU);
	}

	public void ShowRewardBox(object data)
	{
		int i = (int)data;
		_rewardBox.SetActive(true);
		_rewardImage.sprite = SpinController.instance.gameObject.transform.GetChild(i).GetComponent<Image>().sprite;
		_rewardImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200);
        _rewardImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200);
        if (i == 0 || i == 7 || i == 5 || i == 3)
		{
			SpawnController.instance.SpawnConfettiBurst(this.gameObject.transform);
			_rewardText.text = "CONGRACT! YOU HAVE EARNED \n" + SpinController.instance.gameObject.transform.GetChild(i).GetComponentInChildren<Text>().text + " COINS";
			AudioManager.instance.PlaySound(AudioManager.instance.UIClips[8], 0, false);
			AudioManager.instance.PlaySound(AudioManager.instance.UIClips[9], 0, false);
		}
		if (i == 1 || i == 6)
		{
			_rewardText.text = "OH NO! GOOD LUCK NEXT TIME GUNNER :(";
		}
	}

	public void ShowRewardBoxWithGunItem(object data)
	{
		GunData objData = (GunData)data;
		_rewardBox.SetActive(true);
		_rewardImage.sprite = objData.GetSpriteGun();
		_rewardImage.SetNativeSize();
		_rewardText.text = "CONGRACT! YOU HAVE UNLOCKED: " + objData.GetName();
		AudioManager.instance.PlaySound(AudioManager.instance.UIClips[8], 0, false);
		AudioManager.instance.PlaySound(AudioManager.instance.UIClips[9], 0, false);
		SpawnController.instance.SpawnConfettiBurst(this.gameObject.transform);
	}

	public void ShowRewardBoxWithSkinItem(object data)
	{
		SkinData objData = (SkinData)data;
		_rewardBox.SetActive(true);
		_rewardImage.sprite = objData.GetSpriteSkin();
		_rewardImage.SetNativeSize();
		_rewardText.text = "CONGRACT! YOU HAVE UNLOCKED: " + objData.GetName();
		AudioManager.instance.PlaySound(AudioManager.instance.UIClips[8], 0, false);
		AudioManager.instance.PlaySound(AudioManager.instance.UIClips[9], 0, false);
		SpawnController.instance.SpawnConfettiBurst(this.gameObject.transform);
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

	public void WatchVideoBtn()
	{
        if (PlayerPrefs.GetInt(CONSTANTS.DAILY_VID, 1) > 0)
        {
            AdManager.instance.ShowRewardedAd(isShowSuccess =>
            {
                if (isShowSuccess)
                {
                    Debug.LogError("Da xem");
                    PlayerPrefs.SetInt(CONSTANTS.DAILY_VID, PlayerPrefs.GetInt(CONSTANTS.DAILY_VID, 1) - 1);
                    GameManager.instance.money += 100;
                    GameManager.instance.SaveMoney();
                }
            });
		}
		else
		{
			_watchedVidTxt.gameObject.SetActive(true);
            Debug.LogError("You watched today");
        }
        
	}

	private void OnDestroy()
	{
		Observer.instance.RemoveListener(CONSTANTS.UISPIN, ShowRewardBox);
		Observer.instance.RemoveListener(CONSTANTS.UISPIN_RBWGI, ShowRewardBoxWithGunItem);
		Observer.instance.RemoveListener(CONSTANTS.UISPIN_RBWSI, ShowRewardBoxWithSkinItem);
		Observer.instance.RemoveListener(CONSTANTS.UISPIN_NOMONEY, ShowCoinWarning);
		Observer.instance.RemoveListener(CONSTANTS.UISPIN_UPDATECOIN, UpdateCoinTxt);
	}
}