using UnityEngine;
using System.Collections;

public class BtnJob : MonoBehaviour
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

		//メッセージボックスを表示する
		TextWindows.SetActive( true );
	}
}
