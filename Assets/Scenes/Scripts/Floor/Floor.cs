using System.Data;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Floor : MonoBehaviour
{
    public static Floor Instance;

    [SerializeField] RoomSpawner roomSpawner;
    [SerializeField] Map debugMap;
    [SerializeField] Map randomMap;

    GameObject player;

    void Awake(){
        Instance = this;
    }

    void Start()
    {
        player = Player.Instance.gameObject;
        randomMap = MapGenerator.RandomMap(10, 10, 10, 20, 0.2f, 0.2f);
        roomSpawner.SpawnFromMap(randomMap);
        player.transform.position = roomSpawner.StartRoom.Layout.ItemSpawnPoint.position;
        Player.Instance.GetComponent<Rigidbody2D>().position = roomSpawner.StartRoom.transform.position;
        Debug.Log(Player.Instance);
        Debug.Log(roomSpawner.StartRoom.transform.position);
    }

    public void Restart()
    {
        // destroy player 
        GameManager.Instance.Restart();
    }

    public void GoToNext()
    {
        // Restart current floor
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}