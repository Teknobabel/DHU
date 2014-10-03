using UnityEngine;
using System.Collections;

public class TrialButton : MonoBehaviour {

	public int m_TrialNum = 0;

	public UILabel
		m_prizeText;

	// Use this for initialization
	void Start () {
	
	}

	void OnClick () {
		SettingsManager.m_settingsManager.trial = true;
		SettingsManager.m_settingsManager.difficultyLevel = m_TrialNum;
		Application.LoadLevel ("PartySelect01");
		}
	// Update is called once per frame
//	void Update () {
//	
//	}
}
