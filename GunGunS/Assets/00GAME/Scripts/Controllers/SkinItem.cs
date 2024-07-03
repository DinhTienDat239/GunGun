using UnityEngine;
using UnityEngine.UI;

public class SkinItem : MonoBehaviour
{
	[SerializeField] SkinData _skinData;
	[SerializeField] GameObject _lock;
    [SerializeField] Image _itemIcon;

    void Start()
	{
		_itemIcon.sprite = _skinData.GetSpriteSkin();
		if (GameManager.instance.idSkin.Contains(" " + _skinData.GetSkinID() + " "))
		{
			_skinData.SetUnlocked(true);
		}
		else
		{
			_skinData.SetUnlocked(false);
		}
		UpdateLock();       
    }

	public void OnItemClick()
	{
        AudioManager.instance.PlaySound(AudioManager.instance.UIClips[2], 0, false);
        if (_skinData.isUnlocked())
        {
            PlayerController.instance.SetSkin(_skinData);
			PlayerPrefs.SetString(CONSTANTS.IDSKINLAST, _skinData.GetSkinID());
            Observer.instance.Notify(CONSTANTS.UISTORE_PLAYER, null);
        }
        else
        {
            Observer.instance.Notify(CONSTANTS.UISTORE_NOTI, this);
        }
    }

	public SkinData GetData()
	{
		return _skinData;
	}

	public void UpdateLock()
	{
		if (_skinData.isUnlocked())
		{
			_lock.SetActive(false);
			return;
		}
		_lock.SetActive(true);
	}
}
