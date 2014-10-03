using UnityEngine;
using System.Collections;

public class HealthBuffWhileInHand : Item {

	public int
		m_healthBonus = 0;
	
	public override IEnumerator Activate ()
	{
		Player.m_player.permHealth += m_healthBonus;
		
		//Update Effect Stack
		EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
		newEffect.m_effectType = EffectsPanel.Effect.EffectType.None;
		newEffect.m_effectDuration = EffectsPanel.Effect.Duration.WhileInHand;
		string desc = m_name + ": " + m_healthBonus.ToString() + "& while in your hand.";
		newEffect.m_description = desc;
		newEffect.m_spriteName = "Effect_Placeholder";
		newEffect.m_stackable = true;
		newEffect.m_affectedItem = this;
		EffectsPanel.m_effectsPanel.AddEffect(newEffect);

		UIManager.m_uiManager.UpdateHealth(Player.m_player.currentHealth);
		
		yield return true;
	}
	
	public override IEnumerator Deactivate ()
	{
		Player.m_player.permHealth -= m_healthBonus;
		if (Player.m_player.currentHealth > Player.m_player.maxHealth) {
			Player.m_player.currentHealth = Player.m_player.maxHealth;
		}
		
		EffectsPanel.m_effectsPanel.RemoveEffect (this);
		UIManager.m_uiManager.UpdateHealth(Player.m_player.currentHealth);
		
		yield return true;
	}
}
