using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {

	public void OnClickPlay() {
		Application.LoadLevel("Level 1");
	}
}
