using UnityEngine;
using System.Collections;

public class GridCard : MonoBehaviour {

	public Material
		m_material;

	public string
		m_name = "Name",
		m_lockedDescription = "Locked Description",
		m_unlockedDescription = "Unlocked Description";

	public Card.CardType
		m_type = Card.CardType.Normal;
	
}
