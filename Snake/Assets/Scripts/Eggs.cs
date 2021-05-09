using System.Collections.Generic;
using UnityEngine;

public class Eggs : MonoBehaviour
{
    [SerializeField]
    private GameObject eggPrefab = null;
    [SerializeField]
    private EggData eggData = null;
    [SerializeField]
    private EggData goldenEggData = null;

    [SerializeField]
    private int eggsCount = 2;
    private List<Egg> eggs = new List<Egg>();

    private void Awake()
    {
        for (int i = 0; i < eggsCount; i++)
        {
            InstantiateEgg();
        }
    }

    private Egg InstantiateEgg()
    {
        var egg = Instantiate(eggPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<Egg>();
        egg.gameObject.SetActive(false);
        eggs.Add(egg);

        return egg;
    }

    private Egg GetEgg()
    {
        foreach (var egg in eggs)
        {
            if (!egg.gameObject.activeInHierarchy)
            {
                return egg;
            }
        }

        return InstantiateEgg();
    }

    public void CreateEgg(bool golden = false)
    {
        Vector3 position = GameController.Instance.GetRandomPositionOnScreen();

        EggData data = golden ? goldenEggData : eggData;
        Egg egg = GetEgg();

        egg.data = data;
        egg.gameObject.transform.position = position;
        egg.gameObject.SetActive(true);
    }

    public void RemoveEggs()
    {
        foreach (var egg in eggs)
        {
            if (egg.gameObject.activeInHierarchy)
            {
                egg.gameObject.SetActive(false);
            }
        }
    }
}
