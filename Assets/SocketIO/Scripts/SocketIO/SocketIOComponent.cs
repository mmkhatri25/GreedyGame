using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

namespace SocketIO
{
    
    public class SocketIOComponent : MonoBehaviour
    {
        #region Public Properties
        AsyncOperation MainScene;
        public string url = "ws://dragonan-7up.herokuapp.com/socket.io/?EIO=4&transport=websocket";
        public string UAT_url = "ws://dragonan-7up.herokuapp.com/socket.io/?EIO=4&transport=websocket";
        public string Production_url = "ws://dragonan-7up.herokuapp.com/socket.io/?EIO=4&transport=websocket";
        public bool isUAT;
        public bool autoConnect = true;
        public int reconnectDelay = 5;
        public float ackExpirationTime = 1800f;
        public float pingInterval = 25f;
        public float pingTimeout = 60f;

        public WebSocket socket { get { return ws; } }
        public string sid { get; set; }
        public bool IsConnected { get { return connected; } }
        public bool MyScoketDisconnected = false;

        #endregion

        #region Private Properties

        public volatile bool connected;
        private volatile bool thPinging;
        private volatile bool thPong;
        private volatile bool wsConnected;

        private Thread socketThread;
        private Thread pingThread;
        private WebSocket ws;

        private Encoder encoder;
        private Decoder decoder;
        private Parser parser;

        private Dictionary<string, List<Action<SocketIOEvent>>> handlers;
        private List<Ack> ackList;

        private int packetId;

        private object eventQueueLock;
        private Queue<SocketIOEvent> eventQueue;

        private object ackQueueLock;
        private Queue<Packet> ackQueue;

        //================//
        public bool isConnected = true;
        public bool isSocketConnected = true;
        public bool gameReloaded = false;

        //===============//

        public bool firstConnectionAttempt = true;

        #endregion

        #region Unity interface

        public static SocketIOComponent instance;

        private bool getIntentData()
        {
            // Implement getIntentData logic
            return false;
        }

        public bool CreatePushClass(AndroidJavaClass UnityPlayer)
        {
            // Implement CreatePushClass logic
            return false;
        }

        private AndroidJavaObject GetExtras(AndroidJavaObject intent)
        {
            // Implement GetExtras logic
            return null;
        }

        private string GetProperty(AndroidJavaObject extras, string name)
        {
            // Implement GetProperty logic
            return string.Empty;
        }

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else if (instance != this)
            {
                Destroy(this.gameObject);
            }


            // Set URL based on environment
            if (isUAT)
                url = UAT_url;
            else
                url = Production_url;

            encoder = new Encoder();
            decoder = new Decoder();
            parser = new Parser();
            handlers = new Dictionary<string, List<Action<SocketIOEvent>>>();
            ackList = new List<Ack>();
            sid = null;
            packetId = 0;

            // Initialize WebSocket
            ws = new WebSocket(url);
            ws.OnOpen += OnOpen;
            ws.OnMessage += OnMessage;
            ws.OnError += OnError;
            ws.OnClose += OnClose;

            wsConnected = false;

            eventQueueLock = new object();
            eventQueue = new Queue<SocketIOEvent>();

            ackQueueLock = new object();
            ackQueue = new Queue<Packet>();

            connected = false;
            UnityMainThreadDispatcher.Instance();
        }

        public void Start()
        {
            if (autoConnect)
                Connect();
        }

        public void Connect()
        {
            connected = true;

            socketThread = new Thread(RunSocketThread);
            socketThread.Start(ws);

            pingThread = new Thread(RunPingThread);
            pingThread.Start(ws);
            Debug.Log("in Connect() now ");
        }

        public void Update()
        {
            // Process event and ack queues
            lock (eventQueueLock)
            {
                while (eventQueue.Count > 0)
                {
                    EmitEvent(eventQueue.Dequeue());
                }
            }

            lock (ackQueueLock)
            {
                while (ackQueue.Count > 0)
                {
                    InvokeAck(ackQueue.Dequeue());
                }
            }

            if (wsConnected != ws.IsConnected)
            {
                wsConnected = ws.IsConnected;
                if (wsConnected)
                {
                    EmitEvent("connect");
                }
                else
                {
                    EmitEvent("disconnect");
                }
            }

            // GC expired acks
            if (ackList.Count == 0) return;
            if (DateTime.Now.Subtract(ackList[0].time).TotalSeconds < ackExpirationTime) return;
            ackList.RemoveAt(0);
        }

        public void OnDestroy()
        {
            if (socketThread != null)
            {
                connected = false; // Ensure thread loop ends
                socketThread.Join();
            }
            if (pingThread != null)
            {
                connected = false; // Ensure thread loop ends
                pingThread.Join();
            }
        }

        public void OnApplicationQuit()
        {
            Close();
        }

        public void Close()
        {
            EmitClose();
            connected = false;
        }

        public void On(string ev, Action<SocketIOEvent> callback)
        {
            if (!handlers.ContainsKey(ev))
            {
                handlers[ev] = new List<Action<SocketIOEvent>>();
            }
            handlers[ev].Add(callback);
        }

        public void Off(string ev, Action<SocketIOEvent> callback)
        {
            if (!handlers.ContainsKey(ev))
            {
                Debug.Log("[SocketIO] No callbacks registered for event: " + ev);
                return;
            }

            List<Action<SocketIOEvent>> l = handlers[ev];
            if (!l.Contains(callback))
            {
                Debug.Log("[SocketIO] Couldn't remove callback action for event: " + ev);
                return;
            }

            l.Remove(callback);
            if (l.Count == 0)
            {
                handlers.Remove(ev);
            }
        }

        public void Emit(string ev)
        {
            EmitMessage(-1, string.Format("[\"{0}\"]", ev));
        }

        public void Emit(string ev, Action<JSONObject> action)
        {
            EmitMessage(++packetId, string.Format("[\"{0}\"]", ev));
            ackList.Add(new Ack(packetId, action));
        }

        public void Emit(string ev, JSONObject data)
        {
            EmitMessage(-1, string.Format("[\"{0}\",{1}]", ev, data));
        }

        public void Emit(string ev, JSONObject data, Action<JSONObject> action)
        {
            EmitMessage(++packetId, string.Format("[\"{0}\",{1}]", ev, data));
            ackList.Add(new Ack(packetId, action));
        }

        public void Emit(string ev, string data, Action<JSONObject> action)
        {
            EmitMessage(++packetId, string.Format("[\"{0}\",{1}]", ev, data));
            ackList.Add(new Ack(packetId, action));
        }

        private void RunSocketThread(object obj)
        {
            WebSocket webSocket = (WebSocket)obj;
            try
            {
                while (connected)
                {
                    if (!webSocket.IsConnected)
                    {
                        //if (firstConnectionAttempt)
                        //{
                        //    firstConnectionAttempt = false;
                        //}
                        //else
                        //{
                        //    MyScoketDisconnected = true;
                        //    Debug.Log("Socket disconnected. Reloading the scene...");
                        //    ReloadScene();
                        //    break;
                        //}

                        Debug.Log("Socket disconnected. Attempting to reconnect...");
                        webSocket.Connect();
                    }
                    Thread.Sleep(reconnectDelay * 1000); // Convert seconds to milliseconds
                }
                webSocket.Close();
            }
            catch (ThreadAbortException e)
            {
                webSocket.Close();
                Thread.ResetAbort();
                Debug.Log("Thread Abort Exception Occurred and Caught: " + e.Message);
            }
        }

        private void ReloadScene()
        {
            // Ensure this runs on the main thread, as Unity API calls are not thread-safe
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                //Titli_ServerResponse.Instance.removeSocketListener();
                //PlayerPrefs.SetInt("firstConnectionAttempt", 1);

                if (ws != null)
                {
                    ws.OnOpen -= OnOpen;
                    ws.OnMessage -= OnMessage;
                    ws.OnError -= OnError;
                    ws.OnClose -= OnClose;
                }

                //SceneManager.LoadScene(0);
                StartCoroutine(WaitAndLoadScene());

            });
        }
        private IEnumerator WaitAndLoadScene()
        {
            
            yield return new WaitForSeconds(2.0f);
            print("333 Titli_ServerResponse Removed Listners..." + MyScoketDisconnected);

            // Load the scene
            SceneManager.LoadScene(0);
        }

        private void RunPingThread(object obj)
        {
            WebSocket webSocket = (WebSocket)obj;

            int timeoutMilis = Mathf.FloorToInt(pingTimeout * 1000);
            int intervalMilis = Mathf.FloorToInt(pingInterval * 1000);

            DateTime pingStart;

            try
            {
                while (connected)
                {
                    if (!wsConnected)
                    {
                        Thread.Sleep(reconnectDelay * 1000);
                    }
                    else
                    {
                        thPinging = true;
                        thPong = false;

                        EmitPacket(new Packet(EnginePacketType.PING));
                        pingStart = DateTime.Now;

                        while (webSocket.IsConnected && thPinging && (DateTime.Now.Subtract(pingStart).TotalMilliseconds < timeoutMilis))
                        {
                            Thread.Sleep(200);
                        }

                        if (!thPong)
                        {
                            webSocket.Close();
                        }

                        Thread.Sleep(intervalMilis);
                    }
                }
            }
            catch (ThreadAbortException e)
            {
                Debug.Log("Thread Abort Exception Occurred and Caught: " + e.Message);
                Thread.ResetAbort();
            }
        }

        private void EmitMessage(int id, string raw)
        {
            Packet packet = new Packet(EnginePacketType.MESSAGE, SocketPacketType.EVENT, 0, "/", id, new JSONObject(raw));
            EmitPacket(packet);
        }

        private void EmitClose()
        {
            Packet packet = new Packet(EnginePacketType.MESSAGE, SocketPacketType.DISCONNECT, 0, "/", -1, new JSONObject(string.Empty));
            EmitPacket(packet);
        }

        private void EmitPacket(Packet packet)
        {
            if (!connected)
            {
                Debug.Log("Can't emit packet: not connected");
                return;
            }

            try
            {
                ws.Send(encoder.Encode(packet));
            }
            catch (Exception ex)
            {
                Debug.Log("WebSocket send exception: " + ex.Message);
            }
        }

        private void EmitEvent(string type)
        {
            EmitEvent(new SocketIOEvent(type));
        }

        private void EmitEvent(SocketIOEvent ev)
        {
            if (!handlers.ContainsKey(ev.name)) return;
            foreach (Action<SocketIOEvent> handler in handlers[ev.name])
            {
                try
                {
                    handler(ev);
                }
                catch (Exception ex)
                {
                    Debug.Log("Exception in event handler: " + ex.Message);
                }
            }
        }

        private void InvokeAck(Packet packet)
        {
            for (int i = 0; i < ackList.Count; i++)
            {
                if (ackList[i].packetId != packet.id) continue;
                ackList[i].Invoke(packet.json);
                ackList.RemoveAt(i);
                return;
            }
        }

        private void OnOpen(object sender, EventArgs e)
        {
            EmitEvent("open");
            Debug.Log("Socket connected.");
            wsConnected = true;
            MyScoketDisconnected = true;
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            Packet packet = decoder.Decode(e);
            switch (packet.enginePacketType)
            {
                case EnginePacketType.OPEN:
                    HandleOpen(packet);
                    break;
                case EnginePacketType.CLOSE:
                    EmitEvent("close");
                    break;
                case EnginePacketType.PING:
                    HandlePing();
                    break;
                case EnginePacketType.PONG:
                    HandlePong();
                    break;
                case EnginePacketType.MESSAGE:
                    HandleMessage(packet);
                    break;
            }
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            EmitEvent("error");
        }

        private void OnClose(object sender, CloseEventArgs e)
        {
            EmitEvent("close");
            Debug.Log("OnClose Socket disconnected.");
            wsConnected = false;
           
        }

        private void HandleOpen(Packet packet)
        {
            Debug.Log("Received OPEN packet");

            sid = packet.json["sid"].str;
            EmitEvent("open");
        }

        private void HandlePing()
        {
            EmitPacket(new Packet(EnginePacketType.PONG));
        }

        private void HandlePong()
        {
            thPong = true;
            thPinging = false;
        }

        private void HandleMessage(Packet packet)
        {
            if (packet.json == null) return;

            if (packet.socketPacketType == SocketPacketType.ACK)
            {
                lock (ackQueueLock) { ackQueue.Enqueue(packet); }
                return;
            }

            if (packet.socketPacketType == SocketPacketType.EVENT)
            {
                SocketIOEvent e = parser.Parse(packet.json);
                lock (eventQueueLock) { eventQueue.Enqueue(e); }
            }
        }

       
    }

    // Singleton dispatcher class to run actions on the main thread
    public class UnityMainThreadDispatcher : MonoBehaviour
    {
        private static readonly Queue<Action> _executionQueue = new Queue<Action>();

        public void Update()
        {
            lock (_executionQueue)
            {
                while (_executionQueue.Count > 0)
                {
                    _executionQueue.Dequeue().Invoke();
                }
            }
        }

        public void Enqueue(Action action)
        {
            lock (_executionQueue)
            {
                _executionQueue.Enqueue(action);
            }
        }

        private static UnityMainThreadDispatcher _instance = null;

        public static UnityMainThreadDispatcher Instance()
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<UnityMainThreadDispatcher>();
                if (!_instance)
                {
                    var obj = new GameObject("MainThreadDispatcher");
                    _instance = obj.AddComponent<UnityMainThreadDispatcher>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }
}
#endregion
