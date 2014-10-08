using UnityEngine;
using System.Collections;

public class DamageDiceBuff : Item {

	public GameManager.Dice
		m_attackBonusDice;

	public override IEnumerator Activate ()
	{
		string newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 uses \\8" + m_name;
		UIManager.m_uiManager.UpdateActions (newString);

		InputManager.m_inputManager.cardsMoving = true;
		yield return StartCoroutine (CenterCard ());

		int numSides = 4;
		switch (m_attackBonusDice)
		{
		case GameManager.Dice.D1:
			numSides = 1;
			break;
		case GameManager.Dice.D6:
			numSides = 6;
			break;
		case GameManager.Dice.D8:
			numSides = 8;
			break;
		case GameManager.Dice.D10:
			numSides = 10;
			break;
		case GameManager.Dice.D12:
			numSides = 12;
			break;
		case GameManager.Dice.D20:
			numSides = 20;
			break;
		}
		
		int diceRoll = Random.Range(1, numSides+1);

		newString = "\\1" + GameManager.m_gameManager.currentFollower.m_nameText + "\\0 rolls a " + diceRoll.ToString();
		UIManager.m_uiManager.UpdateActions (newString);

		Player.m_player.GainTurnDamage (diceRoll);
		//Player.m_player.turnDamage += diceRoll;
		//damage += diceRoll;
		
		//Update Effect Stack
		EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
		newEffect.m_effectType = EffectsPanel.Effect.EffectType.TurnDamageBonus;
		newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
		string desc = m_name + ": " + diceRoll.ToString() + "$ until next turn.";
		newEffect.m_description = desc;
		newEffect.m_spriteName = "Effect_DamageBonus";
		newEffect.m_affectedItem = this;
		EffectsPanel.m_effectsPanel.AddEffect(newEffect);
		
		//Player.m_player.turnDamage = damage;
		UIManager.m_uiManager.SpawnFloatingText("+" + diceRoll.ToString(), UIManager.Icon.MeleeDamage, Player.m_player.m_playerMesh.transform);

		yield return StartCoroutine( PayForCard());
		yield return new WaitForSeconds(0.5f);
		
		yield return StartCoroutine (SendToGrave ());
		
		InputManager.m_inputManager.cardsMoving = false;
		yield return true;
	}

}
