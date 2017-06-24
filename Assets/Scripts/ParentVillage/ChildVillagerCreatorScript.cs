using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildVillagerCreatorScript : MonoBehaviour {

    public GameObject childVillager;

    public void CreateChildVillager(Vector3 destination)
    {
        CreateChildVillager(transform.position, destination);
    }

    public void CreateChildVillager(Vector3 source, Vector3 destination)
    {
        if ((destination - source).sqrMagnitude > ChildVillagerScript.Threshold)
        {
            GameObject childVillagerInstance = Instantiate(childVillager);
            childVillagerInstance.transform.position = source;
            childVillagerInstance.GetComponent<ChildVillagerScript>().Destination = destination;
        }
    }
}
