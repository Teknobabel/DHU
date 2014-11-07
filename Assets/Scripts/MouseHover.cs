using UnityEngine;
using System.Collections;

public class MouseHover : MonoBehaviour {

	public string m_text = "None";
	public UILabel m_label;
	public TypogenicText m_label02;

	public GameObject[]
		m_objects,
		m_deactivate;

	public bool m_active = false;

	// Use this for initialization
	void Start () {
	
	}

	void OnMouseEnter()
	{
		if (this.enabled) {
			if (m_label != null) {
				m_label.text = m_text;
				m_label.gameObject.SetActive (true);
				m_active = true;
			} else if (m_label02 != null) {
				m_label02.Text = m_text;
				m_label02.gameObject.SetActive (true);
				m_active = true;
				}
			foreach (GameObject go in m_objects) {
				go.SetActive (true);
			}
			foreach (GameObject go in m_deactivate) {
				go.SetActive (false);
			}
		}
	}

	void OnDisable()
	{
		foreach (GameObject go in m_objects)
		{
			go.SetActive(false);
		}
		if (m_label02 != null && m_active) {
			m_label02.gameObject.SetActive (false);
			m_active = false;
		}
//		if (m_deactivate.Length > 0)
//		{
//			foreach (GameObject go in m_deactivate)
//			{
//				go.SetActive(true);
//			}
//		}
	}

	void OnMouseExit()
	{
		if (m_label != null)
		{
			m_label.gameObject.SetActive (false);
			m_active = false;
		} else if (m_label02 != null) {
			m_label02.gameObject.SetActive (false);
			m_active = false;
		}

		if (m_objects.Length > 0) {
			foreach (GameObject go in m_objects) {
				go.SetActive (false);
			}
		}
		if (m_deactivate.Length > 0) {
			foreach (GameObject go in m_deactivate) {
				go.SetActive (true);
			}
		}
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}
}
