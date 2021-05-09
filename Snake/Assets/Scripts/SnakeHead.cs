using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : BodyPart
{
    public float maxSpeed = 2.5f;
    public float speed;
    private Vector2 direction;
    private BodyPart tail = null;

    [SerializeField]
    private const float TIMETOADDBODYPART = .1f;
    private float addTimer = TIMETOADDBODYPART;
    private int partsToAdd = 0;

    [SerializeField]
    private BodyPart bodyPartPrefab = null;
    [SerializeField]
    private int bodyPartsCount = 20;
    private List<BodyPart> bodyParts = new List<BodyPart>();

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip dieSound;
    [SerializeField]
    private AudioClip[] eatSounds;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SwipeControls.OnSwipe += SetDirection;
    }

    private void Awake()
    {
        for (int i = 0; i < bodyPartsCount; i++)
        {
            InstantiateBodyPart();
        }
    }

    private BodyPart InstantiateBodyPart()
    {
        var bodyPart = Instantiate(bodyPartPrefab, Vector3.zero, Quaternion.identity, transform.parent).GetComponent<BodyPart>();
        bodyPart.gameObject.SetActive(false);
        bodyParts.Add(bodyPart);

        return bodyPart;
    }

    private BodyPart GetBodyPart()
    {
        foreach (var bodyPart in bodyParts)
        {
            if (!bodyPart.gameObject.activeInHierarchy)
            {
                return bodyPart;
            }
        }

        return InstantiateBodyPart();
    }

    private void RemoveBody()
    {
        foreach (var bodyPart in bodyParts)
        {
            if (bodyPart.gameObject.activeInHierarchy)
            {
                bodyPart.gameObject.SetActive(false);
            }
        }
    }

    protected override void Update()
    {
        if (!GameController.Instance.isPlaying) return;

        base.Update();

        SetMovement(direction * speed * Time.deltaTime);
        UpdatePosition();
        UpdateDirection();

        if (partsToAdd > 0)
        {
            addTimer -= Time.deltaTime;

            if (addTimer <= 0)
            {
                addTimer = TIMETOADDBODYPART;
                AddBodyPart();
                partsToAdd--;
            }
        }
    }

    internal void ResetSnake()
    {
        RemoveBody();
        ClearPreviousPositions();

        tail = null;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        direction = Vector2.up;
        addTimer = TIMETOADDBODYPART;
        partsToAdd = 5;
    }

    private void SetDirection(SwipeControls.SwipeDirection swipeDirecton)
    {
        switch (swipeDirecton)
        {
            case SwipeControls.SwipeDirection.Down:
                direction = Vector2.down;
                break;
            case SwipeControls.SwipeDirection.Right:
                direction = Vector2.right;
                break;
            case SwipeControls.SwipeDirection.Left:
                direction = Vector2.left;
                break;
            case SwipeControls.SwipeDirection.Up:
                direction = Vector2.up;
                break;
        }
    }

    private void AddBodyPart()
    {
        if (tail == null)
        {
            Vector3 newPosition = transform.position;
            newPosition.z += 0.01f;

            BodyPart newPart = GetBodyPart();
            newPart.gameObject.transform.position = newPosition;
            newPart.gameObject.transform.rotation = Quaternion.identity;


            tail = newPart;
            tail.TurnIntoTail();
            tail.following = this;
            tail.gameObject.SetActive(true);
        }
        else
        {
            Vector3 newPosition = tail.transform.position;
            newPosition.z += 0.01f;

            BodyPart newPart = GetBodyPart();
            newPart.gameObject.transform.position = newPosition;
            newPart.gameObject.transform.rotation = tail.transform.rotation;

            newPart.following = tail;
            newPart.TurnIntoTail();
            tail.TurnBodyPartTail();
            tail = newPart;
            tail.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Egg egg = collision.GetComponent<Egg>();
        if (egg != null)
        {
            EatEgg(egg);
        }
        else
        {
            audioSource.PlayOneShot(dieSound);
            GameController.Instance.GameOver();
        }
    }

    private void EatEgg(Egg egg)
    {
        audioSource.PlayOneShot(eatSounds[UnityEngine.Random.Range(0, eatSounds.Length)]);
        partsToAdd += egg.GetPartsToAdd();
        GameController.Instance.EggEaten(egg);
    }
}
