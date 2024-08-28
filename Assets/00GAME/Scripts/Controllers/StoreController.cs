using UnityEngine;
using UnityEngine.UI;

public class StoreController : MonoBehaviour
{
    [SerializeField] GameObject _skinCharacterStore, _gunStore;
    [SerializeField] ScrollRect _scrollRectSkinCharacter, _scrollRectGun;

    [SerializeField] Button _btnSkin;
    [SerializeField] Button _btnGun;

    [SerializeField] Sprite _spriteSkin;
    [SerializeField] Sprite _spriteSkinGlow;
    [SerializeField] Sprite _spriteGun;
    [SerializeField] Sprite _spriteGunGlow;

    void Start()
    {
        Init();
    }

    void Init()
    {
        _skinCharacterStore.SetActive(true);
        _gunStore.SetActive(false);
        _btnSkin.image.sprite = _spriteSkinGlow;
        _btnSkin.image.color = Color.white;
        _btnGun.image.color = Color.black;
        _btnGun.image.sprite = _spriteGun;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActiveSkinCharacterStore()
    {
        AudioManager.instance.PlaySound(AudioManager.instance.UIClips[6],0,false);
        if (_skinCharacterStore.activeSelf)
            return;
        _skinCharacterStore.SetActive(true);
        _scrollRectSkinCharacter.verticalNormalizedPosition = 1f;
        _gunStore.SetActive(false);
        _btnSkin.image.sprite = _spriteSkinGlow;
        _btnSkin.image.color = Color.white;
        _btnGun.image.color = Color.black;
        _btnGun.image.sprite = _spriteGun;
    }

    public void ActiveGunStore()
    {
        AudioManager.instance.PlaySound(AudioManager.instance.UIClips[6], 0, false);
        if (_gunStore.activeSelf)
            return;
        _gunStore.SetActive(true);
        _scrollRectGun.verticalNormalizedPosition = 1f;
        _skinCharacterStore.SetActive(false);
        _btnSkin.image.sprite = _spriteSkin;
        _btnGun.image.sprite = _spriteGunGlow;
        _btnSkin.image.color = Color.black;
        _btnGun.image.color = Color.white;
    }
}
