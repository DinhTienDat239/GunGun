using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreController : MonoBehaviour
{
    [SerializeField] GameObject _skinCharacterStore, _gunStore;
	[SerializeField] ScrollRect _scrollRectSkinCharacter, _scrollRectGun;
	void Start()
    {
        Init();
    }

    void Init()
    {
		_skinCharacterStore.SetActive(true);
        _gunStore.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActiveSkinCharacterStore()
    {
        if (_skinCharacterStore.activeSelf)
            return;
		_skinCharacterStore.SetActive(true);
		_scrollRectSkinCharacter.verticalNormalizedPosition = 1f;
		_gunStore.SetActive(false);
	}

	public void ActiveGunStore()
	{
        if(_gunStore.activeSelf)
            return;	
		_gunStore.SetActive(true);
		_scrollRectGun.verticalNormalizedPosition = 1f;
		_skinCharacterStore.SetActive(false);
	}
}
