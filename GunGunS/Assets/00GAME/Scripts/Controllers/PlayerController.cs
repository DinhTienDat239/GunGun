using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] float _speed;
    //Facing var
    public int facingDir = -1;

    //Bool vars
    [Header("-- Bool Vars --")]
    [SerializeField] public bool fireCorrect;
    [SerializeField] public bool isMove;
    [SerializeField] bool _isJump;
    [SerializeField] public bool shooted;

    //Aiming mechanic vars
    [Header("-- Aim info --")]
    public Vector2 _aimPos = Vector2.zero;

    [SerializeField] float _speedRotate;
    [SerializeField] float _angleRotate;
    [SerializeField] int _rotationDir;
    [SerializeField] GameObject _gun;
    [SerializeField] public VisionCone _vs;

    //Components
    Rigidbody2D _rb;
    LineRenderer _lineAim;
    SpriteRenderer _spriteRenderer;

    //Pos origin
    Vector3? posOrigin = null;

    public void Start()
    {
        _lineAim = GetComponentInChildren<LineRenderer>();
        _lineAim.startWidth = 0.03f;
        _lineAim.endWidth = 0.03f;
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        posOrigin = this.transform.position;
    }

    public void Update()
    {
        if (GameManager.instance.gameState != GameManager.GAME_STATE.PLAY)
        {
            DeleteLineAim();
            return;
        }

        if (Input.GetMouseButtonDown(0) && !shooted)
        {
            shooted = true;
            GameManager.instance.ResetTime();
            _gun.GetComponent<GunController>().Fire();
        }

        Movement();
        UpdateDificult();

    }

    void UpdateDificult()
    {
        if (GameManager.instance.score > 300)
        {
            _speedRotate = 70;
            _speed = 4.5f;
        }
        else if (GameManager.instance.score > 200)
        {
            _speedRotate = 65f;
            _speed = 4f;
        }
        else if (GameManager.instance.score > 100)
        {
            _speedRotate = 60f;
            _speed = 3.8f;
        }
        else if (GameManager.instance.score > 50)
        {
            _speedRotate = 55f;
            _speed = 3.6f;
        }
        else if (GameManager.instance.score > 20)
        {
            _speedRotate = 50f;
            _speed = 3.4f;
        }
        else
        {
            _speedRotate = 45f;
            _speed = 3.2f;
        }
    }

    public void Init()
    {
        _speed = 3;
        _speedRotate = 45f;
        _gun.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        _angleRotate = 180f;
        this.transform.position = (Vector3)posOrigin;
        if (this.facingDir == 1)
            this.Flip();
        fireCorrect = true;
        shooted = false;

		SpawnController.instance._bullet = this.GetComponentInChildren<GunController>().GetGunData().GetBullet();
        _vs.transform.position = _gun.GetComponent<GunController>().transform.position;
    }

    private void Movement()
    {
        if (shooted)
            DeleteLineAim();

        if (!isMove)
        {
            if (!shooted)
                UpdateAim();
            _rb.velocity = new Vector2(0, _rb.velocity.y);
            return;
        }

        if (_isJump)
        {
            _rb.velocity = new Vector2(_speed * facingDir, _speed * (_speed >= 3.6f ? 1.2f : 1.4f));
            return;
        }

        _rb.velocity = new Vector2(_speed * facingDir, _rb.velocity.y);
    }

    void UpdateAim()
    {
        _vs.gameObject.SetActive(true);
        PlayerController.instance._vs.VisionRange = _gun.GetComponent<GunController>().GetGunData().GetRadiusRotate() * 3;

        if (facingDir == -1)
        {
            _gun.transform.eulerAngles = new Vector3(_gun.transform.eulerAngles.x, _gun.transform.eulerAngles.y, 180 - _angleRotate);
            float t = _speedRotate * _rotationDir * Time.deltaTime;
            if (_angleRotate - t > 120 && _angleRotate - t < 180)
                _angleRotate -= t;
            else
                _rotationDir *= -1;

            _vs.VisionAngle = Mathf.Deg2Rad * (180 - _angleRotate);
            _vs.transform.rotation = Quaternion.Euler(new Vector3(- (180 -_angleRotate) / 2, -90, 90));
        }
        else
        {
            _gun.transform.eulerAngles = new Vector3(_gun.transform.eulerAngles.x, _gun.transform.eulerAngles.y, -_angleRotate);
            float t = _speedRotate * _rotationDir * Time.deltaTime;
            if (_angleRotate + t > 0 && _angleRotate + t < 60)
                _angleRotate += t;
            else
                _rotationDir *= -1;

            _vs.VisionAngle = Mathf.Deg2Rad * _angleRotate;
            _vs.transform.rotation = Quaternion.Euler(new Vector3((_angleRotate / 2) +180, -90, 90));
        }


        

        float x = _gun.GetComponent<GunController>().GetObOriginAim().transform.position.x + _gun.GetComponent<GunController>().GetGunData().GetRadiusRotate() * Mathf.Cos(_angleRotate * Mathf.Deg2Rad);
        float y = _gun.GetComponent<GunController>().GetObOriginAim().transform.position.y + _gun.GetComponent<GunController>().GetGunData().GetRadiusRotate() * Mathf.Sin(_angleRotate * Mathf.Deg2Rad);
        _aimPos = new Vector2(x, y);
        _lineAim.positionCount = 2;
        _lineAim.SetPosition(0, _gun.GetComponent<GunController>().GetObOriginAim().transform.position);
        _lineAim.SetPosition(1, _aimPos);
        
    }

    void DeleteLineAim()
    {
        _lineAim.positionCount = 0;
        _vs.VisionAngle = 0;
        _vs.gameObject.SetActive(false);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(_gun.GetComponent<GunController>().GetObOriginAim().transform.position, (Vector2)_aimPos);
    }

    public void Flip()
    {
        facingDir = facingDir * -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        if (facingDir == -1)
            _angleRotate = 180;
        else
            _angleRotate = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Jump")
        {
            _isJump = true;
        }

        if (collision.gameObject.tag == "Stop")
        {
            Flip();
            isMove = false;
            shooted = false;
        }

        if (collision.gameObject.tag == "ColSpawn")
        {
            // Tìm Stop
            Transform parentTransform = collision.transform.parent;
            GameObject stopObject = null;

            if (parentTransform != null)
            {
                foreach (Transform child in parentTransform)
                {
                    if (child.CompareTag("Stop"))
                    {
                        stopObject = child.gameObject;
                        break;
                    }
                }
            }

            // Spawn
            if (stopObject != null)
            {
                Vector2 spawnPosition = stopObject.transform.position;
                spawnPosition = new Vector2(spawnPosition.x, spawnPosition.y + 0.4f);
                if (GameManager.instance.enemyDie % 10 == 0 && GameManager.instance.enemyDie != 0)
                    SpawnController.instance.SpawnSpecialEnemy(spawnPosition, parentTransform.rotation.y == 0 ? -1 : 1);
                else
                    SpawnController.instance.SpawnEnemy(spawnPosition, parentTransform.rotation.y == 0 ? -1 : 1);
            }
            else
            {
                Debug.LogWarning("No Stop");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Jump")
            _isJump = false;
    }

    public void SetSkin(SkinData skinData)
    {
        _spriteRenderer.sprite = skinData.GetSpriteSkin();
        
    }
    public Sprite GetSkin()
    {
        return _spriteRenderer.sprite;
    }
}