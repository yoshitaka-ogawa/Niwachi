using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadScript : MonoBehaviour {

	// Use this for initialization
	void Start()
	{
		string evo = GameDataScript.GetEvolution();

		Debug.Log( evo );

		//現在が卵
		if( DefinedScript.EVOLUTION_EGG == evo )
		{
			SceneManager.LoadScene("Egg");
		}
		//ひよこ、ひな鳥、にわとりなMainへ
		else if( DefinedScript.EVOLUTION_HIYOKO == evo || DefinedScript.EVOLUTION_HINADORI == evo || DefinedScript.EVOLUTION_NIWATORI == evo )
		{
			SceneManager.LoadScene("Main");
		}
		//死体ならDeadへ
		else
		{
			SceneManager.LoadScene("Dead");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
