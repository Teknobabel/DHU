using UnityEngine;
using System.Collections;

public class RecoverEnergyPerEnemyInGrave : Item {

	public int
		m_energyBonus = 0;

	public override IEnumerator Activate ()
	{
		InputManager.m_inputManager.cardsMoving = true;
		yield return StartCoroutine (CenterCard ());

		int bonus = 0;

		foreach (GameManager.GraveSlot gs in GameManager.m_gameManager.grave)
		{
			if (gs.type == GameManager.GraveSlot.ObjectType.Enemy && gs.enemy != null)
			{
				bonus += m_energyBonus;
			}
		}

		if (bonus > 0) {

			string newString = GameManager.m_gameManager.currentFollower.m_nameText + " uses " + m_name;
			UIManager.m_uiManager.UpdateActions (newString);

			GameObject pFX = AssetManager.m_assetManager.m_particleFX[1];
			Instantiate(pFX, Player.m_player.m_playerMesh.transform.position, pFX.transform.rotation);
			UIManager.m_uiManager.SpawnFloatingText("+" + bonus.ToString(), UIManager.Icon.Energy, Player.m_player.m_playerMesh.transform);
			Player.m_player.GainEnergy(bonus);

		}

		yield return StartCoroutine( PayForCard());
		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine (SendToGrave ());
		InputManager.m_inputManager.cardsMoving = false;
		
		yield return true;
	}
}
