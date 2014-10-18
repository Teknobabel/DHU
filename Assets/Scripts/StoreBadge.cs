using UnityEngine;
using System.Collections;

public class StoreBadge : MonoBehaviour {

	public enum State
	{
		Owned,
		NotOwned,
		Equipped,
		UnEquipped,
		Locked,
		BadgeSlot,
	}

	private State m_state = State.NotOwned;

	private int 
		m_price = -1,
		m_ID = -1;

	public UISprite 
		m_sprite,
		m_priceBG;

	public UILabel m_priceLabel;

	public GameObject m_soldSign;

	public UISprite[]
		m_miscSprites;

	public UILabel[]
		m_miscLabels;

	public GameObject
		m_badge;

	private Color m_startTextColor = Color.white;

	// Use this for initialization
	void Start () {

	}

	public void Initialize (int price, State state, int ID)
	{
		if (m_priceLabel != null)
		{
			m_startTextColor = m_priceLabel.color;
		}
		m_ID = ID;
		m_price = price;
		ChangeState (state);
	}

	public void ChangeState (State newState)
	{
		State oldState = m_state;

		UIButtonScale bs = (UIButtonScale)this.gameObject.GetComponent("UIButtonScale");
		UIButton b = (UIButton)this.gameObject.GetComponent("UIButton");

		switch (newState)
		{
		case State.Owned:
			m_state = State.Owned;
			m_sprite.color = Color.gray;
			m_priceLabel.gameObject.SetActive(false);
			m_priceBG.color = Color.gray;
			foreach (UISprite s in m_miscSprites)
			{
				s.color = Color.gray;
			}
			foreach (UILabel l in m_miscLabels)
			{
				l.color = Color.gray;
			}
			m_soldSign.gameObject.SetActive(true);
			bs.enabled = false;
			b.enabled = false;
			break;
		case State.NotOwned:
			m_state = State.NotOwned;
			m_sprite.color = Color.white;
			m_priceLabel.text = m_price.ToString();
			m_priceLabel.color = Color.white;
			bs.enabled = true;
			b.enabled = false;
			break;
		case State.Equipped:
			m_sprite.color = Color.black;
			m_priceBG.color = Color.black;
			m_priceLabel.color = Color.black;
			break;
		case State.UnEquipped:
			m_sprite.color = Color.white;
			m_priceBG.color = Color.white;
			m_priceLabel.color = m_startTextColor;

			if (oldState == State.Locked)
			{
				bs.enabled = true;
				b.enabled = true;
				m_priceLabel.color = m_startTextColor;
				m_priceBG.gameObject.SetActive(true);
				m_priceLabel.gameObject.SetActive(true);
				BoxCollider bc = (BoxCollider)this.GetComponent("BoxCollider");
				bc.enabled = true;
			}
			break;
		case State.Locked:
			m_sprite.color = Color.black;
			//m_priceLabel.color = Color.gray;
			BoxCollider bc = (BoxCollider)this.GetComponent("BoxCollider");
			bc.enabled = false;
			m_priceLabel.gameObject.SetActive(false);
			m_priceBG.gameObject.SetActive(false);
			bs.enabled = false;
			b.enabled = false;
			break;
		}

		m_state = newState;
	}

	void OnClick () {

		if (m_state == State.BadgeSlot)
		{
			if (m_priceBG.gameObject.activeSelf)
			{
				//find linked badge
				foreach (StoreBadge b in PartySelectMenu.m_partySelectMenu.m_storeBadges)
				{
					if (b.id == m_ID && b.m_state == State.Equipped)
					{
						//unequip linked badge
						b.ChangeState(State.UnEquipped);
						PartySelectMenu.m_partySelectMenu.currentBadges --;
						SettingsManager.m_settingsManager.badgeStates[m_ID] = 1;
						//MainMenu.m_mainMenu.badgeStatesChanged = true;
						ClearBadge();
						break;
					}
				}
			}
		}
		else if (m_state == State.NotOwned && SettingsManager.m_settingsManager.xp >= m_price && MainMenu.m_mainMenu.menuMode == UIManager.MenuMode.BadgeStore)
		{
			ChangeState(State.Owned);
			SettingsManager.m_settingsManager.badgeStates[m_ID] = 1;
			SettingsManager.m_settingsManager.xp -= m_price;
			MainMenu.m_mainMenu.badgeStatesChanged = true;
			MainMenu.m_mainMenu.m_typogenicText[0].Text = SettingsManager.m_settingsManager.xp.ToString ();

			//unlock in badge menu
			if (m_ID < MainMenu.m_mainMenu.m_badges.Length)
			{
				MainMenu.m_mainMenu.m_badges[m_ID].ChangeState(State.UnEquipped);
			}

			//increase max badges if needed
			if (m_ID >= 13 && m_ID <= 17)
			{
				MainMenu.m_mainMenu.maxBadges ++;
				MainMenu.m_mainMenu.m_labels [1].text = "BADGES CURRENTLY IN USE: " + MainMenu.m_mainMenu.currentBadges + "/" + MainMenu.m_mainMenu.maxBadges.ToString();

				//unlock new badge in Badges menu
				foreach (StoreBadge b in PartySelectMenu.m_partySelectMenu.m_badgeSlots)
				{
					if (!b.gameObject.activeSelf)
					{
						b.gameObject.SetActive(true);
						break;
					}
				}
			}

		} else if (m_state != State.Locked )
		{

			if (m_state == State.UnEquipped && PartySelectMenu.m_partySelectMenu.currentBadges < PartySelectMenu.m_partySelectMenu.partyCount )
			{
				ChangeState(State.Equipped);
				PartySelectMenu.m_partySelectMenu.currentBadges++;
				SettingsManager.m_settingsManager.badgeStates[m_ID] = 2;


				foreach (PartySelectMenu.PartySlot ps in PartySelectMenu.m_partySelectMenu.partySlots)
				{
					if (ps.m_badgeState == PartySelectMenu.PartySlot.State.Empty)
					{
						ps.m_badgePortrait.SetBadge(this);
						ps.m_badgeState = PartySelectMenu.PartySlot.State.Occupied;
						ps.m_badge = m_badge;
						break;
					}
				}
				// update active badges UI
//				foreach (StoreBadge b in PartySelectMenu.m_partySelectMenu.m_badgeSlots)
//				{
//					if (b.gameObject.activeSelf && b.m_sprite.spriteName == "BadgeSlot01")
//					{
//						b.SetBadge(this);
//						break;
//					}
//				}




				//MainMenu.m_mainMenu.badgeStatesChanged = true;
			} else if (m_state == State.Equipped)
			{
				ChangeState(State.UnEquipped);
				MainMenu.m_mainMenu.currentBadges --;
				SettingsManager.m_settingsManager.badgeStates[m_ID] = 1;
				MainMenu.m_mainMenu.badgeStatesChanged = true;

				foreach (StoreBadge b in PartySelectMenu.m_partySelectMenu.m_badgeSlots)
				{
					if (b.gameObject.activeSelf && b.m_priceLabel.text == m_priceLabel.text)
					{
						b.ClearBadge();
						break;
					}
				}
			}

			//MainMenu.m_mainMenu.m_labels [1].text = "BADGES CURRENTLY IN USE: " + MainMenu.m_mainMenu.currentBadges + "/" + MainMenu.m_mainMenu.maxBadges.ToString();
		}
	}

	public void SetBadge (StoreBadge newBadge)
	{
		m_sprite.spriteName = newBadge.m_sprite.spriteName;
		m_priceLabel.text = newBadge.m_priceLabel.text;
		m_priceLabel.color = newBadge.textColor;
		m_ID = newBadge.id;
		m_priceBG.gameObject.SetActive (true);
		m_priceLabel.gameObject.SetActive (true);
		MouseHover mh1 = (MouseHover)this.GetComponent ("MouseHover");
		MouseHover mh2 = (MouseHover)newBadge.transform.GetComponent ("MouseHover");
		mh1.m_text = mh2.m_text;
		BoxCollider bc = (BoxCollider)this.GetComponent("BoxCollider");
		bc.enabled = true;
		UIButtonScale bs = (UIButtonScale)this.gameObject.GetComponent("UIButtonScale");
		bs.enabled = true;
	}

	public void ClearBadge ()
	{
		m_sprite.spriteName = "BadgeSlot01";
		m_priceLabel.text = "";
		m_ID = -1;
		m_priceBG.gameObject.SetActive (false);
		m_priceLabel.gameObject.SetActive (false);
		MouseHover mh1 = (MouseHover)this.GetComponent ("MouseHover");
		mh1.m_text = "Empty, assign an Available Badge to gain its benefits.";

		UIButtonScale bs = (UIButtonScale)this.gameObject.GetComponent("UIButtonScale");
		bs.enabled = false;
	}

	public Color textColor {get{return m_startTextColor;}}
	public int id {get{return m_ID;}}
}
