using UnityEngine;
using System.Collections;

public class RangedDamageBuffWhileInHand : Item {

	public int
		m_rangedDamageBonus = 0;
	
	public override IEnumerator Activate ()
	{
		Player.m_player.permRangedDamageBonus += m_rangedDamageBonus;
		
		//Update Effect Stack
		EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
		newEffect.m_effectType = EffectsPanel.Effect.EffectType.None;
		newEffect.m_effectDuration = EffectsPanel.Effect.Duration.WhileInHand;
		string desc = m_name + ": Ranged attacks gain +" + m_rangedDamageBonus.ToString() + " damage while in your hand.";
		newEffect.m_description = desc;
		newEffect.m_spriteName = "Effect_DamageBonus";
		newEffect.m_stackable = true;
		newEffect.m_affectedItem = this;
		EffectsPanel.m_effectsPanel.AddEffect(newEffect);
		
		yield return true;
	}
	
	public override IEnumerator Deactivate ()
	{
		Player.m_player.permRangedDamageBonus -= m_rangedDamageBonus;
		
		EffectsPanel.m_effectsPanel.RemoveEffect (this);
		
		yield return true;
	}
}
