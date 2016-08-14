using UnityEngine;
using System.Collections;

public class BtnJobScript : MonoBehaviour
{
	public GameObject TextWindows = null;	//テキストウィンドウ
/*
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
*/
	public void OnClick()
	{
		TextWindows.GetComponent<TextWindowScript>().SetData( DefinedScript.E_MSG_TYPE.JOB, DefinedScript.MSG_JOB_START );
		TextWindows.SetActive( true );
	}
}
