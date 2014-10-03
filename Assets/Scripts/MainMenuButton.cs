using UnityEngine;
using System.Collections;

public class MainMenuButton : MonoBehaviour {

	public bool m_exit = false;

	public bool m_changeMenu = false;
	public UIManager.MenuMode m_menuMode;

	public bool m_changeScene = false;
	public string m_sceneName = "None";

	public UISlicedSprite m_buttonSprite;
	public UILabel m_label;

	private Color m_startColor = Color.white;


	// Use this for initialization
	void Start () {
		if (m_label != null)
		{
			m_startColor = m_label.color;
		}
	}
	
	// Update is called once per frame
	void OnClick () {

		if (m_exit)
		{
			if (MainMenu.m_mainMenu.badgeStatesChanged)
			{
				MainMenu.m_mainMenu.badgeStatesChanged = false;
				SettingsManager.m_settingsManager.gameState.saveState();
			}

			Application.Quit();
		}
		else if (m_changeMenu)
		{
			MainMenu.m_mainMenu.ChangeMenu(m_menuMode);
		} else if (m_changeScene && m_sceneName != "None")
		{
			if (MainMenu.m_mainMenu != null)
			{
				if (MainMenu.m_mainMenu.badgeStatesChanged)
				{
					MainMenu.m_mainMenu.badgeStatesChanged = false;
					SettingsManager.m_settingsManager.gameState.saveState();
				}

				if (SettingsManager.m_settingsManager.demo && m_sceneName == "GameScene01")
				{
					//check for shortcut availabililty

					if (SettingsManager.m_settingsManager.shortcutStates.Count > 0)
					{
						for (int i=0; i < SettingsManager.m_settingsManager.shortcutStates.Count; i++)
						{
							int sc = (int)SettingsManager.m_settingsManager.shortcutStates[i];
							if (sc == 1 && i != 0)
							{
								//load shortcut menu
								MainMenu.m_mainMenu.ChangeMenu(UIManager.MenuMode.ChapterSelect);
								return;
							}
						}
					}
					m_sceneName = "GameScene01";
				} else if (!SettingsManager.m_settingsManager.demo && m_sceneName == "GameScene01")
				{
					if (SettingsManager.m_settingsManager.shortcutStates.Count > 0)
					{
						for (int i=0; i < SettingsManager.m_settingsManager.shortcutStates.Count; i++)
						{
							int sc = (int)SettingsManager.m_settingsManager.shortcutStates[i];
							if (sc == 1 && i != 0)
							{
								//load shortcut menu
								MainMenu.m_mainMenu.ChangeMenu(UIManager.MenuMode.ChapterSelect);
								return;
							}
						}
					}
					m_sceneName = "PartySelect01";
				}
			}

			if (HeroMenu.m_heroMenu != null)
			{
				SettingsManager.m_settingsManager.gameState.saveState();
			}

			Application.LoadLevel(m_sceneName);
		}
	
	}

	public void ChangeState (bool on)
	{
		if (on)
		{
			m_buttonSprite.spriteName = "Button_Yellow01";
			m_buttonSprite.gameObject.SetActive(true);
			if (m_label != null)
			{
				m_label.color = Color.black;
			}
		} else {
			m_buttonSprite.gameObject.SetActive(false);
			if (m_label != null)
			{
				m_label.color = m_startColor;
			}
		}
	}
}
