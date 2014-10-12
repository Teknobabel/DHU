using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {
	
	public enum CardState
	{
		Hidden,
		Normal,
	}
	
	public enum HighlightState
	{
		NotHighlighted,
		Highlighted
	}
	
	public enum CardType
	{
		Normal,
		Entrance,
		Exit,
		DungeonEntrance,
		Trap_Flipper,
		Portal,
		Gate,
		Trap_Razorvine,
		Warren,
		Quicksand,
		Fort,
		Tower,
		HighGround,
		Fire,
		Stalactite,
		Spore,
		Spikes,
		Unholy,
		AshCloud,
		Magma,
		Mine,
		Darkness,
		ClawingHands,
		Whispers,
		FrostSnap,
		RazorGlade,
		ManaBurn,
		BrokenGround,
	}
	
	public enum SubType
	{
		None,
		Web,
		Cemetary,
		Mushroom,
		Lava,
		Bugs,
		Hive,
		Catacomb,
		Broken,
		Ash,
		Ash02,
		Snow01,
		
	}
	
	public enum SubBiomeType
	{
		None,
		Beehive,
		Mushrooms,
		Bugs,
		Spiderwebs,
		Stalactites,
		Spikes,
		Catacomb,
		Warren,
		AshCloud,
		MagmaFlow,
		Inferno,
		Minefield,
		Shadows,
		Hands,
		Whispers,
		Frost,
		RazorGlade,
		ManaBurn,
		CrystalField,
		Cliffs,
	}
	
	public MeshRenderer
		m_cardMesh,
		m_highlightMesh,
		m_flipTrapMesh,
		m_fx01;
	
	public string
		m_displayName = "Site",
		m_abilityText = "Ability Text",
		m_portraitSpriteName = "Card_Tower01";
	
	public GameObject
		m_spawn;

	public Transform
		m_actorBase;

	public GameObject[]
		m_meshes;
	
	public float
		m_speedModifier = 1.0f;
	
	private CardType
		m_type = CardType.Normal;
	
	private SubType
		m_subType = SubType.None;

	private Card
		m_northCard,
		m_southCard,
		m_eastCard,
		m_westCard,
		m_destination; //which card on new map to take player to
	
	private Card[]
		m_linkedCards;
	
	private Enemy
		m_enemy;
	
	private Chest
		m_chest;
	
	private Player
		m_player;

	private Shop
		m_shop;
	
	private GameObject
		m_goal,
		m_follower;
		
	private int
		m_id,
		m_mapID = -1, //id of the map linked to exit
		m_row,
		m_column,
		m_numLinked = 0,
		m_distanceToPlayer = 99,
		m_turnTicker = 0,
		m_siteArmorBonus = 0,
		m_siteDamageBonus = 0,
		m_siteRangeBonus = 0,
		m_energyValue = 1;
	
	private bool
		m_isOccupied = false,
		m_doUpkeep = false,
		m_doFlipTrap = false,
		m_doAmbush = false;
	
	private CardState
		m_cardState = CardState.Hidden;
	
	private HighlightState
		m_highlightState = HighlightState.NotHighlighted;

	private Color
		m_highlightColor = Color.white;

	void Awake ()
	{
		neutralColor = m_highlightMesh.material.color;
		Color frameColor = m_flipTrapMesh.material.color;
		frameColor *= Random.Range (0.6f, 1.0f);
		m_flipTrapMesh.material.color = frameColor;
	}
	public void Initialize (int id, int col, int row, CardState state, CardType type) {
		animation["CardFlip02"].speed = m_speedModifier;
		m_id = id;
		m_row = row;
		m_column = col;
		m_cardState = state;
		m_type = type;
		
		if (m_cardState == CardState.Normal)
		{
			Vector3 newRot = m_cardMesh.transform.parent.eulerAngles;
			newRot.z = 180;
			m_cardMesh.transform.parent.eulerAngles = newRot;
			
			if (m_doFlipTrap)
			{
				//m_flipTrapMesh.enabled = true;
				m_highlightMesh.material.color = Color.red;
				StartCoroutine(RedFade());
			}
		}
		
		//play intro animation
//		float speedMod = Random.Range(0.6f, 1.0f);
//		float newSpeed = animation["CardDrop01"].speed;
//		newSpeed *= speedMod;
//		animation["CardDrop01"].speed = newSpeed;
//		
//		animation.Play("CardDrop01");
		
	}
	
	public IEnumerator DoTurn ()
	{
		Debug.Log ("DOING CARD UPKEEP: " + m_id.ToString());

		//bla
		if (m_type == CardType.FrostSnap && m_cardState == CardState.Normal && m_player != null)
		{
			for (int i=0; i < GameManager.m_gameManager.followers.Count; i++)
			{
				Follower follower = GameManager.m_gameManager.followers[i];
				if (follower.followerState != Follower.FollowerState.Stunned && follower.m_followerType != GameManager.m_gameManager.playerFollower.m_followerType)
				{
					yield return StartCoroutine(follower.DoStun(3));
					i = 99;
					
				}
			}
		}
		else if (m_type == CardType.Whispers && m_cardState == CardState.Normal && (m_player != null || m_enemy != null))
		{
			if (m_player != null)
			{
				FollowCamera.m_followCamera.SetTarget(Player.m_player.gameObject);
				yield return new WaitForSeconds(0.5f);	
				UIManager.m_uiManager.SpawnAbilityName("Whispers", Player.m_player.m_playerMesh.transform);
				yield return new WaitForSeconds(0.25f);	
				yield return StartCoroutine(Player.m_player.TakeDamage(1));	
				yield return new WaitForSeconds(0.25f);		
				Player.m_player.GainEnergy(-1);
				UIManager.m_uiManager.SpawnAbilityName("1 E", Player.m_player.m_playerMesh.transform);
				yield return new WaitForSeconds(0.25f);	
			}else if (m_enemy != null)
			{
				yield return new WaitForSeconds(0.5f);	
				UIManager.m_uiManager.SpawnAbilityName("Whispers", m_enemy.gameObject.transform);
				yield return new WaitForSeconds(0.25f);	
				yield return StartCoroutine(m_enemy.TakeDamage(1));	
				yield return new WaitForSeconds(0.25f);		
				if (m_enemy.energy > 0)
				{
					yield return StartCoroutine(m_enemy.GainEnergy(-1));
					UIManager.m_uiManager.SpawnAbilityName("1 E", m_enemy.transform);
					yield return new WaitForSeconds(0.25f);	
				}
			}
		}
		else if (m_type == CardType.Mine && m_cardState == CardState.Normal)
		{
			int numAdjacent = 0;
			for (int i=0; i < 4; i++)
			{
				if (m_linkedCards[i] != null)
				{
					Card linkedCard = m_linkedCards[i];
					if ((linkedCard.enemy != null || linkedCard.player != null) && linkedCard.cardState == CardState.Normal)
					{
						numAdjacent ++;	
					}
				}
			}
			
			if (numAdjacent >= 2)
			{
				for (int i=0; i < 4; i++)
				{
					if (m_linkedCards[i] != null)
					{
						Card linkedCard = m_linkedCards[i];
						if (linkedCard.player != null)
						{
							string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 is affected by \\9" + Player.m_player.currentCard.m_displayName;
							UIManager.m_uiManager.UpdateActions (newString);

							FollowCamera.m_followCamera.SetTarget(linkedCard.gameObject);
							yield return new WaitForSeconds(0.5f);	
							UIManager.m_uiManager.SpawnAbilityName("Magma Mine", linkedCard.player.m_playerMesh.transform);
							yield return new WaitForSeconds(0.25f);	
							yield return StartCoroutine(Player.m_player.TakeDamage(3));	
							yield return new WaitForSeconds(0.25f);	
						} else if (linkedCard.enemy != null)
						{
							string newString = "\\4" + m_enemy.m_displayName + "\\0 is affected by \\9" + m_enemy.currentCard.m_displayName;
							UIManager.m_uiManager.UpdateActions (newString);

							UIManager.m_uiManager.SpawnAbilityName("Magma Mine", linkedCard.enemy.transform);
							yield return new WaitForSeconds(0.5f);
							yield return StartCoroutine(linkedCard.enemy.TakeDamage(3));	
							yield return new WaitForSeconds(0.25f);	
						} else if (linkedCard.follower != null)
						{
//							UIManager.m_uiManager.SpawnAbilityName("Magma Mine", linkedCard.follower.transform);
//							yield return new WaitForSeconds(0.5f);
//							Follower thisF = (Follower)linkedCard.follower.GetComponent("Follower");
//							thisF.TakeDamage(3);
//							UIManager.m_uiManager.SpawnDamageNumber(5, thisF.transform);
//							yield return new WaitForSeconds(0.25f);	
						}
					}
				}
			}
		}
		else if (m_type == CardType.Magma && m_isOccupied && m_chest == null)
		{
			if (m_player != null)
			{
				string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 is affected by \\9" + Player.m_player.currentCard.m_displayName;
				UIManager.m_uiManager.UpdateActions (newString);

				FollowCamera.m_followCamera.SetTarget(this.gameObject);
				yield return new WaitForSeconds(0.5f);
				UIManager.m_uiManager.SpawnAbilityName("Magma", m_player.m_playerMesh.transform);
				yield return new WaitForSeconds(0.5f);
				yield return StartCoroutine(Player.m_player.TakeDamage(3));	
				yield return new WaitForSeconds(0.25f);		
			} else if (m_enemy != null)
			{
				string newString = "\\4" + m_enemy.m_displayName + "\\0 is affected by \\9" + m_enemy.currentCard.m_displayName;
				UIManager.m_uiManager.UpdateActions (newString);

				UIManager.m_uiManager.SpawnAbilityName("Magma", m_enemy.transform);
				yield return new WaitForSeconds(0.5f);
				yield return StartCoroutine(m_enemy.TakeDamage(3));	
				yield return new WaitForSeconds(0.25f);		
			} else if (m_follower != null)
			{
//				Follower thisF = (Follower)m_follower.GetComponent("Follower");
//				UIManager.m_uiManager.SpawnAbilityName("Magma", m_follower.transform);
//				yield return new WaitForSeconds(0.5f);
//				thisF.TakeDamage(3);
//				UIManager.m_uiManager.SpawnDamageNumber(3, thisF.transform);
//				yield return new WaitForSeconds(0.25f);	
			}
		}
		else if (m_type == CardType.Spikes && m_isOccupied && m_chest == null)
		{
			int damage = 0;
			for (int i=0; i < 4; i++)
			{
				if (linkedCards[i] != null)
				{
					Card linkedCard = linkedCards[i];
					if (linkedCard.isOccupied && linkedCard.cardState == CardState.Normal && linkedCard.chest == null)
					{
						if (linkedCard.m_player != null)
						{
							damage ++;
						} else if (linkedCard.m_enemy != null)
						{
							damage ++;
						} else if (linkedCard.m_follower != null)
						{
							damage ++;
						}
					}
				}
			}
			
			if (damage > 0)
			{
				if (m_player != null)
				{
					string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 is affected by \\9" + Player.m_player.currentCard.m_displayName;
					UIManager.m_uiManager.UpdateActions (newString);

					FollowCamera.m_followCamera.SetTarget(this.gameObject);
					yield return new WaitForSeconds(0.5f);
					UIManager.m_uiManager.SpawnAbilityName("Spikes", m_player.m_playerMesh.transform);
					yield return new WaitForSeconds(0.5f);
					yield return StartCoroutine(Player.m_player.TakeDamage(damage));	
					yield return new WaitForSeconds(0.25f);		
				} else if (m_enemy != null)
				{
					string newString = "\\4" + m_enemy.m_displayName + "\\0 is affected by \\9" + m_enemy.currentCard.m_displayName;
					UIManager.m_uiManager.UpdateActions (newString);

					UIManager.m_uiManager.SpawnAbilityName("Spikes", m_enemy.transform);
					yield return new WaitForSeconds(0.5f);
					yield return StartCoroutine(m_enemy.TakeDamage(damage));	
					yield return new WaitForSeconds(0.25f);		
				} else if (m_follower != null)
				{
//					UIManager.m_uiManager.SpawnAbilityName("Spikes", m_follower.transform);
//					yield return new WaitForSeconds(0.5f);
//					Follower thisF = (Follower)m_follower.GetComponent("Follower");
//					thisF.TakeDamage(damage);
//					UIManager.m_uiManager.SpawnDamageNumber(1, thisF.transform);
//					yield return new WaitForSeconds(0.25f);	
				}
			}
		}
		else if (m_type == CardType.Spore && m_isOccupied && m_chest == null)
		{
			//damage any adjacent heroes or enemies
			for (int i=0; i < 4; i++)
			{
				if (linkedCards[i] != null)
				{
					Card linkedCard = linkedCards[i];
					if (linkedCard.isOccupied && linkedCard.cardState == CardState.Normal && linkedCard.chest == null)
					{
						if (linkedCard.m_player != null)
						{
							FollowCamera.m_followCamera.SetTarget(linkedCard.gameObject);
							yield return new WaitForSeconds(0.5f);
							UIManager.m_uiManager.SpawnAbilityName("Spore", linkedCard.m_player.m_playerMesh.transform);
							yield return new WaitForSeconds(0.5f);
							yield return StartCoroutine(Player.m_player.TakeDirectDamage(1));	
							yield return new WaitForSeconds(0.25f);	
						} else if (linkedCard.m_enemy != null)
						{
							string newString = "\\4" + m_enemy.m_displayName + "\\0 is affected by \\9" + m_enemy.currentCard.m_displayName;
							UIManager.m_uiManager.UpdateActions (newString);

							UIManager.m_uiManager.SpawnAbilityName("Spore", linkedCard.m_enemy.transform);
							yield return new WaitForSeconds(0.5f);
							yield return StartCoroutine (linkedCard.enemy.TakeDirectDamage(1));	
							yield return new WaitForSeconds(0.25f);	
						} else if (linkedCard.m_follower != null)
						{
//							Follower thisF = (Follower)linkedCard.follower.GetComponent("Follower");
//							UIManager.m_uiManager.SpawnAbilityName("Spore", linkedCard.m_follower.transform);
//							yield return new WaitForSeconds(0.5f);
//							thisF.TakeDamage(1);
//							UIManager.m_uiManager.SpawnDamageNumber(1, thisF.transform);
//							yield return new WaitForSeconds(0.25f);	
						}
					}
				}
			}
		}
		else if (m_type == CardType.Trap_Razorvine && m_isOccupied && m_chest == null)
		{
			
			FollowCamera.m_followCamera.SetTarget(this.gameObject);
			yield return new WaitForSeconds(0.5f);	
			if (m_player != null)
			{
				string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 is affected by \\9" + Player.m_player.currentCard.m_displayName;
				UIManager.m_uiManager.UpdateActions (newString);

				FollowCamera.m_followCamera.SetTarget(this.gameObject);
				yield return new WaitForSeconds(0.5f);
				UIManager.m_uiManager.SpawnAbilityName("Razorvine", m_player.m_playerMesh.transform);
				yield return new WaitForSeconds(0.5f);
				yield return StartCoroutine(m_player.TakeDamage(1));
				yield return new WaitForSeconds(0.5f);
			} else if (m_enemy != null)
			{
				string newString = "\\4" + m_enemy.m_displayName + "\\0 is affected by \\9" + m_enemy.currentCard.m_displayName;
				UIManager.m_uiManager.UpdateActions (newString);

				Debug.Log("DAMAGING ENEMY");
				UIManager.m_uiManager.SpawnAbilityName("Razorvine", m_enemy.transform);
				yield return new WaitForSeconds(0.5f);
				yield return StartCoroutine(m_enemy.TakeDamage(1));
				yield return new WaitForSeconds(0.5f);
			} else if (m_follower != null)
			{
//				UIManager.m_uiManager.SpawnAbilityName("Razorvine", m_follower.transform);
//				yield return new WaitForSeconds(0.5f);
//				Follower thisFollower = (Follower)m_follower.GetComponent("Follower");
//				thisFollower.TakeDamage(2);
			}
			
		} else if (m_type == CardType.Quicksand && m_player != null)
		{
			string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 is affected by \\9" + Player.m_player.currentCard.m_displayName;
			UIManager.m_uiManager.UpdateActions (newString);

			FollowCamera.m_followCamera.SetTarget(this.gameObject);
			yield return new WaitForSeconds(0.5f);
			UIManager.m_uiManager.SpawnAbilityName("Web", m_player.m_playerMesh.transform);
			yield return new WaitForSeconds(0.5f);	
			UIManager.m_uiManager.SpawnFloatingText("-2", UIManager.Icon.Energy, Player.m_player.m_playerMesh.transform);
			Player.m_player.GainEnergy (-2);
			yield return new WaitForSeconds(0.5f);
			
		} else if ((m_type == CardType.Warren || m_type == CardType.Unholy) && !m_isOccupied)
		{
			if (!m_isOccupied)
			{
				m_turnTicker ++;
				if (m_turnTicker >= 3)
				{
					m_turnTicker = 0;
	//				FollowCamera.m_followCamera.SetTarget(this.gameObject);
					yield return new WaitForSeconds(0.5f);	
					
					GameObject thisEnemy = m_spawn;
					Vector3 pos = this.transform.position;
					Vector3 rot = thisEnemy.transform.eulerAngles;
					Enemy newEnemy = (Enemy)((GameObject)Instantiate(thisEnemy, pos, Quaternion.Euler(rot))).transform.GetComponent("Enemy");
					enemy = newEnemy;
					newEnemy.Initialize(Enemy.EnemyState.Idle, this);
					newEnemy.gameObject.transform.parent = this.m_cardMesh.transform;
					GameManager.m_gameManager.currentMap.m_enemies.Add(newEnemy);
				}
			} else if (m_isOccupied && m_turnTicker > 0)
			{
				m_turnTicker = 0;	
			}
			
		} else if (m_type == CardType.Fire)
		{
//			if (m_player != null)
//			{
//				FollowCamera.m_followCamera.SetTarget(this.gameObject);
//				yield return new WaitForSeconds(0.5f);	
//				UIManager.m_uiManager.SpawnAbilityName("Fire", m_player.m_playerMesh.transform);
//				yield return new WaitForSeconds(0.25f);	
//				yield return StartCoroutine(m_player.TakeDamage(1));
//				yield return new WaitForSeconds(0.25f);	
//				
//			} else if (m_enemy != null && m_enemy.m_abilityType != Enemy.AbilityType.Firewalker)
//			{
//				FollowCamera.m_followCamera.SetTarget(this.gameObject);
//				yield return new WaitForSeconds(0.5f);	
//				UIManager.m_uiManager.SpawnAbilityName("Fire", m_enemy.transform);
//				yield return new WaitForSeconds(0.25f);
//				yield return StartCoroutine(m_enemy.TakeDamage(1));
//				yield return new WaitForSeconds(0.25f);	
//			}
			
			for (int i=0; i < 4; i++)
			{
				if (m_linkedCards[i] != null)
				{
					Card linkedCard = m_linkedCards[i];
					if (linkedCard.player != null && linkedCard.cardState == CardState.Normal)
					{
						FollowCamera.m_followCamera.SetTarget(linkedCard.transform.gameObject);
						yield return new WaitForSeconds(0.5f);	
						UIManager.m_uiManager.SpawnAbilityName("Fire", linkedCard.player.m_playerMesh.transform);
						yield return new WaitForSeconds(0.25f);	
						yield return StartCoroutine(linkedCard.player.TakeDamage(1));
						yield return new WaitForSeconds(0.25f);	
						
					} else if (linkedCard.enemy != null && linkedCard.enemy.m_abilityType != Enemy.AbilityType.Firewalker && linkedCard.enemy.m_abilityType != Enemy.AbilityType.Firestrength && linkedCard.cardState == CardState.Normal)
					{
						FollowCamera.m_followCamera.SetTarget(linkedCard.transform.gameObject);
						yield return new WaitForSeconds(0.5f);	
						UIManager.m_uiManager.SpawnAbilityName("Fire", linkedCard.enemy.transform);
						yield return new WaitForSeconds(0.25f);
						yield return StartCoroutine(linkedCard.enemy.TakeDamage(1));
						yield return new WaitForSeconds(0.25f);	
					}
				}
			}
		}

		Debug.Log ("FINISHING CARD UPKEEP: " + m_id.ToString());
		yield return null;	
	}
	
	public void SetLinkedCards (Card[] linkedCards)
	{
		m_linkedCards = linkedCards;
		if (linkedCards[0] != null)
		{
			m_northCard = linkedCards[0];
			m_numLinked ++;
		}
		if (linkedCards[1] != null)
		{
			m_southCard = linkedCards[1];
			m_numLinked ++;
		}
		if (linkedCards[2] != null)
		{
			m_eastCard = linkedCards[2];
			m_numLinked ++;
		}
		if (linkedCards[3] != null)
		{
			m_westCard = linkedCards[3];
			m_numLinked ++;
		}
	}
	
	public IEnumerator ChangeCardType (Card newCard)
	{
		m_type = newCard.type;
		m_displayName = newCard.m_displayName;
		m_abilityText = newCard.m_abilityText;
		m_portraitSpriteName = newCard.m_portraitSpriteName;
		m_cardMesh.material = newCard.m_cardMesh.sharedMaterial;
		m_siteArmorBonus = newCard.m_siteArmorBonus;
		m_siteDamageBonus = newCard.m_siteDamageBonus;
		m_siteRangeBonus = newCard.siteRangeBonus;
		//m_doUpkeep = newCard.doUpkeep;
		m_doFlipTrap = false;
		yield return null;
	}

	private Color neutralColor = Color.blue;
	public void SetColor (Color newColor)
	{
		neutralColor = newColor;
	}

	public IEnumerator FlashFade ()
	{
		float flashTimer = 0;
		float flashTime = 1;
		Color startColor = Color.white;
		Color endColor = neutralColor;
		
		m_highlightMesh.material.color = startColor;
		
		while (flashTimer < flashTime) {
			flashTimer = Mathf.Clamp(flashTimer + Time.deltaTime, 0, flashTime);
			Color newColor = Color.Lerp (startColor, endColor, flashTimer / flashTime);
			m_highlightMesh.material.color = newColor;
			
			yield return null;
		}
		
		m_highlightMesh.material.color = endColor;
		
		yield return null;
	}

	public IEnumerator RedFade ()
	{
		yield return new WaitForSeconds (1.0f);

		float flashTimer = 0;
		float flashTime = 0.75f;
		Color startColor = Color.red;
		Color endColor = new Color (0.15f, 0, 0);
		SetColor (endColor);
		m_highlightMesh.material.color = startColor;
		
		while (flashTimer < flashTime) {
			flashTimer = Mathf.Clamp(flashTimer + Time.deltaTime, 0, flashTime);
			Color newColor = Color.Lerp (startColor, endColor, flashTimer / flashTime);
			m_highlightMesh.material.color = newColor;
			
			yield return null;
		}
		
		m_highlightMesh.material.color = endColor;
		yield return null;
	}

	public IEnumerator LightFlash (float time)
	{
		float flashTimer = 0;
		float flashTime = time;
		Color startColor = Color.white;
		Color endColor = neutralColor;

		m_highlightMesh.material.color = startColor;

		m_fx01.gameObject.SetActive (true);
		ScrollTexture st = (ScrollTexture)m_fx01.GetComponent<ScrollTexture> ();
		st.enabled = true;
		m_fx01.animation.Play ();
		//yield return new WaitForSeconds (0.4f);


		while (flashTimer < flashTime) {
			flashTimer = Mathf.Clamp(flashTimer + Time.deltaTime, 0, flashTime);
			Color newColor = Color.Lerp (startColor, endColor, flashTimer / flashTime);
			m_highlightMesh.material.color = newColor;

			yield return null;
		}



		st.enabled = false;
		m_fx01.gameObject.SetActive (false);
		m_highlightMesh.material.color = endColor;

		yield return null;
	}

	void OnMouseEnter ()
	{
		if (m_cardState == CardState.Normal) {
			Color highlightColor = neutralColor * 1.5f;

			if (m_highlightState == HighlightState.Highlighted)
			{
				highlightColor = m_highlightColor * 0.25f;
			}
			
			m_highlightMesh.material.color = highlightColor;
		}
	}

	void OnMouseExit ()
	{
		if (m_cardState == CardState.Normal) {
			if (m_highlightState == HighlightState.NotHighlighted)
			{
				m_highlightMesh.material.color = neutralColor;
			} else {
				m_highlightMesh.material.color = m_highlightColor;
			}
		}
	}
	
	public IEnumerator ChangeCardState (CardState newState)
	{
		Debug.Log ("CHANGING CARD STATE: " + newState);
		CardState oldState = m_cardState;
		m_cardState = newState;
		
		if (newState == CardState.Normal)
		{
			if (oldState == CardState.Hidden)
			{
				GameManager.m_gameManager.numTilesFlipped += 1;
			}

			yield return StartCoroutine(LightFlash(0.35f));

			if (m_type == CardType.Exit)
			{
				string newString = "The Exit appears";
				UIManager.m_uiManager.UpdateActions (newString);

				SetColor(Color.yellow);
				m_highlightMesh.material.color = Color.yellow;
				m_meshes[1].SetActive(true);

			} else if (m_type != CardType.Normal && m_type != CardType.Entrance)
			{
				Color p = new Color(1,0,1);
				SetColor(p);
				m_highlightMesh.material.color = p;
				m_meshes[1].SetActive(true);
			}

			animation.Play("CardFlip02");
			if (m_enemy != null) //turn on enemy visuals
			{
				string newString = "\\4" + m_enemy.m_displayName + "\\0 appears!";
				UIManager.m_uiManager.UpdateActions (newString);

				AssetManager.m_assetManager.PlaySFX(AssetManager.SFXType.EnemyReveal);
				StartCoroutine(m_enemy.ChangeState(Enemy.EnemyState.Idle));	
				BoxCollider bc = (BoxCollider) m_enemy.transform.GetComponent("BoxCollider");
				bc.enabled = true;
			} else if (m_chest != null)
			{
				AssetManager.m_assetManager.PlaySFX(AssetManager.SFXType.ChestReveal);
				m_chest.m_chestMesh.renderer.enabled = true;	
				BoxCollider bc = (BoxCollider) m_chest.transform.GetComponent("BoxCollider");
				bc.enabled = true;
			}
			else if (m_goal != null)
			{
				m_goal.SetActive(true);	
			}else if (m_follower != null)
			{
				m_follower.SetActive(true);	
				BoxCollider bc = (BoxCollider) m_follower.transform.GetComponent("BoxCollider");
				bc.enabled = true;
			} else if (m_shop != null)
			{
				m_shop.SetFacing(this);

				foreach (MeshRenderer mr in m_shop.m_shopMesh)
				{
					mr.renderer.enabled = true;
				}
				BoxCollider bc = (BoxCollider) m_shop.transform.GetComponent("BoxCollider");
				bc.enabled = true;
			}
			
			if (m_doFlipTrap)
			{
				string newString = "\\9Flipper Tile\\0 activates";
				UIManager.m_uiManager.UpdateActions (newString);

				//m_flipTrapMesh.enabled = true;
				m_highlightMesh.material.color = Color.red;
				m_meshes[0].SetActive(true);
				StartCoroutine(RedFade());
			}
			
//			while (animation.IsPlaying("CardFlip02"))
//			{
//				yield return null;	
//			}
			yield return new WaitForSeconds(0.2f);
			if (!m_doFlipTrap) {
			StartCoroutine(LightFlash(0.7f));
			}
			if (m_doFlipTrap)
			{
				AssetManager.m_assetManager.PlaySFX(AssetManager.SFXType.Flipper);
				yield return StartCoroutine(MapManager.m_mapManager.ActivateFlipTrap(this));
			} else {
				AssetManager.m_assetManager.PlaySFX(AssetManager.SFXType.CardFlip);
			}
			
			
		} else if (newState == CardState.Hidden)
		{
			if (oldState == CardState.Normal)
			{
				GameManager.m_gameManager.numTilesFlipped -= 1;
			}

			animation.Play("CardFlip02Reverse");
			if (m_enemy != null)
			{
				StartCoroutine(m_enemy.ChangeState(Enemy.EnemyState.Inactive));	
			} else if (m_chest != null)
			{
				m_chest.m_chestMesh.renderer.enabled = false;
			} else if (m_goal != null)
			{
				m_goal.SetActive(false);	
			}	
		}
		yield break;
	}
	
	public IEnumerator ActivateCard ()
	{
		if (m_type == CardType.Stalactite)
		{
			if (m_player != null)
			{
				yield return new WaitForSeconds(0.25f);	
				UIManager.m_uiManager.SpawnAbilityName("Stalactite", Player.m_player.m_playerMesh.transform);
				yield return new WaitForSeconds(0.25f);	
				yield return StartCoroutine(Player.m_player.TakeDamage(1));
				yield return new WaitForSeconds(0.25f);	
			} else if (m_enemy != null)
			{
				yield return new WaitForSeconds(0.25f);	
				UIManager.m_uiManager.SpawnAbilityName("Stalactite", m_enemy.transform);
				yield return new WaitForSeconds(0.25f);	
				yield return StartCoroutine(m_enemy.TakeDamage(1));
				yield return new WaitForSeconds(0.25f);	
			}
		} else if (m_type == CardType.ManaBurn)
		{
			if (m_player != null)
			{
				yield return new WaitForSeconds(0.25f);	
				UIManager.m_uiManager.SpawnAbilityName("Mana Burn", Player.m_player.m_playerMesh.transform);
				yield return new WaitForSeconds(0.25f);	
				yield return StartCoroutine(Player.m_player.TakeDamage(2));
				yield return new WaitForSeconds(0.25f);	
			} else if (m_enemy != null)
			{
				yield return new WaitForSeconds(0.25f);	
				UIManager.m_uiManager.SpawnAbilityName("Mana Burn", m_enemy.transform);
				yield return new WaitForSeconds(0.25f);	
				yield return StartCoroutine(m_enemy.TakeDamage(2));
				yield return new WaitForSeconds(0.25f);	
			}
		}
		else if (m_type == CardType.RazorGlade)
		{
			if (m_player != null)
			{
				yield return new WaitForSeconds(0.25f);	
				UIManager.m_uiManager.SpawnAbilityName("Razor Glade", Player.m_player.m_playerMesh.transform);
				yield return new WaitForSeconds(0.25f);	
				yield return StartCoroutine(Player.m_player.TakeDamage(2));
				yield return new WaitForSeconds(0.5f);
			} else if (m_enemy != null)
			{
				yield return new WaitForSeconds(0.25f);	
				UIManager.m_uiManager.SpawnAbilityName("Razor Glade", m_enemy.transform);
				yield return new WaitForSeconds(0.25f);	
				yield return StartCoroutine(m_enemy.TakeDamage(2));
				yield return new WaitForSeconds(0.5f);
			}
		}
		yield return null;
	}

	public IEnumerator DoAmbush ()
	{
		m_doAmbush = false;

		if (m_enemy != null) {

			string newString = "\\4" + m_enemy.m_displayName + " uses \\8Ambush";
			UIManager.m_uiManager.UpdateActions (newString);

		}

		yield return new WaitForSeconds(0.25f);	
		UIManager.m_uiManager.SpawnAbilityName("Ambush", m_enemy.transform);
		yield return new WaitForSeconds(0.25f);	
		yield return StartCoroutine (ChangeCardState (CardState.Normal));

		yield return true;
	}

	
	public int GetEnergyValue ()
	{
		int ev = m_energyValue;
		
		//check for adjacent, revealed Ash Cloud
		for (int i=0; i < 4; i++)
		{
			if (m_linkedCards[i] != null)
			{
				Card thisCard = m_linkedCards[i];
				if (thisCard.type == CardType.AshCloud && thisCard.cardState == CardState.Normal)
				{
					ev = 0;
					i = 99;
				}
			}
		}
		
		
		return ev;
	}

	private Color previousColor = Color.black;
	public void ChangeHighlightState(bool HLon)
	{
		if (HLon)
		{
			m_highlightState = HighlightState.Highlighted;
			//m_highlightMesh.enabled = true;
			previousColor = m_highlightMesh.material.color;
			m_highlightMesh.material.color = m_highlightColor;
		} else
		{
			m_highlightState = HighlightState.NotHighlighted;	
			//m_highlightMesh.enabled = false;
			m_highlightMesh.material.color = previousColor;
		}
	}
	
	
	public CardType type
	{
		get
		{
			return m_type;	
		}
		set 
		{
			m_type = value;	
			if (m_type == CardType.Trap_Razorvine || m_type == CardType.Quicksand || m_type == CardType.Warren || m_type == CardType.Spore || m_type == CardType.Spikes
				|| m_type == CardType.Unholy || m_type == CardType.Magma || m_type == CardType.Fire || m_type == CardType.Mine || m_type == CardType.Whispers || m_type == CardType.FrostSnap)
			{
				m_doUpkeep = true;

			} else {
				m_doUpkeep = false;	
			}
			if (m_type == CardType.Fort)
			{
				m_siteArmorBonus = 3;	
			} else if (m_type == CardType.Tower)
			{
				m_siteRangeBonus += 1;
			} else if (m_type == CardType.HighGround)
			{
				m_siteDamageBonus += 2;	
			} else if (m_type == CardType.ClawingHands)
			{
				m_siteArmorBonus = -3;
				m_siteDamageBonus = -3;
			}
//			if (m_flipTrapMesh.gameObject.activeSelf)
//			{
//				m_flipTrapMesh.gameObject.SetActive(false);
//			}
		}
	}
	public int numLinked
	{
		get
		{
			return m_numLinked;	
		}
	}
	public int distanceToPlayer
	{
		get
		{
			return m_distanceToPlayer;
		}
		set
		{
			m_distanceToPlayer = value;	
		}
	}
	public int mapID
	{
		get
		{
			return m_mapID;
		}
		set
		{
			m_mapID = value;	
		}
	}
	public int row
	{
		get
		{
			return m_row;	
		}
	}
	public int column
	{
		get
		{
			return m_column;	
		}
	}
	public MeshRenderer cardMesh
	{
		get
		{
			return m_cardMesh;	
		}
	}
	public CardState cardState
	{
		get
		{
			return m_cardState;	
		}
	}
	public Card northCard
	{
		get
		{
			return m_northCard;	
		}
	}
	public Card southCard
	{
		get
		{
			return m_southCard;	
		}
	}
	public Card eastCard
	{
		get
		{
			return m_eastCard;	
		}
	}
	public Card westCard
	{
		get
		{
			return m_westCard;	
		}
	}
	public Card destination
	{
		get
		{
			return m_destination;	
		}
		set
		{
			m_destination = value;	
		}
	}
	public Chest chest
	{
		get
		{
			return m_chest;	
		}
		set
		{
			m_chest = value;
			if (m_chest != null)
			{
				m_isOccupied = true;	
			} else if (m_player == null && m_enemy == null && m_goal == null && m_follower == null && m_shop == null) {
				m_isOccupied = false;	
			}
		}
	}
	public GameObject goal
	{
		get
		{
			return m_goal;	
		}
		set
		{
			m_goal = value;
			if (m_goal != null)
			{
				m_isOccupied = true;	
			} else if (m_player == null && m_chest == null && m_enemy == null && m_follower == null && m_shop == null) {
				m_isOccupied = false;	
			}
		}
	}
	public GameObject follower
	{
		get
		{
			return m_follower;	
		}
		set
		{
			m_follower = value;
			if (m_follower != null)
			{
				m_isOccupied = true;	
			} else if (m_player == null && m_chest == null && m_enemy == null && m_goal == null && m_shop == null) {
				m_isOccupied = false;	
			}
		}
	}
	public Enemy enemy
	{
		get
		{
			return m_enemy;	
		}
		set
		{
			m_enemy = value;
			if (m_enemy != null)
			{
				m_isOccupied = true;	
				if (enemy.m_abilityType == Enemy.AbilityType.Ambush)
				{
					m_doAmbush = true;
				}
			} else if (m_player == null && m_chest == null && m_goal == null && m_follower == null && m_shop == null) {
				m_isOccupied = false;	
				m_doAmbush = false;
			}
		}
	}
	public Player player
	{
		get
		{
			return m_player;	
		}
		set
		{
			m_player = value;	
			if (m_player != null)
			{
				m_isOccupied = true;	
			} else if (m_enemy == null && m_chest == null && m_goal == null && m_follower == null && m_shop == null) {
				m_isOccupied = false;	
			}
		}
	}

	public Shop shop {get{return m_shop;}set{
			m_shop = value;
			if (m_shop != null)
			{
				m_isOccupied = true;
			} else if (m_enemy == null && m_chest == null && m_goal == null && m_follower == null && m_player == null)
			{
				m_isOccupied = false;
			}
		}}

	public bool isOccupied
	{
		get
		{
			return m_isOccupied;	
		}
	}
	public Card[] linkedCards
	{
		get
		{
			return m_linkedCards;	
		}
	}
	public Card.HighlightState highlightState
	{
		get
		{
			return m_highlightState;	
		}
	}
	public bool doUpkeep {get{return m_doUpkeep;}set{m_doUpkeep = value;}}
	public int siteArmorBonus {get{return m_siteArmorBonus;}set{m_siteArmorBonus = value;}}
	public int siteDamageBonus {get{return m_siteDamageBonus;}set{m_siteDamageBonus = value;}}
	public int siteRangeBonus {get{return m_siteRangeBonus;}set{m_siteRangeBonus = value;}}
	public bool doFlipTrap {get{return m_doFlipTrap;}set{m_doFlipTrap = value;}}
	public bool doAmbush {get{return m_doAmbush;}}
	public SubType subType {get{return m_subType;} set{m_subType = value;}}
	public int id {get{return m_id;}}
}
