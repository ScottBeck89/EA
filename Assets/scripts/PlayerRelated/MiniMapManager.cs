using UnityEngine;
using System.Collections;

public class MiniMapManager : MonoBehaviour {

	public Camera minimapCamera;

	void Start () {
		minimapCamera.rect = new Rect(0.1f, 0.1f, 0.25f, 0.25f);
		minimapCamera.clearFlags = CameraClearFlags.Skybox;
		minimapCamera.clearFlags = CameraClearFlags.Depth;
	}
	
	void Update () {
	
	}
}
