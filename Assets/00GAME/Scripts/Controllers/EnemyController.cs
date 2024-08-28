using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	[SerializeField] GameObject _gun;
	[SerializeField] GameObject bodyOB;

	protected int _coin;
	protected int _coins;
	protected bool _isDeath;
	protected Rigidbody2D _rb;

	[SerializeField] Collider2D _headCol;
	[SerializeField] Collider2D _bodyCol;
	[SerializeField] List<GunData> _gunDatas;

	// Start is called before the first frame update
	public virtual void Start()
	{
		_rb = this.GetComponent<Rigidbody2D>();
	}

	public virtual void Init(Vector2 pos, int dir)
	{
		_coin = 1;
		_coins = Random.Range(2, 4);
		this._isDeath = false;
		_headCol.isTrigger = false;
		_bodyCol.isTrigger = false;

		//init sung
		int numRandomGun = Random.Range(0, _gunDatas.Count);
		this.GetComponentInChildren<GunController>().SetGunData(_gunDatas[numRandomGun]);

		bodyOB.GetComponent<SpriteRenderer>().material.color = new Color(Random.value, Random.value, Random.value);

		this.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

		if (dir == -1)
			this.transform.localScale = new Vector3(this.transform.localScale.x > 0 ? -this.transform.localScale.x : this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
		else
			this.transform.localScale = new Vector3(this.transform.localScale.x < 0 ? -this.transform.localScale.x : this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);

		foreach (Transform a in this.transform)
		{
			a.gameObject.SetActive(true);
		}
		transform.position = pos;
		

        if (_rb != null)
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        this.gameObject.SetActive(true);

    }

	public virtual void Update()
	{
		if (GameManager.instance.gameState != GameManager.GAME_STATE.PLAY)
		{
			return;
		}

		if (this.transform.position.y < PlayerController.instance.transform.position.y - 5f)
		{
			this.gameObject.SetActive(false);
		}

		if (_isDeath)
			return;

		//auto aim player
		Vector2 direction = PlayerController.instance.transform.position - _gun.transform.position;

		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		if (angle < -90)
			angle = angle + 180;

		_gun.transform.eulerAngles = new Vector3(_gun.transform.eulerAngles.x, _gun.transform.eulerAngles.y, -angle);

		//ban player
		if (PlayerController.instance.fireCorrect == false)
		{
			_gun.GetComponent<GunController>().Fire(((Vector2)PlayerController.instance.transform.position - (Vector2)_gun.transform.position), (this.transform.localScale.x > 0 ? -1 : 1));
			PlayerController.instance.fireCorrect = true;
		}
	}

	public virtual void TakeDamage(int hit)
	{
		if (_isDeath) return;

		Death(hit);
	}

	public virtual void Death(int hit)
	{
		if (_isDeath) return;

        _rb.constraints = RigidbodyConstraints2D.None;
        SpawnController.instance.SpawnBloodBurst(this.transform.position);

		SpawnController.instance._canUpColor = true;
		GameManager.instance.UpdateScoreAndMoney(hit, (hit == 1 ? 1 : GetCoin()));
		_isDeath = true;

		_headCol.isTrigger = true;
		_bodyCol.isTrigger = true;
		if (hit == 1)
			SpawnController.instance.SpawnCoins(this.transform.position, _coin);
		else if (hit == 2)
		{
			SpawnController.instance.SpawnCoins(this.transform.position, _coins);
			Observer.instance.Notify(CONSTANTS.UIPLAY_COMBOTXT, null);
			Observer.instance.Notify(CONSTANTS.UIPLAY_HEADSHOT, null);
			AudioManager.instance.PlaySound(AudioManager.instance.UIClips[7], 0, false);
		}
		_rb.AddForce(Vector2.up * Random.Range(400f, 500f));
		_rb.AddTorque(Random.Range(500f, 600f));
		AudioManager.instance.PlaySound(AudioManager.instance.UIClips[11], 0, false);
		PlayerController.instance.isMove = true;
		PlayerController.instance.spawnCheck = true;
		GameManager.instance.enemyDie++;

	}

	public int GetCoin()
	{
		return _coins;
	}

}