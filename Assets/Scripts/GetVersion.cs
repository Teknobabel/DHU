using UnityEngine;
using System.Collections;

public class GetVersion : MonoBehaviour {
	
	private UILabel
		m_versionLabel;

	// Use this for initialization
	void Start () {
		m_versionLabel = (UILabel)transform.GetComponent("UILabel");
		if (SettingsManager.m_settingsManager != null && m_versionLabel != null)
		{
			m_versionLabel.text = "Pre-Alpha " + SettingsManager.m_settingsManager.version;
		}
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}
}
