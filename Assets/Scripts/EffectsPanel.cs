using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectsPanel : MonoBehaviour {
	
	[System.Serializable]
	public class Effect {
		
		public enum EffectType {
			None,
			Brand,
			CounterAttack,
			TurnArmorBonus,
			TurnDamageBonus,
			ArmorPierce,
			AttackDamageBonus,
			AttackArmorBonus,
			PlayerInflictPoison,
			PlayerStun,
			PlayerPoisoned,
			Dark,
			Tower,
			Fort,
			HighGround,
			RazorVine,
			Quicksand,
			Stalactites,
			Samurai,
			ReflectMeleeDamage,
			LifeTap,
			SoulTap,
			Rage,
			SoulArmor,
			Sacrifice,
			Cover,
			Wound,
			Corrupt,
			Faust,
		}
		
		public enum Duration {
			None,
			EndOfTurn,
			NextPlayerAttack,
			NextPlayerHit,
			WhilePresent,
			EndOfLevel,
			WhileInHand,
			EndOfRun,
		}
		
		public Effect.EffectType
			m_effectType = Effect.EffectType.None;
		
		public Effect.Duration
			m_effectDuration = Effect.Duration.None;
		
		public string
			m_description = null,
			m_spriteName = "Effect_Placeholder";
		
		public UISprite
			m_sprite = null;
		
		public bool
			m_stackable = true,
			m_visible = true;
		
		public Follower
			m_affectedFollower = null;

		public Item
			m_affectedItem = null;
	}
	
	public static EffectsPanel 
		m_effectsPanel;
	
	public UISprite[]
		m_effectSprites;

	public UIAnchor
		m_anchor;
	
	public GameObject
		m_tipParent,
		m_UIParent;
	
	public UILabel
		m_tipText;

	public Transform[]
		m_uiElements;
	
	private List<Effect>
		m_effectStack = new List<Effect>();

	private UISprite
		m_displayedSpriteTip = null;

	private bool
		m_dropDownMode = false;
	
	
	
	
	
	
	void Awake () {
		m_effectsPanel = this;
	}
	
	public void UpdateEffects (Effect.Duration duration)
	{
		//remove effects who have met their duration
		if (m_effectStack.Count > 0)
		{
			bool listIsClean = false;
			
			while (!listIsClean)
			{
				foreach (Effect thisEffect in m_effectStack)
				{
					if (thisEffect.m_effectDuration == duration)
					{
						if (thisEffect.m_affectedItem != null)
						{
//							Item i = (Item)thisEffect.m_affectedItem;
//							if (i.itemState == Item.ItemState.Spent)
//							{
//								i.ChangeState(Item.ItemState.Normal);
//							}
						}

						if (thisEffect.m_effectType == Effect.EffectType.PlayerPoisoned)
						{
							
							//Player.m_player.poisonDuration --;
							if (Player.m_player.poisonDuration == 0)
							{
								RemoveEffect(thisEffect);
								listIsClean = false;
								break;
							} else {
								
								thisEffect.m_description =  "Poisoned: " + Player.m_player.poisonDuration.ToString() + " turns";
							}
						}
						else if (thisEffect.m_effectType == Effect.EffectType.PlayerStun)
						{
							if (thisEffect.m_affectedFollower.stunDuration == 0)
							{
								RemoveEffect(thisEffect);
								listIsClean = false;
								break;
							}
						} else {
							RemoveEffect(thisEffect);
							listIsClean = false;
							break;
						}
					}
					listIsClean = true;
				}
				
				if (m_effectStack.Count == 0)
				{
					listIsClean = true;	
				}
			}
		} else {

			// Hide effects panel UI
//			if (m_UIParent.activeSelf)
//			{
//				m_UIParent.SetActive(false);
//			}
		}
	}
	
	public void AddEffect (Effect newEffect)
	{
		if (newEffect.m_stackable == false)
		{
			//if not stackable, don't add effect if identical effect is already in place
			foreach (Effect thisEffect in m_effectStack)
			{
				if (thisEffect.m_effectType == newEffect.m_effectType && thisEffect.m_effectDuration == newEffect.m_effectDuration)
				{
					if (thisEffect.m_effectType == Effect.EffectType.PlayerPoisoned)
					{
						thisEffect.m_description =  "Poisoned: " + Player.m_player.poisonDuration.ToString() + " turns";	
					}
					Debug.Log("NON STACKABLE EFFECT ALREADY EXISTS");
					return;	
				}
			}
		}
		
		//Debug.Log(newEffect.m_effectType);
		m_effectStack.Add(newEffect);	
		UpdatePanel();
		//Debug.Log("Num Effects: " + m_effectStack.Count);
	}

	public void RemoveEffect (Item item)
	{
		for (int i=0; i < m_effectStack.Count; i++)
		{
			Effect thisEffect = m_effectStack[i];
			if (thisEffect.m_affectedItem == item)
			{
				Debug.Log("REMOVING EFFECT");
				m_effectStack.RemoveAt(i);
				UpdatePanel();
				return;
			}
		}
		
		Debug.Log("EFFECT REMOVAL: REQUESTED EFFECT NOT FOUND");
	}
	
	public void RemoveEffect (Effect oldEffect)
	{
		for (int i=0; i < m_effectStack.Count; i++)
		{
			Effect thisEffect = m_effectStack[i];
			if (thisEffect == oldEffect)
			{
				Debug.Log("REMOVING EFFECT");
				m_effectStack.RemoveAt(i);
				UpdatePanel();
				return;
			}
		}
		
		Debug.Log("EFFECT REMOVAL: REQUESTED EFFECT NOT FOUND");
	}

	public void UpdateEffect (Effect e)
	{
		foreach (Effect thisEffect in m_effectStack)
		{
			if (thisEffect == e)
			{
				if (thisEffect.m_effectType == Effect.EffectType.PlayerPoisoned)
				{

					if (Player.m_player.poisonDuration == 0)
					{
						RemoveEffect(thisEffect);
					} else {
						thisEffect.m_description =  "Poisoned: " + Player.m_player.poisonDuration.ToString() + " turns";
					}
				}

				return;
			}
		}
	}
	
	public void UpdatePanel ()
	{
		bool visible = false;
		foreach (UISprite sprite in m_effectSprites)
		{
			sprite.gameObject.SetActive(false);	
		}
		int numSprite = 0;
		foreach (Effect thisEffect in m_effectStack)
		{
			if (numSprite < m_effectSprites.Length && thisEffect.m_visible)
			{
				UISprite sprite = (UISprite)m_effectSprites[numSprite];
				sprite.spriteName = thisEffect.m_spriteName;
				sprite.gameObject.SetActive(true);
				thisEffect.m_sprite = sprite;
				numSprite ++;	
				visible = true;
			}
		}

		if (!visible && m_UIParent.activeSelf)
		{
			m_UIParent.gameObject.SetActive(false);
		} else if (visible && !m_UIParent.activeSelf)
		{
			m_UIParent.gameObject.SetActive(true);
		}
	}
	
	public void DisplayTip (UISprite sprite)
	{
		if (m_effectStack.Count > 0)
		{
			
			foreach (Effect e in m_effectStack)
			{
				if (e.m_sprite == sprite)
				{
					//show tool tip
					Vector3 tipPos = m_tipParent.transform.localPosition;
					if (m_effectStack.Count < 7)
					{
						tipPos.y = 615.4717f;	
//						if (m_dropDownMode){tipPos.y = -103.9039f;}
					} else {
						tipPos.y = 579.1042f;
					}
//						else if (m_effectStack.Count < 7)
//					{
//						tipPos.y = 110.4f;
//						if (m_dropDownMode){tipPos.y = -137.0841f;}
//					} else if (m_effectStack.Count < 10)
//					{
//						tipPos.y = 145.0f;
//						if (m_dropDownMode){tipPos.y = -172.2645f;}
//					} else if (m_effectStack.Count < 13)
//					{
//						tipPos.y = 178.5f;
//						if (m_dropDownMode){tipPos.y = -203.9237f;}
//					}
					
					string s = e.m_description;
					m_tipText.text = s;
					m_tipParent.transform.localPosition = tipPos;
					m_tipParent.gameObject.SetActive(true);
					m_displayedSpriteTip = sprite;
					//Debug.Log(s);
				}
			}
		}
	}
	
	public void DeactivateTip ()
	{
		m_displayedSpriteTip = null;
		m_tipParent.gameObject.SetActive(false);
	}

	public void SetAnchor (int anchor)
	{
		if (anchor == 1)
		{
			m_dropDownMode = true;
			m_anchor.side = UIAnchor.Side.TopRight;
			Vector3 aPos = this.gameObject.transform.localPosition;
			aPos.x = -219.8438f;
			aPos.y = -65.94568f;
			this.gameObject.transform.localPosition = aPos;
			aPos = m_uiElements[0].localPosition;
			aPos.y = -0.6762489f;
			m_uiElements[0].localPosition = aPos;

			aPos = m_uiElements[1].localPosition;
			aPos.y = -47.12103f;
			m_uiElements[1].localPosition = aPos;

			aPos = m_uiElements[2].localPosition;
			aPos.y = -80.70538f;
			m_uiElements[2].localPosition = aPos;

			aPos = m_uiElements[3].localPosition;
			aPos.y = -115.1301f;
			m_uiElements[3].localPosition = aPos;

			aPos = m_uiElements[4].localPosition;
			aPos.y = -148.8624f;
			m_uiElements[4].localPosition = aPos;

			aPos = m_uiElements[5].localPosition;
			aPos.y = -205.2342f;
			m_uiElements[5].localPosition = aPos;
		}
	}
	
	
	public UISprite displayedSpriteTip {get{return m_displayedSpriteTip;}}
	public List<Effect> effectStack{get{return m_effectStack;}}

}
