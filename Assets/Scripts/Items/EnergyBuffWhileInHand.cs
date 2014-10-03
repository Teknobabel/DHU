using UnityEngine;
using System.Collections;

public class EnergyBuffWhileInHand : Item {

	public int
		m_energyBonus = 0;
	
	public override IEnumerator Activate ()
	{
		Player.m_player.permEnergy += m_energyBonus;
		
		//Update Effect Stack
		EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
		newEffect.m_effectType = EffectsPanel.Effect.EffectType.None;
		newEffect.m_effectDuration = EffectsPanel.Effect.Duration.WhileInHand;
		string desc = m_name + ": " + m_energyBonus.ToString() + "# while in your hand.";
		newEffect.m_description = desc;
		newEffect.m_spriteName = "Effect_Placeholder";
		newEffect.m_stackable = true;
		newEffect.m_affectedItem = this;
		EffectsPanel.m_effectsPanel.AddEffect(newEffect);

		UIManager.m_uiManager.UpdateEnergy(Player.m_player.currentEnergy);
		
		yield return true;
	}
	
	public override IEnumerator Deactivate ()
	{
		Player.m_player.permEnergy -= m_energyBonus;
		if (Player.m_player.currentEnergy > Player.m_player.maxEnergy) {
			Player.m_player.currentEnergy = Player.m_player.maxEnergy;
		}

		UIManager.m_uiManager.UpdateEnergy(Player.m_player.currentEnergy);
		
		EffectsPanel.m_effectsPanel.RemoveEffect (this);
		
		yield return true;
	}
}
