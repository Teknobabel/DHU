using UnityEngine;
using System.Collections;

public class EquipCards : MonoBehaviour {
	
	public UICard[]
		m_items;
	
	private UICard
		m_hoveredCard;
	
	private Vector3
		m_selectedScale = new Vector3(0.94f,0.94f,0.94f),
		m_unselectedScale = new Vector3(0.44f,0.44f,0.44f),
		m_boundsNormalPos = new Vector3(15,166.9f,0),
		m_boundsNormalSize = new Vector3(204.3f,311.3f,10),
		m_boundsHoveredPos = new Vector3(1.8f,76.7f,0),
		m_boundsHoveredSize = new Vector3(102.4f,152.6f,10);
	
	private float
		m_offset = 100;
	
	public static EquipCards
		m_equipCards;
	
	void Awake ()
	{
		m_equipCards = this;
	}
	
	public void Initialize ()
	{
		foreach (UICard card in m_items)
		{
			card.m_nameUI.gameObject.SetActive(false);
			card.m_abilityUI.gameObject.SetActive(false);
			card.m_shortCutUI.gameObject.SetActive(false);
			card.m_portrait.gameObject.SetActive(false);
			card.m_rankUI.gameObject.SetActive(false);
			card.m_miscOBJ[0].gameObject.SetActive(false);
			card.m_healthIcon.gameObject.SetActive(false);
//			foreach (UISprite sprite in card.m_keywords)
//			{
//				sprite.gameObject.SetActive(false);	
//			}
		}

		//disable equip slots based on badge state
//		int maxItems = 1 + GameManager.m_gameManager.itemBonus;
//		for (int i=0; i < m_items.Length; i++)
//		{
//			if (i >= maxItems)
//			{
//				m_items[i].gameObject.SetActive(false);
//			}
//		}
	}
	
	public void CardHovered (GameObject hoveredCard)
	{
		if (m_hoveredCard == null)
		{
			UICard uiCard = (UICard)hoveredCard.transform.GetComponent("UICard");
			m_hoveredCard = uiCard;
			if (m_hoveredCard.m_itemData == null)
			{
				m_hoveredCard.m_portrait.gameObject.SetActive(true);
				m_hoveredCard.m_portrait.spriteName = "Card_Empty";
			} else 
			{
				uiCard.m_portrait.spriteName = uiCard.m_itemData.m_portraitSpriteName;
				uiCard.m_nameUI.gameObject.SetActive(true);
				uiCard.m_abilityUI.gameObject.SetActive(true);
				uiCard.m_shortCutUI.gameObject.SetActive(false);
				uiCard.m_rankUI.gameObject.SetActive(true);
				uiCard.m_miscOBJ[0].gameObject.SetActive(true);
				uiCard.m_healthIcon.gameObject.SetActive(true);
				//uiCard.SetKeywords();
				
				hoveredCard.transform.localScale = m_selectedScale;

				BoxCollider bc = (BoxCollider)uiCard.gameObject.GetComponent("BoxCollider");
				bc.size = m_boundsHoveredSize;
				bc.center = m_boundsHoveredPos;

				uiCard.m_portrait.depth += 100;
				uiCard.m_nameUI.depth += 100;
				uiCard.m_rankUI.depth += 100;
				uiCard.m_abilityUI.depth += 100;
				uiCard.m_shortCutUI.depth += 100;
				uiCard.m_damageIcon.depth += 100;
				uiCard.m_healthUI.depth += 100;
				uiCard.m_healthIcon.depth += 100;

				
//				foreach (UICard card in m_items)
//				{
//					if (card != m_hoveredCard && card.transform.position.x > m_hoveredCard.transform.position.x)
//					{
//						Vector3 newPos = card.transform.localPosition;
//						newPos.x += m_offset;
//						card.transform.localPosition = newPos;
//					}
//				}
			}
		} else if (m_hoveredCard.gameObject != hoveredCard.gameObject)
		{
			if (m_hoveredCard.m_itemData == null)
			{
				m_hoveredCard.m_portrait.gameObject.SetActive(false);
			} else {
			
//				if (m_hoveredCard.itemData.itemState == Item.ItemState.Spent)
//				{
//					m_hoveredCard.m_portrait.spriteName = "Card_Back03";
//					m_hoveredCard.m_shortCutUI.gameObject.SetActive(false);
//				} else if (m_hoveredCard.itemData.itemState == Item.ItemState.Normal)
//				{
//					m_hoveredCard.m_portrait.spriteName = m_hoveredCard.itemData.m_fullPortraitSpriteName;
//					m_hoveredCard.m_shortCutUI.gameObject.SetActive(true);
//				}

				//m_hoveredCard.m_portrait.spriteName = m_hoveredCard.m_itemData.m_fullPortraitSpriteName;
				m_hoveredCard.transform.localScale = m_unselectedScale;
				m_hoveredCard.m_nameUI.gameObject.SetActive(false);
				m_hoveredCard.m_abilityUI.gameObject.SetActive(false);
				m_hoveredCard.m_rankUI.gameObject.SetActive(false);
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
				m_hoveredCard.m_damageIcon.depth -= 100;
				m_hoveredCard.m_healthUI.depth -= 100;
				m_hoveredCard.m_healthIcon.depth -= 100;

//				foreach (UISprite sprite in m_hoveredCard.m_keywords)
//				{
//					sprite.gameObject.SetActive(false);	
//				}
				
//				foreach (UICard card in m_items)
//				{
//					if (card != m_hoveredCard && card.transform.position.x > m_hoveredCard.transform.position.x)
//					{
//						Vector3 newPos = card.transform.localPosition;
//						newPos.x -= m_offset;
//						card.transform.localPosition = newPos;
//					}
//				}
			}
			
			UICard uiCard = (UICard)hoveredCard.transform.GetComponent("UICard");
			if (uiCard.m_itemData == null)
			{
				m_hoveredCard = uiCard;
				m_hoveredCard.m_portrait.gameObject.SetActive(true);
				m_hoveredCard.m_portrait.spriteName = "Card_Empty";
			} else {
				m_hoveredCard = uiCard;
				m_hoveredCard.m_portrait.spriteName = uiCard.m_itemData.m_portraitSpriteName;
				m_hoveredCard.m_nameUI.gameObject.SetActive(true);
				m_hoveredCard.m_abilityUI.gameObject.SetActive(true);
				m_hoveredCard.m_shortCutUI.gameObject.SetActive(false);
				m_hoveredCard.m_rankUI.gameObject.SetActive(true);
				m_hoveredCard.m_miscOBJ[0].gameObject.SetActive(true);
				m_hoveredCard.m_healthIcon.gameObject.SetActive(true);

				BoxCollider bc = (BoxCollider)m_hoveredCard.gameObject.GetComponent("BoxCollider");
				bc.size = m_boundsHoveredSize;
				bc.center = m_boundsHoveredPos;

				m_hoveredCard.m_portrait.depth += 100;
				m_hoveredCard.m_nameUI.depth += 100;
				m_hoveredCard.m_rankUI.depth += 100;
				m_hoveredCard.m_abilityUI.depth += 100;
				m_hoveredCard.m_shortCutUI.depth += 100;
				m_hoveredCard.m_damageIcon.depth += 100;
				m_hoveredCard.m_healthUI.depth += 100;
				m_hoveredCard.m_healthIcon.depth += 100;
//				uiCard.SetKeywords();
				
				hoveredCard.transform.localScale = m_selectedScale;
				
//				foreach (UICard card in m_items)
//				{
//					if (card != m_hoveredCard && card.transform.position.x > m_hoveredCard.transform.position.x)
//					{
//						Vector3 newPos = card.transform.localPosition;
//						newPos.x += m_offset;
//						card.transform.localPosition = newPos;
//					}
//				}
			}
		}
	}
	
	public void ClearSelection ()
	{
		if (m_hoveredCard != null)
		{
			if (m_hoveredCard.m_itemData == null)
			{
				m_hoveredCard.m_portrait.gameObject.SetActive(false);
			} else {

//				if (m_hoveredCard.itemData.itemState == Item.ItemState.Spent)
//				{
//					m_hoveredCard.m_portrait.spriteName = "Card_Back03";
//					m_hoveredCard.m_shortCutUI.gameObject.SetActive(false);
//				} else if (m_hoveredCard.itemData.itemState == Item.ItemState.Normal)
//				{
//					m_hoveredCard.m_portrait.spriteName = m_hoveredCard.itemData.m_fullPortraitSpriteName;
//					m_hoveredCard.m_shortCutUI.gameObject.SetActive(true);
//				}

				//m_hoveredCard.m_portrait.spriteName = m_hoveredCard.m_itemData.m_fullPortraitSpriteName;
				m_hoveredCard.transform.localScale = m_unselectedScale;
				m_hoveredCard.m_nameUI.gameObject.SetActive(false);
				m_hoveredCard.m_abilityUI.gameObject.SetActive(false);
				m_hoveredCard.m_rankUI.gameObject.SetActive(false);
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
				m_hoveredCard.m_damageIcon.depth -= 100;
				m_hoveredCard.m_healthUI.depth -= 100;
				m_hoveredCard.m_healthIcon.depth -= 100;

//				foreach (UISprite sprite in m_hoveredCard.m_keywords)
//				{
//					sprite.gameObject.SetActive(false);	
//				}
				
//				foreach (UICard card in m_items)
//				{
//					if (card != m_hoveredCard && card.transform.position.x > m_hoveredCard.transform.position.x)
//					{
//						Vector3 newPos = card.transform.localPosition;
//						newPos.x -= m_offset;
//						card.transform.localPosition = newPos;
//					}
//				}
			}
			m_hoveredCard = null;
		}
	}
	
	public void AddItem (Item newItem)
	{
		foreach (UICard card in m_items)
		{
//			if (card.m_itemData == null)
//			{
//				card.m_portrait.gameObject.SetActive(true);
//				card.m_nameUI.text = newItem.m_name;
//				card.m_portrait.spriteName = newItem.m_fullPortraitSpriteName;
//				card.m_abilityUI.text = newItem.m_description;
//				card.m_shortCutUI.text = newItem.m_shortcutText;
//				card.m_shortCutUI.gameObject.SetActive(true);
//				card.m_rankUI.text = newItem.m_keywordText;
//				card.m_itemData = newItem;
//
//				if (newItem.m_energyCost > 0)
//				{
//					card.m_healthUI.text = newItem.m_energyCost.ToString();
//					card.m_damageIcon.spriteName = "Icon_Energy";
//					card.m_healthUI.gameObject.SetActive(true);
//					card.m_damageIcon.gameObject.SetActive(true);
//				} else if (newItem.m_healthCost > 0)
//				{
//					card.m_healthUI.text = newItem.m_healthCost.ToString();
//					card.m_damageIcon.spriteName = "Icon_Health";
//					card.m_healthUI.gameObject.SetActive(true);
//					card.m_damageIcon.gameObject.SetActive(true);
//				} //else if (newItem.HasKeyword(Item.Keyword.Consumeable) || newItem.HasKeyword(Item.Keyword.Chain))
//				else
//				{
//					card.m_healthUI.gameObject.SetActive(false);
//					card.m_damageIcon.gameObject.SetActive(false);
//				}
//
//				//card.m_miscOBJ[0].gameObject.SetActive(true);
//
//				return;	
//			}
		}
	}
	
	public void RemoveItem (Item oldItem)
	{
		foreach (UICard card in m_items)
		{
			if (card.m_itemData != null)
			{
				if (card.m_itemData.id == oldItem.id)
				{
					//reset sizing if needed
					if (m_hoveredCard == card)
					{
						ClearSelection();	
					}
					
					//remove item from equipped items list
					for (int i=0; i<GameManager.m_gameManager.equippedItems.Count; i++)
					{
						Item item = GameManager.m_gameManager.equippedItems[i];
						if (item == oldItem)
						{
							GameManager.m_gameManager.equippedItems.RemoveAt(i);
							break;
						}
					}
					
					//clear card UI
					card.m_portrait.gameObject.SetActive(false);
					card.m_nameUI.gameObject.SetActive(false);
					card.m_abilityUI.gameObject.SetActive(false);
					card.m_shortCutUI.gameObject.SetActive(false);
					card.m_rankUI.gameObject.SetActive(false);
					card.m_miscOBJ[0].gameObject.SetActive(false);
					card.m_healthIcon.gameObject.SetActive(false);
//					foreach (UISprite sprite in card.m_keywords)
//					{
//						sprite.gameObject.SetActive(false);	
//					}
					card.itemData = null;

					//reset colors
//					if (oldItem.itemState == Item.ItemState.Spent)
//					{
//						card.m_portrait.color = Color.white;
//						card.m_nameUI.color = Color.white;
//						card.m_abilityUI.color = Color.white;
//					}

					//remove effects from effect list if needed
					foreach (EffectsPanel.Effect e in EffectsPanel.m_effectsPanel.effectStack)
					{
						if (e.m_affectedItem == oldItem)
						{
							EffectsPanel.m_effectsPanel.RemoveEffect(e);
							break;
						}
					}
					
					
					return;
				}
			}
		}
	}
	
	public UICard hoveredCard {get {return m_hoveredCard;}}
	public int freeEquipSlots {
		get {
			int numFree = 0;
			int maxEquipped = 1 + GameManager.m_gameManager.itemBonus;
			int i =  0;
	
			foreach (UICard card in m_items)
			{
				i++;

				if (card.m_itemData == null && i <= maxEquipped)
				{
					numFree ++;	
				}
			}
//			Debug.Log("NUM FREE " + numFree.ToString());
			return numFree;
		}
	}
}
