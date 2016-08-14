using UnityEngine;
using System.Collections;

//==========================================================
//	グローバル変数　この値をセーブする
public static class GameDataScript
{
	//-----------------------------------------------
	//	マルチプル画像を読み込む
	public static Sprite GetSprite(string fileName, string spriteName)
	{
		Sprite[] sprites = Resources.LoadAll<Sprite>(fileName);
		return System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(spriteName));
	}

	//-----------------------------------------------
	//	現在時刻との時間の差を秒で返す
	public static int GetSecNowTimeSpan( System.DateTime dt )
	{
		System.TimeSpan sn = System.DateTime.Now - dt;
		return (int)sn.TotalSeconds;
	}

	//-----------------------------------------------
	//	現在時刻との時間の差を秒で返す
	public static void InitData()
	{
		//これがUnity上では動くが端末だと動かないので、下記にて自分で初期化する
		//めんどいからやめたいけど
		//PlayerPrefs.DeleteAll();	//すべて初期化
		//PlayerPrefs.Save();		//保存すれば初期化できるか？ > だめだった

		PlayerPrefs.SetInt( KEY_TOUCH_TIME, 0 );			//撫でた時間（秒）
		PlayerPrefs.SetString( KEY_EVOLUTION, DefinedScript.EVOLUTION_EGG );	//進化
		PlayerPrefs.SetString( KEY_START_TIME, "" );		//卵からかえった日
		PlayerPrefs.SetString( KEY_LAST_EAT_TIME, "" );		//最後に食事した時間
		PlayerPrefs.SetInt( KEY_FEED_ENABLED, 0 );			//食事があるかないか
		PlayerPrefs.SetString( KEY_LAST_NADE_TIME, "" );	//最後になでた時間
		PlayerPrefs.SetString( KEY_LAST_UNKO_TIME, "" );	//最後にうんこした時間
		PlayerPrefs.SetString( KEY_UNKO_POS, "" );			//うんこした場所
		PlayerPrefs.SetString( KEY_DEAD_TIME, "" );			//死んだ日時
		PlayerPrefs.SetString( KEY_DEAD_MSG, "" );			//死因
	}

	//-----------------------------------------------
	//	文字列を日付に変換する
	private static System.DateTime ConvertStringToDateTime( string key )
	{
		string str = PlayerPrefs.GetString( key, "" );

		System.DateTime dt;

		if( System.DateTime.TryParse( str, out dt ) )
		{
			return dt;
		}
		else
		{
			//日時変換できなければ今にする
			System.DateTime now = System.DateTime.Now;
			PlayerPrefs.SetString( key, now.ToString() );
			return now;
		}
	}

	//-----------------------------------------------
	//撫でた時間(秒)
	private const string KEY_TOUCH_TIME = "TouchTime"; 
	public static void SetTouchTime( int n )
	{
		PlayerPrefs.SetInt( KEY_TOUCH_TIME, n );
	}
	public static int GetTouchTime()
	{
		return PlayerPrefs.GetInt( KEY_TOUCH_TIME, 0 );
	}

	//-----------------------------------------------
	//	進化状態
	private const string KEY_EVOLUTION = "Evolution";
	public static void  SetEvolution( string s )
	{
		//Debug.Log( "set " + s );
		PlayerPrefs.SetString( KEY_EVOLUTION, s );
	}
	public static string GetEvolution()
	{
		return PlayerPrefs.GetString( KEY_EVOLUTION, DefinedScript.EVOLUTION_EGG );
	}

	//-----------------------------------------------
	//	卵から孵った日
	private const string KEY_START_TIME = "StartTime";
	public static void SetStartTime( System.DateTime dt )
	{
		PlayerPrefs.SetString( KEY_START_TIME, dt.ToString() );
	}
	public static System.DateTime GetStartTime()
	{
		return ConvertStringToDateTime( KEY_START_TIME );
	}

	//-----------------------------------------------
	//	最後に食事を食べた時間
	private const string KEY_LAST_EAT_TIME = "LastEatTime";
	public static void SetLastEatTime( System.DateTime dt )
	{
		PlayerPrefs.SetString( KEY_LAST_EAT_TIME, dt.ToString() );

		SetFeedEnabled( 0 );	//食事したので食事は消える
	}
	public static System.DateTime GetLastEatTime()
	{
		return ConvertStringToDateTime( KEY_LAST_EAT_TIME );
	}

	//-----------------------------------------------
	//	食事がおいてあるかのフラグ（0=ない、1=ある）
	private const string KEY_FEED_ENABLED = "FeedEnabled";
	public static void SetFeedEnabled( int n )
	{
		PlayerPrefs.SetInt( KEY_FEED_ENABLED, n );
	}
	public static int GetFeedEnabled()
	{
		return PlayerPrefs.GetInt( KEY_FEED_ENABLED, 0 );
	}

	//-----------------------------------------------
	//	最後になでた時間
	private const string KEY_LAST_NADE_TIME = "LastNadeTime";
	public static void SetLastNadeTime( System.DateTime dt )
	{
		PlayerPrefs.SetString( KEY_LAST_NADE_TIME, dt.ToString() );
	}
	public static System.DateTime GetLastNadeTime()
	{
		return ConvertStringToDateTime( KEY_LAST_NADE_TIME );
	}

	//-----------------------------------------------
	//	最後にうんこした時間
	private const string KEY_LAST_UNKO_TIME = "LastUnkoTime";
	public static void SetLastUnkoTime( System.DateTime dt )
	{
		PlayerPrefs.SetString( KEY_LAST_UNKO_TIME, dt.ToString() );
	}
	public static System.DateTime GetLastUnkoTime()
	{
		return ConvertStringToDateTime( KEY_LAST_UNKO_TIME );
	}
	
	//-----------------------------------------------
	//	うんこした場所を記憶
	private const string KEY_UNKO_POS = "UnkoPos";
	public static void SetUnkoPos( ArrayList ary )
	{
		string data = "";
		foreach( GameObject go in ary )
		{
			data = data + go.transform.position.x.ToString() + "," + go.transform.position.y.ToString() + ",";
		}
		PlayerPrefs.SetString( KEY_UNKO_POS, data );
	}
	public static string GetUnkoPos()
	{
		return PlayerPrefs.GetString( KEY_UNKO_POS, "" );
	}

	//-----------------------------------------------
	//	死んだ日時
	private const string KEY_DEAD_TIME = "DeadTime";
	public static void SetDeadTime( System.DateTime dt )
	{
		PlayerPrefs.SetString( KEY_DEAD_TIME, dt.ToString() );
	}
	public static System.DateTime GetDeadTime()
	{
		return ConvertStringToDateTime( KEY_DEAD_TIME );
	}

	//-----------------------------------------------
	//	死因を記憶
	private const string KEY_DEAD_MSG = "DeadMsg";
	public static void SetDeadMsg( string s )
	{
		PlayerPrefs.SetString( KEY_DEAD_MSG, s );
	}
	public static string GetDeadMsg()
	{
		return PlayerPrefs.GetString( KEY_DEAD_MSG, "" );
	}

	
	//-----------------------------------------------
	//	コイン所持枚数（このデータは初期化しません）
	private const string KEY_COIN_NUM = "CoinNum";
	public static void SetCoinNum( int n )
	{
		PlayerPrefs.SetInt( KEY_COIN_NUM, n );
	}
	public static int GetCoinNum()
	{
		return PlayerPrefs.GetInt( KEY_COIN_NUM, 0 );
	}

	//-----------------------------------------------

}
