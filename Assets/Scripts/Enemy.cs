using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {
	
	public enum EnemyState
	{
		Inactive, // not revealed
		Idle, // revealed, but not actively engaging player
		Dead,
		Active, // engaging player (not used yet)
	}
	
	public enum AttackType
	{
		Melee,
		Ranged,
	}

	public enum EnemyType
	{
		None,
		Beast,
		Demon,
		Construct,
		Pet,
		Boss,
	}
	
	public enum AbilityType
	{
		None,
		Rush, // move 2 times
		Counterattack,
		Summon,
		Web,
		Poison,
		Knockback,
		Stun,
		Firewalker,
		Farsight,
		Flight,
		Quicksand,
		Heal,
		CriticalHit,
		Shield,
		Frenzy,
		Mob,
		Firestrength,
		Wall, // doesn't attack
		Charge, // if 1 tile away, move and attack
		EnergySyphon, // drains 1 energy from party when killed (by anything)
		Shadow, // turns the player's Site into Darkness
		Sapper, // drops a bomb upon death
		Explode, // deals x damage to adjacent sites
		Regen, // gives health back to the player when enemy dies
		Rust, // Destroys an equipped item when it attacks
		Ambush, // The card they are on is immediately revealed when the player lands on an adjacent site
		Celerity, // +2 Actions when defeating enemy
		Voodoo, // Ranged Stun
		Dummy, // don't do anything
		Wound, // wound damage can't be healed until the next level
		Corrupt, // reduces max Energy until next level
		GraveDigger, // +1D for each enemy in the grave
		Discard, //discard a random card from the player's hand
		DrawCostMod, // increase draw cost if player is in range
	}
	
	public MeshRenderer[]
		m_enemyMesh;
	
	public int
		m_maxHealth = 20,
		m_damage = 10,
		m_energy = 0,
		m_armor = 0,
		m_chaseDistance = 5,
		m_range = 0,
		m_lootLevel = 0;
	
	public AttackType
		m_attackType = AttackType.Melee;
	
	public AbilityType
		m_abilityType = AbilityType.None;
	
	public GameObject
		m_summon;
	
	public int 
		m_poisonDuration = 0,
		m_level = 0,
		m_abilityEnergyCost = 0,
		m_abilityModifier = 0,
		m_abilityRange = 0;

	public EnemyType
		m_enemyType = EnemyType.Beast;
	
	public bool
		m_doDropChest = false;
	
	public string
		m_displayName = "Enemy",
		m_abilityText = "Ability Text",
		m_portraitSpriteName = "Card_Portrait_Skel",
		m_stackPortraitName = "Portrait_Enemy";
	
	private GameManager.Direction
		m_facing = GameManager.Direction.North;
	
	private int
		m_currentHealth = 0,
		m_timesChased = 0,
		m_effectDuration = 0,
		m_turnArmor = 0,
		m_turnDamage = 0,
		m_stunDuration = 0,
		m_graveDamageBonus = 0,
		m_initiative = 0;
	
	private bool
		m_activatedThisTurn = false,
		m_damagedThisTurn = false,
		m_doStatBar = true,
		m_inFocus = false;
	
	private EnemyState 
		m_enemyState = EnemyState.Inactive;
	
	protected Card
		m_currentCard = null;

	private UICard
		m_statBar = null;
	
	private GameManager.StatusEffect
		m_currentEffect = GameManager.StatusEffect.None;
	
	protected Vector3
		m_moveStart = Vector3.zero,
		m_moveEnd = Vector3.zero;
	
	protected float
		m_moveTime = 0,
		m_moveTimer = 0;

	// Use this for initialization
	void Start () {
	
	}

	void OnDestroy ()
	{
		if (m_statBar != null)
		{
			Destroy(m_statBar.gameObject);
		}
	}
	
	public void Initialize (EnemyState state, Card currentCard)
	{
		m_currentHealth = m_maxHealth;	
		m_currentCard = currentCard;
		this.transform.parent = m_currentCard.cardMesh.transform;
		
		//randomize starting direction
		List<GameManager.Direction> startingDir = new List<GameManager.Direction>();
		startingDir.Add(GameManager.Direction.North);
		startingDir.Add(GameManager.Direction.South);
		startingDir.Add(GameManager.Direction.East);
		startingDir.Add(GameManager.Direction.West);
		ChangeFacing(startingDir[Random.Range(0, startingDir.Count)]);
		
		StartCoroutine( ChangeState(state));
		
		if (m_abilityType == AbilityType.Firewalker && m_currentCard.type != Card.CardType.Fire)
		{
			StartCoroutine(m_currentCard.ChangeCardType((Card)(MapManager.m_mapManager.m_cardTypes[14].GetComponent("Card"))));	
			m_currentCard.type = Card.CardType.Fire;
			m_currentCard.doUpkeep = true;
		}

		animation["EnemyIdle01"].speed = Random.Range(1.0f, 2.0f);
		animation ["EnemyIdle01"].time = Random.Range (0, 1.0f);
	}
	
	public void Upkeep()
	{
		m_turnArmor = 0;
		m_turnDamage = 0;

		// determine if enemy is active this turn
		if (m_enemyState == EnemyState.Idle && CanAct() && !m_activatedThisTurn)
		{
			StartCoroutine(ChangeState(EnemyState.Active));
		}

		if (m_activatedThisTurn)
		{
			m_activatedThisTurn = false;
		}
	}
	private bool CanAct ()
	{
		bool canAct = false;
		if (CheckForAdjacentPlayerNoFacing() || m_currentCard.distanceToPlayer <= m_chaseDistance || m_currentEffect != GameManager.StatusEffect.None || m_stunDuration > 0)
		{
			canAct = true;
		} else if ((m_attackType == AttackType.Ranged && m_abilityType != AbilityType.Web) || (m_attackType == AttackType.Ranged && m_abilityType == AbilityType.Web && m_energy >=2 && Player.m_player.currentEnergy > 0) 
		           || (m_abilityType == AbilityType.Quicksand && Player.m_player.currentCard.type != Card.CardType.Quicksand && m_energy >=2 ))
		{
			List<GameManager.Direction> directions = new List<GameManager.Direction>();
			directions.Add(GameManager.Direction.North);
			directions.Add(GameManager.Direction.South);
			directions.Add(GameManager.Direction.East);
			directions.Add(GameManager.Direction.West);
			List<Card> validCards = MapManager.m_mapManager.GetCardsInRange(m_currentCard, (m_range + m_currentCard.siteRangeBonus), directions, false, false, false);
			
			foreach (Card thisCard in validCards)
			{
				if (thisCard == Player.m_player.currentCard)
				{
					canAct = true;
					break;
				}
			}
		} else if (m_abilityType == AbilityType.Flight && m_currentCard.distanceToPlayer <= m_chaseDistance+1)
		{
			canAct = true;
		} else if (m_abilityType == AbilityType.Charge &&  m_currentCard.distanceToPlayer == 2 && m_energy >= 2)
		{
			canAct = true;
		} else if ((m_abilityType == AbilityType.Shield || m_abilityType == AbilityType.Frenzy) && currentCard.distanceToPlayer < 3 && m_energy >= 1)
		{
			canAct = true;
		} else if (m_abilityType == AbilityType.Farsight && m_energy >= 1)
		{
			canAct = true;
		} else if (CheckForAdjacentPet())
		{
			canAct = true;
		} else if (m_abilityType == AbilityType.Heal && currentCard.distanceToPlayer <= m_chaseDistance+1 && m_energy >= 2)
		{
			canAct = true;
		}
		return canAct;
	}

	public IEnumerator GetFocus ()
	{
		m_inFocus = true;

		FollowCamera.m_followCamera.SetTarget(this.gameObject);
		
		yield return StartCoroutine(UIManager.m_uiManager.DisplayTargetEnemy(this));
		
		yield return new WaitForSeconds(1.0f);

		yield return null;
	}
	
	public virtual IEnumerator DoTurn () {

		Debug.Log ("STARTING TURN");

		//resolve status effects
		if (m_currentEffect == GameManager.StatusEffect.Poison)
		{
			UpdateTurnIcon ();
			//FollowCamera.m_followCamera.SetTarget(this.gameObject);
			yield return StartCoroutine(GetFocus());

			yield return new WaitForSeconds(1);
			UIManager.m_uiManager.SpawnAbilityName((GameManager.StatusEffect.Poison).ToString(), this.transform);
			yield return new WaitForSeconds(1);
			StartCoroutine(TakeDamage(2));
			if (m_currentHealth <= 0)
			{
				yield break;	
			}
			yield return new WaitForSeconds(1);
			m_effectDuration -= 1;
			if (m_effectDuration <=0)
			{
				ChangeEffectState(GameManager.StatusEffect.None, 0);	
			}
		}


		//check current card to see if effects need to be applied
		if (m_currentCard.doUpkeep) {
			Debug.Log("DOING UPKEEP");
			yield return StartCoroutine(m_currentCard.DoTurn());	
			if (m_enemyState == EnemyState.Dead)
			{
				Debug.Log("ENEMY DEAD, ABORTING TURN");
				yield break;
			}
		}


		if (!m_activatedThisTurn && m_stunDuration == 0)
		{
			//check for use of farsight ability
			if (m_abilityType == AbilityType.Farsight && m_energy >= 1)
			{

				yield return StartCoroutine (GainEnergy(-1));
				List<GameManager.Direction> dir = new List<GameManager.Direction>();
				dir.Add(GameManager.Direction.North);
				dir.Add(GameManager.Direction.South);
				dir.Add(GameManager.Direction.East);
				dir.Add(GameManager.Direction.West);
				List<Card> cardsInRange = MapManager.m_mapManager.GetCardsInRange(m_currentCard, 3, dir, false, false, true);
				List<Card> hCards = new List<Card>();
				int hiddenCards = 0;
				foreach (Card thisCard in cardsInRange)
				{
					if (thisCard.cardState == Card.CardState.Hidden)
					{
						hiddenCards++;	
						hCards.Add(thisCard);
					}
				}
				
				if (hiddenCards > 1)
				{
					UpdateTurnIcon ();
					//FollowCamera.m_followCamera.SetTarget(this.gameObject);
					yield return StartCoroutine(GetFocus());
					yield return new WaitForSeconds(0.5f);
					UIManager.m_uiManager.SpawnAbilityName(m_abilityType.ToString(), this.transform);
					yield return new WaitForSeconds(0.5f);

					foreach (Card card in hCards)
					{
						yield return StartCoroutine(card.ChangeCardState(Card.CardState.Normal));	
					}
					
					yield return new WaitForSeconds(0.5f);
					yield return StartCoroutine(m_currentCard.ActivateCard());
					yield break;
				}
			}
			
			//check for buffing abilities
			if ((m_abilityType == AbilityType.Shield || m_abilityType == AbilityType.Frenzy) && currentCard.distanceToPlayer < 3 && m_energy >= 1)
			{
				//int adjacentAllies = 0;
				List<Enemy> adjacentAllies = new List<Enemy>();
				foreach (Card thisCard in currentCard.linkedCards)
				{
					if (thisCard != null)
					{
						if (thisCard.enemy != null && thisCard.cardState == Card.CardState.Normal)
						{
							adjacentAllies.Add(thisCard.enemy);
						}
					}
				}
				
				if (adjacentAllies.Count > 1)
				{
					UpdateTurnIcon ();
					yield return StartCoroutine (GainEnergy(-1));
					
					//FollowCamera.m_followCamera.SetTarget(this.gameObject);
					yield return StartCoroutine(GetFocus());
					//yield return new WaitForSeconds(0.5f);
					UIManager.m_uiManager.SpawnAbilityName(m_abilityType.ToString(), this.transform);
					yield return new WaitForSeconds(0.5f);	
					
					foreach (Enemy thisEnemy in adjacentAllies)
					{
						//FollowCamera.m_followCamera.SetTarget(thisEnemy.gameObject);
						yield return StartCoroutine(GetFocus());
						if (m_abilityType == AbilityType.Shield)
						{
							thisEnemy.turnArmor += 2;
							UIManager.m_uiManager.SpawnDamageNumber(2, thisEnemy.transform);
						} else if (m_abilityType == AbilityType.Frenzy)
						{
							thisEnemy.turnDamage += 3;	
							UIManager.m_uiManager.SpawnDamageNumber(3, thisEnemy.transform);
						}
						yield return new WaitForSeconds(0.5f);
					}
					yield return StartCoroutine(m_currentCard.ActivateCard());
					yield break;	
				}
			}
			
			//if player is adjacent, then attack
			bool playerAdjacent = CheckForAdjacentPlayer();
			bool adjacentPet = false;
			if (!playerAdjacent) {adjacentPet = CheckForAdjacentPet();}
			
			if (playerAdjacent && m_abilityType != AbilityType.Wall && m_abilityType != AbilityType.Dummy)
			{
				m_timesChased = 0;
				
				if (m_abilityType == AbilityType.CriticalHit && GameManager.m_gameManager.numStunned < GameManager.m_gameManager.followers.Count-1 && GameManager.m_gameManager.followers.Count > 1 && m_energy >= 3)
				{
					UpdateTurnIcon ();
					yield return StartCoroutine (GainEnergy(-3));
					//FollowCamera.m_followCamera.SetTarget(this.gameObject);
					yield return StartCoroutine(GetFocus());
					//yield return new WaitForSeconds(0.5f);
					UIManager.m_uiManager.SpawnAbilityName(m_abilityType.ToString(), this.transform);
					yield return new WaitForSeconds(0.5f);
					FollowCamera.m_followCamera.SetTarget(Player.m_player.m_playerMesh.gameObject);
					foreach (Follower thisFollower in GameManager.m_gameManager.followers)
					{
						if (thisFollower.m_followerType != GameManager.m_gameManager.playerFollower.m_followerType)
						{
							yield return new WaitForSeconds(0.05f);
							yield return StartCoroutine(thisFollower.DoStun(3));	
						}
					}
					yield return new WaitForSeconds(0.5f);
					yield return StartCoroutine(m_currentCard.ActivateCard());
					yield break;
					
				} else if (m_abilityType == AbilityType.Knockback && Player.m_player.CanBeKnockedBack(m_facing))
				{
					UpdateTurnIcon ();
					//FollowCamera.m_followCamera.SetTarget(this.gameObject);
					yield return StartCoroutine(GetFocus());
					yield return new WaitForSeconds(0.5f);
					UIManager.m_uiManager.SpawnAbilityName(m_abilityType.ToString(), this.transform);

					string newString = m_displayName + " uses Knockback on " + GameManager.m_gameManager.currentFollower.m_nameText;
					UIManager.m_uiManager.UpdateActions (newString);

					yield return new WaitForSeconds(1.0f);
					
					Debug.Log("ATTACKING PLAYER");
					animation.Play("EnemyJump01");
					yield return StartCoroutine(Attack());
					yield return new WaitForSeconds(0.25f);
					
					yield return StartCoroutine(Player.m_player.DoKnockback(m_facing));
					yield return new WaitForSeconds(0.5f);
					FollowCamera.m_followCamera.SetTarget(Player.m_player.m_playerMesh.gameObject);
					animation.Play("EnemyIdle01");
					yield break;
				} else if (m_abilityType == AbilityType.Stun)
				{
					// get all unstunned heroes
					
//					List<Follower> unstunned = new List<Follower>();
//					foreach (Follower f in GameManager.m_gameManager.followers)
//					{
//						if (f.followerState != Follower.FollowerState.Stunned)
//						{
//							unstunned.Add(f);
//						}
//					}
					
					//if there are more than 0 unstunned heroes, choose one at random
					
//					if (unstunned.Count > 0)
//					{	
//						UpdateTurnIcon ();
//						Follower randFollower = unstunned[Random.Range(0, unstunned.Count)];
//						
//						FollowCamera.m_followCamera.SetTarget(this.gameObject);
//						
//						yield return new WaitForSeconds(0.5f);
//						UIManager.m_uiManager.SpawnAbilityName(m_abilityType.ToString(), this.transform);
//						yield return new WaitForSeconds(1.0f);
//						yield return StartCoroutine(randFollower.DoStun(3));
//						yield return new WaitForSeconds(0.5f);
//						FollowCamera.m_followCamera.SetTarget(Player.m_player.m_playerMesh.gameObject);
//						yield break;
//					}

					yield return StartCoroutine(Player.m_player.playerFollower.DoStun(m_poisonDuration));
				}
				
				//attack	
				Debug.Log("ATTACKING PLAYER");

				yield return StartCoroutine(GetFocus());

				animation.Play("EnemyJump01");
				yield return StartCoroutine(Attack());

				if (m_abilityType == AbilityType.Discard && GameManager.m_gameManager.inventory.Count > 0)
				{
					// get a random item
					int r = Random.Range(0, GameManager.m_gameManager.inventory.Count);
					Item randItem = (Item)GameManager.m_gameManager.inventory[r];

					string newString = m_displayName + " sends " + randItem.m_name + " to The Grave";
					UIManager.m_uiManager.UpdateActions (newString);

					// send item to grave
					randItem.card.transform.localScale = UIManager.m_uiManager.selectedScale;
					yield return StartCoroutine(randItem.CenterCard());
					yield return StartCoroutine(randItem.SendToGrave());
					
				}

				yield return new WaitForSeconds(1.5f);
				animation.Play("EnemyIdle01");
//				StartCoroutine(UIManager.m_uiManager.TurnOffTargetCard());
//				yield return new WaitForSeconds(0.5f);

			} 
			else if (adjacentPet && m_abilityType != AbilityType.Wall && m_abilityType != AbilityType.Dummy)
			{
				Pet p = null;
				for (int i=0; i < m_currentCard.linkedCards.Length; i++)
				{
					Card lc = m_currentCard.linkedCards[i];
					if (lc != null)
					{
						if (lc.enemy != null)
						{
							if (lc.enemy.m_enemyType == EnemyType.Pet)
							{
								if (i ==0)
								{
									ChangeFacing(GameManager.Direction.North);	
								} else if (i == 1)
								{
									ChangeFacing(GameManager.Direction.South);	
								} else if (i == 2)
								{
									ChangeFacing(GameManager.Direction.East);	
								} else if (i == 3)
								{
									ChangeFacing(GameManager.Direction.West);	
								}
								p = (Pet)lc.enemy;
								i = 99;
							}
						}
					}
				}

				if (p != null)
				{
					//attack pet
					Debug.Log("ATTACKING PET");
					//FollowCamera.m_followCamera.SetTarget(this.gameObject);
					yield return StartCoroutine(GetFocus());
					yield return new WaitForSeconds(0.5f);
					animation.Play("EnemyJump01");
					yield return StartCoroutine(AttackPet(p));
					yield return new WaitForSeconds(0.5f);
					animation.Play("EnemyIdle01");
				}
			}
			else {
				
				//check for use of flight ability
				if (m_abilityType == AbilityType.Flight && currentCard.distanceToPlayer <= m_chaseDistance+1)
				{
					List<GameManager.Direction> dir = new List<GameManager.Direction>();
					dir.Add(GameManager.Direction.North);
					dir.Add(GameManager.Direction.South);
					dir.Add(GameManager.Direction.East);
					dir.Add(GameManager.Direction.West);
					List<Card> cardsInRange = MapManager.m_mapManager.GetCardsInRange(m_currentCard, 3, dir, true, false, false);
					
					foreach (Card thisCard in cardsInRange)
					{
						if (thisCard.distanceToPlayer < (m_currentCard.distanceToPlayer-1) && !thisCard.isOccupied)
						{
							UpdateTurnIcon ();
							//FollowCamera.m_followCamera.SetTarget(this.gameObject);
							yield return StartCoroutine(GetFocus());
							if (thisCard.animation.isPlaying)
							{
								while (thisCard.animation.isPlaying)
								{
									yield return null;
								}
							}
							yield return new WaitForSeconds(0.5f);
							UIManager.m_uiManager.SpawnAbilityName(m_abilityType.ToString(), this.transform);
							yield return new WaitForSeconds(0.5f);
							
							Card nextCard = thisCard;
							m_moveTimer = 0;
							m_moveTime = animation["EnemyJump01"].length;
							m_moveStart = m_currentCard.transform.position;
							m_moveEnd = nextCard.m_actorBase.position;
							m_currentCard.enemy = null;
							m_currentCard = nextCard;
							m_currentCard.enemy = this;
							this.transform.parent = m_currentCard.cardMesh.transform;
							yield return StartCoroutine(Move());
							yield break;
						}
					}
				}
				
				//check for heal ability
				if (m_abilityType == AbilityType.Heal && currentCard.distanceToPlayer <= m_chaseDistance+1 && m_energy >= 2)
				{

					yield return StartCoroutine (GainEnergy(-2));
					List<GameManager.Direction> directions = new List<GameManager.Direction>();
					directions.Add(GameManager.Direction.North);
					directions.Add(GameManager.Direction.South);
					directions.Add(GameManager.Direction.East);
					directions.Add(GameManager.Direction.West);
					List<Card> validCards = MapManager.m_mapManager.GetCardsInRange(m_currentCard, (m_range + m_currentCard.siteRangeBonus), directions, true, false, false);
					
					//foreach (Card thisCard in validCards)
					while (validCards.Count > 0)
					{
						int rand = Random.Range(0, validCards.Count);
						Card thisCard = validCards[rand];
						validCards.RemoveAt(rand);
						
						if (thisCard.enemy != null && thisCard.cardState != Card.CardState.Hidden)
						{
							UpdateTurnIcon ();
							//FollowCamera.m_followCamera.SetTarget(this.gameObject);
							yield return StartCoroutine(GetFocus());
							//yield return new WaitForSeconds(0.5f);
							UIManager.m_uiManager.SpawnAbilityName(m_abilityType.ToString(), this.transform);
							yield return new WaitForSeconds(0.5f);
							FollowCamera.m_followCamera.SetTarget(thisCard.enemy.gameObject);
							yield return new WaitForSeconds(0.5f);
							yield return StartCoroutine(thisCard.enemy.GainHealth(5));
							UIManager.m_uiManager.SpawnDamageNumber(5, thisCard.enemy.transform);
							yield return new WaitForSeconds(0.5f);
							
							yield break;
						}
					}
					
					yield return StartCoroutine(m_currentCard.ActivateCard());
				}
				
				//check for use of charge ability
				if (m_abilityType == AbilityType.Charge &&  m_currentCard.distanceToPlayer == 2 && m_energy >= 2)
				{

					
					//check that the intervening space is free
					for (int i=0; i < m_currentCard.linkedCards.Length; i++)
					{
						Card lc = m_currentCard.linkedCards[i];
						if (lc != null)
						{
							if (!lc.isOccupied && lc.cardState == Card.CardState.Normal && lc.distanceToPlayer == 1)
							{
								string newString = m_displayName + " uses " + m_abilityType.ToString();
								UIManager.m_uiManager.UpdateActions (newString);

								UpdateTurnIcon ();
								yield return StartCoroutine (GainEnergy(-2));
								//FollowCamera.m_followCamera.SetTarget(this.gameObject);
								yield return StartCoroutine(GetFocus());
								//yield return new WaitForSeconds(0.5f);
								UIManager.m_uiManager.SpawnAbilityName(m_abilityType.ToString(), this.transform);
								yield return new WaitForSeconds(0.5f);
								
								if (i ==0)
								{
									ChangeFacing(GameManager.Direction.North);	
								} else if (i == 1)
								{
									ChangeFacing(GameManager.Direction.South);	
								} else if (i == 2)
								{
									ChangeFacing(GameManager.Direction.East);	
								} else if (i == 3)
								{
									ChangeFacing(GameManager.Direction.West);	
								}
								
								m_moveTimer = 0;
								m_moveTime = animation["EnemyJump01"].length;
								m_moveStart = m_currentCard.transform.position;
								m_moveEnd = lc.m_actorBase.position;
								m_currentCard.enemy = null;
								m_currentCard = lc;
								m_currentCard.enemy = this;
								this.transform.parent = m_currentCard.cardMesh.transform;
								yield return StartCoroutine(Move());
								CheckForAdjacentPlayer();
								yield return new WaitForSeconds(0.5f);
								animation.Play("EnemyJump01");
								yield return StartCoroutine(Attack());
								yield return new WaitForSeconds(0.5f);
								animation.Play("EnemyIdle01");
								yield break;
							}
						}
					}
				}
				
				//check for use of summon ability
				if (m_abilityType == AbilityType.Summon && m_summon != null && m_energy >= 3 && currentCard.distanceToPlayer <= m_chaseDistance)
				{
					foreach (Card lc in m_currentCard.linkedCards)
					{
						if (lc != null)
						{
							if (!lc.isOccupied && lc.cardState == Card.CardState.Normal)
							{
								UpdateTurnIcon ();
								//spawn enemy
								yield return StartCoroutine (GainEnergy(-3));
//								FollowCamera.m_followCamera.SetTarget(this.gameObject);
//								yield return new WaitForSeconds(0.5f);
								yield return StartCoroutine(GetFocus());
								UIManager.m_uiManager.SpawnAbilityName(m_abilityType.ToString(), this.transform);
								yield return new WaitForSeconds(1);
								GameObject thisEnemy = m_summon;

								Vector3 pos = lc.transform.position;
								Vector3 rot = thisEnemy.transform.eulerAngles;
								Enemy newEnemy = (Enemy)((GameObject)Instantiate(thisEnemy, pos, Quaternion.Euler(rot))).transform.GetComponent("Enemy");
								lc.enemy = newEnemy;
								newEnemy.Initialize(Enemy.EnemyState.Idle, lc);
								newEnemy.gameObject.transform.parent = lc.m_cardMesh.transform;
								GameManager.m_gameManager.currentMap.m_enemies.Add(newEnemy);

								string newString = m_displayName + " Summons " + newEnemy.m_displayName;
								UIManager.m_uiManager.UpdateActions (newString);

								yield return new WaitForSeconds(1);
								
								yield return StartCoroutine(m_currentCard.ActivateCard());
								
								yield break;
							}
						}
					}
				}
				//check for player in range if ranged type
				bool playerInRange = false;
				if ((m_attackType == AttackType.Ranged && m_abilityType != AbilityType.Web) || (m_attackType == AttackType.Ranged && m_abilityType == AbilityType.Web && m_energy >=2 && Player.m_player.currentEnergy > 0) 
					|| (m_abilityType == AbilityType.Quicksand && Player.m_player.currentCard.type != Card.CardType.Quicksand && m_energy >=2 ))
				{
					List<GameManager.Direction> directions = new List<GameManager.Direction>();
					directions.Add(GameManager.Direction.North);
					directions.Add(GameManager.Direction.South);
					directions.Add(GameManager.Direction.East);
					directions.Add(GameManager.Direction.West);
					List<Card> validCards = MapManager.m_mapManager.GetCardsInRange(m_currentCard, (m_range + m_currentCard.siteRangeBonus), directions, false, false, false);
					
					foreach (Card thisCard in validCards)
					{
						if (thisCard == Player.m_player.currentCard)
						{
							playerInRange = true;	
						}
					}
					
					if (playerInRange)
					{
						m_timesChased = 0;
						List<Card> range = MapManager.m_mapManager.GetCardsInRange(m_currentCard, (m_range + m_currentCard.siteRangeBonus), directions, false, true, false);
						
						if (m_abilityType == AbilityType.Web && m_energy >= 2)
						{
							UpdateTurnIcon ();
							yield return StartCoroutine (GainEnergy(-2));
							//FollowCamera.m_followCamera.SetTarget(this.gameObject);
							yield return StartCoroutine(GetFocus());
							UIManager.m_uiManager.SpawnAbilityName(m_abilityType.ToString(), this.transform);
							yield return new WaitForSeconds(1);
							AttackEnergy(m_damage);
							
							yield return StartCoroutine(m_currentCard.ActivateCard());
						} else if (m_abilityType == AbilityType.Quicksand && m_energy >= 2)
						{
							UpdateTurnIcon ();
							yield return StartCoroutine (GainEnergy(-2));
//							FollowCamera.m_followCamera.SetTarget(this.gameObject);
//							yield return new WaitForSeconds(0.5f);
							yield return StartCoroutine(GetFocus());
							UIManager.m_uiManager.SpawnAbilityName(m_abilityType.ToString(), this.transform);
							yield return new WaitForSeconds(0.5f);
							FollowCamera.m_followCamera.SetTarget(Player.m_player.gameObject);
							yield return new WaitForSeconds(0.5f);
							Card playerCard = Player.m_player.currentCard;
							yield return StartCoroutine(playerCard.ChangeCardType((Card)(MapManager.m_mapManager.m_cardTypes[8].GetComponent("Card"))));							
							playerCard.type = Card.CardType.Quicksand;
							playerCard.doUpkeep = true;
							
							yield return new WaitForSeconds(0.5f);
							
						} else if (m_abilityType == AbilityType.Voodoo && m_energy >= 2)
						{
							UpdateTurnIcon ();
							yield return StartCoroutine (GainEnergy(-2));

							List<Follower> unstunned = new List<Follower>();
							foreach (Follower f in GameManager.m_gameManager.followers)
							{
								if (f.followerState != Follower.FollowerState.Stunned)
								{
									unstunned.Add(f);
								}
							}
							
							//if there are more than 0 unstunned heroes, choose one at random
							if (unstunned.Count > 0)
							{	
								Follower randFollower = unstunned[Random.Range(0, unstunned.Count)];
								
//								FollowCamera.m_followCamera.SetTarget(this.gameObject);
//								yield return new WaitForSeconds(0.5f);
								yield return StartCoroutine(GetFocus());
								UIManager.m_uiManager.SpawnAbilityName(m_abilityType.ToString(), this.transform);
								yield return new WaitForSeconds(0.5f);
								FollowCamera.m_followCamera.SetTarget(Player.m_player.gameObject);
								yield return StartCoroutine(randFollower.DoStun(3));
								UIManager.m_uiManager.SpawnAbilityName("Stun", Player.m_player.m_playerMesh.transform);
								yield return new WaitForSeconds(0.5f);
								//yield break;
							}

							if (m_energy <= 2)
							{
								m_attackType = AttackType.Melee;
							}

						}
						else
						{
//							FollowCamera.m_followCamera.SetTarget(this.gameObject);
//							yield return new WaitForSeconds(1);
							yield return StartCoroutine(GetFocus());
							yield return StartCoroutine(Attack());
						}
						
						foreach (Card vc in range)
						{
							vc.ChangeHighlightState(false);	
						}
						
						yield return false;
					}
				}
				
//				//check for adjacent followers to attack
//				
//				foreach (Card lc in m_currentCard.linkedCards)
//				{
//					if (lc != null)
//					{
//						if (lc.follower != null && lc.cardState == Card.CardState.Normal)
//						{
//							//attack follower
//							m_timesChased = 0;
//							GameObject fGO = lc.follower;
//							Debug.Log(fGO.name);
//							Follower thisFollower = (Follower)fGO.transform.GetComponent("Follower");
//							FollowCamera.m_followCamera.SetTarget(this.gameObject);
//							yield return new WaitForSeconds(0.5f);
//							animation.Play("EnemyJump01");
//							thisFollower.TakeDamage(m_damage+m_turnDamage);
//							UIManager.m_uiManager.SpawnDamageNumber(m_damage+m_turnDamage, thisFollower.transform);
//							yield return new WaitForSeconds(1.0f);
//							yield break;
//						}
//					}
//				}
				
				//else get path to player and move toward if he's not too far away
				if ((m_currentCard.distanceToPlayer <= m_chaseDistance && !playerInRange && m_timesChased < 5) || m_damagedThisTurn)
				{
					m_damagedThisTurn = false;

					int minDistance = 999;
					Card nextCard = null;
					//foreach (Card linkedCard in m_currentCard.linkedCards)
					for (int i=0; i < m_currentCard.linkedCards.Length; i++)
					{
						Card linkedCard = m_currentCard.linkedCards[i];
						if (linkedCard != null)
						{
							if (linkedCard.distanceToPlayer < minDistance && linkedCard != Player.m_player.currentCard &&
								(linkedCard.cardState == Card.CardState.Hidden || (linkedCard.cardState == Card.CardState.Normal && !linkedCard.isOccupied)))
							{
								if (i ==0)
								{
									ChangeFacing(GameManager.Direction.North);	
								} else if (i == 1)
								{
									ChangeFacing(GameManager.Direction.South);	
								} else if (i == 2)
								{
									ChangeFacing(GameManager.Direction.East);	
								} else if (i == 3)
								{
									ChangeFacing(GameManager.Direction.West);	
								}
								minDistance = linkedCard.distanceToPlayer;
								nextCard = linkedCard;
							}
						}
					}
					
					if (nextCard != null)
					{
						//FollowCamera.m_followCamera.SetTarget(this.gameObject);
						
						if (nextCard.cardState == Card.CardState.Normal)
						{
							if (nextCard.animation.isPlaying)
							{
								while (nextCard.animation.isPlaying)
								{
									yield return null;
								}
							}
							m_timesChased ++;
							
							m_moveTimer = 0;
							m_moveTime = animation["EnemyJump01"].length;
							m_moveStart = m_currentCard.transform.position;
							m_moveEnd = nextCard.m_actorBase.position;
							m_currentCard.enemy = null;
							m_currentCard = nextCard;
							m_currentCard.enemy = this;
							this.transform.parent = m_currentCard.cardMesh.transform;
							StartCoroutine(Move());
							
							//check for use of rush ability
							if (m_abilityType == AbilityType.Rush && m_energy >= 2)
							{
								bool pAdjacent = CheckForAdjacentPlayer();
								if (!pAdjacent)
								{
									m_abilityType = AbilityType.None;
									UIManager.m_uiManager.SpawnAbilityName((AbilityType.Rush).ToString(), m_enemyMesh[0].gameObject.transform);
									yield return new WaitForSeconds(0.5f);
									yield return StartCoroutine(DoTurn());
									m_abilityType = AbilityType.Rush;
									yield return StartCoroutine (GainEnergy(-2));
									StartCoroutine(UIManager.m_uiManager.DisplayTargetEnemy(this));
								}
							}
						} else if (nextCard.cardState == Card.CardState.Hidden)
						{
							animation.Play("EnemyJump01");
							while (animation.IsPlaying("EnemyJump01"))
							{
								yield return null;
							}

							yield return StartCoroutine(nextCard.ChangeCardState(Card.CardState.Normal));

							animation.Play("EnemyIdle01");
						}
					}
				} else if (m_currentCard.distanceToPlayer > m_chaseDistance)
				{
					StartCoroutine(ChangeState(EnemyState.Idle));
				}
//				List<Card> path = MapManager.m_mapManager.GetPathToTarget(m_currentCard, Player.m_player.currentCard);
//				if (path.Count > 0)
//				{
//					Card nextCard = path[Random.Range(0, path.Count)];
//					m_moveTimer = 0;
//					m_moveTime = animation["EnemyJump01"].length;
//					m_moveStart = m_currentCard.transform.position;
//					m_moveEnd = nextCard.transform.position;
//					m_currentCard.enemy = null;
//					m_currentCard = nextCard;
//					m_currentCard.enemy = this;
//					yield return StartCoroutine(Move());
//				}
			}
		} 
//		else if (m_activatedThisTurn)
//		{
//			m_activatedThisTurn = false;
//			//animation.Play("EnemyIdle01");
//		}
		
		if (m_stunDuration > 0)
		{
			m_stunDuration --;

			if (m_stunDuration == 0)
			{
				string newString = m_displayName + " is no longer Stunned";
				UIManager.m_uiManager.UpdateActions (newString);
			}
		}
		
		//if enemy has stopped chasing, reset if player gets within 2 tiles
		if (m_timesChased >= 5 && currentCard.distanceToPlayer <= 2)
		{
			m_timesChased = 0;
		}
		
		
//		if (animation.IsPlaying("EnemyJump01") && Input.GetKeyUp(KeyCode.Space))
//		{
//			animation["EnemyJump01"].time = animation["EnemyJump01"].length;
//			animation.Stop();
//		}
		
//		while (animation.IsPlaying("EnemyJump01"))
//		{	
//			yield return null;
//		}
	}

	public IEnumerator EndTurn ()
	{
		if (m_inFocus) {
			StartCoroutine(UIManager.m_uiManager.TurnOffTargetCard());
			m_inFocus = false;
		}

		yield return null;
	}


	public IEnumerator Move () {

		UpdateTurnIcon ();
		//yield return new WaitForSeconds(0.4f);
		AssetManager.m_assetManager.PlaySFX(AssetManager.SFXType.PlayerMove);
		animation.Stop ();
		animation.Play("EnemyJump01");
		while (animation.IsPlaying("EnemyJump01"))
		{
			//Debug.Log("moving");
			float t = Mathf.Clamp(m_moveTimer / m_moveTime, 0, 1);
			Vector3 newPos = Vector3.Lerp(m_moveStart, m_moveEnd, t);
			m_moveTimer += Time.deltaTime;
			
			transform.position = newPos;
			
			yield return null;
		}

		transform.position = m_moveEnd;
		
		if (m_abilityType == AbilityType.Firewalker && m_currentCard.type != Card.CardType.Fire && m_currentCard.type != Card.CardType.Exit)
		{
			yield return StartCoroutine(m_currentCard.ChangeCardType((Card)(MapManager.m_mapManager.m_cardTypes[14].GetComponent("Card"))));	
			m_currentCard.type = Card.CardType.Fire;
			m_currentCard.doUpkeep = true;
		}

		//yield return StartCoroutine (Player.m_player.FinishMove ());

		UpdateStatBar ();

		animation.Play("EnemyIdle01");

		yield return null;
		
	}
	
	private IEnumerator Attack ()
	{
		string newString = m_displayName + " attacks " + GameManager.m_gameManager.currentFollower.m_nameText + "!";
		UIManager.m_uiManager.UpdateActions (newString);

		UpdateTurnIcon ();

		int mobDamage = 0;
		if (m_abilityType == AbilityType.Mob)
		{
			for (int i=0; i < 4; i++)
			{
				if (m_currentCard.linkedCards[i] != null)
				{
					Card linkedCard = m_currentCard.linkedCards[i];

					if (linkedCard.enemy != null && linkedCard.cardState == Card.CardState.Normal)
					{
						if (linkedCard.enemy.m_abilityType == AbilityType.Mob)
						{
							mobDamage += 1;
						}
					}
					
				}
			}
		}
		if (m_abilityType == AbilityType.Firestrength)
		{
			for (int i=0; i < 4; i++)
			{
				if (m_currentCard.linkedCards[i] != null)
				{
					Card linkedCard = m_currentCard.linkedCards[i];

					if (linkedCard.type == Card.CardType.Fire && linkedCard.cardState == Card.CardState.Normal)
					{
						mobDamage ++;
					}
					
				}
			}
		}
		
		if (m_abilityType == AbilityType.Wall)
		{
			mobDamage += 5;	
		}

		if (m_abilityType == AbilityType.Poison && m_energy >= 2)
		{
			yield return StartCoroutine (GainEnergy(-2));
			//UIManager.m_uiManager.
			UIManager.m_uiManager.SpawnAbilityName((GameManager.StatusEffect.Poison).ToString(), Player.m_player.transform);
			Player.m_player.ChangeEffectState(GameManager.StatusEffect.Poison, m_poisonDuration);	
		} else {
			yield return StartCoroutine(Player.m_player.TakeDamage(m_damage + m_currentCard.siteDamageBonus + m_turnDamage + mobDamage + m_graveDamageBonus, this));
		}
		
		if (m_currentCard.type == Card.CardType.RazorGlade)
		{
			yield return StartCoroutine(m_currentCard.ActivateCard());
		}



		yield return null;
	}

	private IEnumerator AttackPet (Pet target)
	{
		UpdateTurnIcon ();
		int mobDamage = 0;
		if (m_abilityType == AbilityType.Mob)
		{
			for (int i=0; i < 4; i++)
			{
				if (m_currentCard.linkedCards[i] != null)
				{
					Card linkedCard = m_currentCard.linkedCards[i];
					
					if (linkedCard.enemy != null && linkedCard.cardState == Card.CardState.Normal)
					{
						if (linkedCard.enemy.m_abilityType == AbilityType.Mob)
						{
							mobDamage += 1;
						}
					}
					
				}
			}
		}
		if (m_abilityType == AbilityType.Firestrength)
		{
			for (int i=0; i < 4; i++)
			{
				if (m_currentCard.linkedCards[i] != null)
				{
					Card linkedCard = m_currentCard.linkedCards[i];
					
					if (linkedCard.type == Card.CardType.Fire && linkedCard.cardState == Card.CardState.Normal)
					{
						mobDamage ++;
					}
					
				}
			}
		}
		
		if (m_abilityType == AbilityType.Wall)
		{
			mobDamage += 5;	
		}
		
//		if (m_abilityType == AbilityType.Poison && m_energy >= 2)
//		{
//			yield return StartCoroutine (GainEnergy(-2));
//			//UIManager.m_uiManager.
//			UIManager.m_uiManager.SpawnAbilityName((GameManager.StatusEffect.Poison).ToString(), Player.m_player.transform);
//			Player.m_player.ChangeEffectState(GameManager.StatusEffect.Poison, m_poisonDuration);	
//		} else {
		yield return StartCoroutine (target.TakeDamage (m_damage + m_currentCard.siteDamageBonus + m_turnDamage + mobDamage));
			//yield return StartCoroutine(Player.m_player.TakeDamage(m_damage + m_currentCard.siteDamageBonus + m_turnDamage + mobDamage, this));
//		}
		
		if (m_currentCard.type == Card.CardType.RazorGlade)
		{
			yield return StartCoroutine(m_currentCard.ActivateCard());
		}
		
		yield return null;
	}
	
	private void AttackEnergy (int energyDamage)
	{
		int energy = Player.m_player.currentEnergy;
		if (energyDamage > energy)
		{
			energyDamage -= energyDamage-energy;	
		}
		energy = Mathf.Clamp(energy - energyDamage, 0, 99);
		Player.m_player.currentEnergy = energy;
		UIManager.m_uiManager.SpawnDamageNumber(energyDamage, Player.m_player.transform);
		UIManager.m_uiManager.UpdateEnergy(Player.m_player.currentEnergy);
	}
	
	public void ChangeFacing (GameManager.Direction dir)
	{
		m_facing = dir;
		Vector3 newDirection = 	m_enemyMesh[0].transform.eulerAngles;
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
		
		m_enemyMesh[0].transform.eulerAngles = newDirection;
	}
	
	public void ChangeEffectState (GameManager.StatusEffect newEffect, int duration)
	{
		m_currentEffect = newEffect;	
		m_effectDuration = duration;
		
//		if (newEffect == GameManager.StatusEffect.Poison)
//		{
//			
//		}
	}
	
	public IEnumerator ChangeState (EnemyState newState)
	{
		Debug.Log ("CHANGING ENEMY STATE: " + newState.ToString ());
		EnemyState oldState = m_enemyState;
		m_enemyState = newState;

		if (newState == EnemyState.Active)
		{
			if (m_initiative == 0)
			{
				m_initiative = GameManager.m_gameManager.initiative;
				StartCoroutine( UIManager.m_uiManager.UpdateStack());
			}
			if (oldState == EnemyState.Idle || oldState == EnemyState.Inactive)
			{
				StartCoroutine( UIManager.m_uiManager.UpdateStack());
			}

			animation.Play("EnemyIdle01");
		}
		else if (newState == EnemyState.Inactive)
		{
			animation.Stop();

			// turn off collider
			BoxCollider bc = (BoxCollider) transform.GetComponent("BoxCollider");
			bc.enabled = false;
			
			//turn off visuals
			foreach (MeshRenderer thisMesh in m_enemyMesh)
			{
				thisMesh.renderer.enabled = false;
			}

			if (m_statBar != null)
			{
				m_statBar.gameObject.SetActive(false);
			}
		} else if (newState == EnemyState.Idle && oldState == EnemyState.Inactive)
		{

			if (m_initiative == 0)
			{
				m_initiative = GameManager.m_gameManager.initiative;
				StartCoroutine( UIManager.m_uiManager.UpdateStack());
			}

			foreach (MeshRenderer thisMesh in m_enemyMesh)
			{
				thisMesh.renderer.enabled = true;
			}
			m_activatedThisTurn = true;

			if (m_statBar != null)
			{
				m_statBar.gameObject.SetActive(true);
			} else {
				//create new stat bar
				GameObject go = (GameObject)(Instantiate(AssetManager.m_assetManager.m_UIelements[0], Vector3.zero, AssetManager.m_assetManager.m_UIelements[0].transform.rotation ));
				go.transform.parent = UIManager.m_uiManager.m_HUD.transform;
				go.transform.localScale = Vector3.one;
				GUIFollow f = (GUIFollow)go.GetComponent("GUIFollow");
				f.SetTarget(this.gameObject);
				m_statBar = (UICard)go.GetComponent("UICard");
				UpdateStatBar();
				ToggleStatBar(m_doStatBar);
			}

			if (currentCard.distanceToPlayer <= m_chaseDistance)
			{
				yield return StartCoroutine( PlayAlert());
			}
		} else if (newState == EnemyState.Dead)
		{
			animation.Stop();

			if (m_enemyType != EnemyType.Pet)
			{
				List<Enemy> enemies = GameManager.m_gameManager.enemies;
				for (int i=0; i < enemies.Count; i++)
				{
					Enemy thisEnemy = enemies[i];
					if (thisEnemy == this)
					{
						enemies.RemoveAt(i);
						thisEnemy.gameObject.transform.parent = null;
						break;
					}
				}
				GameManager.m_gameManager.enemies = enemies;
			} else {
				List<Pet> pets = GameManager.m_gameManager.currentMap.m_pets;
				for (int i=0; i < pets.Count; i++)
				{
					Pet thisPet = pets[i];
					if (thisPet == this)
					{
						pets.RemoveAt(i);
						break;
					}
				}
				GameManager.m_gameManager.currentMap.m_pets = pets;
			}

			m_currentCard.enemy = null;



			BoxCollider bc = (BoxCollider)transform.GetComponent("BoxCollider");
			if (bc != null)
			{
				bc.enabled = false;	
			}
			
			GameManager.m_gameManager.deadEnemies.Add(this);
			GameManager.m_gameManager.SendToGrave(this);
			
			//spawn chest if needed
			if (m_doDropChest)
			{
				Vector3 cardPos = m_currentCard.transform.position;
				Vector3 cardRot = m_currentCard.transform.eulerAngles;
				
				Chest newChest = (Chest)((GameObject)Instantiate(MapManager.m_mapManager.m_chest, cardPos, Quaternion.Euler(cardRot))).transform.GetComponent("Chest");
				newChest.currentCard = m_currentCard;
				newChest.InitializeBossChest(m_lootLevel);
//				newChest.m_chestMesh.renderer.enabled = false;
				m_currentCard.chest = newChest;
				newChest.gameObject.transform.parent = m_currentCard.cardMesh.transform;
				GameManager.m_gameManager.currentMap.m_chests.Add(newChest);	
			}

			if (m_enemyType == EnemyType.Boss)
			{
				Card c = m_currentCard;
				if (c.type != Card.CardType.Exit)
				{
					c.type = Card.CardType.Exit;
					c.m_cardMesh.material = ((Card)MapManager.m_mapManager.m_cardTypes[2].transform.GetComponent("Card")).cardMesh.sharedMaterial;
//					UIManager.m_uiManager.m_exitButton.SetActive(true);
				}
			}
		}

		yield return null;
	}	

	public void ToggleStatBar (bool doStatBar)
	{
		m_doStatBar = doStatBar;
		if (doStatBar)
		{
			if (m_statBar != null)
			{
				m_statBar.gameObject.SetActive(true);
			}
		} else {
			if (m_statBar != null)
			{
				m_statBar.gameObject.SetActive(false);
			}
		}
	}

	private void UpdateStatBar ()
	{

		if (m_statBar != null)
		{
			m_statBar.m_miscText[0].text = damage.ToString();
			m_statBar.m_miscText[1].text = m_currentHealth.ToString();
			m_statBar.m_miscText[2].text = m_energy.ToString();
			if (m_energy > 0)
			{

				if (armor < 1)
				{
					//resize the stat bar bg
					Vector3 size = m_statBar.m_miscOBJ[4].transform.localScale;
					size.x = 0.762f;
					m_statBar.m_miscOBJ[4].transform.localScale = size;
				}
			} else {
				m_statBar.m_miscText[2].gameObject.SetActive(false);
				m_statBar.m_miscOBJ[2].gameObject.SetActive(false);
			}

			if (armor > 0)
			{
				if (m_energy < 1)
				{
					//move armor UI over
					m_statBar.m_miscText[3].transform.position = m_statBar.m_miscText[2].transform.position;
					m_statBar.m_miscOBJ[3].transform.position = m_statBar.m_miscOBJ[2].transform.position;
					//resize the stat bar bg
					Vector3 size = m_statBar.m_miscOBJ[4].transform.localScale;
					size.x = 0.762f;
					m_statBar.m_miscOBJ[4].transform.localScale = size;

				}
				m_statBar.m_miscText[3].text = armor.ToString();
				m_statBar.m_miscText[3].gameObject.SetActive(true);
				m_statBar.m_miscOBJ[3].gameObject.SetActive(true);
			} else {
				m_statBar.m_miscText[3].gameObject.SetActive(false);
				m_statBar.m_miscOBJ[3].gameObject.SetActive(false);

				if (armor < 1 && m_energy < 1)
				{
					//resize the stat bar bg
					Vector3 size = m_statBar.m_miscOBJ[4].transform.localScale;
					size.x = 0.537f;
					m_statBar.m_miscOBJ[4].transform.localScale = size;
				}
			}
		}
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
	
	public IEnumerator DoKnockback (GameManager.Direction direction)
	{
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
		
		animation.Stop();
		//animation["PlayerJump01"].speed = 0.5f;
		
		m_moveTimer = 0;
		m_moveTime = animation["EnemyJump01"].length;
		m_moveStart = m_currentCard.transform.position;
		m_moveEnd = nextCard.m_actorBase.position;
		
		//Debug.Log(m_currentCard.row.ToString() + "," + m_currentCard.column.ToString() + " / " + nextCard.row.ToString() + "," + nextCard.column.ToString());
		
		animation.Play("EnemyJump01");
		
		while (animation.IsPlaying("EnemyJump01"))
		{
			m_moveTimer = Mathf.Clamp(m_moveTimer + Time.deltaTime, 0, m_moveTime);
			float t = Mathf.Clamp(m_moveTimer / m_moveTime, 0, 1);
			//animation["EnemyJump01"].time = m_moveTime * t;
			Vector3 newPos = Vector3.Lerp(m_moveStart, m_moveEnd, t);
			
			if (m_moveTimer == m_moveTime)
			{
				m_currentCard.enemy = null;
				m_currentCard = nextCard;
				m_currentCard.enemy = this;
				
				newPos = m_moveEnd;
				animation.Stop();
			}
			transform.position = newPos;
			yield return null;	
		}

		transform.position = m_moveEnd;

		animation.Play("EnemyIdle01");
		
		//animation["PlayerJump01"].speed = 0;
		yield return null;	
	}
	
	private bool CheckForAdjacentPlayer ()
	{
		
		for (int i=0; i < m_currentCard.linkedCards.Length; i++)
		{
			Card lc = m_currentCard.linkedCards[i];
			if (lc != null)
			{
				if (lc == Player.m_player.currentCard)
				{
					if (i ==0)
					{
						ChangeFacing(GameManager.Direction.North);	
					} else if (i == 1)
					{
						ChangeFacing(GameManager.Direction.South);	
					} else if (i == 2)
					{
						ChangeFacing(GameManager.Direction.East);	
					} else if (i == 3)
					{
						ChangeFacing(GameManager.Direction.West);	
					}
					return true;	
				}
			}
		}
		return false;
	}

	private bool CheckForAdjacentPlayerNoFacing ()
	{
		
		for (int i=0; i < m_currentCard.linkedCards.Length; i++)
		{
			Card lc = m_currentCard.linkedCards[i];
			if (lc != null)
			{
				if (lc == Player.m_player.currentCard)
				{
					return true;	
				}
			}
		}
		return false;
	}
	

	private bool CheckForAdjacentPet ()
	{
		
		for (int i=0; i < m_currentCard.linkedCards.Length; i++)
		{
			Card lc = m_currentCard.linkedCards[i];
			if (lc != null)
			{
				if (lc.enemy != null)
				{
					if (lc.enemy.m_enemyType == EnemyType.Pet)
					{
						return true;	
					}
				}
			}
		}
		return false;
	}
	
	public IEnumerator GainHealth (int amount)
	{
		m_currentHealth += amount;

		//updated target card if necessary
		if (UIManager.m_uiManager.targetCard == this.gameObject)
		{
			StartCoroutine(UIManager.m_uiManager.DisplayTargetEnemy(this));
		}

		UpdateStatBar ();

		yield return null;
	}
	
	public IEnumerator GainEnergy (int amount)
	{
		m_energy = Mathf.Clamp(m_energy + amount, 0, 99);

		//updated target card if necessary
		if (UIManager.m_uiManager.targetCard == this.gameObject)
		{
			StartCoroutine(UIManager.m_uiManager.DisplayTargetEnemy(this));
		}

		UpdateStatBar ();

		yield return null;
	}
	
	public IEnumerator TakeDirectDamage (int damage)
	{
		m_damagedThisTurn = true;

		m_currentHealth = Mathf.Clamp (m_currentHealth - damage, 0, m_maxHealth);
		UpdateStatBar ();
		UIManager.m_uiManager.SpawnFloatingText ("-" + damage.ToString (), UIManager.Icon.Health, this.transform);
		if (m_currentHealth <= 0)
		{
			AssetManager.m_assetManager.PlaySFX(AssetManager.SFXType.EnemyDefeated);
			
			//check for active skills based on enemy death
			
			if (m_abilityType == AbilityType.Explode)
			{
				yield return new WaitForSeconds(0.5f);
				UIManager.m_uiManager.SpawnAbilityName("Explode", this.m_enemyMesh[0].transform);
				
				Card[] lc = m_currentCard.linkedCards;
				foreach (Card c in lc)
				{
					if (c != null)
					{
						if (c.enemy != null)
						{
							yield return new WaitForSeconds(0.5f);
							yield return StartCoroutine(c.enemy.TakeDamage(2));
						} else if (c.player != null)
						{
							yield return new WaitForSeconds(0.5f);
							yield return StartCoroutine(c.player.TakeDamage(2));
						}
					}
				}
			}
			
			if (m_abilityType == AbilityType.Celerity)
			{
				yield return new WaitForSeconds(0.5f);
				UIManager.m_uiManager.SpawnAbilityName("Celerity", this.m_enemyMesh[0].transform);
				yield return new WaitForSeconds(0.5f);
				UIManager.m_uiManager.SpawnAbilityName("+2A", Player.m_player.m_playerMesh.transform);
				Player.m_player.GainActionPoints(2);
				yield return new WaitForSeconds(0.5f);
			}

			if (m_abilityType == AbilityType.EnergySyphon && Player.m_player.currentEnergy > 0)
			{
				string newString = m_displayName + " uses Energy Syphon";
				UIManager.m_uiManager.UpdateActions (newString);

				yield return new WaitForSeconds(0.5f);
				UIManager.m_uiManager.SpawnAbilityName("Energy Syphon", this.transform);
				yield return new WaitForSeconds(0.5f);
				Player.m_player.GainEnergy(-2);
				UIManager.m_uiManager.SpawnFloatingText ("-2", UIManager.Icon.Energy, Player.m_player.m_playerMesh.transform);
				yield return new WaitForSeconds(0.5f);
			}
			
			if (m_abilityType == AbilityType.Regen && Player.m_player.currentHealth < Player.m_player.maxHealth)
			{
				yield return new WaitForSeconds(0.5f);
				UIManager.m_uiManager.SpawnAbilityName("Regen", this.transform);
				yield return new WaitForSeconds(1.0f);
				Player.m_player.GainHealth(1);
				UIManager.m_uiManager.SpawnFloatingText ("+1", UIManager.Icon.Health, Player.m_player.m_playerMesh.transform);
				yield return new WaitForSeconds(1.0f);
			}

			yield return StartCoroutine(SendCardToGrave());
			yield return StartCoroutine(ChangeState(EnemyState.Dead));
		}
		yield return true;
	}


	public IEnumerator SendCardToGrave ()
	{
		Debug.Log ("PLAYING CARD TO GRAVE ANIM");

		foreach (MeshRenderer thisMesh in m_enemyMesh)
		{
			thisMesh.renderer.enabled = false;
		}
		
		if (m_statBar != null)
		{
			Destroy(m_statBar.gameObject);
		}
		
		Instantiate(AssetManager.m_assetManager.m_particleFX[0], this.transform.position, Quaternion.identity);
		yield return new WaitForSeconds (0.15f);

		Vector3 screenPos = FollowCamera.m_followCamera.m_camera.WorldToScreenPoint(this.transform.position);
		float screenHeight = Screen.height;
		float screenWidth = Screen.width;
		screenPos.x -= (screenWidth / 2.0f);
		screenPos.y -= (screenHeight / 2.0f);

		GameObject GO = (GameObject)UIManager.m_uiManager.m_cardSmall;
		GameObject cardGO = (GameObject)Instantiate (GO, Vector3.zero, GO.transform.rotation);
		UICard card = (UICard)cardGO.GetComponent ("UICard");
		card.transform.parent = UIManager.m_uiManager.m_HUD.transform;
		cardGO.transform.localPosition = screenPos;

		card.m_rankUI.text = "Level " + m_level.ToString() + " " + m_enemyType.ToString();
		card.m_nameUI.text = m_displayName;
		card.m_abilityUI.text =  m_abilityText;
		card.m_portrait.spriteName =  m_portraitSpriteName;
		card.m_portrait.gameObject.SetActive (true);
		card.m_nameUI.gameObject.SetActive (true);
		card.m_abilityUI.gameObject.SetActive (true);
		card.m_rankUI.gameObject.SetActive (true);
		card.m_healthIcon.gameObject.SetActive (true);

		float t = 0;
		float time = 0.25f;
		Vector3 startScale = Vector3.one * 0.15f;
		Vector3 endScale = card.transform.localScale;
		while (t < time)
		{
			t += Time.deltaTime;;
			Vector3 newScale = Vector3.Lerp(startScale, endScale, t / time);
			card.transform.localScale = newScale;
			yield return null;
		}




		yield return new WaitForSeconds (0.5f);

		t = 0;
		time = 0.5f;
		Vector3 startPos = card.transform.position;
		startScale = card.transform.localScale;
		Vector3 endPos = UIManager.m_uiManager.m_backpackButton.transform.position;
		endScale = Vector3.one * 0.15f;
		while (t < time)
		{
			t += Time.deltaTime;;
			Vector3 nPos = Vector3.Lerp(startPos, endPos , t / time);
			Vector3 newScale = Vector3.Lerp(startScale, endScale, t / time);
			card.transform.position = nPos;
			card.transform.localScale = newScale;
			yield return null;
		}
		//card.transform.position = endPos;
		//card.transform.localScale = endScale;

		Destroy (card.gameObject);

		//yield return new WaitForSeconds (0.5f);
		yield return true;
	}
	
	public IEnumerator TakeDamage (int damage)
	{
		Debug.Log ("ENEMY TAKING DAMAGE");
		m_damagedThisTurn = true;

		int effectiveArmor = m_currentCard.siteArmorBonus + m_armor + m_turnArmor;
		if ((Player.m_player.doArmorPierce || Player.m_player.doPermArmorPierce) && GameManager.m_gameManager.currentTurn == GameManager.Turn.Player)
		{
			effectiveArmor = 0;	
		}
		if (m_currentCard.type == Card.CardType.BrokenGround)
		{
			effectiveArmor = 0;	
		}
		
		damage = Mathf.Clamp(damage - effectiveArmor, 0, 100);

		string newString = m_displayName + " takes " + damage.ToString () + " damage!";
		if (effectiveArmor > 0) {
			newString += " (" + effectiveArmor.ToString() + " Armor)";
		}
		UIManager.m_uiManager.UpdateActions (newString);

		m_timesChased = 0;
		int temp = m_currentHealth;
		m_currentHealth = Mathf.Clamp(m_currentHealth - damage, 0, 100);
		UpdateStatBar ();
		UIManager.m_uiManager.SpawnDamageNumber(damage, m_currentCard.transform);
		if (m_currentHealth <= 0)
		{
			string newString2 = m_displayName + " enters The Grave";
			UIManager.m_uiManager.UpdateActions (newString2);

			AssetManager.m_assetManager.PlaySFX(AssetManager.SFXType.EnemyDefeated);
			//GameManager.m_gameManager.currentChain ++;

//			if (Player.m_player.currentActionPoints > 1)
//			{
				//UIManager.m_uiManager.UpdateChainGUI(GameManager.m_gameManager.currentChain);
//			}
			
			//check for Samurai ability
//			if (temp == m_maxHealth && GameManager.m_gameManager.HasFollower(Follower.FollowerType.Samurai))
//			{
//				foreach (Follower thisFollower in GameManager.m_gameManager.followers)
//				{
//					if (thisFollower.m_followerType == Follower.FollowerType.Samurai)
//					{
//						StartCoroutine(GameManager.m_gameManager.ActivateFollower(thisFollower));	
//					}
//				}
//			}

			//check for active skills based on enemy death

			if (m_abilityType == AbilityType.Explode)
			{
				yield return new WaitForSeconds(0.5f);
				UIManager.m_uiManager.SpawnAbilityName("Explode", this.m_enemyMesh[0].transform);

				Card[] lc = m_currentCard.linkedCards;
				foreach (Card c in lc)
				{
					if (c != null)
					{
						if (c.enemy != null)
						{
							yield return new WaitForSeconds(0.5f);
							yield return StartCoroutine(c.enemy.TakeDamage(2));
						} else if (c.player != null)
						{
							yield return new WaitForSeconds(0.5f);
							yield return StartCoroutine(c.player.TakeDamage(2));
						}
					}
				}
			}

			if (m_abilityType == AbilityType.Celerity)
			{
				yield return new WaitForSeconds(0.5f);
				UIManager.m_uiManager.SpawnAbilityName("Celerity", this.m_enemyMesh[0].transform);
				yield return new WaitForSeconds(0.5f);
				//UIManager.m_uiManager.SpawnAbilityName("+ 2A", Player.m_player.m_playerMesh.transform);
				UIManager.m_uiManager.SpawnFloatingText("+2",UIManager.Icon.Actions, Player.m_player.m_playerMesh.transform);
				Player.m_player.GainActionPoints(2);
				yield return new WaitForSeconds(0.5f);
			}

			if (Player.m_player.lifeTap > 0 && Player.m_player.currentHealth < Player.m_player.maxHealth)
			{
				yield return new WaitForSeconds(0.5f);
				UIManager.m_uiManager.SpawnAbilityName("Life Tap", Player.m_player.m_playerMesh.transform);
				yield return new WaitForSeconds(0.5f);
				//UIManager.m_uiManager.SpawnAbilityName("+" + Player.m_player.lifeTap.ToString() + "H", Player.m_player.m_playerMesh.transform);
				UIManager.m_uiManager.SpawnFloatingText("+" + Player.m_player.lifeTap.ToString(),UIManager.Icon.Health, Player.m_player.m_playerMesh.transform);
				Player.m_player.GainHealth(Player.m_player.lifeTap);
				yield return new WaitForSeconds(0.5f);
			}
			
			if (Player.m_player.soulTap > 0 && Player.m_player.currentEnergy < Player.m_player.maxEnergy)
			{
				yield return new WaitForSeconds(0.5f);
				UIManager.m_uiManager.SpawnAbilityName("Soul Tap", Player.m_player.m_playerMesh.transform);
				yield return new WaitForSeconds(0.5f);
				//UIManager.m_uiManager.SpawnAbilityName("+" + Player.m_player.soulTap.ToString() + "E", Player.m_player.m_playerMesh.transform);
				UIManager.m_uiManager.SpawnFloatingText("+" + Player.m_player.soulTap.ToString(),UIManager.Icon.Energy, Player.m_player.m_playerMesh.transform);
				Player.m_player.GainEnergy(Player.m_player.soulTap);
				yield return new WaitForSeconds(0.5f);
			}
			
			if (Player.m_player.doDancer)
			{
				Player.m_player.turnDamage +=2;
				StartCoroutine(UIManager.m_uiManager.UpdateDamage(Player.m_player.damage + Player.m_player.turnDamage + Player.m_player.tempDamage + Player.m_player.permDamage + Player.m_player.currentCard.siteDamageBonus));
			}

			if (m_abilityType == AbilityType.EnergySyphon && Player.m_player.currentEnergy > 0)
			{
				yield return new WaitForSeconds(0.5f);
				UIManager.m_uiManager.SpawnAbilityName("Energy Syphon", this.transform);
				yield return new WaitForSeconds(0.5f);
				Player.m_player.GainEnergy(-2);
				UIManager.m_uiManager.SpawnFloatingText ("-2", UIManager.Icon.Energy, Player.m_player.m_playerMesh.transform);
				yield return new WaitForSeconds(0.5f);
			}

			if (m_abilityType == AbilityType.Regen && Player.m_player.currentHealth < Player.m_player.maxHealth)
			{
				yield return new WaitForSeconds(0.5f);
				UIManager.m_uiManager.SpawnAbilityName("Regen", this.transform);
				yield return new WaitForSeconds(1.0f);
				Player.m_player.GainHealth(1);
				UIManager.m_uiManager.SpawnFloatingText ("+1", UIManager.Icon.Health, Player.m_player.m_playerMesh.transform);
				yield return new WaitForSeconds(1.0f);
			}
			
//			//check for Fencer ability
//			if (GameManager.m_gameManager.HasFollower(Follower.FollowerType.Fencer))
//			{
//				Player.m_player.GainEnergy(1);
//			}
			yield return StartCoroutine(SendCardToGrave());
			yield return StartCoroutine(ChangeState(EnemyState.Dead));
			yield return null;
		}
		else if (m_currentHealth > 0 && (m_abilityType == AbilityType.Counterattack || m_abilityType == AbilityType.Wall))
		{
			yield return new WaitForSeconds(0.5f);
			Debug.Log("COUNTER ATTACK");
			UIManager.m_uiManager.SpawnAbilityName((AbilityType.Counterattack).ToString(), this.transform);
			yield return new WaitForSeconds(1.0f);
			yield return StartCoroutine(Attack());
			animation.Play("EnemyJump01");
			yield return new WaitForSeconds(1.0f);
			animation.Play("EnemyIdle01");
		}

		//updated target card if necessary
		if (UIManager.m_uiManager.targetCard == this.gameObject)
		{
			StartCoroutine(UIManager.m_uiManager.DisplayTargetEnemy(this));
		}


		AssetManager.m_assetManager.PlaySFX(AssetManager.SFXType.EnemyHit);
		yield return null;
	}

	private void UpdateTurnIcon ()
	{
//		if (m_enemyState != EnemyState.Active)
//		{
//			StartCoroutine(ChangeState(EnemyState.Active));
//		}
//
//		if (CurrentTurnIcon.m_currentTurnIcon.turnIconState != CurrentTurnIcon.TurnIconState.Enemy && m_enemyType != EnemyType.Pet)
//		{
//			CurrentTurnIcon.m_currentTurnIcon.ChangeTurn(GameManager.Turn.Enemy);
//		}
	}

	private IEnumerator PlayAlert ()
	{
		Vector3 pos = this.transform.position;
		GameObject go = AssetManager.m_assetManager.m_props [31];
		Instantiate (go, pos, go.transform.rotation);
		yield return new WaitForSeconds (0.5f);

		yield return null;
	}
	
	public EnemyState enemyState
	{
		get
		{
			return m_enemyState;	
		}
	}
	public Card currentCard
	{
		get
		{
			return m_currentCard;
		}
	}
	public int currentHealth
	{
		get
		{
			return m_currentHealth;	
		}
	}
	public int damage
	{
		get
		{
			return m_damage + m_turnDamage + m_graveDamageBonus + m_currentCard.siteDamageBonus;	
		}
	}
	public int energy {get{return m_energy;}}
	public int armor {get{return m_armor + m_turnArmor + m_currentCard.siteArmorBonus;}}
	public int turnArmor {get{return m_turnArmor;} set{m_turnArmor = value;}}
	public int turnDamage {get{return m_turnDamage;} set{m_turnDamage = value;}}
	public GameManager.StatusEffect currentEffect {get{return m_currentEffect;}}
	public int effectDuration {get{return m_effectDuration;}}
	public int stunDuration {get{return m_stunDuration;} set{m_stunDuration = value;}}
	public int graveDamageBonus {get{return m_graveDamageBonus;} set{m_graveDamageBonus = value; if (m_statBar != null){ UpdateStatBar();}}}
	public int initiative {get{return m_initiative;}}
}
