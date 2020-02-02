using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //Full heart sprite
    public Sprite fullHeart;
    //3/4 full heart sprite
    public Sprite almostFullHeart;
    //1/2 full heart sprite
    public Sprite halfHeart;
    //1/4 full heart sprite
    public Sprite almostDeadHeart;
    //Empty heart
    public Sprite emptyHeart;
    //Array of ui health images
    public Image[] healthImages;

    //Get hp reference
    Character play;
    [Range(0, 12)]
    public int amt = 12;

    //Weapon sprites
    public Sprite defSprite;
    public Sprite fireSprite;

    //Current weapon UI
    public Image weaponImage;

    //Get weapon reference
    [Range(0, 1)]
    public int weapon = 0;

    private void Awake()
    {
        play = GetComponent<Character>();
    }

    public void UpdateHealth()
    {
        //Health combinations
        //0-12 health
        //1st health bar - 0-4
        //2nd health bar - 5-8
        //3rd health bar - 9-12

        switch(play.getHealth())
        {
            case 0:
                //Dead
                foreach (Image hpImg in healthImages)
                {
                    hpImg.sprite = emptyHeart;
                }
                break;
            case 1:
                healthImages[0].sprite = almostDeadHeart;
                healthImages[1].sprite = emptyHeart;
                healthImages[2].sprite = emptyHeart;
                break;
            case 2:
                healthImages[0].sprite = halfHeart;
                healthImages[1].sprite = emptyHeart;
                healthImages[2].sprite = emptyHeart;
                break;
            case 3:
                healthImages[0].sprite = almostFullHeart;
                healthImages[1].sprite = emptyHeart;
                healthImages[2].sprite = emptyHeart;
                break;
            case 4:
                healthImages[0].sprite = fullHeart;
                healthImages[1].sprite = emptyHeart;
                healthImages[2].sprite = emptyHeart;
                break;
            case 5:
                healthImages[0].sprite = fullHeart;
                healthImages[1].sprite = almostDeadHeart;
                healthImages[2].sprite = emptyHeart;
                break;
            case 6:
                healthImages[0].sprite = fullHeart;
                healthImages[1].sprite = halfHeart;
                healthImages[2].sprite = emptyHeart;
                break;
            case 7:
                healthImages[0].sprite = fullHeart;
                healthImages[1].sprite = almostFullHeart;
                healthImages[2].sprite = emptyHeart;
                break;
            case 8:
                healthImages[0].sprite = fullHeart;
                healthImages[1].sprite = fullHeart;
                healthImages[2].sprite = emptyHeart;
                break;
            case 9:
                healthImages[0].sprite = fullHeart;
                healthImages[1].sprite = fullHeart;
                healthImages[2].sprite = almostDeadHeart;
                break;
            case 10:
                healthImages[0].sprite = fullHeart;
                healthImages[1].sprite = fullHeart;
                healthImages[2].sprite = halfHeart;
                break;
            case 11:
                healthImages[0].sprite = fullHeart;
                healthImages[1].sprite = fullHeart;
                healthImages[2].sprite = almostFullHeart;
                break;
            case 12:
                healthImages[0].sprite = fullHeart;
                healthImages[1].sprite = fullHeart;
                healthImages[2].sprite = fullHeart;
                break;
        }
    }

    public void UpdateWeapon()
    {
        switch(weapon)
        {
            case 0:
                weaponImage.sprite = defSprite;
                break;
            case 1:
                weaponImage.sprite = fireSprite;
                break;
        }
    }

    private void Update()
    {
        UpdateHealth();
        UpdateWeapon();
    }
}
