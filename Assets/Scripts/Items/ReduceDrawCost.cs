using UnityEngine;
using System.Collections;

public class ReduceDrawCost : Item {

	public int
		m_drawCostReduction = 0;

	public override IEnumerator Activate ()
	{
		string newString = "\\01" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 uses \\8" + m_name;
		UIManager.m_uiManager.UpdateActions (newString);

		InputManager.m_inputManager.cardsMoving = true;
		yield return StartCoroutine (CenterCard ());
		
		GameManager.m_gameManager.drawCost = Mathf.Clamp (GameManager.m_gameManager.drawCost - m_drawCostReduction, 0, 99);
		
		//Update Effect Stack
		EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
		newEffect.m_effectType = EffectsPanel.Effect.EffectType.None;
		newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
		string desc = m_name + ": Reduce Draw Cost by " + m_drawCostReduction + " until next turn.";
		newEffect.m_description = desc;
		newEffect.m_spriteName = "Effect_Placeholder";
		newEffect.m_stackable = false;
		newEffect.m_affectedItem = this;
		EffectsPanel.m_effectsPanel.AddEffect(newEffect);

		yield return StartCoroutine( PayForCard());
		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine (SendToGrave ());
		
		InputManager.m_inputManager.cardsMoving = false;
		yield return true;
	}
}
