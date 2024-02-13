using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class resourceManager : MonoBehaviour {

    [Header("Gold Variables")]
    public static int goldAmount;
    public int goldIncreasePerSec;
    public Text goldUI;
    public bool autoGenGold;
    
    [Header("Orb Variables")]
    public static int orbAmount;
    public Text orbUI;

    [Header("Structure Variables")]
    public GameObject signPost;
    public GameObject[] structureSlots;
    public static Dictionary<object, object> structureDict = new Dictionary<object, object>();
    private GameObject availSlot;
    private int structureNameIndex;
    private int signPostNameIndex; 
    private object locationKey;

    private void Awake()
    {
        if(structureSlots.Length > 0)
        {
            structureNameIndex = 0;
            signPostNameIndex = 0;

            for (int i = 0; i < structureSlots.Length; i++)
            {
                var newSignPost = Instantiate(signPost, structureSlots[i].transform.position, structureSlots[i].transform.rotation);
                newSignPost.name = "SignPost" + signPostNameIndex;
                signPostNameIndex++;
                structureDict.Add(structureSlots[i], newSignPost);
            }
        }
        
    }
    // Use this for initialization
    void Start ()
    {
        
        if (autoGenGold)
        {
            IncrementGold();
        }
	}
    private void FixedUpdate()
    {
        goldUI.text = goldAmount.ToString();
        orbUI.text = orbAmount.ToString();
    }

    public void reduceGoldTotal(int cost)
    {
        goldAmount -= cost;
        
    }

    public void reduceOrbTotal(int cost)
    {
        orbAmount -= cost;
        
    }

    public void IncrementGold()
    {
        goldAmount += goldIncreasePerSec;
        
        StartCoroutine("GenerateGold");
    }

    private GameObject GetAvailSlot()
    {
        foreach (KeyValuePair<object,object> structureSlot in structureDict)
        {
            GameObject keyValue = structureSlot.Value as GameObject;
            if (keyValue.name.Contains("SignPost"))
            {
                print("Found a slot " + structureSlot.Key);
                return structureSlot.Key as GameObject;
                
            }
        }
        return null;
    }

    public void PurchaseStructure(GameObject structure, int cost)
    {
        availSlot = GetAvailSlot();
        if (availSlot != null)
        {
            GameObject signPostToDestroy = structureDict[availSlot] as GameObject;
            Debug.Log("Sign post name: " + signPostToDestroy.name);
            Destroy(signPostToDestroy);
            
            var newStructure = Instantiate(structure, availSlot.transform.position, availSlot.transform.rotation);
            if (structure.name == "Gold Mine")
            {
                newStructure.name = "Gold Mine " + structureNameIndex;
                Debug.Log("New name is now: " + newStructure.name);
                goldAmount -= cost;
            }
            else if(structure.name == "Energy Orb Mine")
            {
                newStructure.name = "Energy Orb Mine " + structureNameIndex;
                goldAmount -= cost;
            }
            structureNameIndex++;
            structureDict[availSlot] = newStructure;
        }
    }

    public void RemoveStructure(GameObject structure)
    {
        
        foreach (KeyValuePair<object, object> structureSlot in structureDict)
        {
            
            if (structureSlot.Value == (object)structure)
            {
                locationKey = structureSlot.Key;
                break;
                
            }
            else
            {
                print("Not found yet");
            }
        }
        if(locationKey != null)
        {
            Destroy(structure);
            GameObject structSlot = locationKey as GameObject;
            var newSignPost = Instantiate(signPost, structSlot.transform.position, structSlot.transform.rotation);
            newSignPost.name = "SignPost" + signPostNameIndex;
            signPostNameIndex++;
            structureDict[locationKey] = newSignPost;
        }
    }

    IEnumerator GenerateGold()
    {
        yield return new WaitForSeconds(1);
        IncrementGold();
    }
}
