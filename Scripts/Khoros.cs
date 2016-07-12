using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SocketIO;


[RequireComponent (typeof (Rigidbody))]
public class Khoros : MonoBehaviour {

	public string singRoom;
	public string listenRoom;
	
	private SocketIOComponent socket;
	
	// Properties
	
	public static bool IsConnected
	{
		get
		{
			return Instance.socket.IsConnected;
		}
	}	
	
	// Singleton
	
	private static Khoros instance;
	public static Khoros Instance
	{
		get
		{
			if(instance == null) instance = GameObject.FindObjectOfType<Khoros>();
			return instance;
		}
	}	


	// Use this for initialization.
	
	void OnEnable () {
		socket = gameObject.GetComponent<SocketIOComponent>();				
	}
	
	void Start() {
		socket.On ("connect", OnSocketConnect);
//		socket.On ("disconnect", OnSocketDisonnect);
//		socket.On ("open", OnSocketOpen);
//		socket.On ("close", OnSocketClose);
//		socket.On ("error", OnSocketError);
		
		Debug.Log("Connecting socket.");		
		
		socket.Connect();
	}

	public void OnSocketConnect(SocketIOEvent eventData) {
		Debug.Log("Socket connected.");
		Khoros.Init(socket, singRoom, listenRoom);			
	}
	
//	public void OnSocketDisonnect(SocketIOEvent eventData) {
//		Debug.Log(eventData);		
//	}
//	
//	public void OnSocketOpen(SocketIOEvent eventData) {
//		Debug.Log(eventData);	
//	}
//	
//	public void OnSocketClose(SocketIOEvent eventData) {
//		Debug.Log(eventData);	
//	}
//
//	public void OnSocketError(SocketIOEvent eventData) {
//		Debug.Log(eventData);
//	}

	// Initilize Khoros.

	public static void Init(SocketIOComponent socket, string singRoom, string listenRoom) {
	
		if (!string.IsNullOrEmpty(listenRoom)) {
			Debug.Log("Khoros: Listening to room : " + listenRoom);
			string jsonString = "{ \"khoros\": { \"room\": \"" + listenRoom + "\" } }";
			socket.Emit("khoros.join", new JSONObject(jsonString));
		} else {
			Debug.Log("Khoros: Not listening to any room.");
		}
		
	}
	
	// Sing broadcasts data.
	
	public static void Sing(string song, JSONObject data) {
		if (!string.IsNullOrEmpty(Instance.singRoom)) {
			JSONObject khorosObject = new JSONObject();
			khorosObject.AddField("room", Instance.singRoom);		
			data.AddField("khoros", khorosObject);		
			Instance.socket.Emit("khoros." + song, data);
		} else {
			Debug.Log("Khoros: No sing room defined.");
		}	
	}
	
	// Listen receives data.
	
	public static void Listen(string song, Action<JSONObject> callback) {
		Instance.socket.On("khoros." + song, (SocketIOEvent eventData) => {
			callback(eventData.data);
		});
	}

	
}
