using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {
	
	public static FollowCamera
		m_followCamera;
	
    public GameObject target;
	public Transform m_moveTransform;
    public float damping = 1;

	public Camera m_camera;
	
	private bool
		m_manualMove = false;

	private Vector3
		m_maxZoom = new Vector3(0.4999f,-3.4355f,2.1422f),
		m_minZoom = new Vector3(0,1.9172f,-1.1955f);

	private float
		m_zoomDist = 0;

	private Vector3 offset = Vector3.zero;
	private Vector3 targetOffset = new Vector3(0,0,-1.5f);
	
	private DepthOfFieldScatter
		m_DOF;
	
	void Awake ()
	{
		m_followCamera = this;
		m_DOF = (DepthOfFieldScatter)transform.GetComponent("DepthOfFieldScatter");	
	}
	
    public void Initialize (GameObject cameraTarget) {
		
		offset = new Vector3(0,1.5f,-4);
		
		target = cameraTarget;
		Vector3 position = Player.m_player.transform.position;
		position.y = m_moveTransform.position.y + offset.y;
		position.z += offset.z;
		m_moveTransform.position = position;

		ChangeZoomDistance (0.5f);
//		Vector3 camTarget = target.transform.position;
//		camTarget.z -= 1;
//        transform.LookAt(camTarget + targetOffset);
		
    }
	
	public void MoveCamera (Vector3 pos)
	{
		if (!m_manualMove)
		{
			m_manualMove = true;	
		}
		Vector3 newPos = m_moveTransform.position + pos;
		m_moveTransform.position = newPos;
	}
	
	public void SetTarget (GameObject cameraTarget)
	{
		m_manualMove = false;
		target = cameraTarget;
		m_DOF.focalTransform = cameraTarget.transform;

	}
	
	public void RotateCamera (float orbitDegrees)
	{
//		float orbitDistance = 5.17f;
//		
//		transform.position = target.transform.position + (transform.position - target.transform.position).normalized * orbitDistance;
//        transform.RotateAround(target.transform.position, Vector3.up, orbitDegrees * Time.deltaTime);
//		
//		offset = transform.position - target.transform.position;
	}
	
    void LateUpdate() {

		if (target != null && !m_manualMove)
		{
			Vector3 desiredPosition = target.transform.position;
			desiredPosition.y = m_moveTransform.position.y;
			desiredPosition.z +=  offset.z;

			Vector3 position = Vector3.Lerp(m_moveTransform.position, desiredPosition, Time.deltaTime * damping);
			//position.y = transform.position.y;
			m_moveTransform.position = position;
		}
    }

	public void ChangeZoomDistance (float zoomChange)
	{
		m_zoomDist = Mathf.Clamp (m_zoomDist + zoomChange, 0.0f, 1.0f);
		Vector3 newPos = Vector3.Lerp (m_minZoom, m_maxZoom, m_zoomDist);
		Vector3 tOffset = Vector3.Lerp (targetOffset, Vector3.zero, m_zoomDist);
		//Debug.Log (newPos);
		m_camera.transform.localPosition = newPos;
		if (target != null) {
			transform.LookAt (target.transform.position + tOffset);
		}
		//Debug.Log (m_camera.transform.position);
	}

	public void SetZoomDistance (float newDist)
	{
		m_zoomDist = Mathf.Clamp (newDist, 0.0f, 1.0f);
		Vector3 newPos = Vector3.Lerp (m_minZoom, m_maxZoom, m_zoomDist);
		Vector3 tOffset = Vector3.Lerp (targetOffset, Vector3.zero, m_zoomDist);
		m_camera.transform.localPosition = newPos;
		if (target != null) {
			transform.LookAt (target.transform.position + tOffset);
		}
	}

	public float zoomDist {get{return m_zoomDist;}}
}
