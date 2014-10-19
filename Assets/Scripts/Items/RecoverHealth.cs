using UnityEngine;
using System.Collections;

public class RecoverHealth : Item {

	public int
		m_healthBonus = 0;

	public override IEnumerator Activate ()
	{
		string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 uses \\8" + m_name;
		UIManager.m_uiManager.UpdateActions (newString);

		InputManager.m_inputManager.cardsMoving = true;
		yield return StartCoroutine (CenterCard ());
		
		Player.m_player.GainHealth(m_healthBonus);	
		UIManager.m_uiManager.SpawnFloatingText("+" + m_healthBonus.ToString(), UIManager.Icon.Health, Player.m_player.m_playerMesh.transform);
			
		yield return StartCoroutine( PayForCard());
		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine (SendToGrave ());
		
		InputManager.m_inputManager.cardsMoving = false;
		yield return true;
	}
}
