using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic; //Needed for Lists
using System.Xml; //Needed for XML functionality
using System.Xml.Serialization; //Needed for XML Functionality
using System.IO;
using System.Xml.Linq; //Needed for XDocument
using UnityEngine.UI;

public class SessionXMLScript : MonoBehaviour
{
	float xmlFileLength;
	XmlDocument xmlDoc, SessionDataXml;
	Color newCol;
	XmlElement elmRootChat;
	string xmlFileText;
	TextAsset xmlAsset;
    public InputField _emailId, _pwd;

    // Start is called before the first frame update
    void Start()
    {
        xmlDoc = new XmlDocument ();
        LoginScript _loginScript = FindObjectOfType<LoginScript>();
        _loginScript._completedata.Clear();
		// Debug.LogError("path  " + Application.persistentDataPath);
		if (File.Exists (Application.persistentDataPath + "/SessionDataFile.xml")) 
		{
			SessionDataXml = new XmlDocument();
			
			SessionDataXml.Load(Application.persistentDataPath + "/SessionDataFile.xml");

			for (int i = 0; i < SessionDataXml.FirstChild.ChildNodes.Count; i++) 
			{
				string[] _parseddata = SessionDataXml.FirstChild.ChildNodes[i].InnerText.Split(char.Parse(","));
                _loginScript._emailId.Add(_parseddata[0]);
                _loginScript._pwd.Add(_parseddata[1]);
			}
            for(int j = 0; j < _loginScript._emailId.Count; j++)
            {
                _loginScript._completedata.Add(_loginScript._emailId[j], _loginScript._pwd[j]);
            }
		}
    }

	/*public void DisplaySessionData()
	{
		Controller _controlScript = FindObjectOfType<Controller>();
		loginscript _loginScript = FindObjectOfType<loginscript>();
		if (File.Exists (Application.persistentDataPath + "/SessionDataFile.xml")) 
		{
			SessionDataXml = new XmlDocument();
			
			SessionDataXml.Load(Application.persistentDataPath + "/SessionDataFile.xml");

			for (int i = 0; i < SessionDataXml.FirstChild.ChildNodes.Count; i++) 
			{
				string[] _parseddata = SessionDataXml.FirstChild.ChildNodes[i].InnerText.Split(char.Parse(","));
				if(_parseddata[0] == PlayerPrefs.GetString("Playername"))
				{
					_loginScript.NameList.Add(_parseddata[0]);
					_loginScript.TimeList.Add(_parseddata[1]);
					_loginScript.DateList.Add(_parseddata[2]);
				}
			}

			for(int j = _loginScript.NameList.Count - 1; j > 0; j--)
			{
				GameObject _sessionObj = Instantiate(_loginScript.SessionGameObject);
				_sessionObj.transform.parent = _loginScript.SessionParentObj.transform;

				_sessionObj.transform.GetChild(0).gameObject.GetComponent<Text>().text = _loginScript.NameList[j];
				_sessionObj.transform.GetChild(1).gameObject.GetComponent<Text>().text = _loginScript.TimeList[j];
				_sessionObj.transform.GetChild(2).gameObject.GetComponent<Text>().text =_loginScript.DateList[j];
			}
		}
	}*/

    public void CreateXMLFile_data()
	{

		if (File.Exists (Application.persistentDataPath + "/SessionDataFile.xml")) 
		{
			SessionDataXml = new XmlDocument();
			// Debug.Log("Data... ");
			SessionDataXml.Load(Application.persistentDataPath + "/SessionDataFile.xml");
		} 
		else
		{
			xmlAsset = Resources.Load ("SessionDataFile") as TextAsset;
			File.WriteAllText(Application.persistentDataPath + "/SessionDataFile.xml",xmlAsset.text);
			SessionDataXml = new XmlDocument();
			SessionDataXml.Load(Application.persistentDataPath + "/SessionDataFile.xml");
		}

		string filepath = Application.persistentDataPath +"/SessionDataFile.xml";
		FileStream stream;// = new FileStream ();
		if (File.Exists (filepath))
		{
			stream = new FileStream (Application.persistentDataPath + "/SessionDataFile.xml", FileMode.Open);
		}
		else
		{
			stream = new FileStream (Application.persistentDataPath + "/SessionDataFile.xml", FileMode.Create);
		}

		xmlFileLength = stream.Length;
		stream.Close ();
		SaveDataToXML ();
	}
	

	public void SaveDataToXML()
	{
		string filepath = Application.persistentDataPath +"/SessionDataFile.xml";
		// Debug.Log ("filepath...." + filepath);

		if (xmlFileLength == 0.00) 
		{
			xmlDoc.LoadXml ("<SessionData></SessionData>");
			elmRootChat = xmlDoc.DocumentElement;
			EnterDataToXml (elmRootChat);
		}
		else
		{
			xmlDoc.Load (filepath);
			elmRootChat = xmlDoc.DocumentElement;
			EnterDataToXml (elmRootChat);
		}
		xmlDoc.Save(filepath);																		// save file
	}

    public void EnterDataToXml(XmlElement rm,string msg="")
	{
        LoginScript _loginScript = FindObjectOfType<LoginScript>();
        XmlNodeList roomList = xmlDoc.GetElementsByTagName ("SessionData");
		// Debug.Log("roomlist... "  + roomList.Count);

        foreach (XmlNode messageInfo in roomList) 
        {
			// Debug.Log("add in the list  " + _loginscript.NameList.Count );
            XmlNodeList messagecontent = messageInfo.ChildNodes;

				// Debug.Log("i   " + i);
            XmlElement messages = xmlDoc.CreateElement ("SessionInfo");
            messages.InnerText = _emailId.text + "," + _pwd.text ;
            _loginScript._emailId.Add(_emailId.text);
            _loginScript._pwd.Add(_pwd.text);
            _loginScript._completedata.Add(_emailId.text, _pwd.text);
            // messages.InnerText = _loginscript.NameList[i] + "," + _loginscript.TimeList[i] + "," + _loginscript.DateList[i];
            rm.AppendChild (messages);
            // Debug.LogError( "  msg  " + messages.InnerText);
        }
	}

}