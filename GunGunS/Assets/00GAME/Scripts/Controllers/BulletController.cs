using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    GunController _gunController;
    [SerializeField] float _speed;
    bool _collided;
    Rigidbody2D _rb;
    Vector2 _dir;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 pos, Vector2 dir, float speed, int facing, string tag)
    {
        if(tag == CONSTANTS.BULLET_PLAYER)
            _gunController = PlayerController.instance.GetComponentInChildren<GunController>();
        _speed = speed;
        if(facing == -1)
        {
            if(this.transform.localScale.x > 0)
                this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
            else
				this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
		}
        else
        {
			if (this.transform.localScale.x > 0)
				this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
			else
				this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
		}
        this.gameObject.tag = tag;
        _dir = dir.normalized;
        this.transform.position = new Vector3(pos.x, pos.y, this.transform.position.z);
        _collided = false;
        this.gameObject.SetActive(true);
    }

    void Update()
    {
        _rb.velocity = _dir * _speed;
        if (CheckFireNotCorrect())
        {
            _gunController.CheckBulletStatus(true);
            this.gameObject.SetActive(false);
        }
    }

    bool CheckFireNotCorrect()
    {
        if (Mathf.Abs(this.transform.position.x - PlayerController.instance.transform.position.x) > 10 ||
            Mathf.Abs(this.transform.position.y - PlayerController.instance.transform.position.y) > 10)
        {
            if (this.gameObject.tag == "BulletPlayer")
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_collided)
            return;

        if (this.gameObject.tag == "BulletPlayer")
        {
            if (collision.gameObject.tag == "EnemyBody")
            {
                _gunController.CheckBulletStatus2(true);
                _collided = true;
                this.gameObject.SetActive(false);
                EnemyController _ec = collision.transform.parent.gameObject.GetComponentInParent<EnemyController>();
                _ec.TakeDamage(1);
                return;
            }
            if (collision.gameObject.tag == "EnemyHead")
            {
                _gunController.CheckBulletStatus2(true);
                _collided = true;
                this.gameObject.SetActive(false);
                EnemyController _ec = collision.transform.parent.gameObject.GetComponentInParent<EnemyController>();
                _ec.TakeDamage(2);
                return;
            }
        }

        if (this.gameObject.tag == "BulletEnemy" && collision.gameObject.tag == "Player")
        {
            //lose
            GameManager.instance.ChangeState(GameManager.GAME_STATE.OVER);
            this.gameObject.SetActive(false);
            PlayerController.instance.Death();
        }
    }
}