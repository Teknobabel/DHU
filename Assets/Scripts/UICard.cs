using UnityEngine;
using System.Collections;

public class UICard : MonoBehaviour {
	
	public enum CardType
	{
		Item,
		Char,
	}
	
	public enum SelectState //used in new party select menu
	{
		Locked,
		Unselected,
		Selected,
	}
	
	public CardType
		m_cardType = CardType.Item;
	
	public UILabel
		m_nameUI,
		m_healthUI,
		m_damageUI,
		m_energyUI,
		m_abilityUI,
		m_armorUI,
		m_rankUI,
		m_passive01UI,
		m_passive02UI,
		m_shortCutUI;

	public TypogenicText
		m_nameText;

	public UILabel[]
		m_miscText;

	public UISprite[]
		m_miscSprite;
	
	public UISprite
		m_portrait,
		m_cardSprite,
		m_damageIcon,
		m_healthIcon,
		m_energyIcon;

	public Transform[]
		m_miscOBJ;
	
	public UISprite[]
		m_keywords;
	
	public Follower 
		m_followerData;

	public Enemy
		m_enemyData;
	
	public Item
		m_itemData;

	public UICard[]
		m_skillCards;
	
	private SelectState
		m_selectState = SelectState.Locked;

	private Vector3
		m_startPos = Vector3.zero,
		m_startRot = Vector3.zero;
	

	// Use this for initialization
	void Start () {
		if (m_miscOBJ.Length > 1) {
			m_startPos = m_miscOBJ [1].transform.position;
			m_startRot = m_miscOBJ [1].transform.eulerAngles;
		}
	}
	
	public void Deactivate ()
	{
		//m_nameUI.text = "Name";
		m_nameUI.gameObject.SetActive(false);
		m_portrait.spriteName = "Card_Back03";
		m_abilityUI.gameObject.SetActive(false);

		if (m_shortCutUI != null)
		{
			//m_shortCutUI.text = "";
			m_shortCutUI.gameObject.SetActive(false);
		}

		if (m_rankUI != null)
		{
			m_rankUI.gameObject.SetActive(false);
		}
		
		if (m_passive01UI != null)
		{
			m_passive01UI.gameObject.SetActive(false);
			m_passive02UI.gameObject.SetActive(false);
		}

		if (m_healthIcon != null)
		{
			m_healthIcon.gameObject.SetActive(false);
		}

		if (m_miscOBJ.Length > 0)
		{
			m_miscOBJ[0].gameObject.SetActive(false);
		}
		
		//m_cardSprite.spriteName = "Card_Back01";
		
		foreach (UISprite thisSprite in m_keywords)
		{
			thisSprite.gameObject.SetActive(false);	
		}
	}

	public void SetCard (Enemy enemy, bool doSmall)
	{
		m_nameUI.text = enemy.m_displayName;
		m_nameUI.gameObject.SetActive(true);
		m_abilityUI.text = enemy.m_abilityText;
		m_abilityUI.gameObject.SetActive(true);
		m_rankUI.text =  "Level " + enemy.m_level.ToString() + " " + enemy.m_enemyType.ToString();
		m_rankUI.gameObject.SetActive (true);
		m_healthIcon.gameObject.SetActive(true);
		m_healthUI.gameObject.SetActive(false);
		m_damageIcon.gameObject.SetActive(false);
		m_enemyData = enemy;
		m_portrait.spriteName = enemy.m_portraitSpriteName;

		m_portrait.gameObject.SetActive(true);
	}

	public void SetCard (Item item, bool doSmall)
	{

		m_nameUI.text = item.m_name;
		m_abilityUI.text = item.m_description;
//		m_rankUI.text = item.m_keywordText;
		m_itemData = item;

//		if (m_shortCutUI != null)
//		{
//			m_shortCutUI.text = item.m_shortcutText;
//		}
			

			if (item.m_energyCost > 0)
			{
				m_healthUI.text = item.m_energyCost.ToString();
				m_damageIcon.spriteName = "Icon_Energy";
				m_healthUI.gameObject.SetActive(true);
				m_damageIcon.gameObject.SetActive(true);
			} else if (item.m_healthCost > 0)
			{
				m_healthUI.text = item.m_healthCost.ToString();
				m_damageIcon.spriteName = "Icon_Health";
				m_healthUI.gameObject.SetActive(true);
				m_damageIcon.gameObject.SetActive(true);
			}
//			else if (item.HasKeyword(Item.Keyword.Consumeable))
//			{
//				m_healthUI.gameObject.SetActive(false);
//				m_damageIcon.spriteName = "Icon_Limbo";
//				m_damageIcon.gameObject.SetActive(true);
//			}
//			else if (item.HasKeyword(Item.Keyword.Limbo))
//			{
//				m_healthUI.gameObject.SetActive(false);
//				m_damageIcon.spriteName = "Icon_Limbo02";
//				m_damageIcon.gameObject.SetActive(true);
//			}
//			else
//			{
//				m_healthUI.gameObject.SetActive(false);
//				m_damageIcon.gameObject.SetActive(false);
//			}


		if (doSmall)
		{
//			m_portrait.spriteName = item.m_fullPortraitSpriteName;
			m_miscOBJ[0].gameObject.SetActive(false);
			m_rankUI.gameObject.SetActive(false);
			m_abilityUI.gameObject.SetActive(false);
			if (m_shortCutUI != null)
			{
				m_shortCutUI.gameObject.SetActive(true);
			}
			m_nameUI.gameObject.SetActive(false);
		} else {
			m_nameUI.gameObject.SetActive(true);
			m_portrait.spriteName = item.m_portraitSpriteName;
			m_miscOBJ[0].gameObject.SetActive(true);
			m_rankUI.gameObject.SetActive(true);
			m_healthIcon.gameObject.SetActive(true);
			m_abilityUI.gameObject.SetActive(true);
			if (m_shortCutUI != null)
			{
				m_shortCutUI.gameObject.SetActive(false);
			}
		}

		if (item.HasKeyword (Item.Keyword.LostSoul) || item.HasKeyword (Item.Keyword.Key)) {
			m_healthUI.gameObject.SetActive(false);
			m_damageIcon.gameObject.SetActive(false);
		}

		m_portrait.gameObject.SetActive(true);

	}

	public void SetCard (Follower follower, bool doSmall)
	{
		m_abilityUI.text = PartyCards.m_partyCards.UpdateAbilityText(follower);
//		if (m_shortCutUI != null)
//		{
//			m_shortCutUI.text = PartyCards.m_partyCards.UpdateShortAbilityText(follower);
//		}

		m_nameUI.text = follower.m_nameText;
//		m_rankUI.text = "Level " + (follower.currentLevel+1).ToString() + " " + follower.m_followerClass.ToString();
		m_followerData = follower;

//		m_damageIcon.spriteName = "Icon_Energy";
//		m_healthUI.text = follower.abilityCost.ToString ();
		
//		if (item.m_energyCost > 0)
//		{
//			m_healthUI.text = item.m_energyCost.ToString();
//			m_damageIcon.spriteName = "Icon_Energy";
//			m_healthUI.gameObject.SetActive(true);
//			m_damageIcon.gameObject.SetActive(true);
//		} else if (item.m_healthCost > 0)
//		{
//			m_healthUI.text = item.m_healthCost.ToString();
//			m_damageIcon.spriteName = "Icon_Health";
//			m_healthUI.gameObject.SetActive(true);
//			m_damageIcon.gameObject.SetActive(true);
//		}
//		else
//		{
//			m_healthUI.gameObject.SetActive(false);
//			m_damageIcon.gameObject.SetActive(false);
//		}
		
//		if (doSmall)
//		{
//			m_portrait.spriteName = follower.m_fullPortraitSpriteName;
//			m_miscOBJ[0].gameObject.SetActive(false);
//			m_rankUI.gameObject.SetActive(false);
//			m_abilityUI.gameObject.SetActive(false);
//			m_shortCutUI.gameObject.SetActive(true);
//			m_nameUI.gameObject.SetActive(false);
//		} else {
			m_nameUI.gameObject.SetActive(true);
			m_portrait.spriteName = follower.m_portraitSpriteName;
//			m_miscOBJ[0].gameObject.SetActive(true);
//			m_rankUI.gameObject.SetActive(true);
//			m_healthIcon.gameObject.SetActive(true);
//			m_abilityUI.gameObject.SetActive(true);
//			if (m_shortCutUI != null)
//			{
//				m_shortCutUI.gameObject.SetActive(false);
//			}
//		}
		
		m_portrait.gameObject.SetActive(true);
		
	}

	public void SetDark(bool doDark)
	{
		Color color = Color.grey;
		if (!doDark)
		{
			color = Color.white;
			m_selectState = SelectState.Unselected;
		} else {
			m_selectState = SelectState.Locked;
		}

		m_portrait.color = color;
		m_nameUI.color = color;
		m_abilityUI.color = color;
		m_shortCutUI.color = color;
		m_rankUI.color = color;
		m_damageIcon.color = color;
		m_healthUI.color = color;
	}
	
	public void SetKeywords ()
	{
//		if (m_itemData.m_keywords.Length > 0)
//		{
//			m_keywords[0].gameObject.SetActive(true);
//			for (int i=0; i < m_itemData.m_keywords.Length; i++)
//			{
//				Item.Keyword thisKeyword = m_itemData.m_keywords[i];
//				if (thisKeyword != Item.Keyword.Chain)
//				{
//					m_keywords[i+1].gameObject.SetActive(true);
//					switch (thisKeyword)
//					{
//					case Item.Keyword.Consumeable:
//						m_keywords[i+1].spriteName = "Keyword_Consumeable";
//						break;
//					case Item.Keyword.Equippable:
//						m_keywords[i+1].spriteName = "Keyword_Equippable";
//						break;
//					case Item.Keyword.Craftable:
//						m_keywords[i+1].spriteName = "Keyword_Craftable";
//						break;
//					}
//				}
//			}
//		}
	}
	
	public Item itemData {
		get{
			return m_itemData;	
		}
		set{
			m_itemData = value;
//			if (m_itemData != null && m_keywords.Length > 0)
//			{
//				SetKeywords();
//			}
		}}
	
	public SelectState selectState { get{return m_selectState;} set{m_selectState = value;}}
	public UICard[] skillCards { get{return m_skillCards;} set{m_skillCards = value;}}
	public Vector3 startPos {get{return m_startPos;}}
	public Vector3 startRot {get{return m_startRot;}}
	
}
