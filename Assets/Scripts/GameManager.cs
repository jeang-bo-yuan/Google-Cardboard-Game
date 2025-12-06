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
    public static GameManager Instance { get; private set; }
    [SerializeField] private int game_level = 1;
    private int room_type = 0; // 0: normal; 1: abnormal
    [SerializeField] private List<GameObject> room_prefabs = new List<GameObject>();
    public Transform roomParent;
    public GameObject current_room;
    public TextMeshPro level_text;

    public GameObject playerCamera;
    public GameObject resultPanel;
    private bool isGameEnded = false;

    void Awake()
    {
        if (Instance != null)
            Debug.LogError("more than one GameManager instance");

        Instance = this;
        resultPanel.SetActive(false);
    }

    void Start()
    {
        game_level = 1;
        changeRoom();
    }

    void Update()
    {
        if (game_level == 2 && !isGameEnded) { 
            isGameEnded = true;
            
            MouseWalker walker = FindFirstObjectByType<MouseWalker>();
            if (walker != null) walker.enabled = false;

            //MouseRotater rotater = FindFirstObjectByType<MouseRotater>();
            //if (rotater != null) rotater.enabled = false;

            if (resultPanel != null) resultPanel.SetActive(true);
        }    
    }

    public void changeRoom()
    {
        int type = 0;
        
        if (game_level > 1)
        {
            //random
            int rand = Random.Range(0, 2);
            if (rand == 0) // normal room
            {
                type = 0;
                room_type = 0;
            }
            else
            {
                rand = Random.Range(1, room_prefabs.Count);
                type = rand;
                room_type = 1;
            }
        }
        else
        {
            room_type = 0;
        }

        //delete and instantiate room
        if (current_room != null)
        {
            Destroy(current_room);
        }
        current_room = Instantiate(room_prefabs[type], roomParent);

        //update level text
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
