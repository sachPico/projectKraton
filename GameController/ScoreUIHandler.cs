using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUIHandler:MonoBehaviour
{
    
    public List<Image> scoreUIList;
    public List<Sprite> scoreImageList;
    public GameObject scoreImages;
    public static uint score;
    public float anchorDist;
    
    int digitcount;

    Vector2 bakeScoreUIPos;
    GameObject bakeNewScoreUI;

    void SetScoreImages()
    {
        for(int i = 0; i<scoreImages.transform.childCount;i++)
        {
            scoreUIList.Add(scoreImages.transform.GetChild(i).GetComponent<Image>());
        }
    }
    void UpdateScore()
    {
        int bakeScore1;
        int bakeScore2;
        bakeScore1 = (int)score;
        digitcount = ((int) Mathf.Log10(score))+1;
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
            //Debug.Log(digitcount);
            scoreUIList.Add(bakeNewScoreUI.GetComponent<Image>());
        }
        for(int i=digitcount-1; i>=0; i--)
        {
            bakeScore2 = (int)(bakeScore1/(Mathf.Pow(10,i)));
            bakeScore1 %= (int)Mathf.Pow(10,i);
            scoreUIList[i].sprite = scoreImageList[bakeScore2];
        }
    }
    
    public void AddScore(int sc)
    {
        score += (uint)sc;
        UpdateScore();
    }

    void Start()
    {
        //SETTING UI SCORE
        SetScoreImages();

        //CEK JARAK ANCHOR UI SKOR
        anchorDist = scoreUIList[0].GetComponent<RectTransform>().anchorMax.x-scoreUIList[0].GetComponent<RectTransform>().anchorMin.x;
    }
}