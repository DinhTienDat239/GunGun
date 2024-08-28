using UnityEngine;

public class RunEnemyController : EnemyController
{
    [SerializeField] float _speed;
    [SerializeField] int _dirMove;
    [SerializeField] float _patrolDistance;
    Vector2 _startPosition;
    float _currentPatrolDistance;

    public override void Start()
    {
        base.Start();
    }

    public override void Init(Vector2 pos, int dir)
    {
        base.Init(pos, dir);
        _coin = 2;
        _coins = Random.Range(3, 6);
        _startPosition = pos;
        _currentPatrolDistance = 0;
        _dirMove = -dir;
        UpdateSpeed();
    }

    void UpdateSpeed()
    {
        if (GameManager.instance.score > 300)
        {
            _speed = 2.5f;
        }
        else if (GameManager.instance.score > 200)
        {
            _speed = 2f;
        }
        else if (GameManager.instance.score > 100)
        {
            _speed = 1.8f;
        }
        else if (GameManager.instance.score > 50)
        {
            _speed = 1.6f;
        }
        else if (GameManager.instance.score > 20)
        {
            _speed = 1.4f;
        }
        else
        {
            _speed = 1.2f;
        }
    }

    public override void Update()
    {
        if (GameManager.instance.gameState != GameManager.GAME_STATE.PLAY)
        {
            return;
        }

        base.Update();

        Movement();
    }

    public override void TakeDamage(int hit)
    {
        base.TakeDamage(hit);
    }

    public override void Death(int hit)
    {
        base.Death(hit);
    }

    void Movement()
    {
        if (_isDeath)
            return;
        //khoang cach di duoc
        _currentPatrolDistance += _speed * Time.deltaTime;

        //neu di het khoang cach thi doi huong
        if (_currentPatrolDistance >= _patrolDistance)
        {
            _dirMove = -_dirMove;
            _currentPatrolDistance = 0;
        }

        //di chuyen
        this.transform.Translate(Vector2.right * _speed * _dirMove * Time.deltaTime);

    }
}