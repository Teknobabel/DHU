using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Follower : MonoBehaviour {
	
	public enum FollowerType
	{
		Brand,
		Telina,
		August,
		Jin,
		Elf,
		Ranger,
		Seer,
		Mystic,
		Barbarian,
		Knight,
		Berserker,
		Pyromage,
		Dragoon,
		Samurai,
		Lagomorph,
		Fencer,
		Wrestler,
		Succubus,
		Dancer,
		Thief,
		Psychic,
		Monk,
	}
	
	public enum FollowerClass
	{
		None,
		Warrior,
		Mage,
		Duelist,
		Rogue,
		Demon,
		Brand,
		August,
		Telina,
		Jin,
		Limbo,
	}
	
	public enum FollowerState
	{
		Normal,
		Spent,
		Stunned,
	}

	public enum HeroBadge
	{
		PassiveMeleeDamage_plus1,
		MaxHealth_plus1,
		MaxEnergy_plus1,
		PassiveArmor_plus1,
		ItemLimit_plus1,
		AbilityCost_minus1,
		SkillBonus_Melee_plus1,
		SkillBonus_Health_plus1,
		None,
		Item_ShortSword,
		Item_Shield,
		Item_Spellbook,
		Item_Crossbow,
		SkillBonus_Range_plus1,
		SkillBonus_RangeDamage_plus1,
		SkillBonus_ArmorPierce,
		Item_Adrenaline,
		Item_Headbutt,
		Item_Dodge,
		Item_Crystal,
		Item_Shard,
		Item_BleedingEdge,
		Item_Venom,
		Item_Shovel,
		Item_Panacea,
		Item_BattleCry,
		Item_SpikedArmor,
		Item_Insight,
		SkillBonus_Actions_plus1,
		SkillBonus_CounterAttack_plus1,
		SkillBonus_KnockbackDamage_plus1,
		SkillBonus_Armor_plus1,
		Item_Roundhouse,
		Item_Lifetap,
		Item_Dash,
		Item_TakeCover,
	}

	[System.Serializable]
	public class HeroBadgeSlot
	{
		public HeroBadge
			m_leftBadge,
			m_rightBadge;
	}
	

	[System.Serializable]
	public class Level
	{
		public int 
			m_damageMod = 0,
			m_healthMod = 0,
			m_energyMod = 0,
			m_armorMod = 0,
			m_abilityCostMod = 0,
			m_abilityEffectMod = 0,
			m_abilityRangeMod = 0;
	}
	[System.Serializable]
	public class Deck
	{
		public GameObject[] m_levelCards;
	}

	public FollowerType
		m_followerType;
	
	public FollowerClass
		m_followerClass = FollowerClass.None;
	
	public string
		m_abilityText = "Text",
		m_nameText = "Name";
	
	public string
		m_portraitSpriteName = "Portrait",
		m_fullPortraitSpriteName = "Portrait_Full";
	
	public int
		m_startingDamage = 0,
		m_startingHealth = 1,
		m_startingEnergy = 0,
		m_startingArmor = 0,
		m_minDifficulty = 0,
		m_abilityCost = 0,
		m_abilityEffect = 0,
		m_abilityRange = 0;

	
	public Level[] m_levelTable;

	public HeroBadgeSlot[]
		m_heroBadgeTable;

	public Deck[]
		m_deck;
	
	public GameObject
		m_followerMesh,
		m_shadowMesh;
	
	private bool
		m_isLocked = true,
		m_inflictedThisTurn = false,
		m_doArmorPierce = false;
	
	private int
		m_currentHealth = 20,
		m_currentLevel = 1,
		m_currentXP = 0,
		m_maxXP = 500,
		m_stunDuration = 0,
		m_baseAbilityCost = 0,
		m_baseAbilityEffect = 0,
		m_baseAbilityRange = 0,
		m_id = -1,
		m_maxSkills = 1,
		m_currentSkills = 0,
		m_badgeBonus_SkillCost = 0,
		m_badgeBonus_SkillEffectBonus = 0,
		m_badgeBonus_ItemLimit = 0,
		m_badgeBonus_PassiveDamage = 0,
		m_badgeBonus_PassiveHealth = 0,
		m_badgeBonus_PassiveEnergy = 0,
		m_badgeBonus_PassiveArmor = 0,
		m_badgeBonus_Range = 0;

	private float
		m_XPBonus = 1.0f;

	private Card
		m_currentCard = null;
	
	private Level
		m_level;
	
	private FollowerState
		m_followerState = FollowerState.Normal;

	private int[]
		m_heroBadgeStates;

	private List<HeroBadge> m_activeBadges = new List<HeroBadge>();

	void Awake ()
	{
		m_baseAbilityCost = m_abilityCost;
		m_baseAbilityEffect = m_abilityEffect;
		m_baseAbilityRange = m_abilityRange;

		if (GameManager.m_gameManager != null)
		{
			m_maxSkills += GameManager.m_gameManager.itemBonus;
		}
	}
	
	public void SetLevel ()
	{
		//Debug
//		Level newLevel = new Level();
//		newLevel.m_damageMod = 2;
//		newLevel.m_healthMod = 5;
//		m_level = newLevel;
		//m_currentLevel = 5;
		
		if (m_currentLevel <= m_levelTable.Length)
		{
//			Debug.Log(m_currentLevel);
			m_level = m_levelTable[m_currentLevel-1];	
		}
		
		if (m_currentLevel == 1)
		{
			m_maxXP = 1000;
		}
		else if (m_currentLevel == 2)
		{
			m_maxXP = 1500;
		} else if (m_currentLevel == 3)
		{
			m_maxXP = 2500;
		} else if (m_currentLevel == 4)
		{
			m_maxXP = 5000;
		}
		else if (m_currentLevel == 5)
		{
			m_maxXP = 0;
			m_currentXP = 0;	
		}

		//update badge states if needed
		if (m_heroBadgeStates != null)
		{
			for (int i=0; i < m_heroBadgeStates.Length; i++)
			{
				//Debug.Log(m_nameText);
				if (i < m_currentLevel)
				{
					if (m_heroBadgeStates[i] == 2)
					{
						m_heroBadgeStates[i] = 3;
					}
				}
			}
		} else {Debug.Log("NO HERO BADGE STATES FOUND");}
	}
	
	public void TakeDamage (int amt)
	{
		m_currentHealth = Mathf.Clamp(m_currentHealth - amt, 0, 20);
		if (m_currentHealth <= 0)
		{
			if (DHeart.m_dHeart.isBeating)
			{
				DHeart.m_dHeart.StopHeartBeat();	
			}
			
			m_currentCard.follower = null;
			Destroy(this.gameObject);	
		}
	}
	
	public IEnumerator DoTurn ()
	{
		if (m_followerState == FollowerState.Stunned && !m_inflictedThisTurn)
		{
			m_stunDuration --;
			if (m_stunDuration <= 0)
			{
				m_stunDuration = 0;
				yield return StartCoroutine(ChangeState(FollowerState.Normal));
			}
			UIManager.m_uiManager.UpdateFollowerEffectGUI(this, m_stunDuration);
		} else if (m_followerState == FollowerState.Stunned && m_inflictedThisTurn)
		{
			m_inflictedThisTurn = false;
		}
		
		yield return null;
	}
	
	public IEnumerator DoStun (int stunDuration)
	{

//		if (m_stunDuration <= 0)
//		{
//			m_inflictedThisTurn = true;	
//			if (PartyCards.m_partyCards.hoveredCard != null)
//			{
//				if (PartyCards.m_partyCards.hoveredCard.m_followerData != null)
//				{
//					if (PartyCards.m_partyCards.hoveredCard.m_followerData == this)
//					{
//						PartyCards.m_partyCards.ClearSelection();	
//					}
//				}
//			}
//			
//			//Update Effect Stack
//			EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
//			newEffect.m_effectType = EffectsPanel.Effect.EffectType.PlayerStun;
//			newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
//			newEffect.m_affectedFollower = this;
//			string desc = m_nameText + ": Stunned";
//			newEffect.m_description = desc;
//			newEffect.m_spriteName = "Effect_Stun";
//			EffectsPanel.m_effectsPanel.AddEffect(newEffect);
//			
//		}
//		m_stunDuration += stunDuration;
//		yield return StartCoroutine(ChangeState(FollowerState.Stunned));


		//check if stun effect already exists
		bool stunExists = false;
		EffectsPanel.Effect thisStun = null;
		foreach (EffectsPanel.Effect e in EffectsPanel.m_effectsPanel.effectStack) {
			if (e.m_effectType == EffectsPanel.Effect.EffectType.PlayerStun)
			{
				thisStun = e;
				stunExists = true;
				break;
			}
		}

		//if so, increment it and update UI
		if (stunExists) {
			Player.m_player.stunDuration += stunDuration;
			string desc = "Stunned: Swapping Leaders drains 2# - " + Player.m_player.stunDuration.ToString() + " Turns remaining";

			thisStun.m_description = desc;
		} else {

			//if not, add stun effect
			Player.m_player.stunDuration = stunDuration;

			//Update Effect Stack
			EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
			newEffect.m_effectType = EffectsPanel.Effect.EffectType.PlayerStun;
			newEffect.m_effectDuration = EffectsPanel.Effect.Duration.None;
			newEffect.m_affectedFollower = this;
			string desc = "Stunned: Swapping Leaders drains 2# - " + Player.m_player.stunDuration.ToString() + " Turns remaining";
			newEffect.m_description = desc;
			newEffect.m_spriteName = "Effect_Stun";
			EffectsPanel.m_effectsPanel.AddEffect(newEffect);
		}
		yield return null;	
	}
	
	public IEnumerator ChangeState (FollowerState newState)
	{
		FollowerState oldState = m_followerState;
		m_followerState = newState;	
		//Debug.Log("CHANGING STATE: " + newState);
		//get card asset
		//Vector3 cardPos;
		Color spentPortraitColor = new Color(0.4f, 0.4f, 0.4f, 1);
		Color spentTextColor = new Color(0.7f, 0.7f, 0.7f, 1);
		foreach (GameObject fCard in UIManager.m_uiManager.m_followerCards)
		{
			UICard follower = (UICard)fCard.GetComponent("UICard");
			if (follower.m_followerData.m_followerType == m_followerType)
			{
				
				if (oldState == FollowerState.Stunned)
				{
					follower.m_portrait.spriteName = follower.m_followerData.m_fullPortraitSpriteName;
					follower.m_shortCutUI.gameObject.SetActive(true);
				}
				
				if (newState == FollowerState.Normal)
				{
//					follower.m_portrait.color = Color.white;
//					follower.m_nameUI.color = Color.white;
//					follower.m_abilityUI.color = Color.white;

					if (PartyCards.m_partyCards.hoveredCard == null)
					{
						follower.m_portrait.spriteName = follower.m_followerData.m_fullPortraitSpriteName;
						follower.m_shortCutUI.gameObject.SetActive(true);
					}
					else if (PartyCards.m_partyCards.hoveredCard != null)
					{
						if (PartyCards.m_partyCards.hoveredCard.m_followerData != null)
						{

							if ( this != PartyCards.m_partyCards.hoveredCard.m_followerData)
							{
								follower.m_portrait.spriteName = follower.m_followerData.m_fullPortraitSpriteName;
								follower.m_shortCutUI.gameObject.SetActive(true);
							} else if (!PartyCards.m_partyCards.hoveredCard.m_abilityUI.gameObject.activeSelf){
								follower.m_portrait.spriteName = follower.m_followerData.m_fullPortraitSpriteName;
								follower.m_shortCutUI.gameObject.SetActive(true);
							}
//							else if (this == PartyCards.m_partyCards.hoveredCard.m_followerData && !PartyCards.m_partyCards.hoveredCard.m_abilityUI.gameObject.activeSelf )
//							{
//								follower.m_portrait.spriteName = follower.m_followerData.m_fullPortraitSpriteName;
//								follower.m_shortCutUI.gameObject.SetActive(true);
//							}
						}
					}

				} else if (newState == FollowerState.Spent)
				{
//					follower.m_portrait.color = spentPortraitColor;
//					follower.m_nameUI.color = spentTextColor;
//					follower.m_abilityUI.color = spentTextColor;

					if (PartyCards.m_partyCards.hoveredCard == null)
					{
						follower.m_portrait.spriteName = "Card_Back03";
						follower.m_shortCutUI.gameObject.SetActive(false);
					}
					if (PartyCards.m_partyCards.hoveredCard != null)
					{
						if (PartyCards.m_partyCards.hoveredCard.m_followerData != null)
						{
//							if ( this != PartyCards.m_partyCards.hoveredCard.m_followerData)
//							{
//								follower.m_portrait.spriteName = "Card_Back03";
//								follower.m_shortCutUI.gameObject.SetActive(false);
//							}

							if ( this == PartyCards.m_partyCards.hoveredCard.m_followerData)
							{
								follower.m_portrait.spriteName = "Card_Back03";
								follower.m_nameUI.gameObject.SetActive(false);
								follower.m_rankUI.gameObject.SetActive(false);
								follower.m_abilityUI.gameObject.SetActive(false);
								follower.m_miscOBJ[0].gameObject.SetActive(false);
								follower.m_healthIcon.gameObject.SetActive(false);

								BoxCollider bc = (BoxCollider)follower.gameObject.GetComponent("BoxCollider");
								bc.size = PartyCards.m_partyCards.boundsNormalSize;
								bc.center = PartyCards.m_partyCards.boundsNormalPos;

								follower.transform.localScale = PartyCards.m_partyCards.unselectedScale;
							}
						}
					}

					// any unspent items are spent
//					UICard[] sCards = PartyCards.m_partyCards.GetSkillCards(this);
//					foreach (UICard c in sCards)
//					{
//						if (c.itemData != null)
//						{
//							if (c.itemData.itemState != Item.ItemState.Spent)
//							{
//								c.itemData.ChangeState(Item.ItemState.Spent);
//								c.itemData.AddInvisEffect();
//							}
//						}
//					}

				} else if (newState == FollowerState.Stunned)
				{
					follower.m_portrait.spriteName = "Card_Back03";
					follower.m_nameUI.gameObject.SetActive(false);
					follower.m_abilityUI.gameObject.SetActive(false);
					follower.m_shortCutUI.gameObject.SetActive(false);
					UIManager.m_uiManager.UpdateFollowerEffectGUI(this, m_stunDuration);
				}
				yield break;
			}
		}
		
		yield return null;
	}
	
	public void GainXP (int amount)
	{
		if (m_currentLevel < 5)
		{
			m_currentXP += amount;
			if (m_currentXP >= m_maxXP)
			{
				m_currentLevel ++;
				m_currentXP -= m_maxXP;
				SetLevel();
				
				PartyCards.m_partyCards.UpdateCard(this);
			}
		}
	}

	public void UpdateHeroBadges ()
	{
//		Debug.Log (m_nameText);
		m_badgeBonus_SkillCost = 0;
		m_badgeBonus_SkillEffectBonus = 0;
		m_badgeBonus_ItemLimit = 0;
		m_badgeBonus_PassiveDamage = 0;
		m_badgeBonus_PassiveHealth = 0;
		m_badgeBonus_PassiveEnergy = 0;
		m_badgeBonus_PassiveArmor = 0;
		m_badgeBonus_Range = 0;
		m_doArmorPierce = false;

		List<HeroBadge> badges = new List<HeroBadge> ();

		for (int i = 0; i < m_heroBadgeStates.Length; i++)
		{
			if (i < m_heroBadgeTable.Length)
			{
				HeroBadgeSlot hBS = m_heroBadgeTable[i];
				if (m_heroBadgeStates[i] == 0)
				{
					badges.Add(hBS.m_leftBadge);
				} else if (m_heroBadgeStates[i] == 1)
				{
					badges.Add(hBS.m_rightBadge);
				}
			}
		}
		m_activeBadges = badges;

		//update hero stats
		foreach (HeroBadge hB in m_activeBadges)
		{
			switch (hB)
			{
			case HeroBadge.AbilityCost_minus1:
				m_badgeBonus_SkillCost --;
				break;
			case HeroBadge.SkillBonus_Melee_plus1:
				m_badgeBonus_SkillEffectBonus ++;
				break;
			case HeroBadge.SkillBonus_Health_plus1:
				m_badgeBonus_SkillEffectBonus ++;
				break;
			case HeroBadge.ItemLimit_plus1:
				m_badgeBonus_ItemLimit ++;
				break;
			case HeroBadge.PassiveArmor_plus1:
				m_badgeBonus_PassiveArmor ++;
				break;
			case HeroBadge.PassiveMeleeDamage_plus1:
				m_badgeBonus_PassiveDamage ++;
				break;
			case HeroBadge.MaxHealth_plus1:
				m_badgeBonus_PassiveHealth ++;
				break;
			case HeroBadge.MaxEnergy_plus1:
				m_badgeBonus_PassiveEnergy ++;
				break;
			case HeroBadge.SkillBonus_Range_plus1:
				m_badgeBonus_Range ++;
				break;
			case HeroBadge.SkillBonus_RangeDamage_plus1:
				m_badgeBonus_SkillEffectBonus ++;
				break;
			case HeroBadge.SkillBonus_ArmorPierce:
				m_doArmorPierce = true;
				break;
			case HeroBadge.SkillBonus_Actions_plus1:
				m_badgeBonus_SkillEffectBonus ++;
				break;
			case HeroBadge.SkillBonus_CounterAttack_plus1:
				m_badgeBonus_SkillEffectBonus ++;
				break;
			case HeroBadge.SkillBonus_KnockbackDamage_plus1:
				m_badgeBonus_SkillEffectBonus ++;
				break;
			case HeroBadge.SkillBonus_Armor_plus1:
				m_badgeBonus_SkillEffectBonus ++;
				break;
			}

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
	
	public int currentLevel {get{return m_currentLevel;}set{m_currentLevel = value;}}
	public Level levelModifiers {get{return m_level;}}
	public int currentXP {get{return m_currentXP;}set{m_currentXP = value;}}
	public int maxXP {get{return m_maxXP;}set{m_maxXP = value;}}
	public bool isLocked {get{return m_isLocked;}set{m_isLocked = value;}}
	public FollowerState followerState {get{return m_followerState;}}
	public int abilityCost {get{return m_baseAbilityCost + m_badgeBonus_SkillCost;}}
	public int baseAbilityCost {get{return m_baseAbilityCost;} set{m_baseAbilityCost = value;}}
	public int abilityEffect {get{return m_baseAbilityEffect + m_badgeBonus_SkillEffectBonus;}}
	public int baseAbilityEffect {get{return m_baseAbilityEffect;} set{m_baseAbilityEffect = value;}}
	public int baseAbilityRange {get{return m_baseAbilityRange;} set{m_baseAbilityRange = value;}}
	public int abilityRange {get{return m_baseAbilityRange + m_badgeBonus_Range;}}
	public int stunDuration {get {return m_stunDuration;}}
	public int id {get {return m_id;} set{m_id = value;}}
	public int maxSkills {get {return m_maxSkills + m_badgeBonus_ItemLimit;} set{m_maxSkills = value;}}
	public int currentSkills {get {return m_currentSkills;} set{m_currentSkills = value;}}
	public int[] heroBadgeStates {get{return m_heroBadgeStates;} set{m_heroBadgeStates = value; UpdateHeroBadges();}}
	public int badgeBonus_PassiveHealth {get{return m_badgeBonus_PassiveHealth;}}
	public int badgeBonus_PassiveArmor {get{return m_badgeBonus_PassiveArmor;}}
	public int badgeBonus_PassiveDamage {get{return m_badgeBonus_PassiveDamage;}}
	public int badgeBonus_PassiveEnergy {get{return m_badgeBonus_PassiveEnergy;}}
	public List<HeroBadge> activeBadges {get{return m_activeBadges;}}
	public bool doArmorPierce {get {return m_doArmorPierce;}}
	public float XPBonus {get{return m_XPBonus;} set{m_XPBonus = value;}}

}
