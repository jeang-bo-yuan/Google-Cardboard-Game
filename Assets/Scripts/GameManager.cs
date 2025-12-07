using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum TpPointType
{
    ENTRY,
    EXIT
}

public class GameManager : MonoBehaviour
{   
    static readonly public int FINAL_ROOM_LEVEL = 9;

    public static GameManager Instance { get; private set; }
    
    [SerializeField] private int game_level = 1;
    private int room_type = 0; // 0: normal; 1: abnormal

    [Tooltip("index 0 -> 正常房間。最後一項 -> 結局房")]
    [SerializeField] private List<GameObject> room_prefabs = new List<GameObject>();
    public Transform roomParent;
    public GameObject current_room;
    public TextMeshPro level_text;

    public GameObject ExitHallway;

    void Awake()
    {
        if (Instance != null)
            Debug.LogError("more than one GameManager instance");

        Instance = this;
    }

    void Start()
    {
        game_level = 1;
        changeRoom();
    }

    public void changeRoom()
    {
        int type = 0;
        
        // 結局房
        if (game_level == FINAL_ROOM_LEVEL)
        {
            type = room_prefabs.Count - 1;
            room_type = 0;
            ExitHallway.SetActive(false);
        }
        // 隨機選房
        else if (game_level > 1)
        {
            //random
            int rand = Random.Range(0, 10);
            if (rand <= 3) // normal room
            {
                type = 0;
                room_type = 0;
            }
            else
            {
                rand = Random.Range(1, room_prefabs.Count - 1);
                type = rand;
                room_type = 1;
            }
        }
        // Level 1 必為正常
        else
        {
            room_type = 0;
            ExitHallway.SetActive(true);
        }

        //delete and instantiate room
        if (current_room != null)
        {
            Destroy(current_room);
        }
        current_room = Instantiate(room_prefabs[type], roomParent);

        //update level text
        if (game_level == FINAL_ROOM_LEVEL)
            level_text.text = "";
        else
            level_text.text = "Level: " + game_level.ToString();
    }

    public void computeLevel(TpPointType type)
    {
        if(room_type == 0) // normal room
        {
            if (type == TpPointType.EXIT)
            {
                game_level += 1;
            }
            else if (type == TpPointType.ENTRY)
            {
                game_level = 1;
            }
        }
        else if(room_type == 1) // abnormal room
        {
            if (type == TpPointType.EXIT)
            {
                game_level = 1;
            }
            else if (type == TpPointType.ENTRY)
            {
                game_level += 1;
            }
        }
    }

    public void resetGame()
    {
        // Scene restart (Best for now)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void backToMenu()
    {
        // Load menu scene
        SceneManager.LoadScene("Menu");   
    }
}
