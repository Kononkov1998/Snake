using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField]
    private const int PARTSREMEMBERED = 10;
    public Vector3[] previousPositions = new Vector3[PARTSREMEMBERED];
    public int setIndex = 0;
    public int getIndex = -(PARTSREMEMBERED - 1);

    [HideInInspector]
    public BodyPart following = null;

    private Vector2 deltaPosition;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite tailSprite = null;
    [SerializeField]
    private Sprite bodyPartSprite = null;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnDisable()
    {
        ClearPreviousPositions();
    }

    protected void ClearPreviousPositions()
    {
        setIndex = 0;
        getIndex = -(PARTSREMEMBERED - 1);
    }

    protected virtual void Update()
    {
        if (!GameController.Instance.isPlaying) return;

        Vector3 followPosition = Vector3.zero;
        if (following != null)
        {
            if (following.getIndex > -1)
            {
                followPosition = following.previousPositions[following.getIndex];
            }
            else
            {
                followPosition = following.transform.position;
            }
        }


        previousPositions[setIndex].x = gameObject.transform.position.x;
        previousPositions[setIndex].y = gameObject.transform.position.y;
        previousPositions[setIndex].z = gameObject.transform.position.z;

        setIndex++;
        if (setIndex >= PARTSREMEMBERED)
        {
            setIndex = 0;
        }
        getIndex++;
        if (getIndex >= PARTSREMEMBERED)
        {
            getIndex = 0;
        }

        if (following != null)
        {
            Vector3 newPosition;
            newPosition = followPosition;
            newPosition.z += 0.01f;

            SetMovement(newPosition - gameObject.transform.position);
            UpdateDirection();
            UpdatePosition();
        }
    }

    protected void SetMovement(Vector2 movement)
    {
        deltaPosition = movement;
    }

    protected void UpdatePosition()
    {
        gameObject.transform.position += (Vector3)deltaPosition;
    }

    protected void UpdateDirection()
    {
        if (deltaPosition.y > 0)
        {
            gameObject.transform.localEulerAngles = Vector3.zero;
        }
        else if (deltaPosition.y < 0)
        {
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);
        }
        else if (deltaPosition.x > 0)
        {
            gameObject.transform.localEulerAngles = new Vector3(0, 0, -90);
        }
        else if (deltaPosition.x < 0)
        {
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
    }

    internal void TurnIntoTail()
    {
        spriteRenderer.sprite = tailSprite;
    }

    internal void TurnBodyPartTail()
    {
        spriteRenderer.sprite = bodyPartSprite;
    }
}
