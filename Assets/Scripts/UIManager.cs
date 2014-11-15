using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {
	
	public enum MenuMode
	{
		None,
		CharSelect,
		Chest,
		Inventory,
		Crafting,
		EndLevel,
		FollowerSwap,
		GameOver,
		Storage,
		Pause,
		Shop,
		Badges,
		BadgeStore,
		MainMenu,
		SelectHero,
		Limbo,
		Trials,
		ChapterSelect,
	}
	
	public enum ReactMode
	{
		ForceHidden,
		ForceActive,
		MouseProx,
	}

	public enum Icon
	{
		None,
		Energy,
		Armor,
		Health,
		Actions,
		Gold,
		MeleeDamage,
	}
	
	public static UIManager
		m_uiManager;
	
	public ReactMode
		m_currentReactMode = ReactMode.ForceActive;
	
	public GameObject[]
		m_followerCards;
	
	public Animation
		m_partyCards,
		m_equipCards,
		m_buttons;
	
	public GameObject
		m_damageText,
		m_UIdamageText,
		m_charSelectUI,
		m_inventoryUI,
		m_chestUI,
		m_craftingUI,
		m_HUD,
		m_buttonUI,
		m_levelEndUI,
		m_followerSwapUI,
		m_gameOverUI,
		m_shopUI,
		m_actionGUI,
		m_statusEffectGUI,
		m_enemyStatusGUI,
		m_storageUI,
		m_pauseUI,
		m_itemCard,
		m_cardSmall,
		m_charCard,
		m_uiCamera,
		m_exitButton,
		m_fuseButton,
		m_backpackButton,
		m_shopPortalGUI;
	
	public UILabel[]
		m_nameUI,
		m_healthUI,
		m_damageUI,
		m_energyUI,
		m_armorUI,
		m_followerNames,
		m_followerAbilityText,
		m_actionUI,
		m_levelEndHeader,
		m_chainUI,
		m_XPUI;
	
	public UISprite[]
		m_portraits,
		m_goals,
		m_keys,
		m_statSprites;
	
	public UICard[]
		m_craftingSlots,
		m_equipSlots,
		m_charUnlockSlots,
		m_pauseMenuSlots,
		m_storageSlots,
		m_charSelectSlots,
		m_limboSlots,
		m_ShopSlots;

	public StoreBadge[]
		m_currentBadges;
	
	private bool
		m_targetDisplayed = false,
		m_partyCardsActive = false,
		m_equipCardsActive = false,
		m_buttonsActive = false,
		m_doPortal = false,
		m_leaveDungeon = false;
	
	private UICard
		m_invHoveredCard = null;
	
	private Vector3
		m_selectedScale = new Vector3(0.94f,0.94f,0.94f),
		m_unselectedScale = new Vector3(0.44f,0.44f,0.44f),
		//m_miniScale = new Vector3(0.27f, 0.27f,0.27f),
		m_miniScale = new Vector3(0.35f, 0.35f,0.35f),
		m_boundsNormalPos = new Vector3(0,160,0),
		m_boundsNormalSize = new Vector3(220.7f,311.3f,10),
		m_boundsHoveredPos = new Vector3(0,77.6f,0),
		m_boundsHoveredSize = new Vector3(103.8f,156.1f,10);
	
	private MenuMode
		m_currentMenuMode = MenuMode.None;
	
	private List<GameObject> 
		m_cardList = new List<GameObject>();
	
	private List<FuseButton> 
		m_fuseButtons = new List<FuseButton>();

	private GameObject
		m_targetCard = null;

	private UICard
		m_chestCard = null;
	
	
	void Awake ()
	{
		m_uiManager = this;	

		if (m_followerCards.Length > 4) {
			originPosEC = m_followerCards [4].transform.localPosition;
			originScaleEC = m_followerCards [4].transform.localScale;
		}
	}
	
	public void Initialize()
	{
		//EquipCards.m_equipCards.Initialize();
//		foreach (UICard thisCard in m_equipSlots)
//		{
//			thisCard.Deactivate();
//			thisCard.gameObject.SetActive(false);
//		}
		
//		StartCoroutine(UIManager.m_uiManager.ChangeReactMode(UIManager.ReactMode.ForceActive));	
	}
	
	public void SetFollowers (List<Follower> followers)
	{
		Debug.Log ("SETTING PARTY");
		for (int i=0; i < m_followerCards.Length; i++)
		{
			if (i < followers.Count)
			{
				Follower thisFollower = followers[i];
				m_followerCards[i].SetActive(true);
				UICard cardUI = (UICard)m_followerCards[i].GetComponent("UICard");
				cardUI.selectState = UICard.SelectState.Unselected;
				cardUI.m_followerData = thisFollower;
				thisFollower.SetLevel();
				PartyCards.m_partyCards.UpdateCard(thisFollower);

//				cardUI.m_nameUI.gameObject.SetActive(true);

			} else {
				UICard cardUI = (UICard)m_followerCards[i].GetComponent("UICard");
				if (cardUI != null && i != 6)
				{
					cardUI.m_followerData = null;
					cardUI.m_portrait.gameObject.SetActive(false);

					foreach (Transform go in cardUI.m_miscOBJ)
					{
						go.gameObject.SetActive(false);
					}
				}
			}
		}
	}

	public IEnumerator DisplayTargetCard (Shop thisShop)
	{
		Debug.Log ("DISPLAYING SHOP CARD");
		m_targetCard = thisShop.gameObject;
		
		m_targetDisplayed = true;

		UICard card = (UICard)m_followerCards[4].GetComponent ("UICard");
		
		card.m_nameUI.text = thisShop.m_displayName;
		card.m_abilityUI.text = thisShop.m_abilityText;
		card.m_portrait.spriteName = thisShop.m_portraitSprite;
		card.m_portrait.gameObject.SetActive (true);
		card.m_rankUI.gameObject.SetActive (false);
		card.m_healthIcon.gameObject.SetActive (false);
		m_followerCards[4].SetActive(true);	
		
		//fly card out from enemy position
		Vector3 screenPos = FollowCamera.m_followCamera.m_camera.WorldToScreenPoint(m_targetCard.transform.position);
		float screenHeight = Screen.height;
		float screenWidth = Screen.width;
		screenPos.x -= (screenWidth / 2.0f);
		screenPos.y -= (screenHeight / 2.0f);
		
		float t = 0;
		float time = 0.2f;
		Vector3 startPos = screenPos;
		Vector3 startScale = Vector3.one * 0.25f;
		Vector3 endPos = originPosEC;
		Vector3 endScale = originScaleEC;
		while (t < time)
		{
			t += Time.deltaTime;;
			Vector3 nPos = Vector3.Lerp(startPos, endPos , t / time);
			Vector3 newScale = Vector3.Lerp(startScale, endScale, t / time);
			card.transform.localPosition = nPos;
			card.transform.localScale = newScale;
			yield return null;
		}
		card.transform.localPosition = endPos;
		card.transform.localScale = endScale;
		
		yield return true;
	}
	
	public IEnumerator DisplayTargetCard (Chest thisChest)
	{
		m_targetCard = thisChest.gameObject;
		
		m_targetDisplayed = true;	

		UICard card = (UICard)m_followerCards[4].GetComponent ("UICard");
		
		card.m_nameUI.text = "Chest";
		card.m_abilityUI.text = "Draw a card.";
		card.m_portrait.spriteName = "Card_Item_Chest";
		card.m_portrait.gameObject.SetActive (true);
		card.m_rankUI.gameObject.SetActive (false);
		card.m_healthIcon.gameObject.SetActive (false);
		m_followerCards[4].SetActive(true);	
		
		//fly card out from enemy position
		Vector3 screenPos = FollowCamera.m_followCamera.m_camera.WorldToScreenPoint(m_targetCard.transform.position);
		float screenHeight = Screen.height;
		float screenWidth = Screen.width;
		screenPos.x -= (screenWidth / 2.0f);
		screenPos.y -= (screenHeight / 2.0f);
		
		float t = 0;
		float time = 0.2f;
		Vector3 startPos = screenPos;
		Vector3 startScale = Vector3.one * 0.25f;
		Vector3 endPos = originPosEC;
		Vector3 endScale = originScaleEC;
		while (t < time)
		{
			t += Time.deltaTime;;
			Vector3 nPos = Vector3.Lerp(startPos, endPos , t / time);
			Vector3 newScale = Vector3.Lerp(startScale, endScale, t / time);
			card.transform.localPosition = nPos;
			card.transform.localScale = newScale;
			yield return null;
		}
		card.transform.localPosition = endPos;
		card.transform.localScale = endScale;

		yield return true;
	}
	
	public IEnumerator DisplayTargetCard (Card thisCard, GameObject displayCard)
	{
		m_targetCard = thisCard.gameObject;

		m_targetDisplayed = true;	

		UICard card = (UICard)displayCard.GetComponent ("UICard");
		
		card.m_nameUI.text = thisCard.m_displayName;
		card.m_abilityUI.text = thisCard.m_abilityText;
		card.m_portrait.spriteName = thisCard.m_portraitSpriteName;
		card.m_portrait.gameObject.SetActive (true);
		card.m_rankUI.gameObject.SetActive (false);
		card.m_healthIcon.gameObject.SetActive (false);
		displayCard.SetActive(true);	
		
		//fly card out from enemy position
		Vector3 screenPos = FollowCamera.m_followCamera.m_camera.WorldToScreenPoint(m_targetCard.transform.position);
		float screenHeight = Screen.height;
		float screenWidth = Screen.width;
		screenPos.x -= (screenWidth / 2.0f);
		screenPos.y -= (screenHeight / 2.0f);
		
		float t = 0;
		float time = 0.2f;
		Vector3 startPos = screenPos;
		Vector3 startScale = Vector3.one * 0.25f;
		Vector3 endPos = originPosEC;
		Vector3 endScale = originScaleEC;
		while (t < time)
		{
			t += Time.deltaTime;;
			Vector3 nPos = Vector3.Lerp(startPos, endPos , t / time);
			Vector3 newScale = Vector3.Lerp(startScale, endScale, t / time);
			card.transform.localPosition = nPos;
			card.transform.localScale = newScale;
			yield return null;
		}
		card.transform.localPosition = endPos;
		card.transform.localScale = endScale;

		yield return true;
	}

	private Vector3 originPosEC = Vector3.zero;
	private Vector3 originScaleEC = Vector3.zero;

	public IEnumerator DisplayTargetEnemy (Enemy targetEnemy)
	{
		Debug.Log ("DISPLAYING ENEMY CARD");
		m_targetCard = targetEnemy.gameObject;

		if (targetEnemy.currentEffect != GameManager.StatusEffect.None)
		{
			if (targetEnemy.effectDuration > 0)
			{
				m_enemyStatusGUI.SetActive(true);
				m_actionUI[1].text = targetEnemy.effectDuration.ToString();
			} else if (targetEnemy.effectDuration <= 0 && m_enemyStatusGUI.activeSelf)
			{
				m_enemyStatusGUI.SetActive(false);	
			}
		} else if (targetEnemy.currentEffect == GameManager.StatusEffect.None && m_enemyStatusGUI.gameObject.activeSelf)
		{
			m_enemyStatusGUI.SetActive(false);	
		}
		
//		if (m_followerCards[5].activeSelf || m_followerCards[6].activeSelf)
//		{
//			StartCoroutine(TurnOffTargetCard());	
//		}
		m_targetDisplayed = true;
		

		UICard card = (UICard)m_followerCards[4].GetComponent ("UICard");

		card.m_rankUI.text = "Level " + targetEnemy.m_level.ToString() + " " + targetEnemy.m_enemyType.ToString();
		card.m_nameUI.text = targetEnemy.m_displayName;
		card.m_abilityUI.text =  targetEnemy.m_abilityText;
		card.m_portrait.spriteName =  targetEnemy.m_portraitSpriteName;
		card.m_portrait.gameObject.SetActive (true);
		card.m_rankUI.gameObject.SetActive (true);
		card.m_healthIcon.gameObject.SetActive (true);



		//m_nameUI[1].text = targetEnemy.m_displayName;
		//m_followerAbilityText[4].text = targetEnemy.m_abilityText;
//		m_healthUI[1].gameObject.SetActive(true);
//		m_damageUI[1].gameObject.SetActive(true);
//		m_energyUI[1].gameObject.SetActive(true);
//		m_armorUI[1].gameObject.SetActive(true);
//		m_statSprites[0].gameObject.SetActive(true);
//		m_statSprites[1].gameObject.SetActive(true);
//		m_healthUI[1].text = targetEnemy.currentHealth.ToString();
//		m_damageUI[1].text = targetEnemy.damage.ToString();
//		m_energyUI[1].text = targetEnemy.energy.ToString();
//		m_armorUI[1].text = targetEnemy.armor.ToString();
		//m_portraits[4].spriteName = targetEnemy.m_portraitSpriteName;

		if (targetEnemy.m_abilityEnergyCost > 0)
		{
			card.m_miscText[0].text = targetEnemy.m_abilityEnergyCost.ToString();
			card.m_damageIcon.spriteName = "Icon_Energy";
		} 
		else 
		{
			card.m_miscText[0].gameObject.SetActive(false);
			card.m_damageIcon.gameObject.SetActive(false);
		}
		card.m_miscOBJ[0].gameObject.SetActive(true);

		Vector3 offset = Vector3.zero;
		offset.x -= 135;
//		((GUIFollow)card.gameObject.GetComponent ("GUIFollow")).SetTarget (targetEnemy.gameObject, offset);

		m_followerCards[4].SetActive(true);	

		//fly card out from enemy position
		Vector3 screenPos = FollowCamera.m_followCamera.m_camera.WorldToScreenPoint(targetEnemy.transform.position);
		float screenHeight = Screen.height;
		float screenWidth = Screen.width;
		screenPos.x -= (screenWidth / 2.0f);
		screenPos.y -= (screenHeight / 2.0f);

		float t = 0;
		float time = 0.2f;
		Vector3 startPos = screenPos;
		Vector3 startScale = Vector3.one * 0.25f;
		Vector3 endPos = originPosEC;
		Vector3 endScale = originScaleEC;
		while (t < time)
		{
			t += Time.deltaTime;;
			Vector3 nPos = Vector3.Lerp(startPos, endPos , t / time);
			Vector3 newScale = Vector3.Lerp(startScale, endScale, t / time);
			card.transform.localPosition = nPos;
			card.transform.localScale = newScale;
			yield return null;
		}
		card.transform.localPosition = endPos;
		card.transform.localScale = endScale;

		yield return true;
	}
	
	public void DisplayTargetCard (Follower thisFollower)
	{
//		m_targetCard = thisFollower.gameObject;
//		//m_enemyStatusGUI.SetActive(false);
//		if (m_followerCards[5].activeSelf || m_followerCards[4].activeSelf)
//		{
//			TurnOffTargetCard();	
//		}
//		m_targetDisplayed = true;
//		
//		UICard card = (UICard)m_followerCards[6].GetComponent("UICard");
//		card.m_followerData = thisFollower;
//		card.m_nameUI.text = thisFollower.m_nameText;
//		card.m_portrait.spriteName = thisFollower.m_portraitSpriteName;
//		card.m_rankUI.text = "Level " + (thisFollower.currentLevel+1).ToString() + " " + thisFollower.m_followerClass.ToString();	
//		card.m_abilityUI.text = PartyCards.m_partyCards.UpdateAbilityText(thisFollower, thisFollower.currentLevel);
//		//card.m_shortCutUI.text = PartyCards.m_partyCards.UpdateShortAbilityText(thisFollower, thisFollower.currentLevel);
//		PartyCards.m_partyCards.UpdatePassiveText(card, thisFollower, thisFollower.currentLevel);
//
//		card.m_healthUI.text = thisFollower.m_abilityCost.ToString();
//		card.m_damageIcon.spriteName = "Icon_Energy";
//		card.m_miscOBJ [0].gameObject.SetActive (true);
//
//		Vector3 offset = Vector3.zero;
//		offset.x -= 135;
//		((GUIFollow)card.gameObject.GetComponent ("GUIFollow")).SetTarget (thisFollower.gameObject, offset);
//
//		m_followerCards[6].SetActive(true);	
	}

	public IEnumerator TurnOffSiteCard ()
	{
		m_followerCards[5].SetActive(false);

		//if card hovered, move cards to first position
		//AssetManager.m_assetManager.m_props [33].transform.position = AssetManager.m_assetManager.m_props [31].transform.position;
		yield return null;
	}
	
	public IEnumerator TurnOffTargetCard ()
	{
		//fly card back to source position
//		Vector3 screenPos = FollowCamera.m_followCamera.m_camera.WorldToScreenPoint(m_targetCard.transform.position);
//		float screenHeight = Screen.height;
//		float screenWidth = Screen.width;
//		screenPos.x -= (screenWidth / 2.0f);
//		screenPos.y -= (screenHeight / 2.0f);
//		
//		float t = 0;
//		float time = 0.2f;
//		Vector3 startPos = originPosEC;
//		Vector3 startScale = originScaleEC;
//		Vector3 endPos = screenPos;
//		Vector3 endScale = Vector3.one * 0.25f;
//		while (t < time)
//		{
//			t += Time.deltaTime;;
//			Vector3 nPos = Vector3.Lerp(startPos, endPos , t / time);
//			Vector3 newScale = Vector3.Lerp(startScale, endScale, t / time);
//			m_followerCards[4].transform.localPosition = nPos;
//			m_followerCards[4].transform.localScale = newScale;
//			yield return null;
//		}
////		m_followerCards[4].transform.localPosition = endPos;
////		m_followerCards[4].transform.localScale = endScale;
//		m_followerCards[4].transform.localPosition = startPos;
//		m_followerCards [4].transform.localScale = startScale;

		m_targetDisplayed = false;
		m_targetCard = null;
		
		m_followerCards[4].SetActive(false);	
		//m_followerCards[5].SetActive(false);
		//m_followerCards[6].SetActive(false);



		yield return null;
	}



	private List<GameObject> m_stackGOList = new List<GameObject> ();
	public IEnumerator UpdateStack ()
	{
		if (m_stackGOList.Count > 0) {

			foreach (GameObject go in m_stackGOList)
			{
				Destroy(go);
			}
			m_stackGOList.Clear();
		}

		//get all enemies with an initiative
		if (GameManager.m_gameManager.enemies != null) {

			List<Enemy> activeEnemies = new List<Enemy> ();
			GameObject lastStackGO = null;
			foreach (Enemy e in GameManager.m_gameManager.enemies) {
				if (e.initiative > 0) {
					activeEnemies.Add (e);
				}
			}

			activeEnemies.Sort (delegate(Enemy i2, Enemy i1) {
				return i2.initiative.CompareTo (i1.initiative);
			});

			int numCards = Mathf.Clamp (activeEnemies.Count - 1, 1, 99);
			float height = 0.09f;

			Vector3 startPos = AssetManager.m_assetManager.m_props [27].transform.position;
			Vector3 endPos = AssetManager.m_assetManager.m_props [28].transform.position;
		
			for (int i=0; i <activeEnemies.Count; i++) {
				Vector3 cardPos = Vector3.Lerp (startPos, endPos, ((float)i) / ((float)numCards));
				GameObject fCard = (GameObject)Instantiate (AssetManager.m_assetManager.m_props [30], cardPos, m_cardSmall.transform.rotation);
				UISprite spr = (UISprite)fCard.GetComponent ("UISprite");
				StackObject so = (StackObject)fCard.GetComponent ("StackObject");

				Enemy thisE = (Enemy)activeEnemies [i];
				so.m_target = thisE.transform;
				spr.spriteName = thisE.m_stackPortraitName;
				if (thisE.enemyState == Enemy.EnemyState.Idle || thisE.enemyState == Enemy.EnemyState.Inactive) {
					spr.color = Color.gray;
				}

				if (lastStackGO != null) {
					if (lastStackGO.transform.position.y - fCard.transform.position.y > height) {
						Vector3 newPos = lastStackGO.transform.position;
						newPos.y -= height;
						fCard.transform.position = newPos;
					}
				}
				lastStackGO = fCard;

				fCard.transform.parent = AssetManager.m_assetManager.m_props [29].transform;
				m_stackGOList.Add (fCard);
			}
		}

		yield return true;
	}
	
	public void UpdateName (string name)
	{
		Debug.Log(name);
		m_nameUI[0].text = name;
	}
	
	public void UpdateKey (bool turnON)
	{
		foreach (UISprite thisSprite in m_keys)
		{
			if (turnON && !thisSprite.gameObject.activeSelf)
			{
				thisSprite.gameObject.SetActive(true);
				return;
			} else if (!turnON && thisSprite.gameObject.activeSelf)
			{
				thisSprite.gameObject.SetActive(false);
				return;
			}
		}
	}
	
	public void UpdateEnergy (int newEnergy)
	{
		UILabel energyText = m_energyUI[0];
//		if (Player.m_player != null)
//		{
//			energyText = Player.m_player.statBar.m_miscText [2];
//		}

		if (Player.m_player != null)
		{
//			energyText.text = Player.m_player.currentEnergy.ToString();
//			AssetManager.m_assetManager.m_labels[6].text = "/" + Player.m_player.maxEnergy.ToString();
			AssetManager.m_assetManager.m_typogenicText[6].Text = Player.m_player.currentEnergy.ToString();
			AssetManager.m_assetManager.m_typogenicText[7].Text = "/" + Player.m_player.maxEnergy.ToString();
			if (Player.m_player.currentEnergy < Player.m_player.maxEnergy)
			{
				//energyText.color = Color.red;
				AssetManager.m_assetManager.m_typogenicText [6].ColorTopLeft = Color.red;
				AssetManager.m_assetManager.m_typogenicText [6].ColorTopRight = Color.red;
				AssetManager.m_assetManager.m_typogenicText [6].ColorBottomRight = Color.red;
				AssetManager.m_assetManager.m_typogenicText [6].ColorBottomLeft = Color.red;
			} else 
			{
				//energyText.color = Color.white;
				AssetManager.m_assetManager.m_typogenicText [6].ColorTopLeft = Color.white;
				AssetManager.m_assetManager.m_typogenicText [6].ColorTopRight = Color.white;
				AssetManager.m_assetManager.m_typogenicText [6].ColorBottomRight = Color.white;
				AssetManager.m_assetManager.m_typogenicText [6].ColorBottomLeft = Color.white;
			}
		} else {
//			energyText.text = newEnergy.ToString();	
//			m_energyUI[1].text = "/" + newEnergy.ToString();
			AssetManager.m_assetManager.m_typogenicText[6].Text = newEnergy.ToString();	
			AssetManager.m_assetManager.m_typogenicText[7].Text = "/" + newEnergy.ToString();                                   
		}
	}

	public void ReflowStats ()
	{
		Debug.Log ("REFLOWING STATS");
		float scaleFactor = 1.0f;
		List<Transform> displayedElements = new List<Transform> ();
		List<Vector3> elementPos = new List<Vector3> ();

		// get all currently displayed stat elements
		displayedElements.Add (AssetManager.m_assetManager.m_UIelements [7].transform);
		displayedElements.Add (AssetManager.m_assetManager.m_UIelements [8].transform);
		displayedElements.Add (AssetManager.m_assetManager.m_UIelements [9].transform);
		if (AssetManager.m_assetManager.m_UIelements [10].activeSelf)
		{
			displayedElements.Add (AssetManager.m_assetManager.m_UIelements [10].transform);
		}
		if (AssetManager.m_assetManager.m_UIelements [11].activeSelf)
		{
			displayedElements.Add (AssetManager.m_assetManager.m_UIelements [11].transform);
		}

		// update scale factor as needed
		if (displayedElements.Count == 5) {
			scaleFactor = 0.831f;
		}

		// determine element positions based on # of displayed elements
		if (displayedElements.Count == 3) {
			elementPos.Add(new Vector3(-110.9004f,-52.83594f,0));
			elementPos.Add(new Vector3(31.98047f,-52.83609f,0));
			elementPos.Add(new Vector3(176.0742f,-52.83594f,0));
		} else if (displayedElements.Count == 4) {
			elementPos.Add(new Vector3(-171.2871f,-52.83594f,0));
			elementPos.Add(new Vector3(-28.40625f,-52.83594f,0));
			elementPos.Add(new Vector3(115.6875f,-52.83594f,0));
			elementPos.Add(new Vector3(257.748f,-52.83594f,0));
		} else if (displayedElements.Count == 5) {
			elementPos.Add(new Vector3(-200.2852f,-52.83594f,0));
			elementPos.Add(new Vector3(-80.7832f,-52.83594f,0));
			elementPos.Add(new Vector3(37.99219f,-52.83594f,0));
			elementPos.Add(new Vector3(156.6738f,-52.83594f,0));
			elementPos.Add(new Vector3(264.3633f,-52.83594f,0));
		}

		//move and scale elements
		for (int i=0; i < displayedElements.Count; i++) {
			if (i < elementPos.Count)
			{
				Transform e = (Transform)displayedElements[i];
				e.localScale = Vector3.one * scaleFactor;
				e.localPosition = elementPos[i];
			}
		}
	}
	
	public void UpdateHealth (int newHealth)
	{
		UILabel healthText = m_healthUI [0];
//		if (Player.m_player != null)
//		{
//			healthText = Player.m_player.statBar.m_miscText [1];
//		}

		if (Player.m_player != null)
		{
			//healthText.text = Player.m_player.currentHealth.ToString();
			//AssetManager.m_assetManager.m_labels[7].text = "/" + Player.m_player.maxHealth.ToString();
			AssetManager.m_assetManager.m_typogenicText[8].Text = Player.m_player.currentHealth.ToString();
			AssetManager.m_assetManager.m_typogenicText[9].Text = "/" + Player.m_player.maxHealth.ToString();
			if (Player.m_player.currentHealth < Player.m_player.maxHealth)
			{
				//healthText.color = Color.red;
				AssetManager.m_assetManager.m_typogenicText [8].ColorTopLeft = Color.red;
				AssetManager.m_assetManager.m_typogenicText [8].ColorTopRight = Color.red;
				AssetManager.m_assetManager.m_typogenicText [8].ColorBottomRight = Color.red;
				AssetManager.m_assetManager.m_typogenicText [8].ColorBottomLeft = Color.red;
			} else 
			{
				//healthText.color = Color.white;
				AssetManager.m_assetManager.m_typogenicText [8].ColorTopLeft = Color.white;
				AssetManager.m_assetManager.m_typogenicText [8].ColorTopRight = Color.white;
				AssetManager.m_assetManager.m_typogenicText [8].ColorBottomRight = Color.white;
				AssetManager.m_assetManager.m_typogenicText [8].ColorBottomLeft = Color.white;
			}
		} else {
			//healthText.text = newHealth.ToString();	
			//m_healthUI[1].text = "/" + newHealth.ToString();
			AssetManager.m_assetManager.m_typogenicText[8].Text = newHealth.ToString();	
			AssetManager.m_assetManager.m_typogenicText[9].Text = "/" + newHealth.ToString();
		}
	}
	
	public IEnumerator UpdateDamage (int newDamage)
	{
		//UILabel damageText = m_damageUI[0];
//		if (Player.m_player != null)
//		{
//			damageText = Player.m_player.statBar.m_miscText [0];
//		}

		Color textColor = Color.white;
		if (Player.m_player != null)
		{
			if (newDamage > Player.m_player.damage)
			{
				textColor = Color.green;
			} else if (newDamage < Player.m_player.damage)
			{
				textColor = Color.red;	
			}
		}
//		damageText.color = textColor;
//		damageText.text = newDamage.ToString();	

		AssetManager.m_assetManager.m_typogenicText [5].ColorTopLeft = textColor;
		AssetManager.m_assetManager.m_typogenicText [5].ColorTopRight = textColor;
		AssetManager.m_assetManager.m_typogenicText [5].ColorBottomRight = textColor;
		AssetManager.m_assetManager.m_typogenicText [5].ColorBottomLeft = textColor;

		AssetManager.m_assetManager.m_typogenicText[5].Text =  newDamage.ToString();	
		
		yield return null;
	}
	
	public void UpdateXP (int newXP)
	{
		//string newAmt = "XP: " + newXP.ToString();
		//m_XPUI[0].text = newAmt;
		AssetManager.m_assetManager.m_typogenicText [19].Text = newXP.ToString ("D2");
	}
	
	public IEnumerator UpdateArmor (int newArmor)
	{
//		UILabel armorText = m_armorUI[0];
//		if (Player.m_player != null)
//		{
//			armorText = Player.m_player.statBar.m_miscText [3];
//		}

		Color textColor = Color.white;
		if (Player.m_player != null)
		{
			if (newArmor > Player.m_player.currentArmor)
			{
				textColor = Color.green;
			} else if (newArmor < Player.m_player.currentArmor)
			{
				textColor = Color.red;	
			}
		}
		
//		armorText.color = textColor;
//		armorText.text = newArmor.ToString();

		AssetManager.m_assetManager.m_typogenicText [4].ColorTopLeft = textColor;
		AssetManager.m_assetManager.m_typogenicText [4].ColorTopRight = textColor;
		AssetManager.m_assetManager.m_typogenicText [4].ColorBottomRight = textColor;
		AssetManager.m_assetManager.m_typogenicText [4].ColorBottomLeft = textColor;

		AssetManager.m_assetManager.m_typogenicText[4].Text = newArmor.ToString();

		if (newArmor == 0) {
			AssetManager.m_assetManager.m_UIelements [10].SetActive (false);
		} else if (!AssetManager.m_assetManager.m_UIelements [10].activeSelf) {
			AssetManager.m_assetManager.m_UIelements [10].SetActive (true);
			}

		UIManager.m_uiManager.ReflowStats ();
		
		yield return null;
	}
	
	public void UpdateActionPoints (int newAmount)
	{
//		if (newAmount > 1)
//		{
//			if (!m_actionGUI.activeSelf)
//			{
//				m_actionGUI.SetActive(true);
//				GUIFollow.m_guiFollow.SetTarget(Player.m_player.gameObject);
//			}
//			
//			UILabel actionLabel = GUIFollow.m_guiFollow.m_labels[0];
//			actionLabel.text = "+" + (newAmount-1).ToString();
//		} else if (m_actionGUI.activeSelf) {
//			GUIFollow.m_guiFollow.SetTarget(null);
//			m_actionGUI.SetActive(false);
//		}
		
//		GUIFollow gf = (GUIFollow)m_actionGUI.GetComponent("GUIFollow");
//		if (newAmount <= 0 && m_actionGUI.activeSelf)
//		{
//			gf.SetTarget(null);
//			m_actionGUI.SetActive(false);			
//		} else if (newAmount > 0) {
//			if (!m_actionGUI.activeSelf)
//			{
//				Vector3 offset = Vector3.zero;
//				offset.y = 140;
//				offset.x = 15;
//				m_actionGUI.SetActive(true);
//				gf.SetTarget(Player.m_player.gameObject, offset);
//			}	
//			UILabel actionLabel = gf.m_labels[0];
//			actionLabel.text = (newAmount).ToString();
		AssetManager.m_assetManager.m_typogenicText [18].Text = newAmount.ToString ();
//		}
	}
	
	public void SetGoalActive()
	{
//		for (int i=0; i<m_goals.Length; i++)
//		{
//			UISprite thisGoal = m_goals[i];
//			if (thisGoal.spriteName == "Icon_Goal_Off")
//			{
//				thisGoal.spriteName = "Icon_Goal_On";	
//				return;
//			}
//		}
	}
	
	public IEnumerator ChangeReactMode (ReactMode newMode)
	{
		m_currentReactMode = newMode;
		
		if (newMode == ReactMode.ForceActive)
		{
			if (!m_partyCardsActive)
			{
				UIManager.m_uiManager.m_partyCards.Play("HUDMenu_PartyShow");
				UIManager.m_uiManager.partyCardsActive = true;
			}
//			if (!m_equipCardsActive)
//			{
//				UIManager.m_uiManager.m_equipCards.Play("HUDMenu_EquipShow");
//				UIManager.m_uiManager.equipCardsActive = true;
//			}
			if (!m_buttonsActive)
			{
				UIManager.m_uiManager.m_buttons.Play("HUDMenu_ButtonShow");
				UIManager.m_uiManager.buttonsActive = true;
			}
		}
		yield return null;
	}
	
	public IEnumerator ChangeMenuMode (MenuMode newMode)
	{
		Debug.Log ("CHANGING MENU MODE : " + newMode);
		MenuMode oldMode = m_currentMenuMode;
		m_currentMenuMode = newMode;

		if (newMode == MenuMode.None)
		{
//			if (oldMode == MenuMode.CharSelect && m_currentReactMode == ReactMode.MouseProx)
//			{
//				m_partyCards.Play("HUDMenu_PartyHide");
//				m_equipCards.Play("HUDMenu_EquipHide");
//				m_buttons.Play("HUDMenu_ButtonHide");
//			}
		}
		else if (newMode == MenuMode.Limbo)
		{
			// populate limbo cards
			for (int i=0; i < GameManager.m_gameManager.limboCards.Length; i++)
			{
//				Item item= (Item)GameManager.m_gameManager.limboCards[i];
//				UICard card = (UICard)m_limboSlots[i];
//
//				if (item != null )
//				{
//					card.SetCard(item, false);
//				} else {
//					card.Deactivate();
//					card.m_portrait.spriteName = "Card_Empty";
//				}

				GameManager.GraveSlot gs = (GameManager.GraveSlot) GameManager.m_gameManager.grave[i];
				UICard card = (UICard)m_limboSlots[i];
				if (gs.type == GameManager.GraveSlot.ObjectType.Item)
				{
					card.SetCard(gs.item, false);
				} else if (gs.type == GameManager.GraveSlot.ObjectType.Enemy)
				{
					card.SetCard(gs.enemy, false);
				} else if (gs.type == GameManager.GraveSlot.ObjectType.None)
				{
					card.Deactivate();
					card.m_portrait.spriteName = "Card_Empty";
				}
			}

			m_HUD.gameObject.SetActive(false);
			m_buttonUI.gameObject.SetActive(false);
			m_inventoryUI.gameObject.SetActive(true);
			PartyCards.m_partyCards.gameObject.SetActive(false);
			AssetManager.m_assetManager.m_props[16].gameObject.SetActive(false);
			AssetManager.m_assetManager.m_props[17].gameObject.SetActive(false);
			AssetManager.m_assetManager.m_props[19].SetActive(false);

			yield return new WaitForSeconds(0.01f);
			Time.timeScale = 0;

			List<Card> validCards = new List<Card>();
			yield return StartCoroutine(InputManager.m_inputManager.GetMouseSelection(validCards));

			Time.timeScale = 1;

			m_inventoryUI.gameObject.SetActive(false);
			m_HUD.gameObject.SetActive(true);
			m_buttonUI.gameObject.SetActive(true);
			PartyCards.m_partyCards.gameObject.SetActive(true);
			AssetManager.m_assetManager.m_props[16].gameObject.SetActive(true);
			AssetManager.m_assetManager.m_props[17].gameObject.SetActive(true);
			AssetManager.m_assetManager.m_props[19].SetActive(true);
			m_currentMenuMode = MenuMode.None;
		}
		else if (newMode == MenuMode.SelectHero)
		{
			AssetManager.m_assetManager.m_props[8].gameObject.SetActive(true);

			// turn off fuse buttons
			if (m_fuseButtons.Count > 0)
			{
				foreach (FuseButton fb in m_fuseButtons)
				{
					fb.gameObject.SetActive(false);
				}
			}

			List<Card> validCards = new List<Card>();
			yield return StartCoroutine(InputManager.m_inputManager.GetMouseSelection(validCards));

			AssetManager.m_assetManager.m_props[8].gameObject.SetActive(false);

			// turn on fuse buttons
			if (m_fuseButtons.Count > 0)
			{
				foreach (FuseButton fb in m_fuseButtons)
				{
					fb.gameObject.SetActive(true);
				}
			}
		}
		else if (newMode == MenuMode.Shop)
		{

			// Get bonus from shop and Update shop UI with bonus type
			switch (Shop.m_shop.bonus)
			{
			case Shop.ShopBonus.DamageBonus1:
				AssetManager.m_assetManager.m_typogenicText[13].Text = "1";
				AssetManager.m_assetManager.m_uiSprites[2].spriteName = "Icon_Attack";
				break;
			case Shop.ShopBonus.DamageBonus2:
				AssetManager.m_assetManager.m_typogenicText[13].Text = "2";
				AssetManager.m_assetManager.m_uiSprites[2].spriteName = "Icon_Attack";
				break;
			case Shop.ShopBonus.HealthBonus1:
				AssetManager.m_assetManager.m_typogenicText[13].Text = "1";
				AssetManager.m_assetManager.m_uiSprites[2].spriteName = "Icon_Health";
				break;
			case Shop.ShopBonus.HealthBonus2:
				AssetManager.m_assetManager.m_typogenicText[13].Text = "2";
				AssetManager.m_assetManager.m_uiSprites[2].spriteName = "Icon_Health";
				break;
			case Shop.ShopBonus.EnergyBonus1:
				AssetManager.m_assetManager.m_typogenicText[13].Text = "1";
				AssetManager.m_assetManager.m_uiSprites[2].spriteName = "Icon_Energy";
				break;
			case Shop.ShopBonus.EnergyBonus2:
				AssetManager.m_assetManager.m_typogenicText[13].Text = "2";
				AssetManager.m_assetManager.m_uiSprites[2].spriteName = "Icon_Energy";
				break;
			case Shop.ShopBonus.ArmorBonus1:
				AssetManager.m_assetManager.m_typogenicText[13].Text = "1";
				AssetManager.m_assetManager.m_uiSprites[2].spriteName = "Icon_Armor";
				break;
			}




			AssetManager.m_assetManager.m_props[10].SetActive(true);
			m_HUD.SetActive(false);
			PartyCards.m_partyCards.gameObject.SetActive(false);
			AssetManager.m_assetManager.m_props[16].gameObject.SetActive(false);
			m_buttonUI.SetActive(false);
			AssetManager.m_assetManager.m_props[19].SetActive(false);

			m_shopUI.SetActive(true);

			List<Card> validCards = new List<Card>();
			yield return StartCoroutine(InputManager.m_inputManager.GetMouseSelection(validCards));

			AssetManager.m_assetManager.m_props[10].SetActive(false);
			m_HUD.SetActive(true);
			PartyCards.m_partyCards.gameObject.SetActive(true);
			AssetManager.m_assetManager.m_props[16].gameObject.SetActive(true);
			m_buttonUI.SetActive(true);
			AssetManager.m_assetManager.m_props[19].SetActive(true);
			
			m_shopUI.SetActive(false);

			m_currentMenuMode = MenuMode.None;

			if (m_doPortal)
			{
				m_doPortal = false;
				StartCoroutine(MapManager.m_mapManager.NextLevel(2));
			}
		}
		else if (newMode == MenuMode.CharSelect)
		{
//			SetFollowers(new List<Follower>());
//			m_buttonUI.SetActive(false);
//			
//			//get list of followers
//			GameObject[] followerBank = GameManager.m_gameManager.m_followerBank;
//			
//			//create a character card for each
//			int unlockedCharNum = 0;
//			foreach (GameObject thisFollower in followerBank)
//			{
//				Follower fScript = (Follower)thisFollower.GetComponent("Follower");	
//				
//				//get follower progress
//				GameState.ProgressState charProgress = GameManager.m_gameManager.gameProgress[0];
//				foreach (GameState.ProgressState thisCharState in GameManager.m_gameManager.gameProgress)
//				{
//					if (thisCharState.m_followerType == fScript.m_followerType)
//					{
//						charProgress = thisCharState;
//						
//						//Debug
//						//charProgress.m_level = 5;
//						break;
//					}
//				}
//				
//				GameObject fCard = (GameObject)Instantiate(m_charCard, m_charCard.transform.position, m_charCard.transform.rotation);
//				m_cardList.Add(fCard);
//				UICard cardUI = (UICard)fCard.GetComponent("UICard");
//
//				cardUI.m_damageUI.gameObject.SetActive(false);
//				cardUI.m_healthUI.gameObject.SetActive(false);
//				cardUI.m_energyUI.gameObject.SetActive(false);
//				cardUI.m_armorUI.gameObject.SetActive(false);
//				cardUI.m_damageIcon.gameObject.SetActive(false);
//				
//				//show card back if not unlocked
//				if (charProgress.m_isLocked)
//				{
//					cardUI.m_portrait.spriteName = "Card_Back03";
//					cardUI.m_nameUI.gameObject.SetActive(false);
//					cardUI.m_abilityUI.gameObject.SetActive(false);
//					cardUI.m_rankUI.gameObject.SetActive(false);
//					cardUI.m_passive01UI.gameObject.SetActive(false);
//					cardUI.m_passive02UI.gameObject.SetActive(false);
//
//				} else
//				{
//					//change name so unlocked chars show up first in sorting
//					cardUI.name = " " + fScript.m_nameText;
//					unlockedCharNum ++;
//					
//					cardUI.m_nameUI.text = fScript.m_nameText;
//					cardUI.m_abilityUI.text = PartyCards.m_partyCards.UpdateAbilityText(fScript, charProgress.m_level);
//					//cardUI.m_shortCutUI.text = PartyCards.m_partyCards.UpdateShortAbilityText(fScript, charProgress.m_level);
//					cardUI.m_rankUI.text = "Level " + (charProgress.m_level+1).ToString() + " " + fScript.m_followerClass.ToString();
//					
//					cardUI.m_portrait.spriteName = fScript.m_portraitSpriteName;
//					cardUI.m_followerData = fScript;
//					PartyCards.m_partyCards.UpdatePassiveText(cardUI, fScript, charProgress.m_level);
//				}
//				
//				Transform cardParent = m_charSelectUI.transform.Find("InventoryPanel");
//				cardUI.transform.parent = cardParent;
//				cardUI.transform.localPosition = Vector3.zero;
//			}
//			
//			UIGrid grid = (UIGrid) m_charSelectUI.transform.Find("InventoryPanel").GetComponent("UIGrid");
//			grid.Reposition();
//			
//			m_charSelectUI.SetActive(true);
//			
//			List<Card> validCards = new List<Card>();
//			yield return StartCoroutine(InputManager.m_inputManager.GetMouseSelection(validCards));	
//			
//			m_charSelectUI.SetActive(false);
//			m_HUD.SetActive(true);
//			m_buttonUI.SetActive(true);
//			foreach (GameObject card in m_cardList)
//			{
//				Destroy(card);	
//			}
//			m_cardList.Clear();
		}
		else if (newMode == MenuMode.Inventory)
		{

			SpawnInventoryCards(m_inventoryUI.transform, true, 200);
			
			//OLD INVENTORY SYSTEM
			
			//create and parent cards
//			for (int i=0; i < 10; i++)
//			{
//				GameObject fCard = (GameObject)Instantiate(m_itemCard, m_itemCard.transform.position, m_itemCard.transform.rotation);	
//				m_cardList.Add(fCard);
//				UICard cardUI = (UICard)fCard.GetComponent("UICard");
//				if (i < GameManager.m_gameManager.inventory.Count)
//				{
//					Item thisItem = (Item)GameManager.m_gameManager.inventory[i].GetComponent("Item");
//					fCard.name = " " + thisItem.m_name;
//					cardUI.m_nameUI.text = thisItem.m_name;
//					cardUI.m_abilityUI.text = thisItem.m_description;
//					cardUI.m_portrait.spriteName = thisItem.m_portraitSpriteName;
//					cardUI.itemData = thisItem;
//				} else {
//					cardUI.m_nameUI.gameObject.SetActive(false);
//					cardUI.m_abilityUI.gameObject.SetActive(false);
//					cardUI.m_portrait.spriteName = "Card_Back03";
//					cardUI.m_itemData = null;
//				}
//				
//				Transform cardParent = m_inventoryUI.transform.Find("InventoryPanel");
//				cardUI.transform.parent = cardParent;
//				cardUI.transform.localPosition = Vector3.zero;
//			}
			
//			UIGrid grid = (UIGrid) m_inventoryUI.transform.Find("InventoryPanel").GetComponent("UIGrid");
//			grid.Reposition();

			UIManager.m_uiManager.UpdateGoldUI();
			UIManager.m_uiManager.m_XPUI[1].gameObject.SetActive(true);
			m_inventoryUI.SetActive(true);
			yield return new WaitForSeconds(0.01f);
			List<Card> validCards = GameManager.m_gameManager.currentMap.m_cards;
			
			
			yield return StartCoroutine(InputManager.m_inputManager.GetMouseSelection(validCards));	
			
			
			Debug.Log("MENU CLOSED");
			m_inventoryUI.SetActive(false);
			UIManager.m_uiManager.m_XPUI[1].gameObject.SetActive(false);
			foreach (GameObject card in m_cardList)
			{
				Destroy(card);	
			}
			foreach (FuseButton fb in m_fuseButtons)
			{
				Destroy(fb.gameObject);	
			}
			
			m_fuseButtons.Clear();
			m_cardList.Clear();
			m_currentMenuMode = MenuMode.None;

		}
		else if (newMode == MenuMode.Chest)
		{
			AssetManager.m_assetManager.PlaySFX(AssetManager.SFXType.ChestOpened);

//			StartCoroutine(TurnOffTargetCard());

			// turn off fuse buttons
			if (m_fuseButtons.Count > 0)
			{
				foreach (FuseButton fb in m_fuseButtons)
				{
					fb.gameObject.SetActive(false);
				}
			}

			//m_buttonUI.SetActive(false);
			m_chestUI.SetActive(true);	
			
			List<Card> validCards = GameManager.m_gameManager.currentMap.m_cards;
			yield return StartCoroutine(InputManager.m_inputManager.GetMouseSelection(validCards));	
			
			m_chestUI.SetActive(false);
			//m_buttonUI.SetActive(true);

			// turn on fuse buttons
			if (m_fuseButtons.Count > 0)
			{
				foreach (FuseButton fb in m_fuseButtons)
				{
					fb.gameObject.SetActive(true);
				}
			}

			m_currentMenuMode = MenuMode.None;
		}
		else if (newMode == MenuMode.Crafting)
		{
//			
//			//create and parent cards
//			foreach (Item thisItem in GameManager.m_gameManager.inventory)
//			{
//				if (thisItem.HasKeyword(Item.Keyword.Craftable))
//				{
//					GameObject fCard = (GameObject)Instantiate(m_itemCard, m_itemCard.transform.position, m_itemCard.transform.rotation);	
//					m_cardList.Add(fCard);
//					UICard cardUI = (UICard)fCard.GetComponent("UICard");
//					fCard.name = thisItem.m_name;
//					cardUI.m_nameUI.text = thisItem.m_name;
//					cardUI.m_abilityUI.text = thisItem.m_description;
//					cardUI.m_portrait.spriteName = thisItem.m_portraitSpriteName;
//					cardUI.itemData = thisItem;
//					
//					Transform cardParent = m_craftingUI.transform.Find("InventoryPanel");
//					cardUI.transform.parent = cardParent;	
//					cardUI.transform.localPosition = Vector3.zero;
//				}
//			}
//			UIGrid grid = (UIGrid) m_craftingUI.transform.Find("InventoryPanel").GetComponent("UIGrid");
//			grid.Reposition();
//			
//			m_HUD.SetActive(false);
//			m_craftingUI.SetActive(true);	
//			
//			foreach (UICard thisCard in UIManager.m_uiManager.m_craftingSlots)
//			{
//				thisCard.Deactivate();	
//			}
//			
//			yield return new WaitForSeconds(0.01f);
//			List<Card> validCards = GameManager.m_gameManager.currentMap.m_cards;
//			yield return StartCoroutine(InputManager.m_inputManager.GetMouseSelection(validCards));	
//			
//			m_HUD.SetActive(true);
//			m_craftingUI.SetActive(false);
//			foreach (GameObject card in m_cardList)
//			{
//				Destroy(card);	
//			}
//			m_cardList.Clear();
//			m_currentMenuMode = MenuMode.None;
		} else if (newMode == MenuMode.EndLevel || newMode == MenuMode.GameOver)
		{
			StartCoroutine( TurnOffTargetCard());
			StartCoroutine(TurnOffSiteCard());
			if (DHeart.m_dHeart.isBeating)
			{
				DHeart.m_dHeart.StopHeartBeat();	
			}

//			StartCoroutine(TurnOffTargetCard());

			if (newMode != MenuMode.GameOver)
			{
				EffectsPanel.m_effectsPanel.UpdateEffects(EffectsPanel.Effect.Duration.EndOfTurn);
				EffectsPanel.m_effectsPanel.UpdateEffects(EffectsPanel.Effect.Duration.WhilePresent);
				EffectsPanel.m_effectsPanel.UpdateEffects(EffectsPanel.Effect.Duration.EndOfLevel);

				if (Player.m_player.wounds > 0)
				{
					Player.m_player.wounds = 0;
				}
				if (Player.m_player.corruption > 0)
				{
					Player.m_player.corruption = 0;
				}

				// health recovery if enabled
				if (GameManager.m_gameManager.healthRecover > 0 && Player.m_player.currentHealth < Player.m_player.maxHealth)
				{
					Player.m_player.GainHealth(GameManager.m_gameManager.healthRecover);
				}

				// energy recover if enabled
				if (GameManager.m_gameManager.energyRecover > 0 && Player.m_player.currentEnergy < Player.m_player.maxEnergy)
				{
					Player.m_player.GainEnergy(GameManager.m_gameManager.energyRecover);
				}
				
				//update header text
				m_levelEndHeader[0].text = "Floor " + (SettingsManager.m_settingsManager.difficultyLevel + 1).ToString() + " Clear";

				if (SettingsManager.m_settingsManager.trial)
				{
//					// turn off limbo, turn on trial ui elements
//					AssetManager.m_assetManager.m_UIelements[3].gameObject.SetActive(false);
//					AssetManager.m_assetManager.m_UIelements[4].gameObject.SetActive(true);
//
//					// update text
//					string prize = "";
//					if (GameManager.m_gameManager.currentMap.m_trial.m_gpPrize > 0)
//					{
//						prize = "Prize: +" + GameManager.m_gameManager.currentMap.m_trial.m_gpPrize.ToString() + "GP";
//						SettingsManager.m_settingsManager.gold += GameManager.m_gameManager.currentMap.m_trial.m_gpPrize;
//					} else if (GameManager.m_gameManager.currentMap.m_trial.m_xpPrize > 0)
//					{
//						prize = "Prize: +" + GameManager.m_gameManager.currentMap.m_trial.m_xpPrize.ToString() + "XP";
//						GameManager.m_gameManager.accruedXP += GameManager.m_gameManager.currentMap.m_trial.m_xpPrize;
//					}
//					string turns = "Turns Taken: " + GameManager.m_gameManager.currentTurnNum.ToString();
//					AssetManager.m_assetManager.m_labels[2].text = prize;
//					AssetManager.m_assetManager.m_labels[3].text = turns;
//
//					// save completed trial state
//					SettingsManager.m_settingsManager.trialStates[SettingsManager.m_settingsManager.difficultyLevel] = 1;

				} else {

					int bonusxp = 0;
					int xpCards = 0;

					List<Item> discards = new List<Item>();
					for (int i=0; i < GameManager.m_gameManager.inventory.Count; i++)
					{
						Item thisItem = (Item)GameManager.m_gameManager.inventory[i];
//						Debug.Log("LIMBO: " + i.ToString());

						if (thisItem.HasKeyword(Item.Keyword.Limbo))
						{
							Debug.Log("LIMBO CARD FOUND");
							xpCards += ((XPBonus)thisItem).m_xpBonus;
							GameManager.m_gameManager.inventory.RemoveAt(i);
							discards.Add(thisItem);
							if (GameManager.m_gameManager.inventory.Count > 0)
							{
								i = -1;
							} else { i = 999;}
						} else if (thisItem.HasKeyword(Item.Keyword.LostSoul))
						{
							//unlock hero
							for (int j=0; j<GameManager.m_gameManager.gameProgress.Count; j++)
							{
								GameState.ProgressState progress = GameManager.m_gameManager.gameProgress[j];
								if (progress.m_isLocked)
								{
									progress.m_isLocked = false;
									GameManager.m_gameManager.gameProgress[j] = progress;
									j=99;
								}
							}

							// update hero unlocked UI
							AssetManager.m_assetManager.m_labels[11].text = GameManager.m_gameManager.currentMap.m_follower.m_nameText;
							AssetManager.m_assetManager.m_uiSprites[1].spriteName = GameManager.m_gameManager.currentMap.m_follower.m_portraitSpriteName;
							AssetManager.m_assetManager.m_props[23].SetActive(true);

							string newString = "\\6" + GameManager.m_gameManager.currentMap.m_follower.m_nameText + "'s Soul has been recovered!";
							UIManager.m_uiManager.UpdateActions (newString);

							// get rid of item
							GameManager.m_gameManager.inventory.RemoveAt(i);
							discards.Add(thisItem);
							if (GameManager.m_gameManager.inventory.Count > 0)
							{
								i = 0;
							} else { i = 999;}
						}

					}
				
					if (discards.Count > 0)
					{
						while (discards.Count > 0)
						{
							Item d = (Item)discards[0];
							discards.RemoveAt(0);
							Destroy(d.gameObject);
						}
					}
				

					if (GameManager.m_gameManager.numTilesFlipped == (GameManager.m_gameManager.numTiles-1) && GameManager.m_gameManager.numTiles > 0)
					{

						bonusxp = (int)Mathf.Clamp(((GameManager.m_gameManager.accruedXP + xpCards) * 0.1f),1, 999);

						Debug.Log("ALL TILES FLIPPED : " + bonusxp);
					}



					int totalXP = GameManager.m_gameManager.accruedXP + bonusxp + xpCards;

					AssetManager.m_assetManager.m_labels[8].text = "+" + bonusxp.ToString();
					AssetManager.m_assetManager.m_labels[9].text = "+" + xpCards.ToString();
					AssetManager.m_assetManager.m_labels[10].text = "+" + totalXP.ToString();
					SettingsManager.m_settingsManager.xp += totalXP;
					AssetManager.m_assetManager.m_labels[5].text = "+" + GameManager.m_gameManager.accruedXP.ToString();
					//AssetManager.m_assetManager.m_labels[4].text = SettingsManager.m_settingsManager.xp.ToString();

				}
			} else {

				int ascendBonusXP = 0;
				int bonusxp = 0;
				if (oldMode == MenuMode.Pause)
				{
					m_pauseUI.SetActive(false);
					AssetManager.m_assetManager.m_labels[7].text = "Ascend Bonus";
					ascendBonusXP += GameManager.m_gameManager.levelsCompletedBonus;
				}

				if (GameManager.m_gameManager.numTilesFlipped == (GameManager.m_gameManager.numTiles-1) && GameManager.m_gameManager.numTiles > 0)
				{
					
					bonusxp = (int)Mathf.Clamp(((GameManager.m_gameManager.accruedXP + ascendBonusXP) * 0.1f),1, 999);
					
					Debug.Log("ALL TILES FLIPPED : " + bonusxp);
				}

				int totalXP = GameManager.m_gameManager.accruedXP + ascendBonusXP + bonusxp;
				
				AssetManager.m_assetManager.m_labels[8].text = "+" + bonusxp.ToString();
				AssetManager.m_assetManager.m_labels[9].text = "+" + ascendBonusXP.ToString();
				AssetManager.m_assetManager.m_labels[10].text = "+" + totalXP.ToString();
				SettingsManager.m_settingsManager.xp += totalXP;
				AssetManager.m_assetManager.m_labels[5].text = "+" + GameManager.m_gameManager.accruedXP.ToString();

				//m_levelEndHeader[0].text = "Game Over";
				AssetManager.m_assetManager.m_typogenicText[16].Text = "MENU";
				AssetManager.m_assetManager.m_UIelements[2].SetActive(false);
				AssetManager.m_assetManager.m_props[32].SetActive(true);
			}

			int accruedGP = 0;

			//unlock any locked followers
			int unlockSlot = 0;
			List<Follower> fList = new List<Follower>();
			List<int> startingXP = new List<int>();
			List<int> endingXP = new List<int>();
			List<int> levelUpXP = new List<int>();

//			foreach (Follower f in GameManager.m_gameManager.lostSouls)
//			{
//				fList.Add(f);
//			}
//			foreach (Follower thisFollower in GameManager.m_gameManager.followers)
//			{
//				if (thisFollower.isLocked)
//				{
					//fList.Add(thisFollower);

					//thisFollower.isLocked = false;
//					UICard cardUI = m_charUnlockSlots[unlockSlot];
//					cardUI.m_followerData = thisFollower;
//					cardUI.m_nameUI.text = thisFollower.m_nameText;
//					cardUI.m_portrait.spriteName = thisFollower.m_portraitSpriteName;
//					cardUI.m_abilityUI.text = thisFollower.m_abilityText;
//					cardUI.m_miscText[0].gameObject.SetActive(true);
//					cardUI.gameObject.SetActive(true);
//					
//					cardUI.m_rankUI.text = "Level " + (thisFollower.currentLevel+1).ToString() + " " + thisFollower.m_followerClass.ToString();
//					cardUI.m_rankUI.gameObject.SetActive(true);
//					
//					cardUI.m_abilityUI.text = PartyCards.m_partyCards.UpdateAbilityText(thisFollower);
//					//cardUI.m_shortCutUI.text = PartyCards.m_partyCards.UpdateShortAbilityText(thisFollower);
//
//					cardUI.m_armorUI.text = thisFollower.m_abilityCost.ToString();
//					cardUI.m_damageIcon.spriteName = "Icon_Energy";
//					cardUI.m_miscOBJ[0].gameObject.SetActive(true);
//
//					PartyCards.m_partyCards.UpdatePassiveText(cardUI, thisFollower, thisFollower.currentLevel);
//					cardUI.m_passive01UI.gameObject.SetActive(true);
//					cardUI.m_passive02UI.gameObject.SetActive(true);
					
//				} else {
//					startingXP.Add(thisFollower.currentXP);
//					levelUpXP.Add (thisFollower.maxXP);
//
//					int startLevel = thisFollower.currentLevel;
//					UICard cardUI = m_charUnlockSlots[unlockSlot];
//					cardUI.m_followerData = thisFollower;
//					cardUI.m_nameUI.text = thisFollower.m_nameText;
//					cardUI.m_portrait.spriteName = thisFollower.m_portraitSpriteName;
//
//					int modifiedXP = GameManager.m_gameManager.accruedXP;
//					cardUI.m_energyUI.text = "XP Gain: +" + modifiedXP.ToString();
//
//					if (thisFollower.XPBonus > 1.0f)
//					{
//						float mXP = ((float)modifiedXP) * thisFollower.XPBonus;
//						modifiedXP = ((int)mXP);
//						cardUI.m_energyUI.color = Color.green;
//						cardUI.m_energyUI.text = "XP Gain: +" + modifiedXP.ToString() + " ( x" + thisFollower.XPBonus.ToString() +" )";
//					}
//					endingXP.Add(thisFollower.currentXP + modifiedXP);
//					cardUI.m_energyUI.gameObject.SetActive(true);
//
//					cardUI.m_healthUI.text = "XP: " + thisFollower.currentXP.ToString() + " / " + thisFollower.maxXP.ToString();
//					cardUI.m_healthUI.gameObject.SetActive(true);
//
//					cardUI.m_rankUI.text = "Level " + (thisFollower.currentLevel+1).ToString() + " " + thisFollower.m_followerClass.ToString();
//					cardUI.m_rankUI.gameObject.SetActive(true);
//					
//					cardUI.m_abilityUI.text = PartyCards.m_partyCards.UpdateAbilityText(thisFollower);
//
//					cardUI.m_armorUI.text = thisFollower.m_abilityCost.ToString();
//					cardUI.m_damageIcon.spriteName = "Icon_Energy";
//					cardUI.m_miscOBJ[0].gameObject.SetActive(true);
//					
//					PartyCards.m_partyCards.UpdatePassiveText(cardUI, thisFollower, thisFollower.currentLevel);
//					cardUI.m_passive01UI.gameObject.SetActive(true);
//					cardUI.m_passive02UI.gameObject.SetActive(true);
//					
//					
//					
//					cardUI.gameObject.SetActive(true);
//
//					thisFollower.GainXP(modifiedXP);
//				}
				
//				unlockSlot++;
				
//				//update progressState
//				for (int i=0; i<GameManager.m_gameManager.gameProgress.Count; i++)
//				{
//					GameState.ProgressState progress = GameManager.m_gameManager.gameProgress[i];
//					if (progress.m_followerType == thisFollower.m_followerType)
//					{
//						progress.m_isLocked = false;
//						progress.m_level = thisFollower.currentLevel;
//						progress.m_XP = thisFollower.currentXP;
//						GameManager.m_gameManager.gameProgress[i] = progress;
//						i=99;
//					}
//				}
//			}


			//check for XP && GP bonus cards in player's inventory and add to accrued XP and GP
			if (GameManager.m_gameManager.inventory.Count > 0)
			{
				List<Item> bList = new List<Item>();
				List<Item> lsList = new List<Item>();
				int xpItems = 0;
				int gpItems = 0;
				int lsItems = 0;
				foreach (Item thisItem in GameManager.m_gameManager.inventory)
				{
//					if (thisItem.m_XPBonus > 0 || thisItem.m_goldBonus > 0)
//					{
//						bList.Add(thisItem);
//					}
//					if (thisItem.m_XPBonus > 0)
//					{
//						//						GameManager.m_gameManager.accruedXP += thisItem.m_XPBonus;
//						xpItems ++;
//					}
//					if (thisItem.m_goldBonus > 0)
//					{
//						//						SettingsManager.m_settingsManager.gold += thisItem.m_goldBonus;
//						//						accruedGP += thisItem.m_goldBonus;
//						gpItems ++;
//					}
//
//					if (thisItem.HasKeyword(Item.Keyword.Limbo) && thisItem.m_XPBonus == 0 && thisItem.m_goldBonus == 0)
//					{
//						lsItems ++;
//						lsList.Add(thisItem);
//					}
				}
				
				while (xpItems > 0)
				{
					for (int i = 0; i < GameManager.m_gameManager.inventory.Count; i++)
					{
						Item invItem = (Item)GameManager.m_gameManager.inventory[i];
						
//						if (invItem.m_XPBonus > 0)
//						{
//							GameManager.m_gameManager.inventory.RemoveAt(i);
//							xpItems --;	
//							break;
//						}
					}
					
					//xpItems = 0;
				}
				
				while (gpItems > 0)
				{
					for (int i = 0; i < GameManager.m_gameManager.inventory.Count; i++)
					{
						Item invItem = (Item)GameManager.m_gameManager.inventory[i];
						
//						if (invItem.m_goldBonus > 0)
//						{
//							GameManager.m_gameManager.inventory.RemoveAt(i);
//							gpItems --;	
//							break;
//						}
					}
					
					//gpItems = 0;
				}

				while (lsItems > 0)
				{
					for (int i = 0; i < GameManager.m_gameManager.inventory.Count; i++)
					{
						Item invItem = (Item)GameManager.m_gameManager.inventory[i];
						
//						if (invItem.HasKeyword(Item.Keyword.Limbo) && invItem.m_XPBonus == 0 && invItem.m_goldBonus == 0)
//						{
//							GameManager.m_gameManager.inventory.RemoveAt(i);
//							Destroy(invItem.gameObject);
//							lsItems --;	
//							break;
//						}
					}
					
					//gpItems = 0;
				}
				
				GameManager.m_gameManager.bonusItems.AddRange(bList);
				if (bList.Count > 0)
				{
					RefreshInventoryMenu();
				}
			}
			
			// display all collected bonus cards
			Vector3 startPos =  AssetManager.m_assetManager.m_props[14].transform.position;
			Vector3 endPos = AssetManager.m_assetManager.m_props[15].transform.position;

			GameObject lastCard = null;
			List<UICard> cList = new List<UICard>();
			int numCards = Mathf.Clamp((GameManager.m_gameManager.bonusItems.Count + fList.Count)-1, 1, 99);
			
			for (int i=0; i < (GameManager.m_gameManager.bonusItems.Count + fList.Count) ; i++)
			{

				Vector3 cardPos = Vector3.Lerp(startPos, endPos, ((float)i) / numCards);
				//			float randPos = Random.Range(-0.01f, 0.01f);
				//			cardPos.y += randPos;
				GameObject fCard = (GameObject)Instantiate(m_cardSmall, cardPos, m_cardSmall.transform.rotation);
				if (lastCard != null)
				{
					if (fCard.transform.position.x - lastCard.transform.position.x > 0.2407f)
					{
						Vector3 newPos = lastCard.transform.position;
						newPos.x += 0.2407f;
						fCard.transform.position = newPos;
					}
				}

				UICard cardUI = (UICard)fCard.GetComponent("UICard");
				cList.Add(cardUI);

				if (i < fList.Count)
				{
					Follower f = (Follower)fList[i];
					cardUI.SetCard(f,true);
				} else {
					Item thisItem = (Item)GameManager.m_gameManager.bonusItems[i-fList.Count].GetComponent("Item");
					cardUI.SetCard(thisItem,true);
				}
				lastCard = fCard;
				fCard.transform.parent = UIManager.m_uiManager.m_levelEndUI.transform;
			}

			AssetManager.m_assetManager.m_props[10].SetActive(true);
			m_levelEndUI.SetActive(true);
			m_HUD.SetActive(false);
			m_buttonUI.SetActive(false);
			AssetManager.m_assetManager.m_props[19].SetActive(false);
			PartyCards.m_partyCards.gameObject.SetActive(false);
			AssetManager.m_assetManager.m_props[16].gameObject.SetActive(false);
			AssetManager.m_assetManager.m_props[17].gameObject.SetActive(false);
			AssetManager.m_assetManager.m_props[38].gameObject.SetActive(false);
			AssetManager.m_assetManager.m_props[39].gameObject.SetActive(false);
			AssetManager.m_assetManager.m_props[42].gameObject.SetActive(false);

			UIManager.m_uiManager.UpdateGoldUI();
			AssetManager.m_assetManager.m_UIelements[5].gameObject.SetActive(true);

			if (accruedGP > 0)
			{

				UIManager.m_uiManager.m_XPUI[2].text = "+ " + accruedGP.ToString();
				UIManager.m_uiManager.m_XPUI[2].gameObject.SetActive(true);

			}

			RefreshInventoryMenu();

//			Vector3 startScale = m_charUnlockSlots[0].m_healthUI.transform.localScale;
//			foreach (UICard fC in m_charUnlockSlots)
//			{
//				fC.m_healthUI.transform.localScale = startScale * 2;
//			}
//
//
//			//play xp ticker
//			float timer = 0;
//			bool xpDone = false;
//			while (!xpDone)
//			{
//				timer += Time.deltaTime;
//				if (timer >= 0.03f)
//				{
//					timer = 0;
//					for (int i=0; i < m_charUnlockSlots.Length; i++)
//					{
//						UICard c = (UICard)m_charUnlockSlots[i];
//						if (c.m_followerData != null)
//						{
//							if (startingXP[i] < endingXP[i])
//							{
//								startingXP[i] += 1;
//								c.m_healthUI.text = "XP: " + startingXP[i].ToString() + " / " + levelUpXP[i].ToString();
//
//								//level up hero
//								if (startingXP[i] == levelUpXP[i])
//								{
//									c.m_damageUI.gameObject.SetActive(true);
//									Player.m_player.SetPassiveFollowerBonuses(GameManager.m_gameManager.followers);
//									PartyCards.m_partyCards.UpdateCard(c.m_followerData);
//									c.m_rankUI.text = "Level " + (c.m_followerData.currentLevel+1).ToString() + " " + c.m_followerData.m_followerClass.ToString();
//									c.m_abilityUI.text = PartyCards.m_partyCards.UpdateAbilityText(c.m_followerData);
//									c.m_armorUI.text = c.m_followerData.m_abilityCost.ToString();
//									PartyCards.m_partyCards.UpdatePassiveText(c, c.m_followerData, c.m_followerData.currentLevel);
//									c.m_passive01UI.gameObject.SetActive(true);
//									c.m_passive02UI.gameObject.SetActive(true);
//
//									// update numbers in ticker
//									levelUpXP[i] = c.m_followerData.maxXP;
//									int t = startingXP[i];
//									startingXP[i] -= t;
//									endingXP[i] -= t;
//								}
//							} 
//						}
//					}
//					
//					xpDone = true;
//					for (int j=0; j < startingXP.Count; j++)
//					{
//						if (startingXP[j] < endingXP[j])
//						{
//							xpDone = false;
//							j = 100;
//						}
//					}
//				}
//				yield return null;
//			}

//			foreach (UICard fC in m_charUnlockSlots)
//			{
//				fC.m_healthUI.transform.localScale = startScale;
//			}

			if (newMode == MenuMode.GameOver)
			{
				// lose any cards / heroes in Limbo
				foreach (UICard c in cList)
				{
					GameObject newDT = (GameObject)Instantiate(m_UIdamageText, Vector3.zero, m_damageText.transform.rotation);
					UILabel dtText = (UILabel)newDT.GetComponentInChildren<UILabel>();
					if (c.m_followerData != null)
					{
						dtText.text = "Hero Lost!";
						dtText.color = Color.red;
					} else if (c.m_itemData != null)
					{
						dtText.text = "Card Lost!";
						dtText.color = Color.red;

					}
					
					newDT.transform.position = c.transform.position;
					newDT.animation.Stop ();
					newDT.animation.Play ("TextFloat");
					
					yield return new WaitForSeconds(0.35f);
					c.gameObject.SetActive(false);
				}
			}

			List<Card> validCards = new List<Card>();
			yield return StartCoroutine(InputManager.m_inputManager.GetMouseSelection(validCards));	

			if (m_leaveDungeon)
			{   
				bool t = false;
				if (cList.Count > 0){t=true;}
				foreach (UICard c in cList)
				{
					GameObject newDT = (GameObject)Instantiate(m_UIdamageText, Vector3.zero, m_damageText.transform.rotation);
					UILabel dtText = (UILabel)newDT.GetComponentInChildren<UILabel>();
					if (c.m_followerData != null)
					{
						dtText.text = "Hero Unlocked!";
					} else if (c.m_itemData != null)
					{
//						if (c.itemData.m_XPBonus > 0)
//						{
//							dtText.text = "+" + c.itemData.m_XPBonus.ToString() + "XP";
//
//							int cSlot = 0;
//							foreach (Follower thisFollower in GameManager.m_gameManager.followers)
//							{
//								UICard cardUI = (UICard)m_charUnlockSlots[cSlot];
//								cSlot ++;
//								int startLevel = thisFollower.currentLevel;
//								thisFollower.GainXP(c.itemData.m_XPBonus);
//								
//								if (startLevel != thisFollower.currentLevel)
//								{
//									//character leveled up
//									cardUI.m_damageUI.gameObject.SetActive(true);
//									Player.m_player.SetPassiveFollowerBonuses(GameManager.m_gameManager.followers);
//									PartyCards.m_partyCards.UpdateCard(thisFollower);
//
//									//update card in level end menu w new stats
//									cardUI.m_rankUI.text = "Level " + (thisFollower.currentLevel+1).ToString() + " " + thisFollower.m_followerClass.ToString();
//									cardUI.m_abilityUI.text = PartyCards.m_partyCards.UpdateAbilityText(thisFollower);
//									cardUI.m_armorUI.text = thisFollower.m_abilityCost.ToString();
//									cardUI.m_damageIcon.spriteName = "Icon_Energy";
//									PartyCards.m_partyCards.UpdatePassiveText(cardUI, thisFollower, thisFollower.currentLevel);
//									cardUI.m_passive01UI.gameObject.SetActive(true);
//									cardUI.m_passive02UI.gameObject.SetActive(true);
//								}
//
//								cardUI.m_healthUI.text = "XP: " + thisFollower.currentXP.ToString() + " / " + thisFollower.maxXP.ToString();
//							}
//						}

//						if (c.itemData.m_goldBonus > 0)
//						{
//							dtText.text = "+" + c.itemData.m_goldBonus.ToString() + "^";
//							SettingsManager.m_settingsManager.gold += c.itemData.m_goldBonus;
//							UpdateGoldUI();
//						}
					}

					newDT.transform.position = c.transform.position;
					newDT.animation.Stop ();
					newDT.animation.Play ("TextFloat");

					yield return new WaitForSeconds(0.25f);
					c.gameObject.SetActive(false);
				}

				if (t){yield return new WaitForSeconds(3.0f);}


				m_leaveDungeon = false;
//				foreach (Follower thisFollower in GameManager.m_gameManager.followers)
//				{
//					//update progressState
//					for (int i=0; i<GameManager.m_gameManager.gameProgress.Count; i++)
//					{
//						GameState.ProgressState progress = GameManager.m_gameManager.gameProgress[i];
//						if (progress.m_followerType == thisFollower.m_followerType)
//						{
//							progress.m_isLocked = false;
//							progress.m_level = thisFollower.currentLevel;
//							progress.m_XP = thisFollower.currentXP;
//							progress.m_XPBonus = 1.0f;
//							GameManager.m_gameManager.gameProgress[i] = progress;
//							i=99;
//						}
//					}
//				}

				for (int i=0; i<GameManager.m_gameManager.gameProgress.Count; i++)
				{
					GameState.ProgressState pState = GameManager.m_gameManager.gameProgress[i];
					bool inParty = false;
					foreach (Follower thisF in GameManager.m_gameManager.followers)
					{
						if (pState.m_followerType == thisF.m_followerType)
						{
							inParty = true;
							pState.m_isLocked = false;
							pState.m_level = thisF.currentLevel;
							pState.m_XP = thisF.currentXP;
							pState.m_XPBonus = 1.0f;
							GameManager.m_gameManager.gameProgress[i] = pState;
							break;
						}
					}

					if (!inParty && !pState.m_isLocked)
					{
						//cancel out first few levels
						int levelsTravelled = Mathf.Clamp(SettingsManager.m_settingsManager.levelsTravelled - 3, 0, 99);

						if (levelsTravelled > 0)
						{
							pState.m_XPBonus = Mathf.Clamp(pState.m_XPBonus + (((float)levelsTravelled) / 100.0f), 1.0f, 2.0f);
							GameManager.m_gameManager.gameProgress[i] = pState;
						}
					}
				}

				SettingsManager.m_settingsManager.levelsTravelled = 0;
				GameManager.m_gameManager.gameState.saveState();
				SettingsManager.m_settingsManager.difficultyLevel = 0;
				if (SettingsManager.m_settingsManager.trial)
				{
					Application.LoadLevel("MainMenu01");
				} else 
				{
					//Application.LoadLevel("PartySelect01");
					Application.LoadLevel("MainMenu01");
				}
			}

			AssetManager.m_assetManager.m_uiSprites[0].gameObject.SetActive (true);
			yield return new WaitForSeconds(0.1f);
//			AssetManager.m_assetManager.m_uiSprites[0].color = Color.white;
//			AssetManager.m_assetManager.m_uiSprites [0].transform.localPosition = Vector3.zero;

//			foreach (UICard charCard in m_charUnlockSlots)
//			{
//				charCard.m_followerData = null;
//				charCard.m_miscText[0].gameObject.SetActive(false);
//				//charCard.m_armorUI.gameObject.SetActive(false);
//				charCard.m_energyUI.gameObject.SetActive(false);
//				charCard.m_healthUI.gameObject.SetActive(false);
//				charCard.m_damageUI.gameObject.SetActive(false);
//				charCard.gameObject.SetActive(false);	
//			}

			foreach (UICard card in cList)
			{
				Destroy(card.gameObject);
			}
			
			GameManager.m_gameManager.accruedXP = 0;
			UIManager.m_uiManager.UpdateXP(GameManager.m_gameManager.accruedXP);
			AssetManager.m_assetManager.m_UIelements[5].gameObject.SetActive(false);
			AssetManager.m_assetManager.m_props[23].SetActive(false);
			
			if (SettingsManager.m_settingsManager.difficultyLevel == SettingsManager.m_settingsManager.demoEnd && SettingsManager.m_settingsManager.demoEnd != 0)
			{
				//game completed
				Debug.Log("YOU BEAT THE DEMO");
				
				//save unlocked followers
				foreach (Follower thisFollower in GameManager.m_gameManager.followers)
				{
					//update progressState
					for (int i=0; i<GameManager.m_gameManager.gameProgress.Count; i++)
					{
						GameState.ProgressState progress = GameManager.m_gameManager.gameProgress[i];
						if (progress.m_followerType == thisFollower.m_followerType)
						{
							progress.m_isLocked = false;
							progress.m_level = thisFollower.currentLevel;
							progress.m_XP = thisFollower.currentXP;
							GameManager.m_gameManager.gameProgress[i] = progress;
							i=99;
						}
					}
				}
				
				GameManager.m_gameManager.gameState.saveState();
				
				SettingsManager.m_settingsManager.sceneNum = 2;
				Application.LoadLevel("DialogueScene01");
			}
			else if (SettingsManager.m_settingsManager.difficultyLevel == 40)
			{
				//game completed
				Debug.Log("YOU BEAT THE GAME");
				
				//save unlocked followers
				foreach (Follower thisFollower in GameManager.m_gameManager.followers)
				{
					//update progressState
					for (int i=0; i<GameManager.m_gameManager.gameProgress.Count; i++)
					{
						GameState.ProgressState progress = GameManager.m_gameManager.gameProgress[i];
						if (progress.m_followerType == thisFollower.m_followerType)
						{
							progress.m_isLocked = false;
							progress.m_level = thisFollower.currentLevel;
							progress.m_XP = thisFollower.currentXP;
							GameManager.m_gameManager.gameProgress[i] = progress;
							i=99;
						}
					}
				}
				
				GameManager.m_gameManager.gameState.saveState();
				
				SettingsManager.m_settingsManager.sceneNum = 1;
				Application.LoadLevel("DialogueScene01");
			} else {
				
//				Debug.Log("RESETTING PASSIVE BONUSES");
//				Player.m_player.SetPassiveFollowerBonuses(GameManager.m_gameManager.followers);

				// turn on fuse buttons
				if (m_fuseButtons.Count > 0)
				{
					foreach (FuseButton fb in m_fuseButtons)
					{
						fb.gameObject.SetActive(true);
					}
				}

				m_levelEndUI.SetActive(false);
				m_HUD.SetActive(true);
				m_buttonUI.SetActive(true);
				AssetManager.m_assetManager.m_props[19].SetActive(true);
				PartyCards.m_partyCards.gameObject.SetActive(true);
				AssetManager.m_assetManager.m_props[16].gameObject.SetActive(true);
				AssetManager.m_assetManager.m_props[17].gameObject.SetActive(true);
				AssetManager.m_assetManager.m_props[38].gameObject.SetActive(true);
				AssetManager.m_assetManager.m_props[39].gameObject.SetActive(true);
				AssetManager.m_assetManager.m_props[42].gameObject.SetActive(true);
				AssetManager.m_assetManager.m_props[10].SetActive(false);
//				UIManager.m_uiManager.m_XPUI[1].gameObject.SetActive(false);
				UIManager.m_uiManager.m_XPUI[2].gameObject.SetActive(false);
				yield return StartCoroutine(MapManager.m_mapManager.NextLevel(1));


//				AssetManager.m_assetManager.m_uiSprites [0].gameObject.SetActive (false);

				m_currentMenuMode = MenuMode.None;

			}
			
		} else if (newMode == MenuMode.FollowerSwap)
		{

			Follower thisFollower = (Follower)InputManager.m_inputManager.selectedObject.transform.GetComponent("Follower");
			//GameObject fCard = (GameObject)Instantiate(m_charCard, m_charCard.transform.position, m_charCard.transform.rotation);	
			GameObject fCard = (GameObject)Instantiate(m_itemCard, m_itemCard.transform.position, m_itemCard.transform.rotation);	
			m_cardList.Add(fCard);
			UICard cardUI = (UICard)fCard.GetComponent("UICard");
			cardUI.m_nameUI.text = thisFollower.m_nameText;
			cardUI.m_portrait.spriteName = thisFollower.m_portraitSpriteName;
			cardUI.m_abilityUI.text = PartyCards.m_partyCards.UpdateAbilityText(thisFollower);
			cardUI.m_rankUI.text =  "Level " + (thisFollower.currentLevel+1).ToString() + " " + thisFollower.m_followerClass.ToString();
			cardUI.m_healthIcon.gameObject.SetActive(true);
			cardUI.m_followerData = thisFollower;

			cardUI.m_healthUI.text = thisFollower.m_abilityCost.ToString();
			cardUI.m_damageIcon.spriteName = "Icon_Energy";
			cardUI.m_miscOBJ[0].gameObject.SetActive(true);

//			PartyCards.m_partyCards.UpdatePassiveText(cardUI, thisFollower, thisFollower.currentLevel);
//			cardUI.m_damageUI.gameObject.SetActive(false);
//			cardUI.m_healthUI.gameObject.SetActive(false);
//			cardUI.m_energyUI.gameObject.SetActive(false);
//			cardUI.m_armorUI.gameObject.SetActive(false);
//			cardUI.m_damageIcon.gameObject.SetActive(false);
			
			Transform cardParent = m_followerSwapUI.transform.Find("InventoryPanel");
			cardUI.transform.parent = cardParent;	
			cardUI.transform.localPosition = Vector3.zero;
			
			UIGrid grid = (UIGrid) m_followerSwapUI.transform.Find("InventoryPanel").GetComponent("UIGrid");
			grid.Reposition();

			// turn off fuse buttons
			if (m_fuseButtons.Count > 0)
			{
				foreach (FuseButton fb in m_fuseButtons)
				{
					fb.gameObject.SetActive(false);
				}
			}

			m_followerSwapUI.SetActive(true);
			
			List<Card> validCards = GameManager.m_gameManager.currentMap.m_cards;
			yield return StartCoroutine(InputManager.m_inputManager.GetMouseSelection(validCards));	

			// turn on fuse buttons
			if (m_fuseButtons.Count > 0)
			{
				foreach (FuseButton fb in m_fuseButtons)
				{
					fb.gameObject.SetActive(true);
				}
			}
			
			m_followerSwapUI.SetActive(false);
//			foreach (GameObject card in m_cardList)
//			{
//				Destroy(card);	
//			}
//			m_cardList.Clear();
			m_currentMenuMode = MenuMode.None;
		} 
//		else if (newMode == MenuMode.GameOver)
//		{
//			AssetManager.m_assetManager.m_props[10].SetActive(true);
//			m_gameOverUI.SetActive(true);
//			m_HUD.SetActive(false);
//			m_buttonUI.SetActive(false);
//			
//			List<Card> validCards = GameManager.m_gameManager.currentMap.m_cards;
//			yield return StartCoroutine(InputManager.m_inputManager.GetMouseSelection(validCards));
//			
//		} 
		else if (newMode == MenuMode.Storage)
		{
			//m_HUD.SetActive(false);
			m_buttonUI.SetActive(false);

			// turn off fuse buttons
			if (m_fuseButtons.Count > 0)
			{
				foreach (FuseButton fb in m_fuseButtons)
				{
					fb.gameObject.SetActive(false);
				}
			}
			
			// activate # of storage slots based on badge state
			for (int j=0; j < GameManager.m_gameManager.maxStorage; j++)
			{
				m_storageSlots[j].gameObject.SetActive(true);
			}

			//update storage slot cards with current storage items
			for (int i=0; i < GameManager.m_gameManager.storageItems.Count; i++)
			{
				Item storageItem = GameManager.m_gameManager.storageItems[i];
				//UIManager.m_uiManager.SetStorageSlot(storageItem);
				m_storageSlots[i].SetCard(storageItem, false);
			}

			m_storageUI.SetActive(true);
			m_buttonUI.SetActive(false);
			PartyCards.m_partyCards.gameObject.SetActive(false);
			AssetManager.m_assetManager.m_props[17].gameObject.SetActive(false);

			List<Card> validCards = GameManager.m_gameManager.currentMap.m_cards;
			yield return StartCoroutine(InputManager.m_inputManager.GetMouseSelection(validCards));	
			GameManager.m_gameManager.gameState.SaveStorageState();
			
			foreach (UICard thisCard in m_storageSlots)
			{
				thisCard.Deactivate();
				thisCard.itemData = null;
			}
			//m_HUD.SetActive(true);
//			if (DHeart.m_dHeart.isBeating)
//			{
//				DHeart.m_dHeart.StartHeartBeat();
//			}
			m_storageUI.SetActive(false);
			m_buttonUI.SetActive(true);
			PartyCards.m_partyCards.gameObject.SetActive(true);
			AssetManager.m_assetManager.m_props[17].gameObject.SetActive(true);

//			foreach (GameObject card in m_cardList)
//			{
//				Destroy(card);	
//			}
//			m_cardList.Clear();

			// turn on fuse buttons
			if (m_fuseButtons.Count > 0)
			{
				foreach (FuseButton fb in m_fuseButtons)
				{
					fb.gameObject.SetActive(true);
				}
			}
			
			m_currentMenuMode = MenuMode.None;
			
		} else if (newMode == MenuMode.Pause)
		{

			int unlockSlot = 0;
//			foreach (Follower thisFollower in GameManager.m_gameManager.followers)
//			{
//
//				if (thisFollower.isLocked)
//				{
//					UICard cardUI = m_pauseMenuSlots[unlockSlot];
//					cardUI.m_followerData = thisFollower;
//					cardUI.m_nameUI.text = thisFollower.m_nameText;
//					cardUI.m_portrait.spriteName = thisFollower.m_portraitSpriteName;
//					cardUI.m_miscText[0].gameObject.SetActive(true);
//					cardUI.m_damageUI.text = thisFollower.m_abilityCost.ToString();
//					cardUI.m_damageIcon.spriteName = "Icon_Energy";
//					cardUI.m_miscOBJ[0].gameObject.SetActive(true);
//					cardUI.gameObject.SetActive(true);
//
//					
//					cardUI.m_rankUI.text = "Level " + (thisFollower.currentLevel+1).ToString() + " " + thisFollower.m_followerClass.ToString();
//					cardUI.m_rankUI.gameObject.SetActive(true);
//					
//					cardUI.m_abilityUI.text = PartyCards.m_partyCards.UpdateAbilityText(thisFollower);
//					
//					PartyCards.m_partyCards.UpdatePassiveText(cardUI, thisFollower, thisFollower.currentLevel);
//					cardUI.m_passive01UI.gameObject.SetActive(true);
//					cardUI.m_passive02UI.gameObject.SetActive(true);
//					
//				} else {
////					int startLevel = thisFollower.currentLevel;
//					UICard cardUI = m_pauseMenuSlots[unlockSlot];
//					cardUI.m_followerData = thisFollower;
//					cardUI.m_nameUI.text = thisFollower.m_nameText;
//					cardUI.m_portrait.spriteName = thisFollower.m_portraitSpriteName;
//					cardUI.m_damageUI.text = thisFollower.m_abilityCost.ToString();
//					cardUI.m_damageIcon.spriteName = "Icon_Energy";
//					cardUI.m_miscOBJ[0].gameObject.SetActive(true);
////					
////					cardUI.m_energyUI.text = "XP Gain: +" + GameManager.m_gameManager.accruedXP.ToString();
////					cardUI.m_energyUI.gameObject.SetActive(true);
////				
//					cardUI.m_healthUI.text = "XP: " + thisFollower.currentXP.ToString() + " / " + thisFollower.maxXP.ToString();
//					cardUI.m_healthUI.gameObject.SetActive(true);
//					
//					cardUI.m_rankUI.text = "Level " + (thisFollower.currentLevel+1).ToString() + " " + thisFollower.m_followerClass.ToString();
//					cardUI.m_rankUI.gameObject.SetActive(true);
//					
//					cardUI.m_abilityUI.text = PartyCards.m_partyCards.UpdateAbilityText(thisFollower);
//					
//					PartyCards.m_partyCards.UpdatePassiveText(cardUI, thisFollower, thisFollower.currentLevel);
//					cardUI.m_passive01UI.gameObject.SetActive(true);
//					cardUI.m_passive02UI.gameObject.SetActive(true);
//					
//					cardUI.gameObject.SetActive(true);
//				}
//				
//				unlockSlot++;
//			}

			if (GameManager.m_gameManager.levelsCompletedBonus > 0)
			{
				AssetManager.m_assetManager.m_typogenicText[17].Text = GameManager.m_gameManager.levelsCompletedBonus.ToString("D2");
				AssetManager.m_assetManager.m_props[36].SetActive(true);
			}

			AssetManager.m_assetManager.m_props[10].SetActive(true);
			m_HUD.SetActive(false);
			PartyCards.m_partyCards.gameObject.SetActive(false);
			AssetManager.m_assetManager.m_props[16].gameObject.SetActive(false);
			AssetManager.m_assetManager.m_props[17].gameObject.SetActive(false);
			AssetManager.m_assetManager.m_props[38].gameObject.SetActive(false);
			AssetManager.m_assetManager.m_props[39].gameObject.SetActive(false);
			AssetManager.m_assetManager.m_props[42].gameObject.SetActive(false);
			AssetManager.m_assetManager.m_UIelements[6].gameObject.SetActive(false);
			m_buttonUI.SetActive(false);
			m_pauseUI.SetActive(true);
			AssetManager.m_assetManager.m_props[19].SetActive(false);
			UIManager.m_uiManager.UpdateGoldUI();
			AssetManager.m_assetManager.m_UIelements[5].gameObject.SetActive(true);

			yield return new WaitForSeconds(0.01f);
			Time.timeScale = 0;
			
			List<Card> validCards = GameManager.m_gameManager.currentMap.m_cards;
			yield return StartCoroutine(InputManager.m_inputManager.GetMouseSelection(validCards));


			
			m_HUD.SetActive(true);
			AssetManager.m_assetManager.m_props[16].gameObject.SetActive(true);
			AssetManager.m_assetManager.m_props[17].gameObject.SetActive(true);
			AssetManager.m_assetManager.m_props[38].gameObject.SetActive(true);
			AssetManager.m_assetManager.m_props[39].gameObject.SetActive(true);
			AssetManager.m_assetManager.m_props[42].gameObject.SetActive(true);
			AssetManager.m_assetManager.m_UIelements[6].gameObject.SetActive(true);
			m_buttonUI.SetActive(true);
			m_pauseUI.SetActive(false);
			AssetManager.m_assetManager.m_props[19].SetActive(true);
			AssetManager.m_assetManager.m_props[10].SetActive(false);
			AssetManager.m_assetManager.m_UIelements[5].gameObject.SetActive(false);
			PartyCards.m_partyCards.gameObject.SetActive(true);
			Time.timeScale = 1;


			if (DHeart.m_dHeart.isBeating)
			{
				DHeart.m_dHeart.StartHeartBeat();
			}

			m_currentMenuMode = MenuMode.None;
		}
	}
	
	public void EquipItem (Item item)
	{
//		Debug.Log("EQUIPPPPPPP");
//		foreach (UICard equipSlot in m_equipSlots)
//		{
//			if (equipSlot.m_nameUI.text == "Name")
//			{
//				equipSlot.m_cardSprite.spriteName = "Card_Front01";
//				equipSlot.m_nameUI.gameObject.SetActive(true);
//				equipSlot.m_nameUI.text = item.m_name;
//				equipSlot.m_portrait.gameObject.SetActive(true);
//				equipSlot.m_portrait.spriteName = item.m_portraitSpriteName;
//				equipSlot.m_abilityUI.gameObject.SetActive(true);
//				equipSlot.m_abilityUI.text = item.m_description;
//				equipSlot.gameObject.SetActive(true);
//				equipSlot.itemData = item;
//				return;
//			}
//			
//		}
	}
	
//	public void SetStorageSlot(Item item)
//	{
//		Debug.Log ("SETTING STORAGE SLOT");
//		foreach (UICard craftingSlot in m_storageSlots)
//		{
//			if (craftingSlot.m_nameUI.text == "Name")
//			{
//				craftingSlot.m_nameUI.gameObject.SetActive(true);
//				craftingSlot.m_nameUI.text = item.m_name;
//				craftingSlot.m_portrait.gameObject.SetActive(true);
//				craftingSlot.m_portrait.spriteName = item.m_portraitSpriteName;
//				craftingSlot.m_abilityUI.gameObject.SetActive(true);
//				craftingSlot.m_abilityUI.text = item.m_description;
//				craftingSlot.m_itemData = item;
//				craftingSlot.m_rankUI.text = item.m_keywordText;
//				craftingSlot.m_rankUI.gameObject.SetActive(true);
//				craftingSlot.m_healthIcon.gameObject.SetActive(true);
//
//				if (item.m_energyCost > 0)
//				{
//					craftingSlot.m_healthUI.text = item.m_energyCost.ToString();
//					craftingSlot.m_damageIcon.spriteName = "Icon_Energy";
//					craftingSlot.m_healthUI.gameObject.SetActive(true);
//					craftingSlot.m_damageIcon.gameObject.SetActive(true);
//				} else if (item.m_healthCost > 0)
//				{
//					craftingSlot.m_healthUI.text = item.m_healthCost.ToString();
//					craftingSlot.m_damageIcon.spriteName = "Icon_Health";
//					craftingSlot.m_healthUI.gameObject.SetActive(true);
//					craftingSlot.m_damageIcon.gameObject.SetActive(true);
//				}
//				else
//				{
//					craftingSlot.m_healthUI.gameObject.SetActive(false);
//					craftingSlot.m_damageIcon.gameObject.SetActive(false);
//				}
//
//				craftingSlot.m_miscOBJ[0].gameObject.SetActive(true);
//				return;
//			}
//		}
//	}
	
	public void SetCharSelectSlot (UICard sChar)
	{
				foreach (UICard craftingSlot in m_charSelectSlots)
		{
			if (craftingSlot.m_nameUI.text == "Name")
			{
				//craftingSlot.m_cardSprite.spriteName = "Card_Front01";
				craftingSlot.m_nameUI.gameObject.SetActive(true);
				craftingSlot.m_nameUI.text = sChar.m_followerData.m_nameText;
				craftingSlot.m_portrait.gameObject.SetActive(true);
				craftingSlot.m_portrait.spriteName = sChar.m_followerData.m_portraitSpriteName;
				craftingSlot.m_abilityUI.gameObject.SetActive(true);
				craftingSlot.m_abilityUI.text = sChar.m_followerData.m_abilityText;
				craftingSlot.m_followerData = sChar.m_followerData;
				
				return;
			}
		}
	}

	private List<string> m_oldActions = new List<string> ();
	public void UpdateActions (string newAction)
	{
		string oldAction = AssetManager.m_assetManager.m_typogenicText [14].Text;
		AssetManager.m_assetManager.m_typogenicText[14].Text = newAction;

		// update old actions list
		//m_oldActions.Add (oldAction);
		m_oldActions.Insert (0, oldAction);
		if (m_oldActions.Count > AssetManager.m_assetManager.m_oldActions.Length) {
			m_oldActions.RemoveAt(m_oldActions.Count-1);
		}

		for (int i=0; i < AssetManager.m_assetManager.m_oldActions.Length; i++) {
			if (i < m_oldActions.Count)
			{
				AssetManager.m_assetManager.m_oldActions[i].Text = m_oldActions[i];
			}
		}
	}
	
	public void SetCraftingSlot (UICard item)
	{
		foreach (UICard craftingSlot in m_craftingSlots)
		{
			if (craftingSlot.m_nameUI.text == "Name")
			{
				//craftingSlot.m_cardSprite.spriteName = "Card_Front01";
				craftingSlot.m_nameUI.gameObject.SetActive(true);
				craftingSlot.m_nameUI.text = item.m_itemData.m_name;
				craftingSlot.m_portrait.gameObject.SetActive(true);
				craftingSlot.m_portrait.spriteName = item.m_itemData.m_portraitSpriteName;
				craftingSlot.m_abilityUI.gameObject.SetActive(true);
				craftingSlot.m_abilityUI.text = item.m_itemData.m_description;
				craftingSlot.itemData = item.m_itemData;
				
				//populate results slot if 3 items are present
//				if (numCraftingItems == 3)
//				{
//					if (m_craftingSlots[0].m_nameUI.text == m_craftingSlots[1].m_nameUI.text && m_craftingSlots[0].m_nameUI.text == m_craftingSlots[2].m_nameUI.text && m_craftingSlots[0].m_nameUI.text != "Name")
//					{
//						UICard resultsSlot = (UICard)m_craftingSlots[m_craftingSlots.Length-1];
//						Item resultsItem = (Item)m_craftingSlots[0].m_itemData.m_craftResult.GetComponent("Item");
//						//resultsSlot.m_cardSprite.spriteName = "Card_Front01";
//						resultsSlot.m_nameUI.gameObject.SetActive(true);
//						resultsSlot.m_nameUI.text = resultsItem.m_name;
//						resultsSlot.m_portrait.gameObject.SetActive(true);
//						resultsSlot.m_portrait.spriteName = resultsItem.m_portraitSpriteName;
//						resultsSlot.m_abilityUI.gameObject.SetActive(true);
//						resultsSlot.m_abilityUI.text = resultsItem.m_description;
//						resultsSlot.itemData = item.m_itemData;
//					}
//				}
				
				return;
			}
		}
	}
	
	public void SpawnDamageNumber (int damage, Transform origin)
	{
//		Vector3 randOffset = new Vector3(Random.Range(-0.25f, 0.25f), 0, Random.Range(-0.25f, 0.25f));
//		GameObject newDT = (GameObject)Instantiate(m_damageText, (origin.position + m_damageText.transform.position + randOffset), m_damageText.transform.rotation);
//		TextMesh dtText = (TextMesh)newDT.GetComponentInChildren<TextMesh>();
//		dtText.text = damage.ToString();
//		newDT.transform.LookAt(Camera.main.transform.position);

		Vector3 screenPos = FollowCamera.m_followCamera.m_camera.WorldToScreenPoint(origin.position);
		float screenHeight = Screen.height;
		float screenWidth = Screen.width;
		screenPos.x -= (screenWidth / 2.0f);
		screenPos.y -= (screenHeight / 2.0f);
		//transform.localPosition = screenPos;

		GameObject newDT = (GameObject)Instantiate(m_UIdamageText, Vector3.zero, m_damageText.transform.rotation);
		newDT.transform.parent = m_HUD.transform;
		UILabel dtText = (UILabel)newDT.GetComponentInChildren<UILabel>();
		dtText.text = damage.ToString ();
		newDT.transform.localPosition = screenPos;

		GUIFollow f = (GUIFollow)newDT.GetComponent<GUIFollow>();
		f.SetTarget (origin.gameObject);
	}
	
	public void SpawnAbilityName (string abilityName, Transform origin)
	{
//		Vector3 randOffset = new Vector3(Random.Range(-0.25f, 0.25f), 0, Random.Range(-0.25f, 0.25f));
//		GameObject newDT = (GameObject)Instantiate(m_damageText, (origin.position + m_damageText.transform.position + randOffset), m_damageText.transform.rotation);
//		TextMesh dtText = (TextMesh)newDT.GetComponentInChildren<TextMesh>();
//		dtText.text = abilityName;
//		newDT.transform.LookAt(Camera.main.transform.position);

		Vector3 screenPos = FollowCamera.m_followCamera.m_camera.WorldToScreenPoint(origin.position);
		float screenHeight = Screen.height;
		float screenWidth = Screen.width;
		screenPos.x -= (screenWidth / 2.0f);
		screenPos.y -= (screenHeight / 2.0f);

		GameObject newDT = (GameObject)Instantiate(m_UIdamageText, Vector3.zero, m_damageText.transform.rotation);
		newDT.transform.parent = m_HUD.transform;
		UILabel dtText = (UILabel)newDT.GetComponentInChildren<UILabel>();
		dtText.text = abilityName;
		newDT.transform.localPosition = screenPos;
		
		GUIFollow f = (GUIFollow)newDT.GetComponent<GUIFollow>();
		f.SetTarget (origin.gameObject);
		
	}

	public void SpawnFloatingText (string text, Icon icon, Transform origin)
	{
//		Vector3 randOffset = new Vector3(Random.Range(-0.25f, 0.25f), 0, Random.Range(-0.25f, 0.25f));
//		GameObject newDT = (GameObject)Instantiate(m_damageText, (origin.position + m_damageText.transform.position + randOffset), m_damageText.transform.rotation);
//		TextMesh dtText = (TextMesh)newDT.GetComponentInChildren<TextMesh>();
//
//		if (icon != Icon.None)
//		{
//			TimerDestroy i = (TimerDestroy)newDT.transform.GetComponent("TimerDestroy");
//			Material mat = AssetManager.m_assetManager.m_materials[0];
//			if (icon == Icon.Gold)
//			{
//				mat = AssetManager.m_assetManager.m_materials[4];
//			} else if (icon == Icon.Health)
//			{
//				mat = AssetManager.m_assetManager.m_materials[1];
//			}
//			dtText.anchor = TextAnchor.MiddleRight;
//			i.m_objects[0].renderer.material = mat;
//			i.m_objects[0].gameObject.SetActive(true);
//
//		}
//
//		dtText.text = text;
//		newDT.transform.LookAt(Camera.main.transform.position);
//		newDT.animation.Stop ();
//		newDT.animation.Play ("TextFloat");

		Vector3 screenPos = FollowCamera.m_followCamera.m_camera.WorldToScreenPoint(origin.position);
		float screenHeight = Screen.height;
		float screenWidth = Screen.width;
		screenPos.x -= (screenWidth / 2.0f);
		screenPos.y -= (screenHeight / 2.0f);

		GameObject newDT = (GameObject)Instantiate(m_UIdamageText, Vector3.zero, m_damageText.transform.rotation);
		UILabel dtText = (UILabel)newDT.GetComponentInChildren<UILabel>();
		string s = text;
		newDT.transform.parent = m_HUD.transform;

		if (icon != Icon.None)
		{
			if (icon == Icon.Gold)
			{
				s = s + "^";
			} else if (icon == Icon.Health)
			{
				s = s + "&";
			} else if (icon == Icon.Energy)
			{
				s = s + "#";
			} else if (icon == Icon.MeleeDamage)
			{
				s = s + "$";
			} else if (icon == Icon.Armor)
			{
				s = s + "%";
			} else if (icon == Icon.Actions)
			{
				s = s + "A";
			}
		}

		dtText.text = s;
		newDT.transform.localPosition = screenPos;
		
		GUIFollow f = (GUIFollow)newDT.GetComponent<GUIFollow>();
		f.SetTarget (origin.gameObject);

		newDT.animation.Stop ();
		newDT.animation.Play ("TextFloat");
	}

	public void UpdateGoldUI ()
	{
		//m_XPUI [1].text = SettingsManager.m_settingsManager.gold.ToString ();
		if (AssetManager.m_assetManager.m_typogenicText[0] != null) {
			AssetManager.m_assetManager.m_typogenicText [0].Text = SettingsManager.m_settingsManager.xp.ToString ("D4");
		}
	}
	
	public void UpdateChainGUI (int chainSize)
	{
		if (chainSize > 1)
		{
			if (!m_chainUI[0].gameObject.activeSelf)
			{
				m_chainUI[0].gameObject.SetActive(true);
			}
			m_chainUI[0].text = chainSize.ToString() + " Chain!";
		} else if (m_chainUI[0].gameObject.activeSelf)
		{
			m_chainUI[0].gameObject.SetActive(false);
		}

//		List<UICard> skillCards = PartyCards.m_partyCards.GetAllSkills (false);
//		foreach (UICard c in skillCards)
//		{
//			if (c.itemData.itemState == Item.ItemState.Normal && c.itemData.HasKeyword(Item.Keyword.Chain) && GameManager.m_gameManager.currentChain == c.itemData.m_chainLevel)
//			{
//				StartCoroutine(c.itemData.ActivateItem());
//			}
//		}
		
//		foreach (Item thisItem in GameManager.m_gameManager.equippedItems)
//		{
//			if (thisItem.HasKeyword(Item.Keyword.Chain) && thisItem.itemState == Item.ItemState.Normal && GameManager.m_gameManager.currentChain == thisItem.m_chainLevel)
//			{
//				StartCoroutine(thisItem.ActivateItem());
//			}
//		}
	}
	
	public void UpdateEffectGUI (GameManager.StatusEffect effect, int duration)
	{
		if (duration > 0)
		{
			if (!m_statusEffectGUI.activeSelf)
			{
				m_statusEffectGUI.SetActive(true);	
			}
			m_actionUI[0].text = duration.ToString();
		} else if (m_statusEffectGUI.activeSelf) {
			m_statusEffectGUI.SetActive(false);	
		}
	}
	
	public void UpdateFollowerEffectGUI (Follower thisFollower, int duration)
	{
		foreach (UICard card in PartyCards.m_partyCards.m_party)
		{
			if (card.m_followerData == thisFollower)
			{
				if (duration > 0)
				{
					card.m_healthUI.text = duration.ToString();
					card.m_healthUI.transform.parent.gameObject.SetActive(true);
				} else if (card.m_healthUI.gameObject.activeInHierarchy) {
					card.m_healthUI.transform.parent.gameObject.SetActive(false);
				}
			}
		}
		
//		foreach (GameObject f in UIManager.m_uiManager.m_followerCards)
//		{
//			UICard fCard = (UICard)f.GetComponent("UICard");
//			if (fCard.m_followerData != null)
//			{
//				if (fCard.m_followerData.m_followerType == thisFollower.m_followerType)
//				{
//					if (duration > 0)
//					{
//						if (!fCard.m_healthUI.gameObject.activeInHierarchy)
//						{
//							fCard.m_healthUI.transform.parent.gameObject.SetActive(true);
//						}
//						fCard.m_healthUI.text = duration.ToString();
//					} else if (fCard.m_healthUI.gameObject.activeInHierarchy)
//					{
//						fCard.m_healthUI.transform.parent.gameObject.SetActive(false);
//					}
//					break;					
//				}
//			}
//		}
	}
	
	public void UpdateEnemyEffectGUI (GameManager.StatusEffect effect, int duration)
	{
		if (duration > 0)
		{
			if (!m_enemyStatusGUI.activeSelf)
			{
				m_enemyStatusGUI.SetActive(true);	
			}
			m_actionUI[1].text = duration.ToString();
		} else if (m_enemyStatusGUI.activeSelf) {
			m_enemyStatusGUI.SetActive(false);	
		}
	}

	private void ChangeLayer (bool bringForward, UICard card)
	{
		if (bringForward)
		{
			// update rank color text as needed


//			card.m_portrait.spriteName = card.m_itemData.m_portraitSpriteName;
//			card.m_nameUI.gameObject.SetActive(true);
//			card.m_abilityUI.gameObject.SetActive(true);
//			card.m_shortCutUI.gameObject.SetActive(false);
//			card.m_rankUI.gameObject.SetActive(true);
//			card.m_miscOBJ[0].gameObject.SetActive(true);
//			card.m_healthIcon.gameObject.SetActive(true);
			card.transform.rotation = Quaternion.Euler(Vector3.zero);
			
			BoxCollider bc = (BoxCollider)card.gameObject.GetComponent("BoxCollider");
			bc.size = m_boundsHoveredSize;
			bc.center = m_boundsHoveredPos;

			card.m_portrait.depth += 100;
			card.m_nameUI.depth += 100;
			card.m_rankUI.depth += 100;
			card.m_abilityUI.depth += 100;
			card.m_shortCutUI.depth += 100;
			card.m_damageIcon.depth += 100;
			card.m_healthUI.depth += 100;
			card.m_healthIcon.depth += 100;
			
			m_invHoveredCard.transform.localScale = m_selectedScale;

			// move card down if too close to the top of the screen
			float maxY = 0.1586f;
			if (m_invHoveredCard.transform.position.y > maxY)
			{
				Vector3 newPos = m_invHoveredCard.transform.position;
				newPos.y = maxY;
				m_invHoveredCard.m_miscOBJ[1].transform.position = newPos;
			}

			for (int j=0; j < card.m_itemData.m_graveBonus.Length; j++)
			{
				UISprite s = (UISprite)card.m_miscSprite[j];
				//s.gameObject.SetActive(true);
				s.depth += 100;
			}
		} else {

			for (int j=0; j < card.m_itemData.m_graveBonus.Length; j++)
			{
				UISprite s = (UISprite)card.m_miscSprite[j];
				//s.gameObject.SetActive(false);
				s.depth -= 100;
			}

//			card.m_portrait.spriteName = card.itemData.m_fullPortraitSpriteName;
			card.transform.localScale = m_unselectedScale;
			card.transform.rotation = Quaternion.Euler(card.startRot);

			//reset position in case card was moved down due to being too close to the top of the screen
			card.m_miscOBJ[1].transform.position = card.startPos;

//			card.m_nameUI.gameObject.SetActive(false);
//			card.m_abilityUI.gameObject.SetActive(false);
//			card.m_shortCutUI.gameObject.SetActive(true);
//			card.m_rankUI.gameObject.SetActive(false);
//			card.m_miscOBJ[0].gameObject.SetActive(false);
//			card.m_healthIcon.gameObject.SetActive(false);
			
			BoxCollider bc = (BoxCollider)m_invHoveredCard.gameObject.GetComponent("BoxCollider");
			bc.size = m_boundsNormalSize;
			bc.center = m_boundsNormalPos;
			
			card.m_portrait.depth -= 100;
			card.m_nameUI.depth -= 100;
			card.m_rankUI.depth -= 100;
			card.m_abilityUI.depth -= 100;
			card.m_shortCutUI.depth -= 100;
			card.m_damageIcon.depth -= 100;
			card.m_healthUI.depth -= 100;
			card.m_healthIcon.depth -= 100;


		}
	}
	
	public void InventoryCardHovered (GameObject hoveredCard)
	{
		if (m_invHoveredCard == null) // if was no card being hovered over last frame
		{
			UICard uiCard = (UICard)hoveredCard.transform.GetComponent("UICard");

			if (uiCard.itemData != null) 
			{
				m_invHoveredCard = uiCard;
				ChangeLayer(true, uiCard);
//				uiCard.m_portrait.spriteName = uiCard.m_itemData.m_portraitSpriteName;
//				uiCard.m_nameUI.gameObject.SetActive(true);
//				uiCard.m_abilityUI.gameObject.SetActive(true);
//				uiCard.m_shortCutUI.gameObject.SetActive(false);
//				uiCard.m_rankUI.gameObject.SetActive(true);
//				uiCard.m_miscOBJ[0].gameObject.SetActive(true);
//				uiCard.m_healthIcon.gameObject.SetActive(true);
//
//				BoxCollider bc = (BoxCollider)uiCard.gameObject.GetComponent("BoxCollider");
//				bc.size = m_boundsHoveredSize;
//				bc.center = m_boundsHoveredPos;
//				
//				uiCard.m_portrait.depth += 100;
//				uiCard.m_nameUI.depth += 100;
//				uiCard.m_rankUI.depth += 100;
//				uiCard.m_abilityUI.depth += 100;
//				uiCard.m_shortCutUI.depth += 100;
//				uiCard.m_damageIcon.depth += 100;
//				uiCard.m_healthUI.depth += 100;
//				uiCard.m_healthIcon.depth += 100;
//				
//				m_invHoveredCard.transform.localScale = m_selectedScale;
				
//				float offset = 100;
//				foreach (GameObject go in m_cardList)
//				{
//					UICard card = (UICard)go.transform.GetComponent("UICard");
//					if (card != m_invHoveredCard && card.transform.position.x > m_invHoveredCard.transform.position.x)
//					{
//						Vector3 newPos = card.transform.localPosition;
//						newPos.x += offset;
//						card.transform.localPosition = newPos;
//					}
//				}
			}
		} else if (m_invHoveredCard.gameObject != hoveredCard) // if the card currently being hovered is different than last frame
		{
			UICard uiCard = (UICard)hoveredCard.transform.GetComponent("UICard");
			
			if (uiCard.itemData != null)
			{
				ChangeLayer(false, m_invHoveredCard);
//				m_invHoveredCard.m_portrait.spriteName = m_invHoveredCard.itemData.m_fullPortraitSpriteName;
//				m_invHoveredCard.transform.localScale = m_unselectedScale;
//				m_invHoveredCard.m_nameUI.gameObject.SetActive(false);
//				m_invHoveredCard.m_abilityUI.gameObject.SetActive(false);
//				m_invHoveredCard.m_shortCutUI.gameObject.SetActive(true);
//				m_invHoveredCard.m_rankUI.gameObject.SetActive(false);
//				m_invHoveredCard.m_miscOBJ[0].gameObject.SetActive(false);
//				m_invHoveredCard.m_healthIcon.gameObject.SetActive(false);
//
//				BoxCollider bc = (BoxCollider)m_invHoveredCard.gameObject.GetComponent("BoxCollider");
//				bc.size = m_boundsNormalSize;
//				bc.center = m_boundsNormalPos;
//				
//				m_invHoveredCard.m_portrait.depth -= 100;
//				m_invHoveredCard.m_nameUI.depth -= 100;
//				m_invHoveredCard.m_rankUI.depth -= 100;
//				m_invHoveredCard.m_abilityUI.depth -= 100;
//				m_invHoveredCard.m_shortCutUI.depth -= 100;
//				m_invHoveredCard.m_damageIcon.depth -= 100;
//				m_invHoveredCard.m_healthUI.depth -= 100;
//				m_invHoveredCard.m_healthIcon.depth -= 100;
				
//				float offset = 100;
//				foreach (GameObject go in m_cardList)
//				{
//					UICard card = (UICard)go.transform.GetComponent("UICard");
//					if (card != m_invHoveredCard && card.transform.position.x > m_invHoveredCard.transform.position.x)
//					{
//						Vector3 newPos = card.transform.localPosition;
//						newPos.x -= offset;
//						card.transform.localPosition = newPos;
//					}
//				}
				
				m_invHoveredCard = uiCard;
				ChangeLayer(true, uiCard);
//				m_invHoveredCard.m_portrait.spriteName = m_invHoveredCard.itemData.m_portraitSpriteName;
//				m_invHoveredCard.m_nameUI.gameObject.SetActive(true);
//				m_invHoveredCard.m_abilityUI.gameObject.SetActive(true);
//				m_invHoveredCard.m_shortCutUI.gameObject.SetActive(false);
//				m_invHoveredCard.m_rankUI.gameObject.SetActive(true);
//				m_invHoveredCard.m_miscOBJ[0].gameObject.SetActive(true);
//				m_invHoveredCard.m_healthIcon.gameObject.SetActive(true);
//
//				bc = (BoxCollider)m_invHoveredCard.gameObject.GetComponent("BoxCollider");
//				bc.size = m_boundsHoveredSize;
//				bc.center = m_boundsHoveredPos;
//				
//				m_invHoveredCard.m_portrait.depth += 100;
//				m_invHoveredCard.m_nameUI.depth += 100;
//				m_invHoveredCard.m_rankUI.depth += 100;
//				m_invHoveredCard.m_abilityUI.depth += 100;
//				m_invHoveredCard.m_shortCutUI.depth += 100;
//				m_invHoveredCard.m_damageIcon.depth += 100;
//				m_invHoveredCard.m_healthUI.depth += 100;
//				m_invHoveredCard.m_healthIcon.depth += 100;
//				
//				m_invHoveredCard.transform.localScale = m_selectedScale;
					
//				foreach (GameObject go in m_cardList)
//				{
//					UICard card = (UICard)go.transform.GetComponent("UICard");
//					if (card != m_invHoveredCard && card.transform.position.x > m_invHoveredCard.transform.position.x)
//					{
//						Vector3 newPos = card.transform.localPosition;
//						newPos.x += offset;
//						card.transform.localPosition = newPos;
//					}
//				}
			} else if (m_invHoveredCard != null) {
					ClearInvSelection();
			}
		}
	}
	
	public void ClearInvSelection ()
	{
		if (m_invHoveredCard != null)
		{
//			if (m_invHoveredCard.itemData != null)
//			{
////				m_invHoveredCard.m_portrait.spriteName = m_invHoveredCard.itemData.m_fullPortraitSpriteName;
//				m_invHoveredCard.m_shortCutUI.gameObject.SetActive(true);
//			} else {
//				m_invHoveredCard.m_shortCutUI.gameObject.SetActive(false);
//			}
			//ChangeLayer(false, m_invHoveredCard);
			m_invHoveredCard.transform.localScale = m_unselectedScale;
			m_invHoveredCard.m_miscOBJ[1].transform.position = m_invHoveredCard.startPos;
//			m_invHoveredCard.m_nameUI.gameObject.SetActive(false);
//			m_invHoveredCard.m_abilityUI.gameObject.SetActive(false);
//			m_invHoveredCard.m_rankUI.gameObject.SetActive(false);
//			m_invHoveredCard.m_miscOBJ[0].gameObject.SetActive(false);
//			m_invHoveredCard.m_healthIcon.gameObject.SetActive(false);

			if (m_invHoveredCard.itemData != null)
			{
				for (int j=0; j < m_invHoveredCard.m_itemData.m_graveBonus.Length; j++)
				{
					UISprite s = (UISprite)m_invHoveredCard.m_miscSprite[j];
					//s.gameObject.SetActive(false);
					s.depth -= 100;
				}
			}

			BoxCollider bc = (BoxCollider)m_invHoveredCard.gameObject.GetComponent("BoxCollider");
			bc.size = m_boundsNormalSize;
			bc.center = m_boundsNormalPos;
			m_invHoveredCard.transform.rotation = Quaternion.Euler(m_invHoveredCard.startRot);
			m_invHoveredCard.m_portrait.depth -= 100;
			m_invHoveredCard.m_nameUI.depth -= 100;
			m_invHoveredCard.m_rankUI.depth -= 100;
			m_invHoveredCard.m_abilityUI.depth -= 100;
			m_invHoveredCard.m_shortCutUI.depth -= 100;
			m_invHoveredCard.m_damageIcon.depth -= 100;
			m_invHoveredCard.m_healthUI.depth -= 100;
			m_invHoveredCard.m_healthIcon.depth -= 100;
			
//			float offset = 100;
//			foreach (GameObject go in m_cardList)
//			{
//				UICard card = (UICard)go.transform.GetComponent("UICard");
//				if (card != m_invHoveredCard && card.transform.position.x > m_invHoveredCard.transform.position.x)
//				{
//					Vector3 newPos = card.transform.localPosition;
//					newPos.x -= offset;
//					card.transform.localPosition = newPos;
//				}
//			}

			m_invHoveredCard = null;
		}
	}
	
	public void RefreshInventoryMenu()
	{
		foreach (GameObject go in m_cardList)
		{
			Destroy(go);	
		}
		
		m_cardList.Clear();
		
//		foreach (FuseButton fb in m_fuseButtons)
//		{
//			Destroy(fb.gameObject);	
//		}
//		
//		m_fuseButtons.Clear();
		
		SpawnInventoryCards(AssetManager.m_assetManager.m_props[6].transform, true, 200);
		UpdateHandSize ();
//		if (InputManager.m_inputManager.shiftHeld && m_currentMenuMode == MenuMode.Inventory)
//		{
//			HighlightConsumables(true);
//		}

		//AssetManager.m_assetManager.m_typogenicText[2].Text = "BACKPACK: " + GameManager.m_gameManager.inventory.Count.ToString() + "/" + GameManager.m_gameManager.maxBP.ToString(); 
	}

	public void UpdateHandSize ()
	{
		string handSize = GameManager.m_gameManager.inventory.Count.ToString();
		string handMax = "/" + GameManager.m_gameManager.maxBP.ToString();
		AssetManager.m_assetManager.m_typogenicText[2].Text = handSize;
		AssetManager.m_assetManager.m_typogenicText[3].Text = handMax;
	}

	public void HighlightConsumables (bool doHighlight)
	{
		Color inactiveColor = Color.gray;
		Color activeColor = Color.white;

		foreach (GameObject go in m_cardList)
		{
			UICard c = (UICard) go.GetComponent("UICard");

			if (c.itemData != null)
			{
//				if (!c.itemData.HasKeyword(Item.Keyword.UseFromInv))
//				{
//					if (doHighlight)
//					{
//						//darken card visuals
//						c.m_portrait.color = inactiveColor;
//						c.m_shortCutUI.color = inactiveColor;
//
//						if (m_invHoveredCard != null)
//						{
//							if (m_invHoveredCard == c)
//							{
//								//unselect card
//								ChangeLayer(false, m_invHoveredCard);
//								m_invHoveredCard.m_portrait.spriteName = m_invHoveredCard.itemData.m_fullPortraitSpriteName;
//								m_invHoveredCard.m_shortCutUI.gameObject.SetActive(true);
//								m_invHoveredCard.transform.localScale = m_unselectedScale;
//								m_invHoveredCard.m_nameUI.gameObject.SetActive(false);
//								m_invHoveredCard.m_abilityUI.gameObject.SetActive(false);
//								m_invHoveredCard.m_rankUI.gameObject.SetActive(false);
//								m_invHoveredCard.m_miscOBJ[0].gameObject.SetActive(false);
//								m_invHoveredCard.m_healthIcon.gameObject.SetActive(false);
//								
//								BoxCollider bc = (BoxCollider)m_invHoveredCard.gameObject.GetComponent("BoxCollider");
//								bc.size = m_boundsNormalSize;
//								bc.center = m_boundsNormalPos;
//								
//								m_invHoveredCard.m_portrait.depth -= 100;
//								m_invHoveredCard.m_nameUI.depth -= 100;
//								m_invHoveredCard.m_rankUI.depth -= 100;
//								m_invHoveredCard.m_abilityUI.depth -= 100;
//								m_invHoveredCard.m_shortCutUI.depth -= 100;
//								m_invHoveredCard.m_damageIcon.depth -= 100;
//								m_invHoveredCard.m_healthUI.depth -= 100;
//								m_invHoveredCard.m_healthIcon.depth -= 100;

//								m_invHoveredCard = null;
//							}
//						}
//					} else {
//						c.m_portrait.color = activeColor;
//						c.m_shortCutUI.color = activeColor;
//					}
//				}		
			}
		}

		if (m_fuseButtons.Count > 0)
		{
			if (doHighlight)
			{
				foreach (FuseButton fb in m_fuseButtons)
				{
					fb.gameObject.SetActive(false);
				}
			} else {
				foreach (FuseButton fb in m_fuseButtons)
				{
					fb.gameObject.SetActive(true);	
				}
			}
		}
	}

	public void SpawnBadges (List<string> badgeNames)
	{
		for (int i=0; i < m_currentBadges.Length; i++) {
			StoreBadge thisBadge = (StoreBadge)m_currentBadges[i];
			if (i < badgeNames.Count)
			{
				//populate badge name
				thisBadge.m_priceLabel.text = badgeNames[i];

			} else {
				//deactivate badge art
				thisBadge.m_priceLabel.gameObject.SetActive(false);
				thisBadge.m_priceBG.gameObject.SetActive(false);
				thisBadge.m_sprite.gameObject.SetActive(false);
			}
		}
	}

	public void SpawnDeck (List<GameObject> itemDeck)
	{
		Vector3 startPos = AssetManager.m_assetManager.m_props[21].transform.position;
		Vector3 endPos = AssetManager.m_assetManager.m_props[22].transform.position;
		int numCards = Mathf.Clamp( itemDeck.Count-1, 1, 99);

		for (int i=0; i < itemDeck.Count; i++)
		{
			Item thisItem = (Item)itemDeck[i].GetComponent("Item");
			Vector3 cardPos = Vector3.Lerp(startPos, endPos, ((float)i) / ((float)numCards));
			GameObject fCard = (GameObject)Instantiate(UIManager.m_uiManager.m_cardSmall, cardPos, UIManager.m_uiManager.m_cardSmall.transform.rotation);
			fCard.transform.parent = UIManager.m_uiManager.m_pauseUI.transform;
			UICard cardUI = (UICard)fCard.GetComponent("UICard");
				
			Color c = Color.white;
			if (thisItem.m_class == Follower.FollowerClass.August)
			{
				c = Color.blue;
			} else if (thisItem.m_class == Follower.FollowerClass.Jin)
			{
				c = Color.yellow;
			} else if (thisItem.m_class == Follower.FollowerClass.Telina)
			{
				c = Color.green;
			} else if (thisItem.m_class == Follower.FollowerClass.Brand)
			{
				c = Color.red;
			}
			cardUI.m_portrait.color = c;

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
	
	public void SpawnInventoryCards (Transform p, bool doFusion, float yOffset)
	{
		//BoxCollider c = (BoxCollider)m_cardSmall.GetComponent("BoxCollider");
		yOffset = -315;
		//float xOffset = 430;
		float xOffset = 200;
		

//		Vector3 endPos =  new Vector3 (200.3662f, -0.87f, 200);
//		Vector3 startPos = new Vector3 (201.5221f, -0.87f, 200);
		Vector3 endPos =  AssetManager.m_assetManager.m_props[13].transform.position;
		Vector3 startPos = AssetManager.m_assetManager.m_props[12].transform.position;

		//sort cards, separated by: Items / Skills / Gold / XP / Misc

//		List<Item> items = new List<Item> ();
//		List<Item> skills = new List<Item> ();
//		List<Item> gold = new List<Item> ();
//		List<Item> xp = new List<Item> ();
//		List<Item> craft = new List<Item> ();
		List<Item> f1 = new List<Item> ();
		List<Item> f2 = new List<Item> ();
		List<Item> f3 = new List<Item> ();
		List<Item> f4 = new List<Item> ();
		List<Item> f5 = new List<Item> ();
		List<Item> sortedList = new List<Item> ();

		//foreach (Item i in GameManager.m_gameManager.inventory)
		for (int i=0; i < GameManager.m_gameManager.inventory.Count; i++)
		{
			//sortedList.Add(i);
			Item thisItem = (Item)GameManager.m_gameManager.inventory[i];
			bool itemPlaced = false;
			for (int j=0; j < GameManager.m_gameManager.followers.Count; j++)
			{
				Follower f = (Follower)GameManager.m_gameManager.followers[j];

				if (thisItem.m_class == f.m_followerClass)
				{
					itemPlaced = true;
					if (j == 0)
					{
						f1.Add(thisItem);
					} else if (j == 1)
					{
						f2.Add(thisItem);
					} else if (j == 2)
					{
						f3.Add(thisItem);
					} else if (j == 3)
					{
						f4.Add(thisItem);
					}
				}
			}

			if (!itemPlaced)
			{
				f5.Add(thisItem);
			}
		}
		//sortedList.Sort(delegate(Item i1, Item i2) { return i1.m_class.CompareTo(i2.m_class); });

		if (f5.Count > 0) {
			f1.Sort(delegate(Item i2, Item i1) { return i1.name.CompareTo(i2.name); });
			sortedList.AddRange(f5);
		}
		if (f4.Count > 0) {
			f4.Sort(delegate(Item i2, Item i1) { return i1.name.CompareTo(i2.name); });
			sortedList.AddRange(f4);
		}
		if (f3.Count > 0) {
			f3.Sort(delegate(Item i2, Item i1) { return i1.name.CompareTo(i2.name); });
			sortedList.AddRange(f3);
		}
		if (f2.Count > 0) {
			f2.Sort(delegate(Item i2, Item i1) { return i1.name.CompareTo(i2.name); });
			sortedList.AddRange(f2);
		}
		if (f1.Count > 0) {
			f1.Sort(delegate(Item i2, Item i1) { return i1.name.CompareTo(i2.name); });
			sortedList.AddRange(f1);
		}





		GameManager.m_gameManager.inventory = sortedList;
		

		
		//fusion checking
		Item lastItem = null;
		List<UICard> fusionList = new List<UICard>();
		GameObject lastCard = null;
		int numCards = Mathf.Clamp( GameManager.m_gameManager.inventory.Count-1, 1, 99);

		for (int i=0; i < GameManager.m_gameManager.inventory.Count ; i++)
		{
			Vector3 cardPos = Vector3.Lerp(startPos, endPos, ((float)i) / numCards);
//			float randPos = Random.Range(-0.01f, 0.01f);
//			cardPos.y += randPos;
			GameObject fCard = (GameObject)Instantiate(m_cardSmall, cardPos, m_cardSmall.transform.rotation);
			if (lastCard != null)
			{
				if (lastCard.transform.position.x - fCard.transform.position.x  > 0.2407f)
				{
					Vector3 newPos = lastCard.transform.position;
					newPos.x -= 0.2407f;
					fCard.transform.position = newPos;
				}
			}
			lastCard = fCard;

			fCard.transform.parent = p;
//			fCard.transform.localPosition = cardPos;
			m_cardList.Add(fCard);
			UICard cardUI = (UICard)fCard.GetComponent("UICard");
			
			if (i < GameManager.m_gameManager.inventory.Count)
			{
				Item thisItem = (Item)GameManager.m_gameManager.inventory[i].GetComponent("Item");

				Color c = Color.white;
				if (thisItem.m_class == Follower.FollowerClass.August)
				{
					c = Color.blue;
				} else if (thisItem.m_class == Follower.FollowerClass.Jin)
				{
					c = Color.yellow;
				} else if (thisItem.m_class == Follower.FollowerClass.Telina)
				{
					c = Color.green;
				} else if (thisItem.m_class == Follower.FollowerClass.Brand)
				{
					c = Color.red;
				}
				cardUI.m_portrait.color = c;

				thisItem.card = cardUI;
				fCard.name = " " + thisItem.m_name;
				cardUI.m_nameUI.text = thisItem.m_name;
				cardUI.m_abilityUI.text = thisItem.m_description;
				cardUI.m_rankUI.text = thisItem.m_class.ToString();
//				cardUI.m_shortCutUI.text = thisItem.m_shortcutText;
//				cardUI.m_portrait.spriteName = thisItem.m_fullPortraitSpriteName;
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

				//check if card is equippable, color red if not
//				if (PartyCards.m_partyCards.MeetsSkillReqs(thisItem))
//				{
//					if (PartyCards.m_partyCards.NumHeroesEligible(thisItem) == 0)
//					{
//						cardUI.m_rankUI.color = Color.grey;
//					} else {
//						cardUI.m_rankUI.color = Color.white;
//					}
//				}
//				else {
//					cardUI.m_rankUI.color = Color.red;
//				}

//				cardUI.m_rankUI.text = thisItem.m_keywordText;
				cardUI.itemData = thisItem;

//				if (thisItem.m_energyCost > 0)
//				{
					//cardUI.m_healthUI.text = thisItem.m_energyCost.ToString();
				cardUI.m_healthUI.text = thisItem.adjustedEnergyCost.ToString();
				if (thisItem.adjustedEnergyCost > thisItem.m_energyCost)
				{
					cardUI.m_healthUI.color = Color.red;
				} else if (thisItem.adjustedEnergyCost < thisItem.m_energyCost)
				{
					cardUI.m_healthUI.color = Color.green;
				}

				cardUI.m_damageIcon.spriteName = "Icon_Energy";
				cardUI.m_healthUI.gameObject.SetActive(true);
				cardUI.m_damageIcon.gameObject.SetActive(true);
//				}
				if (thisItem.m_healthCost > 0)
				{
					//cardUI.m_healthUI.text = thisItem.m_healthCost.ToString();
					cardUI.m_healthUI.text = thisItem.adjustedHealthCost.ToString();
					cardUI.m_damageIcon.spriteName = "Icon_Health";
					cardUI.m_healthUI.gameObject.SetActive(true);
					cardUI.m_damageIcon.gameObject.SetActive(true);
				} 
//				else if (thisItem.HasKeyword(Item.Keyword.Consumeable))
//				{
//					cardUI.m_healthUI.gameObject.SetActive(false);
//					cardUI.m_damageIcon.spriteName = "Icon_Limbo";
//					cardUI.m_damageIcon.gameObject.SetActive(true);
//				}
//				else if (thisItem.m_class == Follower.FollowerClass.Limbo)
//				{
//					cardUI.m_healthUI.gameObject.SetActive(false);
//					cardUI.m_damageIcon.spriteName = "Icon_Limbo02";
//					cardUI.m_damageIcon.gameObject.SetActive(true);
//				}
//				else
//				{
//					cardUI.m_healthUI.gameObject.SetActive(false);
//					cardUI.m_damageIcon.gameObject.SetActive(false);
//				}

				for (int j=0; j < thisItem.m_graveBonus.Length; j++)
				{
					Item.GraveBonus gb = (Item.GraveBonus)thisItem.m_graveBonus[j];
					UISprite s = (UISprite)cardUI.m_miscSprite[j];

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

				if (!thisItem.HasKeyword(Item.Keyword.WhileInHand) && !thisItem.HasKeyword(Item.Keyword.Limbo))
				{
					cardUI.m_miscOBJ[0].gameObject.SetActive(true);
				}

				if (thisItem.HasKeyword (Item.Keyword.LostSoul) || thisItem.HasKeyword (Item.Keyword.Key)) {
					cardUI.m_healthUI.gameObject.SetActive(false);
					cardUI.m_damageIcon.gameObject.SetActive(false);
				}
//				cardUI.m_shortCutUI.gameObject.SetActive(true);
				
				
				//checking for valid fusion
//				if (doFusion)
//				{
//					if (lastItem == null && thisItem.HasKeyword(Item.Keyword.Craftable))
//					{
//						lastItem = thisItem;
//						fusionList.Add(cardUI);
//					} 
					
//					if (lastItem != null)
//					{
//						if (thisItem.m_name == lastItem.m_name && thisItem.HasKeyword(Item.Keyword.Craftable)) //item is same as last
//						{
//							//make sure it's not already in the list
//							bool inList = false;
//							foreach (UICard c in fusionList)
//							{
//								if (c.itemData.gameObject == thisItem.gameObject)
//								{
//									inList = true;
//								}
//							}
//
//							if (!inList)
//							{
//								fusionList.Add(cardUI);
//
//								if (fusionList.Count == 3)
//								{
//									//create fusion button
//									GameObject go = (GameObject)Instantiate(m_fuseButton, Vector3.zero, m_fuseButton.transform.rotation);
//									go.transform.parent = p;
//									FuseButton fb = (FuseButton)go.GetComponent("FuseButton");
//									m_fuseButtons.Add(fb);
//									fb.Initialize(fusionList);
//									if (UIManager.m_uiManager.menuMode != MenuMode.None && UIManager.m_uiManager.menuMode != MenuMode.Chest)
//									{
//										fb.gameObject.SetActive(false);
//									}
//
//									//clear list
//									fusionList.Clear();
//								}
//							}
//							
//						} else if (thisItem.m_name != lastItem.m_name && thisItem.HasKeyword(Item.Keyword.Craftable)) //item is different than last, restart
//						{
//							lastItem = thisItem;
//							fusionList.Clear();
//							fusionList.Add(cardUI);
//						}
//					}
//				}
				
			} else {
				cardUI.m_nameUI.gameObject.SetActive(false);
				cardUI.m_abilityUI.gameObject.SetActive(false);
				cardUI.m_portrait.spriteName = "Card_Back03";
				cardUI.m_itemData = null;
				cardUI.gameObject.SetActive(false);
			}
		}
		

	}
	
	public UICard invHoveredCard {get {return m_invHoveredCard;}}
	
	public List<FuseButton> fuseButtons {get {return m_fuseButtons;}}
	
	public bool targetDisplayed
	{
		get
		{
			return m_targetDisplayed;	
		}
	}
	
	public MenuMode menuMode
	{
		get
		{
			return m_currentMenuMode;	
		}

		set
		{
			m_currentMenuMode = value;
		}
	}
	
	public int numCraftingItems
	{
		get
		{
			int num = 0;
			foreach	(UICard thisCard in m_craftingSlots)
			{
				if (thisCard.m_nameUI.text != "Name")
				{
					num ++;
				}
			}
			return num;
		}
	}
	
	public int numEquippedItems
	{
		get
		{
			int num = 99;
//			foreach	(UICard thisCard in m_equipSlots)
//			{
//				if (thisCard.m_nameUI.text != "Name")
//				{
//					num ++;
//				}
//			}
			return num;
		}
	}
	public List<GameObject> cardList {get{return m_cardList;}set{m_cardList = value;}}
	public bool partyCardsActive {get{return m_partyCardsActive;} set{m_partyCardsActive = value;}}
	public bool equipCardsActive {get{return m_equipCardsActive;} set{m_equipCardsActive = value;}}
	public bool buttonsActive {get{return m_buttonsActive;} set{m_buttonsActive = value;}}
	public bool doPortal {get{return m_doPortal;} set{m_doPortal = value;}}
	public bool leaveDungeon {get{return m_leaveDungeon;} set{m_leaveDungeon = value;}}
	public UIManager.ReactMode currentReactMode {get{return m_currentReactMode;}}
	public GameObject targetCard {get {return m_targetCard;}}
	public UICard chestCard {get {return m_chestCard;} set {m_chestCard = value;}}
	public Vector3 selectedScale {get{return m_selectedScale;}}
	public Vector3 unselectedScale { get { return m_unselectedScale; } set { m_unselectedScale = value; }}
	public Vector3 miniScale {get{return m_miniScale;}}

}
