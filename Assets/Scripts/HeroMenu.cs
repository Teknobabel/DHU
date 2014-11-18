using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroMenu : MonoBehaviour {

	public enum MenuState
	{
		Heroes,
		Cards,
	}

	[System.Serializable]
	public class HeroBadgeSlot
	{
		public List<HeroBadge>
			m_badgeSlot;
	}

	public static HeroMenu
		m_heroMenu;

	public GameObject
		m_settingsManager;

	public GameObject[]
		m_followerBank,
		m_miniPlacement,
		m_lights,
		m_cardsStart,
		m_cardsEnd,
		m_levelUpButtons;

	public Camera
		m_miniCam;

	public UICard
		m_heroCard;

	public List<HeroBadgeSlot>
		m_badgeTable = new List<HeroBadgeSlot>();

	private GameObject
		m_currentlyHoveredFollower = null,
		m_currentlySelectedFollower = null;

	private List<GameObject>
		m_cardList = new List<GameObject>();

	private List<Follower>
		m_followers = new List<Follower>();

	private MenuState
		m_menuState = MenuState.Heroes;

	// Use this for initialization
	void Awake () {
		m_heroMenu = this;
		if (SettingsManager.m_settingsManager == null)
		{
			Instantiate(m_settingsManager, Vector3.zero, Quaternion.identity);	
			SettingsManager.m_settingsManager.gameState = new GameState();
//			SettingsManager.m_settingsManager.gameState.InitializeSaveState();
//			SettingsManager.m_settingsManager.gameProgress = SettingsManager.m_settingsManager.gameState.loadState();
//			SettingsManager.m_settingsManager.gameState.saveState();
			SettingsManager.m_settingsManager.gameProgress = SettingsManager.m_settingsManager.gameState.loadState();
		}
	}

	void Start () {
	
		ClearMenu ();
		AssetManager.m_assetManager.m_props [1].gameObject.SetActive (true);
		UIManager.m_uiManager.UpdateGoldUI(); 
		int numObjects = m_followerBank.Length-1;
		
		for (int i=0; i <= numObjects; i++)
		{
			Follower f = (Follower)m_followerBank[i].GetComponent("Follower");

			//get follower progress
			GameState.ProgressState charProgress = SettingsManager.m_settingsManager.gameProgress[0];
			foreach (GameState.ProgressState thisCharState in SettingsManager.m_settingsManager.gameProgress)
			{
				if (thisCharState.m_followerType == f.m_followerType)
				{
					charProgress = thisCharState;
					break;
				}
			}
			if (!charProgress.m_isLocked)
			{

				//place model
				if (i < m_miniPlacement.Length)
				{
					GameObject miniPlace = m_miniPlacement[i];
					GameObject go = (GameObject)(Instantiate (f.gameObject, miniPlace.transform.position, miniPlace.transform.rotation));
					Follower fol = (Follower)go.GetComponent("Follower");
					m_followers.Add(fol);
					fol.currentXP = charProgress.m_XP;
					fol.currentLevel = charProgress.m_level;
					//fol.currentLevel = 4;
				

					int[] heroBadgeStates = new int[5];
					heroBadgeStates[0] = charProgress.m_badgeLevel1;
					heroBadgeStates[1] = charProgress.m_badgeLevel2;
					heroBadgeStates[2] = charProgress.m_badgeLevel3;
					heroBadgeStates[3] = charProgress.m_badgeLevel4;
					heroBadgeStates[4] = charProgress.m_badgeLevel5;
					//heroBadgeStates[5] = charProgress.m_badgeLevel6;
					fol.heroBadgeStates = heroBadgeStates;

					//fol.SetLevel();
					fol.id = i;
					
					go.transform.parent = miniPlace.transform;
				}
				
			} 
			else {
				//Color newColor = new Color(0.29f, 0.18f, 0.04f);
				Color newColor = new Color(0.25f, 0.25f, 0.25f);
				Renderer r = (Renderer)m_lights[i].GetComponent("Renderer");
				r.material.color = newColor;
			}
		}	
	}

	void Update () {



		if (Input.GetKeyUp(KeyCode.Escape))
		{
			SettingsManager.m_settingsManager.gameState.saveState();

			if (m_selectedFollower != null)
			{
				ClearMenu();
			} else {
				Application.LoadLevel("MainMenu01");
			}
		}

		if (Input.GetMouseButtonUp(0))
		{
			Ray miniTouchRay = m_miniCam.ScreenPointToRay(Input.mousePosition);
			RaycastHit miniHit;
			if(Physics.Raycast(miniTouchRay, out miniHit))
			{

				if (miniHit.transform.gameObject.tag == "Follower" && miniHit.transform.gameObject != m_currentlySelectedFollower && m_menuState == MenuState.Heroes )
				{
					m_currentlyHoveredFollower = miniHit.transform.gameObject;
					Follower f = (Follower)miniHit.transform.GetComponent("Follower");
					Hover(f);

//					if (m_currentlySelectedFollower != null)
//					{
//						//unselect hero
//						Follower sF = (Follower) m_currentlySelectedFollower.transform.GetComponent("Follower");
//						sF.transform.localScale = Vector3.one;
//					}
//
//					m_currentlySelectedFollower = hit.transform.gameObject;
//					Follower f = (Follower) hit.transform.GetComponent("Follower");
//					f.transform.localScale = Vector3.one * 1.35f;
//					m_miniPlacement[f.id].animation["ModelSelect01"].speed = 1.25f;
//					m_miniPlacement[f.id].animation.Play("ModelSelect01");
				}
			}

			Ray cardTouchRay = UIManager.m_uiManager.m_uiCamera.camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(cardTouchRay, out hit))
			{
				if (hit.transform.gameObject.tag == "BackButton" )
				{
					if (m_selectedFollower != null)
					{
						ClearMenu();
					} else {
						Application.LoadLevel("MainMenu01");
					}
				} else if (hit.transform.gameObject.tag == "SkipButton" )
				{
					//load next hero, wrap back to first if currently at last
					Follower nextFollower = null;
					if (m_followers.Count > 1 && m_selectedFollower != null)
					{
						for (int i=0; i < m_followers.Count; i++)
						{
							Follower f = (Follower)m_followers[i];
							//Debug.Log( f + " / " + m_selectedFollower);
							if ( f == m_selectedFollower)
							{
//								Debug.Log(i.ToString() + " al;ksdf;lskjdf");
								if (i+1 < m_followers.Count)
								{
									nextFollower = m_followers[i+1];
								} else {
									nextFollower = m_followers[0];
								}
							}
						}
					}

					ClearMenu();

					if (nextFollower != null)
					{
						Hover(nextFollower);
					}



				}

				int price = -1;
				if (hit.transform.gameObject.tag == "Chapter2")
				{
					price = 25;
				} else if (hit.transform.gameObject.tag == "Chapter3")
				{
					price = 50;
				} else if (hit.transform.gameObject.tag == "Chapter4")
				{
					price = 100;
				} else if (hit.transform.gameObject.tag == "Chapter5")
				{
					price = 200;
				} else if (hit.transform.gameObject.tag == "Chapter6")
				{
					price = 300;
				} else if (hit.transform.gameObject.tag == "Chapter7")
				{
					price = 400;
				}

				if (price > 0 && SettingsManager.m_settingsManager.xp >= price)
				{
					if (m_selectedFollower != null)
					{
//						Debug.Log (price);
						SettingsManager.m_settingsManager.xp -= price;
						UIManager.m_uiManager.UpdateGoldUI(); 
						//Follower f = (Follower)m_.transform.GetComponent("Follower");
						BuyLevel(m_selectedFollower);
					}
				}
			}
		}

		DoHoveredCards ();

		//check for mouse hovering over hero card
		if (!Input.GetMouseButton(0))
		{
			Ray cardTouchRay = m_miniCam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(cardTouchRay, out hit))
			{
				if (hit.transform.gameObject.tag == "Follower" && hit.transform.gameObject != m_currentlyHoveredFollower && m_menuState == MenuState.Heroes )
				{
					m_currentlyHoveredFollower = hit.transform.gameObject;
					Follower f = (Follower)hit.transform.GetComponent("Follower");
					m_miniPlacement[f.id].animation.Play();
//					Hover(f);
				}
			} 
			else if (m_currentlyHoveredFollower != null)        
			{
//				if  (m_currentlySelectedFollower != null)
//				{
//					Follower f = (Follower) m_currentlySelectedFollower.transform.GetComponent("Follower");
					m_currentlyHoveredFollower = null;
//					Hover(f);
//				} else {ClearMenu();}
			}
		}

	}

	private void BuyLevel (Follower f)
	{
		Debug.Log ("BUYING LEVEL");
		f.currentLevel ++;
		ClearMenu ();
		Hover (f);

		//update progress state
		for (int i=0; i < SettingsManager.m_settingsManager.gameProgress.Count; i++)
		//foreach (GameState.ProgressState thisF in SettingsManager.m_settingsManager.gameProgress) 
		{
			GameState.ProgressState thisF = (GameState.ProgressState)SettingsManager.m_settingsManager.gameProgress[i];
			if (thisF.m_followerType == f.m_followerType)
			{

				thisF.m_level = f.currentLevel;
				SettingsManager.m_settingsManager.gameProgress[i] = thisF;
				break;
			}
		}


		SettingsManager.m_settingsManager.gameState.saveState();
	}

	private void DoHoveredCards()
	{

		Ray cardTouchRay = UIManager.m_uiManager.m_uiCamera.camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(cardTouchRay, out hit))
		{
			if (hit.transform.gameObject.tag == "InvCard")
			{
				UICard c = (UICard)hit.transform.GetComponent("UICard");
				if (c !=  UIManager.m_uiManager.invHoveredCard)
				{

					UIManager.m_uiManager.InventoryCardHovered(hit.transform.gameObject);
				}
			}
		} else
		{
			if (UIManager.m_uiManager.invHoveredCard != null)
			{
				UIManager.m_uiManager.ClearInvSelection();	
			}

		}
	}

	private void ClearMenu ()
	{
		m_menuState = MenuState.Heroes;

		m_currentlyHoveredFollower = null;
		m_selectedFollower = null;
		m_miniCam.gameObject.SetActive (true);
		UIManager.m_uiManager.m_inventoryUI.SetActive (true);
		m_heroCard.m_nameUI.gameObject.SetActive (false);
		m_heroCard.m_abilityUI.gameObject.SetActive (false);
		m_heroCard.m_rankUI.gameObject.SetActive (false);
		m_heroCard.m_miscOBJ[0].gameObject.SetActive(false);
		m_heroCard.m_healthIcon.gameObject.SetActive (false);
		m_heroCard.m_portrait.spriteName = "Card_Back03";
		m_heroCard.m_passive01UI.gameObject.SetActive (false);
		m_heroCard.m_passive02UI.gameObject.SetActive (false);
		m_heroCard.m_miscText[0].text = "XP: ";

//		foreach (HeroBadgeSlot h in m_badgeTable)
//		{
//			foreach (HeroBadge s in h.m_badgeSlot)
//			{
//				s.ClearBadge();
//			}
//		}

		foreach (GameObject go in m_cardList) {
			Destroy(go);
		}

		m_cardList.Clear ();
//		AssetManager.m_assetManager.m_props [1].gameObject.SetActive (true);
//		AssetManager.m_assetManager.m_props [2].gameObject.SetActive (true);
		AssetManager.m_assetManager.m_props [3].gameObject.SetActive (false);
		AssetManager.m_assetManager.m_props [0].gameObject.SetActive (false);
		UIManager.m_uiManager.m_inventoryUI.SetActive (false);
	}

	private Follower m_selectedFollower = null;

	private void Hover (Follower f)
	{
		m_menuState = MenuState.Cards;

		m_selectedFollower = f;
		m_miniCam.gameObject.SetActive (false);
		UIManager.m_uiManager.m_inventoryUI.SetActive (true);

		//m_heroCard.m_nameUI.text = f.m_nameText;
		m_heroCard.m_nameText.Text = f.m_nameText;
		m_heroCard.m_portrait.spriteName = f.m_portraitSpriteName;
		//m_heroCard.m_nameUI.gameObject.SetActive (true);
		AssetManager.m_assetManager.m_props [0].gameObject.SetActive (true);
		AssetManager.m_assetManager.m_props [1].gameObject.SetActive (false);
		AssetManager.m_assetManager.m_props [3].gameObject.SetActive (true);
//		AssetManager.m_assetManager.m_props [2].gameObject.SetActive (false);


		//display a purchase button only for the next level
		for (int l=0; l < m_levelUpButtons.Length; l ++) { 
			if (l != f.currentLevel-1)
			{
				m_levelUpButtons[l].SetActive(false);
			} else{
				m_levelUpButtons[l].SetActive(true);
			}
		}

		// spawn cards
		for (int j=0; j < m_cardsStart.Length; j ++) {
			GameObject[] items = f.m_deck[j].m_levelCards;
			Vector3 startPos = m_cardsStart[j].transform.position;
			Vector3 endPos = m_cardsEnd[j].transform.position;
			int numCards = Mathf.Clamp( items.Length-1, 1, 99);

			for (int i=0; i < items.Length; i++)
			{
				Item thisItem = (Item)items[i].GetComponent("Item");
				Vector3 cardPos = Vector3.Lerp(startPos, endPos, ((float)i) / ((float)numCards));
				GameObject fCard = (GameObject)Instantiate(UIManager.m_uiManager.m_cardSmall, cardPos, UIManager.m_uiManager.m_cardSmall.transform.rotation);
				fCard.transform.parent = UIManager.m_uiManager.m_inventoryUI.transform;
				m_cardList.Add(fCard);
				UICard cardUI = (UICard)fCard.GetComponent("UICard");

				if (j >= f.currentLevel)
				{
					//show card back
					cardUI.m_portrait.spriteName = "Card_Back03";
				} else {

					//display cards
	//				thisItem.card = cardUI;
					cardUI.itemData = thisItem;
					fCard.name = " " + thisItem.m_name;
					cardUI.m_nameUI.text = thisItem.m_name;
					cardUI.m_abilityUI.text = thisItem.m_description;
					cardUI.m_rankUI.text = thisItem.m_class.ToString();
					cardUI.m_portrait.spriteName = thisItem.m_portraitSpriteName;
					
					cardUI.m_shortCutUI.gameObject.SetActive(false);
					cardUI.m_abilityUI.gameObject.SetActive(true);
					cardUI.m_nameUI.gameObject.SetActive(true);
					cardUI.m_rankUI.gameObject.SetActive(true);
					
					cardUI.m_nameUI.depth += i+i;
					cardUI.m_abilityUI.depth += i+i;
					cardUI.m_shortCutUI.depth += i+i;
					cardUI.m_portrait.depth += i+i;
					cardUI.m_rankUI.depth += i+i;
					cardUI.m_healthUI.depth += i+i;
					cardUI.m_healthIcon.depth += i+i;
					cardUI.m_damageIcon.depth += i+i;
					
					foreach (UISprite s in cardUI.m_miscSprite)
					{
						s.depth += i+i;
					}

					for (int k=0; k < thisItem.m_graveBonus.Length; k++)
					{
						Item.GraveBonus gb = (Item.GraveBonus)thisItem.m_graveBonus[k];
						UISprite s = (UISprite)cardUI.m_miscSprite[k];
						
						if (gb == Item.GraveBonus.Attack)
						{
							s.spriteName = "Icon_Attack";
						} else if (gb == Item.GraveBonus.Health)
						{
							s.spriteName = "Icon_Health";
						} else if (gb == Item.GraveBonus.Energy)
						{
							s.spriteName = "Icon_Energy";
						} else if (gb == Item.GraveBonus.Armor)
						{
							s.spriteName = "Icon_Armor";
						} else if (gb == Item.GraveBonus.Actions)
						{
							s.spriteName = "Icon_Stopwatch";
						}
						s.depth += i+i;
						s.gameObject.SetActive(true);
					}

					cardUI.m_healthUI.text = thisItem.m_energyCost.ToString();
					cardUI.m_damageIcon.spriteName = "Icon_Energy";
					cardUI.m_healthUI.gameObject.SetActive(true);
					cardUI.m_damageIcon.gameObject.SetActive(true);

					if (thisItem.m_healthCost > 0)
					{
						cardUI.m_healthUI.text = thisItem.m_healthCost.ToString();
						cardUI.m_damageIcon.spriteName = "Icon_Health";
						cardUI.m_healthUI.gameObject.SetActive(true);
						cardUI.m_damageIcon.gameObject.SetActive(true);
					} 
					cardUI.m_miscOBJ[0].gameObject.SetActive(true);
				}
			}
		}
	}

	public Follower selectedFollower {
	get {
			if (m_currentlySelectedFollower != null)
			{
				Follower f = (Follower)m_currentlySelectedFollower.GetComponent("Follower");
			return f;
			} else { return null;}
		}
	}
}
