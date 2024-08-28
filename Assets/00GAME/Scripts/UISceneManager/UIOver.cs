using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOver : MonoBehaviour
{
    [SerializeField] Text _score;
    [SerializeField] Text _bestScore;
	[SerializeField] Button _rewardBtn;
	[SerializeField] Slider _rewardBar;
	[SerializeField] float _rewardTimer = 10;

	[SerializeField] bool _canShowReward;

	// Start is called before the first frame update
	void Start()
    {
		_canShowReward = false;
        Observer.instance.AddListener(CONSTANTS.UIOVER_UPDATESCORE,UpdateScore);
        Observer.instance.AddListener(CONSTANTS.UIOVER_UPDATEBESTSCORE,UpdateBestScore);

        Observer.instance.Notify(CONSTANTS.UIOVER_UPDATESCORE,null);
        Observer.instance.Notify(CONSTANTS.UIOVER_UPDATEBESTSCORE, null);
    }

	void Update()
	{
        if (GameManager.instance.isFirstTimeOver && AdManager.instance.rewardShowTimer < 0 )
            _canShowReward = true;

        if (AdManager.instance.rewardShowTimer < 0 && _canShowReward && AdManager.instance.CheckRewardAd())
        {
			ActiveReward(true);
			_rewardTimer -= Time.deltaTime;
			_rewardBar.value = _rewardTimer / 10;
		}
		else
		{
			ActiveReward(false);
		}

		if (_rewardTimer < 0)
		{
			ActiveReward(false);
			_canShowReward = false;
			_rewardTimer = 10;
		}
	}

	void ActiveReward(bool t)
	{
		_rewardBtn.gameObject.SetActive(t);
		_rewardBar.gameObject.SetActive(t);
	}

	public void RewardBtnClick()
	{
		Debug.Log("an nut quang cao");
		AdManager.instance.ShowRewardedAd(isShowSuccess =>
		{
			if (isShowSuccess)
			{
				//reset
				AdManager.instance.ResetRewardShowTimer();
				ActiveReward(false);
				_rewardTimer = 10;

				Debug.LogError("Da xem hoi sinh");
				//hoi sinh
				PlayerController.instance.LoadAfterRevive();
				GameManager.instance.checkLoadRePlay = false;
				GameManager.instance.ChangeState(GameManager.GAME_STATE.PLAY);
				AdManager.instance.LoadRewardedAd();
			}
		});
	}

	//Texts
	void UpdateScore(object obj)
    {
        this._score.text = "YOUR SCORE: " + GameManager.instance.score;
    }

    void UpdateBestScore(object obj)
    {
        this._bestScore.text = "YOUR HIGHEST SCORE: " + GameManager.instance.scoreBest;
    }

    //Buttons
    public void PlayAgainBtn()
    {
		GameManager.instance.checkLoadRePlay = true;
        GameManager.instance.ChangeState(GameManager.GAME_STATE.PLAY);
    }

    public void ToMenuBtn()
    {
        GameManager.instance.ChangeState(GameManager.GAME_STATE.MENU);
    }

    private void OnDestroy()
    {
        Observer.instance.RemoveListener(CONSTANTS.UIOVER_UPDATESCORE, UpdateScore);
        Observer.instance.RemoveListener(CONSTANTS.UIOVER_UPDATEBESTSCORE,UpdateBestScore);
    }
}
