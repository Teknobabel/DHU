using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RemoveWounds : Item {

	public override IEnumerator Activate ()
	{
		string newString = "\\01" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 uses \\8" + m_name;
		UIManager.m_uiManager.UpdateActions (newString);

		InputManager.m_inputManager.cardsMoving = true;
		yield return StartCoroutine (CenterCard ());
		
		List<EffectsPanel.Effect> wounds = new List<EffectsPanel.Effect>();
		foreach (EffectsPanel.Effect e in EffectsPanel.m_effectsPanel.effectStack)
		{
			if (e.m_effectType == EffectsPanel.Effect.EffectType.Wound)
			{
				wounds.Add(e);
			}
		}

		if (wounds.Count > 0)
		{
			newString ="\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 loses all Wounds";
			UIManager.m_uiManager.UpdateActions (newString);

			foreach (EffectsPanel.Effect e in wounds)
			{
				EffectsPanel.m_effectsPanel.RemoveEffect(e);
			}
		}

		Player.m_player.wounds = 0;

		yield return StartCoroutine( PayForCard());
		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine (SendToGrave ());
		
		InputManager.m_inputManager.cardsMoving = false;
		yield return true;
	}
}
