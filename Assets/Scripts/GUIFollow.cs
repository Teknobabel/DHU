using UnityEngine;
using System.Collections;

public class GUIFollow : MonoBehaviour {
	
//	public static GUIFollow m_guiFollow;
	
	private GameObject m_target;
	
	public Camera m_camera;
	
	public UILabel[]
		m_labels;

	private Vector3
		m_offset = Vector3.zero;
	
	void Awake ()
	{
//		m_guiFollow = this;	
	}

	public void SetTarget (GameObject target) {
		m_target = target;
		
	}

	public void SetTarget (GameObject target, Vector3 offset)
	{
		m_target = target;
		m_offset = offset;
		}
	
	// Update is called once per frame
	void Update () {

		if (m_target != null)
		{
			//Vector3 screenPos = m_camera.WorldToScreenPoint(targetPos);
			Vector3 screenPos = FollowCamera.m_followCamera.m_camera.WorldToScreenPoint(m_target.transform.position);
			float screenHeight = Screen.height;
			float screenWidth = Screen.width;
			screenPos.x -= (screenWidth / 2.0f);
			screenPos.y -= (screenHeight / 2.0f);
			transform.localPosition = screenPos + m_offset;
			//transform.localPosition = screenPos;
		}
	
	}
}
