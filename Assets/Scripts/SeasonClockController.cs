using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonClockController : MonoBehaviour
{
    public float seasonProgress = 0;
    public int seasonId = 0;

    public GameObject seasonClockObject;
    private RectTransform seasonClockRectTransform;

    private const int numberOfSeasons = 4;

    // Start is called before the first frame update
    void Start()
    {
        seasonClockRectTransform = seasonClockObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float yearProgress = seasonProgress / numberOfSeasons;
        int activeSeasonAngleModifier = 360 / numberOfSeasons * seasonId;
        seasonClockRectTransform.rotation = Quaternion.Euler(0, 0, yearProgress / 100 * 360 + activeSeasonAngleModifier);
    }
}
