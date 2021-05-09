using UnityEngine;

public class Egg : MonoBehaviour
{
    public EggData data;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnEnable()
    {
        if (data)
        {
            spriteRenderer.sprite = data.sprite;
        }
    }

    public int GetPartsToAdd()
    {
        return data.partsToAdd;
    }
}
