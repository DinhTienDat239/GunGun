using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
   

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < PlayerController.instance.transform.position.y - 5f)
            this.gameObject.SetActive(false);
    }
    public void Init(Vector2 pos)
    {
        
        this.transform.position = pos;
        this.gameObject.SetActive(true);
        _rb.AddForce(new Vector2(Random.Range(-0.5f, 0.5f), 1) * Random.Range(100, 300));

    }
}
