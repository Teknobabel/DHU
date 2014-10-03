using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveInRange : Item {

	public int
		m_range = 0;

	private bool
		m_abort = false;
	
	public override IEnumerator Activate ()
	{
		List<GameManager.Direction> moveDir = new List<GameManager.Direction>();
		moveDir.Add(GameManager.Direction.North);
		moveDir.Add(GameManager.Direction.South);
		moveDir.Add(GameManager.Direction.East);
		moveDir.Add(GameManager.Direction.West);
		List<Card> vcards = MapManager.m_mapManager.GetCardsInRange(Player.m_player.currentCard, m_range, moveDir, true, false, false);

		List<Card> vc = new List<Card> ();
		foreach (Card card in vcards) {
			if (!card.isOccupied)
			{
				vc.Add(card);
				card.ChangeHighlightState(true);
			}
		}

		if (vc.Count > 0)
		{
			GameManager.m_gameManager.selectMode = true;
			yield return StartCoroutine(InputManager.m_inputManager.GetMouseSelection(vc));	
			GameManager.m_gameManager.selectMode = false;
			
			if (GameManager.m_gameManager.selectedCard != null)
			{
				Card c = GameManager.m_gameManager.selectedCard;
				Card pc = Player.m_player.currentCard;

				if (!c.isOccupied)
				{
					GameManager.Direction d = GameManager.Direction.None;
					//determine the movement direction

					if (c.column > pc.column && c.row == pc.row)
					{
						d = GameManager.Direction.East;
					} else if (c.column < pc.column && c.row == pc.row)
					{
						d = GameManager.Direction.West;
					}else if (c.column == pc.column && c.row < pc.row)
					{
						d = GameManager.Direction.North;
					}else if (c.column == pc.column && c.row > pc.row)
					{
						d = GameManager.Direction.South;
					}

					Player.m_player.ChangeFacing(d);
					//move in that direction until selected card is reached
					if (d != GameManager.Direction.None)
					{
						string newString = GameManager.m_gameManager.currentFollower.m_nameText + " uses " + m_name;
						UIManager.m_uiManager.UpdateActions (newString);

						bool atDestination = false;
						while (!atDestination)
						{
							//get next card
							Card[] linkedCards = Player.m_player.currentCard.linkedCards;
							Card nextCard = linkedCards[0];
							if (d == GameManager.Direction.South)
							{
								nextCard = linkedCards[1];
							} else if (d == GameManager.Direction.East)
							{
								nextCard = linkedCards[2];
							}if (d == GameManager.Direction.West)
							{
								nextCard = linkedCards[3];
							}

							yield return StartCoroutine(Player.m_player.DoMove(nextCard));

							if (Player.m_player.currentCard.id == c.id)
							{
								atDestination = true;
								GameManager.m_gameManager.acceptInput = true;
							}
						}
					}
				} else {m_abort = true;}
				
				GameManager.m_gameManager.selectedCard = null;
			} else {m_abort = true;}
			
			foreach (Card card in vc)
			{
				card.ChangeHighlightState(false);	
			}

		} else {m_abort = true;}

		if (!m_abort) {
			InputManager.m_inputManager.cardsMoving = true;
			yield return StartCoroutine (PayForCard ());
			yield return new WaitForSeconds (0.5f);
			yield return StartCoroutine (SendToGrave ());
			InputManager.m_inputManager.cardsMoving = false;
		}
	}
}
