using System.Collections.Generic;
using UnityEngine;

public class objectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefabObject;
    [SerializeField] private int objectsNumberOnStart;

    private List<GameObject> objectsPool = new List<GameObject>();

    private void Start()
    {
        CreateObject(objectsNumberOnStart);
    }

    private void CreateObject(int numberOfObjects)
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            CreateNewObject();
        }
    }


    /// <summary>
    /// instantiate new object and add to list
    /// </summary>
    /// <returns></returns>
    private GameObject CreateNewObject()
    {
        //instantiate
        GameObject newObject = Instantiate(prefabObject);
        //deactivate
        newObject.SetActive(false);
        //add to list
        objectsPool.Add(newObject);

        return null;
    }


    /// <summary>
    /// Take from the list an abaliable object
    /// if not exist create one and active the object
    /// </summary>
    /// <returns></returns>
    public GameObject GetGameObject() 
    {
        //find in the objectsPool an object that is inactive in the game herarchy
        GameObject theObject = objectsPool.Find(x => x.activeInHierarchy == false);

        //if not exist, create one
        if(theObject == null)
        {
            theObject = CreateNewObject();
        }

        theObject.SetActive(true);

        return theObject;
    }
}
