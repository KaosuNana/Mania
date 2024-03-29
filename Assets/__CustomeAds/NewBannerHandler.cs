using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBannerHandler : MonoBehaviour
{
    public bool bannerBottomLeft, bannerBottomRight, bannerTopLeft, bannerTopRight, BannerBox;
    public int bannerBoxPos;
    // Start is called before the first frame update
    void OnEnable()
    {
        if (bannerBottomLeft)
            admanager.instance.showbannerbottomLeft();
        if (bannerBottomRight)
            admanager.instance.showbannerbottomRight();
        if (bannerTopLeft)
            admanager.instance.showbannerTopLeft();
        if (bannerTopRight)
            admanager.instance.showbannerTopRight();
        if (BannerBox)
        {
            admanager.instance.showBoxBanner(bannerBoxPos);
        }
    }
    private void OnDisable()
    {
        if (bannerBottomLeft)
            admanager.instance.hideBottomLeftBanner();
        if (bannerBottomRight)
            admanager.instance.hideBottomRightBanner();
        if (bannerTopLeft)
            admanager.instance.hideTopLeftBanner();
        if (bannerTopRight)
            admanager.instance.hideTopRightBanner();
        if (BannerBox)
        {
            admanager.instance.hideBoxBanner();
        }
    }

}
