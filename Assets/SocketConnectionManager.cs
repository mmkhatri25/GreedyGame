using UnityEngine;
using System.Collections;
using WebSocketSharp;

public class SocketConnectionManager : MonoBehaviour
{
    public string url = "ws://dragonan-7up.herokuapp.com/socket.io/?EIO=4&transport=websocket";
    private WebSocket ws;
    private bool isConnected = false;

    private void Start()
    {
        Connect();
        StartCoroutine(CheckAndReconnect());
    }

    private void Connect()
    {
        ws = new WebSocket(url);
        ws.OnOpen += OnOpen;
        ws.OnClose += OnClose;
        ws.ConnectAsync();
    }

    private void OnOpen(object sender, System.EventArgs e)
    {
        isConnected = true;
        Debug.Log("Socket connected.");
    }

    private void OnClose(object sender, CloseEventArgs e)
    {
        isConnected = false;
        Debug.Log("Socket disconnected.");
        // You can handle the reconnection logic here if needed
    }

    private void OnDestroy()
    {
        if (ws != null && ws.IsAlive)
            ws.Close();
    }

    private void Update()
    {
        // Check WebSocket connection status
        if (ws != null && !ws.IsAlive)
        {
            isConnected = false;
            Debug.Log("Socket disconnected.");
            // You can handle the disconnection here
        }
    }

    private IEnumerator CheckAndReconnect()
    {
        while (true)
        {
            if (Application.internetReachability != NetworkReachability.NotReachable && !isConnected)
            {
                Debug.Log("Internet connection available. Attempting to reconnect...");
                Connect();
            }
            yield return new WaitForSeconds(5f); // Check every 5 seconds
        }
    }
}
