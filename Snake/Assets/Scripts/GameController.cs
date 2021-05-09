using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private int eggIncrementForLevel = 5;
    private static GameController instance;
    public static GameController Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private SnakeHead snakeHead = null;
    [SerializeField]
    private Walls walls = null;
    [SerializeField]
    private Eggs eggs = null;

    [SerializeField]
    private Spikes spikes = null;

    public bool isPlaying = false;

    private int level = 1;
    private int numberOfEggsForNextLevel = 5;

    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text highScoreText;
    private int score;
    private int highScore;

    [SerializeField]
    private GameObject gameOver;
    [SerializeField]
    private GameObject tapToPlay;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PrepareLevel();
    }

    private void Update()
    {
        if (!isPlaying)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Ended)
                {
                    StartLevel();
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                StartLevel();
            }
        }
    }

    private void PrepareLevel()
    {
        walls.CreateWalls();
    }

    private void StartLevel()
    {
        if (!isPlaying)
        {
            spikes.RemoveSpikes();
        }

        OnScoreChanged();
        gameOver.SetActive(false);
        tapToPlay.SetActive(false);
        eggs.RemoveEggs();        
        snakeHead.ResetSnake();
        eggs.CreateEgg();
        spikes.CreateSpike();
        isPlaying = true;
    }

    public void GameOver()
    {
        ChangeLevel(1);
        score = 0;
        isPlaying = false;
        gameOver.SetActive(true);
        tapToPlay.SetActive(true);
    }

    public Vector2 GetRandomPositionOnScreen()
    {
        Vector2 bounds = GetScreenBounds();
        float width = bounds.x;
        float height = bounds.y;

        Vector3 position;
        position.x = -width + Random.Range(1, (width * 2) - 2);
        position.y = -height + Random.Range(1, (height * 2) - 2);
        position.z = 0;

        return position;
    }

    public Vector2 GetScreenBounds()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    }

    public void EggEaten(Egg egg)
    {
        egg.gameObject.SetActive(false);
        numberOfEggsForNextLevel--;
        score += egg.data.partsToAdd;

        if (numberOfEggsForNextLevel <= 0)
        {
            ChangeLevel(++level);
            StartLevel();
        }
        else if (numberOfEggsForNextLevel == 1)
        {
            eggs.CreateEgg(true);
        }
        else
        {
            eggs.CreateEgg();
        }

        OnScoreChanged();

    }

    private void OnScoreChanged()
    {
        scoreText.text = $"Score = {score}";

        if (score > highScore)
        {
            highScore = score;
            highScoreText.text = $"HighScore = {highScore}";
        }
    }

    private void ChangeLevel(int level)
    {
        this.level = level;
        numberOfEggsForNextLevel = eggIncrementForLevel * level;
        snakeHead.speed = 1 + (level - 1) / 4f;
        if (snakeHead.speed > snakeHead.maxSpeed)
        {
            snakeHead.speed = snakeHead.maxSpeed;
        }
    }
}
