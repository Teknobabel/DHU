using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item : MonoBehaviour {
	
	public enum Keyword
	{
		Craftable,
		Consumeable,
		Equippable,
		Pet,
		Chain,
		UseFromInv,
		Skill,
		Limbo,
		WhileInHand,
		LostSoul,
		Key,
	}

	public enum TargetType
	{
		Target,
		All,
		Direction,
	}

	public enum GraveBonus
	{
		Attack,
		Health,
		Energy,
		Armor,
		Actions,
	}
	
	public enum ItemState
	{
		Normal,
		Spent,
	}
	
	public string
		m_name,
//		m_storageName, //temp workaround because current save system doesn't like spaces in names
		m_description,
		m_portraitSpriteName = "Card_Portrait_Item02";
//		m_fullPortraitSpriteName = "Card_ItemPH_Full",
//		m_shortcutText = "none",
//		m_keywordText = "none";
	
	public int
//		m_healthBonus = 0,
//		m_energyBonus = 0,
//		m_attackBonus = 0,
//		m_armorBonus = 0,
//		m_actionBonus = 0,
		m_energyCost = 0,
		m_healthCost = 0;
//		m_chainLevel = 0,
//		m_adjacentDamage = 0,
//		m_XPBonus = 0,
//		m_goldBonus = 0,
//		m_reflectMeleeDamage = 0,
//		m_actionPerHiddenBonus = 0,
//		m_lifeTapBonus = 0,
//		m_soulTapBonus = 0,
//		m_refreshDamage = 0,
//		m_trapDestroyRange = 0,
//		m_moveDistance = 0,
//		m_levelsSkipped = 0,
//		m_stunDuration = 0;
//	
//	public bool
//		m_refreshParty = false,
//		m_rangedAttack = false,
//		m_curePoison = false,
//		m_enableCounterAttack = false,
//		m_enablePoison = false,
//		m_enableArmorPierce = false,
//		m_flipAdjacent = false,
//		m_knockbackAdjacent = false,
//		m_stun = false,
//		m_skipTurn = false,
//		m_doRage = false,
//		m_doSoulArmor = false,
//		m_enableSacrifice = false,
//		m_enableCover = false,
//		m_doSpellbook = false,
//		m_reverseAdjacent = false,
//		m_graveHealth = false,
//		m_graveEnergy = false,
//		m_graveBomb = false,
//		m_graveItemDamage = false,
//		m_rezItems = false,
//		m_clearAllWounds = false,
//		m_clearAllCorruption = false;
//	
//	public int
//		m_attackRange = 0,
//		m_attackDamage = 0;
//
	public Follower.FollowerClass m_class = Follower.FollowerClass.None;
//	public int m_itemLevel = 1;
//	public EffectsPanel.Effect.Duration
//		m_duration = EffectsPanel.Effect.Duration.EndOfTurn;
//	
//	public GameManager.Dice[]
//		m_attackBonusDice,
//		m_armorBonusDice;
//	
	public Keyword[]
		m_keywords;

	public GraveBonus[]
		m_graveBonus;
//	
//	public GameObject
//		m_craftResult;
//	
//	private ItemState
//		m_itemState = ItemState.Normal;

	private UICard
		m_card;
//	
	private int
		m_id = -99,
//		m_attackDiceRoll = 0,
//		m_armorDiceRoll = 0,
		m_adjustedEnergyCost = 0,
		m_adjustedHealthCost = 0;
//
//	private Follower
//		m_attachedFollower = null;


	// Use this for initialization
	void Start () {
		m_id = GameManager.m_gameManager.GetNewID();
	}
	
	public bool HasKeyword (Keyword thisKeyword)
	{
		if (m_keywords.Length > 0)
		{
			foreach (Keyword kw in m_keywords)
			{
				if (kw == thisKeyword)
				{
					return true;	
				}
			}
		}
		
		return false;
	}

	public virtual IEnumerator Activate ()
	{
		yield return true;
	}

	public virtual IEnumerator Deactivate ()
	{
		yield return true;
	}

	public IEnumerator PayForCard ()
	{
		if (m_adjustedEnergyCost > 0)
		{
			Player.m_player.GainEnergy(m_adjustedEnergyCost * -1);
		} else if (m_adjustedHealthCost > 0)
		{
			yield return StartCoroutine(Player.m_player.TakeDirectDamage(m_adjustedHealthCost));
		}

		yield return true;
	}

	public IEnumerator CenterCard ()
	{
		// move card to center
		float t = 0;
		float time = 0.3f;
		Vector3 startPos = card.transform.position;
		Vector3 endPos = UIManager.m_uiManager.m_HUD.transform.position;
		endPos.x -= 0.5f;

		while (t < time)
		{
			t += Time.deltaTime;;
			Vector3 nPos = Vector3.Lerp(startPos, endPos , t / time);
			card.transform.position = nPos;
			yield return null;
		}

		card.transform.position = endPos;

		yield return new WaitForSeconds (0.5f);
		yield return true;
	}

	public IEnumerator SendToGrave ()
	{
		float t = 0;
		float time = 0.5f;
		Vector3 startPos = card.transform.position;
		Vector3 startScale = card.transform.localScale;
		Vector3 endPos = UIManager.m_uiManager.m_backpackButton.transform.position;
		Vector3 endScale = Vector3.one * 0.25f;
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
		card.gameObject.SetActive (false);
		for (int i=0; i < GameManager.m_gameManager.inventory.Count; i++) {
			if (GameManager.m_gameManager.inventory[i].m_id == this.id)
			{
				GameManager.m_gameManager.inventory.RemoveAt(i);
				break;
			}
		}
		GameManager.m_gameManager.SendToGrave (this);
		UIManager.m_uiManager.RefreshInventoryMenu ();
		yield return new WaitForSeconds (0.5f);
		yield return true;
	}

//	public IEnumerator ActivateItem ()
//	{
//		Debug.Log ("ACTIVATING ITEM");
//		bool shiftHeld = false;
//		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
//		{
//			shiftHeld = true;
//		}
//
////		if (this.HasKeyword(Keyword.Equippable) && EquipCards.m_equipCards.freeEquipSlots > 0 && UIManager.m_uiManager.menuMode == UIManager.MenuMode.Inventory && (!shiftHeld || (shiftHeld && !this.HasKeyword(Keyword.UseFromInv))))
////		{
////			EquipCards.m_equipCards.AddItem(this);
////			yield break;
////		} 
//		if (HasKeyword(Keyword.Pet) && m_craftResult != null)
//		{
//
//			// place pet in adjacent site
//			List<GameManager.Direction> directions = new List<GameManager.Direction>();
//			directions.Add(GameManager.Direction.North);
//			directions.Add(GameManager.Direction.South);
//			directions.Add(GameManager.Direction.East);
//			directions.Add(GameManager.Direction.West);
//			List<Card> validCards = MapManager.m_mapManager.GetCardsInRange(Player.m_player.currentCard, 1, directions, false, true, false);
//			if (validCards.Count > 0)
//			{
//				GameManager.m_gameManager.selectMode = true;
//				yield return StartCoroutine(InputManager.m_inputManager.GetMouseSelection(validCards));	
//				GameManager.m_gameManager.selectMode = false;
//				
//				if (GameManager.m_gameManager.selectedCard != null)
//				{
//					GameObject go = (GameObject)Instantiate(AssetManager.m_assetManager.m_props[11], GameManager.m_gameManager.selectedCard.transform.position, AssetManager.m_assetManager.m_props[11].transform.rotation);
//					Pet p = (Pet)go.GetComponent("Pet");
//					GameManager.m_gameManager.currentMap.m_pets.Add(p);
//					p.Initialize(Enemy.EnemyState.Idle, GameManager.m_gameManager.selectedCard);
//					GameManager.m_gameManager.selectedCard.enemy = p;
//
//					Instantiate(AssetManager.m_assetManager.m_particleFX[0], go.transform.position, Quaternion.identity);
//
//				}
//				
//			}
//
//			foreach (Card vc in validCards)
//			{
//				vc.ChangeHighlightState(false);	
//			}
//		}
//
//		if (this.HasKeyword(Keyword.Chain))
//		{
//			if (GameManager.m_gameManager.currentChain == m_chainLevel && m_itemState == ItemState.Normal)
//			{
//				if (m_attackBonus > 0)
//				{
//					int damage = Player.m_player.turnDamage;
//					damage += m_attackBonus;
//					UIManager.m_uiManager.SpawnAbilityName(m_name.ToString(), Player.m_player.m_playerMesh.transform);
//					yield return new WaitForSeconds(0.5f);
//					Player.m_player.turnDamage = damage;
//					StartCoroutine(UIManager.m_uiManager.UpdateDamage(Player.m_player.damage + Player.m_player.turnDamage + Player.m_player.tempDamage));
//					UIManager.m_uiManager.SpawnFloatingText("+" + m_attackBonus.ToString(), UIManager.Icon.MeleeDamage, Player.m_player.m_playerMesh.transform);
//					yield return new WaitForSeconds(0.5f);
//				
//					//Update Effect Stack
//					EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
//					newEffect.m_effectType = EffectsPanel.Effect.EffectType.TurnDamageBonus;
//					newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
//					string desc = m_name + ": " + m_attackBonus.ToString() + "$ until next turn.";
//					newEffect.m_description = desc;
//					newEffect.m_spriteName = "Effect_DamageBonus";
//					newEffect.m_affectedItem = this;
//					EffectsPanel.m_effectsPanel.AddEffect(newEffect);
//				}
//				
//				if (m_armorBonus > 0)
//				{
//					int armor = Player.m_player.turnArmor;
//					armor += m_armorBonus;
//					UIManager.m_uiManager.SpawnAbilityName(m_name.ToString(), Player.m_player.m_playerMesh.transform);
//					yield return new WaitForSeconds(0.5f);
//					Player.m_player.turnArmor = armor;
//					StartCoroutine(UIManager.m_uiManager.UpdateArmor(Player.m_player.currentArmor + Player.m_player.turnArmor + Player.m_player.tempArmor));
//					UIManager.m_uiManager.SpawnFloatingText("+" + m_armorBonus.ToString(), UIManager.Icon.Armor, Player.m_player.m_playerMesh.transform);
//					yield return new WaitForSeconds(0.5f);
//				
//					//Update Effect Stack
//					EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
//					newEffect.m_effectType = EffectsPanel.Effect.EffectType.TurnArmorBonus;
//					newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
//					string desc = m_name + ": " + m_armorBonus.ToString() + "% until next turn.";
//					newEffect.m_description = desc;
//					newEffect.m_spriteName = "Effect_ArmorBonus";
//					newEffect.m_affectedItem = this;
//					EffectsPanel.m_effectsPanel.AddEffect(newEffect);
//				}
//				
//				if (m_healthBonus > 0)
//				{
//					UIManager.m_uiManager.SpawnAbilityName(m_name.ToString(), Player.m_player.m_playerMesh.transform);
//					yield return new WaitForSeconds(0.5f);
//					Player.m_player.GainHealth(m_healthBonus);	
//					UIManager.m_uiManager.SpawnFloatingText("+" + m_healthBonus.ToString(), UIManager.Icon.Health, Player.m_player.m_playerMesh.transform);
//					yield return new WaitForSeconds(0.5f);
//				}
//				
//				if (m_enableCounterAttack)
//				{
//					UIManager.m_uiManager.SpawnAbilityName(m_name.ToString(), Player.m_player.m_playerMesh.transform);
//					yield return new WaitForSeconds(0.5f);
//					Player.m_player.doCounterAttack = true;	
//					UIManager.m_uiManager.SpawnFloatingText("+Counter Attack", UIManager.Icon.None, Player.m_player.m_playerMesh.transform);
//					yield return new WaitForSeconds(0.5f);
//					
//					//Update Effect Stack
//					EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
//					newEffect.m_effectType = EffectsPanel.Effect.EffectType.CounterAttack;
//					newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
//					string desc = "Counter Attack until next turn.";
//					newEffect.m_stackable = false;
//					newEffect.m_description = desc;
//					newEffect.m_spriteName = "Effect_CounterAttack";
//					newEffect.m_affectedItem = this;
//					EffectsPanel.m_effectsPanel.AddEffect(newEffect);
//				}
//				
//				if (m_enablePoison)
//				{
//					UIManager.m_uiManager.SpawnAbilityName(m_name.ToString(), Player.m_player.m_playerMesh.transform);
//					yield return new WaitForSeconds(0.5f);
//					Player.m_player.doPoisonAttack = true;	
//					UIManager.m_uiManager.SpawnFloatingText("+Poison", UIManager.Icon.None, Player.m_player.m_playerMesh.transform);
//					yield return new WaitForSeconds(0.5f);
//					
//					//Update Effect Stack
//					EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
//					newEffect.m_effectType = EffectsPanel.Effect.EffectType.PlayerInflictPoison;
//					newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
//					string desc = "Attacks inflict Poison until next turn.";
//					newEffect.m_stackable = false;
//					newEffect.m_description = desc;
//					newEffect.m_spriteName = "Effect_InflictPoison";
//					newEffect.m_affectedItem = this;
//					EffectsPanel.m_effectsPanel.AddEffect(newEffect);
//				}
//				
//				if (m_actionBonus > 0)
//				{
//					UIManager.m_uiManager.SpawnAbilityName(m_name.ToString(), Player.m_player.m_playerMesh.transform);
//					yield return new WaitForSeconds(0.5f);
//					Player.m_player.GainActionPoints(m_actionBonus);
//					UIManager.m_uiManager.SpawnFloatingText("+" + m_actionBonus.ToString() + "A", UIManager.Icon.None, Player.m_player.m_playerMesh.transform);
//					yield return new WaitForSeconds(0.5f);
//				}
//				
//				if (m_refreshParty)
//				{
//					UIManager.m_uiManager.SpawnAbilityName(m_name.ToString(), Player.m_player.m_playerMesh.transform);
//					yield return new WaitForSeconds(0.5f);
//
//					foreach (Follower thisFollower in GameManager.m_gameManager.followers)
//					{
//						if (thisFollower.followerState == Follower.FollowerState.Spent)
//						{
//							yield return StartCoroutine(thisFollower.ChangeState(Follower.FollowerState.Normal));	
//						}
//					}
//				}
//				
//				ChangeState(ItemState.Spent);
//				yield break;
//			}
//		}
//		//else if (this.HasKeyword(Keyword.Consumeable) && !this.HasKeyword(Keyword.Equippable))
////		else if (this.HasKeyword(Keyword.Consumeable) && this.HasKeyword(Keyword.UseFromInv) && shiftHeld)
////		{
////			if (m_curePoison && Player.m_player.currentEffect == GameManager.StatusEffect.Poison)
////			{
////				Player.m_player.ChangeEffectState(GameManager.StatusEffect.None, 0);	
////			}
////			
////			Player.m_player.GainHealth(m_healthBonus);
////			Player.m_player.GainEnergy(m_energyBonus);
////			
////			if (m_armorBonus > 0)
////			{
////				Player.m_player.turnArmor += m_armorBonus;
////				StartCoroutine(UIManager.m_uiManager.UpdateArmor(Player.m_player.currentArmor + Player.m_player.turnArmor + Player.m_player.tempArmor));
////				UIManager.m_uiManager.SpawnFloatingText("+" + m_armorBonus.ToString(), UIManager.Icon.Armor, Player.m_player.m_playerMesh.transform);
////
////				//Update Effect Stack
////				EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
////				newEffect.m_effectType = EffectsPanel.Effect.EffectType.TurnArmorBonus;
////				newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
////				string desc = m_name + ": " + m_armorBonus.ToString() + "% until next turn.";
////				newEffect.m_description = desc;
////				newEffect.m_spriteName = "Effect_ArmorBonus";
////				newEffect.m_affectedItem = this;
////				EffectsPanel.m_effectsPanel.AddEffect(newEffect);
////			}
////			if (m_attackBonus > 0)
////			{
////				Player.m_player.turnDamage += m_attackBonus;	
////				StartCoroutine(UIManager.m_uiManager.UpdateDamage(Player.m_player.damage + Player.m_player.turnDamage + Player.m_player.tempDamage));
////				UIManager.m_uiManager.SpawnFloatingText("+" + m_attackBonus.ToString(), UIManager.Icon.Armor, Player.m_player.m_playerMesh.transform);
////
////				//Update Effect Stack
////				EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
////				newEffect.m_effectType = EffectsPanel.Effect.EffectType.TurnDamageBonus;
////				newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
////				string desc = m_name + ": " + m_attackBonus.ToString() + "$ until next turn.";
////				newEffect.m_description = desc;
////				newEffect.m_spriteName = "Effect_DamageBonus";
////				newEffect.m_affectedItem = this;
////				EffectsPanel.m_effectsPanel.AddEffect(newEffect);
////			}
////			if (m_actionBonus > 0)
////			{
////				UIManager.m_uiManager.SpawnFloatingText("+" + m_actionBonus.ToString(), UIManager.Icon.Actions, Player.m_player.m_playerMesh.transform);
////
////				Player.m_player.GainActionPoints(m_actionBonus);
////			}
////			if (m_refreshParty)
////			{
////				UIManager.m_uiManager.SpawnFloatingText("Refresh Party", UIManager.Icon.None, Player.m_player.m_playerMesh.transform);
////
////				foreach (Follower thisFollower in GameManager.m_gameManager.followers)
////				{
////					if (thisFollower.followerState == Follower.FollowerState.Spent)
////					{
////						yield return StartCoroutine(thisFollower.ChangeState(Follower.FollowerState.Normal));	
////					}
////				}
////			}
////			
////			if (m_enableCounterAttack)
////			{
////				Player.m_player.doCounterAttack = true;	
////				UIManager.m_uiManager.SpawnFloatingText("Counter Attack", UIManager.Icon.None, Player.m_player.m_playerMesh.transform);
////
////				
////				//Update Effect Stack
////				EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
////				newEffect.m_effectType = EffectsPanel.Effect.EffectType.CounterAttack;
////				newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
////				string desc = "Counter Attack until next turn.";
////				newEffect.m_description = desc;
////				newEffect.m_stackable = false;
////				newEffect.m_spriteName = "Effect_CounterAttack";
////				newEffect.m_affectedItem = this;
////				EffectsPanel.m_effectsPanel.AddEffect(newEffect);
////			}
////		} 
//		else if ((this.HasKeyword(Keyword.Equippable) || this.HasKeyword(Keyword.Skill)) && UIManager.m_uiManager.menuMode == UIManager.MenuMode.None)
//		{
//			if (m_graveHealth)
//			{
//				int healthBonus = 0;
//
//				foreach (GameManager.GraveSlot gs in GameManager.m_gameManager.grave)
//				{
//					if (gs.type == GameManager.GraveSlot.ObjectType.Enemy && gs.enemy != null)
//					{
//						healthBonus ++;
//					}
//				}
//
//				yield return new WaitForSeconds(0.25f);
//				GameObject pFX = AssetManager.m_assetManager.m_particleFX[1];
//				Instantiate(pFX, Player.m_player.m_playerMesh.transform.position, pFX.transform.rotation);
//				UIManager.m_uiManager.SpawnFloatingText("+" + healthBonus.ToString(), UIManager.Icon.Health, Player.m_player.m_playerMesh.transform);
//				Player.m_player.GainHealth(healthBonus);
//				yield return new WaitForSeconds(0.5f);
//			}
//
//			if (m_graveEnergy)
//			{
//				int energyBonus = 0;
//				
//				foreach (GameManager.GraveSlot gs in GameManager.m_gameManager.grave)
//				{
//					if (gs.type == GameManager.GraveSlot.ObjectType.Enemy && gs.enemy != null)
//					{
//						energyBonus += 2;
//					}
//				}
//
//				yield return new WaitForSeconds(0.25f);
//				GameObject pFX = AssetManager.m_assetManager.m_particleFX[1];
//				Instantiate(pFX, Player.m_player.m_playerMesh.transform.position, pFX.transform.rotation);
//				UIManager.m_uiManager.SpawnFloatingText("+" + energyBonus.ToString(), UIManager.Icon.Energy, Player.m_player.m_playerMesh.transform);
//				Player.m_player.GainEnergy(energyBonus);
//				yield return new WaitForSeconds(0.5f);
//			}
//
//			if (m_graveItemDamage)
//			{
//				int damagebonus = 0;
//				foreach (GameManager.GraveSlot gs in GameManager.m_gameManager.grave)
//				{
//					if (gs.type == GameManager.GraveSlot.ObjectType.Item && gs.item != null)
//					{
//						damagebonus += 1;
//					}
//				}
//
//
//				yield return new WaitForSeconds(0.25f);
//
//				UIManager.m_uiManager.SpawnFloatingText("+" + damagebonus.ToString(), UIManager.Icon.MeleeDamage, Player.m_player.m_playerMesh.transform);
//
//				if (damagebonus > 0)
//				{
//					Player.m_player.turnDamage += damagebonus;	
//					StartCoroutine(UIManager.m_uiManager.UpdateDamage(Player.m_player.damage + Player.m_player.turnDamage + Player.m_player.tempDamage));
//					
//					//Update Effect Stack
//					EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
//					newEffect.m_effectType = EffectsPanel.Effect.EffectType.TurnDamageBonus;
//					newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
//					string desc = m_name + ": " + damagebonus.ToString() + "$ until next turn.";
//					newEffect.m_description = desc;
//					newEffect.m_spriteName = "Effect_DamageBonus";
//					newEffect.m_affectedItem = this;
//					EffectsPanel.m_effectsPanel.AddEffect(newEffect);
//				}
//	
//			}
//
//			if (m_rezItems)
//			{
//				foreach (GameManager.GraveSlot gs in GameManager.m_gameManager.grave)
//				{
//					if (gs.type == GameManager.GraveSlot.ObjectType.Item && gs.item != null && GameManager.m_gameManager.inventory.Count < GameManager.m_gameManager.maxBP )
//					{
//						Item rezzedItem = gs.item;
//						// set cost based on current leader
//						if (rezzedItem.m_class == GameManager.m_gameManager.currentFollower.m_followerClass)
//						{
//							rezzedItem.adjustedEnergyCost = 0;
//							rezzedItem.adjustedHealthCost = 0;
//						} else {
//							rezzedItem.adjustedEnergyCost = rezzedItem.m_energyCost;
//							rezzedItem.m_adjustedHealthCost = rezzedItem.m_healthCost;
//						}
//						GameManager.m_gameManager.inventory.Add(rezzedItem);
//						gs.item = null;
//						gs.type = GameManager.GraveSlot.ObjectType.None;
//					}
//				}
//
//				UIManager.m_uiManager.RefreshInventoryMenu();
//			}
//
//			if (m_clearAllWounds)
//			{
//				List<EffectsPanel.Effect> wounds = new List<EffectsPanel.Effect>();
//				foreach (EffectsPanel.Effect e in EffectsPanel.m_effectsPanel.effectStack)
//				{
//					if (e.m_effectType == EffectsPanel.Effect.EffectType.Wound)
//					{
//						wounds.Add(e);
//					}
//				}
//
//				if (wounds.Count > 0)
//				{
//					foreach (EffectsPanel.Effect e in wounds)
//					{
//						EffectsPanel.m_effectsPanel.RemoveEffect(e);
//					}
//				}
//
//				Player.m_player.wounds = 0;
//
//			}
//
//			if (m_clearAllCorruption)
//			{
//				List<EffectsPanel.Effect> corrupt = new List<EffectsPanel.Effect>();
//				foreach (EffectsPanel.Effect e in EffectsPanel.m_effectsPanel.effectStack)
//				{
//					if (e.m_effectType == EffectsPanel.Effect.EffectType.Corrupt)
//					{
//						corrupt.Add(e);
//					}
//				}
//				
//				if (corrupt.Count > 0)
//				{
//					foreach (EffectsPanel.Effect e in corrupt)
//					{
//						EffectsPanel.m_effectsPanel.RemoveEffect(e);
//					}
//				}
//				
//				Player.m_player.corruption = 0;
//			}
//
//			if (m_doSpellbook)
//			{
//				bool itemsAdded = false;
//				//get mage skills out of Grave
//				for (int i=0; i < GameManager.m_gameManager.limboCards.Length; i++)
//				{
//					Item item = GameManager.m_gameManager.limboCards[i];
//					if (item != null && GameManager.m_gameManager.inventory.Count < GameManager.m_gameManager.maxBP)
//					{
//						if (item.m_class == Follower.FollowerClass.Mage)
//						{
//							GameManager.m_gameManager.inventory.Add(item);
//							GameManager.m_gameManager.limboCards[i] = null;
//							itemsAdded = true;
//						}
//					}
//				}
//
//				if (itemsAdded)
//				{
//					UIManager.m_uiManager.RefreshInventoryMenu();
//				}
//			}
//
//			if (m_levelsSkipped > 0)
//			{
////				StartCoroutine (MapManager.m_mapManager.NextLevel(m_levelsSkipped));
//				Card c = Player.m_player.currentCard;
//				if (c.type != Card.CardType.Exit)
//				{
//					c.type = Card.CardType.Exit;
//					c.m_cardMesh.material = ((Card)MapManager.m_mapManager.m_cardTypes[2].transform.GetComponent("Card")).cardMesh.sharedMaterial;
//					UIManager.m_uiManager.m_exitButton.SetActive(true);
//				}
//			}
//
//			if (m_healthBonus > 0)
//			{
//					
//				yield return new WaitForSeconds(0.25f);
//				GameObject pFX = AssetManager.m_assetManager.m_particleFX[1];
//				Instantiate(pFX, Player.m_player.m_playerMesh.transform.position, pFX.transform.rotation);
//				UIManager.m_uiManager.SpawnFloatingText("+" + m_healthBonus.ToString(), UIManager.Icon.Health, Player.m_player.m_playerMesh.transform);
//				Player.m_player.GainHealth(m_healthBonus);
//				yield return new WaitForSeconds(0.5f);
//			}
//
//			if (m_energyBonus > 0)
//			{
//				yield return new WaitForSeconds(0.25f);
//				GameObject pFX = AssetManager.m_assetManager.m_particleFX[1];
//				Instantiate(pFX, Player.m_player.m_playerMesh.transform.position, pFX.transform.rotation);
//				UIManager.m_uiManager.SpawnFloatingText("+" + m_energyBonus.ToString(), UIManager.Icon.Energy, Player.m_player.m_playerMesh.transform);
//				Player.m_player.GainEnergy(m_energyBonus);
//				yield return new WaitForSeconds(0.5f);
//					
//			}
//
//			if (m_curePoison && Player.m_player.currentEffect == GameManager.StatusEffect.Poison)
//			{
//				UIManager.m_uiManager.SpawnFloatingText("Cure Poison", UIManager.Icon.None, Player.m_player.m_playerMesh.transform);
//
//				Player.m_player.ChangeEffectState(GameManager.StatusEffect.None, 0);	
//			}
//
//			if (m_refreshParty)
//			{
//				UIManager.m_uiManager.SpawnFloatingText("Refresh Party", UIManager.Icon.None, Player.m_player.m_playerMesh.transform);
//
//				int rDamage = 0;
//				foreach (Follower thisFollower in GameManager.m_gameManager.followers)
//				{
//					if (thisFollower.followerState == Follower.FollowerState.Spent)
//					{
//						yield return StartCoroutine(thisFollower.ChangeState(Follower.FollowerState.Normal));
//						rDamage ++;
//					}
//				}
//
//				if (rDamage > 0 && m_refreshDamage > 0)
//				{
//					int totalDamage = rDamage * m_refreshDamage;
//					yield return StartCoroutine(Player.m_player.TakeDirectDamage(totalDamage));
//				}
//			}
//
//			if (m_reflectMeleeDamage > 0)
//			{
//				Player.m_player.reflectDamage += m_reflectMeleeDamage;
//				UIManager.m_uiManager.SpawnFloatingText("Reflect", UIManager.Icon.None, Player.m_player.m_playerMesh.transform);
//
//
//				//Update Effect Stack
//				EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
//				newEffect.m_effectType = EffectsPanel.Effect.EffectType.ReflectMeleeDamage;
//				newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
//				string desc = m_name + ": Reflect " + m_reflectMeleeDamage.ToString() + "$ until next turn.";
//				newEffect.m_description = desc;
//				newEffect.m_spriteName = "Effect_ArmorBonus";
//				newEffect.m_affectedItem = this;
//				EffectsPanel.m_effectsPanel.AddEffect(newEffect);
//			}
//
//			if (m_lifeTapBonus > 0)
//			{
//				Player.m_player.lifeTap += m_lifeTapBonus;
//				UIManager.m_uiManager.SpawnFloatingText("Life Tap", UIManager.Icon.None, Player.m_player.m_playerMesh.transform);
//
//
//				//Update Effect Stack
//				EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
//				newEffect.m_effectType = EffectsPanel.Effect.EffectType.LifeTap;
//				newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
//				string desc = m_name + ": Regain " + m_lifeTapBonus.ToString() + "& for each enemy killed";
//				newEffect.m_description = desc;
//				newEffect.m_spriteName = "Effect_CounterAttack";
//				newEffect.m_affectedItem = this;
//				EffectsPanel.m_effectsPanel.AddEffect(newEffect);
//			}
//
//			if (m_soulTapBonus > 0)
//			{
//				Player.m_player.soulTap += m_soulTapBonus;
//				UIManager.m_uiManager.SpawnFloatingText("Soul Tap", UIManager.Icon.None, Player.m_player.m_playerMesh.transform);
//
//
//				//Update Effect Stack
//				EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
//				newEffect.m_effectType = EffectsPanel.Effect.EffectType.SoulTap;
//				newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
//				string desc = m_name + ": Regain " + m_soulTapBonus.ToString() + "# for each enemy killed";
//				newEffect.m_description = desc;
//				newEffect.m_spriteName = "Effect_CounterAttack";
//				newEffect.m_affectedItem = this;
//				EffectsPanel.m_effectsPanel.AddEffect(newEffect);
//			}
//
//			if (m_enableSacrifice)
//			{
//				Player.m_player.doSacrifice = true;
//				UIManager.m_uiManager.SpawnFloatingText("Sacrifice", UIManager.Icon.None, Player.m_player.m_playerMesh.transform);
//
//
//				//Update Effect Stack
//				EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
//				newEffect.m_effectType = EffectsPanel.Effect.EffectType.Sacrifice;
//				newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
//				string desc = m_name + ": Hero Skills drain & instead of #.";
//				newEffect.m_description = desc;
//				newEffect.m_spriteName = "Effect_Poison";
//				newEffect.m_affectedItem = this;
//				newEffect.m_stackable = false;
//				EffectsPanel.m_effectsPanel.AddEffect(newEffect);
//			}
//
//			if (m_enableCover)
//			{
//				Player.m_player.doCover = true;
//				UIManager.m_uiManager.SpawnFloatingText("Cover", UIManager.Icon.None, Player.m_player.m_playerMesh.transform);
//
//
//				//Update Effect Stack
//				EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
//				newEffect.m_effectType = EffectsPanel.Effect.EffectType.Cover;
//				newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
//				string desc = m_name + ": 2% against Ranged Attacks.";
//				newEffect.m_description = desc;
//				newEffect.m_spriteName = "Effect_ArmorBonus";
//				newEffect.m_affectedItem = this;
//				EffectsPanel.m_effectsPanel.AddEffect(newEffect);
//			}
//
//			if (m_doRage && !Player.m_player.berserkerActive)
//			{
//				Player.m_player.berserkerActive = true;
//				UIManager.m_uiManager.SpawnFloatingText("Rage", UIManager.Icon.None, Player.m_player.m_playerMesh.transform);
//
//				//Update Effect Stack
//				EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
//				newEffect.m_effectType = EffectsPanel.Effect.EffectType.Rage;
//				newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
//				string desc = m_name + ": +1 Action when damaged until next turn.";
//				newEffect.m_description = desc;
//				newEffect.m_spriteName = "Effect_CounterAttack";
//				newEffect.m_stackable = false;
//				newEffect.m_affectedItem = this;
//				EffectsPanel.m_effectsPanel.AddEffect(newEffect);
//			}
//
//			if (m_doSoulArmor)
//			{
//				Player.m_player.doSoulArmor = true;
//				UIManager.m_uiManager.SpawnFloatingText("Soul Armor", UIManager.Icon.None, Player.m_player.m_playerMesh.transform);
//
//
//				//Update Effect Stack
//				EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
//				newEffect.m_effectType = EffectsPanel.Effect.EffectType.SoulArmor;
//				newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
//				string desc = m_name + ": Cannot be reduced below 1&.";
//				newEffect.m_description = desc;
//				newEffect.m_spriteName = "Effect_ArmorBonus";
//				newEffect.m_stackable = false;
//				newEffect.m_affectedItem = this;
//				EffectsPanel.m_effectsPanel.AddEffect(newEffect);
//			}
//
//			if (m_trapDestroyRange > 0)
//			{
//				List<GameManager.Direction> directions = new List<GameManager.Direction>();
//				directions.Add(GameManager.Direction.North);
//				directions.Add(GameManager.Direction.South);
//				directions.Add(GameManager.Direction.East);
//				directions.Add(GameManager.Direction.West);
//				List<Card> validCards = MapManager.m_mapManager.GetCardsInRange(Player.m_player.currentCard, m_trapDestroyRange, directions, false, true, false);
//
//				if (validCards.Count > 0)
//				{
//					GameManager.m_gameManager.selectMode = true;
//					yield return StartCoroutine(InputManager.m_inputManager.GetMouseSelection(validCards));	
//					GameManager.m_gameManager.selectMode = false;
//					
//					if (GameManager.m_gameManager.selectedCard != null)
//					{
//						Card c = GameManager.m_gameManager.selectedCard;
//						if (c.type != Card.CardType.Normal && c.type != Card.CardType.Exit && c.type != Card.CardType.Entrance && c.type != Card.CardType.Gate)
//						{
//							//turn this card into a normal card
////							if (currentMap.m_mapType == MapManager.Map.MapType.Cave)
////							{
//								Card newCard = ((Card)MapManager.m_mapManager.m_cardTypes[16].transform.GetComponent("Card"));
//								yield return StartCoroutine(c.ChangeCardType(newCard));
////							} 
//						}
//
//						GameManager.m_gameManager.selectedCard = null;
//					}
//					
//					foreach (Card vc in validCards)
//					{
//						vc.ChangeHighlightState(false);	
//					}
//				}
//			}
//
//			if (m_moveDistance > 0)
//			{
//				List<GameManager.Direction> moveDir = new List<GameManager.Direction>();
//				moveDir.Add(GameManager.Direction.North);
//				moveDir.Add(GameManager.Direction.South);
//				moveDir.Add(GameManager.Direction.East);
//				moveDir.Add(GameManager.Direction.West);
//				List<Card> vcards = MapManager.m_mapManager.GetCardsInRange(Player.m_player.currentCard, m_moveDistance, moveDir, true, true, false);
//				
//				if (vcards.Count > 0)
//				{
//					GameManager.m_gameManager.selectMode = true;
//					yield return StartCoroutine(InputManager.m_inputManager.GetMouseSelection(vcards));	
//					GameManager.m_gameManager.selectMode = false;
//					
//					if (GameManager.m_gameManager.selectedCard != null)
//					{
//						Card c = GameManager.m_gameManager.selectedCard;
//						Card pc = Player.m_player.currentCard;
//
//						if (!c.isOccupied)
//						{
//							GameManager.Direction d = GameManager.Direction.None;
//							//determine the movement direction
//
//							if (c.column > pc.column && c.row == pc.row)
//							{
//								d = GameManager.Direction.East;
//							} else if (c.column < pc.column && c.row == pc.row)
//							{
//								d = GameManager.Direction.West;
//							}else if (c.column == pc.column && c.row < pc.row)
//							{
//								d = GameManager.Direction.North;
//							}else if (c.column == pc.column && c.row > pc.row)
//							{
//								d = GameManager.Direction.South;
//							}
//
//							//move in that direction until selected card is reached
//							if (d != GameManager.Direction.None)
//							{
//								bool atDestination = false;
//								while (!atDestination)
//								{
//									yield return StartCoroutine(Player.m_player.MovePlayer(d));
//
//									if (Player.m_player.currentCard.id == c.id)
//									{
//										atDestination = true;
//									}
//								}
//							}
//						}
//						
//						GameManager.m_gameManager.selectedCard = null;
//					}
//					
//					foreach (Card vc in vcards)
//					{
//						vc.ChangeHighlightState(false);	
//					}
//				}
//			}
//
//			if (m_actionPerHiddenBonus > 0)
//			{
//				int actionBonus = 0;
//				Card[] linkedCards = Player.m_player.currentCard.linkedCards;
//				foreach (Card c in linkedCards)
//				{
//					if (c != null)
//					{
//						if (c.cardState == Card.CardState.Hidden)
//						{
//							actionBonus ++;
//						}
//					}
//				}
//
//				if (actionBonus > 0)
//				{
//					yield return new WaitForSeconds(0.5f);
//					UIManager.m_uiManager.SpawnAbilityName("Insight", Player.m_player.m_playerMesh.transform);
//					yield return new WaitForSeconds(0.5f);
//					UIManager.m_uiManager.SpawnAbilityName("+" + actionBonus.ToString() + "A", Player.m_player.m_playerMesh.transform);
//					Player.m_player.GainActionPoints(actionBonus);
//					yield return new WaitForSeconds(1.0f);
//
//				}
//			}
//
//			if (m_armorBonus > 0)
//			{
//				UIManager.m_uiManager.SpawnFloatingText("+" + m_armorBonus.ToString(), UIManager.Icon.Armor, Player.m_player.m_playerMesh.transform);
//
//				Player.m_player.turnArmor += m_armorBonus;
//				StartCoroutine(UIManager.m_uiManager.UpdateArmor(Player.m_player.currentArmor + Player.m_player.turnArmor + Player.m_player.tempArmor));
//				
//				//Update Effect Stack
//				EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
//				newEffect.m_effectType = EffectsPanel.Effect.EffectType.TurnArmorBonus;
//				newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
//				string desc = m_name + ": " + m_armorBonus.ToString() + "% until next turn.";
//				newEffect.m_description = desc;
//				newEffect.m_spriteName = "Effect_ArmorBonus";
//				newEffect.m_affectedItem = this;
//				EffectsPanel.m_effectsPanel.AddEffect(newEffect);
//			}
//			if (m_attackBonus > 0)
//			{
//				UIManager.m_uiManager.SpawnFloatingText("+" + m_attackBonus.ToString(), UIManager.Icon.MeleeDamage, Player.m_player.m_playerMesh.transform);
//
//				Player.m_player.turnDamage += m_attackBonus;	
//				StartCoroutine(UIManager.m_uiManager.UpdateDamage(Player.m_player.damage + Player.m_player.turnDamage + Player.m_player.tempDamage));
//			
//				//Update Effect Stack
//				EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
//				newEffect.m_effectType = EffectsPanel.Effect.EffectType.TurnDamageBonus;
//				newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
//				string desc = m_name + ": " + m_attackBonus.ToString() + "$ until next turn.";
//				newEffect.m_description = desc;
//				newEffect.m_spriteName = "Effect_DamageBonus";
//				newEffect.m_affectedItem = this;
//				EffectsPanel.m_effectsPanel.AddEffect(newEffect);
//			}
//			if (m_actionBonus > 0)
//			{
//				UIManager.m_uiManager.SpawnFloatingText("+" + m_actionBonus.ToString(), UIManager.Icon.Actions, Player.m_player.m_playerMesh.transform);
//
//				Player.m_player.GainActionPoints(m_actionBonus);
//			}
//			
//			if (m_skipTurn)
//			{
//				UIManager.m_uiManager.SpawnAbilityName("Wait", Player.m_player.transform);
//				yield return new WaitForSeconds(0.5f);
//				Player.m_player.GainActionPoints((Player.m_player.currentActionPoints - 1) * -1);
//				Player.m_player.UseActionPoint();	
//			}
//			
//			if (m_enableCounterAttack)
//			{
//				Player.m_player.doCounterAttack = true;	
//				
//				//Update Effect Stack
//				EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
//				newEffect.m_effectType = EffectsPanel.Effect.EffectType.CounterAttack;
//				newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
//				string desc = "Counter Attack until next turn.";
//				newEffect.m_description = desc;
//				newEffect.m_stackable = false;
//				newEffect.m_spriteName = "Effect_CounterAttack";
//				newEffect.m_affectedItem = this;
//				EffectsPanel.m_effectsPanel.AddEffect(newEffect);
//			}
//			
//			if (m_stun)
//			{
//				List<GameManager.Direction> directions = new List<GameManager.Direction>();
//				directions.Add(GameManager.Direction.North);
//				directions.Add(GameManager.Direction.South);
//				directions.Add(GameManager.Direction.East);
//				directions.Add(GameManager.Direction.West);
//				List<Card> validCards = MapManager.m_mapManager.GetCardsInRange(Player.m_player.currentCard, 1, directions, false, true, false);
//				
//				if (validCards.Count > 0)
//				{
//					GameManager.m_gameManager.selectMode = true;
//					yield return StartCoroutine(InputManager.m_inputManager.GetMouseSelection(validCards));	
//					GameManager.m_gameManager.selectMode = false;
//					
//					if (GameManager.m_gameManager.selectedCard != null)
//					{
//						if (GameManager.m_gameManager.selectedCard.enemy != null)
//						{
//							UIManager.m_uiManager.SpawnAbilityName("Headbutt", Player.m_player.transform);
//							yield return new WaitForSeconds(0.5f);
//
//							Enemy thisEnemy = GameManager.m_gameManager.selectedCard.enemy;
//							thisEnemy.stunDuration = m_stunDuration;
//							UIManager.m_uiManager.SpawnAbilityName("Stunned", thisEnemy.transform);
//							yield return new WaitForSeconds(0.5f);
//						}
//						GameManager.m_gameManager.selectedCard = null;
//					}
//					
//					foreach (Card vc in validCards)
//					{
//						vc.ChangeHighlightState(false);	
//					}
//				}
//			}
//			
//			if (m_knockbackAdjacent)
//			{
//				UIManager.m_uiManager.SpawnAbilityName("Battle Cry", Player.m_player.transform);
//				yield return new WaitForSeconds(0.25f);
//				
//				Card[] linkedCards = Player.m_player.currentCard.linkedCards;
//				for (int i=0; i<linkedCards.Length; i++)
//				{
//					Card thisCard = linkedCards[i];
//					if (thisCard != null)
//					{
//						if (thisCard.enemy != null && thisCard.cardState == Card.CardState.Normal)
//						{
//							GameManager.Direction dir = GameManager.Direction.North;
//							if (i==1)
//							{
//								dir = GameManager.Direction.South;
//							} else if (i==2)
//							{
//								dir = GameManager.Direction.East;	
//							} else if (i==3)
//							{
//								dir = GameManager.Direction.West;	
//							}
//							if (thisCard.enemy.CanBeKnockedBack(dir))
//							{
//								yield return StartCoroutine(thisCard.enemy.DoKnockback(dir));
//							}
//						}
//					}
//				}
//			}
//			
//			if (m_flipAdjacent)
//			{
//				Card[] linkedCards = Player.m_player.currentCard.linkedCards;
//				foreach (Card thisCard in linkedCards)
//				{
//					if (thisCard != null)
//					{
//						if (thisCard.cardState == Card.CardState.Hidden)
//						{
//							Player.m_player.GainEnergy(thisCard.GetEnergyValue());
//							GameManager.m_gameManager.accruedXP += 1;
//
//							if (Player.m_player.currentEnergy < Player.m_player.maxEnergy)
//							{
//								UIManager.m_uiManager.SpawnFloatingText("+1",UIManager.Icon.Energy, thisCard.transform);
//								
//								if (!thisCard.isOccupied)
//								{
//									GameObject pFX = AssetManager.m_assetManager.m_particleFX[1];
//									Instantiate(pFX, thisCard.transform.position, pFX.transform.rotation);
//								}
//							}
//
//							yield return StartCoroutine(thisCard.ChangeCardState(Card.CardState.Normal));
//						}
//					}
//				}
//			}
//			if (m_reverseAdjacent)
//			{
//				Card[] linkedCards = Player.m_player.currentCard.linkedCards;
//				foreach (Card thisCard in linkedCards)
//				{
//					if (thisCard != null)
//					{
//						if (thisCard.cardState == Card.CardState.Hidden)
//						{
//							Player.m_player.GainEnergy(thisCard.GetEnergyValue());
//							GameManager.m_gameManager.accruedXP += 1;
//							
//							if (Player.m_player.currentEnergy < Player.m_player.maxEnergy)
//							{
//								UIManager.m_uiManager.SpawnFloatingText("+1",UIManager.Icon.Energy, thisCard.transform);
//								
//								if (!thisCard.isOccupied)
//								{
//									GameObject pFX = AssetManager.m_assetManager.m_particleFX[1];
//									Instantiate(pFX, thisCard.transform.position, pFX.transform.rotation);
//								}
//							}
//							
//							yield return StartCoroutine(thisCard.ChangeCardState(Card.CardState.Normal));
//						} 
////						else {
////							yield return StartCoroutine(thisCard.ChangeCardState(Card.CardState.Hidden));
////						}
//					}
//				}
//			}
//
//			if (m_adjacentDamage > 0)
//			{
//				UIManager.m_uiManager.SpawnAbilityName("Roundhouse", Player.m_player.transform);
//				yield return new WaitForSeconds(0.25f);
//				
//				Card[] linkedCards = Player.m_player.currentCard.linkedCards;
//				foreach (Card thisCard in linkedCards)
//				{
//					//Debug.Log(thisCard.name);
//					if (thisCard != null)
//					{
//						if (thisCard.cardState != Card.CardState.Hidden && thisCard.enemy != null)
//						{
//							Enemy thisEnemy = thisCard.enemy;
//							yield return StartCoroutine(thisEnemy.TakeDamage(m_adjacentDamage));
//							yield return new WaitForSeconds(0.25f);
//						}
//					}
//				}	
//			}
//			
//			if (m_enableArmorPierce)
//			{
//				UIManager.m_uiManager.SpawnAbilityName("Armor Pierce", Player.m_player.transform);
//				Player.m_player.doArmorPierce = true;	
//				
//				//Update Effect Stack
//				EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
//				newEffect.m_effectType = EffectsPanel.Effect.EffectType.ArmorPierce;
//				newEffect.m_effectDuration = EffectsPanel.Effect.Duration.NextPlayerAttack;
//				string desc = m_name + ": Next attack gains Armor Piercing.";
//				newEffect.m_description = desc;
//				newEffect.m_spriteName = "Effect_ArmorPierce";
//				newEffect.m_stackable = false;
//				newEffect.m_affectedItem = this;
//				EffectsPanel.m_effectsPanel.AddEffect(newEffect);
//			}
//			
//			if (m_rangedAttack)
//			{
//				//Card m_selectedCard = null;
//				
//				List<GameManager.Direction> directions = new List<GameManager.Direction>();
//				directions.Add(GameManager.Direction.North);
//				directions.Add(GameManager.Direction.South);
//				directions.Add(GameManager.Direction.East);
//				directions.Add(GameManager.Direction.West);
//				List<Card> validCards = MapManager.m_mapManager.GetCardsInRange(Player.m_player.currentCard, (2 + Player.m_player.currentCard.siteRangeBonus), directions, true, true, false);
//				
//				if (validCards.Count > 0)
//				{
//					GameManager.m_gameManager.selectMode = true;
//					yield return StartCoroutine(InputManager.m_inputManager.GetMouseSelection(validCards));	
//					GameManager.m_gameManager.selectMode = false;
//					
//					if (GameManager.m_gameManager.selectedCard != null)
//					{
//						if (GameManager.m_gameManager.selectedCard.enemy != null)
//						{
//							Enemy thisEnemy = GameManager.m_gameManager.selectedCard.enemy;
//							//Player.m_player.GainEnergy(m_energyCost * -1);
//							StartCoroutine(thisEnemy.TakeDamage(m_attackDamage));
//						}
//						GameManager.m_gameManager.selectedCard = null;
//					}
//					
//					foreach (Card vc in validCards)
//					{
//						vc.ChangeHighlightState(false);	
//					}
//				//	m_selectMode = false;
//					//thisF.ChangeState(Follower.FollowerState.Spent);
//				}
//			}
//			
//			int attackBonus = 0;
//			foreach (GameManager.Dice thisDice in m_attackBonusDice)
//			{
//				int numSides = 4;
//				switch (thisDice)
//				{
//				case GameManager.Dice.D1:
//					numSides = 1;
//					break;
//				case GameManager.Dice.D6:
//					numSides = 6;
//					break;
//				case GameManager.Dice.D8:
//					numSides = 8;
//					break;
//				case GameManager.Dice.D10:
//					numSides = 10;
//					break;
//				case GameManager.Dice.D12:
//					numSides = 12;
//					break;
//				case GameManager.Dice.D20:
//					numSides = 20;
//					break;
//				}
//				
//				int diceRoll = Random.Range(1, numSides+1);
//				attackBonus += diceRoll;
//			}
//
//			m_attackDiceRoll = attackBonus;
//			
////			int damage = Player.m_player.tempDamage;
////			damage += attackBonus;
////			Player.m_player.tempDamage = damage;
//			Player.m_player.turnDamage += attackBonus;
//			int newDamage = Player.m_player.damage + Player.m_player.turnDamage + Player.m_player.tempDamage;
//			StartCoroutine(UIManager.m_uiManager.UpdateDamage(newDamage));
//			
//			if (attackBonus > 0)
//			{
//				UIManager.m_uiManager.SpawnFloatingText("+" + attackBonus.ToString(), UIManager.Icon.MeleeDamage, Player.m_player.m_playerMesh.transform);
//
//				//Update Effect Stack
//				EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
//				newEffect.m_effectType = EffectsPanel.Effect.EffectType.AttackDamageBonus;
//				newEffect.m_effectDuration = m_duration;
//				string desc = m_name + ": " + attackBonus.ToString() + "$ to next attack.";
//				newEffect.m_description = desc;
//				newEffect.m_spriteName = "Effect_DamageBonus";
//				newEffect.m_affectedItem = this;
//				EffectsPanel.m_effectsPanel.AddEffect(newEffect);
//			}
//			
//			
//			int armorBonus = 0;
//			foreach (GameManager.Dice thisDice in m_armorBonusDice)
//			{
//				int numSides = 4;
//				switch (thisDice)
//				{
//				case GameManager.Dice.D1:
//					numSides = 1;
//					break;
//				case GameManager.Dice.D6:
//					numSides = 6;
//					break;
//				case GameManager.Dice.D8:
//					numSides = 8;
//					break;
//				case GameManager.Dice.D10:
//					numSides = 10;
//					break;
//				case GameManager.Dice.D12:
//					numSides = 12;
//					break;
//				case GameManager.Dice.D20:
//					numSides = 20;
//					break;
//				}
//				
//				int diceRoll = Random.Range(1, numSides);
//				armorBonus += diceRoll;
//
//			}
//
//			m_armorDiceRoll = armorBonus;
//
////			int armor = Player.m_player.tempArmor;
////			armor += armorBonus;
////			Player.m_player.tempArmor = armor;
//			Player.m_player.turnArmor += armorBonus;
//			StartCoroutine(UIManager.m_uiManager.UpdateArmor(Player.m_player.currentArmor + Player.m_player.turnArmor + Player.m_player.tempArmor));
//			
//			if (armorBonus > 0)
//			{
//				UIManager.m_uiManager.SpawnFloatingText("+" + armorBonus.ToString(), UIManager.Icon.Armor, Player.m_player.m_playerMesh.transform);
//
//				//Update Effect Stack
//				EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
//				newEffect.m_effectType = EffectsPanel.Effect.EffectType.AttackArmorBonus;
//				newEffect.m_effectDuration = m_duration;
//				string desc = m_name + ": " + armorBonus.ToString() + "% for next attack.";
//				newEffect.m_description = desc;
//				newEffect.m_spriteName = "Effect_ArmorBonus";
//				newEffect.m_affectedItem = this;
//				EffectsPanel.m_effectsPanel.AddEffect(newEffect);
//			}
//			
//			
//			if (!this.HasKeyword(Keyword.Consumeable))
//			{
//				Debug.Log("SENDING CHANGE TO SPENT STATE");
//				ChangeState(ItemState.Spent);
//			} else {
//
//				if (m_energyCost > 0) { 
//
//					if (Player.m_player.doSacrifice)
//					{
//						if (m_enableSacrifice)
//						{
//							Player.m_player.GainEnergy (m_adjustedEnergyCost * -1); 
//						} else {
//							yield return StartCoroutine(Player.m_player.TakeDirectDamage(m_adjustedEnergyCost));
//						}
//					} else {
//						Player.m_player.GainEnergy (m_adjustedEnergyCost * -1); 
//					}
//				}
//				if (m_healthCost > 0) { yield return StartCoroutine(Player.m_player.TakeDirectDamage(m_adjustedHealthCost));}
//
//				bool skillCard = true;
//				UICard card = PartyCards.m_partyCards.GetSkillCard(this);
//
//				if (this.HasKeyword(Keyword.Skill) && this.HasKeyword(Keyword.Consumeable))
//				{
//					foreach (GameObject go in UIManager.m_uiManager.cardList)
//					{
//						UICard c = (UICard)go.GetComponent("UICard");
//						if (c.itemData == this)
//						{
//							card = c;
//							skillCard = false;
//							break;
//						}
//					}
//				}
//
//				if (m_graveBomb)
//				{
//					yield return new WaitForSeconds(0.25f);
//					yield return StartCoroutine(Player.m_player.TakeDirectDamage(1));
//					UIManager.m_uiManager.SpawnFloatingText("-1", UIManager.Icon.Health, Player.m_player.m_playerMesh.transform);
//					yield return new WaitForSeconds(0.5f);
//				}
//
//				// move card to grave
//				float t = 0;
//				float time = 0.5f;
//				Vector3 startPos = card.transform.position;
//				Vector3 localStartPos = card.transform.localPosition;
//				Vector3 startScale = card.transform.localScale;
////				Debug.Log(startPos);
//				while (t < time)
//				{
//					t += Time.deltaTime;;
//					Vector3 nPos = Vector3.Lerp(startPos, UIManager.m_uiManager.m_backpackButton.transform.position , t / time);
//					Vector3 newScale = Vector3.Lerp(startScale, Vector3.one * 0.25f, t / time);
//					card.transform.position = nPos;
//					card.transform.localScale = newScale;
//					yield return null;
//				}
//
//				card.transform.localPosition = localStartPos;
//				card.transform.localScale = startScale;
//
//				if (skillCard)
//				{
//					card.m_followerData.currentSkills --;
//					PartyCards.m_partyCards.RemoveSkill(this);
//				} else {
//
//					for (int i=0; i < GameManager.m_gameManager.inventory.Count; i++)
//					{
//						Item invItem = GameManager.m_gameManager.inventory[i];
//						if (invItem == this)
//						{
//							GameManager.m_gameManager.inventory.RemoveAt(i);
//							break;
//						}
//					}
//
//					GameManager.m_gameManager.SendToGrave(this);
//					UIManager.m_uiManager.RefreshInventoryMenu();
//				}
//
//			}
//			
//		}
//		yield return null;
//	}
//	
//	public void ChangeState (ItemState newState)
//	{
//		ItemState oldState = m_itemState;
//		m_itemState = newState;	
//
//		//get card asset
//		//Vector3 cardPos;
//		Color spentPortraitColor = new Color(0.4f, 0.4f, 0.4f, 1);
//		Color spentTextColor = new Color(0.7f, 0.7f, 0.7f, 1);
//
//		if (this.HasKeyword(Keyword.Skill))
//		{
//			if (newState == ItemState.Spent)
//			{
//				Debug.Log("SETTING ITEM STATE TO SPENT");
//				if (PartyCards.m_partyCards.hoveredSkill != null)
//				{
//					if (PartyCards.m_partyCards.hoveredSkill.m_itemData == this)
//					{
//						PartyCards.m_partyCards.ClearSelection();
//					}
//				} else {
//					UICard skillCard = PartyCards.m_partyCards.GetSkillCard(this);
////					skillCard.m_portrait.spriteName = "Card_Back03";
////					skillCard.m_nameUI.gameObject.SetActive(false);
//					skillCard.Deactivate();
//				}
//			} else if (newState == ItemState.Normal)
//			{
//				m_attackDiceRoll = 0;
//				m_armorDiceRoll = 0;
//
//				if (PartyCards.m_partyCards.hoveredSkill != null)
//				{
//
//				} else {
//					UICard skillCard = PartyCards.m_partyCards.GetSkillCard(this);
//					skillCard.m_nameUI.gameObject.SetActive (true);
//					skillCard.m_abilityUI.gameObject.SetActive (true);
//					skillCard.m_rankUI.gameObject.SetActive (true);
//					skillCard.m_healthIcon.gameObject.SetActive (true);
//					skillCard.m_portrait.spriteName = skillCard.m_itemData.m_portraitSpriteName;
//				}
//			}
//		} else {
////			foreach (UICard iCard in UIManager.m_uiManager.m_equipSlots)
////			{
////				if (iCard.m_itemData == this)
////				{
////					//cardPos = iCard.transform.localPosition;
////					
////					if (newState == ItemState.Normal)
////					{
////
////						if (EquipCards.m_equipCards.hoveredCard == null)
////						{
////							iCard.m_portrait.spriteName = iCard.m_itemData.m_fullPortraitSpriteName;
////							iCard.m_shortCutUI.gameObject.SetActive(true);
////						}
////						if (EquipCards.m_equipCards.hoveredCard != null)
////						{
////							if (EquipCards.m_equipCards.hoveredCard.itemData != null)
////							{
////								if ( this != EquipCards.m_equipCards.hoveredCard.itemData)
////								{
////									iCard.m_portrait.spriteName = iCard.m_itemData.m_fullPortraitSpriteName;
////									iCard.m_shortCutUI.gameObject.SetActive(true);
////								}
////							}
////						}
////						
////					} else if (newState == ItemState.Spent)
////					{
////
////						if (EquipCards.m_equipCards.hoveredCard == null)
////						{
////							iCard.m_portrait.spriteName = "Card_Back03";
////							iCard.m_shortCutUI.gameObject.SetActive(false);
////						}
////						if (EquipCards.m_equipCards.hoveredCard != null)
////						{
////							if (EquipCards.m_equipCards.hoveredCard.itemData != null)
////							{
////								if ( this != EquipCards.m_equipCards.hoveredCard.itemData)
////								{
////									iCard.m_portrait.spriteName = "Card_Back03";
////									iCard.m_shortCutUI.gameObject.SetActive(false);
////								}
////							}
////						}
////					}
////					break;
////				}
////			}
//		}
//
//		if (!HasKeyword(Keyword.Consumeable) && (m_energyCost > 0 || m_healthCost > 0) && m_duration == EffectsPanel.Effect.Duration.EndOfTurn)
//		{
//			AddInvisEffect();
//		}
//	}
//	
//	public void AddInvisEffect()
//	{
//		EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
//		newEffect.m_effectType = EffectsPanel.Effect.EffectType.None;
//		newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
//		newEffect.m_visible = false;
//		newEffect.m_affectedItem = this;
//		EffectsPanel.m_effectsPanel.AddEffect(newEffect);
//	}
//
//	public IEnumerator ItemDestroyed ()
//	{
//		Debug.Log ("DESTROYING ITEM");
//		if (m_attackDiceRoll > 0)
//		{
//			int damage = Player.m_player.tempDamage;
//			damage -= m_attackDiceRoll;
//			Player.m_player.tempDamage = damage;
//			int newDamage = Player.m_player.damage + Player.m_player.turnDamage + Player.m_player.tempDamage;
//			StartCoroutine(UIManager.m_uiManager.UpdateDamage(newDamage));
//
//			m_attackDiceRoll = 0;
//
//			EffectsPanel.m_effectsPanel.RemoveEffect(this);
//		}
//
//		if (m_armorDiceRoll > 0)
//		{
//			int armor = Player.m_player.tempArmor;
//			armor -= m_armorDiceRoll;
//			Player.m_player.tempArmor = armor;
//			StartCoroutine(UIManager.m_uiManager.UpdateArmor(Player.m_player.currentArmor + Player.m_player.turnArmor + Player.m_player.tempArmor));
//
//			m_armorDiceRoll = 0;
//
//			EffectsPanel.m_effectsPanel.RemoveEffect(this);
//		}
//
//		if (m_attackBonus > 0)
//		{
//			Player.m_player.turnDamage -= m_attackBonus;	
//			StartCoroutine(UIManager.m_uiManager.UpdateDamage(Player.m_player.damage + Player.m_player.turnDamage + Player.m_player.tempDamage));
//
//			EffectsPanel.m_effectsPanel.RemoveEffect(this);
//		}
//
//		if (m_armorBonus > 0)
//		{
//			int armor = Player.m_player.turnArmor;
//			armor -= m_armorBonus;
//			Player.m_player.turnArmor = armor;
//			StartCoroutine(UIManager.m_uiManager.UpdateArmor(Player.m_player.currentArmor + Player.m_player.turnArmor + Player.m_player.tempArmor));
//
//			EffectsPanel.m_effectsPanel.RemoveEffect(this);
//		}
//
//		if (m_enableCounterAttack)
//		{
//			Player.m_player.doCounterAttack = false;
//			EffectsPanel.m_effectsPanel.RemoveEffect(this);
//		}
//
//		if (m_lifeTapBonus > 0)
//		{
//			Player.m_player.lifeTap = 0;
//			EffectsPanel.m_effectsPanel.RemoveEffect(this);
//		}
//
//		if (m_doRage)
//		{
//			Player.m_player.berserkerActive = false;
//			EffectsPanel.m_effectsPanel.RemoveEffect(this);
//		}
//
//		if (m_enableSacrifice)
//		{
//			Player.m_player.doSacrifice = false;
//			EffectsPanel.m_effectsPanel.RemoveEffect(this);
//		}
//
//		if (m_doSoulArmor)
//		{
//			Player.m_player.doSoulArmor = false;
//			EffectsPanel.m_effectsPanel.RemoveEffect(this);
//		}
//
//		if (m_soulTapBonus > 0)
//		{
//			Player.m_player.soulTap = 0;
//			EffectsPanel.m_effectsPanel.RemoveEffect(this);
//		}
//
//		if (m_reflectMeleeDamage > 0)
//		{
//			Player.m_player.reflectDamage = 0;
//			EffectsPanel.m_effectsPanel.RemoveEffect(this);
//		}
//
//		if (m_enableCover)
//		{
//			Player.m_player.doCover = false;
//			EffectsPanel.m_effectsPanel.RemoveEffect(this);
//		}
//
//		if (m_enablePoison)
//		{
//			Player.m_player.doPoisonAttack = false;
//			EffectsPanel.m_effectsPanel.RemoveEffect(this);
//		}
//
//		if (m_enableArmorPierce)
//		{
//			Player.m_player.doArmorPierce = false;
//			EffectsPanel.m_effectsPanel.RemoveEffect(this);
//		}
//			
//			yield return null;
//	}
	
//	public ItemState itemState {get{return m_itemState;}}
	public int id {get{return m_id;}}
	public int adjustedEnergyCost {get{return m_adjustedEnergyCost;} set {m_adjustedEnergyCost = value;}}
	public int adjustedHealthCost {get{return m_adjustedHealthCost;} set {m_adjustedHealthCost = value;}}
	public UICard card {get{return m_card;}set{m_card = value;}}
//	public Follower attachedFollower {get {return m_attachedFollower;} set {m_attachedFollower = value;}}

}
