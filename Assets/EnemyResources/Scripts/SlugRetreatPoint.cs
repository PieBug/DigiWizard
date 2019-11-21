using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugRetreatPoint : MonoBehaviour
{
    private static Dictionary<int, List<GameObject>> retreatPoints;

    public int area;
    private int id;
    public static List<GameObject> GetRetreatPointsInArea(int area)
    {
        return retreatPoints[area];
    }
    // Start is called before the first frame update
    void Awake()
    {
        if(retreatPoints == null)
        {
            retreatPoints = new Dictionary<int, List<GameObject>>();
        }
        if (!retreatPoints.ContainsKey(area))
        {
            retreatPoints.Add(area, new List<GameObject>());
        }
    }

    private void Start()
    {
        
        id = retreatPoints[area].Count;
        retreatPoints[area].Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
