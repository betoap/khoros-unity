using UnityEngine;
using System.Collections;

public class Singer : MonoBehaviour {

	void Update() {
		if (Khoros.IsConnected) {
			JSONObject data =  JSONTemplates.FromVector3( transform.position );
			Khoros.Sing("position", data );
		}
	}

}

