using UnityEngine;
using System.Collections;

public enum DIRECTION { 
	UP = 0,
	RIGHT,
	DOWN,
	LEFT
}

public static class Helper {
	static Vector3[] localDirections = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };

	public static Vector3 Vector3FromDIRECTIONS(DIRECTION d) {
		return localDirections[(int)d];
	}
}