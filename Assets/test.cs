using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class test : MonoBehaviour
{

    [SerializeField] GameObject test1;
    [SerializeField] Tilemap podloga;
    Grid grid;
    public List<PathNode> pathnodelist = new List<PathNode>();

    BoundsInt boundsInt;
    // Start is called before the first frame update
    void Start()
    {
   //jesli tamto nie zadzaiala sporbuj tego
        boundsInt = podloga.cellBounds;

        for(int x = boundsInt.min.x; x < boundsInt.max.x; x++)
        {
            for(int y = boundsInt.min.y; y < boundsInt.max.y; y++)
            {
                Vector3Int testowapozycja = new Vector3Int(x, y, 0);
                if (podloga.HasTile(testowapozycja))
                {
                    Vector3 ok = podloga.GetCellCenterWorld(testowapozycja);
                    // Instantiate(test1, ok, Quaternion.identity);

                    Debug.Log(ok);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
