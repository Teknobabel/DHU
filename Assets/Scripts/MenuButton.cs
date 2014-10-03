using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour {
	
	public UIManager.MenuMode
		m_menuMode;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnClick () {
		
		if (UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
		{
			StartCoroutine(UIManager.m_uiManager.ChangeMenuMode(m_menuMode));
		}
	}
}
