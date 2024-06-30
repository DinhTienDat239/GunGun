using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISetting : MonoBehaviour
{
    [SerializeField] Image _musicOffImage;
    [SerializeField] Image _soundOffImage;
    // Start is called before the first frame update
    void Start()
    {
        
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
            AudioManager.instance.ChangeVolume(true);
        }
        else
        {
            _musicOffImage.gameObject.SetActive(true);
			//Turn off music
			AudioManager.instance.ChangeVolume(false);
		}
    }

    public void SoundBtn()
    {
        if (_soundOffImage.gameObject.activeSelf)
        {
            _soundOffImage.gameObject.SetActive(false);
            //Turn on sound
        }
        else
        {
            _soundOffImage.gameObject.SetActive(true);
            //Turn off sound
        }
    }
}
