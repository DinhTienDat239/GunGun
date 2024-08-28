using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstParticles : MonoBehaviour
{
	[SerializeField] ParticleSystem _particleSystem;

	void Start()
	{
		_particleSystem = GetComponent<ParticleSystem>();
	}

	public GameObject Init(Vector3 pos)
	{
		if (_particleSystem != null)
		{
			this.transform.position = new Vector3(pos.x, pos.y, -9);
			_particleSystem.gameObject.SetActive(true);
			_particleSystem.Play();
			StartCoroutine(StopParticleSystem(3f));
		}
		return this.gameObject;
	}

    private IEnumerator StopParticleSystem(float time)
	{
		yield return new WaitForSeconds(time);

		if (_particleSystem != null)
		{
			_particleSystem.Stop();
			_particleSystem.gameObject.SetActive(false);
		}
	}

}
