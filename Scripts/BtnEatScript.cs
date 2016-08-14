using UnityEngine;
using System.Collections;

public class BtnEatScript : MonoBehaviour
{
	public GameObject TextWindows = null;	//テキストウィンドウ

	// Use this for initialization
	//void Start()
	//{
	//}
/*	
	// Update is called once per frame
	void Update () {
	
	}
	*/

	public void OnClick()
	{
		if( 0 < GameDataScript.GetCoinNum() )
		{
			TextWindows.GetComponent<TextWindowScript>().SetData( DefinedScript.E_MSG_TYPE.EAT, DefinedScript.MSG_EAT_START );
			TextWindows.SetActive( true );
		}
		else
		{
			//コインがなければ稼ぎなさい。
			TextWindows.GetComponent<TextWindowScript>().SetData( DefinedScript.E_MSG_TYPE.JOB, DefinedScript.MSG_NO_COIN );
			TextWindows.SetActive( true );	//メッセージボックスを表示する
		}
	}

}
