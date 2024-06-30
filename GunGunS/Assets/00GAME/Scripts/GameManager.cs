using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] public Color _stairColor;
    [SerializeField] public int score;
    [SerializeField] public int scoreBest;
    [SerializeField] public int money;
	[SerializeField] public string idSkin;
	[SerializeField] public string idGun;

    [SerializeField] public string idSkinLast;
    [SerializeField] public string idGunLast;

	[SerializeField] public int combo;
    [SerializeField] public float time;
    [SerializeField] public float timeAim;

    [SerializeField] public float enemyDie;
    bool _timeActive;

    [SerializeField] public GAME_STATE? gameState = null;

    public enum GAME_STATE
    {
        MENU = 0,
        SETTING = 1,
        STORE = 2,
        PLAY = 3,
        OVER = 4,
        LOADING = 5,
        SPIN = 6
    }
    // Start is called before the first frame update
    void Start()
    {
        //Kh?i t?o các bi?n
        //State ban ??u
        AudioManager.instance.PlaySound(AudioManager.instance.BGMusicClip, 0, true);

        idSkinLast = PlayerPrefs.GetString(CONSTANTS.IDSKINLAST);
        idGunLast = PlayerPrefs.GetString(CONSTANTS.IDGUNLAST);

        SkinData[] skins = Resources.LoadAll("SkinData", typeof(SkinData)).Cast<SkinData>().ToArray();
        foreach(SkinData s in skins)
        {
            if(s.GetSkinID() == idSkinLast)
            {
                PlayerController.instance.SetSkin(s);
            }
        }
        GunData[] guns = Resources.LoadAll("GunData", typeof(GunData)).Cast<GunData>().ToArray();
        foreach (GunData g in guns)
        {
            if (g.GetGunID() == idGunLast)
            {
                PlayerController.instance.GetComponentInChildren<GunController>().SetGunData(g);				
			}
        }

		ChangeState(GAME_STATE.MENU);
	}

    // Update is called once per frame
    void Update()
    {
        if (this.gameState == GAME_STATE.PLAY)
        {
            UpdateTimeAim();
            UpdateDificult();
        }
    }

    void UpdateDificult()
    {
        if (score > 200)
        {
            timeAim = 5;
        }
        else if (score > 150)
        {
            timeAim = 6;
        }
        else if (score > 100)
        {
            timeAim = 7;
        }
        else if (score > 50)
        {
            timeAim = 8;
        }
        else if (score > 10)
        {
            timeAim = 9;
        }
        else
        {
            timeAim = 10;
        }
    }

    void UpdateTimeAim()
    {
        if (_timeActive)
            time -= Time.deltaTime;
        if (time < 0)
        {
            PlayerController.instance.fireCorrect = false;
            _timeActive = false;
            time = timeAim;
        }
    }

    public void ResetTime()
    {
        time = timeAim;
    }

    void Init()
    {
        _stairColor = CONSTANTS.COLORS[Random.Range(0, CONSTANTS.COLORS.Count - 1)];

        scoreBest = PlayerPrefs.GetInt(CONSTANTS.BESTSCORE, 0);
        money = PlayerPrefs.GetInt(CONSTANTS.MONEY, 0);
		idSkin = PlayerPrefs.GetString(CONSTANTS.IDSKIN, " S1 ");
		idGun = PlayerPrefs.GetString(CONSTANTS.IDGUN, " G1 ");

        score = 0;
        combo = 1;

        timeAim = 10;
        time = timeAim;
        _timeActive = true;
        enemyDie = 0;
    }

    public void ChangeState(GAME_STATE gameState)
    {
        if (gameState == this.gameState)
            return;

        if (gameState == GAME_STATE.MENU)
        {
            UIManager.instance.ChangeUI(CONSTANTS.SCENE_MENU);
            this.Init();
            if (this.gameState != GAME_STATE.SETTING)
                SpawnController.instance.Init();
            PlayerController.instance.Init();
        }
        if (gameState == GAME_STATE.PLAY)
        {
            UIManager.instance.ChangeUI(CONSTANTS.SCENE_PLAY);
            this.Init();
            if (this.gameState != GAME_STATE.MENU)
                SpawnController.instance.Init();
            PlayerController.instance.Init();
        }
        if (gameState == GAME_STATE.OVER)
        {
            UIManager.instance.ChangeUI(CONSTANTS.SCENE_OVER);
            CheckScoreBest();
            SaveMoney();
        }
        if (gameState == GAME_STATE.SETTING)
        {
            UIManager.instance.ChangeUI(CONSTANTS.SCENE_SETTING);
        }
        if (gameState == GAME_STATE.STORE)
        {
            UIManager.instance.ChangeUI(CONSTANTS.SCENE_STORE);
        }
        if (gameState == GAME_STATE.SPIN)
        {
            UIManager.instance.ChangeUI(CONSTANTS.SCENE_SPIN);
        }

        this.gameState = gameState;
    }

    public void CheckScoreBest()
    {
        if (score > scoreBest)
        {
            SetBestScore();
        }
    }

    public void UpdateScoreAndMoney(int i, int coins)
    {
        if (i == 2)
        {
            score += i * combo;
            combo++;
        }
        else
        {
            score += i;
            combo = 1;
        }
        money += coins;
    }

    void SetBestScore()
    {
        scoreBest = score;
        PlayerPrefs.SetInt(CONSTANTS.BESTSCORE, scoreBest);
    }

    public void SaveMoney()
    {
        PlayerPrefs.SetInt(CONSTANTS.MONEY, money);
    }

    public void UnlockSkin(string id)
    {
        idSkin += " " + id + " ";
	}

	public void SaveIdSkin()
	{
		PlayerPrefs.SetString(CONSTANTS.IDSKIN, idSkin);
	}

	public void UnlockGun(string id)
	{
		idGun += " " + id + " ";
	}

	public void SaveIdGun()
	{
		PlayerPrefs.SetString(CONSTANTS.IDGUN, idGun);
	}
}