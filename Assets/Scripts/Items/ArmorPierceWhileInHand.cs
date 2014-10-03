using UnityEngine;
using System.Collections;

public class ArmorPierceWhileInHand : Item {

	public override IEnumerator Activate ()
	{
		Player.m_player.doPermArmorPierce = true;
		
		//Update Effect Stack
		EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
		newEffect.m_effectType = EffectsPanel.Effect.EffectType.None;
		newEffect.m_effectDuration = EffectsPanel.Effect.Duration.WhileInHand;
		string desc = m_name + ": Gain Armor Piercing while in your hand.";
		newEffect.m_description = desc;
		newEffect.m_spriteName = "Effect_ArmorPierce";
		newEffect.m_stackable = true;
		newEffect.m_affectedItem = this;
		EffectsPanel.m_effectsPanel.AddEffect(newEffect);
		
		yield return true;
	}
	
	public override IEnumerator Deactivate ()
	{
		Player.m_player.doPermArmorPierce = false;
		
		EffectsPanel.m_effectsPanel.RemoveEffect (this);
		
		yield return true;
	}
}
