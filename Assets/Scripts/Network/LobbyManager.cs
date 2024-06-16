using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField]
    private Button startHost;

    private float fallbackDelay = 5.0f;

    public int connectedClientsCount = 0;
    public GameObject levelManagerPrefab;
    private GameObject levelManagerInstance;

    public string SceneName = "Egyot";
    private void Awake()
    {
        Cursor.visible = true;
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {


        Debug.Log(NetworkManager.Singleton.LocalClientId);

        startHost.onClick.AddListener(() =>
        {
            TryQuickJoin();
            
        });
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

    }

    private void TryQuickJoin()
    {
        Debug.Log("Trying to connect as client...");
        StartCoroutine(TryJoinOrHost());
    }

    private IEnumerator TryJoinOrHost()
    {
        NetworkManager.Singleton.StartClient();
        Debug.Log("Successfully started as client, waiting for approval...");

        // Wait for a while to see if the client connects successfully
        yield return new WaitForSeconds(fallbackDelay);

        // If the client is no longer connected, fall back to hosting
        if (!NetworkManager.Singleton.IsClient || !NetworkManager.Singleton.IsConnectedClient)
        {
            Debug.Log("Client was not approved or disconnected. Starting as host...");
            NetworkManager.Singleton.Shutdown();
            yield return new WaitForSeconds(fallbackDelay);
            StartHost();
        }
        else
        {
            Debug.Log("Client is connected successfully.");
            
        }
    }

    private void StartHost()
    {
          // Ensure the client is stopped before hosting
        if (NetworkManager.Singleton.StartHost())
        {
            Debug.Log("Successfully started as host.");
        }
        else
        {
            Debug.LogError("Failed to start as host.");
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        connectedClientsCount++;
        Debug.Log($"Client connected: {clientId}. Total clients: {connectedClientsCount}");
        CheckClientCount();
    }
    private void OnClientDisconnected(ulong clientId)
    {
        connectedClientsCount--;
    }

    private void CheckClientCount()
    {
        
        // Ensure that the game scene is only loaded if the host is the one making the decision
        if (NetworkManager.Singleton.IsHost && connectedClientsCount == 2)
        {
            Debug.Log("Both players connected");
            StartCoroutine(Delay());
            LoadGameScene();
        }
    }

    private void LoadGameScene()
    {
        // Load the scene on the server and synchronize across clients
        SceneManager.sceneLoaded += OnGameSceneLoaded;
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Egyot", LoadSceneMode.Single);
        }
    }
    private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Egyot")
        {
            SceneManager.sceneLoaded -= OnGameSceneLoaded;
            StartCoroutine(SpawnNetworkObjects());
        }
    }

    private IEnumerator SpawnNetworkObjects()
    {
        // Ensure the NetworkManager has completed its initialization
        yield return new WaitForSeconds(2);

        // Check and spawn all NetworkObjects
        Debug.Log("Checking and spawning network objects...");
        NetworkObject[] networkObjects = FindObjectsOfType<NetworkObject>();
        foreach (var netObj in networkObjects)
        {
            Debug.Log($"Checking NetworkObject: {netObj.gameObject.name}, IsSpawned: {netObj.IsSpawned}");
            if (!netObj.IsSpawned)
            {
                netObj.Spawn();
                Debug.Log($"Spawning: {netObj.gameObject.name}");
                if (NetworkManager.Singleton.IsServer)  // Ensure only the server spawns objects
                {
                    
                }
            }
        }
    }
        private IEnumerator Delay()
    {
        yield return new WaitForSeconds(10f);
    }
}