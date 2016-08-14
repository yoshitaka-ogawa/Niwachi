using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

//======================================================
//  にわとり管理くらす
//======================================================
public class NiwatoriScript : MonoBehaviour
{
    private Animator animator = null;		//アニメーション管理
	
	private float ChangeActionCnt = 0.0f;	//行動変更時間カウント用
	private Vector3 MovePoint = new Vector3( 0.0f, 0.0f, 0.0f );	//移動量
	private DefinedScript.E_STATUS Status = DefinedScript.E_STATUS.NORMAL;	//にわとりの精神状態
	
	public Text txtDay = null;		//飼育期間

	//撫でられ
	private bool NadeNadeFlg = false;		//撫でられている間true
	private float NadeSec = 0.0f;			//なでられた時間を計測

	//うんこ関連
	public GameObject UnkoPrefab = null;	//うんこプレハブ
	private ArrayList UnkoList = new ArrayList();	//うんこのプレハブリスト
	private DefinedScript.E_UNKO_FLOW UnkoFlow = DefinedScript.E_UNKO_FLOW.ANIMATION;	//うんこ処理流れ

	//食事関連
	public GameObject Feed = null;	//ごはんオブジェクト
	private DefinedScript.E_EAT_FLOW EatFlow = DefinedScript.E_EAT_FLOW.MOVE;	//食事処理流れ
	private Vector3 FeedPos = new Vector3( 0.0f, 0.0f, 0.0f );


	//-----------------------------------------------------
    // Use this for initialization
    void Start()
    {
		animator = this.GetComponent<Animator>();

		CheckEvolution();	//進化判定

		InitUnko();	//うんこ初期化

		InitEat();	//食事初期化

		//最初は強制で基本の動きにする
		animator.Play( GameDataScript.GetEvolution() + "_" + DefinedScript.ACTION_EAT );

		ChangeStatus();	//Status変更

		FeedPos = Feed.transform.position;
		FeedPos.x += 0.02f;
	}
	
	//-----------------------------------------------------
	// Update is called once per frame
	void Update()
    {
		//撫でられているときはアニメーション強制
		if( NadeNadeFlg )
		{
			NadeSec += Time.deltaTime;

			//怒っているとき
			if( Status == DefinedScript.E_STATUS.ANGRY )
			{
				//一定時間以上なでられたら、通常に戻る
				if( DefinedScript.NADE_STATUS_STOP_ANGRY_SEC <= NadeSec )
				{
					Status = DefinedScript.E_STATUS.NORMAL;

					//Statusかわったので記録して初期化
					GameDataScript.SetTouchTime( GameDataScript.GetTouchTime() + (int)NadeSec );
					NadeSec = 0.0f;

					ChangeAction( DefinedScript.ACTION_NADENADE );
				}
			}
			//通常のとき
			else if( Status == DefinedScript.E_STATUS.NORMAL )
			{
				//一定時間以上なでられたら、HAPPYになる
				if( DefinedScript.NADE_STATUS_HAPPY_SEC <= NadeSec )
				{
					Status = DefinedScript.E_STATUS.HAPPY;
				}
			}
		}
		else
		{
			/*
			//TODO:: 指がタップされたところに向かうのはあとで
			if( Input.GetMouseButtonDown(0) )
			{
				Debug.Log("nadenade");
			}
			*/
			//	一定ごとに動きを変える
			if( DefinedScript.SEC_CHANGE_ACTION <= (int)ChangeActionCnt )
			{
				CheckEvolution();	//進化判定

				CheckDeadHungry();	//餓死判定

				ChangeStatus();		//Status変更

				ChangeActionCnt = 0.0f;

				//一定時間経過したらうんこしたい
				if( DefinedScript.UNKO_INTERVALS_SEC <= GameDataScript.GetSecNowTimeSpan( GameDataScript.GetLastUnkoTime() ) )
				{
					//Debug.Log("Unko");
					UpdateUnko();
				}
				//一定時間経過＆食事があれば食事したい
				else if( Feed.gameObject.activeSelf && DefinedScript.EAT_HUNGRY_SEC < GameDataScript.GetSecNowTimeSpan( GameDataScript.GetLastEatTime() ) )
				{
					//Debug.Log( "Eat:" + EatFlow.ToString() );
					UpdateEat();
				}
				//その他
				else
				{
					//Debug.Log("Sonota");

					string action = DefinedScript.ACTION_EAT;

					//0=何もしない、1=歩く
					int actionRand = UnityEngine.Random.Range( 0, 2 );

					if( Status == DefinedScript.E_STATUS.HUNGRY )
					{
						if( actionRand == 0 )
						{
							action = DefinedScript.ACTION_BAD;
						}
						else
						{
							action = DefinedScript.ACTION_BAD_WALK;
						}
					}
					else if( Status == DefinedScript.E_STATUS.ANGRY )
					{
						if( actionRand == 0 )
						{
							action = DefinedScript.ACTION_BAD;
						}
						else
						{
							action = DefinedScript.ACTION_BAD_WALK;
						}
					}
					else if( Status == DefinedScript.E_STATUS.HAPPY )
					{
						if( actionRand == 0 )
						{
							action = DefinedScript.ACTION_EAT;
						}
						else
						{
							action = DefinedScript.ACTION_SKIP;
						}
					}
					else
					{
						if( actionRand == 0 )
						{
							action = DefinedScript.ACTION_EAT;
						}
						else
						{
							action = DefinedScript.ACTION_WALK;
						}
					}
					ChangeAction( action );
				}
				
				//飼育日を更新
				System.TimeSpan sn = System.DateTime.Now - GameDataScript.GetStartTime();
				txtDay.text = sn.Days.ToString();
			}
			else
			{
				this.transform.position += MovePoint * Time.deltaTime;

				ChangeActionCnt += Time.deltaTime;
			}
		}
	}

	//-----------------------------------------------------
	//	動き切り替え
	private void ChangeAction( string action )
	{
		//Debug.Log( GameDataScript.GetEvolution() );
		//Debug.Log( action );
		animator.Play( GameDataScript.GetEvolution() + "_" + action );

		//歩くアニメーションなので移動方向と速度を決める
		if( action == DefinedScript.ACTION_BAD_WALK || 
			action == DefinedScript.ACTION_WALK ||
			action == DefinedScript.ACTION_SKIP )
		{
			MovePoint = new Vector3( UnityEngine.Random.Range( -1.0f, 1.0f ), UnityEngine.Random.Range( -1.0f, 1.0f ), 0.0f );
			
			//右移動なら反転する
			if( 0 < MovePoint.x )
			{
				this.transform.localScale = new Vector3( -1.0f, 1.0f, 1.0f );
			}
			else
			{
				this.transform.localScale = new Vector3( 1.0f, 1.0f, 1.0f );
			}
		}
		else
		{
			MovePoint = new Vector3( 0.0f, 0.0f, 0.0f );
		}
	}
	
	//-----------------------------------------------------
	//	指が画面に触れたイベント
	void OnMouseDown()
	{
		NadeNadeFlg = true;

		MovePoint = new Vector3( 0.0f, 0.0f, 0.0f );	//動かない
		ChangeActionCnt = DefinedScript.SEC_CHANGE_ACTION - 1;		//撫で終わったら次にいけるように

		NadeSec = 0.0f;	//撫で時間を初期化

		if( Status == DefinedScript.E_STATUS.HUNGRY )
		{
			ChangeAction( DefinedScript.ACTION_BAD );
		}
		else if( Status == DefinedScript.E_STATUS.ANGRY )
		{
			ChangeAction( DefinedScript.ACTION_ANGRY );
		}
		else if( Status == DefinedScript.E_STATUS.HAPPY )
		{
			ChangeAction( DefinedScript.ACTION_NADENADE );
		}
		else
		{
			ChangeAction( DefinedScript.ACTION_NADENADE );
		}
	}

	//-----------------------------------------------------
	//	指が画面から離れたイベント
	void OnMouseUp()
	{
		//触れていた時間を秒で取得する（ミリ秒は切り捨て）
		GameDataScript.SetTouchTime( GameDataScript.GetTouchTime() + (int)NadeSec );

		//なでた時間を記憶
		GameDataScript.SetLastNadeTime( System.DateTime.Now );

		NadeSec = 0.0f;
		NadeNadeFlg = false;	//なでるの終了
	}

	//-----------------------------------------------------
	//	死んだときの処理
	private void Dead( string msg )
	{
		GameDataScript.SetDeadMsg( msg );	//死亡理由を記憶

		GameDataScript.SetDeadTime( System.DateTime.Now );	//死んだ時間を記録
							
		//シーンを移動
		SceneManager.LoadScene("Dead");
	}
	
	//-----------------------------------------------------
	//	進化判定
	private void CheckEvolution()
	{
		int sec = GameDataScript.GetSecNowTimeSpan( GameDataScript.GetStartTime() );	//生まれてから何時間たったかを取得

		string evo = GameDataScript.GetEvolution();

		//にわとり
		if( evo == DefinedScript.EVOLUTION_NIWATORI )
		{
			//撫でた秒数に応じて寿命が延びる
			int nade = GameDataScript.GetTouchTime();
			int add = nade / DefinedScript.NADE_ADD_LIFE_NADE_SEC * DefinedScript.NADE_ADD_LIFE_ADD_SEC;

			//次の進化までの時間
			int nowLife = DefinedScript.EVOLUTION_HINADORY_SEC + DefinedScript.EVOLUTION_NIWATORI_SEC + DefinedScript.EVOLUTION_DEAD_SEC + add;

			//寿命で死亡
			if( nowLife <= sec )
			{
				this.Dead( DefinedScript.MSG_DEAD_LIFE );
			}
		}
		//ひなどり
		else if( evo == DefinedScript.EVOLUTION_HINADORI )
		{
			//次の進化までの時間
			int nowLife = DefinedScript.EVOLUTION_HINADORY_SEC + DefinedScript.EVOLUTION_NIWATORI_SEC;
			
			//にわとりに進化
			if( nowLife <= sec )
			{
				GameDataScript.SetEvolution( DefinedScript.EVOLUTION_NIWATORI );
			}
		}
		else if( evo == DefinedScript.EVOLUTION_HIYOKO )
		{
			//次の進化までの時間
			int nowLife = DefinedScript.EVOLUTION_HINADORY_SEC;
			
			//ひな鳥に進化
			if( nowLife <= sec )
			{
				GameDataScript.SetEvolution( DefinedScript.EVOLUTION_HINADORI );
			}
		}
	}

	//-----------------------------------------------------
	//	餓死チェック
	private void CheckDeadHungry()
	{
		if( DefinedScript.EAT_DEAD_HUNGRY_SEC <= GameDataScript.GetSecNowTimeSpan( GameDataScript.GetLastEatTime() ) )
		{
			Dead( DefinedScript.MSG_DEAD_EAT );
		}
	}

	//-----------------------------------------------------
	//	Updateでおこなうんこ処理
	private void UpdateUnko()
	{
		//うんこが最大数を超えていれば死亡
		if( DefinedScript.UNKO_DEAD_NUM <= UnkoList.Count )
		{
			this.Dead( DefinedScript.MSG_DEAD_UNKO );
		}
		else
		{
			//ふんばるアニメーション
			if( UnkoFlow == DefinedScript.E_UNKO_FLOW.ANIMATION )
			{
				ChangeAction( DefinedScript.ACTION_SWAGGER );
				UnkoFlow = DefinedScript.E_UNKO_FLOW.OUTPUT;
			}
			//うんこを増やす
			else if( UnkoFlow == DefinedScript.E_UNKO_FLOW.OUTPUT )
			{
				Vector3 pos = this.transform.position;
				pos.y -= 1.1f;
				pos.z = 1.0f;
				GameObject unko = (GameObject)Instantiate( UnkoPrefab, pos, this.transform.rotation );
				UnkoList.Add( unko );

				GameDataScript.SetUnkoPos( UnkoList );

				UnkoFlow = DefinedScript.E_UNKO_FLOW.ANIMATION;		//処理フローを初期化
				GameDataScript.SetLastUnkoTime( System.DateTime.Now );	//最後にうんこした時間を更新する

				ChangeActionCnt = (float)DefinedScript.SEC_CHANGE_ACTION;	//すぐ切り替えられるようにする
			}
		}
	}

	//-----------------------------------------------------
	//	うんこ初期化
	private void InitUnko()
	{
		//カンマ区切りのデータからうんこ座標を取り出しうんこを再配置
		string data = GameDataScript.GetUnkoPos();
		string[] list = data.Split( ',' );
		for( int i=0 ; i<list.Length-1 ; i=i+2 )
		{
			//Debug.Log(list[i] + "," + list[i+1]);
			Vector3 pos = new Vector3( float.Parse(list[i]), float.Parse(list[i+1]), 1.0f );
			GameObject unko = (GameObject)Instantiate( UnkoPrefab, pos, this.transform.rotation  );
			UnkoList.Add( unko );
		}

		//最後のうんこから何秒たっているかでうんこを生成する
		int sec = GameDataScript.GetSecNowTimeSpan( GameDataScript.GetLastUnkoTime() );
		if( 0 < sec )
		{
			int num = sec / DefinedScript.UNKO_INTERVALS_SEC;

			//生成するうんこあり
			if( 0 < num )
			{
				//うんこが最大数を超えていれば死亡
				if( DefinedScript.UNKO_DEAD_NUM <= UnkoList.Count + num )
				{
					this.Dead( DefinedScript.MSG_DEAD_UNKO );
				}
				//最大数を超えていなけれbあ生成
				else
				{
					for( int i=0 ; i<num ; i++ )
					{
						//うんこをランダムな場所に生成する
						Vector3 pos = new Vector3( UnityEngine.Random.Range( -3.0f, 3.0f ), UnityEngine.Random.Range( -2.7f, 2.7f ), 1.0f );
						GameObject unko = (GameObject)Instantiate( UnkoPrefab, pos, this.transform.rotation  );
						UnkoList.Add( unko );
					}

					GameDataScript.SetLastUnkoTime( System.DateTime.Now );  //最後にうんこした時間を更新する
					GameDataScript.SetUnkoPos( UnkoList );  //テーブルを更新
				}
			}
		}
	}
	
	//-----------------------------------------------------
	//	うんこをきれいにする
	public void CleanUnko()
	{
		foreach( GameObject go in UnkoList )
		{
			Destroy( go );
		}
		UnkoList.Clear();

		GameDataScript.SetUnkoPos( UnkoList );
	}

	//-----------------------------------------------------
	//	食事処理
	private void UpdateEat()
	{
		//食事に近づく
		if( EatFlow == DefinedScript.E_EAT_FLOW.MOVE )
		{
			animator.Play( GameDataScript.GetEvolution() + "_" + DefinedScript.ACTION_SKIP );

			MovePoint = Vector3.Lerp( FeedPos, this.transform.position, 1.0f / (float)DefinedScript.SEC_CHANGE_ACTION );
			MovePoint.z = 0.0f;

			//右移動なら反転する
			if( 0 < MovePoint.x )
			{
				this.transform.localScale = new Vector3( -1.0f, 1.0f, 1.0f );
			}
			else
			{
				this.transform.localScale = new Vector3( 1.0f, 1.0f, 1.0f );
			}
		}
		//食事を食べるアニメーション
		else if( EatFlow == DefinedScript.E_EAT_FLOW.ANIMATION )
		{
			ChangeAction( DefinedScript.ACTION_EAT );	//食事Action
			EatFlow = DefinedScript.E_EAT_FLOW.END;
		}
		//食事終了
		else if( EatFlow == DefinedScript.E_EAT_FLOW.END )
		{
			GameDataScript.SetLastEatTime( System.DateTime.Now );	//食事時間を記録
			GameDataScript.SetFeedEnabled( 0 );
			Feed.SetActive( false );
			EatFlow = DefinedScript.E_EAT_FLOW.MOVE;

			//お腹空いた状態なら通常に戻す
			if( Status == DefinedScript.E_STATUS.HUNGRY )
			{
				Status = DefinedScript.E_STATUS.NORMAL;
			}
		}
	}

	//-------------------------------------------------------
	//	他のオブジェクトと衝突したイベント
	void OnCollisionEnter2D( Collision2D other )
	{
		//ぶつかったのが食事なら食事開始
		if( other.gameObject.tag == "Eat" )
		{
			//食事にむけて移動中だった
			if( EatFlow == DefinedScript.E_EAT_FLOW.MOVE )
			{
				EatFlow = DefinedScript.E_EAT_FLOW.ANIMATION;	//食べるアニメーションに切り替える
				ChangeActionCnt = (float)DefinedScript.SEC_CHANGE_ACTION;	//すぐ切り替えられるようにする
			}
		}
	}

	//-----------------------------------------------------
	//	食事初期化
	public void InitEat()
	{
		//食事が置いてあれば現在時刻を食事した時間にしちゃう
		if( GameDataScript.GetFeedEnabled() == 1 )
		{
			//お腹が空いてる
			if( DefinedScript.EAT_HUNGRY_SEC < GameDataScript.GetSecNowTimeSpan( GameDataScript.GetLastEatTime() ) )
			{
				GameDataScript.SetFeedEnabled( 0 );
				GameDataScript.SetLastEatTime( System.DateTime.Now );
				Feed.SetActive( false );

				//お腹空いた状態なら通常に戻す
				if( Status == DefinedScript.E_STATUS.HUNGRY )
				{
					Status = DefinedScript.E_STATUS.NORMAL;
				}
			}
			else
			{
				Feed.SetActive( true );
			}
		}
		else
		{
			Feed.SetActive( false );

			CheckDeadHungry();	//餓死判定
		}
	}

	//-----------------------------------------------------
	//	status判定
	private void ChangeStatus()
	{
		//お腹空いた
		if( DefinedScript.EAT_STATUS_HUNGRY_SEC <= GameDataScript.GetSecNowTimeSpan( GameDataScript.GetLastEatTime() ) )
		{
			Status = DefinedScript.E_STATUS.HUNGRY;
		}
		//なでてほしい
		else if( DefinedScript.NADE_STATUS_ANGRY_SEC <= GameDataScript.GetSecNowTimeSpan( GameDataScript.GetLastNadeTime() ) )
		{
			Status = DefinedScript.E_STATUS.ANGRY;
		}
	}

	//-----------------------------------------------------
}
