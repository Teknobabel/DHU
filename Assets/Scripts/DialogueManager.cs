using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour {
	
	public enum Action
	{
		Speaker01Speak,
		Speaker02Speak,
		CreateSpeaker01,
		CreateSpeaker02,
		Flash,
		Exit,
	}
	
	public class Act
	{
		public DialogueManager.Action
			m_action;
		
		public string
			m_dialogue,
			m_level;

		public Follower.FollowerType
			m_followerType;
	}
	
	public static DialogueManager 
		m_dialogueManager;
	
	public Camera
		m_inputCamera;
	
	public GameObject[]
		m_speakers;
	
//	public UILabel
//		m_speaker01,
//		m_speaker02;
	
	public UICard
		m_speaker01,
		m_speaker02;
	
	private List<string>
		m_dialogue = new List<string>();
	
	public GameObject
		m_settingsManager;
	
	private int
		m_currentLine = 0;
	
	//large size 3.035896
	//small size 2.56077

	// Use this for initialization
	void Awake () {
		m_dialogueManager = this;
		
		if (SettingsManager.m_settingsManager == null)
		{
			Instantiate(m_settingsManager, Vector3.zero, Quaternion.identity);	
		}
	}
	
	void Start () {
		
		int sceneNum = 0;
		if (SettingsManager.m_settingsManager != null)
		{
			sceneNum = SettingsManager.m_settingsManager.sceneNum;	
		}
		
		StartCoroutine(RunDialogue(LoadScene(sceneNum)));
	}
	
	void Update () {
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			Application.LoadLevel("MainMenu01");
		}
	}
	
	private IEnumerator RunDialogue (List<Act> sceneActs)
	{
		//yield return new WaitForSeconds(2);
		while (sceneActs.Count > 0)
		{
			Act thisAct = sceneActs[0];
			sceneActs.RemoveAt(0);
			
			if (thisAct.m_action == Action.CreateSpeaker01 || thisAct.m_action == Action.CreateSpeaker02)
			{
				foreach (GameObject fGO in m_speakers)
				{
					Follower f = (Follower)fGO.GetComponent("Follower");
					if (f.m_followerType == thisAct.m_followerType)
					{
						if (thisAct.m_action == Action.CreateSpeaker01)
						{
							m_speaker01.m_nameUI.text = f.m_nameText;
							m_speaker01.m_portrait.spriteName = f.m_portraitSpriteName;
							Debug.Log("speaker 1: " + f.m_nameText);
						} else if (thisAct.m_action == Action.CreateSpeaker02)
						{
							m_speaker02.m_nameUI.text = f.m_nameText;
							m_speaker02.m_portrait.spriteName = f.m_portraitSpriteName;
							Debug.Log("speaker 2: " + f.m_nameText);
						}
						break;
					}
				}
			} else if (thisAct.m_action == Action.Speaker01Speak || thisAct.m_action == Action.Speaker02Speak)
			{
				UICard lastSpeaker = null;
				
				if (thisAct.m_action == Action.Speaker01Speak)
				{
					lastSpeaker = m_speaker02;	
					m_speaker01.transform.localScale = Vector3.one * 3.035896f;
					m_speaker01.m_portrait.color = Color.white;
					m_speaker01.m_nameUI.color = Color.white;
					m_speaker02.transform.localScale = Vector3.one * 2.56077f;
					m_speaker02.m_portrait.color = new Color(0.25f,0.25f,0.25f,1);
					m_speaker02.m_nameUI.color = new Color(0.25f,0.25f,0.25f,1);
				} else if (thisAct.m_action == Action.Speaker02Speak)
				{
					lastSpeaker = m_speaker01;	
					m_speaker02.transform.localScale = Vector3.one * 3.035896f;
					m_speaker02.m_portrait.color = Color.white;
					m_speaker02.m_nameUI.color = Color.white;
					m_speaker01.transform.localScale = Vector3.one * 2.56077f;
					m_speaker01.m_portrait.color = new Color(0.25f,0.25f,0.25f,1);
					m_speaker01.m_nameUI.color = new Color(0.25f,0.25f,0.25f,1);
				}
				yield return StartCoroutine(StartLine(thisAct.m_dialogue, lastSpeaker.m_abilityUI));
				m_currentLine ++;
				yield return StartCoroutine(GetInput());
				yield return null;
			} else if (thisAct.m_action == Action.Exit)
			{
				//yield return StartCoroutine(GetInput());
				Application.LoadLevel(thisAct.m_level);
			}
				
				yield return null;
		}
		
		
//		UICard lastSpeaker = null;
		
//		yield return new WaitForSeconds(2);
//		while (m_currentLine < m_dialogue.Count)
//		{
//			if (lastSpeaker == null)
//			{
//				lastSpeaker = m_speaker01;	
//				m_speaker01.transform.localScale = Vector3.one * 3.035896f;
//				m_speaker02.transform.localScale = Vector3.one * 2.56077f;
//			} else if (lastSpeaker == m_speaker01)
//			{
//				lastSpeaker = m_speaker02;	
//				m_speaker02.transform.localScale = Vector3.one * 3.035896f;
//				m_speaker01.transform.localScale = Vector3.one * 2.56077f;
//			} else if (lastSpeaker == m_speaker02)
//			{
//				lastSpeaker = m_speaker01;	
//				m_speaker01.transform.localScale = Vector3.one * 3.035896f;
//				m_speaker02.transform.localScale = Vector3.one * 2.56077f;
//			}
//			yield return StartCoroutine(StartLine(m_currentLine, lastSpeaker.m_abilityUI));
//			m_currentLine ++;
//			yield return StartCoroutine(GetInput());
//			yield return null;
//		}
		yield return null;	
	}
	
	public IEnumerator StartLine (string line, UILabel speaker)
	{
//		if (m_currentLine < m_dialogue.Count)
//		{
			string thisLine = line;

			char[] CharList = thisLine.ToCharArray();
			//bool textActive = true;
			float textTime = 0.02f;
			float textTimer = 0;
			int charNum = 1;
			
			while (charNum <= thisLine.Length)
			{
				if (Input.GetMouseButtonUp(0))
				{
					Ray worldTouchRay = m_inputCamera.ScreenPointToRay(Input.mousePosition);
					RaycastHit hitInfo;
					if(Physics.Raycast(worldTouchRay, out hitInfo))
					{
						if (hitInfo.transform.gameObject.tag == "CraftButton")
						{
							charNum = thisLine.Length;
						}
						 else if (hitInfo.transform.gameObject.tag == "BackButton")
						{
							Application.LoadLevel("MainMenu01");
						}
					}
				}
			
				textTimer = Mathf.Clamp(textTimer + Time.deltaTime, 0, textTime);
				
				if (textTimer == textTime)
				{
					textTimer = 0;
					char[] parsedLine = new char[charNum];
					for (int i=0; i < parsedLine.Length; i++)
					{
						parsedLine[i] = CharList[i];	
					}
					charNum ++;
					string newString = new string(parsedLine);
					speaker.text = newString;
				}
				//textActive = false;
				
				yield return null;
			}
//		}
		yield return null;
	}
	
	private IEnumerator GetInput ()
	{
		bool buttonSelected = false;
		
		while (!buttonSelected)
		{
			if (Input.GetMouseButtonUp(0))
			{
				Ray worldTouchRay = m_inputCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hitInfo;
				if(Physics.Raycast(worldTouchRay, out hitInfo))
				{
					if (hitInfo.transform.gameObject.tag == "CraftButton")
					{
						return true;
					} else if (hitInfo.transform.gameObject.tag == "BackButton")
					{
						Application.LoadLevel("MainMenu01");
					}
				}
			}
			yield return null;	
		}
		yield return null;
	}
	
	private List<Act> LoadScene (int sceneNum)
	{
		List<Act> acts = new List<Act>();
		
		if (sceneNum == 0)
		{
			Act newAct01 = new Act();
			newAct01.m_action = Action.CreateSpeaker01;
			newAct01.m_followerType = Follower.FollowerType.Brand;
			acts.Add(newAct01);
			
			Act newAct02 = new Act();
			newAct02.m_action = Action.CreateSpeaker02;
			newAct02.m_followerType = Follower.FollowerType.Telina;
			acts.Add(newAct02);
			
			Act newAct03 = new Act();
			newAct03.m_action = Action.Speaker01Speak;
			newAct03.m_dialogue = "This is a line of dialogue.";
			acts.Add(newAct03);
			
			Act newAct04 = new Act();
			newAct04.m_action = Action.Speaker02Speak;
			newAct04.m_dialogue = "This too is text. Now I can play you the music of my people.";
			acts.Add(newAct04);
			
			Act newAct05 = new Act();
			newAct05.m_action = Action.Speaker02Speak;
			newAct05.m_dialogue = "I can't believe I lorem ipsumed the whole thing.";
			acts.Add(newAct05);
			
			Act newAct06 = new Act();
			newAct06.m_action = Action.Speaker01Speak;
			newAct06.m_dialogue = "ZA WARUDO!";
			acts.Add(newAct06);
			
			Act newAct07 = new Act();
			newAct07.m_action = Action.Exit;
			newAct07.m_level = "MainMenu01";
			acts.Add(newAct07);
		} else if (sceneNum == 1)
		{
			Act newAct01 = new Act();
			newAct01.m_action = Action.CreateSpeaker01;
			newAct01.m_followerType = Follower.FollowerType.Brand;
			acts.Add(newAct01);
			
			Act newAct02 = new Act();
			newAct02.m_action = Action.CreateSpeaker02;
			newAct02.m_followerType = Follower.FollowerType.August;
			acts.Add(newAct02);
			
			Act newAct03 = new Act();
			newAct03.m_action = Action.Speaker01Speak;
			newAct03.m_dialogue = "OMG I beat the game!";
			acts.Add(newAct03);
			
			Act newAct04 = new Act();
			newAct04.m_action = Action.Speaker02Speak;
			newAct04.m_dialogue = "I suppose there will be a proper ending here at some point.";
			acts.Add(newAct04);
			
			Act newAct05 = new Act();
			newAct05.m_action = Action.Speaker02Speak;
			newAct05.m_dialogue = "Damn lazy ass game developers.";
			acts.Add(newAct05);
			
			Act newAct06 = new Act();
			newAct06.m_action = Action.Exit;
			newAct06.m_level = "MainMenu01";
			acts.Add(newAct06);
		} else if (sceneNum == 2)
		{
			Act newAct01 = new Act();
			newAct01.m_action = Action.CreateSpeaker01;
			newAct01.m_followerType = Follower.FollowerType.Brand;
			acts.Add(newAct01);
			
			Act newAct02 = new Act();
			newAct02.m_action = Action.CreateSpeaker02;
			newAct02.m_followerType = Follower.FollowerType.August;
			acts.Add(newAct02);
			
			Act newAct03 = new Act();
			newAct03.m_action = Action.Speaker01Speak;
			newAct03.m_dialogue = "Is this the end already?!";
			acts.Add(newAct03);
			
			Act newAct04 = new Act();
			newAct04.m_action = Action.Speaker02Speak;
			newAct04.m_dialogue = "The end of the Demo. Damn lazy ass game developers...";
			acts.Add(newAct04);
			
			Act newAct05 = new Act();
			newAct05.m_action = Action.Speaker02Speak;
			newAct05.m_dialogue = "Follow Teknobabel on Twitter to follow development of the game...";
			acts.Add(newAct05);
			
			Act newAct06 = new Act();
			newAct06.m_action = Action.Exit;
			newAct06.m_level = "MainMenu01";
			acts.Add(newAct06);
		}
		
		return acts;
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}
}
