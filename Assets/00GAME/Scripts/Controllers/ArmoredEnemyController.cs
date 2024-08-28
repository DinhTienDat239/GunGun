using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoredEnemyController : EnemyController
{
    [SerializeField] GameObject _armor;
    bool _hasArmor;
    [SerializeField] float _armorY;

    public override void Start()
    {
        base.Start();
    }

    public override void Init(Vector2 pos, int dir)
    {
        base.Init(pos, dir);
        _coin = 2;
        _coins = Random.Range(3, 6);
        _hasArmor = true;
        _armor.SetActive(true);
        Rigidbody2D armorRb = _armor.GetComponent<Rigidbody2D>();
        armorRb.gravityScale = 0;
        _armor.transform.localPosition = new Vector3(0, _armorY, 0);
        _armor.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public override void Update()
    {
        base.Update();
		
		if (_armor.transform.position.y < PlayerController.instance.transform.position.y - 5f)
        {
            _armor.gameObject.SetActive(false);
        }

        if (_hasArmor)
        {
            _armor.transform.localPosition = new Vector3(0, _armorY, 0);
        }
    }

    public override void TakeDamage(int hit)
    {
        if (_isDeath) return;

        if (!PlayerController.instance.GetComponentInChildren<GunController>()._fireCorrectCheck)
            return;
        PlayerController.instance.GetComponentInChildren<GunController>().SetFalseCheckCorrect();

        if (_hasArmor)
        {
            _hasArmor = false;
            Rigidbody2D armorRb = _armor.GetComponent<Rigidbody2D>();
            armorRb.gravityScale = 2;
            armorRb.AddForce(Vector2.up * 200f);
            armorRb.AddTorque(100f);
            PlayerController.instance.shooted = false;
            AudioManager.instance.PlaySound(AudioManager.instance.UIClips[10], 0, false);
        }
        else
        {
            base.TakeDamage(hit);
        }
    }

    public override void Death(int hit)
    {
        base.Death(hit);
    }

}