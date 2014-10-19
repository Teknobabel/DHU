using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlipInRange : Item {

	public int 
		m_range = 0;

	public override IEnumerator Activate ()
	{
		List<GameManager.Direction> directions = new List<GameManager.Direction>();
		directions.Add(GameManager.Direction.North);
		directions.Add(GameManager.Direction.South);
		directions.Add(GameManager.Direction.East);
		directions.Add(GameManager.Direction.West);
		List<Card> validCards = MapManager.m_mapManager.GetCardsInRange(Player.m_player.currentCard, m_range, directions, false, false, true);
		
		List <Card> hidden = new List<Card> ();
		foreach (Card c in validCards)
		{
			if (c.cardState == Card.CardState.Hidden)
			{
				hidden.Add(c);
			}
		}

		if (hidden.Count > 0) {
			string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 uses \\8" + m_name;
			UIManager.m_uiManager.UpdateActions (newString);

			foreach (Card thisCard in hidden)
			{
				if (thisCard.cardState == Card.CardState.Hidden)
				{
					Player.m_player.GainEnergy(thisCard.GetEnergyValue());

					if (Player.m_player.currentEnergy < Player.m_player.maxEnergy)
					{
						UIManager.m_uiManager.SpawnFloatingText("+1",UIManager.Icon.Energy, thisCard.transform);
						
						if (!thisCard.isOccupied)
						{
							GameObject pFX = AssetManager.m_assetManager.m_particleFX[1];
							Instantiate(pFX, thisCard.transform.position, pFX.transform.rotation);
						}
					}

					yield return StartCoroutine(thisCard.ChangeCardState(Card.CardState.Normal));
				}
			}

			if (!GameManager.m_gameManager.acceptInput)
			{
				GameManager.m_gameManager.acceptInput = true;
			}
		} else {
			// abort
		}

		yield return StartCoroutine( PayForCard());
		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine (SendToGrave ());

		yield return true;
	}
}
