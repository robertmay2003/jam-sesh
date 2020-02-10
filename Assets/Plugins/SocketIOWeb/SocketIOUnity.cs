using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

public class SocketIOUnity : MonoBehaviour
{

	#region External Methods

	[DllImport("__Internal")]
	private static extern void SocketIO_Init(string name);

	[DllImport("__Internal")]
	private static extern void SocketIO_Emit(string e, string data);

	[DllImport("__Internal")]
	private static extern void SocketIO_Connect();

	[DllImport("__Internal")]
	private static extern string SocketIO_RequestProperty(string key);

	private static SocketIOUnity _unique;

	#endregion

	#region Inspector Variables

	public bool autoconnect;

	#endregion

	#region Public Variables

	public string id => RequestProperty("id");

	public delegate void SocketEventDelegate(string data);

	#endregion

	#region Private Variables

	private Dictionary<string, SocketEventDelegate> _eventDelegates = new Dictionary<string, SocketEventDelegate>();

	private class OnEventData
	{
		public string eventName;
		public string eventData;

		public OnEventData(string eName, string eData)
		{
			eventName = eName;
			eventData = eData;
		}
	}

	#endregion

	#region MonoBehaviour Methods

	void Awake()
	{
		_unique = this;

		SocketIO_Init(gameObject.name);

		if (autoconnect) { SocketIO_Connect(); }
	}

	#endregion

    #region Private Methods

    private string RequestProperty(string key)
    {
		// IntPtr keyPointer = SocketIO_RequestProperty(key);
	    // string info = Marshal.PtrToStringAuto(infoPointer);
	    string info = SocketIO_RequestProperty(key);

	    // Free string
	    // Marshal.Release(infoPointer);

	    return info;
    }

    private void OnEventString(string e, string data = null)
    {
	    if (_eventDelegates.ContainsKey(e))
	    {
		    _eventDelegates[e](data);
	    }
    }

    #endregion

    #region Public Methods

    public void On(string e, SocketEventDelegate handler)
    {
	    _eventDelegates[e] = handler;
    }
    
    public void Emit(string e)
    {
	    // Debug.Log("Mark 2 (no data)");
	    // Debug.Log("Data: {no data}");
	    SocketIO_Emit(e, "{}");
    }

    public void Emit(string e, object data)
    {
	    // Debug.Log("Mark 2 (object data)");
	    // Debug.Log($"Data: {JsonUtility.ToJson(data)}");
	    string message = (data == null) ? "{no data}" : JsonUtility.ToJson(data);
	    SocketIO_Emit(e, message);
    }
    
    public void Emit(string e, string data)
    {
	    // Debug.Log("Mark 2 (string data)");
	    // Debug.Log($"Data: {data}");
	    SocketIO_Emit(e, data);
    }

    public void OnEvent(string dataString)
    {
	    // string dataString = Marshal.PtrToStringAuto(dataPointer);
	    OnEventData data = JsonUtility.FromJson<OnEventData>(dataString);

	    // Debug.Log($"DataString: {dataString}, Event: {data.eventName}, Data: {data.eventData}");
	    OnEventString(data.eventName, data.eventData);
	    
	    // Free string
	    // Marshal.Release(dataPointer);;
    }

    public void OnDisconnect()
    {
	    OnEventString("disconnect", null);
    }

    #endregion
}
