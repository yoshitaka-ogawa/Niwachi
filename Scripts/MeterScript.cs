using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MeterScript : MonoBehaviour
{
	private const float MIN = 64.0f;
	private const float MAX = 320.0f;

	private RectTransform rt = null;
	private float MoveSize = 0.0f;

	// Use this for initialization
	void Start()
	{
		MoveSize = MAX - MIN;
		rt = this.GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2( MIN, MIN );
	}
	
	//----------------------------------------------------
	//	メーターサイズを0.0f～1.0fの範囲で指定する
	public void SetMeterRate( float rate )
	{
		if( rate < 0.0f )
		{
			rate = 0.0f;
		}
		else if( 1.0f < rate )
		{
			rate = 1.0f;
		}

		float x = MoveSize * rate + MIN;
		rt.sizeDelta = new Vector2( x, MIN );
	}
}
