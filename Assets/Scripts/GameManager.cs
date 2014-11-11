using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
	public enum Direction
	{
		North,
		South,
		East,
		West,
		None,
	}

	public enum Dice
	{
		D4,
		D6,
		D8,
		D10,
		D12,
		D20,
		D1,
	}
	
	public enum Turn
	{
		Player,
		Enemy,
		Environment,
	}
	
	public enum StatusEffect
	{
		None,
		Poison,
	}
	
	[System.Serializable]
	public class LootTable
	{
		public GameObject[] m_lootTable;
	}

	[System.Serializable]
	public class GraveSlot {
		public enum ObjectType {
			None,
			Item,
			Enemy,
		}

		public ObjectType type = ObjectType.None;
		public Item item = null;
		public Enemy enemy = null;
	}
	
	public bool
		m_deleteSave = false,
		m_resetIfDiffVersion = false,
		m_unlockHeroes = false,
		m_doCustomParty = false,
		m_doStartItems = false,
		m_doTrial = false,
		m_quickStart = false;

	public int 
		m_startLevel = 0;
	
	public GameObject[] 
		m_customParty,
		m_startItems;
	

	private int
		m_startingFollowers = 0,
		m_currentChain = 0,
		m_currentID = 0,
		m_accruedXP = 0,
		m_currentTurnNum = 0,
		m_levelsCompletedBonus = 0;
	
	public GameObject[]
		m_followerBank,
		m_itemBank,
		m_chestBank,
		m_heroCardsStart,
		m_heroCardsEnd;
	
	public GameObject
		m_camera,
		m_key,
		m_settingsManager;
	
	public static GameManager
		m_gameManager;
	
	public LootTable[]
		m_lootTable,
		m_bossLootTable,
		m_classDecks;
	
	private Turn m_currentTurn = Turn.Player;
	
//	private MapManager.Quest
//		m_currentQuest;
	
	private MapManager.Map
		m_currentMap;
	
	private List<Follower>
		m_followers = new List<Follower>();

	private List<Follower>
		m_lostSouls = new List<Follower>();
	
	private List<Enemy>
		m_deadEnemies = new List<Enemy>(),
		m_distOrderedEnemies = new List<Enemy>();
	
	private List<Item>
		m_inventory = new List<Item>(),
		m_equippedItems = new List<Item>(),
		m_storageItems = new List<Item>(),
		m_bonusItems = new List<Item>();

	private List<GameObject>
		m_drawDeck = new List<GameObject>(),
		m_chestDeck = new List<GameObject>();

	private List<GraveSlot>
		m_grave = new List<GraveSlot>();
	
	private bool
		m_selectMode = false,
		m_acceptInput = false,
		m_doShop = false,
		m_doShopPortal = false,
		m_doStorageChest = false,
		m_doHeartBeat = false,
		m_doStatBar = true,
		m_showDetailedActions = false;

	private int
		m_healthRecover = 0,
		m_energyRecover = 0,
		m_BPbonus = 0,
		m_storageBonus = 0,
		m_itemBonus = 0,
		m_partyBonus = 0,
		m_drawCost = 5,
		m_baseDrawCost = 5,
		m_drawCostMod = 0,
		m_adjustedDrawCost = 0,
		m_numDiscardedThisTurn = 0,
		m_numTiles = 0,
		m_numTilesFlipped = 0,
		m_initiative = 0;
	
	private Card
		m_selectedCard = null;
	
	private List<GameState.ProgressState>
		m_gameProgress;
	
	private GameState
		m_gameState;

	private GameObject
		m_focusGO = null; //when tabbing camera through player and nearest enemies, which are we currently focused on
	
	private Follower
		m_playerFollower,
		m_currentFollower = null;

	private Item[]
	m_limboCards = new Item[4];
	
	// Use this for initialization
	void Awake () {
		AudioListener.volume = 0;
		Application.targetFrameRate = 60;
		m_gameManager = this;
		
		if (SettingsManager.m_settingsManager == null)
		{
			Instantiate(m_settingsManager, Vector3.zero, Quaternion.identity);	
			SettingsManager.m_settingsManager.gameState = new GameState();
			SettingsManager.m_settingsManager.gameProgress = SettingsManager.m_settingsManager.gameState.loadState();
		}


//		if (PlayerPrefs.HasKey("DoTutorial") && !SettingsManager.m_settingsManager.trial)
//		{
//			if (PlayerPrefs.GetInt("DoTutorial") == 0)
//			{
//				Debug.Log("LOAD TUTORIAL");
//				SettingsManager.m_settingsManager.difficultyLevel = -6;
//			}
//		}
	}
	
	void Start ()
	{	
		if (SettingsManager.m_settingsManager.startChapter != -1) {
			
			switch (SettingsManager.m_settingsManager.startChapter)
			{
			case 1:
				m_startLevel = 0;
				break;
			case 2:
				m_startLevel = 10;
				// update chapter name
				AssetManager.m_assetManager.m_typogenicText[10].Text = "CHAPTER 2";
				AssetManager.m_assetManager.m_typogenicText[11].Text = "EREBUS";
				break;
			case 3:
				m_startLevel = 20;
				break;
			case 4:
				m_startLevel = 30;
				break;
			case 5:
				m_startLevel = 40;
				break;
			}
		}

		StartCoroutine(Initialize());
	}
	
	private IEnumerator Initialize ()
	{
		// adjust any UI elements
		if ((((float)Screen.width) / ((float)Screen.height)) < 1.4f)
		{
			EffectsPanel.m_effectsPanel.SetAnchor(1);
		}

		if (!m_quickStart) {
			AssetManager.m_assetManager.m_uiSprites [0].gameObject.SetActive (true);
		}
		m_gameState = SettingsManager.m_settingsManager.gameState;
		
		if (m_deleteSave)
		{
			SettingsManager.m_settingsManager.gameState.clearState();	
		}
		
		if (!PlayerPrefs.HasKey("Version"))
		{
			PlayerPrefs.SetString("Version", SettingsManager.m_settingsManager.version);
			PlayerPrefs.Save();
		}
		
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
		
		if (!SettingsManager.m_settingsManager.gameState.saveStateExists())
		{
			//create initial save	
			SettingsManager.m_settingsManager.gameState.InitializeSaveState();
		}
		
		//m_gameProgress = m_gameState.loadState();
		m_gameProgress = SettingsManager.m_settingsManager.gameProgress;

		if (SettingsManager.m_settingsManager.demo)
		{
			m_doCustomParty = true;
			GameObject[] party = new GameObject[4];
			party[0] = m_followerBank[19];
			party[1] = m_followerBank[0];
			party[2] = m_followerBank[1];
			party[3] = m_followerBank[2];

			m_customParty = party;
		}
		if (m_doCustomParty) {
			for (int j=0; j < SettingsManager.m_settingsManager.badgeStates.Count; j++) {
				if (SettingsManager.m_settingsManager.badgeStates [j] == 2) {
					SettingsManager.m_settingsManager.badgeStates [j] = 1;
				}
			}
		}

		LoadBadges ();
		
		UIManager.m_uiManager.Initialize();
		
		if (!m_doCustomParty && SettingsManager.m_settingsManager.party == null)
		{
			//yield return StartCoroutine(UIManager.m_uiManager.ChangeMenuMode(UIManager.MenuMode.CharSelect));
		} else {
			if (m_doCustomParty)
			{
				m_playerFollower = (Follower)m_customParty[0].GetComponent("Follower");
			} else if (SettingsManager.m_settingsManager.party != null)
			{
				m_playerFollower = (Follower)SettingsManager.m_settingsManager.party[0];	
			}
		}

		if (m_startLevel != 0 && !SettingsManager.m_settingsManager.trial) {
			SettingsManager.m_settingsManager.difficultyLevel = m_startLevel;
		}

		if (m_doTrial)
		{
			SettingsManager.m_settingsManager.trial = true;
			SettingsManager.m_settingsManager.difficultyLevel = m_startLevel;
			m_currentMap = MapManager.m_mapManager.BuildTrial (m_startLevel);
		}
		else if (SettingsManager.m_settingsManager.trial)
		{
			m_currentMap = MapManager.m_mapManager.BuildTrial (SettingsManager.m_settingsManager.difficultyLevel);
		} else 
		{
			m_currentMap = MapManager.m_mapManager.BuildLevel(SettingsManager.m_settingsManager.difficultyLevel);
		}

		
		MapManager.m_mapManager.SpawnPlayer(m_currentMap, m_playerFollower);
		m_currentFollower = Player.m_player.playerFollower;
		StartCoroutine(UIManager.m_uiManager.ChangeMenuMode(UIManager.MenuMode.None));
		
		Follower playerFollower = (Follower)Player.m_player.transform.GetComponent("Follower");
		
		m_followers.Insert(0, playerFollower);
		List<GameObject> followerBank = new List<GameObject>();
		foreach (GameObject tf in m_followerBank)
		{
			Follower thisF = (Follower) tf.GetComponent("Follower");
			if (playerFollower.m_followerType != thisF.m_followerType)
			{
				followerBank.Add(tf);
			}
		}
		
		if (!m_doCustomParty && SettingsManager.m_settingsManager.party == null)
		{
//			for (int i=0; i<m_startingFollowers; i++)
//			{
//				int rand = Random.Range(0, followerBank.Count);
//				GameObject follower = followerBank[rand];
//				Vector3 pos = Vector3.one * 1000;
//				followerBank.RemoveAt(rand);
//				Follower thisFollower = (Follower)((GameObject)Instantiate(follower, pos, follower.transform.rotation)).GetComponent("Follower");
//				m_followers.Add(thisFollower);
//			}
		} else {
			
			if (m_doCustomParty)
			{

				for (int i=1; i < m_customParty.Length; i++)
				{
					GameObject follower = m_customParty[i];
					Vector3 pos = Vector3.one * 100000;
					Follower thisFollower = (Follower)((GameObject)Instantiate(follower, pos, follower.transform.rotation)).GetComponent("Follower");
					thisFollower.transform.parent = Player.m_player.m_playerMesh.transform;
					thisFollower.transform.localPosition = Vector3.zero;
					thisFollower.m_followerMesh.SetActive(false);
					if (thisFollower.m_shadowMesh != null) {thisFollower.m_shadowMesh.gameObject.SetActive(false);}

					//set progress / level values for new follower
					foreach (GameState.ProgressState thisState in GameManager.m_gameManager.gameProgress)
					{
						if (thisState.m_followerType == thisFollower.m_followerType)
						{
							thisFollower.isLocked = thisState.m_isLocked;
							thisFollower.currentLevel = thisState.m_level;
							thisFollower.currentXP = thisState.m_XP;
							thisFollower.XPBonus = thisState.m_XPBonus;
							
							int[] heroBadgeStates = new int[5];
							heroBadgeStates[0] = thisState.m_badgeLevel1;
							heroBadgeStates[1] = thisState.m_badgeLevel2;
							heroBadgeStates[2] = thisState.m_badgeLevel3;
							heroBadgeStates[3] = thisState.m_badgeLevel4;
							heroBadgeStates[4] = thisState.m_badgeLevel5;
							//heroBadgeStates[5] = charProgress.m_badgeLevel6;
							thisFollower.heroBadgeStates = heroBadgeStates;
							
							thisFollower.SetLevel();
							break;
						}
					}

					m_followers.Add(thisFollower);
				}
			} else if (SettingsManager.m_settingsManager.party != null)
			{
				for (int i=1; i < SettingsManager.m_settingsManager.party.Count; i++)
				{
					GameObject follower = SettingsManager.m_settingsManager.party[i].gameObject;
					Vector3 pos = Vector3.one * 1000000;
					Follower thisFollower = (Follower)((GameObject)Instantiate(follower, pos, follower.transform.rotation)).GetComponent("Follower");
					thisFollower.transform.parent = Player.m_player.m_playerMesh.transform;
					thisFollower.transform.localPosition = Vector3.zero;
					thisFollower.m_followerMesh.SetActive(false);
					if (thisFollower.m_shadowMesh != null) {thisFollower.m_shadowMesh.gameObject.SetActive(false);}
					
					//set progress / level values for new follower
					foreach (GameState.ProgressState thisState in GameManager.m_gameManager.gameProgress)
					{
						if (thisState.m_followerType == thisFollower.m_followerType)
						{
							thisFollower.isLocked = thisState.m_isLocked;
							thisFollower.currentLevel = thisState.m_level;
							thisFollower.currentXP = thisState.m_XP;
							thisFollower.XPBonus = thisState.m_XPBonus;

							int[] heroBadgeStates = new int[5];
							heroBadgeStates[0] = thisState.m_badgeLevel1;
							heroBadgeStates[1] = thisState.m_badgeLevel2;
							heroBadgeStates[2] = thisState.m_badgeLevel3;
							heroBadgeStates[3] = thisState.m_badgeLevel4;
							heroBadgeStates[4] = thisState.m_badgeLevel5;
							//heroBadgeStates[5] = charProgress.m_badgeLevel6;
							thisFollower.heroBadgeStates = heroBadgeStates;

							thisFollower.SetLevel();
							break;
						}
					}
					
					m_followers.Add(thisFollower);
				}
				
				//SettingsManager.m_settingsManager.party = null;
			}
		}

		PopulateDrawDeck (m_followers);
		PopulateChestDeck(m_followers);
		UIManager.m_uiManager.SpawnDeck (m_drawDeck);


		UIManager.m_uiManager.SetFollowers(m_followers);

		Player.m_player.SetPassiveFollowerBonuses(m_followers);

		// do item specific badges
		foreach (Follower f in m_followers)
		{
			foreach (Follower.HeroBadge hb in f.activeBadges)
			{
				if (hb == Follower.HeroBadge.Item_Shield && GameManager.m_gameManager.inventory.Count < GameManager.m_gameManager.maxBP)
				{
					GameObject randItem = (GameObject)Instantiate(GameManager.m_gameManager.m_itemBank[7], Vector3.zero, Quaternion.identity);
					Item itemRef = (Item)randItem.GetComponent("Item");
					m_inventory.Add(itemRef);
				}
				if (hb == Follower.HeroBadge.Item_ShortSword && GameManager.m_gameManager.inventory.Count < GameManager.m_gameManager.maxBP)
				{
					GameObject randItem = (GameObject)Instantiate(GameManager.m_gameManager.m_itemBank[8], Vector3.zero, Quaternion.identity);
					Item itemRef = (Item)randItem.GetComponent("Item");
					m_inventory.Add(itemRef);
				}
				if (hb == Follower.HeroBadge.Item_Spellbook && GameManager.m_gameManager.inventory.Count < GameManager.m_gameManager.maxBP)
				{
					GameObject randItem = (GameObject)Instantiate(GameManager.m_gameManager.m_itemBank[56], Vector3.zero, Quaternion.identity);
					Item itemRef = (Item)randItem.GetComponent("Item");
					m_inventory.Add(itemRef);
				}
				if (hb == Follower.HeroBadge.Item_Crossbow && GameManager.m_gameManager.inventory.Count < GameManager.m_gameManager.maxBP)
				{
					GameObject randItem = (GameObject)Instantiate(GameManager.m_gameManager.m_itemBank[57], Vector3.zero, Quaternion.identity);
					Item itemRef = (Item)randItem.GetComponent("Item");
					m_inventory.Add(itemRef);
				}
				if (hb == Follower.HeroBadge.Item_Headbutt && GameManager.m_gameManager.inventory.Count < GameManager.m_gameManager.maxBP)
				{
					GameObject randItem = (GameObject)Instantiate(GameManager.m_gameManager.m_itemBank[34], Vector3.zero, Quaternion.identity);
					Item itemRef = (Item)randItem.GetComponent("Item");
					m_inventory.Add(itemRef);
				}
				if (hb == Follower.HeroBadge.Item_Dodge && GameManager.m_gameManager.inventory.Count < GameManager.m_gameManager.maxBP)
				{
					GameObject randItem = (GameObject)Instantiate(GameManager.m_gameManager.m_itemBank[28], Vector3.zero, Quaternion.identity);
					Item itemRef = (Item)randItem.GetComponent("Item");
					m_inventory.Add(itemRef);
				}
				if (hb == Follower.HeroBadge.Item_Crystal && GameManager.m_gameManager.inventory.Count < GameManager.m_gameManager.maxBP)
				{
					GameObject randItem = (GameObject)Instantiate(GameManager.m_gameManager.m_itemBank[15], Vector3.zero, Quaternion.identity);
					Item itemRef = (Item)randItem.GetComponent("Item");
					m_inventory.Add(itemRef);
				}
				if (hb == Follower.HeroBadge.Item_Shard && GameManager.m_gameManager.inventory.Count < GameManager.m_gameManager.maxBP)
				{
					GameObject randItem = (GameObject)Instantiate(GameManager.m_gameManager.m_itemBank[14], Vector3.zero, Quaternion.identity);
					Item itemRef = (Item)randItem.GetComponent("Item");
					m_inventory.Add(itemRef);
				}
				if (hb == Follower.HeroBadge.Item_BleedingEdge && GameManager.m_gameManager.inventory.Count < GameManager.m_gameManager.maxBP)
				{
					GameObject randItem = (GameObject)Instantiate(GameManager.m_gameManager.m_itemBank[19], Vector3.zero, Quaternion.identity);
					Item itemRef = (Item)randItem.GetComponent("Item");
					m_inventory.Add(itemRef);
				}
				if (hb == Follower.HeroBadge.Item_Venom && GameManager.m_gameManager.inventory.Count < GameManager.m_gameManager.maxBP)
				{
					GameObject randItem = (GameObject)Instantiate(GameManager.m_gameManager.m_itemBank[25], Vector3.zero, Quaternion.identity);
					Item itemRef = (Item)randItem.GetComponent("Item");
					m_inventory.Add(itemRef);
				}
				if (hb == Follower.HeroBadge.Item_Shovel && GameManager.m_gameManager.inventory.Count < GameManager.m_gameManager.maxBP)
				{
					GameObject randItem = (GameObject)Instantiate(GameManager.m_gameManager.m_itemBank[54], Vector3.zero, Quaternion.identity);
					Item itemRef = (Item)randItem.GetComponent("Item");
					m_inventory.Add(itemRef);
				}
				if (hb == Follower.HeroBadge.Item_Panacea && GameManager.m_gameManager.inventory.Count < GameManager.m_gameManager.maxBP)
				{
					GameObject randItem = (GameObject)Instantiate(GameManager.m_gameManager.m_itemBank[21], Vector3.zero, Quaternion.identity);
					Item itemRef = (Item)randItem.GetComponent("Item");
					m_inventory.Add(itemRef);
				}
				if (hb == Follower.HeroBadge.Item_BattleCry && GameManager.m_gameManager.inventory.Count < GameManager.m_gameManager.maxBP)
				{
					GameObject randItem = (GameObject)Instantiate(GameManager.m_gameManager.m_itemBank[33], Vector3.zero, Quaternion.identity);
					Item itemRef = (Item)randItem.GetComponent("Item");
					m_inventory.Add(itemRef);
				}
				if (hb == Follower.HeroBadge.Item_SpikedArmor && GameManager.m_gameManager.inventory.Count < GameManager.m_gameManager.maxBP)
				{
					GameObject randItem = (GameObject)Instantiate(GameManager.m_gameManager.m_itemBank[39], Vector3.zero, Quaternion.identity);
					Item itemRef = (Item)randItem.GetComponent("Item");
					m_inventory.Add(itemRef);
				}
				if (hb == Follower.HeroBadge.Item_Insight && GameManager.m_gameManager.inventory.Count < GameManager.m_gameManager.maxBP)
				{
					GameObject randItem = (GameObject)Instantiate(GameManager.m_gameManager.m_itemBank[40], Vector3.zero, Quaternion.identity);
					Item itemRef = (Item)randItem.GetComponent("Item");
					m_inventory.Add(itemRef);
				}
			}
		}

		//load badges onto heroes in UI
		if (SettingsManager.m_settingsManager.partySlots.Count > 0) {
			Debug.Log("LOADING BADGES FROM PARTY SLOTS");
			for (int i=0; i < SettingsManager.m_settingsManager.partySlots.Count; i++)
			{
				PartySelectMenu.PartySlot ps = (PartySelectMenu.PartySlot) SettingsManager.m_settingsManager.partySlots[i];
				if (ps.m_followerState == PartySelectMenu.PartySlot.State.Occupied && ps.m_badgeState == PartySelectMenu.PartySlot.State.Occupied)
				{
					GameObject go = (GameObject) Instantiate(ps.m_badge, Vector3.zero, ps.m_badge.transform.rotation);
					Badge b = (Badge)go.GetComponent("Badge");
					PartyCards.m_badges.Add(b);
					PartyCards.m_partyCards.m_party[i].m_miscOBJ[4].gameObject.SetActive(true);

					// set mouse hover text from badge
					MouseHover m = (MouseHover) PartyCards.m_partyCards.m_party[i].m_miscOBJ[4].GetComponent("MouseHover");
					m.m_text = b.m_badgeDescription;
				}
			}
		}

		Player.m_player.RefillHealth();
		Player.m_player.RefillEnergy();
		StartCoroutine( GameManager.m_gameManager.ShowFollower (Player.m_player.playerFollower, false));
		UIManager.m_uiManager.UpdateXP(m_accruedXP);
		drawCost = m_drawCost + m_drawCostMod;
		
		DHeart.m_dHeart.SetLevel(SettingsManager.m_settingsManager.difficultyLevel);

		FollowCamera.m_followCamera.SetTarget(Player.m_player.m_cameraTarget);

		// Initialize Grave
		for (int i=0; i <4; i++)
		{
			GraveSlot gs = new GraveSlot();
			gs.type = GraveSlot.ObjectType.None;
			m_grave.Add(gs);
		}

		// play chapter name fly in
		if (!m_quickStart) {
			AssetManager.m_assetManager.m_props [25].gameObject.SetActive (true);

			float time = 0.0f;
			while (time < 2.0f) {
				time += Time.deltaTime;
				float a = Mathf.Lerp (0.0f, 1.0f, time / 2.0f);
				Color c = AssetManager.m_assetManager.m_typogenicText [10].ColorTopLeft;
				c.a = a;
				AssetManager.m_assetManager.m_typogenicText [10].ColorTopLeft = c;
				AssetManager.m_assetManager.m_typogenicText [10].ColorBottomLeft = c;
				AssetManager.m_assetManager.m_typogenicText [10].ColorTopRight = c;
				AssetManager.m_assetManager.m_typogenicText [10].ColorBottomRight = c;
			
				AssetManager.m_assetManager.m_typogenicText [11].ColorTopLeft = c;
				AssetManager.m_assetManager.m_typogenicText [11].ColorBottomLeft = c;
				AssetManager.m_assetManager.m_typogenicText [11].ColorTopRight = c;
				AssetManager.m_assetManager.m_typogenicText [11].ColorBottomRight = c;

				c = AssetManager.m_assetManager.m_props [33].renderer.material.color;
				c.a = a;
				AssetManager.m_assetManager.m_props [33].renderer.material.color = c;
			
				yield return null;
			}
			yield return new WaitForSeconds (3.0f);

			time = 0.0f;
			while (time < 2.0f) {
				time += Time.deltaTime;
				float a = Mathf.Lerp (1.0f, 0.0f, time / 2.0f);
				Color c = AssetManager.m_assetManager.m_typogenicText [10].ColorTopLeft;
				c.a = a;
				AssetManager.m_assetManager.m_typogenicText [10].ColorTopLeft = c;
				AssetManager.m_assetManager.m_typogenicText [10].ColorBottomLeft = c;
				AssetManager.m_assetManager.m_typogenicText [10].ColorTopRight = c;
				AssetManager.m_assetManager.m_typogenicText [10].ColorBottomRight = c;

				AssetManager.m_assetManager.m_typogenicText [11].ColorTopLeft = c;
				AssetManager.m_assetManager.m_typogenicText [11].ColorBottomLeft = c;
				AssetManager.m_assetManager.m_typogenicText [11].ColorTopRight = c;
				AssetManager.m_assetManager.m_typogenicText [11].ColorBottomRight = c;

				c = AssetManager.m_assetManager.m_props [33].renderer.material.color;
				c.a = a;
				AssetManager.m_assetManager.m_props [33].renderer.material.color = c;

				yield return null;
			}

			AssetManager.m_assetManager.m_props [25].gameObject.SetActive (false);


			FollowCamera.m_followCamera.ChangeZoomDistance (1.0f);

			SpawnHeroCards (m_followers);

			yield return new WaitForSeconds (0.5f);
			time = 0.0f;
			while (time < 1.5f) {
				time += Time.deltaTime;
				float a = Mathf.Lerp (1.0f, 0.0f, time / 1.0f);
				Color c = AssetManager.m_assetManager.m_uiSprites [0].color;
				c.a = a;
				AssetManager.m_assetManager.m_uiSprites [0].color = c;
				yield return null;
			}
			AssetManager.m_assetManager.m_uiSprites [0].gameObject.SetActive (false);
			AssetManager.m_assetManager.m_uiSprites [0].color = Color.white;

			yield return new WaitForSeconds (0.25f);

			time = 0.0f;
			while (time < 2.0f) {
				time += Time.deltaTime;

				float change = Mathf.SmoothStep (1.0f, 0.5f, time / 2.0f);
				//Debug.Log(change);
				FollowCamera.m_followCamera.SetZoomDistance (change);
				yield return null;
			}

			yield return StartCoroutine (ShuffleCards ());
		}

		if (m_doStartItems)
		{
			foreach (GameObject go in m_startItems) {
				GameObject randItem = (GameObject)Instantiate(go, Vector3.zero, Quaternion.identity);
				Item itemRef = (Item)randItem.GetComponent("Item");
				m_inventory.Add(itemRef);
			}
		} else {
			yield return StartCoroutine(FillHand());		
		}

		yield return new WaitForSeconds(0.5f);
		acceptInput = true;
	}

	private void PopulateChestDeck (List<Follower> party)
	{

//		foreach (Follower f in party) {
//			Follower.Deck[] d = f.m_deck;
//			for (int i=0; i < d.Length; i++)
//			{
//				if (d[i].m_levelCards.Length > 0)
//				{
//					foreach (GameObject g in d[i].m_levelCards)
//					{
//						m_chestDeck.Add(g);
//					}
//				}
//			}
//		}

		foreach (GameObject g in m_chestBank) {
			m_chestDeck.Add(g);
		}

		int numBonusCards = (int)(m_chestDeck.Count * 0.20f);
		for (int i=0; i < numBonusCards; i++) {
			m_chestDeck.Add(m_itemBank[4]);
		}
	}

	private void PopulateDrawDeck (List<Follower> party)
	{
		foreach (Follower f in party) {
			Follower.Deck[] d = f.m_deck;
			for (int i=0; i < d.Length; i++)
			{
				if (i < f.currentLevel && i < d.Length)
				{
					if (d[i].m_levelCards.Length > 0)
					{
						foreach (GameObject g in d[i].m_levelCards)
						{
							m_drawDeck.Add(g);
						}
					}
				}
			}
		}
	}

	private void PopulateDrawDeck ()
	{
		foreach (LootTable lt in m_classDecks)
		{
			GameObject[] table = lt.m_lootTable;
			
			foreach (GameObject go in table)
			{
				if (m_drawDeck.Count < 5)
				{
					m_drawDeck.Add(go);
				} else {
					m_drawDeck.Insert(Random.Range(0, m_drawDeck.Count), go);
				}
			}
		}
	}

	private List<GameObject> heroCards = new List<GameObject>();
	public void SpawnHeroCards (List<Follower> party)
	{
		for (int i=0; i < party.Count; i++)
		{
			Follower f = party[i];
			Follower.Deck[] d = f.m_deck;
			int numCards = 0;

			//determine number of card backs needed
			for (int j=0; j < d.Length; j++)
			{
				if (j < f.currentLevel && j < d.Length)
				{
					if (d[j].m_levelCards.Length > 0)
					{
						foreach (GameObject g in d[j].m_levelCards)
						{
							numCards ++;
						}
					}
				}
			}

			//spawn card backs
			Vector3 startPos = m_heroCardsStart[i].transform.position;
			Vector3 endPos = m_heroCardsEnd[i].transform.position;
			int numC = Mathf.Clamp( numCards-1, 1, 99);
			for (int j=0; j < numCards; j++)
			{
				Vector3 cardPos = Vector3.Lerp(startPos, endPos, ((float)j) / ((float)numC));
				Vector3 cardRot = new Vector3(0,0,20);
				GameObject fCard = (GameObject)Instantiate(UIManager.m_uiManager.m_cardSmall, cardPos, UIManager.m_uiManager.m_cardSmall.transform.rotation);
				fCard.transform.parent = UIManager.m_uiManager.m_craftingUI.transform;
				heroCards.Add(fCard);
				UICard cardUI = (UICard)fCard.GetComponent("UICard");
				cardUI.m_portrait.spriteName = "Card_Back03";
				fCard.transform.localScale = UIManager.m_uiManager.miniScale;
			}

		}
	}

	public IEnumerator moveCard (Transform target, Transform[] path, Vector3 startScale, Vector3 endScale)
	{
		float t = 0;
		float time = 1.5f;

		while (t < time)
		{
			t += Time.deltaTime;

			iTween.PutOnPath(target, path, t / time);
			
//			Vector3 nPos = Vector3.Lerp(startPos, endPos , t / time);
			Vector3 newScale = Vector3.Lerp(startScale, endScale, t / time);
			//				go.transform.position = nPos;
			target.localScale = newScale;
			yield return null;
			
		}

		Destroy (target.gameObject);
	}

	public IEnumerator ShuffleCards ()
	{
		yield return new WaitForSeconds(1.0f);
		foreach (GameObject go in heroCards)
		{
			StartCoroutine(MoveCardToGrave(go));
//			float t = 0;
//			float time = 0.5f;
//			Vector3 startPos = go.transform.position;
//			Vector3 endPos = AssetManager.m_assetManager.m_props [34].transform.position;
//			Vector3 startScale = go.transform.localScale;
//			Vector3 endScale = startScale * 1.5f;
//
//			Transform[] path = new Transform[2];
//			path[0] = go.transform;
//			//path[1] = UIManager.m_uiManager.m_HUD.transform;
//			path[1] = AssetManager.m_assetManager.m_props [34].transform;
//
//			StartCoroutine(moveCard(go.transform, path, startScale, endScale));
			


			yield return new WaitForSeconds(0.07f);
		}

		yield return new WaitForSeconds(1.0f);

		yield return null;
	}

	private IEnumerator MoveCardToGrave (GameObject go)
	{
		float t = 0;
		float time = 0.5f;
		Vector3 startPos = go.transform.position;
		Vector3 endPos = AssetManager.m_assetManager.m_props [34].transform.position;
		Vector3 startScale = go.transform.localScale;
		Vector3 endScale = startScale * 1.2f;
		
		while (t < time)
		{
			t += Time.deltaTime;;
			Vector3 nPos = Vector3.Lerp(startPos, endPos , t / time);
			Vector3 newScale = Vector3.Lerp(startScale, endScale, t / time);
			go.transform.position = nPos;
			go.transform.localScale = newScale;
			yield return null;
			
		}

		Destroy (go);
	}



	public IEnumerator FillHand ()
	{
		Debug.Log ("Fill Hand");
		bool didChange = false;

		while (inventory.Count < maxBP)
		{
			//didChange = true;
			Item item = (Item) GetItemFromDeck();
			inventory.Add(item);

			// animate face down card moving to deck
			Vector3 cardStartPos = AssetManager.m_assetManager.m_props [34].transform.position;
			GameObject fCard = (GameObject)Instantiate(UIManager.m_uiManager.m_cardSmall, cardStartPos, UIManager.m_uiManager.m_cardSmall.transform.rotation);
			UICard card = (UICard)fCard.GetComponent("UICard");
			card.m_portrait.spriteName = "Card_Back03";

			float t = 0;
			float time = 0.5f;
			Vector3 startPos = cardStartPos;
			Vector3 startScale = fCard.transform.localScale * 1.25f;
			Vector3 endPos = AssetManager.m_assetManager.m_props[16].transform.position;
			Vector3 endScale = fCard.transform.localScale;
			while (t < time)
			{
				t += Time.deltaTime;;
				Vector3 nPos = Vector3.Lerp(startPos, endPos , t / time);
				Vector3 newScale = Vector3.Lerp(startScale, endScale, t / time);
				card.transform.localPosition = nPos;
				card.transform.localScale = newScale;
				yield return null;
			}
			//card.transform.localPosition = endPos;
			//card.transform.localScale = endScale;
			Destroy(card.gameObject);
			UIManager.m_uiManager.RefreshInventoryMenu ();

		}

//		if (didChange) {
//			UIManager.m_uiManager.RefreshInventoryMenu ();
//		}

		yield return true;
	}

	public Item GetItemFromChest ()
	{
		Debug.Log ("GETTING ITEM FROM CHEST");
		int i = Random.Range (0, m_chestDeck.Count);
		GameObject go = (GameObject) Instantiate( m_chestDeck [i], Vector3.zero, Quaternion.identity);
		//GameObject go = (GameObject)Instantiate (m_itemBank [4], Vector3.zero, Quaternion.identity);
		Item item = (Item)go.GetComponent ("Item");
		m_chestDeck.RemoveAt (i);
		if (m_chestDeck.Count == 0)
		{
			PopulateChestDeck(m_followers);
		}

		// adjust cost based on current leader
		item.adjustedEnergyCost = item.m_energyCost;
		item.adjustedHealthCost = item.m_healthCost;
		
		if (m_currentFollower != null)
		{
			if (item.m_class == m_currentFollower.m_followerClass)
			{
				if (item.m_energyCost > 0)
				{
					item.adjustedEnergyCost = 0;
				}
				if (item.m_healthCost > 0)
				{
					item.adjustedHealthCost = 0;
				}
			}
		}

		if (item.HasKeyword (Item.Keyword.WhileInHand)) {
			StartCoroutine(item.Activate());
		}
		
		return item;
	}

	public Item GetItemFromDeck ()
	{
		Debug.Log ("GETTING ITEM FROM DECK");
		int i = Random.Range (0, m_drawDeck.Count);
		GameObject go = (GameObject) Instantiate( m_drawDeck [i], Vector3.zero, Quaternion.identity);
		Item item = (Item)go.GetComponent ("Item");
		m_drawDeck.RemoveAt (i);
		if (m_drawDeck.Count == 0)
		{
			PopulateDrawDeck(m_followers);
		}

		// adjust cost based on current leader
		item.adjustedEnergyCost = item.m_energyCost;
		item.adjustedHealthCost = item.m_healthCost;

		if (m_currentFollower != null)
		{
			if (item.m_class == m_currentFollower.m_followerClass && GameManager.m_gameManager.followers.Count > 1)
			{
				if (item.m_energyCost > 0)
				{
					item.adjustedEnergyCost = 0;
				}
				if (item.m_healthCost > 0)
				{
					item.adjustedHealthCost = 0;
				}
			}
		}

		if (item.HasKeyword (Item.Keyword.WhileInHand)) {
			StartCoroutine(item.Activate());
		}

		return item;
	}

	private float EaseInOut (float t, float b, float c, float d) {
		t /= d/2;
		if (t < 1) return c/2*t*t + b;
		t--;
		return -c/2 * (t*(t-2) - 1) + b;
	}
	
	void Update () {

		if (UIManager.m_uiManager.menuMode == UIManager.MenuMode.None && Player.m_player != null && acceptInput)
		{
			Vector3 rot = Player.m_player.m_activityIndicator.transform.eulerAngles;
			rot.y += Time.deltaTime * 100;
			Player.m_player.m_activityIndicator.transform.eulerAngles = rot;
		}

		if (UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
		{
			if (!m_selectMode)
			{
				StartCoroutine(InputManager.m_inputManager.DoUpdate());
			}
			if (currentTurn != Turn.Player)
			{
		
				InputManager.m_inputManager.DoHoverHand();
			
			}
		}
	}
	
	public IEnumerator EnemyTurn(List<Enemy> activeEnemies)
	{
			
		if (UIManager.m_uiManager.targetDisplayed)
		{
			StartCoroutine(UIManager.m_uiManager.TurnOffTargetCard());
		}

		//do enemy turns
		while (activeEnemies.Count > 0)
		{
			Enemy activeEnemy = activeEnemies[0];

			//yield return StartCoroutine(UIManager.m_uiManager.DisplayTargetEnemy(activeEnemy));
			activeEnemies.RemoveAt(0);
			//UIManager.m_uiManager.DisplayTargetCard(activeEnemy);
			if (UIManager.m_uiManager.menuMode != UIManager.MenuMode.GameOver)
			{
				yield return StartCoroutine(activeEnemy.DoTurn());
				yield return StartCoroutine(activeEnemy.EndTurn());
			}
			//UIManager.m_uiManager.TurnOffTargetCard();
			//StartCoroutine(UIManager.m_uiManager.TurnOffTargetCard());
			yield return null;
		}
		yield return new WaitForSeconds(0.5f);
		Debug.Log("Enemies DONE");
	}
	
//	IEnumerator EnvironmentTurn (List<Card> mapCards)
//	{
//		foreach (Card thisCard in mapCards)
//		{
//			yield return StartCoroutine(thisCard.DoTurn());	
//		}
//		yield return null;	
//	}
	
	public IEnumerator Changeturn (Turn newTurn)
	{
		Debug.Log ("CHANGE TURN: " + newTurn.ToString ());

		StartCoroutine (UIManager.m_uiManager.UpdateStack ());
		m_currentTurn = newTurn;	
		m_currentChain = 0;
		UIManager.m_uiManager.UpdateChainGUI(m_currentChain);

		while (m_deadEnemies.Count > 0)
		{
			Enemy deadEnemy = m_deadEnemies[0];
			m_deadEnemies.RemoveAt(0);
			//Destroy(deadEnemy.gameObject);
		}
		
		if (newTurn == Turn.Enemy)
		{
			// do pet turn
			foreach (Pet p in currentMap.m_pets)
			{
				yield return new WaitForSeconds(0.5f);
				yield return StartCoroutine(p.DoTurn());
			}

			//find all active enemies
			List<Enemy> activeEnemies = new List<Enemy>();
			foreach (Enemy thisEnemy in m_currentMap.m_enemies)
			{
				thisEnemy.Upkeep();
//				if (thisEnemy.enemyState != Enemy.EnemyState.Dead && thisEnemy.enemyState != Enemy.EnemyState.Inactive)
				if (thisEnemy.enemyState == Enemy.EnemyState.Active || (thisEnemy.currentCard.cardState == Card.CardState.Normal && thisEnemy.currentCard.doUpkeep) )
				{ 
					activeEnemies.Add(thisEnemy);	
				}
			}
			if (activeEnemies.Count > 0)
			{
				acceptInput = false;
				//CurrentTurnIcon.m_currentTurnIcon.ChangeTurn (newTurn);
				yield return StartCoroutine(EnemyTurn(activeEnemies));
			} 

			if (UIManager.m_uiManager.menuMode != UIManager.MenuMode.GameOver)
			{
				foreach (Enemy e in m_currentMap.m_enemies)
				{
					if (e.activatedThisTurn)
					{
						e.activatedThisTurn = false;
					}
				}
				OrderEnemies();
				yield return StartCoroutine(Changeturn(Turn.Player));
			}
			yield break;
		} else if (newTurn == Turn.Player)
		{
			//StartCoroutine(PartyCards.m_partyCards.CleanSkills());
			drawCost = m_drawCost;
			OrderEnemies();
			m_currentTurnNum ++;
			//CurrentTurnIcon.m_currentTurnIcon.m_turnNum.text = "Turns this Level: " + m_currentTurnNum.ToString ();
			CurrentTurnIcon.m_currentTurnIcon.ChangeTurn (newTurn);

			//refresh any spent followers
//			foreach (Follower thisFollower in m_followers)
//			{
//				if (thisFollower.followerState == Follower.FollowerState.Spent)
//				{
//					yield return StartCoroutine(thisFollower.ChangeState(Follower.FollowerState.Normal));	
//				}
//			}
			
			//refresh spent equipment
//			foreach (Item equippedItem in m_equippedItems)
//			{
//				if (equippedItem.itemState == Item.ItemState.Spent)
//				{
//					equippedItem.ChangeState(Item.ItemState.Normal);	
//				}
//			}
			
			//do upkeep on followers
//			foreach (Follower thisFollower in m_followers)
//			{
//				yield return StartCoroutine(thisFollower.DoTurn());	
//			}
			
			//update effects that last until next turn
			EffectsPanel.m_effectsPanel.UpdateEffects(EffectsPanel.Effect.Duration.EndOfTurn);
			
			FollowCamera.m_followCamera.SetTarget(Player.m_player.m_cameraTarget);

			yield return StartCoroutine(Player.m_player.StartTurn());
			acceptInput = true;
			yield break;
		} 
//		else if (newTurn == Turn.Environment)
//		{
//			List<Card> upkeepCards = new List<Card>();
//			foreach (Card thisCard in m_currentMap.m_cards)
//			{
//				if (thisCard.doUpkeep && thisCard.cardState == Card.CardState.Normal && thisCard.isOccupied)
//				{
//					upkeepCards.Add(thisCard);	
//				}
//			}
//
//			if (upkeepCards.Count > 0)
//			{
//				acceptInput = false;
//				CurrentTurnIcon.m_currentTurnIcon.ChangeTurn (newTurn);
//				yield return StartCoroutine(EnvironmentTurn(upkeepCards));
//			}
//			yield return StartCoroutine(Changeturn(Turn.Player));
//			yield return null;
//		}
		yield return null;
	}

	public void ShiftFocus ()
	{
		if ((m_focusGO == null || m_focusGO == Player.m_player.gameObject) && m_distOrderedEnemies.Count > 0)
		{
			m_focusGO = m_distOrderedEnemies[0].gameObject;
			FollowCamera.m_followCamera.SetTarget(m_focusGO);
		} else if (m_distOrderedEnemies.Count > 0) {

			for (int i=0; i < m_distOrderedEnemies.Count; i++)
			{
				GameObject go = m_distOrderedEnemies[i].gameObject;
				if (go == m_focusGO)
				{
					if (i+1 >= m_distOrderedEnemies.Count) //end of list, set focus back to player
					{
						m_focusGO = Player.m_player.gameObject;
						FollowCamera.m_followCamera.SetTarget(m_focusGO);
						break;
					} else // set focus to next enemy in queue
					{
						m_focusGO = m_distOrderedEnemies[i+1].gameObject;
						FollowCamera.m_followCamera.SetTarget(m_focusGO);
						break;
					}
				}
			}
		}
	}

	private void OrderEnemies () //arrange enemies in list by distance for tabbing through them
	{
		m_distOrderedEnemies.Clear ();
		foreach (Enemy e in m_currentMap.m_enemies)
		{
			if (e.enemyState == Enemy.EnemyState.Active || e.enemyState == Enemy.EnemyState.Idle && e.currentCard.distanceToPlayer < 10)
			{
				m_distOrderedEnemies.Add(e);
			}
		}

		if (m_distOrderedEnemies.Count > 0)
		{
			//m_distOrderedEnemies.Sort(delegate(Enemy i1, Enemy i2) { return i1.currentCard.distanceToPlayer.CompareTo(i2.currentCard.distanceToPlayer); });
			m_distOrderedEnemies.Sort(delegate(Enemy i1, Enemy i2) { return i2.initiative.CompareTo(i1.initiative); });

		}

//		foreach (Enemy e in m_distOrderedEnemies)
//		{
//			Debug.Log("ENEMY DIST: " + e.currentCard.distanceToPlayer);
//		}
	}
	
	public bool StartCrafting ()
	{
//		UICard[] craftingSlots = UIManager.m_uiManager.m_craftingSlots;
//		
//		//make sure a valid combo exists
//		bool validCombo = false;
//		if (craftingSlots[0].m_nameUI.text == craftingSlots[1].m_nameUI.text && craftingSlots[0].m_nameUI.text == craftingSlots[2].m_nameUI.text && craftingSlots[0].m_nameUI.text != "Name")
//		{
//			validCombo = true;
//		}
//		
//		if (validCombo)
//		{
//			GameObject itemGO = (GameObject)Instantiate(craftingSlots[0].m_itemData.m_craftResult, Vector3.zero, Quaternion.identity);
//			Item newItem = (Item)itemGO.GetComponent("Item");
//			m_inventory.Add(newItem);
//			
//			
//			List<Item> usedItems = new List<Item>();
//			
//			//foreach (UICard thisCard in craftingSlots)
//			for (int i=0; i < craftingSlots.Length; i++)
//			{
//				UICard thisCard = (UICard)craftingSlots[i];
//				if (i < craftingSlots.Length-1)
//				{
//					usedItems.Add(thisCard.m_itemData);
//				}
//				thisCard.Deactivate();	
//			}
//			
//			//remove used resources from inventory
//			foreach (Item usedItem in usedItems)
//			{
//				for (int i=0; i < m_inventory.Count; i++)
//				{
//					Item inv = (Item)m_inventory[i];
//					if (inv == usedItem)
//					{
//						m_inventory.RemoveAt(i);
//						i=99;
//					}
//				}
//			}
//			
////			foreach (Item usedItem in usedItems)
////			{
////				Destroy(usedItem.gameObject);	
////			}
//			while (usedItems.Count > 0)
//			{
//				Item usedItem = usedItems[0];
//				usedItems.RemoveAt(0);
//				Destroy(usedItem.gameObject);
//				//DestroyImmediate(usedItem, false);
//			}
//			
//			List<GameObject> unusedCards = new List<GameObject>();
//			for (int i=0; i < UIManager.m_uiManager.cardList.Count; i++)
//			{
//				//GameObject thisCard = UIManager.m_uiManager.cardList[i];
//				UICard thisCard = (UICard)UIManager.m_uiManager.cardList[i].transform.GetComponent("UICard");
//				if (thisCard.m_portrait.spriteName == "Card_Back03")
//				{
//					thisCard.transform.parent = null;
//					Destroy(thisCard.gameObject);	
//				} else {
//					thisCard.transform.parent = null;
//					unusedCards.Add(thisCard.gameObject);	
//				}
//			}
//			
//			//add new item to crafting item list
//			if (newItem.HasKeyword(Item.Keyword.Craftable))
//			{
//				GameObject fCard = (GameObject)Instantiate(UIManager.m_uiManager.m_itemCard, UIManager.m_uiManager.m_itemCard.transform.position, UIManager.m_uiManager.m_itemCard.transform.rotation);	
//				//UIManager.m_uiManager.cardList.Add(fCard);
//				unusedCards.Add(fCard);
//				UICard cardUI = (UICard)fCard.GetComponent("UICard");
//				fCard.name = newItem.m_name;
//				cardUI.m_nameUI.text = newItem.m_name;
//				cardUI.m_abilityUI.text = newItem.m_description;
//				cardUI.m_portrait.spriteName = newItem.m_portraitSpriteName;
//				cardUI.itemData = newItem;
//			}
//			
//			UIManager.m_uiManager.cardList = unusedCards;
//			
//			foreach (GameObject craftCard in unusedCards)
//			{
//				Transform cardParent = UIManager.m_uiManager.m_craftingUI.transform.Find("InventoryPanel");
//				craftCard.transform.parent = cardParent;	
//				craftCard.transform.localPosition = Vector3.zero;
//			}
//			
//			UIGrid grid = (UIGrid) UIManager.m_uiManager.m_craftingUI.transform.Find("InventoryPanel").GetComponent("UIGrid");
//			grid.Reposition();
//			
//			return true;
//		}
//		
		return false;
	}
	
	public bool HasFollower (Follower.FollowerType fType)
	{
		foreach (Follower thisF in followers)
		{
			if (thisF.m_followerType == fType)
			{
				return true;	
			}
		}
		return false;
	}

	public IEnumerator SwapHeroMesh (Follower f)
	{
		Instantiate (AssetManager.m_assetManager.m_particleFX [0], f.m_followerMesh.transform.position, AssetManager.m_assetManager.m_particleFX [0].transform.rotation);

		if (f.m_followerType != m_currentFollower.m_followerType){m_currentFollower.m_followerMesh.gameObject.SetActive(false);m_currentFollower = f;}
		if (f.m_followerType != Player.m_player.playerFollower.m_followerType){Player.m_player.playerFollower.m_followerMesh.SetActive(false);}
		if (!f.m_followerMesh.gameObject.activeSelf) {f.m_followerMesh.gameObject.SetActive(true); }

		yield return null;
	}

	public IEnumerator ShowFollower (Follower f, bool useAction)
	{
		Debug.Log ("SWAPPING LEADER");

		string newString = "\\1" + f.m_nameText + "\\0 is now the Leader";
		UIManager.m_uiManager.UpdateActions (newString);

		Follower lastFollower = m_currentFollower;

		if (f.m_followerType != m_currentFollower.m_followerType){m_currentFollower.m_followerMesh.gameObject.SetActive(false);m_currentFollower = f;}
		if (f.m_followerType != Player.m_player.playerFollower.m_followerType){Player.m_player.playerFollower.m_followerMesh.SetActive(false);}
		if (!f.m_followerMesh.gameObject.activeSelf) {f.m_followerMesh.gameObject.SetActive(true); }

		if (Player.m_player.stunDuration > 0) {
			Player.m_player.GainEnergy(-2);
		}


		// update selector
		foreach (UICard c in PartyCards.m_partyCards.m_party)
		{
			if (c.m_followerData != null && m_followers.Count > 1)
			{
				if (c.m_followerData == f)
				{
//					AssetManager.m_assetManager.m_props[18].transform.position = c.m_portrait.transform.position;
//					break;

					c.m_miscOBJ[3].gameObject.SetActive(true);
					Vector3 newPos = c.transform.localPosition;
					newPos.x = -218.5371f;
					c.transform.localPosition = newPos;
					MouseHover m = (MouseHover) c.GetComponent("MouseHover");
					m.enabled = false;
				} else {
					c.m_miscOBJ[3].gameObject.SetActive(false);
					Vector3 newPos = c.transform.localPosition;
					newPos.x = -85.44141f;
					c.transform.localPosition = newPos;
					MouseHover m = (MouseHover) c.GetComponent("MouseHover");
					m.enabled = true;
				}
			}
		}

		// update card costs

		foreach (Item item in m_inventory) {
			item.adjustedEnergyCost = item.m_energyCost;
			item.adjustedHealthCost = item.m_healthCost;
			if (item.m_class == f.m_followerClass && GameManager.m_gameManager.followers.Count > 1) {
				if (item.m_energyCost > 0) {
					//item.adjustedEnergyCost = item.m_energyCost - 1;
					item.adjustedEnergyCost = 0;
				}
			
				if (item.m_healthCost > 0) {
					//item.adjustedHealthCost = item.m_healthCost - 1;
					item.adjustedHealthCost = 0;
				}
			}
		}
		

		UIManager.m_uiManager.RefreshInventoryMenu ();




		if (useAction) {
			Instantiate (AssetManager.m_assetManager.m_particleFX [0], f.m_followerMesh.transform.position, AssetManager.m_assetManager.m_particleFX [0].transform.rotation);
			yield return new WaitForSeconds (1.0f);
			Player.m_player.UseActionPoint ();
		} else {
			yield return new WaitForSeconds (1.0f);
		}

		yield return null;
	}
	
	public IEnumerator ActivateFollower (Follower thisF)
	{
		GameManager.m_gameManager.acceptInput = false;
		Follower.FollowerState oldState = thisF.followerState;
		//Check if the player is allowed to use Skills
		if (Player.m_player.currentCard.type == Card.CardType.Darkness)
		{
			yield break;
		}

		int currentEnergy = Player.m_player.currentEnergy;
		Follower.FollowerType fType = thisF.m_followerType;
		switch (fType)
		{
		case Follower.FollowerType.August:
			if (currentEnergy >= thisF.abilityCost && thisF.followerState == Follower.FollowerState.Normal)
			{
//				yield return StartCoroutine( ShowFollower(thisF));
				yield return new WaitForSeconds(0.5f);
				AssetManager.m_assetManager.PlaySFX(AssetManager.SFXType.SkillUsed);
				yield return StartCoroutine(thisF.ChangeState(Follower.FollowerState.Spent));
				Player.m_player.GainHealth(thisF.abilityEffect);
				DrainEnergy(thisF.abilityCost);

				UIManager.m_uiManager.SpawnFloatingText("+" + thisF.abilityEffect.ToString(), UIManager.Icon.Health, Player.m_player.m_playerMesh.transform);
				yield return new WaitForSeconds(1);
//				ShowFollower(Player.m_player.playerFollower);
			}
			break;
		case Follower.FollowerType.Jin:
			if (currentEnergy >= thisF.abilityCost && thisF.followerState == Follower.FollowerState.Normal)
			{
//				yield return StartCoroutine( ShowFollower(thisF));
				yield return new WaitForSeconds(0.5f);
				AssetManager.m_assetManager.PlaySFX(AssetManager.SFXType.SkillUsed);
				Player.m_player.GainActionPoints(thisF.abilityEffect);
				DrainEnergy(thisF.abilityCost);
				UIManager.m_uiManager.SpawnFloatingText("+" + thisF.abilityEffect.ToString() + "A", UIManager.Icon.None, Player.m_player.m_playerMesh.transform);
				yield return StartCoroutine(thisF.ChangeState(Follower.FollowerState.Spent));

				yield return new WaitForSeconds(1.0f);
//				ShowFollower(Player.m_player.playerFollower);
			}
			break;
		case Follower.FollowerType.Dancer:
//			if (currentEnergy >= thisF.abilityCost && thisF.followerState == Follower.FollowerState.Normal)
//			{
//				ShowFollower(thisF);
//				DrainEnergy(thisF.abilityCost);
//				Player.m_player.doDancer = true;
//				yield return StartCoroutine(thisF.ChangeState(Follower.FollowerState.Spent));
//			}
			break;
		case Follower.FollowerType.Telina:
			if (currentEnergy >= thisF.abilityCost && thisF.followerState == Follower.FollowerState.Normal)
			{
//				yield return StartCoroutine( ShowFollower(thisF));
				AssetManager.m_assetManager.PlaySFX(AssetManager.SFXType.SkillUsed);
				List<GameManager.Direction> directions = new List<Direction>();
				directions.Add(Direction.North);
				directions.Add(Direction.South);
				directions.Add(Direction.East);
				directions.Add(Direction.West);
				List<Card> validCards = MapManager.m_mapManager.GetCardsInRange(Player.m_player.currentCard, (thisF.abilityRange + Player.m_player.currentCard.siteRangeBonus), directions, false, true, false);
				if (validCards.Count > 0)
				{
					m_selectMode = true;
					yield return StartCoroutine(InputManager.m_inputManager.GetMouseSelection(validCards));	
					
					if (m_selectedCard != null)
					{
						if (m_selectedCard.enemy != null)
						{
							Enemy thisEnemy = m_selectedCard.enemy;
							DrainEnergy(thisF.abilityCost);

							bool addedAP = false;
							if (thisF.doArmorPierce && !Player.m_player.doArmorPierce)
							{
								Player.m_player.doArmorPierce = true;
								addedAP = true;
							}
							StartCoroutine(thisEnemy.TakeDamage(thisF.abilityEffect));
							if (Player.m_player.doPoisonAttack)
							{
								thisEnemy.ChangeEffectState(StatusEffect.Poison, 3);	
							}
							if (addedAP)
							{
								Player.m_player.doArmorPierce = false;
							}
//							if (!isAlive)
//							{
//								Player.m_player.GainActionPoints(1);
//							}
							yield return StartCoroutine(thisF.ChangeState(Follower.FollowerState.Spent));
						}
						//Player.m_player.UseActionPoint();
						m_selectedCard = null;
					} 
//					else {
//						foreach (Card vc in validCards)
//						{
//							vc.ChangeHighlightState(false);	
//						}
//						m_selectMode = false;
//						ShowFollower(Player.m_player.playerFollower);
//						GameManager.m_gameManager.acceptInput = true;
//						yield break;
//					}
					
					foreach (Card vc in validCards)
					{
						vc.ChangeHighlightState(false);	
					}
					m_selectMode = false;
					yield return new WaitForSeconds(1.0f);
//					ShowFollower(Player.m_player.playerFollower);
				}
			}
			break;
		case Follower.FollowerType.Ranger:
//			if (currentEnergy >= thisF.abilityCost && thisF.followerState == Follower.FollowerState.Normal)
//			{
//				ShowFollower(thisF);
//				List<GameManager.Direction> directions = new List<Direction>();
//				directions.Add(Player.m_player.facing);
//				List<Card> validCards = MapManager.m_mapManager.GetCardsInRange(Player.m_player.currentCard, 99, directions, false, false, true);
//				foreach (Card vc in validCards)
//				{
//					if (vc.cardState == Card.CardState.Hidden)
//					{
//						StartCoroutine(vc.ChangeCardState(Card.CardState.Normal));
//					}
//				}
//				Player.m_player.GainEnergy((thisF.abilityCost * -1) + validCards.Count);
//				//Player.m_player.UseActionPoint();
//				yield return StartCoroutine(thisF.ChangeState(Follower.FollowerState.Spent));
//			}
			break;
		case Follower.FollowerType.Seer:

//			if (currentEnergy >= thisF.abilityCost && thisF.followerState == Follower.FollowerState.Normal)
//			{
//				ShowFollower(thisF);
//				m_selectMode = true;
//				
//				List<GameManager.Direction> directions = new List<Direction>();
//				directions.Add(Direction.North);
//				directions.Add(Direction.South);
//				directions.Add(Direction.East);
//				directions.Add(Direction.West);
//				List<Card> validCards = MapManager.m_mapManager.GetCardsInRange(Player.m_player.currentCard, thisF.abilityRange + Player.m_player.currentCard.siteRangeBonus, directions, false, true, true);
//
//				yield return StartCoroutine(InputManager.m_inputManager.GetMouseSelection(validCards));	
//				
//				if (m_selectedCard != null)
//				{
//					if (m_selectedCard.cardState == Card.CardState.Hidden && m_selectedCard != Player.m_player.currentCard)
//					{
//						StartCoroutine(m_selectedCard.ChangeCardState(Card.CardState.Normal));	
//					} else if (m_selectedCard.cardState == Card.CardState.Normal && m_selectedCard != Player.m_player.currentCard)
//					{
//						StartCoroutine(m_selectedCard.ChangeCardState(Card.CardState.Hidden));
//					}
//				}
//				
//				foreach (Card vc in validCards)
//				{
//					vc.ChangeHighlightState(false);	
//				}
//				
//				DrainEnergy(thisF.abilityCost);
//				//Player.m_player.UseActionPoint();
//				m_selectMode = false;
//				m_selectedCard = null;
//				yield return StartCoroutine(thisF.ChangeState(Follower.FollowerState.Spent));
//			}
			break;
		case Follower.FollowerType.Pyromage:
//			if (currentEnergy >= thisF.abilityCost && thisF.followerState == Follower.FollowerState.Normal)
//			{
//				ShowFollower(thisF);
//				List<GameManager.Direction> directions = new List<Direction>();
//				directions.Add(Direction.North);
//				directions.Add(Direction.South);
//				directions.Add(Direction.East);
//				directions.Add(Direction.West);
//				List<Card> validCards = MapManager.m_mapManager.GetCardsInRange(Player.m_player.currentCard, thisF.abilityRange, directions, false, false, false);
//				foreach (Card vc in validCards)
//				{
//					if (vc.cardState != Card.CardState.Hidden && vc.enemy != null)
//					{
//						Enemy thisEnemy = vc.enemy;
//						FollowCamera.m_followCamera.SetTarget(thisEnemy.gameObject);
//						yield return new WaitForSeconds(1);
//						
//						StartCoroutine(thisEnemy.TakeDamage(thisF.abilityEffect));
//						if (Player.m_player.doPoisonAttack)
//						{
//							thisEnemy.ChangeEffectState(StatusEffect.Poison, 3);	
//						}
//					}
//					
//				}
//				FollowCamera.m_followCamera.SetTarget(Player.m_player.m_cameraTarget);
//				
//				
//				DrainEnergy(thisF.abilityCost);
//				//Player.m_player.UseActionPoint();
//				yield return StartCoroutine(thisF.ChangeState(Follower.FollowerState.Spent));
//			}
			break;
		case Follower.FollowerType.Dragoon:
			if (currentEnergy >= thisF.abilityCost && thisF.followerState == Follower.FollowerState.Normal)
			{
				List<Card> validCards = new List<Card>();

				for (int i=0; i<4; i++)
				{
					if (Player.m_player.currentCard.linkedCards[i] != null)
					{
						Card lc = Player.m_player.currentCard.linkedCards[i];
						
						if (lc.type != Card.CardType.Normal && lc.type != Card.CardType.Exit && lc.type != Card.CardType.Entrance && lc.type != Card.CardType.Gate)
						{
							validCards.Add(lc);
						}
					}
				}

				if (validCards.Count > 0)
				{
//					ShowFollower(thisF);
					m_selectMode = true;
					foreach (Card vc in validCards)
					{
						vc.ChangeHighlightState(true);	
					}
					yield return StartCoroutine(InputManager.m_inputManager.GetMouseSelection(validCards));	
					foreach (Card vc in validCards)
					{
						vc.ChangeHighlightState(false);	
					}
					if (m_selectedCard != null)
					{
						//turn this card into a normal card
						if (currentMap.m_mapType == MapManager.Map.MapType.Cave || currentMap.m_mapType == MapManager.Map.MapType.Plains)
						{
							Card newCard = ((Card)MapManager.m_mapManager.m_cardTypes[16].transform.GetComponent("Card"));
							Instantiate(AssetManager.m_assetManager.m_particleFX[2], m_selectedCard.transform.position, AssetManager.m_assetManager.m_particleFX[2].transform.rotation);
							yield return StartCoroutine(m_selectedCard.ChangeCardType(newCard));
						} else if (currentMap.m_mapType == MapManager.Map.MapType.Dungeon)
						{
							Card newCard = ((Card)MapManager.m_mapManager.m_cardTypes[4].transform.GetComponent("Card"));
							Instantiate(AssetManager.m_assetManager.m_particleFX[2], m_selectedCard.transform.position, AssetManager.m_assetManager.m_particleFX[2].transform.rotation);
							yield return StartCoroutine(m_selectedCard.ChangeCardType(newCard));
						} else if (currentMap.m_mapType == MapManager.Map.MapType.Molten)
						{
							Card newCard = ((Card)MapManager.m_mapManager.m_cardTypes[17].transform.GetComponent("Card"));
							Instantiate(AssetManager.m_assetManager.m_particleFX[2], m_selectedCard.transform.position, AssetManager.m_assetManager.m_particleFX[2].transform.rotation);
							yield return StartCoroutine(m_selectedCard.ChangeCardType(newCard));
						}
						DrainEnergy(thisF.abilityCost);
						yield return StartCoroutine(thisF.ChangeState(Follower.FollowerState.Spent));
						m_selectedCard = null;
					}
					m_selectMode = false;

					yield return new WaitForSeconds(1.0f);
//					ShowFollower(Player.m_player.playerFollower);
				}
				
//				List<GameManager.Direction> directions = new List<Direction>();
//				directions.Add(Player.m_player.facing);
//				List<Card> validCards = MapManager.m_mapManager.GetCardsInRange(Player.m_player.currentCard, 99, directions, false, false);
//
//				foreach (Card vc in validCards)
//				{
//					if (vc.cardState != Card.CardState.Hidden && vc.enemy != null)
//					{
//						Enemy thisEnemy = vc.enemy;
//						FollowCamera.m_followCamera.SetTarget(thisEnemy.gameObject);
//						yield return new WaitForSeconds(1);
//						
//						StartCoroutine(thisEnemy.TakeDamage(10));
//						if (Player.m_player.doPoisonAttack)
//						{
//							thisEnemy.ChangeEffectState(StatusEffect.Poison, 3);	
//						}
//					}
//					
//				}
//				FollowCamera.m_followCamera.SetTarget(Player.m_player.m_cameraTarget);
				

			}
			break;
		case Follower.FollowerType.Brand:
			if (currentEnergy >= thisF.abilityCost && thisF.followerState == Follower.FollowerState.Normal)
			{
//				yield return StartCoroutine( ShowFollower(thisF));
				yield return new WaitForSeconds(0.5f);

				AssetManager.m_assetManager.PlaySFX(AssetManager.SFXType.SkillUsed);
				Player.m_player.turnDamage += thisF.abilityEffect;
				Debug.Log("DAMAGE BONUS: " + thisF.abilityEffect);
				DrainEnergy(thisF.abilityCost);

				UIManager.m_uiManager.SpawnFloatingText("+" + thisF.abilityEffect.ToString(), UIManager.Icon.MeleeDamage, Player.m_player.m_playerMesh.transform);

				yield return StartCoroutine(thisF.ChangeState(Follower.FollowerState.Spent));
				StartCoroutine(UIManager.m_uiManager.UpdateDamage(Player.m_player.damage + Player.m_player.turnDamage + Player.m_player.tempDamage + Player.m_player.permDamage + Player.m_player.currentCard.siteDamageBonus));
				
				//Update Effect Stack
				EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
				newEffect.m_effectType = EffectsPanel.Effect.EffectType.Brand;
				newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
				string desc = "Brand: " + thisF.abilityEffect.ToString() + "$ until next turn.";
				newEffect.m_description = desc;
				newEffect.m_spriteName = "Effect_Brand";
				EffectsPanel.m_effectsPanel.AddEffect(newEffect);

				yield return new WaitForSeconds(1.0f);
//				ShowFollower(Player.m_player.playerFollower);
			}
			break;
		case Follower.FollowerType.Samurai:
			if (currentEnergy >= thisF.abilityCost && thisF.followerState == Follower.FollowerState.Normal)
			{
//				ShowFollower(thisF);
				yield return new WaitForSeconds(0.5f);

				Player.m_player.doSamurai = true;	
				Player.m_player.samuraiBonus = thisF.abilityEffect;
				DrainEnergy(thisF.abilityCost);
				yield return StartCoroutine(thisF.ChangeState(Follower.FollowerState.Spent));
				
				//Update Effect Stack
				EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
				newEffect.m_effectType = EffectsPanel.Effect.EffectType.Samurai;
				newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
				string desc = "+" + thisF.abilityEffect.ToString() + " Actions when defeating an enemy until next turn.";
				newEffect.m_description = desc;
				newEffect.m_spriteName = "Effect_Hero_Placeholder";
				EffectsPanel.m_effectsPanel.AddEffect(newEffect);

				yield return new WaitForSeconds(1.0f);
//				ShowFollower(Player.m_player.playerFollower);
				
			}
			
			break;
		case Follower.FollowerType.Knight:
			if (currentEnergy >= thisF.abilityCost && thisF.followerState == Follower.FollowerState.Normal)
			{
//				ShowFollower(thisF);
				yield return new WaitForSeconds(0.5f);

				Player.m_player.turnArmor += thisF.abilityEffect;
				DrainEnergy(thisF.abilityCost);
				StartCoroutine(UIManager.m_uiManager.UpdateArmor(Player.m_player.currentArmor + Player.m_player.tempArmor + Player.m_player.turnArmor + Player.m_player.permArmor + Player.m_player.currentCard.siteArmorBonus));
				UIManager.m_uiManager.SpawnFloatingText("+" + thisF.abilityEffect.ToString(), UIManager.Icon.Armor, Player.m_player.m_playerMesh.transform);
				yield return StartCoroutine(thisF.ChangeState(Follower.FollowerState.Spent));

				yield return new WaitForSeconds(1.0f);
//				ShowFollower(Player.m_player.playerFollower);
			}
			break;
		case Follower.FollowerType.Berserker:
//			if (currentEnergy >= thisF.abilityCost && thisF.followerState == Follower.FollowerState.Normal)
//			{
//				ShowFollower(thisF);
//				Player.m_player.berserkerActive = true;
//				DrainEnergy(thisF.abilityCost);	
//				yield return StartCoroutine(thisF.ChangeState(Follower.FollowerState.Spent));
//			}
			break;
		case Follower.FollowerType.Barbarian:
//			if (currentEnergy >= thisF.abilityCost && thisF.followerState == Follower.FollowerState.Normal)
//			{
//				ShowFollower(thisF);
//				Card[] linkedCards = Player.m_player.currentCard.linkedCards;
//				foreach (Card thisCard in linkedCards)
//				{
//					if (thisCard != null)
//					{
//						if (thisCard.enemy != null && thisCard.cardState != Card.CardState.Hidden)
//						{
//							Player.m_player.GainActionPoints(1);	
//						}
//					}
//				}
//				DrainEnergy(thisF.abilityCost);	
//				yield return StartCoroutine(thisF.ChangeState(Follower.FollowerState.Spent));
//			}
			break;
		case Follower.FollowerType.Mystic:
//			if (currentEnergy >= thisF.abilityCost && thisF.followerState == Follower.FollowerState.Normal)
//			{
//				ShowFollower(thisF);
//				Player.m_player.mysticActive = true;
//				DrainEnergy(thisF.abilityCost);	
//				yield return StartCoroutine(thisF.ChangeState(Follower.FollowerState.Spent));
//			}
			break;
		case Follower.FollowerType.Succubus:
//			if (Player.m_player.currentHealth > thisF.abilityCost && thisF.followerState == Follower.FollowerState.Normal)
//			{
//				ShowFollower(thisF);
//				yield return StartCoroutine(Player.m_player.TakeDamage(thisF.abilityCost));
//				Player.m_player.GainEnergy(thisF.abilityEffect);	
//				yield return StartCoroutine(thisF.ChangeState(Follower.FollowerState.Spent));
//			}
			break;
		case Follower.FollowerType.Lagomorph:
//			if (currentEnergy >= thisF.abilityCost && thisF.followerState == Follower.FollowerState.Normal)
//			{
//				ShowFollower(thisF);
//				Player.m_player.doPoisonAttack = true;
//				DrainEnergy(thisF.abilityCost);	
//				yield return StartCoroutine(thisF.ChangeState(Follower.FollowerState.Spent));
//			}
			break;
		case Follower.FollowerType.Wrestler:
			if (currentEnergy >= thisF.abilityCost && thisF.followerState == Follower.FollowerState.Normal)
			{
//				ShowFollower(thisF);
				yield return new WaitForSeconds(0.5f);

				Player.m_player.doCounterAttack = true;
				Player.m_player.counterBonus = thisF.abilityEffect;
				DrainEnergy(thisF.abilityCost);	
				if (thisF.abilityEffect == 0) {UIManager.m_uiManager.SpawnFloatingText("Counter Attack", UIManager.Icon.None, Player.m_player.m_playerMesh.transform);}
				else {UIManager.m_uiManager.SpawnFloatingText("Counter Attack +" + thisF.abilityEffect.ToString(), UIManager.Icon.None, Player.m_player.m_playerMesh.transform);}
				yield return StartCoroutine(thisF.ChangeState(Follower.FollowerState.Spent));
				
				//Update Effect Stack
				EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
				newEffect.m_effectType = EffectsPanel.Effect.EffectType.CounterAttack;
				newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
				string desc = "Counter Attack until next turn.";
				newEffect.m_description = desc;
				newEffect.m_stackable = false;
				newEffect.m_spriteName = "Effect_CounterAttack";
				EffectsPanel.m_effectsPanel.AddEffect(newEffect);

				yield return new WaitForSeconds(1.0f);
//				ShowFollower(Player.m_player.playerFollower);
			}
			break;
		case Follower.FollowerType.Psychic:
//			if (currentEnergy >= thisF.abilityCost && thisF.followerState == Follower.FollowerState.Normal)
//			{
//				ShowFollower(thisF);
//				Player.m_player.doPsychicAttack = true;
//				DrainEnergy(thisF.abilityCost);	
//				yield return StartCoroutine(thisF.ChangeState(Follower.FollowerState.Spent));
//			}
			break;
		case Follower.FollowerType.Fencer:
//			if (currentEnergy >= thisF.abilityCost && thisF.followerState == Follower.FollowerState.Normal)
//			{
//				ShowFollower(thisF);
//				Player.m_player.doFencer = true;
//				DrainEnergy(thisF.abilityCost);	
//				yield return StartCoroutine(thisF.ChangeState(Follower.FollowerState.Spent));
//			}
			break;
		case Follower.FollowerType.Monk:
			if (currentEnergy >= thisF.abilityCost && thisF.followerState == Follower.FollowerState.Normal)
			{
//				ShowFollower(thisF);
				yield return new WaitForSeconds(0.5f);

//				Player.m_player.GainActionPoints((Player.m_player.currentActionPoints - 1) * -1);
//				Player.m_player.UseActionPoint();	
				UIManager.m_uiManager.SpawnFloatingText("Knockback", UIManager.Icon.None, Player.m_player.m_playerMesh.transform);

			Card[] linkedCards = Player.m_player.currentCard.linkedCards;
			for (int i=0; i<linkedCards.Length; i++)
			{
				Card thisCard = linkedCards[i];
				if (thisCard != null)
				{
					if (thisCard.enemy != null && thisCard.cardState == Card.CardState.Normal)
					{
							if (thisF.abilityEffect > 0)
							{
								yield return StartCoroutine(thisCard.enemy.TakeDamage(thisF.abilityEffect));
								yield return new WaitForSeconds(0.25f);
							}

						Direction dir = Direction.North;
						if (i==1)
						{
							dir = Direction.South;
						} else if (i==2)
						{
							dir = Direction.East;	
						} else if (i==3)
						{
							dir = Direction.West;	
						}
							if (thisCard.enemy != null)
							{
								if (thisCard.enemy.CanBeKnockedBack(dir))
								{
									yield return StartCoroutine(thisCard.enemy.DoKnockback(dir));
								}
							}
					}
				}
			}
				DrainEnergy(thisF.abilityCost);	
				yield return StartCoroutine(thisF.ChangeState(Follower.FollowerState.Spent));

				yield return new WaitForSeconds(1.0f);
//				ShowFollower(Player.m_player.playerFollower);
			}
			break;
		}
		
		if ((Player.m_player.currentCard.type == Card.CardType.Stalactite || Player.m_player.currentCard.type == Card.CardType.ManaBurn) && (thisF.followerState == Follower.FollowerState.Spent && thisF.followerState != oldState  ))
		{
			yield return StartCoroutine(Player.m_player.currentCard.ActivateCard());	
		}

		GameManager.m_gameManager.acceptInput = true;
		//return false;
	}

	private void DrainEnergy (int DrainAmount)
	{
		if (Player.m_player.doSacrifice)
		{
			StartCoroutine(Player.m_player.TakeDirectDamage(DrainAmount));
		} else {
			Player.m_player.GainEnergy(DrainAmount * -1);
		}
	}
	
	public bool CheckLockState (Follower thisFollower)
	{
		foreach (GameState.ProgressState progressState in m_gameProgress)
		{
			if (thisFollower.m_followerType == progressState.m_followerType)
			{
				return progressState.m_isLocked;	
			}
		}
		
		return true;
	}
	
	public void LoadStorageItem (string itemName)
	{
//		foreach (GameObject thisItem in m_itemBank)
//		{
//			string thisItemName = ((Item)thisItem.GetComponent("Item")).m_storageName;
//			if (itemName == thisItemName)
//			{
//				GameObject randItem = (GameObject)Instantiate(thisItem, Vector3.zero, Quaternion.identity);
//				Item itemRef = (Item)randItem.GetComponent("Item");
//				m_storageItems.Add(itemRef);
//			}
//		}
	}

	public void ToggleStatBars ()
	{
		if (m_doStatBar)
		{
			m_doStatBar = false;
		} else {
			m_doStatBar = true;
		}

		foreach (Enemy e in m_currentMap.m_enemies)
		{
			e.ToggleStatBar(m_doStatBar);
		}
	}

//	public void AddLimboCard (Item newItem)
//	{
//		Debug.Log ("ADDING CARD TO LIMBO");
//		Item item = newItem;
//		int i = 0;
//
//		while (item != null && i < m_limboCards.Length)
//		{
//			Item thisItem = m_limboCards[i];
//			m_limboCards[i] = item;
//			item = thisItem;
//			i++;
//		}
//
//		if (item != null)
//		{
//			Debug.Log("SENDING ITEM TO GRAVEYARD");
//			Destroy(item.gameObject);
//		}
//	}

	public void SendToGrave (Item item)
	{
		Debug.Log ("ADDING ITEM TO GRAVE");

		AssetManager.m_assetManager.m_props [20].animation.Play ();

		GraveSlot gs = new GraveSlot ();
		gs.type = GraveSlot.ObjectType.Item;
		gs.item = item;
		int i = 0;

		if (item.HasKeyword (Item.Keyword.WhileInHand)) {
			StartCoroutine(item.Deactivate());
		}


		while (gs != null && i < m_grave.Count)
		{
			GraveSlot thisGS = m_grave[i];
			m_grave[i] = gs;
			gs = thisGS;
			i++;
		}

		if (gs != null)
		{
			if (gs.type == GraveSlot.ObjectType.Item)
			{
				Debug.Log("SENDING ITEM TO ABYSS");
				Destroy(gs.item.gameObject);
			} else if (gs.type == GraveSlot.ObjectType.Enemy)
			{
				Debug.Log("SENDING ENEMY TO ABYSS");
				Destroy(gs.enemy.gameObject);

				foreach (Enemy e in m_currentMap.m_enemies)
				{
					if (e.m_abilityType == Enemy.AbilityType.GraveDigger && e.graveDamageBonus > 0)
					{
						e.graveDamageBonus -= 1;
					}
				}
			}
			gs = null;
		}
	}

	public void SendToGrave (Enemy enemy)
	{

		Debug.Log ("ADDING ENEMY TO GRAVE");

		AssetManager.m_assetManager.m_props [20].animation.Play ();

		GraveSlot gs = new GraveSlot ();
		gs.type = GraveSlot.ObjectType.Enemy;
		gs.enemy = enemy;
		int i = 0;

		foreach (Enemy e in m_currentMap.m_enemies)
		{
			if (e.m_abilityType == Enemy.AbilityType.GraveDigger)
			{
				e.graveDamageBonus ++;
			}
		}

		while (gs != null && i < m_grave.Count)
		{
			GraveSlot thisGS = m_grave[i];
			m_grave[i] = gs;
			gs = thisGS;
			i++;
		}

		if (gs != null)
		{
			if (gs.type == GraveSlot.ObjectType.Item)
			{
				Debug.Log("SENDING ITEM TO ABYSS");
				Destroy(gs.item.gameObject);

			} else if (gs.type == GraveSlot.ObjectType.Enemy)
			{
				Debug.Log("SENDING ENEMY TO ABYSS");
				Destroy(gs.enemy.gameObject);

				foreach (Enemy e in m_currentMap.m_enemies)
				{
					if (e.m_abilityType == Enemy.AbilityType.GraveDigger && e.graveDamageBonus > 0)
					{
						e.graveDamageBonus -= 1;
					}
				}
			}
			gs = null;
		}
	}

	public IEnumerator UnlockGate ()
	{
		// find key and center it
		Item thisItem = null;
		List<Item> inventory = m_inventory;
		for (int i=0; i< inventory.Count; i++)
		{
			thisItem = (Item)inventory[i];
			if (thisItem.HasKeyword(Item.Keyword.Key))
			{
				string newString = "\\1" + m_currentFollower.m_nameText + "\\0 uses \\8" + thisItem.m_name + " \\0on \\9" + Player.m_player.currentCard.m_displayName.ToString();
				UIManager.m_uiManager.UpdateActions (newString);
				
				yield return StartCoroutine (thisItem.CenterCard ());
				yield return new WaitForSeconds(0.5f);	
				//yield return StartCoroutine (thisItem.SendToGrave ());
				i = 99;
			}
		}
		
		//turn gate into exit
		Player.m_player.numKeys --;
		Card c = Player.m_player.currentCard;
		StartCoroutine( c.ChangeCardType(AssetManager.m_assetManager.m_uniqueCards[0]));
		c.type = Card.CardType.Exit;
		c.SetColor(Color.yellow);
		c.m_highlightMesh.material.color = Color.yellow;

		string newString2 = "The Exit appears";
		UIManager.m_uiManager.UpdateActions (newString2);
		
		// remove key item and card
		
		for (int i=0; i < m_inventory.Count; i++)
		{
			Item inv = (Item)m_inventory[i];
			if (inv == thisItem)
			{
				m_inventory.RemoveAt(i);
				i=99;
			}
		}
		
		Destroy(thisItem);
		UIManager.m_uiManager.RefreshInventoryMenu();
		
		//update action button
		AssetManager.m_assetManager.m_typogenicText[15].Text = "DESCEND";
		StartCoroutine( UIManager.m_uiManager.DisplayTargetCard(Player.m_player.currentCard, UIManager.m_uiManager.m_followerCards[5]));

		yield return null;
	}

	private void LoadBadges ()
	{
		List<int> badgeStates = SettingsManager.m_settingsManager.badgeStates;
		List<string> badgeNames = new List<string> ();

		SettingsManager.m_settingsManager.startingDamage = SettingsManager.m_settingsManager.baseDamage;
		SettingsManager.m_settingsManager.startingHealth = SettingsManager.m_settingsManager.baseHealth;
		SettingsManager.m_settingsManager.startingEnergy = SettingsManager.m_settingsManager.baseEnergy;
		SettingsManager.m_settingsManager.startingArmor = SettingsManager.m_settingsManager.baseArmor;

		if (badgeStates[0] == 2)
		{
//			SettingsManager.m_settingsManager.startingDamage = 99;
//			SettingsManager.m_settingsManager.startingHealth = 99;
			m_doHeartBeat = true;
			badgeNames.Add("Dungeon Heart");
		} else {
			DHeart.m_dHeart.gameObject.SetActive(false);
		}

		if (badgeStates[1] == 2)
		{
			SettingsManager.m_settingsManager.startingDamage += 1;
			badgeNames.Add("Damage +1");
		}
		if (badgeStates[2] == 2)
		{
			SettingsManager.m_settingsManager.startingDamage += 1;
			badgeNames.Add("Damage +1");
		}
		if (badgeStates[3] == 2)
		{
			SettingsManager.m_settingsManager.startingDamage += 1;
			badgeNames.Add("Damage +1");
		}
		if (badgeStates[4] == 2)
		{
			SettingsManager.m_settingsManager.startingDamage += 2;
			badgeNames.Add("Damage +2");
		}

		if (badgeStates[5] == 2)
		{
			SettingsManager.m_settingsManager.startingHealth += 1;
			badgeNames.Add("Health +1");
		}
		if (badgeStates[6] == 2)
		{
			SettingsManager.m_settingsManager.startingHealth += 1;
			badgeNames.Add("Health +1");
		}
		if (badgeStates[7] == 2)
		{
			SettingsManager.m_settingsManager.startingHealth += 1;
			badgeNames.Add("Health +1");
		}
		if (badgeStates[8] == 2)
		{
			SettingsManager.m_settingsManager.startingHealth += 2;
			badgeNames.Add("Health +2");
		}

		if (badgeStates[9] == 2)
		{
			SettingsManager.m_settingsManager.startingEnergy += 2;
			badgeNames.Add("Energy +2");
		}
		if (badgeStates[10] == 2)
		{
			SettingsManager.m_settingsManager.startingEnergy += 2;
			badgeNames.Add("Energy +2");
		}
		if (badgeStates[11] == 2)
		{
			SettingsManager.m_settingsManager.startingEnergy += 3;
			badgeNames.Add("Energy +3");
		}
		if (badgeStates[12] == 2)
		{
			SettingsManager.m_settingsManager.startingEnergy += 3;
			badgeNames.Add("Energy +3");
		}

		if (badgeStates[13] == 2)
		{
			m_BPbonus += 1;
			badgeNames.Add("Hand Size +1");
		}
		if (badgeStates[14] == 2)
		{
			m_BPbonus += 1;
			badgeNames.Add("Hand Size +1");
		}

		if (badgeStates[15] == 2)
		{
			m_healthRecover += 1;
			badgeNames.Add("Health Recovery");
		}

		if (badgeStates[16] == 2)
		{
			m_energyRecover += 1;
			badgeNames.Add("Energy Recovery");
		}
		if (badgeStates[17] == 2)
		{
			m_energyRecover += 1;
			badgeNames.Add("Energy Recovery");
		}

		UIManager.m_uiManager.SpawnBadges (badgeNames);

	}

	public void TurnIndicator (bool turnOn)
	{
		if (GameManager.m_gameManager.followers.Count > 1) {
			foreach (UICard c in PartyCards.m_partyCards.m_party) {
				if (c.m_followerData == GameManager.m_gameManager.currentFollower) {
					c.m_miscOBJ [5].gameObject.SetActive (turnOn);
				} else {
					c.m_miscOBJ [5].gameObject.SetActive (false);
				}
		
			}
		}
	}
	
	public int GetNewID ()
	{
		m_currentID ++;
		return m_currentID;
	}
	
	
	public Card selectedCard
	{
		get
		{
			return m_selectedCard;	
		}
		set
		{
			m_selectedCard = value;	
		}
	}
	public Turn currentTurn
	{
		get
		{
			return m_currentTurn;	
		}
	}
//	public MapManager.Quest currentQuest
//	{
//		get
//		{
//			return m_currentQuest;	
//		}
//		set
//		{
//			m_currentQuest = value;
//		}
//	}
	public MapManager.Map currentMap
	{
		get
		{
			return m_currentMap;	
		}
		set
		{
			m_currentMap = value;
		}
	}
	public List<Enemy> enemies
	{
		get
		{
			if (m_currentMap == null)
			{
				return null;
			} else {
				return m_currentMap.m_enemies;
			}
		}
		set
		{
			m_currentMap.m_enemies = value;	
		}
	}
	public List<Follower> followers
	{
		get
		{
			return m_followers;	
		}
		set
		{
			m_followers = value;	
		}
	}
	public GameObject focusGO {get{return m_focusGO;} set {m_focusGO = value;}}
	public List<Item> inventory {get{return m_inventory;}set{m_inventory = value;}}
	public List<Item> storageItems {get{return m_storageItems;}set{m_storageItems = value;}}
	public List<Item> bonusItems {get{return m_bonusItems;}set{m_bonusItems = value;}}
	public List<Item> equippedItems {get{return m_equippedItems;}set{m_equippedItems = value;}}
	public List<Enemy> deadEnemies {get{return m_deadEnemies;}set{m_deadEnemies = value;}}
	public List<GameState.ProgressState> gameProgress {get{return m_gameProgress;}}
	public GameState gameState {get{return m_gameState;}}
	public Follower playerFollower {get{return m_playerFollower;}set{m_playerFollower = value;}}
	public int numStorageCards {get{return m_storageItems.Count;}}
	public bool selectMode {get{return m_selectMode;} set{m_selectMode = value;}}
	public bool acceptInput {get{return m_acceptInput;} 
		set{
			m_acceptInput = value; Debug.Log("INPUT: " + m_acceptInput);
			if (m_acceptInput == false) {TurnIndicator(false);} else {TurnIndicator(true);}}}
	public bool acceptInput_KeepIcon { get { return m_acceptInput; } set{m_acceptInput = value; Debug.Log("INPUT_KEEP ICON: " + m_acceptInput);}}
	public int currentChain {get {return m_currentChain;} set {m_currentChain = value;}}
	public int accruedXP {get {return m_accruedXP;} set {m_accruedXP = value; UIManager.m_uiManager.UpdateXP(m_accruedXP);}}
	public int currentTurnNum {get {return m_currentTurnNum;}}
	public int healthRecover {get {return m_healthRecover;}}
	public int energyRecover {get {return m_energyRecover;}}
	public bool doHeartBeat {get {return m_doHeartBeat;}}
	public bool doStorageChest {get {return m_doStorageChest;}}
	public bool doShop {get {return m_doShop;}}
	public bool doShopPortal {get {return m_doShopPortal;}}
	public bool doStatBar {get {return m_doStatBar;} set {m_doStatBar = value;}}
	public int itemBonus {get {return m_itemBonus;}}
	public int partyBonus {get {return m_partyBonus;}}
	public int BPbonus {get {return m_BPbonus;} set {m_BPbonus = value;}}
	public int maxBP {get{return 5+m_BPbonus;}}
	public int maxStorage {get {return 1+m_storageBonus;}}
	public Item[] limboCards {get {return m_limboCards;} set {m_limboCards = value;}}
	public List<Follower> lostSouls {get{return m_lostSouls;}set{m_lostSouls = value;}}
	public List<GraveSlot> grave {get {return m_grave;}}
	public Follower currentFollower {get{return m_currentFollower;}}
	public int drawCost {get{return m_adjustedDrawCost;}set{m_adjustedDrawCost = value; AssetManager.m_assetManager.m_typogenicText[1].Text = m_adjustedDrawCost.ToString(); }}
	public int numDiscardedThisTurn {get{ return m_numDiscardedThisTurn; }set {m_numDiscardedThisTurn = value; }}
	public int numTiles { get { return m_numTiles; } set { m_numTiles = value; } }
	public int numTilesFlipped { get { return m_numTilesFlipped; } set { m_numTilesFlipped = value; } }
	public int initiative {get{m_initiative ++; return m_initiative;} set{m_initiative = value;}}
	public bool showDetailedActions {get{return m_showDetailedActions;}}
	public int levelsCompletedBonus {get{return m_levelsCompletedBonus;}set{m_levelsCompletedBonus = value;}}
	public int numStunned {
		get
		{
			int num = 0;
			foreach (Follower thisFollower in m_followers)
			{
				if (thisFollower.followerState == Follower.FollowerState.Stunned)
				{
					num ++;	
				}
			}
			return num;
		}
	}
}
