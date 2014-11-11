using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour {

	public Vector3 m_upVector = Vector3.up;

	// Use this for initialization
	void Start () {
		m_Camera = GameManager.m_gameManager.m_camera;
	}

	private GameObject m_Camera;
	
	void Update()
	{
		transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.back,
		                 m_Camera.transform.rotation * Vector3.up);

//		transform.LookAt (m_Camera.transform.position);
	}
}
