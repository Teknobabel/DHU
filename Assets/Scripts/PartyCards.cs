using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyCards : MonoBehaviour {
	
	public UICard[]
	m_party;

	public List<UICard[]>
				m_skills = new List<UICard[]> ();
	
	private UICard
		m_hoveredCard,
		m_hoveredSkillCard;
	
	private Vector3
		m_selectedScale = new Vector3(0.94f,0.94f,0.94f),
		m_unselectedScale = new Vector3(0.44f,0.44f,0.44f),
		m_boundsNormalPos = new Vector3(15,166.9f,0),
		m_boundsNormalSize = new Vector3(204.3f,311.3f,10),
		m_boundsHoveredPos = new Vector3(15,76.7f,0),
		m_boundsHoveredSize = new Vector3(102.4f,152.6f,10),
		m_skillboundsHoveredSize = new Vector3(204.3f, 100.2f, 10),
		m_skillboundsHoveredPos = new Vector3(-13.2f, 156.4f, 0),
		m_skillboundsNormalSize = new Vector3(204.3f, 44, 10),
		m_skillboundsNormalPos = new Vector3(-13.2f, 305, 0);
	
	private float
		m_offset = 100;
	
	public static PartyCards
		m_partyCards;
	
	void Awake ()
	{
		m_partyCards = this;
	}

	// Use this for initialization
	void Start () {

	}

//	public void Initialize ()
//	{
//		//set up party cards
//		foreach (UICard c in m_party)
//		{
//			if (c.m_followerData != null)
//			{
//				for (int i=0; i < c.skillCards.Length ; i++)
//				{
//					UICard s = c.skillCards[i];
//					if (i < 1 + GameManager.m_gameManager.itemBonus)
//					{
//						//disable everything but collision, set collider to normal size/position
//						BoxCollider bc = (BoxCollider)s.gameObject.GetComponent("BoxCollider");
//						bc.size = m_skillboundsNormalSize;
//						bc.center = m_skillboundsNormalPos;
//						bc.enabled = true;
//
//						s.m_nameUI.gameObject.SetActive (false);
//						s.m_abilityUI.gameObject.SetActive (false);
//						s.m_rankUI.gameObject.SetActive (false);
//						s.m_healthIcon.gameObject.SetActive (false);
//						s.m_portrait.gameObject.SetActive (false);
//						s.gameObject.SetActive(true);
//					}
//				}
//			}
//		}
//	}

	public void SkillCardHovered (UICard hoveredCard)
	{

		if (m_hoveredSkillCard == null)
		{
			m_hoveredSkillCard = hoveredCard;

			if (hoveredCard.itemData == null)
			{
				hoveredCard.m_portrait.spriteName = "Card_Empty";
				hoveredCard.m_portrait.gameObject.SetActive(true);
			} else {
				BringSkillCardForward(m_hoveredSkillCard);
			}

		} else if (m_hoveredSkillCard != hoveredCard)
		{
			ClearSkillSelection();
			m_hoveredSkillCard = hoveredCard;
			//if (m_ho
//			if (m_hoveredSkillCard.itemData == null && hoveredCard.itemData == null)
//			{
				// return currently hovered card to normal state
				

			if (hoveredCard.itemData == null)
			{
				hoveredCard.m_portrait.spriteName = "Card_Empty";
				hoveredCard.m_portrait.gameObject.SetActive(true);
			} else {

				// bring forward new card

				
				BringSkillCardForward(m_hoveredSkillCard);
			}
//			}

		}
//		}
	}

	public void BringSkillCardForward (UICard skill)
	{
		// show full card regardless of item state
		skill.m_nameUI.gameObject.SetActive (true);
		skill.m_abilityUI.gameObject.SetActive (true);
		skill.m_rankUI.gameObject.SetActive (true);
		skill.m_healthIcon.gameObject.SetActive (true);
		skill.m_portrait.spriteName = skill.m_itemData.m_portraitSpriteName;

//		if (skill.itemData.m_energyCost > 0)
//		{
			skill.m_miscOBJ[0].gameObject.SetActive(true);
//		} else if (skill.itemData.HasKeyword(Item.Keyword.Consumeable))
//		{
//
//		}
		
		skill.m_portrait.depth += 300;
		skill.m_nameUI.depth += 300;
		skill.m_rankUI.depth += 300;
		skill.m_abilityUI.depth += 300;
		skill.m_shortCutUI.depth += 300;
		skill.m_passive01UI.depth += 300;
		skill.m_passive02UI.depth += 300;
		skill.m_damageIcon.depth += 300;
		skill.m_healthUI.depth += 300;
		skill.m_healthIcon.depth += 300;

		}

	public void ClearSkillSelection ()
	{
		Debug.Log ("CLEAR SKILL SELECTION");
		if (m_hoveredSkillCard != null)
		{
//			BoxCollider bc = (BoxCollider)m_hoveredSkillCard.gameObject.GetComponent("BoxCollider");
//			bc.size = m_skillboundsNormalSize;
//			bc.center = m_skillboundsNormalPos;

			if (m_hoveredSkillCard.m_itemData != null)
			{
//				if (m_hoveredSkillCard.m_itemData.itemState == Item.ItemState.Spent)
//				{
//					m_hoveredSkillCard.Deactivate();
//				}

//				if (m_hoveredSkillCard.itemData.m_energyCost > 0)
//				{
					m_hoveredSkillCard.m_miscOBJ[0].gameObject.SetActive(false);
//				}

				m_hoveredSkillCard.m_portrait.depth -= 300;
				m_hoveredSkillCard.m_nameUI.depth -= 300;
				m_hoveredSkillCard.m_rankUI.depth -= 300;
				m_hoveredSkillCard.m_abilityUI.depth -= 300;
				m_hoveredSkillCard.m_shortCutUI.depth -= 300;
				m_hoveredSkillCard.m_passive01UI.depth -= 300;
				m_hoveredSkillCard.m_passive02UI.depth -= 300;
				m_hoveredSkillCard.m_damageIcon.depth -= 300;
				m_hoveredSkillCard.m_healthUI.depth -= 300;
				m_hoveredSkillCard.m_healthIcon.depth -= 300;
			} else {
				m_hoveredSkillCard.m_portrait.gameObject.SetActive(false);
			}

			m_hoveredSkillCard = null;
		}
	}
	
	public void CardHovered (GameObject hoveredCard)
	{
//		if (m_hoveredSkillCard != null)
//		{
//			ClearSkillSelection();
//		}

		if (m_hoveredCard == null) // if was no card being hovered over last frame
		{
			UICard uiCard = (UICard)hoveredCard.transform.GetComponent("UICard");
			
			if (uiCard.m_followerData != null)
			{
				if (uiCard.m_followerData.followerState == Follower.FollowerState.Stunned)
				{
					return;
				}
			}
			
			m_hoveredCard = uiCard;
			
			if (m_hoveredCard.m_followerData == null) 
			{
				m_hoveredCard.m_portrait.gameObject.SetActive(true);
				m_hoveredCard.m_portrait.spriteName = "Card_Empty";
			} else 
			{
				if (m_hoveredCard.m_followerData.followerState != Follower.FollowerState.Stunned)
				{

					uiCard.m_portrait.spriteName = uiCard.m_followerData.m_portraitSpriteName;
					uiCard.m_nameUI.gameObject.SetActive(true);
					uiCard.m_abilityUI.gameObject.SetActive(true);
					uiCard.m_rankUI.gameObject.SetActive(true);
//					uiCard.m_passive01UI.gameObject.SetActive(true);
//					uiCard.m_passive02UI.gameObject.SetActive(true);
					uiCard.m_shortCutUI.gameObject.SetActive(false);
					uiCard.m_miscOBJ[0].gameObject.SetActive(true);
					uiCard.m_healthIcon.gameObject.SetActive(true);

					BoxCollider bc = (BoxCollider)uiCard.gameObject.GetComponent("BoxCollider");
					bc.size = m_boundsHoveredSize;
					bc.center = m_boundsHoveredPos;
					
					hoveredCard.transform.localScale = m_selectedScale;

					uiCard.m_portrait.depth += 100;
					uiCard.m_nameUI.depth += 100;
					uiCard.m_rankUI.depth += 100;
					uiCard.m_abilityUI.depth += 100;
					uiCard.m_shortCutUI.depth += 100;
					uiCard.m_passive01UI.depth += 100;
					uiCard.m_passive02UI.depth += 100;
					uiCard.m_damageIcon.depth += 100;
					uiCard.m_healthUI.depth += 100;
					uiCard.m_healthIcon.depth += 100;

					//enable skill card hitboxes
					foreach (UICard c in uiCard.skillCards)
					{
						if (c.itemData != null)
						{
							BoxCollider bc1 = (BoxCollider)c.GetComponent("BoxCollider");
							bc1.enabled = true;
						}
					}
				
				}
			}
			
		} else if (m_hoveredCard.gameObject != hoveredCard.gameObject) // if the card currently being hovered is different than last frame
		{
			
			if (m_hoveredCard.m_followerData == null)
			{
				m_hoveredCard.m_portrait.gameObject.SetActive(false);
			} else {
				
				if (m_hoveredCard.m_followerData.followerState != Follower.FollowerState.Stunned)
				{
					if (m_hoveredCard.m_followerData.followerState == Follower.FollowerState.Spent)
					{
						m_hoveredCard.m_portrait.spriteName = "Card_Back03";
						m_hoveredCard.m_shortCutUI.gameObject.SetActive(false);
					} else if (m_hoveredCard.m_followerData.followerState == Follower.FollowerState.Normal)
					{
						m_hoveredCard.m_portrait.spriteName = m_hoveredCard.m_followerData.m_fullPortraitSpriteName;
						m_hoveredCard.m_shortCutUI.gameObject.SetActive(true);
					}

					//m_hoveredCard.m_portrait.spriteName = m_hoveredCard.m_followerData.m_fullPortraitSpriteName;
					m_hoveredCard.transform.localScale = m_unselectedScale;
					m_hoveredCard.m_nameUI.gameObject.SetActive(false);
					m_hoveredCard.m_abilityUI.gameObject.SetActive(false);
					m_hoveredCard.m_rankUI.gameObject.SetActive(false);
//					m_hoveredCard.m_passive01UI.gameObject.SetActive(false);
//					m_hoveredCard.m_passive02UI.gameObject.SetActive(false);
					m_hoveredCard.m_miscOBJ[0].gameObject.SetActive(false);
					m_hoveredCard.m_healthIcon.gameObject.SetActive(false);

					BoxCollider bc = (BoxCollider)m_hoveredCard.gameObject.GetComponent("BoxCollider");
					bc.size = m_boundsNormalSize;
					bc.center = m_boundsNormalPos;

					m_hoveredCard.m_portrait.depth -= 100;
					m_hoveredCard.m_nameUI.depth -= 100;
					m_hoveredCard.m_rankUI.depth -= 100;
					m_hoveredCard.m_abilityUI.depth -= 100;
					m_hoveredCard.m_shortCutUI.depth -= 100;
					m_hoveredCard.m_passive01UI.depth -= 100;
					m_hoveredCard.m_passive02UI.depth -= 100;
					m_hoveredCard.m_damageIcon.depth -= 100;
					m_hoveredCard.m_healthUI.depth -= 100;
					m_hoveredCard.m_healthIcon.depth -= 100;
					
//					//disable skill card hitboxes
//					foreach (UICard c in m_hoveredCard.skillCards)
//					{
//						if (c.itemData != null)
//						{
//							BoxCollider bc2 = (BoxCollider)c.GetComponent("BoxCollider");
//							bc2.enabled = false;
////							bc2.size = m_skillboundsHoveredSize;
////							bc2.center = m_skillboundsHoveredPos;
//							}
//					}
					
				} else {
					m_hoveredCard = null;	
				}
				
				
			}
			
			
			
			UICard uiCard = (UICard)hoveredCard.transform.GetComponent("UICard");
			if (uiCard.m_followerData == null)
			{
				m_hoveredCard = uiCard;
				m_hoveredCard.m_portrait.gameObject.SetActive(true);
				m_hoveredCard.m_portrait.spriteName = "Card_Empty";
			} else {
				if (uiCard.m_followerData.followerState != Follower.FollowerState.Stunned)
				{
					m_hoveredCard = uiCard;
					m_hoveredCard.m_portrait.spriteName = uiCard.m_followerData.m_portraitSpriteName;
					m_hoveredCard.m_nameUI.gameObject.SetActive(true);
					m_hoveredCard.m_abilityUI.gameObject.SetActive(true);
					m_hoveredCard.m_rankUI.gameObject.SetActive(true);
//					m_hoveredCard.m_passive01UI.gameObject.SetActive(true);
//					m_hoveredCard.m_passive02UI.gameObject.SetActive(true);
					m_hoveredCard.m_shortCutUI.gameObject.SetActive(false);
					m_hoveredCard.m_miscOBJ[0].gameObject.SetActive(true);
					m_hoveredCard.m_healthIcon.gameObject.SetActive(true);
					
					hoveredCard.transform.localScale = m_selectedScale;

					BoxCollider bc = (BoxCollider)m_hoveredCard.gameObject.GetComponent("BoxCollider");
					bc.size = m_boundsHoveredSize;
					bc.center = m_boundsHoveredPos;

					m_hoveredCard.m_portrait.depth += 100;
					m_hoveredCard.m_nameUI.depth += 100;
					m_hoveredCard.m_rankUI.depth += 100;
					m_hoveredCard.m_abilityUI.depth += 100;
					m_hoveredCard.m_shortCutUI.depth += 100;
					m_hoveredCard.m_passive01UI.depth += 100;
					m_hoveredCard.m_passive02UI.depth += 100;
					m_hoveredCard.m_damageIcon.depth += 100;
					m_hoveredCard.m_healthUI.depth += 100;
					m_hoveredCard.m_healthIcon.depth += 100;
					
					///enable skill card hitboxes
					foreach (UICard c in m_hoveredCard.skillCards)
					{
						if (c.itemData != null)
						{
							BoxCollider bc3 = (BoxCollider)c.GetComponent("BoxCollider");
							bc3.enabled = true;
//							bc3.size = m_boundsHoveredSize;
//							bc3.center = m_boundsHoveredPos;
							}
					}
					
				} else {
					m_hoveredCard = null;	
				}
			}
		}
	}
	
	public void ClearSelection ()
	{
		if (m_hoveredCard != null)
		{
			if (m_hoveredCard.m_followerData == null)
			{
				m_hoveredCard.m_portrait.gameObject.SetActive(false);
			} else {
				if (m_hoveredCard.m_followerData.followerState != Follower.FollowerState.Stunned)
				{
					if (m_hoveredCard.m_followerData.followerState == Follower.FollowerState.Spent)
					{
						m_hoveredCard.m_portrait.spriteName = "Card_Back03";
						m_hoveredCard.m_shortCutUI.gameObject.SetActive(false);
					} else if (m_hoveredCard.m_followerData.followerState == Follower.FollowerState.Normal)
					{
						m_hoveredCard.m_portrait.spriteName = m_hoveredCard.m_followerData.m_fullPortraitSpriteName;
						m_hoveredCard.m_shortCutUI.gameObject.SetActive(true);
					}
					//m_hoveredCard.m_portrait.spriteName = m_hoveredCard.m_followerData.m_fullPortraitSpriteName;
					m_hoveredCard.transform.localScale = m_unselectedScale;
					m_hoveredCard.m_nameUI.gameObject.SetActive(false);
					m_hoveredCard.m_abilityUI.gameObject.SetActive(false);
					m_hoveredCard.m_rankUI.gameObject.SetActive(false);
//					m_hoveredCard.m_passive01UI.gameObject.SetActive(false);
//					m_hoveredCard.m_passive02UI.gameObject.SetActive(false);

					m_hoveredCard.m_miscOBJ[0].gameObject.SetActive(false);
					m_hoveredCard.m_healthIcon.gameObject.SetActive(false);

					BoxCollider bc = (BoxCollider)m_hoveredCard.gameObject.GetComponent("BoxCollider");
					bc.size = m_boundsNormalSize;
					bc.center = m_boundsNormalPos;

					m_hoveredCard.m_portrait.depth -= 100;
					m_hoveredCard.m_nameUI.depth -= 100;
					m_hoveredCard.m_rankUI.depth -= 100;
					m_hoveredCard.m_abilityUI.depth -= 100;
					m_hoveredCard.m_shortCutUI.depth -= 100;
					m_hoveredCard.m_passive01UI.depth -= 100;
					m_hoveredCard.m_passive02UI.depth -= 100;
					m_hoveredCard.m_damageIcon.depth -= 100;
					m_hoveredCard.m_healthUI.depth -= 100;
					m_hoveredCard.m_healthIcon.depth -= 100;
				
					//disable skill card hitboxes
					foreach (UICard c in m_hoveredCard.skillCards)
					{
						BoxCollider bc2 = (BoxCollider)c.GetComponent("BoxCollider");
						bc2.enabled = false;
//						bc2.size = m_boundsNormalSize;
//						bc2.center = m_boundsNormalPos;
					}
				}
			}
			m_hoveredCard = null;

			if (m_hoveredSkillCard != null)
			{
				ClearSkillSelection();
			}
		}
	}
	
	public void AddFollower (Follower newFollower)
	{
//		if (UIManager.m_uiManager.menuMode == UIManager.MenuMode.CharSelect)
//		{
//			//Follower hasn't been instantiated yet, get stats from saved data
//			foreach (GameState.ProgressState progressState in GameManager.m_gameManager.gameProgress)
//			{
//				if (newFollower.m_followerType == progressState.m_followerType)
//				{
//					//found this follower's saved data, update the card with this
//					foreach (UICard card in m_party)
//					{
//						if (card.m_followerData == null)
//						{
//							// blank card found
//							card.m_portrait.gameObject.SetActive(true);
//							card.m_followerData = newFollower;
//							card.m_nameUI.text = newFollower.m_nameText;
//							card.m_portrait.spriteName = newFollower.m_fullPortraitSpriteName;
//							card.m_abilityUI.text = UpdateAbilityText(newFollower, progressState.m_level);
//							card.m_shortCutUI.text = UpdateShortAbilityText(newFollower, progressState.m_level);
//							card.m_shortCutUI.gameObject.SetActive(true);
//							card.m_rankUI.text = "Level " + (progressState.m_level+1).ToString() + " " + newFollower.m_followerClass.ToString();
//							card.m_healthUI.text = newFollower.abilityCost.ToString();
//							card.m_damageIcon.spriteName = "Icon_Energy";
//							break;	
//						}
//					}
//					break;
//				}
//			}
//			
//		} else {
//			
//			foreach (UICard card in m_party)
//			{
//				if (card.m_followerData == null)
//				{
//					card.m_portrait.gameObject.SetActive(true);
//					card.m_followerData = newFollower;
//					UpdateCard(newFollower);
//					card.m_shortCutUI.gameObject.SetActive(true);
//					card.selectState = UICard.SelectState.Unselected;
//					return;	
//				}
//			}
//		}
	}
	
	public void RemoveFollower (Follower oldFollower)
	{
		foreach (UICard card in m_party)
		{
			if (card.m_followerData != null)
			{
				if (card.m_followerData.m_followerType == oldFollower.m_followerType)
				{
					if (m_hoveredCard.m_followerData == oldFollower)
					{
						Debug.Log("UNSELECT FOLLOWER CARD");
						ClearSelection();
					}
					
					card.m_nameUI.gameObject.SetActive(false);
					card.m_abilityUI.gameObject.SetActive(false);
					card.m_rankUI.gameObject.SetActive(false);
//					card.m_passive01UI.gameObject.SetActive(false);
//					card.m_passive02UI.gameObject.SetActive(false);
					card.m_portrait.gameObject.SetActive(false);
					card.m_shortCutUI.gameObject.SetActive(false);
					card.m_miscOBJ[0].gameObject.SetActive(false);
					card.m_healthIcon.gameObject.SetActive(false);
					card.m_followerData = null;
					break;
				}
			}
		}
	}
	
	public void UpdateCard(Follower thisFollower)
	{
		foreach (UICard card in m_party)
		{
			if (card.m_followerData != null)
			{
				if (card.m_followerData == thisFollower)
				{
					if (card.m_nameText != null)
					{
						card.m_nameText.Text = thisFollower.m_nameText;
					} else {
						card.m_nameUI.text = thisFollower.m_nameText;
					}

					card.m_portrait.spriteName = thisFollower.m_fullPortraitSpriteName;
//					card.m_abilityUI.text = UpdateAbilityText(thisFollower);
//					card.m_shortCutUI.text = UpdateShortAbilityText(thisFollower);
//					card.m_healthUI.text = thisFollower.abilityCost.ToString();
//					card.m_damageIcon.spriteName = "Icon_Energy";
//					
//					card.m_rankUI.text = "Level " + (thisFollower.currentLevel+1).ToString() + " " + thisFollower.m_followerClass.ToString();	
				}
			}
		}
	}
	
	public string UpdateAbilityText (Follower thisFollower)
	{
		string aString = "";
		
		switch (thisFollower.m_followerType)
		{
		case Follower.FollowerType.Brand:
			aString = "+" + thisFollower.abilityEffect.ToString() + "$ until next turn.";
			break;
		case Follower.FollowerType.August:
			aString = "Regain " + thisFollower.abilityEffect.ToString() + "&.";
			break;
		case Follower.FollowerType.Jin:
			aString = "+" + thisFollower.abilityEffect.ToString() + " Actions.";
			break;
		case Follower.FollowerType.Telina:
			aString = thisFollower.abilityEffect.ToString() + "] Range " + thisFollower.abilityRange.ToString() + " attack.";
			break;
		case Follower.FollowerType.Samurai:
			if (thisFollower.abilityEffect == 1)
			{
				aString = "+" + thisFollower.abilityEffect.ToString() + " Action when defeating an enemy until next turn";
			} else {
				aString = "+" + thisFollower.abilityEffect.ToString() + " Actions when defeating an enemy until next turn";
			}
			break;
		case Follower.FollowerType.Wrestler:
			if (thisFollower.abilityEffect == 0)
			{
				aString = "Gain Counter Attack until next turn.";
			} else {
				aString = "Gain Counter Attack +" + thisFollower.abilityEffect.ToString() + " until next turn.";
			}
			break;
		case Follower.FollowerType.Monk:
			aString = "Knockback all adjacent enemies.";
			if (thisFollower.abilityEffect > 0)
			{
				aString = "Deal " + thisFollower.abilityEffect.ToString() + "$ and Knockback all adjacent enemies.";
			}
			break;
		case Follower.FollowerType.Knight:
			aString = "+" + thisFollower.abilityEffect.ToString() + "% until next turn.";
			break;
		case Follower.FollowerType.Dragoon:
			aString = "Remove an adjacent Trap.";
			break;
		}
		
		return aString;
	}

	public string UpdateShortAbilityText (Follower thisFollower)
	{
		string aString = "";
		
		switch (thisFollower.m_followerType)
		{
		case Follower.FollowerType.Brand:
			aString = "+" + thisFollower.abilityEffect.ToString() + "$";
			break;
		case Follower.FollowerType.August:
			aString = "+" + thisFollower.abilityEffect.ToString() + "&";
			break;
		case Follower.FollowerType.Jin:
			aString = "+" + thisFollower.abilityEffect.ToString() + "A";
			break;
		case Follower.FollowerType.Telina:
			aString = thisFollower.abilityEffect.ToString() + "] Rng " + thisFollower.m_abilityRange.ToString();
			break;
		case Follower.FollowerType.Samurai:
			aString = "+" + thisFollower.abilityEffect.ToString() + "A per kill";
			break;
		case Follower.FollowerType.Wrestler:
			aString = "+Counter";
			break;
		case Follower.FollowerType.Monk:
			aString = "Knockback";
			if (thisFollower.abilityEffect > 0)
			{
				aString = thisFollower.abilityEffect.ToString() + "$ Knockback";
			}
			break;
		case Follower.FollowerType.Knight:
			aString = "+" + thisFollower.abilityEffect.ToString() + "%";
			break;
		case Follower.FollowerType.Dragoon:
			aString = "-Trap";
			break;
		}
		
		return aString;
	}
	
	public void UpdatePassiveText (UICard card, Follower thisFollower, int level)
	{
		if (card.m_followerData.m_levelTable.Length > level)
		{
			card.m_passive01UI.text = "";
			card.m_passive02UI.text = "";
			
			int passiveNum = 1;
//			Follower.Level l = thisFollower.m_levelTable[level];
			
//			if (l.m_damageMod != 0)
//			{
//				card.m_passive01UI.text = "Passive: +" + l.m_damageMod.ToString() + "$";
//				passiveNum++;
//			}
//			if (l.m_healthMod != 0)
//			{
//				if (passiveNum == 1)
//				{
//					card.m_passive01UI.text = "Passive: +" + l.m_healthMod.ToString() + "&";
//					passiveNum++;
//				} else if (passiveNum == 2)
//				{
//					card.m_passive02UI.text = "Passive: +" + l.m_healthMod.ToString() + "&";
//					passiveNum++;
//				}
//			}
//			if (l.m_energyMod != 0)
//			{
//				if (passiveNum == 1)
//				{
//					card.m_passive01UI.text = "Passive: +" + l.m_energyMod.ToString() + "#";
//					passiveNum++;
//				} else if (passiveNum == 2)
//				{
//					card.m_passive02UI.text = "Passive: +" + l.m_energyMod.ToString() + "#";
//					passiveNum++;
//				}
//			}
//			if (l.m_armorMod != 0)
//			{
//				if (passiveNum == 1)
//				{
//					card.m_passive01UI.text = "Passive: +" + l.m_armorMod.ToString() + "%";
//					passiveNum++;
//				} else if (passiveNum == 2)
//				{
//					card.m_passive02UI.text = "Passive: +" + l.m_armorMod.ToString() + "%";
//					passiveNum++;
//				}
//			}

			if (thisFollower.badgeBonus_PassiveDamage != 0)
			{
				card.m_passive01UI.text = "Passive: +" + thisFollower.badgeBonus_PassiveDamage .ToString() + "$";
				passiveNum++;
			}
			if (thisFollower.badgeBonus_PassiveHealth != 0)
			{
				if (passiveNum == 1)
				{
					card.m_passive01UI.text = "Passive: +" + thisFollower.badgeBonus_PassiveHealth.ToString() + "&";
					passiveNum++;
				} else if (passiveNum == 2)
				{
					card.m_passive02UI.text = "Passive: +" + thisFollower.badgeBonus_PassiveHealth.ToString() + "&";
					passiveNum++;
				}
			}
			if (thisFollower.badgeBonus_PassiveEnergy != 0)
			{
				if (passiveNum == 1)
				{
					card.m_passive01UI.text = "Passive: +" + thisFollower.badgeBonus_PassiveEnergy.ToString() + "#";
					passiveNum++;
				} else if (passiveNum == 2)
				{
					card.m_passive02UI.text = "Passive: +" + thisFollower.badgeBonus_PassiveEnergy.ToString() + "#";
					passiveNum++;
				}
			}
			if (thisFollower.badgeBonus_PassiveArmor != 0)
			{
				if (passiveNum == 1)
				{
					card.m_passive01UI.text = "Passive: +" + thisFollower.badgeBonus_PassiveArmor.ToString() + "%";
					passiveNum++;
				} else if (passiveNum == 2)
				{
					card.m_passive02UI.text = "Passive: +" + thisFollower.badgeBonus_PassiveArmor.ToString() + "%";
					passiveNum++;
				}
			}
		}
	}
	
	public string UpdateAbilityText (Follower thisFollower, int level)
	{
		string aString = "";
		
		if (thisFollower.m_levelTable.Length > level)
		{
			
//			Follower.Level l = thisFollower.m_levelTable[level];
//			int cost = thisFollower.m_abilityCost + l.m_abilityCostMod;
//			int effect = thisFollower.m_abilityEffect + l.m_abilityEffectMod;
//			int range = thisFollower.m_abilityRange + l.m_abilityRangeMod;
			int cost = thisFollower.abilityCost;
			int effect = thisFollower.abilityEffect;
			int range = thisFollower.abilityRange;

			switch (thisFollower.m_followerType)
			{
			case Follower.FollowerType.Brand:
				aString = "+" + effect.ToString() + "$ until next turn.";
				break;
			case Follower.FollowerType.August:
				aString = "Regain " + effect.ToString() + "&.";
				break;
			case Follower.FollowerType.Jin:
				aString = "+" + effect.ToString() + " Actions.";
				break;
			case Follower.FollowerType.Telina:
				aString = effect.ToString() + "] Range " + range.ToString() + " attack.";
				break;
			case Follower.FollowerType.Samurai:
				if (effect == 1)
				{
					aString = "+" + effect.ToString() + " Action when defeating an enemy until next turn";
				} else {
					aString = "+" + effect.ToString() + " Actions when defeating an enemy until next turn";
				}
				break;
			case Follower.FollowerType.Wrestler:
				if (effect == 0)
				{
					aString = "Gain Counter Attack until next turn.";
				} else {
					aString = "Gain Counter Attack +" + effect.ToString() + " until next turn.";
				}
				break;
			case Follower.FollowerType.Monk:
				aString = "Knockback all adjacent enemies.";
				if (effect > 0)
				{
					aString = "Deal " + effect.ToString() + "$ and Knockback all adjacent enemies.";
				}
				break;
			case Follower.FollowerType.Knight:
				aString = "+" + effect.ToString() + "% until next turn.";
				break;
			case Follower.FollowerType.Dragoon:
				aString = "Remove an adjacent Trap.";
				break;
			}
		}
		//Debug.Log(aString);
		return aString;
	}

	public string UpdateShortAbilityText (Follower thisFollower, int level)
	{
		string aString = "";
		
		if (thisFollower.m_levelTable.Length > level)
		{
//			
//			Follower.Level l = thisFollower.m_levelTable[level];
//			int cost = thisFollower.m_abilityCost + l.m_abilityCostMod;
//			int effect = thisFollower.m_abilityEffect + l.m_abilityEffectMod;
//			int range = thisFollower.m_abilityRange + l.m_abilityRangeMod;
			int cost = thisFollower.abilityCost;
			int effect = thisFollower.abilityEffect;
			int range = thisFollower.abilityRange;
		
			switch (thisFollower.m_followerType)
			{
			case Follower.FollowerType.Brand:
				aString = "+" + effect.ToString() + "$";
				break;
			case Follower.FollowerType.August:
				aString = "+" + effect.ToString() + "&";
				break;
			case Follower.FollowerType.Jin:
				aString = "+" + effect.ToString() + "A";
				break;
			case Follower.FollowerType.Telina:
				aString = effect.ToString() + "] Rng " + range.ToString();
				break;
			case Follower.FollowerType.Samurai:
				aString = "+" + effect.ToString() + "A per kill";
				break;
			case Follower.FollowerType.Wrestler:
				aString = "+Counter";
				break;
			case Follower.FollowerType.Monk:
				aString = "Knockback";
//				if (level > 0)
//				{
//					aString = effect.ToString() + "$ Knockback";
//				}
				break;
			case Follower.FollowerType.Knight:
				aString = "+" + effect.ToString() + "%";
				break;
			case Follower.FollowerType.Dragoon:
				aString = "- Traps.";
				break;
			}
		}
		//Debug.Log(aString);
		return aString;
	}

	public bool CanEquipSkill (Follower f, Item skill)
	{
//		if ((f.m_followerClass == skill.m_class || skill.m_class == Follower.FollowerClass.None) && f.currentSkills < f.maxSkills && f.currentLevel+1 >= skill.m_itemLevel )
//		{
//			return true;
//		}
//
//		return false;

		return true;
	}

	public bool CanEquipSkill (Item skill)
	{
//		foreach (UICard c in m_party)
//		{
//			if (c.m_followerData != null)
//			{
//				if ((c.m_followerData.m_followerClass == skill.m_class || skill.m_class == Follower.FollowerClass.None) && c.m_followerData.currentSkills < c.m_followerData.maxSkills && c.m_followerData.currentLevel+1 >= skill.m_itemLevel )
//				{
//					return true;
//				}
//			}
//		}
//		return false;

		return true;
	}

	public bool MeetsSkillReqs (Item skill)
	{
//		foreach (UICard c in m_party)
//		{
//			if (c.m_followerData != null)
//			{
//				if ((c.m_followerData.m_followerClass == skill.m_class || skill.m_class == Follower.FollowerClass.None) && c.m_followerData.currentLevel+1 >= skill.m_itemLevel )
//				{
//					return true;
//				}
//			}
//		}
//		return false;

		return true;
	}

	public IEnumerator EquipSkill (Item skill, UICard card)
	{
		foreach (UICard c in m_party)
		{
			if (c.m_followerData != null)
			{
				// find first available hero
				if ((c.m_followerData.m_followerClass == skill.m_class || skill.m_class == Follower.FollowerClass.None) && c.m_followerData.currentSkills < c.m_followerData.maxSkills)
				{
					// find empty skill card
					foreach (UICard sc in c.skillCards)
					{
						if (sc.itemData == null)
						{
							float t = 0;
							float time = 0.3f;
							Vector3 startPos = card.transform.position;
							Vector3 startScale = card.transform.localScale;
							while (t < time)
							{
								t += Time.deltaTime;;
								Vector3 nPos = Vector3.Lerp(startPos, sc.transform.position , t / time);
								Vector3 newScale = Vector3.Lerp(startScale, Vector3.one * 0.25f, t / time);
								card.transform.position = nPos;
								card.transform.localScale = newScale;
								yield return null;
							}
							
							card.transform.position = startPos;
							card.transform.localScale = startScale;


							//equip skill
//							skill.attachedFollower = c.m_followerData;
							c.m_followerData.currentSkills ++;
							sc.itemData = skill;
							sc.m_nameUI.text = skill.m_name;
							sc.m_nameUI.gameObject.SetActive(true);
							sc.m_portrait.spriteName = skill.m_portraitSpriteName;
							sc.m_portrait.gameObject.SetActive(true);
							sc.gameObject.SetActive(true);
							sc.m_abilityUI.text = skill.m_description;
							sc.m_abilityUI.gameObject.SetActive(true);
//							sc.m_rankUI.text = skill.m_keywordText;
							sc.m_rankUI.gameObject.SetActive(true);
							sc.m_followerData = c.m_followerData;

							if (skill.m_energyCost > 0)
							{
								sc.m_healthUI.text = skill.m_energyCost.ToString();
								sc.m_damageIcon.spriteName = "Icon_Energy";
								sc.m_damageIcon.gameObject.SetActive(true);
								sc.m_healthIcon.gameObject.SetActive(true);
								sc.m_healthUI.gameObject.SetActive(true);
							} 
//							else if (skill.HasKeyword(Item.Keyword.Consumeable))
//							{
//								sc.m_healthUI.gameObject.SetActive(false);
//								sc.m_damageIcon.spriteName = "Icon_Limbo";
//								sc.m_healthIcon.gameObject.SetActive(true);
//								sc.m_damageIcon.gameObject.SetActive(true);
//							} 
							else {
								sc.m_healthIcon.gameObject.SetActive(false);
								sc.m_damageIcon.gameObject.SetActive(false);
								sc.m_healthUI.gameObject.SetActive(false);
							}

							BoxCollider bc = (BoxCollider)sc.GetComponent("BoxCollider");
							bc.enabled = false;

							yield break;
						}
					}
				}
			}
		}
	}

	public void EquipSkill (Follower f, Item skill)
	{
		foreach (UICard c in m_party)
		{
			if (c.m_followerData == f)
			{
				// find empty skill card
				foreach (UICard sc in c.skillCards)
				{
					if (sc.itemData == null)
					{
						//equip skill
//						skill.attachedFollower = c.m_followerData;
						c.m_followerData.currentSkills ++;
						sc.itemData = skill;
						sc.m_nameUI.text = skill.m_name;
						sc.m_nameUI.gameObject.SetActive(true);
						sc.m_portrait.spriteName = skill.m_portraitSpriteName;
						sc.m_portrait.gameObject.SetActive(true);
						sc.gameObject.SetActive(true);
						sc.m_abilityUI.text = skill.m_description;
						sc.m_abilityUI.gameObject.SetActive(true);
//						sc.m_rankUI.text = skill.m_keywordText;
						sc.m_rankUI.gameObject.SetActive(true);
						sc.m_followerData = c.m_followerData;

						if (skill.m_energyCost > 0)
						{
							sc.m_healthUI.text = skill.m_energyCost.ToString();
							sc.m_damageIcon.spriteName = "Icon_Energy";
							sc.m_healthIcon.gameObject.SetActive(true);
							sc.m_healthUI.gameObject.SetActive(true);
						} else
						{
							sc.m_healthUI.gameObject.SetActive(false);
							sc.m_damageIcon.gameObject.SetActive(false);
						}
//						else if (skill.HasKeyword(Item.Keyword.Consumeable))
//						{
//							sc.m_healthUI.gameObject.SetActive(false);
//							sc.m_damageIcon.spriteName = "Icon_Limbo";
//							sc.m_healthIcon.gameObject.SetActive(true);
//							sc.m_damageIcon.gameObject.SetActive(true);
//						}

						
						return;
					}
				}
			}
		}
	}

	public UICard GetSkillCard (Item item)
	{
		foreach (UICard c in m_party)
		{
			if (c.m_followerData != null)
			{
				foreach (UICard sc in c.skillCards)
				{
					if (sc.itemData != null)
					{
						if (sc.itemData == item)
						{
							return sc;
						}
					}
				}
			}
		}

		return null;
	}

	public UICard[] GetSkillCards (Follower f)
	{
		foreach (UICard c in m_party)
		{
			if (c.m_followerData != null)
			{
				if (c.m_followerData == f)
				{
					return c.skillCards;
				}
			}
		}

		return null;
	}

	public void ResortSkillCards (Follower f)
	{
		//collect all items
		List<Item> items = new List<Item> ();
		UICard[] sCards = GetSkillCards (f);
		foreach (UICard c in sCards)
		{
			if (c.itemData != null)
			{
				items.Add(c.itemData);
				c.itemData = null;
			}
		}

		//repopulate cards
		for (int i=0; i < sCards.Length; i++)
		{
			UICard card = (UICard)sCards[i];
			if (i < items.Count)
			{
				//populate card
				Item thisItem = (Item)items[i];
				//skill.attachedFollower = c.m_followerData;
				card.itemData = thisItem;
				card.m_nameUI.text = thisItem.m_name;
				card.m_portrait.spriteName = thisItem.m_portraitSpriteName;
				card.gameObject.SetActive(true);
				card.m_abilityUI.text = thisItem.m_description;
				card.m_abilityUI.gameObject.SetActive(true);
//				card.m_rankUI.text = thisItem.m_keywordText;
				card.m_rankUI.gameObject.SetActive(true);
				card.m_healthUI.text = thisItem.m_energyCost.ToString();
				card.m_damageIcon.spriteName = "Icon_Energy";
				card.m_healthIcon.gameObject.SetActive(true);

//				if (thisItem.itemState == Item.ItemState.Spent)
//				{
//					card.Deactivate();
//				}
//				BoxCollider bc = (BoxCollider)card.GetComponent("BoxCollider");
//				bc.enabled = false;

			} else {
				//blank card
				card.gameObject.SetActive(false);
				card.itemData = null;
			}
		}

		items.Clear ();

	}

	public void UnequipSkill (Item item)
	{
//		Debug.Log ("UNEQUIP SKILL");
//		ClearSkillSelection ();
//		ClearSelection ();
//		UICard sCard = GetSkillCard (item);
//		sCard.m_itemData = null;
//		ResortSkillCards (item.attachedFollower);
//		item.attachedFollower = null;
	}

	public void RemoveSkill (Item item)
	{
//		Debug.Log ("REMOVE SKILL");
//		ClearSkillSelection ();
//		ClearSelection ();
//		UICard sCard = GetSkillCard (item);
//		sCard.m_itemData = null;
//		ResortSkillCards (item.attachedFollower);
//		item.attachedFollower = null;
//
//
//		//GameManager.m_gameManager.AddLimboCard (item);
//		GameManager.m_gameManager.SendToGrave (item);
	}

	public IEnumerator CleanSkills ()
	{
		List<UICard> oldSkills = new List<UICard> ();
		List<Vector3> startScale = new List<Vector3>();
		List<Vector3> startPos = new List<Vector3>();

//		foreach (UICard p in m_party)
//		{
//			if (p.m_followerData != null)
//			{
//				UICard[] skills = GetSkillCards(p.m_followerData);
//				for (int i=0; i < skills.Length; i++)
//				{
//					UICard s = skills[i];
//					if (s.itemData != null)
//					{
//						if (s.itemData.itemState == Item.ItemState.Spent && s.itemData.HasKeyword(Item.Keyword.Consumeable) && s.itemData.HasKeyword(Item.Keyword.Skill))
//						{
//							oldSkills.Add(s);
//							startPos.Add(s.transform.position);
//							startScale.Add(s.transform.localScale);
//							p.m_followerData.currentSkills --;
//							i = 0;
//						}
//					}
//				}
//			}
//		}

		if (oldSkills.Count > 0)
		{
			// move cards to limbo
			float t = 0;
			float time = 0.5f;

			while (t < time)
			{
				t += Time.deltaTime;
				for (int i=0; i < oldSkills.Count; i++)
				{
					Vector3 nPos = Vector3.Lerp(startPos[i], UIManager.m_uiManager.m_backpackButton.transform.position , t / time);
					Vector3 newScale = Vector3.Lerp(startScale[i], Vector3.one * 0.25f, t / time);				
					oldSkills[i].transform.position = nPos;
					oldSkills[i].transform.localScale = newScale;
				}


				yield return null;
			}

			for (int i=0; i < oldSkills.Count; i++)
			{
				oldSkills[i].transform.position = startPos[i];
				oldSkills[i].transform.localScale = startScale[i];

				RemoveSkill(oldSkills[i].itemData);
			}
		}

		yield return null;
	}

	public int NumHeroesEligible (Item skill)
	{
		int numEligible = 0;
//		foreach (UICard c in m_party)
//		{
//			if (c.m_followerData != null)
//			{
//				if ((c.m_followerData.m_followerClass == skill.m_class || skill.m_class == Follower.FollowerClass.None) && c.m_followerData.currentLevel+1 >= skill.m_itemLevel  && c.m_followerData.currentSkills < c.m_followerData.maxSkills )
//				{
//					numEligible ++;
//				}
//			}
//		}
		return numEligible;
	}

	public List<UICard> GetAllSkills (bool checkSpent)
	{
		List<UICard> skills = new List<UICard> ();

		foreach (UICard c in m_party)
		{
			if (c.m_followerData != null)
			{
				UICard[] s = c.skillCards;
				foreach (UICard sCard in s)
				{
					if (sCard.itemData != null)
					{
//						if (sCard.itemData.itemState == Item.ItemState.Normal || (sCard.itemData.itemState == Item.ItemState.Spent && checkSpent))
//						{
//							skills.Add(sCard);
//						}
					}
				}
			}
		}

		return skills;
	}
	
	public UICard hoveredCard {get {return m_hoveredCard;}}
	public UICard hoveredSkill {get {return m_hoveredSkillCard;}}
	public Vector3 unselectedScale {get {return m_unselectedScale;}}
	public Vector3 boundsNormalPos {get {return m_boundsNormalPos;}}
	public Vector3 boundsNormalSize {get {return m_boundsNormalSize;}}

}
