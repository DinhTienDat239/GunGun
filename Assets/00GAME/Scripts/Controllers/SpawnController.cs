using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnController : Singleton<SpawnController>
{
	[SerializeField] public List<GameObject> _stairPrefabs;
	[SerializeField] public GameObject _ground;

	[SerializeField] public int _stairNum;
	[SerializeField] GameObject _lastStair;

	[SerializeField] public GameObject _bullet;
	[SerializeField] public GameObject _bulletEnemy;

	[SerializeField] List<GameObject> _enemyPrefab;
	[SerializeField] List<GameObject> _enemySpecialPrefab;
	[SerializeField] GameObject _coinPrefab;

	[SerializeField] GameObject _bloodBurst, _confettiBurst;

    [SerializeField] bool _canSpawnEnemy = true;

    public bool _canUpColor;

	void Start()
	{
		for(int i = 0; i < _stairPrefabs.Count; i++)
		{
            ObjectPooling.instance.CreatePool(_stairPrefabs[i], _stairNum);
        }
		foreach(var i in _enemyPrefab)
		{
            ObjectPooling.instance.CreatePool(i, 1);
        }
        foreach (var i in _enemySpecialPrefab)
        {
            ObjectPooling.instance.CreatePool(i, 1);
        }
        ObjectPooling.instance.CreatePool(_coinPrefab, 5);
		ObjectPooling.instance.CreatePool(_bloodBurst, 3);
		ObjectPooling.instance.CreatePool(_confettiBurst, 1);
	}

	public void Init()
	{
		_canUpColor = false;
		DeSpawnAll();
		_ground.GetComponent<Ground>().Init();
		SpawnStairs(_ground, _stairNum);
	}

	public GameObject SpawnStair(GameObject aboveObj, int leftRight)
	{
		GameObject stair = ObjectPooling.instance.GetObject(_stairPrefabs[Random.Range(0, _stairPrefabs.Count)]);
		Stair s = stair.GetComponent<Stair>();
		if (leftRight != -1)
		{
			stair.transform.rotation = new Quaternion(0, 180, 0, 0);
		}
		else
		{
			stair.transform.rotation = new Quaternion(0, 0, 0, 0);
		}
		stair.SetActive(true);

		if (aboveObj.GetComponent<Stair>() == null)
		{
			stair.transform.position = new Vector3(0, aboveObj.transform.position.y + (aboveObj.transform.lossyScale.y / 2) + (s.GetHeight() / 2), 0);
			s.Init();
		}
		else
		{
			Stair underS = aboveObj.GetComponent<Stair>();
			stair.transform.position = new Vector3(0, aboveObj.transform.position.y + (underS.GetHeight() / 2) + (s.GetHeight() / 2), 0);
			s.Init(underS);
		}
		return stair;
	}

	public void SpawnStairs(GameObject aboveObj, int num)
	{
		_canUpColor = false;
		GameObject obj = SpawnStair(aboveObj, -1);
		for (int i = 1; i < num; i++)
		{
			if (i % 2 == 1)
			{
				obj = SpawnStair(obj, 1);
			}
			else
			{
				obj = SpawnStair(obj, -1);
			}
			if (i == num - 1)
				_lastStair = obj;
		}

	}

	public void DeSpawnAll()
	{
		foreach (Transform child in this.transform)
		{
			child.gameObject.SetActive(false);
		}
	}

	public void SpawnBulletPlayer(int id, Vector2 pos, Vector2 dir, int facing, float speed)
	{
		ObjectPooling.instance.GetObject(_bullet).GetComponent<BulletController>().Init(pos, dir, speed, facing, CONSTANTS.BULLET_PLAYER);
	}

	public void SpawnBulletEnemy(int id, Vector2 pos, Vector2 dir, int facing, float speed)
	{
		ObjectPooling.instance.GetObject(_bulletEnemy).GetComponent<BulletController>().Init(pos, dir, speed, facing, CONSTANTS.BULLET_ENEMY);
	}

	public void SpawnEnemy(Vector2 pos, int dir)
	{
        if (!_canSpawnEnemy)
            return;
        StartCoroutine(WaitSpawn());

        int numRandom = Random.Range(0, _enemyPrefab.Count);
        GameObject g = ObjectPooling.instance.GetObject(_enemyPrefab[numRandom]);
        g.GetComponent<EnemyController>().Init(pos, dir);
	}

	public void SpawnSpecialEnemy(Vector2 pos, int dir)
	{
        if (!_canSpawnEnemy)
            return;
        StartCoroutine(WaitSpawn());

        int numRandom = Random.Range(0, _enemySpecialPrefab.Count);
		GameObject g = ObjectPooling.instance.GetObject(_enemySpecialPrefab[numRandom]);
		g.GetComponent<EnemyController>().Init(pos, dir);
	}

	public void SpawnCoins(Vector2 pos, int numb)
	{
		for (int i = 0; i < numb; i++)
		{
			ObjectPooling.instance.GetObject(_coinPrefab).GetComponent<CoinController>().Init(pos);
		}
	}

	public void SpawnBloodBurst(Vector3 pos)
	{
		ObjectPooling.instance.GetObject(_bloodBurst).GetComponent<BurstParticles>().Init(pos);
	}

	public void SpawnConfettiBurst(Transform parent)
	{
		GameObject g = Instantiate(_confettiBurst,parent);
		this.transform.localPosition = new Vector3(parent.position.x, parent.position.y, 0);

	}

	private void Update()
	{
		if (_lastStair == null)
			return;

		if (_lastStair.transform.position.y - PlayerController.instance.transform.position.y < 8)
			SpawnStairs(_lastStair, _stairNum);
	}
    IEnumerator WaitSpawn()
    {
        _canSpawnEnemy = false;
        yield return new WaitForSeconds(0.5f);
        _canSpawnEnemy = true;
    }
}