using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class XMLManager {

	public List<List<string>> GetEnemyBank (int difficulty)
	{
		//set enemy difficulty values
		//int adjustedDiff = difficulty+1;
		int enemyDifficulty = difficulty;

//		if (difficulty == -6 || difficulty == -5)
//		{
//			enemyDifficulty = 16;
//		}else if (difficulty == -4)
//		{
//			enemyDifficulty = 17;
//		} else if (difficulty == -2)
//		{
//			enemyDifficulty = 18;
//		}else if (difficulty == -1)
//		{
//			enemyDifficulty = 19;
//		}
//		else if (adjustedDiff % 8 == 0)
//		{
//			if (adjustedDiff == 8)
//			{
//				enemyDifficulty = 3;
//			} else if (adjustedDiff == 16)
//			{
//				enemyDifficulty = 10;
//			} else if (adjustedDiff == 24)
//			{
//				enemyDifficulty = 15;
//			}
//		}
//		else 
//			if (adjustedDiff == 1)
//		{
//			enemyDifficulty = 0;	
//		} 
//		else if (adjustedDiff == 2)
//		{
//			enemyDifficulty = 1;	
//		} 
//		else if (adjustedDiff == 3)
//		{
//			enemyDifficulty = 2;	
//		} 
//		else if (adjustedDiff == 4)
//		{
//			enemyDifficulty = 3;	
//		} 
//		else if (adjustedDiff == 5)
//		{
//			enemyDifficulty = 4;	
//		} 
//		else if (adjustedDiff > 2 && adjustedDiff <= 4)
//		{
//			enemyDifficulty = 1;	
//		} 
//		else if (adjustedDiff > 4 && adjustedDiff <= 6)
//		{
//			enemyDifficulty = 2;
//		}
//		else if (adjustedDiff > 6 && adjustedDiff < 9)
//		{
//			enemyDifficulty = 3;
//		} else if (adjustedDiff == 9)
//		{
//			enemyDifficulty = 5;	
//		} else if (adjustedDiff == 10 || adjustedDiff == 11)
//		{
//			enemyDifficulty = 6;	
//		} else if (adjustedDiff == 12 || adjustedDiff == 13)
//		{
//			enemyDifficulty = 7;	
//		} else if (adjustedDiff == 14)
//		{
//			enemyDifficulty = 8;
//		} else if (adjustedDiff == 15)
//		{
//			enemyDifficulty = 9;
//		} else if (adjustedDiff == 17 || adjustedDiff == 18)
//		{
//			enemyDifficulty = 11;
//		} else if (adjustedDiff == 19 || adjustedDiff == 20)
//		{
//			enemyDifficulty = 12;
//		} else if (adjustedDiff == 21 || adjustedDiff == 22)
//		{
//			enemyDifficulty = 13;
//		} else if (adjustedDiff == 23)
//		{
//			enemyDifficulty = 14;
//		}

		List<List<string>> banks = new List<List<string>> ();
		TextAsset enemyBank = (TextAsset) Resources.Load ("Xml/EnemyBank");
		
		if (enemyBank != null) {
			Debug.Log("ENEMY BANK FILE FOUND");

			XmlDocument XmlDoc = new XmlDocument();
			XmlDoc.LoadXml(enemyBank.text);
			List<List<string>> validBankList = new List<List<string>>();

			XmlNodeList eBank = XmlDoc.GetElementsByTagName("EnemyBank");
			XmlNodeList bankList = eBank[0].ChildNodes;
			//XmlNodeList bankList = XmlDoc.GetElementsByTagName("Bank");
			foreach (XmlElement b in bankList)
			{
				List<string> enemies = new List<string>();
				if (b.GetAttribute("level") == enemyDifficulty.ToString())
				{
					foreach (XmlNode enemy in b)
					{
						enemies.Add(enemy.InnerText);
					}
				}

				if (enemies.Count > 0)
				{
					validBankList.Add(enemies);
				}
			}

			if (validBankList.Count > 0)
			{
				//Choose one bank from all available
				List<string> thisBank = validBankList[Random.Range(0, validBankList.Count)];
				//return thisBank;
				banks.Add(thisBank);
			} else {
				Debug.Log("VALID BANK LIST IS EMPTY");
			}

			//check for boss
			if (difficulty == 9 || difficulty == 19 || difficulty == 29)
			{
				XmlNodeList bBank = XmlDoc.GetElementsByTagName("BossBank");
				XmlNodeList bbankList = bBank[0].ChildNodes;

				foreach (XmlElement b in bbankList)
				{
					List<string> enemies = new List<string>();
					if (b.GetAttribute("level") == difficulty.ToString())
					{
						foreach (XmlNode enemy in b)
						{
							enemies.Add(enemy.InnerText);
						}
					}

					if (enemies.Count > 0)
					{
						banks.Add(enemies);
					}
				}

			}

		} else {
			Debug.Log("ENEMY BANK FILE NOT FOUND");
		}

		return banks;
	}
}
