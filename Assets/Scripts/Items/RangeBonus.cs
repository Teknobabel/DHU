using UnityEngine;
using System.Collections;

public class RangeBonus : Item {

	public int
		m_damageBonus = 0;

	public override IEnumerator Activate ()
	{
		string newString = GameManager.m_gameManager.currentFollower.m_nameText + " uses " + m_name;
		UIManager.m_uiManager.UpdateActions (newString);

		InputManager.m_inputManager.cardsMoving = true;
		yield return StartCoroutine (CenterCard ());
		
		Player.m_player.rangedBonus += m_damageBonus;
		
		//Update Effect Stack
		EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
		newEffect.m_effectType = EffectsPanel.Effect.EffectType.TurnDamageBonus;
		newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
		string desc = m_name + ": ] attacks get +" + m_damageBonus.ToString() + "Range until next turn.";
		newEffect.m_description = desc;
		newEffect.m_spriteName = "Effect_DamageBonus";
		newEffect.m_affectedItem = this;
		EffectsPanel.m_effectsPanel.AddEffect(newEffect);

		UIManager.m_uiManager.SpawnFloatingText("+" + m_damageBonus.ToString() + " Range", UIManager.Icon.None, Player.m_player.m_playerMesh.transform);

		yield return StartCoroutine( PayForCard());
		yield return new WaitForSeconds(0.5f);
		
		yield return StartCoroutine (SendToGrave ());
		
		InputManager.m_inputManager.cardsMoving = false;
		yield return true;
	}
}
