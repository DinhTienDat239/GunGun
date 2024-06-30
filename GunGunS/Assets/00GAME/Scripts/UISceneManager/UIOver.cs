using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOver : MonoBehaviour
{
    [SerializeField] Text _score;
    [SerializeField] Text _bestScore;
    // Start is called before the first frame update
    void Start()
    {
        Observer.instance.AddListener(CONSTANTS.UIOVER_UPDATESCORE,UpdateScore);
        Observer.instance.AddListener(CONSTANTS.UIOVER_UPDATEBESTSCORE,UpdateBestScore);

        Observer.instance.Notify(CONSTANTS.UIOVER_UPDATESCORE,null);
        Observer.instance.Notify(CONSTANTS.UIOVER_UPDATEBESTSCORE, null);
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
