using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClipboardBook : MonoBehaviour
{
    [SerializeField] 
    private List<Image> pages;
    [SerializeField]
    private Transform spawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnFlip(int fromPageNumber, int toPageNumber)
    {

        for (int i = 0; toPageNumber - fromPageNumber > i; i++)
        {
            SpawnNextPage(i, spawnPosition);
            
        }
    }

    void SpawnNextPage(int currentPageNumber, Transform position)
    {
        
    }
}
