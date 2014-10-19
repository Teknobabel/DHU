using UnityEngine;
using System.Collections;

public class RecoverEnergy : Item {

	public int
		m_energyBonus = 0;
	
	public override IEnumerator Activate ()
	{
		string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 uses \\8" + m_name;
		UIManager.m_uiManager.UpdateActions (newString);

		InputManager.m_inputManager.cardsMoving = true;
		yield return StartCoroutine (CenterCard ());
		
		GameObject pFX = AssetManager.m_assetManager.m_particleFX[1];
		Instantiate(pFX, Player.m_player.m_playerMesh.transform.position, pFX.transform.rotation);
		UIManager.m_uiManager.SpawnFloatingText("+" + m_energyBonus.ToString(), UIManager.Icon.Energy, Player.m_player.m_playerMesh.transform);
		Player.m_player.GainEnergy(m_energyBonus);

		yield return StartCoroutine( PayForCard());
		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine (SendToGrave ());
		
		InputManager.m_inputManager.cardsMoving = false;
		yield return true;
	}
}
