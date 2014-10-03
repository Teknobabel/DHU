using UnityEngine;
using System.Collections;

public class HeroBadge : MonoBehaviour {

	public UISprite 
		m_sprite,
		m_priceBG;

	public UILabel m_label;

	private Follower.HeroBadge
		m_badgeType = Follower.HeroBadge.None;

	private StoreBadge.State m_state = StoreBadge.State.BadgeSlot;

	private MouseHover m_hoverText;

	private int 
		m_ID = -99,
		m_slot = -99,
		m_hoveredSlot = -99;


	// Use this for initialization
	void Awake () {
		m_hoverText = (MouseHover)this.GetComponent ("MouseHover");
	}

	public void Initialize (Follower.HeroBadge badgeType, StoreBadge.State state, int ID, int slot)
	{
//		if (m_label != null)
//		{
//			m_label.text = label;
//		}
		m_ID = ID;
		m_slot = slot;
//		m_price = price;
		SetBadge (badgeType);
		ChangeState (state);
	}

	void OnMouseEnter()
	{
		if (m_state == StoreBadge.State.UnEquipped && HeroMenu.m_heroMenu.selectedFollower != null)
		{
			Follower f = HeroMenu.m_heroMenu.selectedFollower;
			int[] heroBadgeStates = f.heroBadgeStates;
			m_hoveredSlot = heroBadgeStates[m_ID];
			heroBadgeStates[m_ID] = m_slot;
			f.heroBadgeStates = heroBadgeStates;
			
			for (int i=0; i < SettingsManager.m_settingsManager.gameProgress.Count; i++)
			{
				GameState.ProgressState thisCharState = SettingsManager.m_settingsManager.gameProgress[i];
				if (thisCharState.m_followerType == f.m_followerType)
				{
					thisCharState.m_badgeLevel1 = heroBadgeStates[0];
					thisCharState.m_badgeLevel2 = heroBadgeStates[1];
					thisCharState.m_badgeLevel3 = heroBadgeStates[2];
					thisCharState.m_badgeLevel4 = heroBadgeStates[3];
					thisCharState.m_badgeLevel5 = heroBadgeStates[4];
					SettingsManager.m_settingsManager.gameProgress[i] = thisCharState;
					
					HeroMenu.m_heroMenu.m_heroCard.SetCard(f, false);
					PartyCards.m_partyCards.UpdatePassiveText(HeroMenu.m_heroMenu.m_heroCard, f, thisCharState.m_level);
					i = 99;
				}
			}
		}
	}

	void OnMouseExit()
	{
		if (m_state == StoreBadge.State.UnEquipped && HeroMenu.m_heroMenu.selectedFollower != null)
		{
			Follower f = HeroMenu.m_heroMenu.selectedFollower;
			int[] heroBadgeStates = f.heroBadgeStates;
			heroBadgeStates[m_ID] = m_hoveredSlot;
			m_hoveredSlot = -99;
			f.heroBadgeStates = heroBadgeStates;
			
			for (int i=0; i < SettingsManager.m_settingsManager.gameProgress.Count; i++)
			{
				GameState.ProgressState thisCharState = SettingsManager.m_settingsManager.gameProgress[i];
				if (thisCharState.m_followerType == f.m_followerType)
				{
					thisCharState.m_badgeLevel1 = heroBadgeStates[0];
					thisCharState.m_badgeLevel2 = heroBadgeStates[1];
					thisCharState.m_badgeLevel3 = heroBadgeStates[2];
					thisCharState.m_badgeLevel4 = heroBadgeStates[3];
					thisCharState.m_badgeLevel5 = heroBadgeStates[4];
					SettingsManager.m_settingsManager.gameProgress[i] = thisCharState;
					
					HeroMenu.m_heroMenu.m_heroCard.SetCard(f, false);
					PartyCards.m_partyCards.UpdatePassiveText(HeroMenu.m_heroMenu.m_heroCard, f, thisCharState.m_level);
					i = 99;
				}
			}
		}
	}

	void OnClick () {
		Debug.Log ("HERO BADGE CLICKED");

		if (m_state == StoreBadge.State.UnEquipped)
		{
			// unequipp sister badge if equipped
			HeroMenu.HeroBadgeSlot bS = HeroMenu.m_heroMenu.m_badgeTable[m_ID];
			int oSlot = 0;
			if (m_slot == 0)
			{
				oSlot = 1;
			}
			HeroBadge hB = bS.m_badgeSlot[oSlot];
			if (hB.m_state == StoreBadge.State.Equipped)
			{
				hB.ChangeState(StoreBadge.State.UnEquipped);
			}

			// equip badge
			ChangeState(StoreBadge.State.Equipped);
			if (HeroMenu.m_heroMenu.selectedFollower != null)
			{
				Follower f = HeroMenu.m_heroMenu.selectedFollower;
				int[] heroBadgeStates = f.heroBadgeStates;
				heroBadgeStates[m_ID] = m_slot;
				f.heroBadgeStates = heroBadgeStates;

				for (int i=0; i < SettingsManager.m_settingsManager.gameProgress.Count; i++)
				{
					GameState.ProgressState thisCharState = SettingsManager.m_settingsManager.gameProgress[i];
					if (thisCharState.m_followerType == f.m_followerType)
					{
						thisCharState.m_badgeLevel1 = heroBadgeStates[0];
						thisCharState.m_badgeLevel2 = heroBadgeStates[1];
						thisCharState.m_badgeLevel3 = heroBadgeStates[2];
						thisCharState.m_badgeLevel4 = heroBadgeStates[3];
						thisCharState.m_badgeLevel5 = heroBadgeStates[4];
						SettingsManager.m_settingsManager.gameProgress[i] = thisCharState;

						HeroMenu.m_heroMenu.m_heroCard.SetCard(f, false);
						PartyCards.m_partyCards.UpdatePassiveText(HeroMenu.m_heroMenu.m_heroCard, f, thisCharState.m_level);
						i = 99;
					}
				}
			}

		}
	}

	private void ChangeState (StoreBadge.State newState)
	{
		StoreBadge.State oldState = m_state;
//		UIButtonScale bs = (UIButtonScale)this.gameObject.GetComponent("UIButtonScale");
//		UIButton b = (UIButton)this.gameObject.GetComponent("UIButton");

		switch (newState)
		{
		case StoreBadge.State.Equipped:
			m_sprite.spriteName = "Grphc_DHeart01";
			m_sprite.color = Color.grey;
			m_priceBG.color = Color.grey;
			m_label.color = Color.grey;
			m_priceBG.gameObject.SetActive(true);
			m_label.gameObject.SetActive(true);
			break;
		case StoreBadge.State.UnEquipped:
			m_sprite.spriteName = "Grphc_DHeart01";
			m_sprite.color = Color.white;
			m_priceBG.color = Color.white;
			m_label.color = Color.white;
			m_priceBG.gameObject.SetActive(true);
			m_label.gameObject.SetActive(true);
			//m_label.color = m_startTextColor;
			
//			if (oldState == StoreBadge.State.Locked)
//			{
//				bs.enabled = true;
//				b.enabled = true;
//				//m_label.color = m_startTextColor;
//				m_priceBG.gameObject.SetActive(true);
//				m_label.gameObject.SetActive(true);
//				BoxCollider bc = (BoxCollider)this.GetComponent("BoxCollider");
//				bc.enabled = true;
//			}
			break;
		case StoreBadge.State.Locked:
			m_sprite.color = Color.white;
			m_priceBG.color = Color.white;
			m_label.color = Color.white;
			m_sprite.spriteName = "BadgeSlot01";
			m_priceBG.gameObject.SetActive (false);
			m_label.gameObject.SetActive (false);
			m_hoverText.m_text = "Gain Levels to unlock this Badge.";
			break;
		}

		m_state = newState;
	}

	public void ClearBadge (){
		m_sprite.spriteName = "BadgeSlot01";
		//m_priceLabel.text = "";
		//m_ID = -1;
		m_priceBG.gameObject.SetActive (false);
		m_label.gameObject.SetActive (false);
		m_hoverText.m_text = "Gain Levels to unlock this Badge.";
		
//		UIButtonScale bs = (UIButtonScale)this.gameObject.GetComponent("UIButtonScale");
//		bs.enabled = false;
	}

	private void SetBadge (Follower.HeroBadge badgeType)
	{
		switch (badgeType)
		{
		case Follower.HeroBadge.AbilityCost_minus1:
			m_label.text = "Skill Refine";
			m_hoverText.m_text = "Reduce the cost to use the Hero's Skill by 1.";
			break;
		case Follower.HeroBadge.ItemLimit_plus1:
			m_label.text = "Item Boost";
			m_hoverText.m_text = "Increase the number of items this Hero can have equipped by 1.";
			break;
		case Follower.HeroBadge.PassiveArmor_plus1:
			m_label.text = "Armor Boost";
			m_hoverText.m_text = "Increase the party's Armor stat by 1.";
			break;
		case Follower.HeroBadge.MaxEnergy_plus1:
			m_label.text = "Energy Boost";
			m_hoverText.m_text = "Increase the party's maximum Energy by 1.";
			break;
		case Follower.HeroBadge.MaxHealth_plus1:
			m_label.text = "Health Boost";
			m_hoverText.m_text = "Increase the party's maximum Health by 1.";
			break;
		case Follower.HeroBadge.PassiveMeleeDamage_plus1:
			m_label.text = "Damage Boost";
			m_hoverText.m_text = "Increase the party's Melee Damage by 1.";
			break;
		case Follower.HeroBadge.SkillBonus_Health_plus1:
			m_label.text = "Skill Boost";
			m_hoverText.m_text = "Increase the amount of Health August's Skill restores by 1.";
			break;
		case Follower.HeroBadge.SkillBonus_Melee_plus1:
			m_label.text = "Skill Boost";
			m_hoverText.m_text = "Increase Brand's Melee Damage Skill bonus by 1.";
			break;
		case Follower.HeroBadge.Item_ShortSword:
			m_label.text = "Short Sword";
			m_hoverText.m_text = "Start with a Short Sword in your Backpack.";
			break;
		case Follower.HeroBadge.Item_Shield:
			m_label.text = "Shield";
			m_hoverText.m_text = "Start with a Shield in your Backpack.";
			break;
		case Follower.HeroBadge.Item_Spellbook:
			m_label.text = "Spellbook";
			m_hoverText.m_text = "Start with a Spellbook in your Backpack.";
			break;
		case Follower.HeroBadge.Item_Crossbow:
			m_label.text = "Crossbow";
			m_hoverText.m_text = "Start with a Crossbow in your Backpack.";
			break;
		case Follower.HeroBadge.SkillBonus_Range_plus1:
			m_label.text = "Eagle Eye";
			m_hoverText.m_text = "Increase Telina's Range by 1.";
			break;
		case Follower.HeroBadge.SkillBonus_RangeDamage_plus1:
			m_label.text = "Piercing";
			m_hoverText.m_text = "Increase Telina's Ranged Damage by 1.";
			break;
		case Follower.HeroBadge.SkillBonus_ArmorPierce:
			m_label.text = "Armor Piercing";
			m_hoverText.m_text = "Telina's ranged attacks gain Armor Piercing";
			break;
		case Follower.HeroBadge.Item_Adrenaline:
			m_label.text = "Adrenaline";
			m_hoverText.m_text = "Start with the Adrenaline Skill in your Backpack.";
			break;
		case Follower.HeroBadge.Item_Headbutt:
			m_label.text = "Headbutt";
			m_hoverText.m_text = "Start with the Headbutt Skill in your Backpack.";
			break;
		case Follower.HeroBadge.Item_Dodge:
			m_label.text = "Dodge";
			m_hoverText.m_text = "Start with the Dodge Skill in your Backpack.";
			break;
		case Follower.HeroBadge.Item_Crystal:
			m_label.text = "Crystal";
			m_hoverText.m_text = "Start with a Crystal in your Backpack.";
			break;
		case Follower.HeroBadge.Item_Shard:
			m_label.text = "Shard";
			m_hoverText.m_text = "Start with a Shard in your Backpack.";
			break;
		case Follower.HeroBadge.Item_BleedingEdge:
			m_label.text = "Bleeding Edge";
			m_hoverText.m_text = "Start with the Bleeding Edge Skill in your Backpack.";
			break;
		case Follower.HeroBadge.Item_Venom:
			m_label.text = "Venom";
			m_hoverText.m_text = "Start with the Venom Skill in your Backpack.";
			break;
		case Follower.HeroBadge.Item_Shovel:
			m_label.text = "Shovel";
			m_hoverText.m_text = "Start with a Shovel in your Backpack.";
			break;
		case Follower.HeroBadge.Item_Panacea:
			m_label.text = "Panacea";
			m_hoverText.m_text = "Start with a Panacea in your Backpack.";
			break;
		case Follower.HeroBadge.Item_BattleCry:
			m_label.text = "Battle Cry";
			m_hoverText.m_text = "Start with the Battle Cry Skill in your Backpack.";
			break;
		case Follower.HeroBadge.Item_SpikedArmor:
			m_label.text = "Spiked Armor";
			m_hoverText.m_text = "Start with Spiked Armor in your Backpack.";
			break;
		case Follower.HeroBadge.Item_Insight:
			m_label.text = "Insight";
			m_hoverText.m_text = "Start with the Insight Skill in your Backpack.";
			break;
		case Follower.HeroBadge.SkillBonus_Actions_plus1:
			m_label.text = "Actions Boost";
			m_hoverText.m_text = "Jin grants an additional Action when using his Skill.";
			break;
		case Follower.HeroBadge.SkillBonus_CounterAttack_plus1:
			m_label.text = "Counter Boost";
			m_hoverText.m_text = "Plexx deals an additional damage when using his Skill.";
			break;
		case Follower.HeroBadge.SkillBonus_KnockbackDamage_plus1:
			m_label.text = "Knockback Boost";
			m_hoverText.m_text = "Haku deals 1 additional damage when using his Skill.";
			break;
		case Follower.HeroBadge.SkillBonus_Armor_plus1:
			m_label.text = "Armor Boost";
			m_hoverText.m_text = "Increase Ark's Armor bonus by 1.";
			break;
		case Follower.HeroBadge.Item_Roundhouse:
			m_label.text = "Roundhouse";
			m_hoverText.m_text = "Start with the Roundhouse Skill in your Backpack.";
			break;
		case Follower.HeroBadge.Item_Lifetap:
			m_label.text = "Lifetap";
			m_hoverText.m_text = "Start with the Lifetap Skill in your Backpack.";
			break;
		case Follower.HeroBadge.Item_Dash:
			m_label.text = "Dash";
			m_hoverText.m_text = "Start with the Dash Skill in your Backpack.";
			break;
		case Follower.HeroBadge.Item_TakeCover:
			m_label.text = "Take Cover";
			m_hoverText.m_text = "Start with the Take Cover Skill in your Backpack.";
			break;
		}

	}
	

}
