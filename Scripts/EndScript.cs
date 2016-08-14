using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndScript : MonoBehaviour
{
/*
	// Use this for initialization
	void Start () {
	
	}
	*/
	// Update is called once per frame
	void Update()
	{
		Vector3 scale = this.transform.localScale;
		if( 1.0f < scale.x )
		{
			scale.x = scale.x - Time.deltaTime * 2.0f;
			scale.y = scale.y - Time.deltaTime * 2.0f;
			scale.z = scale.z - Time.deltaTime * 2.0f;
			this.transform.localScale = scale;
		}
		
	}

	//-----------------------------------------------------
	//	指が画面から離れたイベント
	void OnMouseUp()
	{
		//GameDataScript.InitData();
		SceneManager.LoadScene("Egg");
	}
}
