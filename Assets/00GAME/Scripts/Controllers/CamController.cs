using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : Singleton<CamController>
{
    [SerializeField] Transform _player;
    [SerializeField] CinemachineVirtualCamera _virtualCamera;

    Vector3 _offset;

    //shake
    [SerializeField] float _shakeIntensity;
    [SerializeField] float _shakeTime;
    [SerializeField] float _timer;
    [SerializeField] CinemachineBasicMultiChannelPerlin _cbmcp;


    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution((int)Screen.width, (int)Screen.height, true);
        _offset = _player.position - _virtualCamera.transform.position;
        StopShake();
    }

    public void ShakeCamera(float shakeGun)
    {
        _cbmcp = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = _shakeIntensity;

        _shakeIntensity = shakeGun;
		_timer = _shakeTime;
    }

    public void StopShake()
    {
		_cbmcp = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = 0f;

		_timer = 0;
	}

    void Update()
    {
        if (PlayerController.instance.isFollow)
			_virtualCamera.transform.position = new Vector3(_virtualCamera.transform.position.x, _player.position.y - _offset.y, _virtualCamera.transform.position.z);

        if(_timer > 0)
        {
            _timer -= Time.deltaTime;
            if(_timer <= 0)
                StopShake() ;
        }
    }
}
