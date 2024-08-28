using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStore : MonoBehaviour
{
    [SerializeField] GameObject _screen;

    [SerializeField] Text _coinTxt;

    [SerializeField] GameObject _notifyScreen;
    [SerializeField] GameObject _notifyYNBtn;
    [SerializeField] GameObject _notifyCloseBtn;
    [SerializeField] Text _notifyContentTxt;
    [SerializeField] Text _notifyTitleTxt;

    [SerializeField] Image _playerSkin;
    [SerializeField] Image _playerGun;

    

    [SerializeField] float _slideSpeed = 50;

    GunItem _gunItem = null;
    SkinItem _skinItem = null;
    // Start is called before the first frame update
    void Start()
    {
        _screen.transform.localPosition = new Vector3(1500, 0, 0);
        Observer.instance.AddListener(CONSTANTS.UISTORE_NOTI, ShowNotifyScreen);
        Observer.instance.AddListener(CONSTANTS.UISTORE_PLAYER, UpdatePlayerStore);
		Observer.instance.AddListener(CONSTANTS.UISTORE_UPDATECOIN, UpdateCoinTxt);
		Observer.instance.Notify(CONSTANTS.UISTORE_PLAYER, null);
        AudioManager.instance.PlaySound(AudioManager.instance.UIClips[5],0,false);
	}

    // Update is called once per frame
    void Update()
    {
		Observer.instance.Notify(CONSTANTS.UISTORE_UPDATECOIN, null);
		ScreenSlideIn();
    }

    void ScreenSlideIn()
    {
        if (_screen.transform.localPosition.x == 0)
            return;

        if (_screen.transform.localPosition.x > 0)
        {
            _screen.transform.Translate(Vector2.left * Time.deltaTime * _slideSpeed);
        }
        else
            _screen.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void BackBtn()
    {
        GameManager.instance.ChangeState(GameManager.GAME_STATE.MENU);
    }

    public void CloseNotifyBtn()
    {
        _notifyScreen.SetActive(false);
    }
    public void YesNotifyBtn()
    {
        if (_skinItem != null)
        {
            GameManager.instance.money -= _skinItem.GetData().GetPrice();
            _skinItem.GetData().SetUnlocked(true);
            _skinItem.UpdateLock();
            GameManager.instance.UnlockSkin(_skinItem.GetData().GetSkinID());
            GameManager.instance.SaveIdSkin();
        }
        else
        {
            GameManager.instance.money -= _gunItem.GetData().GetPrice();
            _gunItem.GetData().SetUnlocked(true);
            _gunItem.UpdateLock();
			GameManager.instance.UnlockGun(_gunItem.GetData().GetGunID());
			GameManager.instance.SaveIdGun();
		}
        GameManager.instance.SaveMoney();
        _notifyScreen.SetActive(false);
    }

    public void UpdatePlayerStore(object obj)
    {
        _playerGun.sprite = PlayerController.instance.GetComponentInChildren<GunController>().GetGunData().GetSpriteGun();
		_playerGun.SetNativeSize();

		_playerSkin.sprite = PlayerController.instance.GetSkin();
        _playerSkin.SetNativeSize();

	}

    public void ShowNotifyScreen(object obj)
    {
        _notifyScreen.gameObject.SetActive(true);
        if (obj.GetType() == typeof(SkinItem))
        {
            SkinItem item = (SkinItem)obj;
            _skinItem = item;
            _gunItem = null;
            if(GameManager.instance.money > _skinItem.GetData().GetPrice())
            {
                _notifyYNBtn.gameObject.SetActive(true);
                _notifyCloseBtn.gameObject.SetActive(false);
                _notifyTitleTxt.text = "BUY CONFIRM";
                _notifyContentTxt.text = "DO YOU WANT TO BUY THIS ITEM?";
            }
            else {
                _notifyYNBtn.gameObject.SetActive(false);
                _notifyCloseBtn.gameObject.SetActive(true);
                _notifyTitleTxt.text = "OH NO!";
                _notifyContentTxt.text = "YOU DON'T HAVE ENOUGH MONEY";
            }
        }
        else
        {
            GunItem item = (GunItem)obj;
            _gunItem = item;
            _skinItem = null;
            if (GameManager.instance.money > _gunItem.GetData().GetPrice())
            {
                _notifyYNBtn.gameObject.SetActive(true);
                _notifyCloseBtn.gameObject.SetActive(false);
                _notifyTitleTxt.text = "BUY CONFIRM";
                _notifyContentTxt.text = "DO YOU WANT TO BUY THIS ITEM?";
            }
            else
            {
                _notifyYNBtn.gameObject.SetActive(false);
                _notifyCloseBtn.gameObject.SetActive(true);
                _notifyTitleTxt.text = "OH NO!";
                _notifyContentTxt.text = "YOU DON'T HAVE ENOUGH MONEY";
            }
        }
    }

	void UpdateCoinTxt(object obj)
	{
		_coinTxt.text = GameManager.instance.money.ToString();
	}

	private void OnDestroy()
    {
        Observer.instance.RemoveListener(CONSTANTS.UISTORE_NOTI, ShowNotifyScreen);
        Observer.instance.RemoveListener(CONSTANTS.UISTORE_PLAYER, UpdatePlayerStore);
		Observer.instance.RemoveListener(CONSTANTS.UISTORE_UPDATECOIN, UpdateCoinTxt);
	}
}
