using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdsInfo : MonoBehaviour{
    public GameObject pauseObject;
    public Sprite[] sprite;
    [HideInInspector]public Transform adsInfo;
    [HideInInspector]public Transform adsResult;

    void Start(){
        adsInfo = transform.GetChild(0);
        adsInfo.gameObject.SetActive(false);
        adsResult = transform.GetChild(1);
        adsResult.gameObject.SetActive(false);
    }
    public void SetInfo() {
    }

}
