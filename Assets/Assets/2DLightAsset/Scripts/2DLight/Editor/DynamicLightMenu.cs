using UnityEngine;
using UnityEditor;
using System.Collections;

public class DynamicLightMenu : Editor {

	static internal DynamicLight light;
	const string menuPath = "GameObject/2D Dynamic Light [Free]";


	[MenuItem ( menuPath + "/Lights/ ☀ Radial No Material ",false,21)]
	static void addRadialNoMat(){
		Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2DLightAsset/Prefabs/Lights/2DPointLight.prefab", typeof(GameObject));
		GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
		hex.transform.position = new Vector3(0,0,0);
		hex.name = "2DRadialPoint";
	}

	[MenuItem ( menuPath + "/Lights/ ☀ Radial Procedural Gradient ",false,31)]
	static void addRadialGradient(){
		Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2DLightAsset/Prefabs/Lights/2DPointLightWithGradient.prefab", typeof(GameObject));
		GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
		hex.transform.position = new Vector3(0,0,0);
		hex.name = "2DRadialGradientPoint";
	}

	[MenuItem ( menuPath + "/Lights/ ☀ Pseudo Spot Light ",false,41)]
	static void addPseudo(){
		Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2DLightAsset/Prefabs/Lights/2DPseudoSpotLight.prefab", typeof(GameObject));
		GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
		hex.transform.position = new Vector3(0,0,0);
		hex.name = "2DRadialGradientPoint";
	}


	#region Casters Zone

	[MenuItem ( menuPath + "/Casters/Square",false,66)]
	static void addSquare(){
		
		Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2DLightAsset/Prefabs/Casters/square.prefab", typeof(GameObject));
		GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
		hex.transform.position = new Vector3(5,0,0);
		hex.name = "Square";
	}

	[MenuItem ( menuPath + "/Casters/Hexagon",false,67)]
	static void addHexagon(){
		
		Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2DLightAsset/Prefabs/Casters/hexagon.prefab", typeof(GameObject));
		GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
		hex.transform.position = new Vector3(5,0,0);
		hex.name = "Hexagon";
	}

	[MenuItem ( menuPath + "/Casters/Pacman",false,68)]
	static void addPacman(){
		
		Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2DLightAsset/Prefabs/Casters/pacman.prefab", typeof(GameObject));
		GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
		hex.transform.position = new Vector3(5,0,0);
		hex.name = "Pacman";
	}
	[MenuItem ( menuPath + "/Casters/Star",false,69)]
	static void addStar(){

		Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2DLightAsset/Prefabs/Casters/star.prefab", typeof(GameObject));
		GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
		hex.transform.position = new Vector3(5,0,0);
		hex.name = "Star";
	}


	#endregion

}
