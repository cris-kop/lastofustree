using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressController : MonoBehaviour
{
    public int progress;
    public GameObject progressObject;
    public GameObject textObject;

    public Color lowColor = Color.red;
    public Color mediumColor = Color.yellow;
    public Color highColor = Color.green;

    private int startWidth = 0;
    private RectTransform rectTransform;
    private RawImage image;
    private TextMeshProUGUI textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        //progressObject = GameObject.Find("Progress Value");
        //textObject = GameObject.Find("Text");

        rectTransform = progressObject.GetComponentInChildren<RectTransform>();
        image = progressObject.GetComponent<RawImage>();
        startWidth = (int)rectTransform.rect.width;

        textMeshPro = textObject.GetComponent<TextMeshProUGUI>();

        Debug.Log(textObject);
    }

    // Update is called once per frame
    void Update()
    {
        float minMaxedProgres = Mathf.Clamp(progress, 0, 100);

        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, minMaxedProgres / 100 * startWidth);

        if (progress < 50)
        {
            image.color = Color.Lerp(lowColor, mediumColor, minMaxedProgres / 100f * 2f);
        }
        else
        {
            image.color = Color.Lerp(mediumColor, highColor, (minMaxedProgres - 50f) / 100f * 2f);
        }

        textMeshPro.text = $"{Mathf.RoundToInt(minMaxedProgres)}%";
    }
}
