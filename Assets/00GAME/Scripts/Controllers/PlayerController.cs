using System.Collections;
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
    [SerializeField] public bool isDeath;

    //cam follow
    public bool isFollow = true;

    //Aiming mechanic vars
    [Header("-- Aim info --")]
    public Vector2 _aimPos = Vector2.zero;

    [SerializeField] float _speedRotate;
    [SerializeField] float _angleRotate;
    [SerializeField] int _rotationDir;
    [SerializeField] GunController _gun;
    [SerializeField] public VisionCone _vs;

    //Components
    Rigidbody2D _rb;
    LineRenderer _lineAim;
    SpriteRenderer _spriteRenderer;
    Collider2D _col;

    //Pos origin
    Vector3? posOrigin = null;

    //Infor save for revive
    [SerializeField] Vector3? _posSave = null;
    [SerializeField] bool _canFLip = true;

    //Check spawn
    public bool spawnCheck;


    public void Start()
    {

        _lineAim = GetComponentInChildren<LineRenderer>();
        _lineAim.startWidth = 0.015f;
        _lineAim.endWidth = 0.015f;
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _col = GetComponentInChildren<Collider2D>();
        posOrigin = this.transform.position;
    }

    public void Update()
    {
        if (GameManager.instance.gameState != GameManager.GAME_STATE.PLAY)
        {
            DeleteLineAimAndVision();
            return;
        }

        if (Input.GetMouseButtonDown(0) && !shooted)
        {
            shooted = true;
            GameManager.instance.ResetTime();
            _gun.Fire();
        }

        Movement();
        UpdateDificult();

    }

    void UpdateDificult()
    {
        if (GameManager.instance.score > 200)
        {
            _speedRotate = 70;
            _speed = 4.5f;
        }
        else if (GameManager.instance.score > 150)
        {
            _speedRotate = 65f;
            _speed = 4f;
        }
        else if (GameManager.instance.score > 80)
        {
            _speedRotate = 60f;
            _speed = 3.8f;
        }
        else if (GameManager.instance.score > 40)
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
        _canFLip = true;
        _speed = 3;
        _speedRotate = 45f;
        _angleRotate = 180f;
        _col.isTrigger = false;
        isFollow = true;
        isDeath = false;
        _rb.velocity = Vector3.zero;
        _rb.freezeRotation = true;
        this.transform.eulerAngles = Vector3.zero;
        this.transform.position = (Vector3)posOrigin;
        _gun.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        if (this.facingDir == 1)
            this.Flip();
        fireCorrect = true;
        shooted = false;

        spawnCheck = true;
        SpawnController.instance._bullet = this.GetComponentInChildren<GunController>().GetGunData().GetBullet();
        _vs.transform.position = new Vector3(_gun.transform.position.x, _gun.transform.position.y, -0.5f);
        _vs.VisionRange = _gun.GetGunData().GetRadiusRotate() * 3.35f;
    }

    private void Movement()
    {
        if (shooted)
            DeleteLineAimAndVision();

        if (!isMove)
        {
            if (!shooted)
                UpdateAim();
            _rb.velocity = new Vector2(0, _rb.velocity.y);
            return;
        }

        if (_isJump)
        {
            _rb.velocity = new Vector2(_speed * facingDir, _speed * (_speed >= 3.6f ? 1.4f : 1.6f));
            return;
        }

        _rb.velocity = new Vector2(_speed * facingDir, _rb.velocity.y);
    }

    void UpdateAim()
    {
        if (facingDir == -1)
        {
            _gun.transform.eulerAngles = new Vector3(_gun.transform.eulerAngles.x, _gun.transform.eulerAngles.y, 180 - _angleRotate);
            float t = _speedRotate * _rotationDir * Time.deltaTime;
            if (_angleRotate - t > 120 && _angleRotate - t < 180)
                _angleRotate -= t;
            else
                _rotationDir *= -1;
        }
        else
        {
            _gun.transform.eulerAngles = new Vector3(_gun.transform.eulerAngles.x, _gun.transform.eulerAngles.y, -_angleRotate);
            float t = _speedRotate * _rotationDir * Time.deltaTime;
            if (_angleRotate + t > 0 && _angleRotate + t < 60)
                _angleRotate += t;
            else
                _rotationDir *= -1;
        }

        float x = _gun.transform.position.x + _gun.GetGunData().GetRadiusRotate() * Mathf.Cos(_angleRotate * Mathf.Deg2Rad);
        float y = _gun.transform.position.y + _gun.GetGunData().GetRadiusRotate() * Mathf.Sin(_angleRotate * Mathf.Deg2Rad);
        _aimPos = new Vector2(x, y);
        _lineAim.positionCount = 2;
        _lineAim.SetPosition(0, _gun.transform.position);
        _lineAim.SetPosition(1, _aimPos);

        UpdateVision();
    }

    void UpdateVision()
    {
        if (facingDir == -1)
        {
            _vs.VisionAngle = Mathf.Deg2Rad * (180 - _angleRotate);
            _vs.transform.rotation = Quaternion.Euler(new Vector3(-(180 - _angleRotate) / 2, -90, 90));
        }
        else
        {
            _vs.VisionAngle = Mathf.Deg2Rad * _angleRotate;
            _vs.transform.rotation = Quaternion.Euler(new Vector3((_angleRotate / 2) + 180, -90, 90));
        }
        _vs.gameObject.SetActive(true);
    }

    void DeleteLineAimAndVision()
    {
        _vs.gameObject.SetActive(false);
        _lineAim.positionCount = 0;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(_gun.GetObOriginAim().transform.position, (Vector2)_aimPos);
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
        if (isDeath)
            return;

        if (collision.tag == "Jump")
        {
            _isJump = true;
        }

        if (collision.gameObject.tag == "Stop" && _canFLip)
        {
            Flip();
            isMove = false;
            shooted = false;
        }

        if (collision.gameObject.tag == "ColSpawn" && spawnCheck)
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

            // Spawn enemy
            if (stopObject != null)
            {
                Vector2 spawnPosition = stopObject.transform.position;
                if (parentTransform.rotation.y == 0)
                    spawnPosition = new Vector2(spawnPosition.x + Random.Range(0f, 0.5f), spawnPosition.y);
                else
                    spawnPosition = new Vector2(spawnPosition.x + Random.Range(-0.5f, 0f), spawnPosition.y);

                if (GameManager.instance.enemyDie % 5 == 0 && GameManager.instance.enemyDie != 0)
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
        _vs.transform.position = new Vector3(_gun.transform.position.x, _gun.transform.position.y, -0.5f);
        _vs.VisionRange = _gun.GetGunData().GetRadiusRotate() * 3.35f;
    }

    public Sprite GetSkin()
    {
        return _spriteRenderer.sprite;
    }

    public void Death()
    {
        if (isDeath) return;

        SaveBeforeDeath();
        isDeath = true;
        isFollow = false;
        _col.isTrigger = true;
        _rb.freezeRotation = false;
        _canFLip = false;
        _rb.AddForce(Vector2.up * Random.Range(400f, 500f));
        _rb.AddTorque(Random.Range(500f, 600f));

    }

    public void SaveBeforeDeath()
    {
        this._posSave = this.transform.position;

    }

    public void LoadAfterRevive()
    {
        // no spawn
        spawnCheck = false;

        this.transform.position = (Vector3)this._posSave;
        this.transform.rotation = Quaternion.Euler(Vector3.zero);

        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0f;

        shooted = false;
        isDeath = false;
        isFollow = true;
        _col.isTrigger = false;
        _rb.freezeRotation = true;
        StartCoroutine(ResetCanFlip());
    }

    IEnumerator ResetCanFlip()
    {
        yield return new WaitForSeconds(0.2f);
        _canFLip = true;
    }
}
