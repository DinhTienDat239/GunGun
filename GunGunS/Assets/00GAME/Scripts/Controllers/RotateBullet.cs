using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotateBullet : MonoBehaviour
{
	[SerializeField] float _rotationSpeed;

	void Update()
	{
		this.transform.Rotate(0, 0, _rotationSpeed * (this.transform.localScale.x > 0 ? -1 : 1) * Time.deltaTime);
	}

}