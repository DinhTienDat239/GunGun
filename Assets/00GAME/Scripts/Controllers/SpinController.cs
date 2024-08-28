using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpinController : Singleton<SpinController>
{
	[SerializeField] float _rotatePower;
	[SerializeField] float _stopPower;
	[SerializeField] float _timeCountAcceptSpin;

	Rigidbody2D _rb;
	public bool _inRotate;

	readonly int[] probabilities = { 16, 22, 3, 10, 3, 22, 22, 2};
	readonly int totalPro = 100;
	public float _valueIndex;
	public int _reward = 0;

	void Start()
	{
		_rb = GetComponent<Rigidbody2D>();
	}

	void Init()
	{
		_inRotate = false;
		_rotatePower = 2000f;
		_stopPower = 200f;
		_timeCountAcceptSpin = 0;

		_reward = RandomIndexByProb();
		_valueIndex = GetIndexCurrentProbability(transform.eulerAngles.z);
	}

	int RandomIndexByProb()
	{
		int rdValue = Random.Range(1, totalPro + 1);
		int total = 0;
		for (int i = 0; i < probabilities.Length; i++)
		{
			total += probabilities[i];
			if (total >= rdValue)
				return i;
		}
		return 0;
	}

	void Update()
	{
		float rot = transform.eulerAngles.z;
		_valueIndex = GetIndexCurrentProbability(rot);

		if (_rb.angularVelocity > 0)
		{
			if (_rb.angularVelocity < 500 && _valueIndex == _reward)
			{
				_rb.angularVelocity -= _stopPower * 30 * Time.deltaTime;
			}
			else
				_rb.angularVelocity -= _stopPower * Time.deltaTime;

			_rb.angularVelocity = Mathf.Clamp(_rb.angularVelocity, 0, 2000f);
		}

		if (_rb.angularVelocity == 0 && _inRotate)
		{
			_timeCountAcceptSpin += Time.deltaTime;
			if (_timeCountAcceptSpin >= 0.5f)
			{
				GetReward(_reward);
				_inRotate = false;
			}
		}
	}

	public void Rotate()
	{
		if (_inRotate)
			return;

		if (GameManager.instance.money >= 100)
		{
			Init();
			GameManager.instance.money -= 100;
			_rb.AddTorque(_rotatePower);
			_inRotate = true;
            GameManager.instance.SaveMoney();
        }
		else
		{
			Observer.instance.Notify(CONSTANTS.UISPIN_NOMONEY, null);
		}
	}

	void GetReward(int index)
	{
		if (index == 0)
		{
			GameManager.instance.money += 100;
		}
		if (index == 2)
		{
			//Random nhan vu khi
			GunData[] res = Resources.LoadAll("GunData", typeof(GunData)).Cast<GunData>().ToArray();
			int count = 0;
			int i = Random.Range(0, res.Length);
			while (res[i].isUnlocked())
			{
				i = Random.Range(0, res.Length);
				count++;
				if (count > res.Length)
				{
					break;
				}
			}
			res[i].SetUnlocked(true);
			GameManager.instance.UnlockGun(res[i].GetGunID());
			GameManager.instance.SaveIdGun();
			Observer.instance.Notify(CONSTANTS.UISPIN_RBWGI, res[i]);
			return;
		}
		if (index == 3)
		{
			GameManager.instance.money += 200;
		}
		if (index == 4)
		{
			SkinData[] res = Resources.LoadAll("SkinData", typeof(SkinData)).Cast<SkinData>().ToArray();
			int count = 0;
			int i = Random.Range(0, res.Length);
			while (res[i].isUnlocked())
			{
				i = Random.Range(0, res.Length);
				count++;
				if (count > res.Length)
				{
					break;
				}
			}
			res[i].SetUnlocked(true);
			GameManager.instance.UnlockSkin(res[i].GetSkinID());
			GameManager.instance.SaveIdSkin();
			Observer.instance.Notify(CONSTANTS.UISPIN_RBWSI, res[i]);
			return;
		}
		if (index == 5)
		{
			GameManager.instance.money += 50;
		}
		if (index == 7)
		{
			GameManager.instance.money += 1000;
		}
		GameManager.instance.SaveMoney();
		Observer.instance.Notify(CONSTANTS.UISPIN, index);
	}

	float GetIndexCurrentProbability(float rot)
	{
		int index = 0;

		rot %= 360;
		if (rot < 0)
			rot += 360;

		if (rot >= 292.5 && rot < 337.5)
		{
			index = 1;
		}
		else if (rot >= 247.5 && rot < 292.5)
		{
			index = 2;
		}
		else if (rot >= 202.5 && rot < 247.5)
		{
			index = 3;
		}
		else if (rot >= 157.5 && rot < 202.5)
		{
			index = 4;
		}
		else if (rot >= 112.5 && rot < 157.5)
		{
			index = 5;
		}
		else if (rot >= 67.5 && rot < 112.5)
		{
			index = 6;
		}
		else if (rot >= 22.5 && rot < 67.5)
		{
			index = 7;
		}
		else if (rot >= 337.5 || rot < 22.5)
		{
			index = 0;
		}

		return index;
	}

}