using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecoverEnergyPerEnemyInRange : Item {

	public int
		m_energyBonus = 0,
		m_range = 0;
	
	public override IEnumerator Activate ()
	{
		InputManager.m_inputManager.cardsMoving = true;
		yield return StartCoroutine (CenterCard ());

		List<GameManager.Direction> directions = new List<GameManager.Direction>();
		directions.Add(GameManager.Direction.North);
		directions.Add(GameManager.Direction.South);
		directions.Add(GameManager.Direction.East);
		directions.Add(GameManager.Direction.West);
		List<Card> validCards = MapManager.m_mapManager.GetCardsInRange(Player.m_player.currentCard, m_range, directions, false, false, false);

		int bonusEnergy = 0;
		List <Enemy> enemies = new List<Enemy> ();
		foreach (Card c in validCards)
		{
			if (c.enemy != null && c.cardState != Card.CardState.Hidden)
			{
				bonusEnergy += m_energyBonus;
			}
		}

		if (bonusEnergy > 0) {

			string newString = "\\01" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 uses \\8" + m_name;
			UIManager.m_uiManager.UpdateActions (newString);

			GameObject pFX = AssetManager.m_assetManager.m_particleFX [1];
			Instantiate (pFX, Player.m_player.m_playerMesh.transform.position, pFX.transform.rotation);
			UIManager.m_uiManager.SpawnFloatingText ("+" + bonusEnergy.ToString (), UIManager.Icon.Energy, Player.m_player.m_playerMesh.transform);
			Player.m_player.GainEnergy (bonusEnergy);
		}

		yield return StartCoroutine( PayForCard());
		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine (SendToGrave ());
		
		InputManager.m_inputManager.cardsMoving = false;
		yield return true;
	}
}
