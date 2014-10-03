using UnityEngine;
using System.Collections;

public class CineCam : MonoBehaviour {

	public static CineCam m_cineCam;

	public Camera m_camera;

	private float
		m_cineCamTime = 0.5f,
		m_cineCamTimer = 0.0f;

	private bool
		m_cineCamPlaying = false;

	private DepthOfFieldScatter
		m_DOF;

	void Awake ()
	{
		m_cineCam = this;
		m_DOF = (DepthOfFieldScatter)m_camera.transform.GetComponent("DepthOfFieldScatter");	
	}

	// Use this for initialization
	void Start () {
	
	}

	public void ActivateCineCam (Transform target)
	{
		m_cineCamPlaying = true;
		Vector3 pos = target.transform.position;
		pos.y = transform.position.y;
		transform.position = pos;
		Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
		transform.rotation = randomRotation;
		m_DOF.focalTransform = target.transform;
		//m_camera.transform.LookAt (target.position);
		m_camera.gameObject.SetActive (true);
		Time.timeScale = 0.15f;
		animation.Play ();
	}

	private void DeactivateCineCam ()
	{
		m_cineCamPlaying = false;
		m_cineCamTimer = 0.0f;
		Time.timeScale = 1.0f;
		animation.Stop ();
		m_camera.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
		if (m_cineCamPlaying)
		{
			m_cineCamTimer = Mathf.Clamp(m_cineCamTimer + Time.deltaTime, 0.0f, m_cineCamTime);

			if (m_cineCamTimer == m_cineCamTime)
			{
				DeactivateCineCam();
			}
		}
	}
}
