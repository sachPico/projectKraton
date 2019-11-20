using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUIHandler : MonoBehaviour
{
    public List<ImageList> scoreUIList;
    public List<SpriteList> scoreImageList;
    public List<GameObject> scoreImages;
    public List<AnchorList> UIAnchors;

    [System.Serializable]
    public class ImageList
    {
        public List<Image> UIImageList;
    }

    [System.Serializable]
    public class SpriteList
    {
        public List<Sprite> UISpriteList;
    }

    [System.Serializable]
    public class AnchorList
    {
        public Vector2 anchorMax;
        public Vector2 anchorMin;
    }

    //THIS IS FOR MULTI SCORE UI HANDLER
    public Dictionary<int, List<Image>> scoreUIDictionary;

    public static uint score;
    public float anchorDist;

    int digitcount;

    Vector2 bakeScoreUIPos;
    GameObject bakeNewScoreUI;
    
    public static ScoreUIHandler scoreHandler;

    void SetScoreImages()
    {
        for(int i = 0; i<scoreUIList.Count;i++) //ITERATES BASED ON THE NUMBER OF INDEX
        {
            for(int j = 0; j<scoreImages[i].transform.childCount;j++)
            {
                scoreUIList[i].UIImageList.Add(scoreImages[i].transform.GetChild(j).GetComponent<Image>());
            }
        }
    }
    public void UpdateScore(Value _value)
    {
        int bakeScore1;
        int bakeScore2;
        bakeScore1 = (int)_value.value;
        digitcount = ((int) Mathf.Log10(_value.value))+1;
        if(digitcount!=scoreImages[_value.indexID].transform.childCount)
        {
            /*bakeNewScoreUI = Instantiate(scoreImages[_value.indexID].transform.GetChild(0).gameObject,scoreImages[_value.indexID].transform);
            bakeNewScoreUI.transform.SetSiblingIndex(digitcount-1);
            bakeScoreUIPos = bakeNewScoreUI.GetComponent<RectTransform>().anchorMax;
            bakeScoreUIPos.x -= anchorDist*(digitcount-1);
            bakeNewScoreUI.GetComponent<RectTransform>().anchorMax=bakeScoreUIPos;
            bakeScoreUIPos = bakeNewScoreUI.GetComponent<RectTransform>().anchorMin;
            bakeScoreUIPos.x -= anchorDist*(digitcount-1);
            bakeNewScoreUI.GetComponent<RectTransform>().anchorMin=bakeScoreUIPos;
            scoreUIList[_value.indexID].UIImageList.Add(bakeNewScoreUI.GetComponent<Image>());*/
            int oldChildCount = scoreImages[_value.indexID].transform.childCount;
            int limit = digitcount-scoreImages[_value.indexID].transform.childCount;
            for(int i=limit; i>0; i--)
            {
                bakeNewScoreUI = Instantiate(scoreImages[_value.indexID].transform.GetChild(0).gameObject,scoreImages[_value.indexID].transform);
                bakeNewScoreUI.transform.SetSiblingIndex(digitcount-i);
                bakeScoreUIPos = bakeNewScoreUI.GetComponent<RectTransform>().anchorMax;
                bakeScoreUIPos = UIAnchors[_value.indexID].anchorMax;
                bakeScoreUIPos.x -= anchorDist*(digitcount-i);
                Debug.Log(anchorDist*(digitcount-i));
                bakeNewScoreUI.GetComponent<RectTransform>().anchorMax=bakeScoreUIPos;
                
                bakeScoreUIPos = bakeNewScoreUI.GetComponent<RectTransform>().anchorMin;
                bakeScoreUIPos = UIAnchors[_value.indexID].anchorMin;
                bakeScoreUIPos.x -= anchorDist*(digitcount-i);
                Debug.Log(anchorDist);//*(digitcount-i));
                bakeNewScoreUI.GetComponent<RectTransform>().anchorMin=bakeScoreUIPos;

                scoreUIList[_value.indexID].UIImageList.Add(bakeNewScoreUI.GetComponent<Image>());
            }
        }
        for(int i=digitcount-1; i>=0; i--)
        {
            bakeScore2 = (int)(bakeScore1/(Mathf.Pow(10,i)));
            bakeScore1 %= (int)Mathf.Pow(10,i);
            scoreUIList[_value.indexID].UIImageList[i].sprite = scoreImageList[_value.indexID].UISpriteList[bakeScore2];
        }
    }

    void Start()
    {
        //SETTING UI SCORE
        SetScoreImages();
        //Debug.Log(scoreImages.Count);
        //Debug.Break();

        //CEK JARAK ANCHOR UI SKOR DARI INDEX PERTAMA LIST LIST UI SKOR
        //scoreUIList[i].UIImageList[0].GetComponent<RectTransform>().anchorMax.x-scoreUIList[i].UIImageList[0].GetComponent<RectTransform>().anchorMin.x;


        scoreHandler = this;
    }
}