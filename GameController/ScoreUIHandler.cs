using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUIHandler : MonoBehaviour
{
    //THIS IS FOR A SINGLE UI HANDLER
    public List<Image> scoreUIList;
    public List<Sprite> scoreImageList;
    public GameObject scoreImages;

    //THIS IS FOR A MULTI UI HANDLER
    public Dictionary<string, List<Image>> scoreUIDictionary;
    public Dictionary<string, List<Image>> scoreUIImageDictionary;
    public Dictionary<string, GameObject> scoreUIParents;

    public static uint score;
    public float anchorDist;
    
    int digitcount;

    Vector2 bakeScoreUIPos;
    GameObject bakeNewScoreUI;
    
    public static ScoreUIHandler scoreHandler;

    void SetScoreImages()
    {
        for(int i = 0; i<scoreImages.transform.childCount;i++)
        {
            scoreUIList.Add(scoreImages.transform.GetChild(i).GetComponent<Image>());
        }
    }
    public void UpdateScore(Value _value)
    {
        int bakeScore1;
        int bakeScore2;
        bakeScore1 = (int)_value.value;
        digitcount = ((int) Mathf.Log10(_value.value))+1;
        if(digitcount!=scoreImages.transform.childCount)
        {
            bakeNewScoreUI = Instantiate(scoreImages.transform.GetChild(0).gameObject,scoreImages.transform);
            bakeNewScoreUI.transform.SetSiblingIndex(digitcount-1);
            bakeScoreUIPos = bakeNewScoreUI.GetComponent<RectTransform>().anchorMax;
            bakeScoreUIPos.x -= anchorDist*(digitcount-1);
            bakeNewScoreUI.GetComponent<RectTransform>().anchorMax=bakeScoreUIPos;
            bakeScoreUIPos = bakeNewScoreUI.GetComponent<RectTransform>().anchorMin;
            bakeScoreUIPos.x -= anchorDist*(digitcount-1);
            bakeNewScoreUI.GetComponent<RectTransform>().anchorMin=bakeScoreUIPos;
            scoreUIList.Add(bakeNewScoreUI.GetComponent<Image>());
        }
        for(int i=digitcount-1; i>=0; i--)
        {
            bakeScore2 = (int)(bakeScore1/(Mathf.Pow(10,i)));
            bakeScore1 %= (int)Mathf.Pow(10,i);
            scoreUIList[i].sprite = scoreImageList[bakeScore2];
        }
    }

    void Start()
    {
        //SETTING UI SCORE
        SetScoreImages();

        //CEK JARAK ANCHOR UI SKOR
        anchorDist = scoreUIList[0].GetComponent<RectTransform>().anchorMax.x-scoreUIList[0].GetComponent<RectTransform>().anchorMin.x;

        scoreHandler = this;
    }
}