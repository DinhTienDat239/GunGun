using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    [SerializeField] Text _scoreBestTxt;
	[SerializeField] Text _moneyTxt;

	/*int _scoreBest = 0;*/

    // Start is called before the first frame update
    void Start()
    {
        Observer.instance.AddListener(CONSTANTS.UIMENU, UpdateBestScore);
		Observer.instance.AddListener(CONSTANTS.UIMENU, UpdateMoney);
	}

    // Update is called once per frame
    void Update()
    {
        Observer.instance.Notify(CONSTANTS.UIMENU, null);
		Observer.instance.Notify(CONSTANTS.UIMENU, null);

	}

    void UpdateBestScore(object obj)
    {
        /*if (_scoreBest == GameManager.instance.scoreBest)
            return;

        _scoreBest = GameManager.instance.scoreBest;
        _scoreBestTxt.text = "HIGHEST SCORE: " + _scoreBest;*/
		_scoreBestTxt.text = "HIGHEST SCORE: " + GameManager.instance.scoreBest;

	}

    void UpdateMoney(object obj)
    {
        _moneyTxt.text = GameManager.instance.money.ToString();
	}
	public void PlayBtn()
    {
        GameManager.instance.ChangeState(GameManager.GAME_STATE.PLAY);
        AudioManager.instance.PlaySound(AudioManager.instance.UIClips[6],0,false);
    }
    public void SettingBtn()
    {
        GameManager.instance.ChangeState(GameManager.GAME_STATE.SETTING);
        AudioManager.instance.PlaySound(AudioManager.instance.UIClips[6], 0, false);
    }
	public void SpinBtn()
	{
		GameManager.instance.ChangeState(GameManager.GAME_STATE.SPIN);
        AudioManager.instance.PlaySound(AudioManager.instance.UIClips[6], 0, false);
    }
	public void StoreBtn()
    {
        GameManager.instance.ChangeState(GameManager.GAME_STATE.STORE);
        AudioManager.instance.PlaySound(AudioManager.instance.UIClips[6], 0, false);
    }

    private void OnDestroy()
    {
        Observer.instance.RemoveListener(CONSTANTS.UIMENU, UpdateBestScore);
		Observer.instance.RemoveListener(CONSTANTS.UIMENU, UpdateMoney);
	}
}
