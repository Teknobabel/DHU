using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RemoveTrap : Item {

	public int
		m_range = 0;
	
	public TargetType
		m_targetType = TargetType.Target;

	private bool
		m_abort = false;
	
	public override IEnumerator Activate ()
	{
		List<GameManager.Direction> directions = new List<GameManager.Direction>();
		directions.Add(GameManager.Direction.North);
		directions.Add(GameManager.Direction.South);
		directions.Add(GameManager.Direction.East);
		directions.Add(GameManager.Direction.West);
		List<Card> validCards = MapManager.m_mapManager.GetCardsInRange(Player.m_player.currentCard, m_range, directions, false, false, false);

		List<Card> traps = new List<Card> ();

		foreach (Card vc in validCards) {
			if (vc.type != Card.CardType.Normal && vc.type != Card.CardType.Exit && vc.type != Card.CardType.Entrance && vc.type != Card.CardType.Gate)
			{
				traps.Add(vc);
				vc.ChangeHighlightState(true);
			}
		}

		if (traps.Count > 0)
		{

			GameManager.m_gameManager.selectMode = true;
			yield return StartCoroutine(InputManager.m_inputManager.GetMouseSelection(traps));	
			GameManager.m_gameManager.selectMode = false;
			
			if (GameManager.m_gameManager.selectedCard != null)
			{
				string newString = GameManager.m_gameManager.currentFollower.m_nameText + " uses " + m_name;
				UIManager.m_uiManager.UpdateActions (newString);

				Card c = GameManager.m_gameManager.selectedCard;

				newString = GameManager.m_gameManager.selectedCard.m_displayName + " is removed";
				UIManager.m_uiManager.UpdateActions (newString);

				//turn this card into a normal card
				if (GameManager.m_gameManager.currentMap.m_mapType == MapManager.Map.MapType.Plains)
				{
					Card newCard = ((Card)MapManager.m_mapManager.m_cardTypes[16].transform.GetComponent("Card"));
					yield return StartCoroutine(c.ChangeCardType(newCard));
				}
				else if (GameManager.m_gameManager.currentMap.m_mapType == MapManager.Map.MapType.Cave)
				{
					Card newCard = ((Card)MapManager.m_mapManager.m_cardTypes[16].transform.GetComponent("Card"));
					yield return StartCoroutine(c.ChangeCardType(newCard));
				} 
				else if (GameManager.m_gameManager.currentMap.m_mapType == MapManager.Map.MapType.Dungeon)
				{
					Card newCard = ((Card)MapManager.m_mapManager.m_cardTypes[4].transform.GetComponent("Card"));
					yield return StartCoroutine(c.ChangeCardType(newCard));
				}
				else if (GameManager.m_gameManager.currentMap.m_mapType == MapManager.Map.MapType.Crystal)
				{
					Card newCard = ((Card)MapManager.m_mapManager.m_cardTypes[19].transform.GetComponent("Card"));
					yield return StartCoroutine(c.ChangeCardType(newCard));
				}
				else if (GameManager.m_gameManager.currentMap.m_mapType == MapManager.Map.MapType.Molten)
				{
					Card newCard = ((Card)MapManager.m_mapManager.m_cardTypes[17].transform.GetComponent("Card"));
					yield return StartCoroutine(c.ChangeCardType(newCard));
				}
				else if (GameManager.m_gameManager.currentMap.m_mapType == MapManager.Map.MapType.Frozen)
				{
					Card newCard = ((Card)MapManager.m_mapManager.m_cardTypes[18].transform.GetComponent("Card"));
					yield return StartCoroutine(c.ChangeCardType(newCard));
				}

				GameManager.m_gameManager.selectedCard = null;
			} else {m_abort = true;}
			
			foreach (Card vc in traps)
			{
				vc.ChangeHighlightState(false);	
			}
		} else {m_abort = true;}

		if (!m_abort) {
			InputManager.m_inputManager.cardsMoving = true;
			yield return StartCoroutine (PayForCard ());
			yield return new WaitForSeconds (0.5f);
			yield return StartCoroutine (SendToGrave ());
			InputManager.m_inputManager.cardsMoving = false;
		}

		yield return true;
	}
}
