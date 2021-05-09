using UnityEngine;

public class Walls : MonoBehaviour
{
    [SerializeField]
    private GameObject rockPrefab = null;

    public void CreateWalls()
    {
        Vector2 bounds = GameController.Instance.GetScreenBounds();
        float width = bounds.x;
        float height = bounds.y;

        CreateWall(new Vector3(-width, -height), new Vector3(-width, height)); // left wall
        CreateWall(new Vector3(-width, height), new Vector3(width, height)); // top wall
        CreateWall(new Vector3(width, height), new Vector3(width, -height)); // right wall
        CreateWall(new Vector3(width, -height), new Vector3(-width, -height)); // bottom wall
    }

    private void CreateWall(Vector3 startPosition, Vector3 endPosition)
    {
        float distance = Vector3.Distance(endPosition, startPosition);
        int numberOfRocks = (int)(distance * 3);//3);
        Vector3 delta = (endPosition - startPosition) / numberOfRocks;

        Vector3 position = startPosition;
        for (int i = 0; i < numberOfRocks; i++)
        {
            float rotation = Random.Range(0f, 360f);
            float scale = Random.Range(1f, 2f);
            CreateRock(position, scale, rotation);
            position += delta;
        }
    }

    private void CreateRock(Vector3 position, float scale, float rotation)
    {
        GameObject rock = Instantiate(rockPrefab, position, Quaternion.Euler(0, 0, rotation), transform);
        rock.transform.localScale = new Vector3(scale, scale, 1);
    }
}
