using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class EggScript : MonoBehaviour
{
	public MeterScript meterScript = null;
	public Text txtMsg = null;

	private bool touchFlg = false;				//タッチされている間ture
	private float touchCnt = 0.0f;				//タッチされた時間を記憶
	private const float TOUCH_MAX_SEC = 10.0f;	//タッチ最大時間

	// Use this for initialization
	void Start ()
	{
		txtMsg.text = DefinedScript.MSG_EGG_START;

		/*
		string evo = GameDataScript.GetEvolution();

		Debug.Log( evo );

		//現在が卵
		if( DefinedScript.EVOLUTION_EGG == evo )
		{
			txtMsg.text = DefinedScript.MSG_EGG_START;
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
		*/
	}
	
	// Update is called once per frame
	void Update ()
	{
		if( touchFlg )
		{
			touchCnt += Time.deltaTime;

			float rate = touchCnt / TOUCH_MAX_SEC;

			meterScript.SetMeterRate( rate );

			//なで時間が終了したらひよこに進化するアニメーションを再生
			if( 1.0f <= rate )
			{
				txtMsg.text = DefinedScript.MSG_EGG_END;

				Animator animator = this.GetComponent<Animator>();
				animator.Play("tamago");
			}
		}
	}

	//-----------------------------------------------------
	//	指が画面に触れたイベント
	void OnMouseDown()
	{
		touchFlg = true;
	}

	//-----------------------------------------------------
	//	指が画面から離れたイベント
	void OnMouseUp()
	{
		touchFlg = false;
	}

	//-----------------------------------------------------
	//	アニメーション終了時
	void OnAnimationEnd()
	{
		//メッセージを更新
		//txtMsg.text = DefinedScript.MSG_EGG_END;

		GameDataScript.SetEvolution( DefinedScript.EVOLUTION_HIYOKO );
		GameDataScript.SetStartTime( System.DateTime.Now );
		touchCnt = 0.0f;
		touchFlg = false;
		SceneManager.LoadScene("Main");
	}
}
