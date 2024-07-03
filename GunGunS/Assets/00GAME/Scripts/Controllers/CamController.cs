using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] Camera _camera;
    

    Vector3 _offset;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution((int)Screen.width, (int)Screen.height, true);
        _camera = Camera.main;
        _offset = _player.position - _camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.isFollow)
            _camera.transform.position = new Vector3(_camera.transform.position.x, _player.position.y - _offset.y, _camera.transform.position.z);
    }
}
