using UnityEngine;

[CreateAssetMenu(fileName = "GunData_", menuName = "Data/Gun")]

public class GunData : ScriptableObject
{
	[SerializeField] string _gunID;
	[SerializeField] string _gunName;
	[SerializeField] Sprite _spriteGun;
	[SerializeField] float _radiusRotate;
	[SerializeField] int _numBullet;
	[SerializeField] float _speedBullet;
	[SerializeField] GameObject _bullet;
	[SerializeField] bool _unlocked;
	[SerializeField] int _price;
	[SerializeField] AudioClip _shotSound;
	[SerializeField] string _styleFire;
	[SerializeField] float _shakeGun;

	public string GetStyleFire()
	{
		return _styleFire;
	}

	public string GetName()
	{
		return _gunName;
	}

	public int GetNumID()
	{
		string numberString = "";
		foreach (char c in _gunID)
		{
			if (char.IsDigit(c))
			{
				numberString += c;
			}
		}

		// Chuyển đổi chuỗi số thành số nguyên và trả về
		if (int.TryParse(numberString, out int result))
		{
			return result;
		}
		else
		{
			return -1; 
		}
	}

	public string GetGunID()
	{
		return _gunID;
	}
	public Sprite GetSpriteGun()
	{
		return _spriteGun;
	}

	public float GetRadiusRotate()
	{
		return _radiusRotate;
	}
	public int GetNumBullet()
	{
		return _numBullet;
	}
	public float GetSpeedBullet()
	{
		return _speedBullet;
	}
	public GameObject GetBullet()
	{
		return _bullet;
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
	public AudioClip GetShotSound()
	{
		return _shotSound;
	}

	public float GetShakeGun()
	{
		return _shakeGun;
	}
}


