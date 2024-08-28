using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunController : MonoBehaviour
{
	[SerializeField] GunData _gunData;
	[SerializeField] GameObject? _obOriginAim;
	int _bulletsFired;
	int _bulletsNoCorrectCountCheck;
	int _bulletsCorrectCountCheck;
	public bool _fireCorrectCheck;
	public bool _fireAfterCheck;

	void Start()
	{
		Init();
	}

	void Update()
	{
		//tra lai sprite gun
		if (!PlayerController.instance.shooted && _gunData.GetStyleFire() == CONSTANTS.FIRE_1_BULLET_ROTATE)
		{
			this.GetComponent<Renderer>().enabled = true;
		}
	}

	public void Init()
	{
		UpdateGunProperties();

	}

	public GunData GetGunData()
	{
		return _gunData;
	}

	public GameObject GetObOriginAim()
	{
		return _obOriginAim;
	}

	public void Fire()
	{
		_bulletsFired = _gunData.GetNumBullet();
		_bulletsNoCorrectCountCheck = 0;
		_bulletsCorrectCountCheck = 0;
		_fireCorrectCheck = false;
		_fireAfterCheck = false;

		if (_gunData.GetStyleFire() == CONSTANTS.FIRE_1_BULLET_BASIC)
		{
			Fire1BulletBasic();
		}
		else if (_gunData.GetStyleFire() == CONSTANTS.FIRE_MULTI_BULLET_BASIC)
		{
			StartCoroutine(FireMutiBulletBasic());
		}
		else if (_gunData.GetStyleFire() == CONSTANTS.FIRE_SHOT_GUN_BASIC)
		{
			FireShotGunBasic();
		}
		else if (_gunData.GetStyleFire() == CONSTANTS.FIRE_1_BULLET_ROTATE)
		{
			Fire1BulletRotate();
		}
	}

	//Cho enemy
	public void Fire(Vector2 dir, int facing)
	{
		SpawnController.instance._bulletEnemy = _gunData.GetBullet();
		_bulletsFired = _gunData.GetNumBullet();
		_bulletsNoCorrectCountCheck = 0;
		_bulletsCorrectCountCheck = 0;
		_fireCorrectCheck = false;
		_fireAfterCheck = false;

		if (_gunData.GetStyleFire() == CONSTANTS.FIRE_1_BULLET_BASIC)
		{
			Fire1BulletBasic(dir, facing);
		}
		else if (_gunData.GetStyleFire() == CONSTANTS.FIRE_MULTI_BULLET_BASIC)
		{
			StartCoroutine(FireMutiBulletBasic(dir, facing));
		}
		else if (_gunData.GetStyleFire() == CONSTANTS.FIRE_SHOT_GUN_BASIC)
		{
			FireShotGunBasic(dir, facing);
		}
		else if (_gunData.GetStyleFire() == CONSTANTS.FIRE_1_BULLET_ROTATE)
		{
			Fire1BulletRotate(dir, facing);
		}
	}

	//tat ca ban truot
	public void CheckBulletStatus(bool notHitTarget)
	{
		if (notHitTarget)
		{
			_bulletsNoCorrectCountCheck++;
			if (_bulletsNoCorrectCountCheck == _bulletsFired)
			{
				PlayerController.instance.fireCorrect = false;
			}
		}
	}

	//trung 1 vien duy nhat
	public void CheckBulletStatus2(bool hitTarget)
	{
		_bulletsCorrectCountCheck++;
		if (hitTarget && !_fireAfterCheck)
		{
			_fireAfterCheck = true;
			_fireCorrectCheck = true;
		}
	}

	public void SetFalseCheckCorrect()
	{
		_fireCorrectCheck = false;
	}

	//bullet basic
	void Fire1BulletBasic()
	{
        AudioManager.instance.PlaySound(_gunData.GetShotSound(), 0, false);
		CamController.instance.ShakeCamera(_gunData.GetShakeGun());
        SpawnController.instance.SpawnBulletPlayer(_gunData.GetNumID(), this.transform.position, PlayerController.instance._aimPos - (Vector2)this.transform.position, PlayerController.instance.facingDir, _gunData.GetSpeedBullet());
	}

	void Fire1BulletBasic(Vector2 dir, int facing)
	{
		AudioManager.instance.PlaySound(_gunData.GetShotSound(), 0, false);
		SpawnController.instance.SpawnBulletEnemy(_gunData.GetNumID(), this.transform.position, dir, facing, _gunData.GetSpeedBullet());
	}

	IEnumerator FireMutiBulletBasic()
	{
		for (int i = 0; i < _bulletsFired; i++)
		{
			Vector2 dir = (PlayerController.instance._aimPos - (Vector2)this.transform.position).normalized;
			if (i != 0)
			{
				float offsetAngle = Random.Range(-10f, 10f);
				dir = Quaternion.Euler(0, 0, offsetAngle) * dir;
			}
            AudioManager.instance.PlaySound(_gunData.GetShotSound(), 0, false);
			CamController.instance.ShakeCamera(_gunData.GetShakeGun());
			SpawnController.instance.SpawnBulletPlayer(_gunData.GetNumID(), this.transform.position, dir, PlayerController.instance.facingDir, _gunData.GetSpeedBullet());
			yield return new WaitForSeconds(0.05f);
		}
	}

	IEnumerator FireMutiBulletBasic(Vector2 dir, int facing)
	{
		for (int i = 0; i < _bulletsFired; i++)
		{
			if (i != 0)
			{
				float offsetAngle = Random.Range(-10f, 10f);
				dir = Quaternion.Euler(0, 0, offsetAngle) * dir;
			}
            AudioManager.instance.PlaySound(_gunData.GetShotSound(), 0, false);
			SpawnController.instance.SpawnBulletEnemy(_gunData.GetNumID(), this.transform.position, dir, facing, _gunData.GetSpeedBullet());
            yield return new WaitForSeconds(0.05f);
		}
	}

	void FireShotGunBasic()
	{
		for (int i = 0; i < _bulletsFired; i++)
		{
			Vector2 dir = (PlayerController.instance._aimPos - (Vector2)this.transform.position).normalized;
			if (i != 0)
			{
				float offsetAngle = Random.Range(-12f, 12f);
				dir = Quaternion.Euler(0, 0, offsetAngle) * dir;
			}
			else
			{
				float offsetAngle = Random.Range(-2f, 2f);
				dir = Quaternion.Euler(0, 0, offsetAngle) * dir;
			}
            AudioManager.instance.PlaySound(_gunData.GetShotSound(), 0, false);
			CamController.instance.ShakeCamera(_gunData.GetShakeGun());
			SpawnController.instance.SpawnBulletPlayer(_gunData.GetNumID(), this.transform.position, dir, PlayerController.instance.facingDir, _gunData.GetSpeedBullet());
		}
	}

	void FireShotGunBasic(Vector2 dir, int facing)
	{
		for (int i = 0; i < _bulletsFired; i++)
		{
			if (i != 0)
			{
				float offsetAngle = Random.Range(-12f, 12f);
				dir = Quaternion.Euler(0, 0, offsetAngle) * dir;
			}
			else
			{
				float offsetAngle = Random.Range(-2f, 2f);
				dir = Quaternion.Euler(0, 0, offsetAngle) * dir;
			}
            AudioManager.instance.PlaySound(_gunData.GetShotSound(), 0, false);
            SpawnController.instance.SpawnBulletEnemy(_gunData.GetNumID(), this.transform.position, dir, facing, _gunData.GetSpeedBullet());
		}
	}

	//bullet rotate
	void Fire1BulletRotate()
	{
		this.GetComponent<Renderer>().enabled = false;
        AudioManager.instance.PlaySound(_gunData.GetShotSound(), 0, false);
		CamController.instance.ShakeCamera(_gunData.GetShakeGun());
		SpawnController.instance.SpawnBulletPlayer(_gunData.GetNumID(), this.transform.position, PlayerController.instance._aimPos - (Vector2)this.transform.position, PlayerController.instance.facingDir, _gunData.GetSpeedBullet());
	}

	void Fire1BulletRotate(Vector2 dir, int facing)
	{
		SpawnController.instance.SpawnBulletEnemy(_gunData.GetNumID(), this.transform.position, dir, facing, _gunData.GetSpeedBullet());
        AudioManager.instance.PlaySound(_gunData.GetShotSound(), 0, false);
    }

	IEnumerator FireMutiBulletRotate()
	{
		for (int i = 0; i < _bulletsFired; i++)
		{
			Vector2 dir = (PlayerController.instance._aimPos - (Vector2)this.transform.position).normalized;
			if (i != 0)
			{
				float offsetAngle = Random.Range(-10f, 10f);
				dir = Quaternion.Euler(0, 0, offsetAngle) * dir;
			}
            AudioManager.instance.PlaySound(_gunData.GetShotSound(), 0, false);
			CamController.instance.ShakeCamera(_gunData.GetShakeGun());
			SpawnController.instance.SpawnBulletPlayer(_gunData.GetNumID(), this.transform.position, dir, PlayerController.instance.facingDir, _gunData.GetSpeedBullet());
			yield return new WaitForSeconds(0.05f);
		}
	}

	IEnumerator FireMutiBulletRotate(Vector2 dir, int facing)
	{
		for (int i = 0; i < _bulletsFired; i++)
		{
			if (i != 0)
			{
				float offsetAngle = Random.Range(-10f, 10f);
				dir = Quaternion.Euler(0, 0, offsetAngle) * dir;
			}

			SpawnController.instance.SpawnBulletEnemy(_gunData.GetNumID(), this.transform.position, dir, facing, _gunData.GetSpeedBullet());
            AudioManager.instance.PlaySound(_gunData.GetShotSound(), 0, false);
            yield return new WaitForSeconds(0.05f);
		}
	}

	public void SetGunData(GunData newGunData)
	{
		_gunData = newGunData;
		UpdateGunProperties();
	}

	void UpdateGunProperties()
	{
		this.GetComponent<SpriteRenderer>().sprite = _gunData.GetSpriteGun();
    }
}