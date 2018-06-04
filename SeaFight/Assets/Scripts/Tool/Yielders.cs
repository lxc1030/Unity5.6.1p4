using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Yielders : MonoBehaviour 
{
	//--------------------------------------------------------------------------------------WaitSecond
	static private Dictionary<float, WaitForSeconds> _timeInterval = new Dictionary<float, WaitForSeconds>(100);
	static private WaitForSeconds _timeIntervalValue;
	static public WaitForSeconds WaitSecond(float seconds)
	{
		if (!_timeInterval.TryGetValue(seconds, out _timeIntervalValue))
		{
			_timeInterval.Add(seconds, new WaitForSeconds(seconds));
		}
		return _timeInterval[seconds];
	}

	//--------------------------------------------------------------------------------------WaitEndOfFrame
	static private WaitForEndOfFrame _endIfFrame = new WaitForEndOfFrame();
	static public WaitForEndOfFrame WaitEndOfFrame
	{
		get { return _endIfFrame; }
	}

	//--------------------------------------------------------------------------------------WaitFixedUpdate
	static private WaitForFixedUpdate _fixedUpdate = new WaitForFixedUpdate();
	static public WaitForFixedUpdate WaitFixedUpdate
	{
		get { return _fixedUpdate; }
	}
}