using UnityEngine;
using System.Collections;

public class Listener : MonoBehaviour {

	private Vector3 position;

	void Start () {
		Khoros.Listen("position", (JSONObject data) => {	
			position = JSONTemplates.ToVector3(data);			
			transform.position = position;			
		});
	}

}
