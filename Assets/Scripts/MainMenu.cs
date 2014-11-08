using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {

	public static MainMenu
		m_mainMenu;
	
//	public string 
//		m_levelName = "NONE";
//	
//	public bool
//		m_exitApp = false;

	public GameObject[]
		m_menus,
		m_chapters;
//		m_storeBadges;

	public StoreBadge[]
		m_storeBadges,
		m_badges,
		m_badgeSlots;

	public UILabel[]
		m_labels;

	public TypogenicText[]
		m_typogenicText;
		

	public MainMenuButton[]
		m_buttons;

	public TrialButton[]
		m_trialButtons;
	
	public GameObject
		m_settingsManager;

	public bool
		m_Demo = false,
		m_deleteSave = false,
		m_resetIfDiffVersion = false;

	public Camera
		m_uiCamera;

	private UIManager.MenuMode
		m_menuMode = UIManager.MenuMode.MainMenu;

	private bool
		m_badgeStatesChanged = false;

	private int
		m_maxBadges = 3,
		m_currentBadges = 0;

	// Use this for initialization
	void Awake () {


		Application.targetFrameRate = 60;
		m_mainMenu = this;
		if (SettingsManager.m_settingsManager == null && m_settingsManager != null)
		{
			Instantiate(m_settingsManager, Vector3.zero, Quaternion.identity);	
		}

		if (m_deleteSave)
		{
			PlayerPrefs.DeleteAll();
		}


		if (!PlayerPrefs.HasKey("DoTutorial"))
	    {
			PlayerPrefs.SetInt("DoTutorial", 0);
			PlayerPrefs.Save();
		}

		// load save state for badge states
		SettingsManager.m_settingsManager.gameState = new GameState();
//		Debug.Log (SettingsManager.m_settingsManager.gameState.saveStateExists ());
		if (m_Demo || (m_resetIfDiffVersion && PlayerPrefs.GetString("Version") != SettingsManager.m_settingsManager.version) || (!SettingsManager.m_settingsManager.gameState.saveStateExists() && !m_deleteSave))
		{
			Debug.Log("INITIALIZING DEMO STATE");
			SettingsManager.m_settingsManager.demo = true;

			// remove current save
			if (SettingsManager.m_settingsManager.gameState.saveStateExists())
			{
				SettingsManager.m_settingsManager.gameState.clearState();
			}
			SettingsManager.m_settingsManager.gameState.InitializeSaveState();
			SettingsManager.m_settingsManager.gameProgress = SettingsManager.m_settingsManager.gameState.loadState();

			PlayerPrefs.SetInt("DoTutorial", 1);
			PlayerPrefs.SetString("Version", SettingsManager.m_settingsManager.version);
			PlayerPrefs.Save();

			// unlock specified badges
//			for (int i=0; i < SettingsManager.m_settingsManager.badgeStates.Count; i++)
//			{
//
//			}

			//debug unlocking
			SettingsManager.m_settingsManager.badgeStates[2] = 1;

			// unlock specified heroes
//			foreach (GameState.ProgressState thisCharState in SettingsManager.m_settingsManager.gameProgress)
			for (int i=0; i < SettingsManager.m_settingsManager.gameProgress.Count; i++)
			{
				GameState.ProgressState thisCharState = (GameState.ProgressState)SettingsManager.m_settingsManager.gameProgress[i];
				if (thisCharState.m_followerType == Follower.FollowerType.August || thisCharState.m_followerType == Follower.FollowerType.Telina || thisCharState.m_followerType == Follower.FollowerType.Jin
				    || thisCharState.m_followerType == Follower.FollowerType.Brand)
				{
					// set heroes progress state
					thisCharState.m_isLocked = false;
					thisCharState.m_level = 3;
					SettingsManager.m_settingsManager.gameProgress[i] = thisCharState;
				}
			}

			// save new state
			SettingsManager.m_settingsManager.gameState.saveState();

		} else if (!SettingsManager.m_settingsManager.gameState.saveStateExists())
		{
			Debug.Log("NO SAVE STATE DETECTED, INITIALIZING NEW SAVE");
			//create initial save	
			SettingsManager.m_settingsManager.gameState.InitializeSaveState();
			SettingsManager.m_settingsManager.gameProgress = SettingsManager.m_settingsManager.gameState.loadState();
		} else { 

			//Debug.Log("LOADING SAVE STATE");
			SettingsManager.m_settingsManager.gameProgress = SettingsManager.m_settingsManager.gameState.loadState(); }

//			if (m_resetIfDiffVersion && PlayerPrefs.GetString("Version") != SettingsManager.m_settingsManager.version)
//			{
//				SettingsManager.m_settingsManager.gameState.clearState();
//				PlayerPrefs.SetString("Version", SettingsManager.m_settingsManager.version);
//				PlayerPrefs.Save();
//			} else if (!m_resetIfDiffVersion && PlayerPrefs.GetString("Version") != SettingsManager.m_settingsManager.version)
//			{
//				PlayerPrefs.SetString("Version", SettingsManager.m_settingsManager.version);
//				PlayerPrefs.Save();	
//			}


//		m_labels [0].text = SettingsManager.m_settingsManager.gold.ToString ();
		//SettingsManager.m_settingsManager.xp += 1000;
		m_typogenicText[0].Text = SettingsManager.m_settingsManager.xp.ToString ("D4");

		//initialize trial buttons
//		foreach (TrialButton tb in m_trialButtons)
//		{
//			int state = SettingsManager.m_settingsManager.trialStates[tb.m_TrialNum];
//			if (state == 1)
//			{
//				tb.m_prizeText.color = Color.red;
//				tb.m_prizeText.text = "COMPLETED";
//			}
//		}

		//initialize shop badges
		for (int i=0; i < SettingsManager.m_settingsManager.badgeStates.Count; i++)
		{
			int price = -1;
			switch (i)
			{
			case 1: // Damage boost
				price = 50;
				break;
			case 2: // Damage boost
				price = 100;
				break;
			case 3: // Damage boost
				price = 200;
				break;
			case 4: // Damage boost
				price = 500;
				break;
			case 5: // Health boost
				price = 75;
				break;
			case 6: // Health boost
				price = 125;
				break;
			case 7: // Health boost
				price = 250;
				break;
			case 8: // Health boost
				price = 300;
				break;
			case 9: // Energy boost
				price = 30;
				break;
			case 10: // Energy boost
				price = 80;
				break;
			case 11: // Energy boost
				price = 160;
				break;
			case 12: // Energy boost
				price = 300;
				break;
			case 13: // Hand Size Boost
				price = 100;
				break;
			case 14: // Hand Size Boost
				price = 300;
				break;
			case 15: // Health Recovery +1
				price = 150;
				break;
			case 16: // Energy Recovery +1
				price = 150;
				break;
			case 17: // Energy Recovery +1
				price = 250;
				break;
			case 18: // Badge Slot
				price = 100;
				break;
			case 19: // Badge Slot
				price = 200;
				break;
			case 20: // Badge Slot
				price = 300;
				break;
			case 21: // Badge Slot
				price = 400;
				break;
			case 22: // Badge Slot
				price = 500;
				break;
			}

			if (SettingsManager.m_settingsManager.badgeStates[i] == 0)
			{
				m_storeBadges[i].Initialize(price, StoreBadge.State.NotOwned, i);

				if (i < m_badges.Length) {
					m_badges[i].Initialize(-1, StoreBadge.State.Locked, i);
				}
			} else
			{
				m_storeBadges[i].Initialize(price, StoreBadge.State.Owned, i);

				if (i > 17)
				{
					m_maxBadges ++;
				}

				if (SettingsManager.m_settingsManager.badgeStates[i] == 1)
				{
					if (i < m_badges.Length) {
						m_badges[i].Initialize(-1, StoreBadge.State.UnEquipped, i);
					}
				} else if (SettingsManager.m_settingsManager.badgeStates[i] == 2)
				{
					if (i < m_badges.Length) {
						m_badges[i].Initialize(-1, StoreBadge.State.Equipped, i);
						m_currentBadges ++;

						// update active badges UI
						foreach (StoreBadge b in m_badgeSlots)
						{
							if (b.gameObject.activeSelf && b.m_sprite.spriteName == "BadgeSlot01")
							{
								b.Initialize(-1, StoreBadge.State.BadgeSlot, -1);
								b.SetBadge(m_badges[i]);
								break;
							}
						}

					}
				} 
			}
		}

		for (int i=0; i < m_badgeSlots.Length; i++)
		{
			if (i >= m_maxBadges)
			{
				m_badgeSlots[i].gameObject.SetActive(false);
			} else if (!m_badgeSlots[i].m_priceBG.gameObject.activeSelf)
			{
				m_badgeSlots[i].Initialize(-1, StoreBadge.State.BadgeSlot, -1);
				m_badgeSlots[i].ClearBadge();
			}
		}

		MainMenu.m_mainMenu.m_labels [1].text = "BADGES CURRENTLY IN USE: " + m_currentBadges + "/" + m_maxBadges.ToString();
	}

	void Update ()
	{
		if (Input.GetMouseButtonUp (0)) {

			// check for chapter click
			Ray cardTouchRay = m_uiCamera.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (cardTouchRay, out hit)) {
				if (hit.transform.gameObject.tag == "Chapter1" && m_menuMode == UIManager.MenuMode.ChapterSelect) {
					SettingsManager.m_settingsManager.startChapter = 1;
					if (SettingsManager.m_settingsManager.demo) {
						Application.LoadLevel ("GameScene01");
					} else {
						Application.LoadLevel ("PartySelect01");
					}
				} else if (hit.transform.gameObject.tag == "Chapter2" && m_menuMode == UIManager.MenuMode.ChapterSelect) {
						SettingsManager.m_settingsManager.startChapter = 2;
						if (SettingsManager.m_settingsManager.demo) {
							Application.LoadLevel ("GameScene01");
						} else {
							Application.LoadLevel ("PartySelect01");
						}
					} else if (hit.transform.gameObject.tag == "Chapter3" && m_menuMode == UIManager.MenuMode.ChapterSelect) {
							SettingsManager.m_settingsManager.startChapter = 3;
							if (SettingsManager.m_settingsManager.demo) {
								Application.LoadLevel ("GameScene01");
							} else {
								Application.LoadLevel ("PartySelect01");
							}
						} else if (hit.transform.gameObject.tag == "Chapter4" && m_menuMode == UIManager.MenuMode.ChapterSelect) {
								SettingsManager.m_settingsManager.startChapter = 4;
								if (SettingsManager.m_settingsManager.demo) {
									Application.LoadLevel ("GameScene01");
								} else {
									Application.LoadLevel ("PartySelect01");
								}
							} else if (hit.transform.gameObject.tag == "Chapter5" && m_menuMode == UIManager.MenuMode.ChapterSelect) {
									SettingsManager.m_settingsManager.startChapter = 5;
									if (SettingsManager.m_settingsManager.demo) {
										Application.LoadLevel ("GameScene01");
									} else {
										Application.LoadLevel ("PartySelect01");
									}
								} else if (hit.transform.gameObject.tag == "BackButton" && (m_menuMode == UIManager.MenuMode.BadgeStore || m_menuMode == UIManager.MenuMode.ChapterSelect)) {
										ChangeMenu (UIManager.MenuMode.MainMenu);
									}
			}
		} else if (Input.GetKeyUp (KeyCode.Escape) && m_menuMode != UIManager.MenuMode.MainMenu) {
			ChangeMenu (UIManager.MenuMode.MainMenu);
		}
	}

	public void ChangeMenu (UIManager.MenuMode newMode)
	{
		UIManager.MenuMode oldMode = m_menuMode;

		if (m_badgeStatesChanged)
		{
			Debug.Log("BADGE CHANGED STATE");
			m_badgeStatesChanged = false;
			SettingsManager.m_settingsManager.gameState.saveState();
		}

		switch (newMode)
		{
		case UIManager.MenuMode.Badges:
			if (oldMode == newMode)
			{
				m_menus[1].SetActive(false);
				m_menus[3].SetActive(false);
				m_buttons[0].ChangeState(false);
				m_buttons[1].ChangeState(false);
				m_buttons[2].ChangeState(false);
				newMode = UIManager.MenuMode.MainMenu;
			} else {
				m_menus[1].SetActive(true);
				m_menus[2].SetActive(false);
				m_menus[3].SetActive(false);
				m_buttons[0].ChangeState(false);
				m_buttons[1].ChangeState(true);
				m_buttons[2].ChangeState(false);
				m_menus[4].SetActive(false);
				m_buttons[3].ChangeState(false);
				//m_menus[0].SetActive(false);
			}
			break;
		case UIManager.MenuMode.BadgeStore:
			if (oldMode == newMode)
			{
				m_menus[2].SetActive(false);
				m_menus[3].SetActive(false);
				m_buttons[0].ChangeState(false);
				m_buttons[1].ChangeState(false);
				m_buttons[2].ChangeState(false);
				newMode = UIManager.MenuMode.MainMenu;
			} else {
				m_menus[2].SetActive(true);
				m_menus[1].SetActive(false);
				m_menus[3].SetActive(false);
				m_menus[4].SetActive(false);
				m_menus[0].SetActive(false);
//				m_buttons[1].ChangeState(false);
//				m_buttons[0].ChangeState(true);
//				m_buttons[2].ChangeState(false);
//				m_buttons[3].ChangeState(false);
			}
			break;
		case UIManager.MenuMode.MainMenu:
			m_menus[0].SetActive(true);
			m_menus[2].SetActive(false);
			m_menus[1].SetActive(false);
			m_menus[3].SetActive(false);
			m_menus[4].SetActive(false);
			break;
		case UIManager.MenuMode.Trials:
			if (oldMode == newMode)
			{
				m_menus[3].SetActive(false);
				m_buttons[0].ChangeState(false);
				m_buttons[1].ChangeState(false);
				m_buttons[2].ChangeState(false);
				newMode = UIManager.MenuMode.MainMenu;
			} else {
				m_menus[3].SetActive(true);
				m_menus[2].SetActive(false);
				m_menus[1].SetActive(false);
				m_buttons[0].ChangeState(false);
				m_buttons[1].ChangeState(false);
				m_buttons[2].ChangeState(true);
			}
			break;
		case UIManager.MenuMode.ChapterSelect:
			if (oldMode == newMode)
			{
				m_menus[4].SetActive(false);
				m_buttons[3].ChangeState(false);
				newMode = UIManager.MenuMode.MainMenu;
			} else {

				m_menus[4].SetActive(true);
				m_menus[1].SetActive(false);
				m_menus[2].SetActive(false);
				m_menus[0].SetActive(false);
//				m_buttons[0].ChangeState(false);
//				m_buttons[1].ChangeState(false);
//				m_buttons[2].ChangeState(false);
//				m_buttons[3].ChangeState(true);


				for (int i=0; i < SettingsManager.m_settingsManager.shortcutStates.Count; i++)
				{
					if (SettingsManager.m_settingsManager.shortcutStates[i] == 0)
					{
						GameObject t = (GameObject) m_chapters[i];
						t.gameObject.SetActive(false);
//						Color c = new Color(0.1f, 0.1f, 0.1f, 1.0f);
//						t.ColorTopLeft = c;
//						t.ColorTopRight = c;
//						t.ColorBottomLeft = c;
//						t.ColorBottomRight = c;
//						BoxCollider bc = (BoxCollider)t.GetComponent("BoxCollider");
//						bc.enabled = false;
					}
				}

				}
			break;
		}

		m_menuMode = newMode;
	}

	public bool badgeStatesChanged {get{return m_badgeStatesChanged;} set{m_badgeStatesChanged = value;}}
	public UIManager.MenuMode menuMode {get{return m_menuMode;}}
	public int maxBadges {get{return m_maxBadges;} set{m_maxBadges = value;}}
	public int currentBadges {get{return m_currentBadges;} set{m_currentBadges = value;}}
}
