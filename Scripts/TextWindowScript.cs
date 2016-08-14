using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class TextWindowScript : MonoBehaviour
{
	public Text Msg = null;	//メッセージ
	public GameObject Feed = null;	//餌
	public NiwatoriScript niwatori = null;
	public Button BtnOk = null;
	public Button BtnNo = null;
	public Text CoinNum = null;

	private DefinedScript.E_MSG_TYPE Type = DefinedScript.E_MSG_TYPE.EAT;

	//OKとNOボタンを表示するときに位置
	private Vector3 Btn2Ok = new Vector3( -143.0f, -87.0f, 0.0f );
	private Vector3 Btn2No = new Vector3( 143.0f, -87.0f, 0.0f );
	
	//OKボタンのみ表示するときの位置
	private Vector3 Btn1Ok = new Vector3( 0.0f, -87.0f, 0.0f );
	
	// Use this for initialization
	void Start()
	{
		CoinNum.text = GameDataScript.GetCoinNum().ToString();

		this.gameObject.SetActive( false );	//自分自身を閉じる
	}
	/*
	// Update is called once per frame
	void Update () {
	
	}
*/
	//---------------------------------------------------------
	//	メッセージボックスに情報設定
	public void SetData( DefinedScript.E_MSG_TYPE type, string msg )
	{
		Type = type;
		Msg.text = msg;

		//動画再生失敗
		if( Type == DefinedScript.E_MSG_TYPE.NO_JOB )
		{
			BtnNo.gameObject.SetActive( false );

			BtnOk.GetComponent<RectTransform>().localPosition = Btn1Ok;
		}
		//それ以外
		else
		{
			BtnNo.gameObject.SetActive( true );
			
			BtnOk.GetComponent<RectTransform>().localPosition = Btn2Ok;
			BtnNo.GetComponent<RectTransform>().localPosition = Btn2No;
		}
	}

	//---------------------------------------------------------
	//OKボタンが押された
	public void OnOk()
	{
		//Debug.Log( Type );

		//ごはん
		if( Type == DefinedScript.E_MSG_TYPE.EAT )
		{
			Feed.SetActive( true );
			GameDataScript.SetFeedEnabled( 1 );

			//コインを減らす
			GameDataScript.SetCoinNum( GameDataScript.GetCoinNum() - 1 );

			CoinNum.text = GameDataScript.GetCoinNum().ToString();
			this.gameObject.SetActive( false );	//自分自身を閉じる
		}
		//そうじ
		else if( Type == DefinedScript.E_MSG_TYPE.CLEANING )
		{
			niwatori.CleanUnko();	//うんこをすべて消す

			//コインを減らす
			GameDataScript.SetCoinNum( GameDataScript.GetCoinNum() - 1 );

			CoinNum.text = GameDataScript.GetCoinNum().ToString();
			this.gameObject.SetActive( false );	//自分自身を閉じる
		}
		//バイト
		else if( Type == DefinedScript.E_MSG_TYPE.JOB )
		{
			// 広告の準備完了を確認
			if( Advertisement.IsReady() )
			{
				// 広告表示＋最後まで見たら報酬付与場合
				Advertisement.Show(null, new ShowOptions
				{
					resultCallback = result =>
					{
						if( result == ShowResult.Finished )
						{
							//コイン付与
							GameDataScript.SetCoinNum( GameDataScript.GetCoinNum() + 1 );
							CoinNum.text = GameDataScript.GetCoinNum().ToString();
							this.gameObject.SetActive( false );	//自分自身を閉じる
						}
					}
				});
			}
			//広告が表示できないときはメッセージを表示する
			else
			{
				SetData( DefinedScript.E_MSG_TYPE.NO_JOB,  DefinedScript.MSG_NO_JOB );
			}
		}
		else
		{
			GameDataScript.SetCoinNum( GameDataScript.GetCoinNum() + 1 );
			CoinNum.text = GameDataScript.GetCoinNum().ToString();
			this.gameObject.SetActive( false );	//自分自身を閉じる
		}
	}

	//---------------------------------------------------------
	//	Noボタンが押された
	public void OnNo()
	{
		this.gameObject.SetActive( false );
	}
}
