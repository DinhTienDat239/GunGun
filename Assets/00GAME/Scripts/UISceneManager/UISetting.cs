using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISetting : MonoBehaviour
{
    [SerializeField] Image _musicOffImage;
    [SerializeField] Image _soundOffImage;

    [SerializeField] GameObject _rewardBox;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt(CONSTANTS.SOUND_MUSIC, 1) != 0)
            _musicOffImage.gameObject.SetActive(false);
        else
            _musicOffImage.gameObject.SetActive(true);

        if (PlayerPrefs.GetInt(CONSTANTS.SOUND_SFX, 1) != 0)
            _soundOffImage.gameObject.SetActive(false);
        else
            _soundOffImage.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackBtn()
    {
        GameManager.instance.ChangeState(GameManager.GAME_STATE.MENU);
    }
    public void MusicBtn()
    {
        if (_musicOffImage.gameObject.activeSelf)
        {
            _musicOffImage.gameObject.SetActive(false);
            //Turn on music
            AudioManager.instance.ChangeVolumeMusic(true);
            PlayerPrefs.SetInt(CONSTANTS.SOUND_MUSIC, 1);
        }
        else
        {
            _musicOffImage.gameObject.SetActive(true);
			//Turn off music
			AudioManager.instance.ChangeVolumeMusic(false);
            PlayerPrefs.SetInt(CONSTANTS.SOUND_MUSIC, 0);
		}
    }

    public void SoundBtn()
    {
        if (_soundOffImage.gameObject.activeSelf)
        {
            _soundOffImage.gameObject.SetActive(false);
            AudioManager.instance.ChangeVolumeSFX(true);
            PlayerPrefs.SetInt(CONSTANTS.SOUND_SFX, 1);
        }
        else
        {
            _soundOffImage.gameObject.SetActive(true);
            AudioManager.instance.ChangeVolumeSFX(false);
            PlayerPrefs.SetInt(CONSTANTS.SOUND_SFX, 0);
        }
    }

    public void FacebookBtn()
    {
        Application.OpenURL("http://facebook.com/");
        if(PlayerPrefs.GetInt(CONSTANTS.CLAIMED_FB,0) == 0)
        {
            PlayerPrefs.SetInt(CONSTANTS.CLAIMED_FB, 1);
            GameManager.instance.money += 100;
            GameManager.instance.SaveMoney();
            ShowRewardBox();
        }
    }
    public void InstagramBtn()
    {
        Application.OpenURL("http://instagram.com/");
        if (PlayerPrefs.GetInt(CONSTANTS.CLAIMED_IG, 0) == 0)
        {
            PlayerPrefs.SetInt(CONSTANTS.CLAIMED_IG, 1);
            GameManager.instance.money += 100;
            GameManager.instance.SaveMoney();
            ShowRewardBox();
        }
    }

    public void ShowRewardBox()
    {
        _rewardBox.gameObject.SetActive(true);
    }

    public void ClaimBtn()
    {
        _rewardBox.gameObject.SetActive(false);
    }
}
