using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.Text.RegularExpressions;

public class GameSerializer
{
	private static GameSerializer m_instance = null;
	private Dictionary<string, string> m_gameData;
	private bool m_compressData = true;
	
	// Singleton property
	public static GameSerializer Instance
	{
		get
		{
			if(m_instance == null)
			{
				m_instance = new GameSerializer();
			}
			return m_instance;
		}
	}
	
	public bool CompressData
	{
		get
		{
			return m_compressData;
		}
		set
		{
			m_compressData = value;
		}
	}
	
	private GameSerializer()
	{
		m_instance = this;
		m_gameData = new Dictionary<string, string>();
	}
	
	public bool SerializedDataExists(string saveDataKey)
	{
		return PlayerPrefs.HasKey(saveDataKey);
	}
	
	//=================================================
	// Serialization methods
	//=================================================
	
	public void SerializeDataToFile(string saveDataKey)
	{
		StringBuilder builder = new StringBuilder();
		builder.Append("{ ");
		
		// Iterate across the dictionary to create a formatted string.
		foreach(KeyValuePair<string,string> pair in m_gameData)
		{
			string key = pair.Key;
			string data = pair.Value;
			
			string strRep = " { " + key + " : " + data + " } ";
			builder.Append(strRep);
		}
		
		builder.Append(" }");
		
		if(CompressData)
		{
			// Compress the string using zip.
			byte[] compressedData = ZipStr(builder.ToString());
						
			// Save the compressed string to the PlayerPrefs file.
			string compressedString = Convert.ToBase64String(compressedData);//GetString(compressedData);
			PlayerPrefs.SetString(saveDataKey, compressedString);
			PlayerPrefs.Save();
		}
		else
		{
			PlayerPrefs.SetString(saveDataKey, builder.ToString());
			PlayerPrefs.Save();
		}
	}
	
	public void DeserializeDataFromFile(string saveDataKey)
	{
		string uncompressedString = null;
		
		if(CompressData)
		{
			string compressedString = PlayerPrefs.GetString(saveDataKey);
			byte[] compressedData = Convert.FromBase64String(compressedString);//GetBytes(compressedString);
			uncompressedString = UnzipStr(compressedData);
		}
		else
		{		
			uncompressedString = PlayerPrefs.GetString(saveDataKey);
		}
		
		// Turn the formatted string into a dictionary.
		Regex saveEntryPattern = new Regex(@"\{\s*(\S+)\s*:\s*(\S+)\s*\}");
		Match m = saveEntryPattern.Match(uncompressedString);
		
		while(m.Success)
		{
			string key = m.Groups[1].ToString();
			string data = m.Groups[2].ToString();
						
			m_gameData[key] = data;
			m = m.NextMatch();
		}
	}
	
	public void ClearSerializedData()
	{
		m_gameData.Clear();
	}
	
	public void DeleteSerializedDataFromFile(string saveDataKey)
	{
		PlayerPrefs.DeleteKey(saveDataKey);
		PlayerPrefs.Save();
	}
	
	//=================================================
	// Saving methods
	//=================================================
	
	public void SaveInt(string name, int data)
	{
		m_gameData[name] = data.ToString();
	}
	
	public void SaveBool(string name, bool data)
	{
		m_gameData[name] = data.ToString();
	}
	
	public void SaveFloat(string name, float data)
	{
		m_gameData[name] = data.ToString();
	}
	
	public void SaveString(string name, string data)
	{
		m_gameData[name] = data;
	}
	
	//=================================================
	// Loading methods
	//=================================================
	
	public int LoadInt(string name)
	{
		return int.Parse(m_gameData[name]);
	}
	
	public bool LoadBool(string name)
	{
		return bool.Parse(m_gameData[name]);
	}
	
	public float LoadFloat(string name)
	{
		return float.Parse(m_gameData[name]);
	}
	
	public string LoadString(string name)
	{
		return m_gameData[name];
	}
	
	//=================================================
	// Compression methods
	//=================================================
	
	public byte[] ZipStr(string str)
	{
    	using(MemoryStream output = new MemoryStream())
		{
			using(DeflaterOutputStream deflater = new DeflaterOutputStream(output))
			{
				using(StreamWriter writer = new StreamWriter(deflater, System.Text.Encoding.Default))
				{
					writer.Write(str);
				}
			}
			
			return output.ToArray();
		}
	}
	
	public string UnzipStr(byte[] input)
	{
	    using (MemoryStream inputStream = new MemoryStream(input))
	    {
	        using (InflaterInputStream inflater = new InflaterInputStream(inputStream))
	        {
	            using (StreamReader reader = new StreamReader(inflater, System.Text.Encoding.Default))
	            {
	                return reader.ReadToEnd();
	            }
	        }
	    }
	}
	
//	private byte[] GetBytes(string str)
//	{
//	    byte[] bytes = new byte[str.Length * sizeof(char)];
//	    System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
//	    return bytes;
//	}
//	
//	private string GetString(byte[] bytes)
//	{
//	    int arraySize = Mathf.CeilToInt((float)bytes.Length / (float)sizeof(char));
//		char[] chars = new char[arraySize];
//	    System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
//	    return new string(chars);
//	}
}
