using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public enum PlayerState
	{
		Idle,
		Moving,
	}
	
	public static Player
		m_player;
	
	public GameObject
		m_cameraTarget,
		m_playerMesh,
		m_activityIndicator;
	
	public ParticleSystem[]
		m_fx;
	
	private Card
		m_currentCard = null;

	private Follower
		m_playerFollower = null;
	
	private int
		m_currentEnergy = 0,
		m_maxEnergy = 20,
		m_currentHealth = 0,
		m_maxHealth = 10,
		m_wounds = 0,
		m_corruption = 0,
		m_damage = 10,
		m_currentActionPoints = 0,
		m_maxActionPoints = 1,
		m_flipRange = 1,
		m_currentArmor = 0,
		m_tempDamage = 0,
		m_tempArmor = 0,
		m_turnDamage = 0,
		m_turnArmor = 0,
		m_permDamage = 0,
		m_permArmor = 0,
		m_permHealth = 0,
		m_permEnergy = 0,
		m_numKeys = 0,
		m_effectDuration = 0,
		m_poisonDuration = 0,
		m_reflectDamage = 0,
		m_lifeTap = 0,
		m_soulTap = 0,
		m_counterBonus = 0,
		m_samuraiBonus = 0,
		m_turnRangedBonus = 0,
		m_permRangedDamageBonus = 0,
		m_permActionBonus = 0,
		m_stunDuration = 0;
	
	private PlayerState
		m_playerState = PlayerState.Idle;
	
	private float
		m_moveTime = 0,
		m_moveTimer = 0;
	
	private bool
		m_berserkerActive = false,
		m_mysticActive = false,
		m_cardsFlipping = false,
		m_doPoisonAttack = false,
		m_doCounterAttack = false,
		m_doPermCounterAttack = false,
		m_doPsychicAttack = false,
		m_dancerActive = false,
		m_doFencer = false,
		m_doSamurai = false,
		m_doArmorPierce = false,
		m_doPermArmorPierce = false,
		m_doSoulArmor = false,
		m_doSacrifice = false,
		m_canContinuousMove = true,
		m_doCover = false,
		m_doingCounter = false,
		m_doingSiteCard = false;
	
	private GameManager.StatusEffect
		m_currentEffect = GameManager.StatusEffect.None;
	
	private Vector3
		m_moveStart = Vector3.zero,
		m_moveEnd = Vector3.zero;
	
	private GameManager.Direction
		m_facing = GameManager.Direction.North;

	private UICard
		m_statBar = null;

	void Awake () {
		m_player = this;
		animation["PlayerJump01"].speed = 0;
	}	
	
	public void Initialize (Card startCard, Follower myClass)
	{
//		GUIFollow.m_guiFollow.SetTarget(this.gameObject);
		
//		m_damage = myClass.m_startingDamage;
//		m_currentEnergy = myClass.m_startingEnergy;
//		m_currentHealth = myClass.m_startingHealth;
		m_damage = SettingsManager.m_settingsManager.startingDamage;
		m_currentEnergy = SettingsManager.m_settingsManager.startingEnergy;
		m_currentHealth = SettingsManager.m_settingsManager.startingHealth;
		m_currentArmor = SettingsManager.m_settingsManager.startingArmor;
		m_currentCard = startCard;
		currentActionPoints = m_maxActionPoints;
		
		Follower fType = (Follower)transform.GetComponent("Follower");
		m_playerFollower = fType;
		fType.m_nameText = myClass.m_nameText;
		fType.m_abilityText = myClass.m_abilityText;
		fType.m_followerType = myClass.m_followerType;
		fType.m_portraitSpriteName = myClass.m_portraitSpriteName;
		fType.m_fullPortraitSpriteName = myClass.m_fullPortraitSpriteName;
//		fType.m_startingDamage = myClass.m_startingDamage;
//		fType.m_startingEnergy = myClass.m_startingEnergy;
//		fType.m_startingHealth = myClass.m_startingHealth;
		fType.m_startingDamage = m_damage;
		fType.m_startingEnergy = m_currentEnergy;
		fType.m_startingHealth = m_currentHealth;
		fType.m_startingArmor = m_currentArmor;
		fType.m_followerClass = myClass.m_followerClass;
		fType.m_levelTable = myClass.m_levelTable;
		fType.m_abilityCost = myClass.m_abilityCost;
		fType.m_abilityEffect = myClass.m_abilityEffect;
		fType.m_abilityRange = myClass.m_abilityRange;
		fType.baseAbilityCost = myClass.m_abilityCost;
		fType.baseAbilityEffect = myClass.m_abilityEffect;
		fType.baseAbilityRange = myClass.m_abilityRange;
		fType.m_heroBadgeTable = myClass.m_heroBadgeTable;
		fType.m_deck = myClass.m_deck;
		
		
		//set mesh
		GameObject fMesh = (GameObject)Instantiate(myClass.m_followerMesh, m_playerMesh.transform.position, m_playerMesh.transform.rotation);
		fMesh.transform.parent = m_playerMesh.transform;
		fType.m_followerMesh = fMesh;
		
		//set progress values
		foreach (GameState.ProgressState thisState in GameManager.m_gameManager.gameProgress)
		{
			if (thisState.m_followerType == fType.m_followerType)
			{
				fType.isLocked = thisState.m_isLocked;
				fType.currentLevel = thisState.m_level;
				fType.currentXP = thisState.m_XP;
				fType.XPBonus = thisState.m_XPBonus;

				int[] heroBadgeStates = new int[5];
				heroBadgeStates[0] = thisState.m_badgeLevel1;
				heroBadgeStates[1] = thisState.m_badgeLevel2;
				heroBadgeStates[2] = thisState.m_badgeLevel3;
				heroBadgeStates[3] = thisState.m_badgeLevel4;
				heroBadgeStates[4] = thisState.m_badgeLevel5;
				//heroBadgeStates[5] = charProgress.m_badgeLevel6;
				fType.heroBadgeStates = heroBadgeStates;

				fType.SetLevel();
				break;
			}
		}
		
		//Debug.Log(myClass.m_followerType.ToString());
//		UIManager.m_uiManager.UpdateName(fType.m_followerType.ToString());
//		UIManager.m_uiManager.UpdateDamage(m_damage);
//		UIManager.m_uiManager.UpdateEnergy(m_currentEnergy);
//		UIManager.m_uiManager.UpdateHealth(m_currentHealth);
//		UIManager.m_uiManager.UpdateActionPoints(m_currentActionPoints);
		
		//play intro animation
		//animation.Play("PlayerDrop01");
		animation["PlayerIdle01"].speed = Random.Range(0.4f, 0.75f);
		animation["PlayerIdle01"].time = Random.Range (0, 1.0f);

		//create new stat bar
//		GameObject go = (GameObject)(Instantiate(AssetManager.m_assetManager.m_UIelements[1], Vector3.zero, AssetManager.m_assetManager.m_UIelements[0].transform.rotation ));
//		go.transform.parent = UIManager.m_uiManager.m_HUD.transform;
//		go.transform.localScale = Vector3.one;
//		GUIFollow f = (GUIFollow)go.GetComponent("GUIFollow");
//		Vector3 offset = new Vector3 (30,40,0);
//		f.SetTarget(this.gameObject, offset);
//		m_statBar = (UICard)go.GetComponent("UICard");
		m_activityIndicator.SetActive (false);
	}
	
	public IEnumerator StartTurn()
	{
		Debug.Log ("PLAYER START TURN PHASE");
		m_turnDamage = 0;
		m_turnArmor = 0;
		m_reflectDamage = 0;
		m_lifeTap = 0;
		m_soulTap = 0;
		m_counterBonus = 0;
		m_samuraiBonus = 0;
		m_berserkerActive = false;
		m_mysticActive = false;
		m_doPoisonAttack = false;
		m_doCounterAttack = false;
		m_doArmorPierce = false;
		m_doPsychicAttack = false;
		m_doSacrifice = false;
		m_doFencer = false;
		m_doSamurai = false;
		m_dancerActive = false;
		m_doSoulArmor = false;
		m_doCover = false;
		GameManager.m_gameManager.numDiscardedThisTurn = 0;
		m_turnRangedBonus = 0;

		if (m_stunDuration > 0) {
			m_stunDuration --;

			EffectsPanel.Effect thisStun = null;
			foreach (EffectsPanel.Effect e in EffectsPanel.m_effectsPanel.effectStack) {
				if (e.m_effectType == EffectsPanel.Effect.EffectType.PlayerStun)
				{
					if (m_stunDuration == 0)
					{
						thisStun = e;
					} else {
						string desc = "Stunned: Swapping Leaders drains 2# - " + Player.m_player.stunDuration.ToString() + " Turns remaining";
						e.m_description = desc;
					}
					break;
				}
			}

			if (m_stunDuration == 0 && thisStun != null)
			{
				EffectsPanel.m_effectsPanel.RemoveEffect(thisStun);
			}
		}


		StartCoroutine(UIManager.m_uiManager.UpdateDamage(m_damage + m_tempDamage + m_turnDamage + m_permDamage + m_currentCard.siteDamageBonus));
		StartCoroutine(UIManager.m_uiManager.UpdateArmor(m_currentArmor + m_tempArmor + m_turnArmor + m_permArmor + m_currentCard.siteArmorBonus));
//		if (m_currentActionPoints == 0) {
			currentActionPoints += 1 + m_permActionBonus;
//		}
		//resolve status effects
		if (m_poisonDuration > 0)
		{
			yield return new WaitForSeconds(0.2f);
			UIManager.m_uiManager.SpawnAbilityName((GameManager.StatusEffect.Poison).ToString(), this.transform);
			yield return new WaitForSeconds(0.5f);
			yield return StartCoroutine(TakeDamage(1));
			yield return new WaitForSeconds(1);
			m_poisonDuration -= 1;
//			m_effectDuration -= 1;
			if (m_poisonDuration <=0)
			{
//				ChangeEffectState(GameManager.StatusEffect.None, 0);	
			}

			foreach (EffectsPanel.Effect e in EffectsPanel.m_effectsPanel.effectStack)
			{
				if (e.m_effectType == EffectsPanel.Effect.EffectType.PlayerPoisoned)
				{
					EffectsPanel.m_effectsPanel.UpdateEffect(e);
					break;
				}
			}
		}

		//check current card to see if effects need to be applied
		if (Player.m_player.currentCard.doUpkeep) {
			Debug.Log("DOING UPKEEP ON PLAYER CARD");
			yield return StartCoroutine(Player.m_player.currentCard.DoTurn());	
		}
		
		yield return null;
	}
	
	public IEnumerator DoUpdate ()
	{
		if (m_playerState == PlayerState.Moving)
		{
			
//			m_moveTimer = Mathf.Clamp(m_moveTimer + Time.deltaTime, 0, m_moveTime);
//			float t = Mathf.Clamp(m_moveTimer / m_moveTime, 0, 1);
//			animation["PlayerJump01"].time = m_moveTime * t;
//			Vector3 newPos = Vector3.Lerp(m_moveStart, m_moveEnd, t);
//			
//			if (m_moveTimer == m_moveTime)
//			{
//				newPos = m_moveEnd;
//				m_playerState = PlayerState.Idle;
//				animation.Stop();
//				if (m_currentCard.type == Card.CardType.Exit)
//				{
//					
//					yield return StartCoroutine(UIManager.m_uiManager.ChangeMenuMode(UIManager.MenuMode.EndLevel));
//					//MapManager.m_mapManager.SetMapActive(m_currentCard.mapID);
//					yield break;
//				}
//				else if (m_currentCard.type == Card.CardType.Entrance || m_currentCard.type == Card.CardType.DungeonEntrance)
//				{
//					MapManager.m_mapManager.SetMapActive(m_currentCard.mapID);
//					yield break;
//				} else {
//					UseActionPoint();
//				}
//				
//				//update HUD with any stat bonuses from site
////				UIManager.m_uiManager.UpdateHealth(m_currentHealth);
//				UIManager.m_uiManager.UpdateDamage(m_damage + m_tempDamage + m_turnDamage + m_currentCard.siteDamageBonus);
////				UIManager.m_uiManager.UpdateEnergy(m_currentEnergy);
//				UIManager.m_uiManager.UpdateArmor(m_currentArmor + m_tempArmor + m_turnArmor + m_currentCard.siteArmorBonus);
//				
//				MapManager.m_mapManager.SetDistanceToPlayer();
//			}
//			transform.position = newPos;
		}
		
		yield return null;
	}
	
	public IEnumerator MovePlayer (GameManager.Direction dir)
	{

		Debug.Log ("ATTEMPTING TO MOVE");
		bool canMove = false;
		Card currentCard = m_currentCard;
		Card nextCard = null;

		//focus camera if needed
		if (GameManager.m_gameManager.focusGO != Player.m_player.gameObject)
		{
			Debug.Log("GRABBING FOCUS");
			GameManager.m_gameManager.focusGO = Player.m_player.gameObject;
			FollowCamera.m_followCamera.SetTarget(GameManager.m_gameManager.focusGO);
			//yield return new WaitForSeconds(0.5f);
			//yield break;

		}
		
		//determine if valid location exists
		switch (dir)
		{
		case GameManager.Direction.North:
			if (currentCard.northCard != null)
			{
				canMove = true;	
				nextCard = currentCard.northCard;
			}
			break;
		case GameManager.Direction.South:
			if (currentCard.southCard != null)
			{
				canMove = true;	
				nextCard = currentCard.southCard;
			}
			break;
		case GameManager.Direction.East:
			if (currentCard.eastCard != null)
			{
				canMove = true;	
				nextCard = currentCard.eastCard;
			}
			break;
		case GameManager.Direction.West:
			if (currentCard.westCard != null)
			{
				canMove = true;	
				nextCard = currentCard.westCard;
			}
			break;
		}
		
		if (canMove && nextCard != null)
		{
			ChangeFacing(dir);
			if (nextCard.cardState == Card.CardState.Hidden)
			{

				string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 flips a Tile";
				UIManager.m_uiManager.UpdateActions (newString);

				m_canContinuousMove = false;
				List<GameManager.Direction> flipDir = new List<GameManager.Direction>();
				flipDir.Add(dir);
				List<Card> validCards = MapManager.m_mapManager.GetCardsInRange(m_currentCard,m_flipRange, flipDir, false, false, true);
				
				m_cardsFlipping = true;
				foreach (Card thisCard in validCards)
				{
					if (thisCard.cardState == Card.CardState.Hidden)
					{
						if (m_currentEnergy < m_maxEnergy)
						{
							GainEnergy(thisCard.GetEnergyValue());
						}
//						if (!SettingsManager.m_settingsManager.trial)
//						{
//							GameManager.m_gameManager.accruedXP += 1;
//						}

						if (Player.m_player.currentEnergy < Player.m_player.maxEnergy)
						{
							UIManager.m_uiManager.SpawnFloatingText("+1",UIManager.Icon.Energy, thisCard.transform);

							if (!thisCard.isOccupied)
							{
								GameObject pFX = AssetManager.m_assetManager.m_particleFX[1];
								Instantiate(pFX, thisCard.transform.position, pFX.transform.rotation);
							}
						}
						//GameManager.m_gameManager.acceptInput = false;
						yield return StartCoroutine(thisCard.ChangeCardState(Card.CardState.Normal));
						//GameManager.m_gameManager.acceptInput = true;
					}
				}
				m_cardsFlipping = false;
				UIManager.m_uiManager.UpdateEnergy(m_currentEnergy);
				UseActionPoint();
				
			}
			else if (nextCard.type == Card.CardType.Gate && m_numKeys > 0)
			{
				m_canContinuousMove = false;
				m_numKeys --;
				//UIManager.m_uiManager.UpdateKey(false);
				
				//remove key from itemlist
				List<Item> inventory = GameManager.m_gameManager.inventory;
				for (int i=0; i< inventory.Count; i++)
				{
					Item thisItem = (Item)inventory[i];
					if (thisItem.HasKeyword(Item.Keyword.Key))
					{
						yield return StartCoroutine (thisItem.CenterCard ());
						yield return new WaitForSeconds(0.5f);	
						yield return StartCoroutine (thisItem.SendToGrave ());
//						inventory.RemoveAt(i);
//						GameManager.m_gameManager.inventory = inventory;
//						Destroy(thisItem.gameObject);
						i = 99;
					}
				}
				
				nextCard.type = Card.CardType.Exit;
				nextCard.m_cardMesh.material = ((Card)MapManager.m_mapManager.m_cardTypes[2].transform.GetComponent("Card")).cardMesh.sharedMaterial;
				nextCard.SetColor(Color.yellow);
				nextCard.m_highlightMesh.material.color = Color.yellow;

//				if (GameManager.m_gameManager.currentMap.m_mapType == MapManager.Map.MapType.Plains)
//				{
//					nextCard.m_cardMesh.material = ((Card)MapManager.m_mapManager.m_cardTypes[0].transform.GetComponent("Card")).cardMesh.sharedMaterial;
//				} else if (GameManager.m_gameManager.currentMap.m_mapType == MapManager.Map.MapType.Dungeon)
//				{
//					nextCard.m_cardMesh.material = ((Card)MapManager.m_mapManager.m_cardTypes[4].transform.GetComponent("Card")).cardMesh.sharedMaterial;
//				}
				UseActionPoint();
			}
			else if (nextCard.enemy != null)
			{
				m_canContinuousMove = false;
				if (nextCard.enemy.m_enemyType != Enemy.EnemyType.Pet)
				{
					yield return StartCoroutine(Attack(nextCard.enemy));
				} else {
					//move and send pet to player's previous spot
					Pet thisPet = (Pet)nextCard.enemy;
					nextCard.enemy = null;
					Card newPetSpot = m_currentCard;
					yield return StartCoroutine(DoMove(nextCard));
					yield return StartCoroutine (FinishMove ());
					yield return StartCoroutine(thisPet.MovePet(newPetSpot));
					//UseActionPoint();
					GameManager.m_gameManager.acceptInput = true;

				}
				UseActionPoint();
			}	
			else if (nextCard.shop != null)
			{
				m_canContinuousMove = false;
				yield return StartCoroutine(UIManager.m_uiManager.ChangeMenuMode(UIManager.MenuMode.Shop));	

				yield return new WaitForSeconds(0.25f);

				// destroy shop
				if (nextCard.shop != null) // if player used portal, shop is already destroyed
				{
					Shop s = nextCard.shop;
					Instantiate(AssetManager.m_assetManager.m_particleFX[0], s.transform.position, Quaternion.identity);
					Destroy(s.gameObject);
					nextCard.shop = null;
				}
			}
			else if (nextCard.chest != null)
			{
				m_canContinuousMove = false;
//				if  (GameManager.m_gameManager.inventory.Count < GameManager.m_gameManager.maxBP || nextCard.chest.m_doStorage)
//				{
					yield return StartCoroutine(nextCard.chest.ActivateChest());	
					UseActionPoint();
//				} else 
//				{
//					GameManager.m_gameManager.acceptInput = false;
//					UIManager.m_uiManager.SpawnAbilityName("FULL", nextCard.chest.transform);
//					yield return new WaitForSeconds(0.7f);
//					GameManager.m_gameManager.acceptInput = true;
//				}
					
			}else if (nextCard.follower != null)
			{
				m_canContinuousMove = false;
				GameManager.m_gameManager.acceptInput = false;

				// create new Lost Soul item
				Item iLostSoul = (Item) ((GameObject)Instantiate(GameManager.m_gameManager.m_itemBank[5], Vector3.one * 1000, Quaternion.identity)).GetComponent("Item");
				Follower fData = (Follower) nextCard.follower.GetComponent("Follower");
				GameManager.m_gameManager.lostSouls.Add(fData);

				GameObject go = nextCard.follower;
				go.transform.parent = null;
				go.transform.position = Vector3.one * 100000;
				go.transform.localScale = Vector3.zero;

				nextCard.follower = null;

				// update Lost Soul portrait with appropriate hero
				iLostSoul.m_portraitSpriteName = "Card_Portrait_Item02";
//				iLostSoul.m_fullPortraitSpriteName = fData.m_fullPortraitSpriteName;

				// create card, play anim and add to inventory
				GameObject fCard = (GameObject)Instantiate(UIManager.m_uiManager.m_itemCard, UIManager.m_uiManager.m_itemCard.transform.position, UIManager.m_uiManager.m_itemCard.transform.rotation);	
				UICard cardUI = (UICard)fCard.GetComponent("UICard");
				cardUI.SetCard(iLostSoul, false);
				Transform cardParent = UIManager.m_uiManager.m_chestUI.transform.Find("InventoryPanel");
				cardUI.transform.parent = cardParent;
				cardUI.transform.localPosition = Vector3.zero;
//				yield return StartCoroutine(UIManager.m_uiManager.TurnOffTargetCard());
				UIManager.m_uiManager.m_chestUI.SetActive(true);	
				
				yield return new WaitForSeconds(0.75f);
				
				GameManager.m_gameManager.inventory.Add(iLostSoul);
				
				//animated card moving
				float t = 0;
				float time = 0.3f;
				Vector3 startPos = cardUI.transform.position;
				Vector3 startScale = cardUI.transform.localScale;
				
				while (t < time)
				{
					t += Time.deltaTime;
					//Vector3 newPos = Vector3.Lerp(startPos, UIManager.m_uiManager.m_backpackButton.transform.position, t / time);
					Vector3 newPos = Vector3.Lerp(startPos, AssetManager.m_assetManager.m_props[7].transform.position , t / time);
					Vector3 newScale = Vector3.Lerp(startScale, Vector3.one * 0.25f, t / time);
					cardUI.transform.position = newPos;
					cardUI.transform.localScale = newScale;
					yield return null;
				}
				Destroy(fCard.gameObject);
				UIManager.m_uiManager.RefreshInventoryMenu();

//				if (GameManager.m_gameManager.followers.Count < 3 + GameManager.m_gameManager.partyBonus)
//				{
//					GameObject go = nextCard.follower;
//					Follower thisF = (Follower) go.GetComponent("Follower");
//					AddNewFollower(thisF);
//					
//					GameObject thisFollower = nextCard.follower;
//					nextCard.follower = null;
//					thisFollower.transform.parent = null;
//					thisFollower.transform.position = Vector3.one * 100000;
//					thisFollower.transform.localScale = Vector3.zero;
//				} else {
//
//					GameObject go = nextCard.follower;
//					go.transform.parent = null;
//					go.transform.position = Vector3.one * 100000;
//					go.transform.localScale = Vector3.zero;
//
//					nextCard.follower = null;
//					InputManager.m_inputManager.selectedObject = go;
//					yield return StartCoroutine(UIManager.m_uiManager.ChangeMenuMode(UIManager.MenuMode.FollowerSwap));	
//				}
				
				if (DHeart.m_dHeart.isBeating)
				{
					DHeart.m_dHeart.StopHeartBeat();	
				}
				GameManager.m_gameManager.acceptInput = true;

				UseActionPoint();	
			}
			else if (nextCard.goal != null)
			{
				GameObject thisGoal = nextCard.goal;
				UIManager.m_uiManager.SetGoalActive();
				nextCard.goal = null;
				Destroy(thisGoal);
				
				UseActionPoint();	
			}
//			else if (!nextCard.isOccupied && nextCard.type != Card.CardType.Gate) {
//				animation.Play("PlayerJump01");
//				m_moveTimer = 0;
//				m_moveTime = animation["PlayerJump01"].length;
//				m_playerState = PlayerState.Moving;
//				m_moveStart = m_currentCard.transform.position;
//				m_moveEnd = nextCard.transform.position;
//				m_currentCard.player = null;
//				m_currentCard = nextCard;
//				m_currentCard.player = this;
			else if (!nextCard.isOccupied) {
				
				yield return StartCoroutine(DoMove(nextCard));
				yield return StartCoroutine (FinishMove ());
				UseActionPoint();
				if (Player.m_player.m_currentActionPoints > 0)
				{
					GameManager.m_gameManager.acceptInput_KeepIcon = true;
				}

			}
		}


		yield return null;
	}

	public IEnumerator DoMove (Card nextCard)
	{
		string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 moves";
		UIManager.m_uiManager.UpdateActions (newString);

		if (m_doingSiteCard) {
			m_doingSiteCard = false;
			StartCoroutine( UIManager.m_uiManager.TurnOffSiteCard());
		}

		if (m_currentCard.type == Card.CardType.Exit)
		{
			UIManager.m_uiManager.m_exitButton.SetActive(false);
		}
		
		AssetManager.m_assetManager.PlaySFX(AssetManager.SFXType.PlayerMove);
		animation["PlayerJump01"].speed = 0;
		animation ["PlayerJump01"].time = 0;
		m_moveTimer = 0;
		m_moveTime = animation["PlayerJump01"].length;
		m_playerState = PlayerState.Moving;
		m_moveStart = m_currentCard.transform.position;
		m_moveEnd = nextCard.m_actorBase.position;
		animation.Play("PlayerJump01");
		GameManager.m_gameManager.acceptInput_KeepIcon = false;
		
		bool fxPlayed = false;
		while (animation.IsPlaying("PlayerJump01"))
		{
			//					Debug.Log(m_moveTimer);
			m_moveTimer = Mathf.Clamp(m_moveTimer + Time.deltaTime, 0, m_moveTime);
			float t = Mathf.Clamp(m_moveTimer / m_moveTime, 0, 1);
			animation["PlayerJump01"].time = m_moveTime * t;
			t = Mathf.Clamp(t * 2.0f, 0.0f, 1.0f);
			if (t == 1.0f && !fxPlayed)
			{
				fxPlayed = true;	
				m_fx[0].Play();
			}
			Vector3 newPos = Vector3.Lerp(m_moveStart, m_moveEnd, t);
			
			if (m_moveTimer == m_moveTime)
			{
				animation["PlayerJump01"].time = animation["PlayerJump01"].length;
				newPos = m_moveEnd;
				m_playerState = PlayerState.Idle;
				animation.Stop();
				m_currentCard.player = null;
				m_currentCard = nextCard;
				m_currentCard.player = this;
				//StartCoroutine(nextCard.FlashFade());
				MapManager.m_mapManager.SetDistanceToPlayer();
				
				//update effects stack
				
				EffectsPanel.m_effectsPanel.UpdateEffects(EffectsPanel.Effect.Duration.WhilePresent);
				
				if (m_currentCard.type == Card.CardType.Darkness)
				{
					//Update Effect Stack
					EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
					newEffect.m_effectType = EffectsPanel.Effect.EffectType.Dark;
					newEffect.m_effectDuration = EffectsPanel.Effect.Duration.WhilePresent;
					string desc = "Darkness: No Cards can be played.";
					newEffect.m_description = desc;
					newEffect.m_stackable = false;
					newEffect.m_spriteName = "Effect_Dark";
					EffectsPanel.m_effectsPanel.AddEffect(newEffect);
				} else if (m_currentCard.type == Card.CardType.HighGround)
				{
					//Update Effect Stack
					EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
					newEffect.m_effectType = EffectsPanel.Effect.EffectType.HighGround;
					newEffect.m_effectDuration = EffectsPanel.Effect.Duration.WhilePresent;
					string desc = "High Ground: +" + m_currentCard.siteDamageBonus + "$ while present";
					newEffect.m_description = desc;
					newEffect.m_stackable = false;
					newEffect.m_spriteName = "Effect_HighGround";
					EffectsPanel.m_effectsPanel.AddEffect(newEffect);
				} else if (m_currentCard.type == Card.CardType.Fort)
				{
					//Update Effect Stack
					EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
					newEffect.m_effectType = EffectsPanel.Effect.EffectType.Fort;
					newEffect.m_effectDuration = EffectsPanel.Effect.Duration.WhilePresent;
					string desc = "Fort: +" + m_currentCard.siteArmorBonus + "% while present";
					newEffect.m_description = desc;
					newEffect.m_stackable = false;
					newEffect.m_spriteName = "Effect_Fort";
					EffectsPanel.m_effectsPanel.AddEffect(newEffect);
				} else if (m_currentCard.type == Card.CardType.Trap_Razorvine)
				{
					//Update Effect Stack
					EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
					newEffect.m_effectType = EffectsPanel.Effect.EffectType.RazorVine;
					newEffect.m_effectDuration = EffectsPanel.Effect.Duration.WhilePresent;
					string desc = "Razorvine: Lose 2& every turn while present";
					newEffect.m_description = desc;
					newEffect.m_stackable = false;
					newEffect.m_spriteName = "Effect_RazorVine";
					EffectsPanel.m_effectsPanel.AddEffect(newEffect);
				} else if (m_currentCard.type == Card.CardType.Stalactite)
				{
					//Update Effect Stack
					EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
					newEffect.m_effectType = EffectsPanel.Effect.EffectType.Stalactites;
					newEffect.m_effectDuration = EffectsPanel.Effect.Duration.WhilePresent;
					string desc = "Stalactites: Lose 1& each time a Skill is used";
					newEffect.m_description = desc;
					newEffect.m_stackable = false;
					newEffect.m_spriteName = "Effect_Stalactites";
					EffectsPanel.m_effectsPanel.AddEffect(newEffect);
				} else if (m_currentCard.type == Card.CardType.Quicksand)
				{
					//Update Effect Stack
					EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
					newEffect.m_effectType = EffectsPanel.Effect.EffectType.Quicksand;
					newEffect.m_effectDuration = EffectsPanel.Effect.Duration.WhilePresent;
					string desc = "Quicksand: Lose 2# each turn while present";
					newEffect.m_description = desc;
					newEffect.m_stackable = false;
					newEffect.m_spriteName = "Effect_Quicksand";
					EffectsPanel.m_effectsPanel.AddEffect(newEffect);
				} else if (m_currentCard.type == Card.CardType.Tower)
				{
					//Update Effect Stack
					EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
					newEffect.m_effectType = EffectsPanel.Effect.EffectType.Tower;
					newEffect.m_effectDuration = EffectsPanel.Effect.Duration.WhilePresent;
					string desc = "Tower: Ranged attacks get +" + m_currentCard.siteRangeBonus.ToString() + " Range";
					newEffect.m_description = desc;
					newEffect.m_stackable = false;
					newEffect.m_spriteName = "Effect_Tower";
					EffectsPanel.m_effectsPanel.AddEffect(newEffect);
				}
				
				
				// Check for adjacent enemies with the Ambush ability
				Card[] lc = m_currentCard.linkedCards;
				List<Card> ambushCards = new List<Card>();
				foreach (Card c in lc)
				{
					if (c != null)
					{
						if (c.doAmbush && c.cardState == Card.CardState.Hidden)
						{
							ambushCards.Add(c);
							//yield return StartCoroutine(c.DoAmbush());
						}
					}
				}

				if (ambushCards.Count > 0)
				{
					GameManager.m_gameManager.acceptInput = false;
					foreach (Card c in ambushCards)
					{
						yield return StartCoroutine(c.DoAmbush());
					}

					yield return new WaitForSeconds(0.25f);
					GameManager.m_gameManager.acceptInput = true;
				}
				
				if (m_currentCard.type == Card.CardType.Exit && !UIManager.m_uiManager.m_exitButton.activeSelf)
				{
					UIManager.m_uiManager.m_exitButton.SetActive(true);
					
					//							foreach (Follower thisFollower in GameManager.m_gameManager.followers)
					//							{
					//								if (thisFollower.followerState == Follower.FollowerState.Spent)
					//								{
					//									yield return StartCoroutine(thisFollower.ChangeState(Follower.FollowerState.Normal));	
					//								}
					//							}
					//							GameManager.m_gameManager.acceptInput = true;
					//							yield return StartCoroutine(UIManager.m_uiManager.ChangeMenuMode(UIManager.MenuMode.EndLevel));
					//							yield break;
				}
				//						else {
				//						}
				
				//update HUD with any stat bonuses from site
				//				UIManager.m_uiManager.UpdateHealth(m_currentHealth);
				StartCoroutine(UIManager.m_uiManager.UpdateDamage(m_damage + m_tempDamage + m_turnDamage + m_permDamage + m_currentCard.siteDamageBonus));
				//				UIManager.m_uiManager.UpdateEnergy(m_currentEnergy);
				StartCoroutine(UIManager.m_uiManager.UpdateArmor(m_currentArmor + m_tempArmor + m_turnArmor + m_permArmor + m_currentCard.siteArmorBonus));
				
				
				//GameManager.m_gameManager.acceptInput = true;
				transform.position = m_moveEnd;
				animation.Play("PlayerIdle01");

				yield break;
				
			}
			transform.position = newPos;
			yield return null;
		}
		//GameManager.m_gameManager.acceptInput = true;

		yield return null;
	}

	public IEnumerator FinishMove ()
	{
		//check for affects triggered by change in position
//		foreach (Enemy e in GameManager.m_gameManager.enemies) {
//			if (e.m_abilityType == Enemy.AbilityType.DrawCostMod && (e.enemyState == Enemy.EnemyState.Idle || e.enemyState == Enemy.EnemyState.Active)
//			    && (e.currentCard.column == currentCard.column || e.currentCard.row == currentCard.row) && e.currentCard.distanceToPlayer <= e.m_abilityRange)
//			{
//				//player is in range
//				GameManager.m_gameManager.drawCost = 99;
//			}
//		}

		// display target card if current card is special
		if (currentCard.type != Card.CardType.Normal && currentCard.type != Card.CardType.Entrance && !m_doingSiteCard) {
			m_doingSiteCard = true;
			StartCoroutine( UIManager.m_uiManager.DisplayTargetCard(currentCard, UIManager.m_uiManager.m_followerCards[5]));
		}

		yield return null;
	}
	
	public void UseActionPoint ()
	{
		currentActionPoints --;
		UIManager.m_uiManager.UpdateActionPoints(m_currentActionPoints);
		if (m_currentActionPoints <= 0)
		{
			//UIManager.m_uiManager.UpdateActionPoints(m_currentActionPoints);
//			currentActionPoints = m_maxActionPoints;	
			StartCoroutine(GameManager.m_gameManager.Changeturn(GameManager.Turn.Enemy));
		}
//		UIManager.m_uiManager.UpdateActionPoints(m_currentActionPoints);
	}
	
	public void GainActionPoints (int amt)
	{
		Debug.Log("GAINING ACTION POINTS");
		currentActionPoints = Mathf.Clamp(m_currentActionPoints + amt, 0, 20);	
		//UIManager.m_uiManager.UpdateActionPoints(m_currentActionPoints);
		
		
	}
	
	private IEnumerator Attack (Enemy thisEnemy)
	{
		Debug.Log("Player Attacking");

		string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 attacks \\4" + thisEnemy.m_displayName;
		UIManager.m_uiManager.UpdateActions (newString);

//		if (Random.Range(0.0f, 1.0f) >= 0.9f)
//		{
//			CineCam.m_cineCam.ActivateCineCam (Player.m_player.transform);
//		}

		GameManager.m_gameManager.acceptInput_KeepIcon = false;
		int psychicDamage = 0;
		if (m_doPsychicAttack)
		{
			psychicDamage += thisEnemy.energy;	
		}

		int counterDamage = 0;
		if (m_doingCounter && m_counterBonus > 0)
		{
			counterDamage += m_counterBonus;
		}
		int damage = GetDamage (true, true) + psychicDamage + m_counterBonus;
//		if (damage > thisEnemy.currentHealth && thisEnemy.m_maxHealth == thisEnemy.currentHealth)
//		{
//			CineCam.m_cineCam.ActivateCineCam (Player.m_player.transform);
//		}

		animation["PlayerJump01"].speed = 1.5f;
		animation.Play("PlayerJump01");
		yield return new WaitForSeconds (0.1f);
//		while (animation.IsPlaying("PlayerJump01"))
//		{
//			yield return null;
//		}
		StartCoroutine (ShakeCamera (0));

		yield return StartCoroutine(thisEnemy.TakeDamage(damage));
		//if (thisEnemy == null && m_doFencer)
		if (thisEnemy.enemyState == Enemy.EnemyState.Dead)
		{
			// handle chaining
//			GameManager.m_gameManager.currentChain ++;
//			UIManager.m_uiManager.UpdateChainGUI(GameManager.m_gameManager.currentChain);
//			//yield return new WaitForSeconds(0.5f);
//			List<UICard> skillCards = PartyCards.m_partyCards.GetAllSkills (false);
//			foreach (UICard c in skillCards)
//			{
//				if (c.itemData.itemState == Item.ItemState.Normal && c.itemData.HasKeyword(Item.Keyword.Chain) && GameManager.m_gameManager.currentChain == c.itemData.m_chainLevel)
//				{
//					StartCoroutine(c.itemData.ActivateItem());
//				}
//			}


			if ((damage > thisEnemy.currentHealth && thisEnemy.m_maxHealth == thisEnemy.currentHealth && thisEnemy.m_maxHealth > 9) || thisEnemy.m_maxHealth >= 20)
			{
				CineCam.m_cineCam.ActivateCineCam (Player.m_player.transform);
			}

			if (m_doSamurai)
			{
				UIManager.m_uiManager.SpawnFloatingText("+" + m_samuraiBonus.ToString() + "A", UIManager.Icon.None, Player.m_player.m_playerMesh.transform);
				GainActionPoints(m_samuraiBonus);
			}

			if ( m_doPoisonAttack)
			{
				thisEnemy.ChangeEffectState(GameManager.StatusEffect.Poison, 3);
			}

			// Gain XP
			int xp = thisEnemy.m_level;
			GameManager.m_gameManager.accruedXP += xp;
			UIManager.m_uiManager.SpawnFloatingText("+" + xp.ToString(), UIManager.Icon.Gold,m_currentCard.transform);

			// Gain GP
//			if (thisEnemy.m_level > 0 && !SettingsManager.m_settingsManager.trial)
//			{
//				int gp = thisEnemy.m_level;
//				SettingsManager.m_settingsManager.gold += gp;
//				UIManager.m_uiManager.SpawnFloatingText("+" + gp.ToString(), UIManager.Icon.Gold,thisEnemy.currentCard.transform);
//			}
		}


//		if (m_doArmorPierce)
//		{
//			m_doArmorPierce = false;	
//		}
		
		//refresh any used weapons
//		foreach (UICard thisWeapon in UIManager.m_uiManager.m_equipSlots)
//		{
//			if (thisWeapon.m_itemData != null)
//			{
//				
//				if (thisWeapon.m_itemData.itemState == Item.ItemState.Spent && thisWeapon.m_itemData.m_attackBonusDice.Length > 0)
//				{
//					thisWeapon.m_itemData.ChangeState(Item.ItemState.Normal);	
//				}
//			}
//		}
		
		if (m_currentCard.type == Card.CardType.RazorGlade)
		{
			yield return StartCoroutine(m_currentCard.ActivateCard());
		}
		

		
		EffectsPanel.m_effectsPanel.UpdateEffects(EffectsPanel.Effect.Duration.NextPlayerAttack);
		
		GameManager.m_gameManager.acceptInput = true;
		animation.Play ("PlayerIdle01");
	}

	
	public int GetDamage (bool attackDamage, bool tempDamage)
	{
		int damage = 0 + m_currentCard.siteDamageBonus;
		
		if (attackDamage)
		{
			damage += (m_damage + m_turnDamage + m_permDamage);
		}
		if (tempDamage)
		{
			damage += m_tempDamage;
			m_tempDamage = 0;
		}
		StartCoroutine(UIManager.m_uiManager.UpdateDamage(m_damage + m_tempDamage + m_turnDamage + m_permDamage + m_currentCard.siteDamageBonus));
		return damage;	
	}

	private IEnumerator HitFlash (float flashTime)
	{
		AssetManager.m_assetManager.m_uiSprites [0].color = Color.white;
		AssetManager.m_assetManager.m_uiSprites [0].spriteName = "Icon_Health";
		AssetManager.m_assetManager.m_uiSprites [0].transform.localPosition = Vector3.one;

		float t2 = 0.0f;
		while (t2 < flashTime)
		{
			t2 += Time.deltaTime;
			float a = Mathf.Lerp(1.0f, 0.0f, t2 / flashTime);
			Color c = AssetManager.m_assetManager.m_uiSprites[0].color;
			c.a = a;
			AssetManager.m_assetManager.m_uiSprites[0].color = c;
			yield return null;
		}

		AssetManager.m_assetManager.m_uiSprites [0].spriteName = "Effect_Dark";

		yield return true;
	}

	public IEnumerator TakeDamage (int damage, Enemy attacker)
	{

		if (m_doCover && attacker.m_attackType == Enemy.AttackType.Ranged)
		{
			Debug.Log(" TAKING COVER");
			yield return new WaitForSeconds(0.5f);
			UIManager.m_uiManager.SpawnAbilityName("Take Cover", this.transform);
			yield return new WaitForSeconds(0.5f);
			damage = Mathf.Clamp(damage - 2, 0, damage);
		}
		int h = m_currentHealth;
		yield return StartCoroutine(TakeDamage(damage));

		h -= m_currentHealth;
		if (m_currentHealth > 0 && h > 0 && attacker.m_abilityType == Enemy.AbilityType.Wound)
		{

			m_wounds += h;

			yield return new WaitForSeconds(0.5f);
			UIManager.m_uiManager.SpawnAbilityName("Wounded", this.transform);

			string newString = "\\4" + attacker.m_displayName + "\\0 inflicts a Wound on " + "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0!";
			UIManager.m_uiManager.UpdateActions (newString);

			//Update Effect Stack
			EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
			newEffect.m_effectType = EffectsPanel.Effect.EffectType.Wound;
			newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfLevel;
			string desc = "Wound: " + "-" + h.ToString() + " max & until next Level.";
			newEffect.m_description = desc;
			newEffect.m_spriteName = "Effect_Poison";
			newEffect.m_affectedItem = null;
			EffectsPanel.m_effectsPanel.AddEffect(newEffect);

			yield return new WaitForSeconds(0.5f);
		}

		if (m_currentHealth > 0 && h > 0 && attacker.m_abilityType == Enemy.AbilityType.Corrupt)
		{
			yield return new WaitForSeconds(0.5f);
			UIManager.m_uiManager.SpawnAbilityName("Corruption", this.transform);

			string newString ="\\4" +  attacker.m_displayName + "\\0 inflicts Corruption on " + "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0!";
			UIManager.m_uiManager.UpdateActions (newString);

			m_corruption += h;
			GainEnergy(0);

			//Update Effect Stack
			EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
			newEffect.m_effectType = EffectsPanel.Effect.EffectType.Wound;
			newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfLevel;
			string desc = "Corruption: " + "-" + h.ToString() + " max # until next Level.";
			newEffect.m_description = desc;
			newEffect.m_spriteName = "Effect_Hero_Placeholder";
			newEffect.m_affectedItem = null;
			EffectsPanel.m_effectsPanel.AddEffect(newEffect);
			
			yield return new WaitForSeconds(0.5f);
		}
		
		if (m_currentHealth > 0 && (m_doCounterAttack || m_doPermCounterAttack) && attacker != null)
		{
			yield return new WaitForSeconds(0.5f);
			if (m_counterBonus == 0)
			{
				UIManager.m_uiManager.SpawnAbilityName("Counter Attack", this.transform);
			} else {
				UIManager.m_uiManager.SpawnAbilityName("Counter Attack +" + m_counterBonus.ToString(), this.transform);
			}
			yield return new WaitForSeconds(0.5f);
			m_doingCounter = true;
			yield return StartCoroutine(Attack(attacker));
			m_doingCounter = false;
			yield return new WaitForSeconds(1.0f);
			
		}

		if (m_currentHealth > 0 && attacker != null)
		{
			List<Item> pool = new List<Item>(0);
			if (attacker.m_abilityType == Enemy.AbilityType.Rust) // Destroy an equipped item.
			{
				foreach (UICard c in PartyCards.m_partyCards.m_party)
				{
					if (c.m_followerData != null)
					{
						foreach (UICard sc in c.skillCards)
						{
							if (sc.itemData != null)
							{
								pool.Add(sc.itemData);
							}
						}
					}
				}

				if (pool.Count > 0)
				{
					PartyCards.m_partyCards.ClearSkillSelection();

					Item i = (Item)pool[Random.Range(0, pool.Count)];

					yield return new WaitForSeconds(0.5f);
					UIManager.m_uiManager.SpawnAbilityName(i.m_name + " Destroyed", this.transform);
					yield return new WaitForSeconds(0.25f);

					UICard card = PartyCards.m_partyCards.GetSkillCard(i);
					
					// move card to limbo
					float t = 0;
					float time = 0.5f;
					Vector3 startPos = card.transform.position;
					Vector3 localStartPos = card.transform.localPosition;
					Vector3 startScale = card.transform.localScale;

					while (t < time)
					{
						t += Time.deltaTime;;
						Vector3 nPos = Vector3.Lerp(startPos, UIManager.m_uiManager.m_backpackButton.transform.position , t / time);
						Vector3 newScale = Vector3.Lerp(startScale, Vector3.one * 0.25f, t / time);
						card.transform.position = nPos;
						card.transform.localScale = newScale;
						yield return null;
					}
					
					card.transform.localPosition = localStartPos;
					card.transform.localScale = startScale;

					// remove any effects in play from the destroyed item
//					if (i.itemState == Item.ItemState.Spent)
//					{
//						yield return StartCoroutine(i.ItemDestroyed());
//					}

					// remove item
					card.m_followerData.currentSkills --;
					PartyCards.m_partyCards.RemoveSkill(i);
				}
			}
		}

		if (m_reflectDamage > 0 && m_currentHealth > 0 && attacker != null)
		{
			yield return new WaitForSeconds(0.5f);
			UIManager.m_uiManager.SpawnAbilityName("Reflect Damage", this.transform);
			yield return new WaitForSeconds(0.5f);
			yield return StartCoroutine(attacker.TakeDamage(m_reflectDamage));
			yield return new WaitForSeconds(1.0f);
		}

		yield return null;
	}

	public IEnumerator ShakeCamera (int damage)
	{
		float camShakeIntensity = 0.25f;
		float camShakeDecay = 0.017f;
		float hudShakeIntensity = 0.1f;
		float hudShakeDecayMin = 0.003f;
		float hudShakeDecayMax = 0.01f;

		if (damage == 0) {
			camShakeIntensity = 0.05f;
			camShakeDecay = 0.01f;
			hudShakeIntensity = 0.00f;
			hudShakeDecayMin = 0.000f;
			hudShakeDecayMax = 0.00f;
		} else if (damage == 1)
		{
			camShakeIntensity = 0.1f;
			camShakeDecay = 0.02f;
			hudShakeIntensity = 0.05f;
			hudShakeDecayMin = 0.006f;
			hudShakeDecayMax = 0.015f;
		}
		
		// fx
		if (damage > 0) {
			StartCoroutine (HitFlash (0.2f));
		}
		
		ShakeCamera cs = (ShakeCamera)GameManager.m_gameManager.m_camera.GetComponent ("ShakeCamera");
		cs.DoShake (camShakeIntensity, camShakeDecay);
		
		yield return new WaitForSeconds (0.05f);
		
		cs = (ShakeCamera)AssetManager.m_assetManager.m_props [24].GetComponent ("ShakeCamera");
		cs.DoShake (hudShakeIntensity, Random.Range (hudShakeDecayMin, hudShakeDecayMax));
		cs = (ShakeCamera)AssetManager.m_assetManager.m_props [20].GetComponent ("ShakeCamera");
		cs.DoShake (hudShakeIntensity, Random.Range (hudShakeDecayMin, hudShakeDecayMax));
		cs = (ShakeCamera)AssetManager.m_assetManager.m_typogenicText [8].GetComponent ("ShakeCamera");
		cs.DoShake (hudShakeIntensity, Random.Range (hudShakeDecayMin, hudShakeDecayMax));
		cs = (ShakeCamera)AssetManager.m_assetManager.m_typogenicText [6].GetComponent ("ShakeCamera");
		cs.DoShake (hudShakeIntensity, Random.Range (hudShakeDecayMin, hudShakeDecayMax));
		cs = (ShakeCamera)AssetManager.m_assetManager.m_typogenicText [5].GetComponent ("ShakeCamera");
		cs.DoShake (hudShakeIntensity, Random.Range (hudShakeDecayMin, hudShakeDecayMax));
		cs = (ShakeCamera)AssetManager.m_assetManager.m_typogenicText [4].GetComponent ("ShakeCamera");
		cs.DoShake (hudShakeIntensity, Random.Range (hudShakeDecayMin, hudShakeDecayMax));
		
		foreach (GameObject go in UIManager.m_uiManager.cardList) {
			cs = (ShakeCamera)go.GetComponent ("ShakeCamera");
			cs.DoShake (hudShakeIntensity, Random.Range (hudShakeDecayMin, hudShakeDecayMax));
		}
		
		foreach (UICard c in PartyCards.m_partyCards.m_party) {
			cs = (ShakeCamera)c.GetComponent ("ShakeCamera");
			cs.DoShake (hudShakeIntensity, Random.Range (hudShakeDecayMin, hudShakeDecayMax));
			//yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));
		}

		yield return null;
	}
	
	public IEnumerator TakeDamage (int damage)
	{
		
		Debug.Log("TAKING " + damage + " DAMAGE");
		int armor= Mathf.Clamp((m_currentArmor + m_tempArmor + m_turnArmor + m_permArmor + m_currentCard.siteArmorBonus), 0, 99);
		if (m_currentCard.type == Card.CardType.BrokenGround)
		{
			armor = 0;	
		}
		m_tempArmor = 0;
		damage = Mathf.Clamp(damage - armor, 0, 100);
		

		AssetManager.m_assetManager.PlaySFX(AssetManager.SFXType.EnemyHit);
		float camShakeIntensity = 0.25f;
		float camShakeDecay = 0.017f;
		float hudShakeIntensity = 0.1f;
		float hudShakeDecayMin = 0.003f;
		float hudShakeDecayMax = 0.01f;

		StartCoroutine( ShakeCamera (damage));

		
		if (m_mysticActive)
		{
			m_currentEnergy = Mathf.Clamp(m_currentEnergy -= damage, 0, maxEnergy);
			UIManager.m_uiManager.SpawnDamageNumber(damage, this.transform);
			UIManager.m_uiManager.UpdateEnergy(m_currentEnergy);
		}else {
			string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 takes \\5" + damage.ToString () + " damage!";
			UIManager.m_uiManager.UpdateActions (newString);

			m_currentHealth = Mathf.Clamp(m_currentHealth -= damage, 0, maxHealth);

			if (m_doSoulArmor && m_currentHealth < 1)
			{
				m_currentHealth = 1;
			}

			UIManager.m_uiManager.SpawnDamageNumber(damage, this.transform);
			UIManager.m_uiManager.UpdateHealth(m_currentHealth);
		}
		
		if (m_currentHealth == 0)
		{
			string newString ="\\1" +  GameManager.m_gameManager.currentFollower.m_nameText + "\\0 succumbs!";
			UIManager.m_uiManager.UpdateActions (newString);

			animation.Stop();
			animation.Play("PlayerDeath01");
			StartCoroutine(UIManager.m_uiManager.TurnOffTargetCard());
			foreach (Enemy e in GameManager.m_gameManager.currentMap.m_enemies)
			{
				e.ToggleStatBar(false);
			}
			yield return new WaitForSeconds(0.25f);
			CineCam.m_cineCam.ActivateCineCam (Player.m_player.transform);
			yield return new WaitForSeconds(3.0f);
			StartCoroutine(UIManager.m_uiManager.ChangeMenuMode(UIManager.MenuMode.GameOver));
			yield break;
		} else {
			if (m_berserkerActive)
			{
				GainActionPoints(1);	
			}
			
//			if (GameManager.m_gameManager.HasFollower(Follower.FollowerType.Wrestler))
//			{
//				
//			}
		}
		
		//refresh any used armor
//		foreach (UICard thisWeapon in UIManager.m_uiManager.m_equipSlots)
//		{
//			if (thisWeapon.m_itemData != null)
//			{
//				if (thisWeapon.m_itemData.itemState == Item.ItemState.Spent && thisWeapon.m_itemData.m_armorBonusDice.Length > 0)
//				{
//					thisWeapon.m_itemData.ChangeState(Item.ItemState.Normal);	
//				}
//			}
//		}
		
		EffectsPanel.m_effectsPanel.UpdateEffects(EffectsPanel.Effect.Duration.NextPlayerHit);
		
		yield return null;
	}
	
	public void GainEnergy (int amount)
	{
		if (amount > 0) {
			string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 regains " + amount.ToString() + " Energy";
			UIManager.m_uiManager.UpdateActions (newString);
		}

		m_currentEnergy = Mathf.Clamp(m_currentEnergy + amount, 0, maxEnergy - m_corruption);	
		UIManager.m_uiManager.UpdateEnergy(m_currentEnergy);
	}
	
	public void GainHealth (int amount)
	{
		if (amount > 0) {
			string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 regains " + amount.ToString() + " Health";
			UIManager.m_uiManager.UpdateActions (newString);
		}

		m_currentHealth = Mathf.Clamp(m_currentHealth + amount, 0, maxHealth - m_wounds);	

		UIManager.m_uiManager.UpdateHealth(m_currentHealth);
	}
	
	public void ChangeEffectState (GameManager.StatusEffect effect, int duration)
	{
//		m_currentEffect = newEffect;	
//		m_effectDuration = duration;
		
		if (effect == GameManager.StatusEffect.Poison)
		{
			//UIManager.m_uiManager.UpdateEffectGUI(GameManager.StatusEffect.Poison, m_effectDuration);
			m_poisonDuration += duration;
			
			//Update Effect Stack
			EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
			newEffect.m_effectType = EffectsPanel.Effect.EffectType.PlayerPoisoned;
			newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
			string desc = "Poisoned: " + m_poisonDuration.ToString() + " turns";
			newEffect.m_description = desc;
			newEffect.m_stackable = false;
			newEffect.m_spriteName = "Effect_Poison";
			EffectsPanel.m_effectsPanel.AddEffect(newEffect);
			
		} else if (effect == GameManager.StatusEffect.None)
		{
			
		}
	}

	public IEnumerator TakeDirectDamage (int damage)
	{
		m_currentHealth = Mathf.Clamp(m_currentHealth - damage, 0, maxHealth);

		if (m_doSoulArmor && m_currentHealth < 1)
		{
			m_currentHealth = 1;
		}

		UIManager.m_uiManager.UpdateHealth(m_currentHealth);

		if (m_currentHealth == 0)
		{
			StartCoroutine(UIManager.m_uiManager.ChangeMenuMode(UIManager.MenuMode.GameOver));
			yield break;
		} 

		yield return true;
	}
	
	public IEnumerator DoKnockback (GameManager.Direction direction)
	{

		//yield return StartCoroutine(MovePlayer(direction));
		Card[] linkedCards = m_currentCard.linkedCards;
		Card nextCard = linkedCards[0];
		if (direction == GameManager.Direction.South)
		{
			nextCard = linkedCards[1];
		} else if (direction == GameManager.Direction.East)
		{
			nextCard = linkedCards[2];
		}if (direction == GameManager.Direction.West)
		{
			nextCard = linkedCards[3];
		}
		
//		if (thisCard != null)
//		{
//			if (!thisCard.isOccupied)	
//			{
//				return true;	
//			}
//		}
		
		animation.Stop();
		//animation["PlayerJump01"].speed = 0.5f;
		
		m_moveTimer = 0;
		m_moveTime = animation["PlayerJump01"].length;
		m_moveStart = m_currentCard.transform.position;
		m_moveEnd = nextCard.transform.position;
		
		//Debug.Log(m_currentCard.row.ToString() + "," + m_currentCard.column.ToString() + " / " + nextCard.row.ToString() + "," + nextCard.column.ToString());
		
		animation.Play("PlayerJump01");
		
		//while (animation.IsPlaying("PlayerJump01"))
		while (m_moveTimer < m_moveTime)
		{
			m_moveTimer = Mathf.Clamp(m_moveTimer + Time.deltaTime, 0, m_moveTime);
			float t = Mathf.Clamp(m_moveTimer / m_moveTime, 0, 1);
			animation["PlayerJump01"].time = m_moveTime * t;
			Vector3 newPos = Vector3.Lerp(m_moveStart, m_moveEnd, t);
			
			if (m_moveTimer == m_moveTime)
			{				m_currentCard.player = null;
				m_currentCard = nextCard;
				m_currentCard.player = this;
				
				newPos = m_moveEnd;
				animation.Stop();
				if (m_currentCard.type == Card.CardType.Exit)
				{

					yield return StartCoroutine(UIManager.m_uiManager.ChangeMenuMode(UIManager.MenuMode.EndLevel));
					//MapManager.m_mapManager.SetMapActive(m_currentCard.mapID);
					yield break;
				}
//				else if (m_currentCard.type == Card.CardType.Entrance || m_currentCard.type == Card.CardType.DungeonEntrance)
//				{
//					MapManager.m_mapManager.SetMapActive(m_currentCard.mapID);
//					yield break;
//				}
			}
			transform.position = newPos;
			yield return null;	
		}
		
		//animation["PlayerJump01"].speed = 0;
		yield return null;	
	}

	public bool CanBeKnockedBack (GameManager.Direction direction)
	{
		Card[] linkedCards = m_currentCard.linkedCards;
		Card thisCard = linkedCards[0];
		if (direction == GameManager.Direction.South)
		{
			thisCard = linkedCards[1];
		} else if (direction == GameManager.Direction.East)
		{
			thisCard = linkedCards[2];
		}if (direction == GameManager.Direction.West)
		{
			thisCard = linkedCards[3];
		}
		
		if (thisCard != null)
		{
			if (!thisCard.isOccupied)	
			{
				return true;	
			}
		}
		
		return false;
	}
	
	public bool IsAdjacentOpen (GameManager.Direction direction)
	{
		Card[] linkedCards = m_currentCard.linkedCards;
		Card adjacentCard = null;
		if (direction == GameManager.Direction.North && linkedCards[0] != null)
		{
			adjacentCard = linkedCards[0];
			
		} else if (direction == GameManager.Direction.South && linkedCards[1] != null)
		{
			adjacentCard = linkedCards[1];
		} else if (direction == GameManager.Direction.East && linkedCards[2] != null)
		{
			adjacentCard = linkedCards[2];
		} else if (direction == GameManager.Direction.West && linkedCards[3] != null)
		{
			adjacentCard = linkedCards[3];
		}
		
		if (adjacentCard != null)
		{
			if (adjacentCard.chest == null && adjacentCard.enemy == null && adjacentCard.follower == null && adjacentCard.cardState == Card.CardState.Normal)
			{
				return true;
			}
		}
		
		return false;	
	}
	
	public void ChangeFacing (GameManager.Direction dir)
	{
		m_facing = dir;
		Vector3 newDirection = 	m_playerMesh.transform.eulerAngles;
		switch (dir)
		{
		case GameManager.Direction.North:
			newDirection.y = 0;
			break;
		case GameManager.Direction.South:
			newDirection.y = 180;
			break;
		case GameManager.Direction.East:
			newDirection.y = 90;
			break;
		case GameManager.Direction.West:
			newDirection.y = 270;
			break;
		}
		
		m_playerMesh.transform.eulerAngles = newDirection;
	}
	
	public void AddNewFollower (Follower newFollower)
	{
		List<Follower> fList = new List<Follower>();
		fList.Add(newFollower);
		
		List<Follower> newCurrentF = GameManager.m_gameManager.followers;
		newCurrentF.Add(newFollower);
		PartyCards.m_partyCards.AddFollower(newFollower);
		SetPassiveFollowerBonuses(newCurrentF);
	}
	
	public void SetPassiveFollowerBonuses (List<Follower> followers)
	{
		Debug.Log ("SETTING PASSIVE BONUSES");
		List<Follower.FollowerType> followerTypes = new List<Follower.FollowerType>();
		
		m_damage = SettingsManager.m_settingsManager.startingDamage;
		m_currentArmor = SettingsManager.m_settingsManager.startingArmor;
		m_maxHealth = SettingsManager.m_settingsManager.startingHealth;
		m_maxEnergy = SettingsManager.m_settingsManager.startingEnergy;
		
		foreach (Follower thisFollower in followers)
		{
			Debug.Log("FOLLOWER: " + thisFollower.m_nameText + " FOLLOWER LEVEL: " + thisFollower.currentLevel);
			int numPassive = 0;

			if (thisFollower.levelModifiers != null)
			{
//				Follower.Level l = thisFollower.levelModifiers;

//				Debug.Log("Damage Mod: " + l.m_damageMod);
//				Debug.Log("Armor Mod: " + l.m_armorMod);
//				Debug.Log("Energy Mod: " + l.m_energyMod);
//				Debug.Log("Health Mod: " + l.m_healthMod);
//				m_damage += l.m_damageMod;
//				m_currentArmor += l.m_armorMod;
//				m_maxEnergy += l.m_energyMod;
//				m_maxHealth += l.m_healthMod;
				m_damage += thisFollower.badgeBonus_PassiveDamage;
				m_currentArmor += thisFollower.badgeBonus_PassiveArmor;
				m_maxEnergy += thisFollower.badgeBonus_PassiveEnergy;
				m_maxHealth += thisFollower.badgeBonus_PassiveHealth;

				//clamp values
				if (m_currentEnergy > m_maxEnergy){m_currentEnergy = m_maxEnergy;}
				if (m_currentHealth > maxHealth){m_currentHealth = maxHealth;}

//				thisFollower.m_abilityCost = thisFollower.baseAbilityCost + l.m_abilityCostMod;
//				thisFollower.m_abilityEffect = thisFollower.baseAbilityEffect + l.m_abilityEffectMod;
//				thisFollower.m_abilityRange = thisFollower.baseAbilityRange + l.m_abilityRangeMod;
//				
			} else {
				Debug.Log("LEVEL MODIFIERS IS NULL");
			}
			//Debug.Log("DAMAGE: " + m_damage);
		}

		UIManager.m_uiManager.UpdateHealth(m_currentHealth);
		StartCoroutine(UIManager.m_uiManager.UpdateDamage(m_damage + m_tempDamage + m_turnDamage + m_permDamage + m_currentCard.siteDamageBonus));
		UIManager.m_uiManager.UpdateEnergy(m_currentEnergy);
		StartCoroutine(UIManager.m_uiManager.UpdateArmor(m_currentArmor + m_tempArmor + m_turnArmor + m_permArmor + m_currentCard.siteArmorBonus));
	}
	
	public void RefillHealth ()
	{
		m_currentHealth = maxHealth;
		UIManager.m_uiManager.UpdateHealth(m_currentHealth);
	}
	public void RefillEnergy ()
	{
		m_currentEnergy = maxEnergy;
		UIManager.m_uiManager.UpdateEnergy(m_currentEnergy);
	}
	
	public void RemovePassiveFollowerBonuses (List<Follower> followers)
	{
		List<Follower.FollowerType> followerTypes = new List<Follower.FollowerType>();
		foreach (Follower thisFollower in followers)
		{
			followerTypes.Add(thisFollower.m_followerType);	
		}
		
		foreach (Follower.FollowerType ft in followerTypes)
		{
			switch (ft)
			{
//			case Follower.FollowerType.Brand:
//				m_damage -= 5;
//				break;
			case Follower.FollowerType.Elf:
				m_flipRange --;
				break;
//			case Follower.FollowerType.Knight:
//				m_currentArmor -= 3;
//				break;
			}
		}
		UIManager.m_uiManager.UpdateHealth(m_currentHealth);
		StartCoroutine(UIManager.m_uiManager.UpdateDamage(m_damage + m_tempDamage + m_turnDamage + m_permDamage + m_currentCard.siteDamageBonus));
		UIManager.m_uiManager.UpdateEnergy(m_currentEnergy);
		StartCoroutine(UIManager.m_uiManager.UpdateArmor(m_currentArmor + m_tempArmor + m_turnArmor + m_permArmor + m_currentCard.siteArmorBonus));
	}


	//getters setters
	public int currentEnergy
	{
		get
		{
			return m_currentEnergy;	
		}
		set
		{
			m_currentEnergy = value;	
		}
	}
	public int flipRange
	{
		get
		{
			return m_flipRange;	
		}
		set
		{
			m_flipRange = value;	
		}
	}
	public PlayerState playerState
	{
		get
		{
			return m_playerState;	
		}
	}
	public Card currentCard
	{
		get
		{
			return m_currentCard;
		}
		set
		{
			m_currentCard = value;	
		}
	}
	public GameManager.Direction facing
	{
		get
		{
			return m_facing;	
		}
	}
	public int tempDamage
	{
		get
		{
			return m_tempDamage;	
		}
		set
		{
			m_tempDamage = value;	
		}
	}
	public int numKeys
	{
		get
		{
			return m_numKeys;	
		}
		set
		{
			m_numKeys = value;	
		}
	}

	public int tempArmor {get{return m_tempArmor;}set{m_tempArmor = value;}}
	public int turnArmor {get{return m_turnArmor;}set{
			m_turnArmor = value;
			string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 gains " + value.ToString() + " Armor";
			UIManager.m_uiManager.UpdateActions(newString);
			StartCoroutine(UIManager.m_uiManager.UpdateArmor(Player.m_player.currentArmor + Player.m_player.turnArmor + Player.m_player.tempArmor + m_permArmor + m_currentCard.siteArmorBonus ));}}
	public int turnDamage {get{return m_turnDamage;}set{
			m_turnDamage = value; 
			string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 gains " + value.ToString() + " Attack";
			UIManager.m_uiManager.UpdateActions(newString);
			StartCoroutine(UIManager.m_uiManager.UpdateDamage(Player.m_player.damage + Player.m_player.turnDamage + Player.m_player.tempDamage + m_permDamage + m_currentCard.siteDamageBonus ));}}
	public bool berserkerActive {get{return m_berserkerActive;}set{m_berserkerActive = value;}}
	public bool mysticActive {get{return m_mysticActive;}set{m_mysticActive = value;}}
	public int damage {get{return m_damage;}}
	public int maxHealth {get{return m_maxHealth + m_permHealth;}}
	public int maxEnergy {get{return m_maxEnergy + m_permEnergy;}}
	public int currentHealth {get{return m_currentHealth;} set {m_currentHealth = value;}}
	public int currentArmor {get{return m_currentArmor;}}
	public bool cardsFlipping {get{return m_cardsFlipping;}}
	public int currentActionPoints {get {return m_currentActionPoints; } set{m_currentActionPoints = value; UIManager.m_uiManager.UpdateActionPoints(m_currentActionPoints); }}
	public GameManager.StatusEffect currentEffect {get{return m_currentEffect;}}
	public bool doPoisonAttack {get{return m_doPoisonAttack;} set{m_doPoisonAttack = value;}}
	public bool doCounterAttack {get{return m_doCounterAttack;} set{m_doCounterAttack = value;}}
	public bool doPsychicAttack {get{return m_doPsychicAttack;} set{m_doPsychicAttack = value;}}
	public bool doFencer {get{return m_doFencer;} set{m_doFencer = value;}}
	public bool doSamurai {get{return m_doSamurai;} set{m_doSamurai = value;}}
	public bool doDancer {get{return m_dancerActive;} set{m_dancerActive = value;}}
	public bool doArmorPierce {get{return m_doArmorPierce;} set{m_doArmorPierce = value;}}
	public bool doSoulArmor {get{return m_doSoulArmor;} set{m_doSoulArmor = value;}}
	public bool canContinuousMove {get{return m_canContinuousMove;} set{m_canContinuousMove = value;}}
	public int poisonDuration {get {return m_poisonDuration;} set {m_poisonDuration = value;}}
	public int reflectDamage {get {return m_reflectDamage;} set {m_reflectDamage = value;}}
	public int lifeTap {get {return m_lifeTap;} set {m_lifeTap = value;}}
	public int soulTap {get {return m_soulTap;} set {m_soulTap = value;}}
	public bool doSacrifice {get {return m_doSacrifice;} set {m_doSacrifice = value;}}
	public bool doCover {get {return m_doCover;} set {m_doCover = value;}}
	public UICard statBar {get{return m_statBar;}}
	public int counterBonus {get {return m_counterBonus;} set {m_counterBonus = value;}}
	public int samuraiBonus {get {return m_samuraiBonus;} set {m_samuraiBonus = value;}}
	public int wounds {get{return m_wounds;} set {m_wounds = value;}}
	public int corruption {get{return m_corruption;} set {m_corruption = value;}}
	public Follower playerFollower {get {return m_playerFollower;}}
	public int rangedBonus {get{return m_turnRangedBonus;}set{m_turnRangedBonus = value;}}
	public int permRangedDamageBonus {get{return m_permRangedDamageBonus;}set{m_permRangedDamageBonus = value;}}
	public int permDamage {get{return m_permDamage;}set{m_permDamage = value;StartCoroutine(UIManager.m_uiManager.UpdateDamage(Player.m_player.damage + Player.m_player.turnDamage + Player.m_player.tempDamage + m_permDamage + m_currentCard.siteDamageBonus));}}
	public int permArmor {get{return m_permArmor;}set{m_permArmor = value;StartCoroutine(UIManager.m_uiManager.UpdateArmor(Player.m_player.currentArmor + Player.m_player.turnArmor + Player.m_player.tempArmor + m_permArmor + m_currentCard.siteArmorBonus));}}
	public int permHealth {get{return m_permHealth;}set{m_permHealth = value; UIManager.m_uiManager.UpdateHealth(Player.m_player.currentHealth); }}
	public int permEnergy {get{return m_permEnergy;}set{m_permEnergy = value; UIManager.m_uiManager.UpdateEnergy(Player.m_player.currentEnergy); }}
	public int permActions {get{return m_permActionBonus;}set{m_permActionBonus = value;}}
	public int stunDuration {get{return m_stunDuration;}set{m_stunDuration = value;}}
	public bool doPermArmorPierce {get{return m_doPermArmorPierce;}set{m_doPermArmorPierce = value;}}
	public bool doPermCounterAttack {get{return m_doPermCounterAttack;}set{m_doPermCounterAttack = value;}}
//	public bool doingSiteCard {get{return m_doingSiteCard;}}
}
