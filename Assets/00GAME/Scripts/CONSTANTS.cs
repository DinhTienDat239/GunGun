using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CONSTANTS
{
    public static List<Color> COLORS = new List<Color> { new Color(200f/255f,200f/255f,200f/255f), new Color(2000f / 255f,150f/ 255f,200f / 255f),
        new Color(150f/255f,200f / 255f,150f/255f), new Color(150f/255f,200f / 255f,200f/255f), new Color(150f/255f, 150f/255f,200f/255f) };

    //PlayerPref vars
    public static string BESTSCORE = "BestScore";
    public static string MONEY = "Money";
    public static string IDSKIN = "IDSkin";
    public static string IDGUN = "IDGun";
    public static string IDSKINLAST = "IDSkinLast";
    public static string IDGUNLAST = "IDGunLast";
    public static string SOUND_MUSIC = "SoundMusic";
    public static string SOUND_SFX = "SoundSFX";
    public static string CLAIMED_FB = "Fb";
    public static string CLAIMED_IG = "Ig";
    public static string DAILY_VID = "Daily";
    public static string DAILY_Y = "DailyY";
    public static string DAILY_M = "DailyM";
    public static string DAILY_D = "DailyD";

    //Scenes
    public static string SCENE_MAIN = "Main";
    public static string SCENE_MENU = "Menu";
    public static string SCENE_STORE = "Store";
	public static string SCENE_SPIN = "Spin";
	public static string SCENE_PLAY = "Play";
    public static string SCENE_OVER = "Over";
    public static string SCENE_SETTING = "Setting";
    public static string SCENE_LOADING = "Loading";

    //UI Observer
    public static string UIPLAY_UPDATESCORE = "UIPLAY_US";
	public static string UIPLAY_UPDATECOIN = "UIPLAY_UC";
	public static string UIPLAY_UPDATETIME = "UIPLAY_UT";
    public static string UIPLAY_COMBOTXT = "UIPLAY_CBT";
    public static string UIPLAY_HEADSHOT = "UIPLAY_HS";
    public static string UIOVER_UPDATESCORE = "UIOVER_US";
    public static string UIOVER_UPDATEBESTSCORE = "UIOVER_UBS";
    public static string UIMENU = "UIMENU";
    public static string UISPIN = "UISPIN";
    public static string UISPIN_RBWI = "UISPIN_RBWI";
    public static string UISPIN_UPDATECOIN = "UISPIN_UC";
	public static string UISPIN_NOMONEY = "UISPIN_NM";
	public static string UISPIN_RBWGI = "UISPIN_RBWGI";
	public static string UISPIN_RBWSI = "UISPIN_RBWSI";
	public static string UISTORE_NOTI = "UISTORE_NOTI";
    public static string UISTORE_UPDATECOIN = "UISTORE_UC";
    public static string UISTORE_PLAYER = "UISTORE_UP";

    //tag
    public static string BULLET_PLAYER = "BulletPlayer";
	public static string BULLET_ENEMY = "BulletEnemy";

    //style fire
    public static string FIRE_1_BULLET_BASIC = "Fire1BulletBasic";
	public static string FIRE_MULTI_BULLET_BASIC = "FireMultiBulletBasic";
	public static string FIRE_SHOT_GUN_BASIC = "FireShotGunBasic";
	public static string FIRE_1_BULLET_ROTATE = "Fire1BulletRotate";
}
