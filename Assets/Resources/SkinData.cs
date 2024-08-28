using UnityEngine;

[CreateAssetMenu(fileName = "SkinData_", menuName = "Data/Skin")]

public class SkinData : ScriptableObject
{
    [SerializeField] private string _skinID;
	[SerializeField] private string _skinName;
    [SerializeField] private Sprite _spriteSkin;
    [SerializeField] private bool _unlocked;
    [SerializeField] private int _price;

	public string GetFileName()
	{
		return this.name;
	}

	public string GetName()
	{
		return this._skinName;
	}

	public string GetSkinID()
	{
		return _skinID;
	}

	public Sprite GetSpriteSkin()
	{
		return _spriteSkin;
	}
	
	public bool isUnlocked()
	{
		return _unlocked;
	}

	public void SetUnlocked(bool t)
	{
		_unlocked = t;
	}
	public int GetPrice()
	{
		return _price;
	}
	public void SetPrice(int price)
	{
		_price = price;
	}
}


