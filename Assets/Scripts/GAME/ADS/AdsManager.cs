//using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AdsManager : MonoBehaviour
{

    public GameController gameController;
    public enum Earned
    {
        gold,
        artifacte,
        resurrection,
        sword
    }
    int idEarned;
    public GameObject pauseObject;
    public Transform adsInfo;
    int amountEarned;
    Image imageInfo;
    GameObject buttonAds;
    GameObject value;
    GameObject close;
    GameObject title;
    int itemIndex;
    public InfoPanel infoPanel;
    public ArtifactesInfo artifactesInfo;
    public GameObject menuUI;
    public Sprite treasure;
    bool isEarned;
    //public GameManager gameManager;

    Sprite[] bonusAbility;
    public UpdateCharacteristicsInfo abilityObject;

    void Awake()
    {
        gameController = GameObject.Find("GameManager").GetComponent<GameController>();
        adsInfo.parent.localPosition = new Vector3(0, 0, 0);
        imageInfo = adsInfo.GetChild(1).GetChild(0).GetComponent<Image>();
        buttonAds = adsInfo.GetChild(2).gameObject;
        value = adsInfo.GetChild(3).gameObject;
        close = adsInfo.GetChild(4).gameObject;
        title = adsInfo.GetChild(5).gameObject;
        adsInfo.gameObject.SetActive(false);
        bonusAbility = abilityObject.characteristicSprite;
    }


    public void ShowInterAd()
    {
        admanager.instance.ShowGenericVideoAd();
      //  AdManager.instance.ShowAd();
    }
    //sameerad
    void EarnedResult()
    {
        if (!HeroInformation.player.settings.mute)
            GameController.audioListener.mute = false;
        switch (idEarned)
        {
            //gold
            case 0:
                HeroInformation.player.gold += amountEarned;
                HeroInformation.UpdateInformation();
                menuUI.SetActive(true);
                GetComponent<AudioManager>().ItemAudio(2);
                infoPanel.SetMenuState(4);
                break;
            //artifactes
            case 1:
                HeroInformation.player.inventoryArtifacte.Add(itemIndex);
                menuUI.SetActive(true);
                GetComponent<AudioManager>().WinAudio(1);
                infoPanel.SetMenuState(3);
                artifactesInfo.SetArtifactes();
                break;
            //resurection
            case 2:
                gameController.Resurection(true);
                GamePlayManager.canResurection = false;
                break;
            //sword
            case 3:
                ItemDataManager.itemDataBase.swords[HeroInformation.player.indicatorSwordList].avialable = true;
                HeroInformation.player.indicatorSwordList++;
                break;
            default:
                break;
        }
    }

    public void AdInfo(Earned earned, int amount)
    {
        if (!GameController.pause) gameController.Pause();
        pauseObject.SetActive(false);
        adsInfo.gameObject.SetActive(true);
        if (IapManager.CheckNoAds()) buttonAds.transform.GetChild(0).gameObject.SetActive(false);
        else buttonAds.transform.GetChild(0).gameObject.SetActive(true);
        amountEarned = amount;
        idEarned = (int)earned;
        switch (idEarned)
        {
            case 0:
                if (LocalizationManager.localizationIndex == 0)
                {
                    title.GetComponent<Text>().text = "Клад!!!";
                }
                else
                {
                    title.GetComponent<Text>().text = "Treasure!!!";
                }
                adsInfo.GetChild(6).gameObject.SetActive(false);
                imageInfo.sprite = treasure;
                value.SetActive(true);
                value.transform.GetChild(0).GetComponent<Text>().text = amount.ToString();
                break;
            case 1:
                value.SetActive(false);
                itemIndex = Random.Range(0, ItemDataManager.itemDataBase.artifactes.Count);
                imageInfo.sprite = ItemDataManager.itemDataBase.artifactes[itemIndex].icon;
                title.GetComponent<Text>().text = ItemDataManager.itemDataBase.artifactes[itemIndex].itemLocalizations[LocalizationManager.localizationIndex].name;
                Transform valueArt = adsInfo.GetChild(6);
                valueArt.gameObject.SetActive(true);
                valueArt.transform.GetChild(0).GetComponent<Image>().sprite = bonusAbility[(int)ItemDataManager.itemDataBase.artifactes[itemIndex].bonus];
                if (HeroInformation.player.characterList[(int)ItemDataManager.itemDataBase.artifactes[itemIndex].bonus].percent)
                {
                    valueArt.transform.GetChild(1).GetComponent<Text>().text = "+" + ItemDataManager.itemDataBase.artifactes[itemIndex].bonusValue.ToString() + "%";
                }
                else
                {
                    valueArt.transform.GetChild(1).GetComponent<Text>().text = "+" + ItemDataManager.itemDataBase.artifactes[itemIndex].bonusValue.ToString();
                }
                break;
            case 3:
                imageInfo.sprite = ItemDataManager.itemDataBase.swords[HeroInformation.player.indicatorSwordList].icon;
                title.GetComponent<Text>().text = ItemDataManager.itemDataBase.swords[HeroInformation.player.indicatorSwordList].itemLocalizations[LocalizationManager.localizationIndex].name;
                break;
            default:
                //imageInfo.sprite = sprite[(int)earned];
                title.GetComponent<Text>().text = "";
                break;
        }
    }
    public void ShowAd(Earned e)
    {
        idEarned = (int)e;
        if (IapManager.CheckNoAds()) EarnedResult();
        //sameerad  else rewardedAd.Show();
    }

    public void ShowAd()
    {
        if (IapManager.CheckNoAds()) EarnedResult();
        //sameeradelse rewardedAd.Show();
    }

    public void ShoeAdBonus()
    {
        idEarned = 1;
        itemIndex = Random.Range(0, ItemDataManager.itemDataBase.artifactes.Count);
        if (IapManager.CheckNoAds())
        {
            EarnedResult();
        }
        else
        {
            //sameeradrewardedAd.Show();
        }
    }
    public static bool CheckAds()
    {
       //sameerad if (rewardedAd.IsLoaded()) return true;
         return false;
    }

}
  
    //sameerad
    //public void HandleRewardedAdClosed(object sender, System.EventArgs args) {
    //    print("isEarned - " + isEarned);
    //    if (!isEarned) {
    //        if (idEarned != 2) {
    //            gameController.Pause();
    //            pauseObject.SetActive(true);
    //        }
    //    }
    //    CreateAndLoadRewardedAd();
    //}

//    public void HandleUserEarnedReward(object sender, Reward args) {
//        isEarned = true;
//        EarnedResult();
//    }
//}
