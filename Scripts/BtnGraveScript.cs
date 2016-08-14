using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BtnGraveScript : MonoBehaviour
{
	public GameObject Grave = null;			//お墓画像
	public GameObject Die = null;			//死体画像
	public GameObject DeadAnim = null;		//魂画像
	//public GameObject Counter = null;		//飼育期間
	public Text msg = null;					//メッセージ
	public GameObject Logo = null;			//ロゴ

	// Use this for initialization
	void Start()
	{
		if( DefinedScript.EVOLUTION_BONE_SEC <= GameDataScript.GetSecNowTimeSpan( GameDataScript.GetDeadTime() ) )
		{
			msg.text = DefinedScript.MSG_DEAD_BONE;
			Die.GetComponent<SpriteRenderer>().sprite = GameDataScript.GetSprite( "Graphics/ui_sprite", "dead" );
			DeadAnim.SetActive( false );

			GameDataScript.SetEvolution( DefinedScript.EVOLUTION_BONE );
		}
		else
		{
			msg.text = GameDataScript.GetDeadMsg();

			if( GameDataScript.GetEvolution() == DefinedScript.EVOLUTION_HIYOKO )
			{
				Die.GetComponent<SpriteRenderer>().sprite = GameDataScript.GetSprite( "Graphics/ui_sprite", "hiyoko_die" );
			}
			else if( GameDataScript.GetEvolution() == DefinedScript.EVOLUTION_HINADORI )
			{
				Die.GetComponent<SpriteRenderer>().sprite = GameDataScript.GetSprite( "Graphics/ui_sprite", "hiyodori_die" );
			}
			else if( GameDataScript.GetEvolution() == DefinedScript.EVOLUTION_NIWATORI )
			{
				Die.GetComponent<SpriteRenderer>().sprite = GameDataScript.GetSprite( "Graphics/ui_sprite", "niwatori_die" );
			}
			GameDataScript.SetEvolution( DefinedScript.EVOLUTION_DEAD );
		}
	}

	/*
	// Update is called once per frame
	void Update()
	{
	}
	*/
	public void OnClick()
	{
		DeadAnim.SetActive( false );
		Grave.SetActive( true );
		Die.SetActive( false );
		//Counter.SetActive( false );
		Logo.SetActive( true );
		this.gameObject.SetActive( false );

		msg.text = DefinedScript.MSG_END;

		//PlayerPrefs.DeleteAll();	//すべて初期化
		GameDataScript.InitData();
	}
}
