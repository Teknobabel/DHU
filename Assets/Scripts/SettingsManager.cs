using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SettingsManager : MonoBehaviour {
	
	public static SettingsManager
		m_settingsManager;
	
	private int
		m_difficultyLevel = 0,
		m_lastRandomSeed = 99,
		m_startingHealth = 0,
		m_startingEnergy = 0,
		m_startingDamage = 0,
		m_startingArmor = 0,
		m_baseHealth = 3,
		m_baseDamage = 1,
		m_baseEnergy = 10,
		m_baseArmor = 0,
		m_sceneNum = 0,
		m_gold = 0,
		m_demoEnd = 23,
		m_levelsTravelled = 0,
		m_xp = 0,
		m_startChapter = -1;
	
	private string
		m_version = "0.0.833";
	
	private List<Follower>
		m_party = null;
	
	private List<GameState.ProgressState>
		m_gameProgress;

	private List<int>
		m_badgeStates = new List<int>(),
		m_trialStates = new List<int>(),
		m_shortcutStates = new List<int>();

	private bool[] m_gridCardStates;

	private GameState
		m_gameState;

	private float 
		m_gameSpeed = 1.0f;

	private bool
		m_demo = false,
		m_trial = false;

	private List<PartySelectMenu.PartySlot>
		m_partySlots = new List<PartySelectMenu.PartySlot>();
	
	void Awake ()
	{
		m_settingsManager = this;	
		DontDestroyOnLoad(this.gameObject);
	}
	
	void Start () {
	
	}
	
	public void EndLevel (int difficultyIncrease)
	{
		if (difficultyLevel == -1)
		{
			if (PlayerPrefs.GetInt("DoTutorial") == 0)
			{
				PlayerPrefs.SetInt("DoTutorial", 1);
				PlayerPrefs.Save();

				//refresh party's health and energy
				if (Player.m_player != null)
				{
					Player.m_player.GainHealth(99);
					Player.m_player.GainEnergy(99);
				}
			}
		}

		m_difficultyLevel += difficultyIncrease;			
	}

	public void RemoveBadges ()
	{
		if (m_badgeStates.Count > 0) {
			for (int i = 0; i < m_badgeStates.Count; i ++)
			{
				if (m_badgeStates[i] == 2)
				{
					m_badgeStates[i] = 1;
				}
			}
		}
	} 

	public string version{get{return m_version;}}
	public int difficultyLevel{get{return m_difficultyLevel;} set {m_difficultyLevel = value;}}
	public int lastRandomSeed{get{return m_lastRandomSeed;} set{m_lastRandomSeed = value;}}
	public int startingHealth{get{return m_startingHealth;} set{m_startingHealth = value;}}
	public int startingEnergy{get{return m_startingEnergy;} set{m_startingEnergy = value;}}
	public int startingDamage{get{return m_startingDamage;} set{m_startingDamage = value;}}
	public int startingArmor{get{return m_startingArmor;} set{m_startingArmor = value;}}
	public int baseDamage {get{return m_baseDamage;}}
	public int baseHealth {get{return m_baseHealth;}}
	public int baseEnergy {get{return m_baseEnergy;}}
	public int baseArmor {get{return m_baseArmor;}}
	public int gold{ get { return m_gold; } set { m_gold = value; } }
	public int sceneNum{get{return m_sceneNum;} set{m_sceneNum = value;}}
	public int demoEnd{get{return m_demoEnd;}}
	public List<Follower> party {get{return m_party;}set{m_party = value;}}
	public List<int> badgeStates {get{return m_badgeStates;}set{m_badgeStates = value;}}
	public List<int> trialStates {get{return m_trialStates;}set{m_trialStates = value;}}
	public List<int> shortcutStates {get{return m_shortcutStates;}set{m_shortcutStates = value;}}
	public List<GameState.ProgressState> gameProgress {get{return m_gameProgress;} set{m_gameProgress = value;}}
	public GameState gameState { get { return m_gameState; } set { m_gameState = value; } }
	public bool demo {get{return m_demo;}set{m_demo = value;}}
	public bool trial {get{return m_trial;}set{m_trial = value;}}
	public int levelsTravelled {get{return m_levelsTravelled;} set{m_levelsTravelled = value;}}
	public int xp {get{return m_xp;} set {m_xp = value;}}
	public int startChapter {get{return m_startChapter;} set {m_startChapter = value;}}
	public List<PartySelectMenu.PartySlot> partySlots { get { return m_partySlots; } set { m_partySlots = value;}}
	public bool[] gridCardStates {get{return m_gridCardStates;} set{m_gridCardStates = value;}}
	public float gameSpeed {get{ return m_gameSpeed; } set{m_gameSpeed = value;}}
}
