using UnityEngine;
using System.Collections;

public class HeroPortraitHover : MonoBehaviour {

	private Vector3
		m_hoverPos,
		m_normalPos;

	private UICard
		m_card;

	void Start ()
	{
		m_normalPos = this.transform.localPosition;
		m_hoverPos = this.transform.localPosition;
		m_hoverPos.x = -105.9492f;

		m_card = (UICard) this.GetComponent ("UICard");
	}

	void OnMouseEnter()
	{
		if (m_card.m_followerData != null) {
			if (m_card.m_followerData != GameManager.m_gameManager.currentFollower)
			{
//				transform.localPosition = m_hoverPos;
				m_card.m_miscOBJ[3].gameObject.SetActive(true);
			}
		}
	}

	void OnMouseExit()
	{
		if (m_card.m_followerData != null) {
			if (m_card.m_followerData != GameManager.m_gameManager.currentFollower)
			{
//				transform.localPosition = m_normalPos;
				m_card.m_miscOBJ[3].gameObject.SetActive(false);
			}
		}
	}
}
