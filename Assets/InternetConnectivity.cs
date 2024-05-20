using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class InternetConnectivity : MonoBehaviour
{
    private TcpClient socketConnection;
    private NetworkStream networkStream;

    // Server IP and port
    public string serverIP = "127.0.0.1";
    public int serverPort = 8080;

    void Start()
    {
        ConnectToServer();
    }

    void ConnectToServer()
    {
        try
        {
            socketConnection = new TcpClient(serverIP, serverPort);
            networkStream = socketConnection.GetStream();
            Debug.Log("Connected to server.");
        }
        catch (Exception e)
        {
            Debug.Log("Socket error: " + e);
        }
    }

    void Update()
    {
        if (networkStream != null && networkStream.DataAvailable)
        {
            byte[] bytes = new byte[socketConnection.ReceiveBufferSize];
            networkStream.Read(bytes, 0, bytes.Length);
            string serverMessage = Encoding.UTF8.GetString(bytes).TrimEnd('\0');
            Debug.Log("Server message received: " + serverMessage);
        }
    }

    public void SendMessageToServer(string message)
    {
        if (networkStream != null)
        {
            try
            {
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                networkStream.Write(messageBytes, 0, messageBytes.Length);
                networkStream.Flush();
                Debug.Log("Message sent to server: " + message);
            }
            catch (Exception e)
            {
                Debug.Log("Socket send error: " + e);
            }
        }
    }

    void OnApplicationQuit()
    {
        if (networkStream != null)
        {
            networkStream.Close();
        }
        if (socketConnection != null)
        {
            socketConnection.Close();
        }
    }
}
