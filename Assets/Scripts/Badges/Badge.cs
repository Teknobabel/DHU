using UnityEngine;
using System.Collections;

public class Badge : MonoBehaviour {

	public enum BadgeType
	{
		None,
		Passive, //always active
		Useable, // user initiates use
		Conditional, // activates when conditions are met
		Conditional_Leader, // active only while equipped hero is the leader
		Conditional_LostSoul,
		Conditional_EndLevel,
	}

	public string
		m_badgeName = "Badge Name",
		m_badgeDescription = "Badge Description",
		m_badgePortraitSpriteName = "Grphc_DHeart01";

	public BadgeType
		m_badgeType = BadgeType.None;

	public int
		m_energyCost = 0,
		m_healthCost = 0;

	private Follower
		m_attachedFollower;

	public virtual IEnumerator Activate ()
	{
		yield return true;
	}
	
	public virtual IEnumerator Deactivate ()
	{
		yield return true;
	}
}
