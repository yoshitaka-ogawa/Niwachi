using UnityEngine;
using System.Collections;

public static class DefinedScript
{
	//進化状態
	public const string EVOLUTION_EGG = "egg";
	public const string EVOLUTION_HIYOKO = "hiyoko";
	public const string EVOLUTION_HINADORI = "hinadori";
	public const string EVOLUTION_NIWATORI = "niwatori";
	public const string EVOLUTION_DEAD = "dead";
	public const string EVOLUTION_BONE = "bone";

	//行動
	public const string ACTION_EAT = "eat";				//通常＆食事
	public const string ACTION_BAD = "bad";				//不機嫌
	public const string ACTION_SWAGGER = "swagger";		//うんち
	public const string ACTION_WALK = "walk";			//通常歩き
	public const string ACTION_SKIP = "skip";			//喜び歩き
	public const string ACTION_BAD_WALK = "bad_walk";	//不機嫌歩き
	public const string ACTION_NADENADE = "nadenade";	//撫でられてる
	public const string ACTION_ANGRY = "angry";			//撫でられ怒り

	//メッセージ一覧
	public const string MSG_EGG_START = "ゆびで あたためて";
	public const string MSG_EGG_END = "おや・・・？";
	public const string MSG_DEAD_LIFE = "うまれてきて しあわせ でした";
	public const string MSG_DEAD_UNKO = "いきるのが くるしかった";
	public const string MSG_DEAD_EAT = "ごはん もっと たべたかった";
	public const string MSG_DEAD_NADE = "ぼくは ひつよう なかったんだね…";
	public const string MSG_DEAD_BONE = "・・・・・・";
	public const string MSG_END = "あそんで くれて ありがとう";
	public const string MSG_JOB_START = "どうがを みて コインを かせぎますか？\n（ どうがは さいごまで みてね ）";
	public const string MSG_CREANING_START = "そうじを するには\nコインが １まい ひつよう です";
	public const string MSG_EAT_START = "ごはんを ようい するには\nコインが １まい ひつよう です";
	public const string MSG_NO_JOB = "いい しごとは みつかりません でしたが\nコインを ひろいました";
	public const string MSG_NO_COIN = "こいんが たりません。\nどうがを みて コインを かせぎますか？\n（ どうがは さいごまで みてね ）";

	//メッセージウィンドウの種類
	//この値でメッセージウィンドウでOKとNOボタンが押されたときの処理を決める
	public enum E_MSG_TYPE
	{
		EAT = 0,	//食事
		CLEANING,	//そうじ
		JOB,		//バイト
		NO_JOB		//動画再生失敗
	};

	//ステータス
	//数字が大きいほうが優先
	public enum E_STATUS
	{
		NORMAL = 0,		//通常
		HAPPY,			//喜び
		ANGRY,			//怒り（１日以上撫でられなければ）
		HUNGRY			//腹減り（12時間以上ごはんなし）
	};

	//うんこの流れ
	public enum E_UNKO_FLOW
	{
		ANIMATION = 0,		//ふんばりアニメーション
		OUTPUT				//出す
	};

	//食事の流れ
	public enum E_EAT_FLOW
	{
		MOVE = 0,	//食事に近づくために移動する
		ANIMATION,	//食べるアニメーション
		END			//食事を消す
	};

	public const int SEC_CHANGE_ACTION = 3;     //動きを切り替える時間（秒）

	//進化関係
	public const int EVOLUTION_HINADORY_SEC = 60 * 60 * 24;	//ひな鳥に進化するまでの時間（秒）
	public const int EVOLUTION_NIWATORI_SEC = 60 * 60 * 24 * 2;	//鶏に進化するまでの時間（秒）
	public const int EVOLUTION_DEAD_SEC = 60 * 60 * 24 * 3;		//鶏が死体するまでの時間（秒）（基本値）
	public const int EVOLUTION_BONE_SEC = 60 * 60 * 24;		//死体が骨になるまでの時間（秒）

	//なで時間
	public const int NADE_ADD_LIFE_NADE_SEC = 10;		//この時間撫でると寿命が延びる（秒）
	public const int NADE_ADD_LIFE_ADD_SEC = 60;		//撫でられた時間に応じて延びる寿命（秒）
	public const int NADE_STATUS_HAPPY_SEC = 10;		//この時間撫でられたらstatusがHAPPYになる（秒）
	public const int NADE_STATUS_ANGRY_SEC = 60 * 60;	//この時間撫でられなければstatusがANGRYになる（秒）
	public const int NADE_STATUS_STOP_ANGRY_SEC = 10;	//この時間以上なでられればANGRYじゃなくなる（秒）

	//食事関係
	public const int EAT_HUNGRY_SEC = 60 * 60 * 3;			//食事してからお腹すくまでの時間（秒）
	public const int EAT_DEAD_HUNGRY_SEC = 60 * 60 * 24;	//この時間食事していなかったら死亡（秒）
	public const int EAT_STATUS_HUNGRY_SEC = 60 * 60 * 12;	//この時間食事しなかったらstatusがHUNGRYになる（秒）

	//うんこ関係
	public const int UNKO_INTERVALS_SEC = 60 * 60;	//うんこをする間隔（秒）
	public const int UNKO_DEAD_NUM = 30;		//この数以上うんこ貯めたら死亡

}
