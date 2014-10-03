using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FuseButton : MonoBehaviour {
	
	public UICard
		m_card;
	
	private List<UICard> m_cardList;
	
	private bool m_isHovered = false;
	
	private Item
		m_craftingResult;
	
	private float
		m_xOffset = 30;

	// Use this for initialization
	void Start () {
	
	}
	
	public void Initialize(List<UICard> fuseList)
	{
//		float yOffset = 0.1f;
//		float xOffset = 0;
//
//		UICard centerCard = fuseList[1];
//		
//		Vector3 pos = centerCard.transform.position;
//		pos.x += xOffset;
//		pos.y += yOffset;
//		transform.position = pos;
//		
//		m_cardList = new List<UICard>(fuseList);
//		
//		Item sourceItem = (Item)fuseList[0].itemData;
//		m_craftingResult = (Item)sourceItem.m_craftResult.GetComponent("Item");
//		m_card.m_nameUI.text = m_craftingResult.m_name;
//		m_card.m_portrait.spriteName = m_craftingResult.m_portraitSpriteName;
//		m_card.m_rankUI.text = m_craftingResult.m_keywordText;
//		m_card.m_abilityUI.text = m_craftingResult.m_description;
//
//		if (m_craftingResult.m_energyCost > 0)
//		{
//			m_card.m_healthUI.text = m_craftingResult.m_energyCost.ToString();
//			m_card.m_damageIcon.spriteName = "Icon_Energy";
//			m_card.m_healthUI.gameObject.SetActive(true);
//			m_card.m_damageIcon.gameObject.SetActive(true);
//		} else if (m_craftingResult.m_healthCost > 0)
//		{
//			m_card.m_healthUI.text = m_craftingResult.m_healthCost.ToString();
//			m_card.m_damageIcon.spriteName = "Icon_Health";
//			m_card.m_healthUI.gameObject.SetActive(true);
//			m_card.m_damageIcon.gameObject.SetActive(true);
//		} //else if (newItem.HasKeyword(Item.Keyword.Consumeable) || newItem.HasKeyword(Item.Keyword.Chain))
//		else
//		{
//			m_card.m_healthUI.gameObject.SetActive(false);
//			m_card.m_damageIcon.gameObject.SetActive(false);
//		}
//
//		m_card.m_miscOBJ [0].gameObject.SetActive (true);
	}
	
	public void Hover ()
	{
		m_isHovered = true;	
		
		//move outer cards together
		Vector3 pos = m_cardList[0].transform.localPosition;
		pos.x -= m_xOffset;
		m_cardList[0].transform.localPosition = pos;
		pos = m_cardList[2].transform.localPosition;
		pos.x += m_xOffset;
		m_cardList[2].transform.localPosition = pos;
		
		((BoxCollider)m_cardList[0].transform.GetComponent("BoxCollider")).enabled = false;
		((BoxCollider)m_cardList[1].transform.GetComponent("BoxCollider")).enabled = false;
		((BoxCollider)m_cardList[2].transform.GetComponent("BoxCollider")).enabled = false;
		
		m_card.gameObject.SetActive(true);
	}
	
	public void ClearSelection ()
	{
		m_isHovered = false;	
		
		//move cards back into place
		Vector3 pos = m_cardList[0].transform.localPosition;
		pos.x += m_xOffset;
		m_cardList[0].transform.localPosition = pos;
		pos = m_cardList[2].transform.localPosition;
		pos.x -= m_xOffset;
		m_cardList[2].transform.localPosition = pos;
		
		((BoxCollider)m_cardList[0].transform.GetComponent("BoxCollider")).enabled = true;
		((BoxCollider)m_cardList[1].transform.GetComponent("BoxCollider")).enabled = true;
		((BoxCollider)m_cardList[2].transform.GetComponent("BoxCollider")).enabled = true;
		
		m_card.gameObject.SetActive(false);
	}
	
	public bool isHovered {get{return m_isHovered;}}
	
	public List<UICard> cardList {get {return m_cardList;}}
	
	public Item craftingResult {get {return m_craftingResult;}}
}
