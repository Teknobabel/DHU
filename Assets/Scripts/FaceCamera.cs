using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		m_Camera = GameManager.m_gameManager.m_camera;
	}
	
	private GameObject m_Camera;
	
	void Update()
	{
		transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.back,
		                 m_Camera.transform.rotation * Vector3.up);
	}
}
