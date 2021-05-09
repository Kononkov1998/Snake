using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField]
    private GameObject spikePrefab = null;

    [SerializeField]
    private int spikesCount = 5;
    private List<GameObject> spikes = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < spikesCount; i++)
        {
            InstantiateSpike();
        }
    }

    private GameObject InstantiateSpike()
    {
        var spike = Instantiate(spikePrefab, Vector3.zero, Quaternion.identity, transform);
        spike.SetActive(false);
        spikes.Add(spike);

        return spike;
    }

    private GameObject GetSpike()
    {
        foreach (var spike in spikes)
        {
            if (!spike.activeInHierarchy)
            {
                return spike;
            }
        }

        return InstantiateSpike();
    }

    public void CreateSpike()
    {
        Vector3 position = GameController.Instance.GetRandomPositionOnScreen();
        GameObject spike = GetSpike();

        spike.transform.position = position;
        spike.SetActive(true);
    }

    public void RemoveSpikes()
    {
        foreach (var spike in spikes)
        {
            if (spike.activeInHierarchy)
            {
                spike.SetActive(false);
            }
        }
    }
}
