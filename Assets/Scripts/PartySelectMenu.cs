using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartySelectMenu: MonoBehaviour {

	public static PartySelectMenu
		m_partySelectMenu;

	public class PartySlot {
		public enum State
		{
			Empty,
			Occupied,
		}

		public State m_followerState = State.Empty;
		public State m_badgeState = State.Empty;
		public GameObject m_badge = null;
		public Follower m_follower = null;
		public StoreBadge m_badgePortrait = null;
		public UICard m_portrait = null;

	}
	
	public GameObject 
		m_card,
		m_charCard,
		m_settingsManager;
	
	public Camera
		m_UICam,
		m_miniCam;
	
	public GameObject[]
		m_followerBank,
		m_miniPlacement,
		m_bonusLabels,
		m_lights,
		m_decksStart,
		m_decksEnd;

	public StoreBadge[]
		m_storeBadges,
		m_badgeSlots;
	
	public GameObject
		m_selectFX,
		m_highlight;
	
	public bool
		m_deleteSave = false,
		m_resetIfDiffVersion = false;
	
	private UICard
		m_openCard,
		m_scaledCard;
	
	private float
		m_unselectedScale = 0.5f,
		m_selectedScale = 0.65f;
	
	private Vector3
		m_scaledcardPos = Vector3.zero;
	
	private List<Follower>
		m_followers = new List<Follower>();
	
	private List<UICard>
		m_unlockedHeroCards = new List<UICard>();

	private List<UICard>
		m_cardList = new List<UICard>();

	private List<List<GameObject>>
		m_deck = new List<List<GameObject>>();

	private List<PartySlot>
		m_partySlots = new List<PartySlot>();

	private int
		m_startingDamage = 0,
		m_startingHealth = 0,
		m_startingEnergy = 0,
		m_startingArmor = 0,
		m_maxPartySize = 4,
		m_currentBadges = 0;
	
	private Color 
		spentPortraitColor = new Color(0.4f, 0.4f, 0.4f, 1),
		m_enabledTextTop = Color.blue,
		m_enabledTextBottom = Color.blue,
		m_disabledTextColor = new Color(0.2f, 0.2f, 0.2f, 1);

	void Awake () {

		m_partySelectMenu = this;

		if (SettingsManager.m_settingsManager == null)
		{
			Instantiate(m_settingsManager, Vector3.zero, Quaternion.identity);	
			SettingsManager.m_settingsManager.gameState = new GameState();
			if (!SettingsManager.m_settingsManager.gameState.saveStateExists())
			{
				//create initial save	
				SettingsManager.m_settingsManager.gameState.InitializeSaveState();
			} else {
				SettingsManager.m_settingsManager.gameProgress = SettingsManager.m_settingsManager.gameState.loadState();
				}
		}


	}
	// Use this for initialization
	void Start () {

//		if (m_deleteSave)
//		{
//			SettingsManager.m_settingsManager.gameState.clearState();
//		}
//		
//		if (!PlayerPrefs.HasKey("Version"))
//		{
//			PlayerPrefs.SetString("Version", SettingsManager.m_settingsManager.version);
//			PlayerPrefs.Save();
//		}
//				
//		if (m_resetIfDiffVersion && PlayerPrefs.GetString("Version") != SettingsManager.m_settingsManager.version)
//		{
//			SettingsManager.m_settingsManager.gameState.clearState();
//			
//			PlayerPrefs.SetString("Version", SettingsManager.m_settingsManager.version);
//			PlayerPrefs.Save();
//		} else if (!m_resetIfDiffVersion && PlayerPrefs.GetString("Version") != SettingsManager.m_settingsManager.version)
//		{
//			PlayerPrefs.SetString("Version", SettingsManager.m_settingsManager.version);
//			PlayerPrefs.Save();	
//		}
		
		
//		m_startingDamage = SettingsManager.m_settingsManager.startingDamage;
//		m_startingHealth = SettingsManager.m_settingsManager.startingHealth;
//		m_startingEnergy = SettingsManager.m_settingsManager.startingEnergy;
//		m_startingArmor = SettingsManager.m_settingsManager.startingArmor;

//		if (!SettingsManager.m_settingsManager.gameState.saveStateExists())
//		{
//			//create initial save	
//			SettingsManager.m_settingsManager.gameState.InitializeSaveState();
//		}

		UIManager.m_uiManager.UpdateGoldUI ();
		
		// set up party cards
//		if (SettingsManager.m_settingsManager.badgeStates[3] == 2)
//		{
//			m_maxPartySize ++;
//		}

		for (int j=0; j < SettingsManager.m_settingsManager.badgeStates.Count; j++) {
			if (SettingsManager.m_settingsManager.badgeStates[j] == 2)
			{
				SettingsManager.m_settingsManager.badgeStates[j] = 1;
			}
		}
//
//		if (m_maxPartySize < 4)
//		{
//			Vector3 pos = UIManager.m_uiManager.m_levelEndUI.transform.localPosition;
//			pos.x = 466.1777f;
//			UIManager.m_uiManager.m_levelEndUI.transform.localPosition = pos;
//		}

		for (int j=0; j < PartyCards.m_partyCards.m_party.Length; j++)
		{
			UICard thisCard = PartyCards.m_partyCards.m_party[j];

			if (j < m_maxPartySize)
			{
				//thisCard.m_portrait.spriteName = "Card_Empty";
				thisCard.m_portrait.spriteName = "Token_Empty";
				thisCard.m_nameText.gameObject.SetActive(false);
				thisCard.m_miscOBJ[1].gameObject.SetActive(false);
			} else {
				thisCard.m_portrait.gameObject.SetActive(false);
				thisCard.m_shortCutUI.gameObject.SetActive(false);
			}
		}
		
		//test circle spawning
//		Vector3 center = Vector3.one * 200;
//		center.y = 2.5f;
//		float radiusA = 3;
//		float radiusB = 3;
//		float angle = 0;
		int numObjects = m_followerBank.Length-1;
		
		for (int i=0; i <= numObjects; i++)
		{
//			angle = Mathf.Lerp(210, 150, ((float)i) / ((float)numObjects));
//			Vector3 pos = Vector3.zero;
//			pos.x = center.x + radiusB * Mathf.Sin(angle * Mathf.Deg2Rad);
//			pos.y = center.y + radiusA * Mathf.Cos(angle * Mathf.Deg2Rad);
//			pos.z = center.z + (i * 0.01f);
//			GameObject GO = (GameObject)Instantiate(m_charCard, pos, m_charCard.transform.rotation);
//			Vector3 scale = GO.transform.localScale;
//			scale *= m_unselectedScale;
//			GO.transform.localScale = scale;
//			GO.transform.LookAt(center, Vector3.forward);
//			
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
//			
//			UICard card = (UICard)GO.transform.GetComponent("UICard");
			//if (!charProgress.m_isLocked || charProgress.m_isLocked)
			if (!charProgress.m_isLocked)
			{
//				card.selectState = UICard.SelectState.Unselected;
//				card.m_nameUI.gameObject.SetActive(false);
//				card.m_abilityUI.gameObject.SetActive(false);
//				card.m_shortCutUI.text = PartyCards.m_partyCards.UpdateShortAbilityText(f, f.currentLevel);
//				card.m_shortCutUI.gameObject.SetActive(true);
//				card.m_rankUI.gameObject.SetActive(false);
//				card.m_passive01UI.gameObject.SetActive(false);
//				card.m_passive02UI.gameObject.SetActive(false);
//				card.m_portrait.spriteName = f.m_fullPortraitSpriteName;
//				card.m_followerData = f;
//				m_unlockedHeroCards.Add(card);
//
				//place model
				if (i < m_miniPlacement.Length)
				{
					GameObject miniPlace = m_miniPlacement[i];
					GameObject go = (GameObject)(Instantiate (f.gameObject, miniPlace.transform.position, miniPlace.transform.rotation));
					Follower fol = (Follower)go.GetComponent("Follower");
					fol.currentXP = charProgress.m_XP;
					fol.currentLevel = charProgress.m_level;


					int[] heroBadgeStates = new int[5];
					heroBadgeStates[0] = charProgress.m_badgeLevel1;
					heroBadgeStates[1] = charProgress.m_badgeLevel2;
					heroBadgeStates[2] = charProgress.m_badgeLevel3;
					heroBadgeStates[3] = charProgress.m_badgeLevel4;
					heroBadgeStates[4] = charProgress.m_badgeLevel5;
					//heroBadgeStates[5] = charProgress.m_badgeLevel6;
					fol.heroBadgeStates = heroBadgeStates;

					if (charProgress.m_XPBonus > 1.0f)
					{
						fol.XPBonus = charProgress.m_XPBonus;
						m_bonusLabels[i].gameObject.SetActive(true);
						UILabel l = (UILabel)m_bonusLabels[i].transform.GetComponentInChildren<UILabel>();
						if (l != null)
						{
							l.text = "XP x" + charProgress.m_XPBonus.ToString();
						}
					}


					fol.SetLevel();
					fol.id = i;

					go.transform.parent = miniPlace.transform;
				}

			} 
			else {
//				card.m_nameUI.gameObject.SetActive(false);
//				card.m_abilityUI.gameObject.SetActive(false);
//				card.m_rankUI.gameObject.SetActive(false);
//				card.m_passive01UI.gameObject.SetActive(false);
//				card.m_passive02UI.gameObject.SetActive(false);
//				card.m_portrait.spriteName = "Card_Back03";

				//Color newColor = new Color(0.29f, 0.18f, 0.04f);
				Color newColor = new Color(0.25f, 0.25f, 0.25f);
				Renderer r = (Renderer)m_lights[i].GetComponent("Renderer");
				r.material.color = newColor;
			}
			}	
//		}
		
		m_openCard = PartyCards.m_partyCards.m_party[0];
		UpdateHighlight(m_openCard.m_portrait.transform);

		List<int> badgeStates = SettingsManager.m_settingsManager.badgeStates;


		SettingsManager.m_settingsManager.startingDamage = SettingsManager.m_settingsManager.baseDamage;
		SettingsManager.m_settingsManager.startingHealth = SettingsManager.m_settingsManager.baseHealth;
		SettingsManager.m_settingsManager.startingEnergy = SettingsManager.m_settingsManager.baseEnergy;
		SettingsManager.m_settingsManager.startingArmor = SettingsManager.m_settingsManager.baseArmor;

		if (badgeStates[1] == 2)
		{
			SettingsManager.m_settingsManager.startingDamage += 1;
		}
		if (badgeStates[2] == 2)
		{
			SettingsManager.m_settingsManager.startingDamage += 1;
		}
		if (badgeStates[3] == 2)
		{
			SettingsManager.m_settingsManager.startingDamage += 1;
		}
		if (badgeStates[4] == 2)
		{
			SettingsManager.m_settingsManager.startingDamage += 2;
		}
		
		if (badgeStates[5] == 2)
		{
			SettingsManager.m_settingsManager.startingHealth += 1;
		}
		if (badgeStates[6] == 2)
		{
			SettingsManager.m_settingsManager.startingHealth += 1;
		}
		if (badgeStates[7] == 2)
		{
			SettingsManager.m_settingsManager.startingHealth += 1;
		}
		if (badgeStates[8] == 2)
		{
			SettingsManager.m_settingsManager.startingHealth += 2;
		}
		
		if (badgeStates[9] == 2)
		{
			SettingsManager.m_settingsManager.startingEnergy += 2;
		}
		if (badgeStates[10] == 2)
		{
			SettingsManager.m_settingsManager.startingEnergy += 2;
		}
		if (badgeStates[11] == 2)
		{
			SettingsManager.m_settingsManager.startingEnergy += 3;
		}
		if (badgeStates[12] == 2)
		{
			SettingsManager.m_settingsManager.startingEnergy += 3;
		}
		
		UIManager.m_uiManager.UpdateEnergy(SettingsManager.m_settingsManager.startingEnergy);
		UIManager.m_uiManager.UpdateHealth(SettingsManager.m_settingsManager.startingHealth);
		StartCoroutine(UIManager.m_uiManager.UpdateArmor(SettingsManager.m_settingsManager.startingArmor));
		StartCoroutine(UIManager.m_uiManager.UpdateDamage(SettingsManager.m_settingsManager.startingDamage));

		UIManager.m_uiManager.unselectedScale = UIManager.m_uiManager.miniScale;

		for (int i=0; i < SettingsManager.m_settingsManager.badgeStates.Count; i++)
		{
//			Debug.Log(i.ToString());
			if (SettingsManager.m_settingsManager.badgeStates[i] == 0 && i<17)
			{
				m_storeBadges[i].Initialize(-1, StoreBadge.State.Locked, i);

			} else
			{
				
				if (SettingsManager.m_settingsManager.badgeStates[i] == 1 && i<17)
				{
					m_storeBadges[i].Initialize(-1, StoreBadge.State.UnEquipped, i);
					
				} else if (SettingsManager.m_settingsManager.badgeStates[i] == 2 && i<17)
				{

					m_storeBadges[i].Initialize(-1, StoreBadge.State.Equipped, i);
					m_currentBadges ++;
					
					// update active badges UI
					foreach (StoreBadge b in m_badgeSlots)
					{
						if (b.gameObject.activeSelf && b.m_sprite.spriteName == "BadgeSlot01")
						{
							b.Initialize(-1, StoreBadge.State.BadgeSlot, -1);
							b.SetBadge(m_storeBadges[i]);
							break;
						}
					}
						
					
				} 
			}
		}


		for (int i=0; i < m_maxPartySize; i++) {
			PartySlot ps = new PartySlot();
			ps.m_portrait = PartyCards.m_partyCards.m_party[i];
			ps.m_badgePortrait = m_badgeSlots[i];
			m_partySlots.Add(ps);
		}



		//disable start and reset text
		m_enabledTextTop = AssetManager.m_assetManager.m_typogenicText [10].ColorTopLeft;
		m_enabledTextBottom = AssetManager.m_assetManager.m_typogenicText [10].ColorBottomLeft;

		AssetManager.m_assetManager.m_typogenicText [10].ColorTopLeft = m_disabledTextColor;
		AssetManager.m_assetManager.m_typogenicText [10].ColorTopRight = m_disabledTextColor;
		AssetManager.m_assetManager.m_typogenicText [10].ColorBottomLeft = m_disabledTextColor;
		AssetManager.m_assetManager.m_typogenicText [10].ColorBottomRight = m_disabledTextColor;

		AssetManager.m_assetManager.m_typogenicText [11].ColorTopLeft = m_disabledTextColor;
		AssetManager.m_assetManager.m_typogenicText [11].ColorTopRight = m_disabledTextColor;
		AssetManager.m_assetManager.m_typogenicText [11].ColorBottomLeft = m_disabledTextColor;
		AssetManager.m_assetManager.m_typogenicText [11].ColorBottomRight = m_disabledTextColor;
	}


	
	
	void Update () {
		
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			Application.LoadLevel("MainMenu01");
		}
		
		//check for mouse hovering over hero card
		if (!Input.GetMouseButton(0))
		{
			//Ray cardTouchRay = m_UICam.ScreenPointToRay(Input.mousePosition);
			Ray cardTouchRay = m_miniCam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(cardTouchRay, out hit))
			{
				//if (hit.transform.gameObject.tag == "UICard" && m_followers.Count < 4)
				if (hit.transform.gameObject.tag == "Follower" && m_followers.Count < m_maxPartySize && m_openCard.m_followerData == null)
				{
					Follower f = (Follower)hit.transform.GetComponent("Follower");
					HoverCard(f);
//					UICard card = (UICard)hit.transform.GetComponent("UICard");
//					
//					if (card.selectState != UICard.SelectState.Selected)
//					{
//						if (m_scaledCard != null)
//						{
//							Vector3 s =  m_charCard.transform.localScale;
//							s *= m_unselectedScale;
//							m_scaledCard.transform.localScale = s;	
//							m_scaledCard.transform.position = m_scaledcardPos;
//							m_scaledCard = null;
//						}
//						
//						Vector3 scale =  m_charCard.transform.localScale;
//						scale *= m_selectedScale;
//						card.transform.localScale = scale;
//						m_scaledCard = card;
//						
//						Vector3 pos = card.transform.position;
//						m_scaledcardPos = pos;
//						pos.z = 199.8f;
//						card.transform.position = pos;
//					
//					
//						if (card.selectState == UICard.SelectState.Unselected)
//						{
//							HoverCard(card.m_followerData);
//						} else if ((card.selectState == UICard.SelectState.Locked || card.selectState == UICard.SelectState.Selected) && m_openCard.m_followerData != null)
//						{
//							BlankCard(m_openCard);	
//						}
//					} else if (card.selectState == UICard.SelectState.Selected && m_scaledCard != null)
//					{
//						BlankCard(m_openCard);
//						Vector3 s =  m_charCard.transform.localScale;
//						s *= m_unselectedScale;
//						m_scaledCard.transform.localScale = s;	
//						m_scaledCard.transform.position = m_scaledcardPos;
//						m_scaledCard = null;
//					}
					
				}
				
			} else
			{
				if (m_openCard != null)
				{
					if (m_openCard.m_followerData != null)
					{
						BlankCard(m_openCard);
					}
				}
				
//				if (m_scaledCard != null)
//				{
//					Vector3 scale =  m_charCard.transform.localScale;
//					scale *= m_unselectedScale;
//					m_scaledCard.transform.localScale = scale;	
//					m_scaledCard.transform.position = m_scaledcardPos;
//					m_scaledCard = null;
//				}
			}
		}
		
		
		
		
		
		
		
		
		
		if (Input.GetMouseButtonUp(0))
		{
			Ray cardTouchRay = m_miniCam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(cardTouchRay, out hit))
			{
				//if (hit.transform.gameObject.tag == "UICard")
				if (hit.transform.gameObject.tag == "Follower" && m_followers.Count < m_maxPartySize)
				{
//					UICard card = (UICard)hit.transform.GetComponent("UICard");
//					if (card.selectState == UICard.SelectState.Unselected)
//					{
						//add hero to party
						//Follower f = card.m_followerData;
					Follower f = (Follower) hit.transform.GetComponent("Follower");
					//disable hitbox
					((BoxCollider)f.transform.GetComponent("BoxCollider")).enabled = false;
						m_followers.Add(f);
//						m_openCard.animation.Play();
					f.transform.localScale = Vector3.one * 1.35f;

					m_miniPlacement[f.id].animation["ModelSelect01"].speed = 1.25f;
					m_miniPlacement[f.id].animation.Play("ModelSelect01");

						
						//update card in deck
//						card.selectState = UICard.SelectState.Selected;
//						card.m_portrait.color = spentPortraitColor;
//						card.m_shortCutUI.color = spentPortraitColor;
//						//card.m_portrait.spriteName = "Card_HeroPH_Full";
//						Vector3 scale =  m_charCard.transform.localScale;
//						scale *= m_unselectedScale;
//						m_scaledCard.transform.localScale = scale;	
//						m_scaledCard.transform.position = m_scaledcardPos;
//						m_scaledCard = null;

						// update stats
						GameState.ProgressState charProgress = SettingsManager.m_settingsManager.gameProgress[0];
						foreach (GameState.ProgressState thisCharState in SettingsManager.m_settingsManager.gameProgress)
						{
							if (thisCharState.m_followerType == f.m_followerType)
							{
								charProgress = thisCharState;
								break;
							}
						}

					foreach (PartySelectMenu.PartySlot ps in PartySelectMenu.m_partySelectMenu.partySlots)
					{
						if (ps.m_followerState == PartySelectMenu.PartySlot.State.Empty)
						{
							ps.m_followerState = PartySelectMenu.PartySlot.State.Occupied;
							break;
						}
					}
//						Follower.Level l = f.m_levelTable[charProgress.m_level];
//						if (l != null)
//						{
//							m_startingHealth += l.m_healthMod;
//							m_startingEnergy += l.m_energyMod;
//							m_startingDamage += l.m_damageMod;
//							m_startingArmor += l.m_armorMod;
							
//							UIManager.m_uiManager.UpdateHealth(m_startingHealth);	
//							UIManager.m_uiManager.UpdateEnergy(m_startingEnergy);
//							StartCoroutine(UIManager.m_uiManager.UpdateDamage(m_startingDamage));
//							StartCoroutine(UIManager.m_uiManager.UpdateArmor(m_startingArmor));
//						}
						
						
					
						//update open card
						bool partyFull = true;
						foreach (UICard c in PartyCards.m_partyCards.m_party)
						{

							if (c.m_followerData == null && c.m_portrait.gameObject.activeSelf)
							{
								m_openCard = c;
							UpdateHighlight(m_openCard.m_portrait.transform);
								partyFull = false;
								break;
							}


						}

						if (partyFull)
						{
							m_openCard = null;
							m_highlight.gameObject.SetActive(false);
						}
//					}

					if (m_followers.Count == 1)
					{
						AssetManager.m_assetManager.m_typogenicText [10].ColorTopLeft = m_enabledTextTop;
						AssetManager.m_assetManager.m_typogenicText [10].ColorTopRight = m_enabledTextTop;
						AssetManager.m_assetManager.m_typogenicText [10].ColorBottomLeft = m_enabledTextBottom;
						AssetManager.m_assetManager.m_typogenicText [10].ColorBottomRight = m_enabledTextBottom;
						
						AssetManager.m_assetManager.m_typogenicText [11].ColorTopLeft = m_enabledTextTop;
						AssetManager.m_assetManager.m_typogenicText [11].ColorTopRight = m_enabledTextTop;
						AssetManager.m_assetManager.m_typogenicText [11].ColorBottomLeft = m_enabledTextBottom;
						AssetManager.m_assetManager.m_typogenicText [11].ColorBottomRight = m_enabledTextBottom;
					}
				} 
			}
				cardTouchRay = m_UICam.ScreenPointToRay(Input.mousePosition);
				//RaycastHit hit;
				if(Physics.Raycast(cardTouchRay, out hit))
				{
					if (hit.transform.gameObject.tag == "ResetButton" && m_followers.Count > 0)
					{

						// enable hitboxes
						foreach (Follower f in m_followers)
						{
							((BoxCollider)f.transform.GetComponent("BoxCollider")).enabled = true;
						f.transform.localScale = Vector3.one;
						}

						//unselect heroes in deck
	//					foreach (UICard c in m_unlockedHeroCards)
	//					{
	//						if (c.selectState == UICard.SelectState.Selected)
	//						{
	//							c.selectState = UICard.SelectState.Unselected;
	//							//c.m_portrait.spriteName = c.m_followerData.m_fullPortraitSpriteName;
	//							c.m_portrait.color = Color.white;
	//							c.m_shortCutUI.color = Color.white;
	//						}
	//					}
						
						//remove all followers from party
						foreach (UICard c in PartyCards.m_partyCards.m_party)
						{
							if (c.m_followerData != null)
							{
								//remove stat bonuses
								Follower f = c.m_followerData;
								GameState.ProgressState charProgress = SettingsManager.m_settingsManager.gameProgress[0];
								foreach (GameState.ProgressState thisCharState in SettingsManager.m_settingsManager.gameProgress)
								{
									if (thisCharState.m_followerType == f.m_followerType)
									{
										charProgress = thisCharState;
										break;
									}
								}
								Follower.Level l = f.m_levelTable[charProgress.m_level];
								if (l != null)
								{
									m_startingHealth -= l.m_healthMod;
									m_startingEnergy -= l.m_energyMod;
									m_startingDamage -= l.m_damageMod;
									m_startingArmor -= l.m_armorMod;
									
//									UIManager.m_uiManager.UpdateHealth(m_startingHealth);	
//									UIManager.m_uiManager.UpdateEnergy(m_startingEnergy);
//									StartCoroutine(UIManager.m_uiManager.UpdateDamage(m_startingDamage));
//									StartCoroutine(UIManager.m_uiManager.UpdateArmor(m_startingArmor));
								}
								
								
								BlankCard(c);
								m_openCard = PartyCards.m_partyCards.m_party[0];

								if (!m_highlight.gameObject.activeSelf)
								{
									m_highlight.gameObject.SetActive(true);
								}

							UpdateHighlight(m_openCard.m_portrait.transform);
							}
						}
						
						m_followers.Clear();

					// remove any equipped badges
					foreach (PartySlot ps in m_partySlots)
					{
						if (ps.m_followerState == PartySlot.State.Occupied)
						{
							ps.m_followerState = PartySlot.State.Empty;
						}

						if (ps.m_badgeState == PartySlot.State.Occupied)
						{
							//reactivate badge in bank


							//deactivate badge in party menu
							m_storeBadges[ps.m_badgePortrait.id].ChangeState(StoreBadge.State.UnEquipped);
							m_currentBadges --;
							SettingsManager.m_settingsManager.badgeStates[ps.m_badgePortrait.id] = 1;
							ps.m_badgePortrait.ClearBadge();
							ps.m_badgeState = PartySlot.State.Empty;
							ps.m_badge = null;
						}
					}

					// disable button text color
					AssetManager.m_assetManager.m_typogenicText [10].ColorTopLeft = m_disabledTextColor;
					AssetManager.m_assetManager.m_typogenicText [10].ColorTopRight = m_disabledTextColor;
					AssetManager.m_assetManager.m_typogenicText [10].ColorBottomLeft = m_disabledTextColor;
					AssetManager.m_assetManager.m_typogenicText [10].ColorBottomRight = m_disabledTextColor;
					
					AssetManager.m_assetManager.m_typogenicText [11].ColorTopLeft = m_disabledTextColor;
					AssetManager.m_assetManager.m_typogenicText [11].ColorTopRight = m_disabledTextColor;
					AssetManager.m_assetManager.m_typogenicText [11].ColorBottomLeft = m_disabledTextColor;
					AssetManager.m_assetManager.m_typogenicText [11].ColorBottomRight = m_disabledTextColor;

					} 
					else if (hit.transform.gameObject.tag == "CraftButton" && m_followers.Count > 0)
					{
						StartCoroutine(LoadGameScene());
					}
					else if (hit.transform.gameObject.tag == "BackButton")
					{
						Application.LoadLevel("MainMenu01");
					}
				}
		}

		DoHoveredCards ();
	
	}

	private IEnumerator MoveCardToGrave (GameObject go)
	{
		float t = 0;
		float time = 0.5f;
		Vector3 startPos = go.transform.position;
		Vector3 endPos = AssetManager.m_assetManager.m_props[2].transform.position;
		Vector3 startScale = go.transform.localScale;
		Vector3 endScale = startScale * 1.5f;
		
		while (t < time)
		{
			t += Time.deltaTime;;
			Vector3 nPos = Vector3.Lerp(startPos, endPos , t / time);
			Vector3 newScale = Vector3.Lerp(startScale, endScale, t / time);
			go.transform.position = nPos;
			go.transform.localScale = newScale;
			yield return null;

		}
		
		//go.transform.position = endPos;
		Destroy (go);
	}

	private IEnumerator LoadGameScene ()
	{
		// play deck building anim

//		AssetManager.m_assetManager.m_props [2].SetActive (true);
//		AssetManager.m_assetManager.m_props [3].SetActive (false);
//
//		foreach (List<GameObject> l in m_deck) {
//			foreach (GameObject go in l)
//			{
//				UICard c = (UICard)go.GetComponent("UICard");
//				c.m_abilityUI.gameObject.SetActive(false);
//				c.m_nameUI.gameObject.SetActive(false);
//				c.m_miscOBJ[0].gameObject.SetActive(false);
//				c.m_rankUI.gameObject.SetActive(false);
//				foreach (UISprite s in c.m_miscSprite)
//				{
//					s.gameObject.SetActive(false);
//				}
//				c.m_portrait.spriteName = "Card_Back03";
//				StartCoroutine(MoveCardToGrave(go));
//				yield return new WaitForSeconds(0.05f);
//			}
//		}
//
//		yield return new WaitForSeconds(1.0f);

		// player fade out anim
		Color clr = Color.white;
		clr.a = 0;
		UIManager.m_uiManager.m_statSprites[0].color = clr;
		UIManager.m_uiManager.m_statSprites[0].gameObject.SetActive(true);

		float time = 0.0f;
		while (time < 1.0f)
		{
			time += Time.deltaTime;
			float a = Mathf.Lerp(0.0f, 1.0f, time / 1.0f);
			Color c = UIManager.m_uiManager.m_statSprites[0].color;
			c.a = a;
			UIManager.m_uiManager.m_statSprites[0].color = c;
			yield return null;
		}
		
		SettingsManager.m_settingsManager.party = new List<Follower>();
		SettingsManager.m_settingsManager.partySlots = m_partySlots;
		foreach (Follower f in m_followers)
		{
			foreach (GameObject fGO in m_followerBank)
			{
				Follower f2 = (Follower)fGO.GetComponent("Follower");
				if (f.m_followerType == f2.m_followerType)
				{
					SettingsManager.m_settingsManager.party.Add(f2);
					break;
				}
			}
		}
		//SettingsManager.m_settingsManager.party = m_followers;
		Debug.Log("LOADING GAME SCENE");
		Application.LoadLevel("GameScene01");

		yield return null;
	}
	

	private void HoverCard (Follower f)
	{
		if (m_openCard.m_followerData != f && m_followers.Count < m_maxPartySize)
		{
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

//			f.currentLevel = charProgress.m_level;
//			f.SetLevel();
			m_openCard.m_followerData = f;
			m_openCard.m_nameText.Text = f.m_nameText;
			m_openCard.m_nameText.gameObject.SetActive(true);
			m_openCard.m_miscOBJ[1].gameObject.SetActive(true);
			m_openCard.m_portrait.spriteName = f.m_portraitSpriteName;
//			m_openCard.m_abilityUI.text = PartyCards.m_partyCards.UpdateAbilityText(f, charProgress.m_level);
			//m_openCard.m_shortCutUI.text = PartyCards.m_partyCards.UpdateShortAbilityText(f, charProgress.m_level);
//			m_openCard.m_abilityUI.gameObject.SetActive(true);
//			m_openCard.m_rankUI.text = "Level " + (charProgress.m_level+1) + " " + f.m_followerClass.ToString();
//			m_openCard.m_rankUI.gameObject.SetActive(true);
//			PartyCards.m_partyCards.UpdatePassiveText(m_openCard, f, charProgress.m_level);
//			m_openCard.m_passive01UI.gameObject.SetActive(true);
//			m_openCard.m_passive02UI.gameObject.SetActive(true);
//			m_openCard.m_healthIcon.gameObject.SetActive(true);

//			Follower.Level l = f.m_levelTable[charProgress.m_level];
//			int cost = f.m_abilityCost + l.m_abilityCostMod;
//			int cost = f.abilityCost;
//			m_openCard.m_armorUI.text = cost.ToString();
//			m_openCard.m_damageIcon.spriteName = "Icon_Energy";
//			m_openCard.m_miscOBJ[0].gameObject.SetActive(true);
//			m_openCard.m_miscText[0].text = "XP: " + charProgress.m_XP.ToString() + " / " + f.maxXP.ToString();
//			m_openCard.m_miscText[0].gameObject.SetActive(true);




			//display deck

			//GameObject[] items = f.m_deck[j].m_levelCards;
			List<GameObject> cards = new List<GameObject>();
			List<GameObject> items = new List<GameObject>();
			// only get unlocked cards
			for (int i = 0; i < f.m_deck.Length; i ++)
			{
				if (i < f.currentLevel)
				{
					Follower.Deck levelCards = f.m_deck[i];
					foreach (GameObject go in levelCards.m_levelCards)
					{
						items.Add(go);
					}
				}
			}

			Vector3 startPos = m_decksStart[m_followers.Count].transform.position;
			Vector3 endPos = m_decksEnd[m_followers.Count].transform.position;
			int numCards = Mathf.Clamp( items.Count-1, 1, 99);
			int depthOffset = m_followers.Count*10;
			
			for (int i=0; i < items.Count; i++)
			{
				Item thisItem = (Item)items[i].GetComponent("Item");
				Vector3 cardPos = Vector3.Lerp(startPos, endPos, ((float)i) / ((float)numCards));
				Vector3 cardRot = new Vector3(0,0,20);
				//GameObject fCard = (GameObject)Instantiate(UIManager.m_uiManager.m_cardSmall, cardPos, Quaternion.Euler(cardRot));
				GameObject fCard = (GameObject)Instantiate(UIManager.m_uiManager.m_cardSmall, cardPos, UIManager.m_uiManager.m_cardSmall.transform.rotation);
				fCard.transform.parent = UIManager.m_uiManager.m_inventoryUI.transform;
				cards.Add(fCard);
				UICard cardUI = (UICard)fCard.GetComponent("UICard");
				fCard.transform.localScale = UIManager.m_uiManager.miniScale;
			
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
				
				cardUI.m_nameUI.depth += i+i+depthOffset;
				cardUI.m_abilityUI.depth += i+i+depthOffset;
				cardUI.m_shortCutUI.depth += i+i+depthOffset;
				cardUI.m_portrait.depth += i+i+depthOffset;
				cardUI.m_rankUI.depth += i+i+depthOffset;
				cardUI.m_healthUI.depth += i+i+depthOffset;
				cardUI.m_healthIcon.depth += i+i+depthOffset;
				cardUI.m_damageIcon.depth += i+i+depthOffset;
				
				foreach (UISprite s in cardUI.m_miscSprite)
				{
					s.depth += i+i+depthOffset;
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
					//s.depth += i+i+depthOffset;
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
			m_deck.Add(cards);


			m_miniPlacement[f.id].animation.Play();
		}
	}
	
	private void BlankCard (UICard c)
	{
		c.m_followerData = null;
		c.m_portrait.spriteName = "Token_Empty";
		c.m_nameText.gameObject.SetActive(false);
		c.m_miscOBJ[1].gameObject.SetActive(false);
//		c.m_abilityUI.gameObject.SetActive(false);
//		c.m_rankUI.gameObject.SetActive(false);
//		c.m_passive01UI.gameObject.SetActive(false);
//		c.m_passive02UI.gameObject.SetActive(false);
//		c.m_miscOBJ[0].gameObject.SetActive(false);
//		c.m_healthIcon.gameObject.SetActive(false);
//		c.m_miscText[0].gameObject.SetActive (false);

		if (m_deck.Count > 0) {
			List<GameObject> cards = m_deck[m_deck.Count-1];

			foreach (GameObject go in cards) {
				Destroy(go);
			}

			m_deck.RemoveAt(m_deck.Count-1);
		}


	}
	
	private void UpdateHighlight(Transform newPos)
	{
		Vector3 pos = newPos.position;
		pos.z = m_highlight.transform.position.z;
		m_highlight.transform.position = pos;
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

	public int currentBadges {get{ return m_currentBadges; }set{m_currentBadges = value;}}
	public int partyCount {get{return m_followers.Count;}}
	public List<PartySlot> partySlots { get { return m_partySlots; } }
	
}
