using UnityEngine;
using System.Collections;

public class ArmorDiceBuff : Item {

	public GameManager.Dice
		m_armorBonusDice;
	
	public override IEnumerator Activate ()
	{
		string newString = GameManager.m_gameManager.currentFollower.m_nameText + " uses " + m_name;
		UIManager.m_uiManager.UpdateActions (newString);

		InputManager.m_inputManager.cardsMoving = true;
		yield return StartCoroutine (CenterCard ());
		
		int numSides = 4;
		switch (m_armorBonusDice)
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
		
		int armor = Player.m_player.turnArmor;
		armor += diceRoll;
		
		//Update Effect Stack
		EffectsPanel.Effect newEffect = new EffectsPanel.Effect();
		newEffect.m_effectType = EffectsPanel.Effect.EffectType.TurnDamageBonus;
		newEffect.m_effectDuration = EffectsPanel.Effect.Duration.EndOfTurn;
		string desc = m_name + ": " + diceRoll.ToString() + "% until next turn.";
		newEffect.m_description = desc;
		newEffect.m_spriteName = "Effect_ArmorBonus";
		newEffect.m_affectedItem = this;
		EffectsPanel.m_effectsPanel.AddEffect(newEffect);
		
		Player.m_player.turnArmor = armor;
		UIManager.m_uiManager.SpawnFloatingText("+" + diceRoll.ToString(), UIManager.Icon.Armor, Player.m_player.m_playerMesh.transform);

		yield return StartCoroutine( PayForCard());
		yield return new WaitForSeconds(0.5f);
		
		yield return StartCoroutine (SendToGrave ());
		
		InputManager.m_inputManager.cardsMoving = false;
		yield return true;
	}
}
