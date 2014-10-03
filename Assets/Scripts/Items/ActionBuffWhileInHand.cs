using UnityEngine;
using System.Collections;

public class ActionBuffWhileInHand : Item {

	public int
		m_actionBonus = 0;
	
	public override IEnumerator Activate ()
	{
		Player.m_player.permActions += m_actionBonus;
		if (GameManager.m_gameManager.currentTurn == GameManager.Turn.Player) {
			Player.m_player.currentActionPoints += m_actionBonus;
		}
		
		//Update Effect Stack
		EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
		newEffect.m_effectType = EffectsPanel.Effect.EffectType.None;
		newEffect.m_effectDuration = EffectsPanel.Effect.Duration.WhileInHand;
		string desc = m_name + ": " + m_actionBonus.ToString() + "Actions per turn while in your hand.";
		newEffect.m_description = desc;
		newEffect.m_spriteName = "Effect_Placeholder";
		newEffect.m_stackable = true;
		newEffect.m_affectedItem = this;
		EffectsPanel.m_effectsPanel.AddEffect(newEffect);
		
		yield return true;
	}
	
	public override IEnumerator Deactivate ()
	{
		Player.m_player.permActions -= m_actionBonus;
		if (GameManager.m_gameManager.currentTurn == GameManager.Turn.Player) {
			Player.m_player.currentActionPoints -= m_actionBonus;
		}
		EffectsPanel.m_effectsPanel.RemoveEffect (this);
		
		yield return true;
	}
}
