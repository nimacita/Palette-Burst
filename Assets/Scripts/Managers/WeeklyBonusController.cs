using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class WeeklyBonusController : MonoBehaviour
{

    private DateTime currentTime;

    [SerializeField]
    private MyMainMenu mMainMenu;

    [SerializeField]
    private int lastClaimDay;

    [SerializeField]
    private bool canClaim;
    [Space]
    [SerializeField]
    private TextMeshProUGUI timeToNextReward;
    [SerializeField]
    private TextMeshProUGUI currentClaimDay;


    public enum WeeklyBonusItem
    {
        CoinBonus = 0,
        KeyBonus = 1,
        MoneyBonus = 2,
    }
    [Space]
    [Header("Weekly Bonus")]
    [SerializeField]
    private int[] weeklyBonusValue;
    [SerializeField]
    private WeeklyBonusItem[] weeklyBonusItems;
 
    [Space]
    [Header("Streak")]
    [SerializeField]
    private int maxStreak;
    [SerializeField]
    private int currentStreak;
    [SerializeField]
    private int currentBonus;

    [Space]
    [Header("View")]
    [SerializeField]
    private GameObject anyWeeklyBonusView;
    [SerializeField]
    private GameObject claimBonusView;
    [SerializeField]
    private TextMeshProUGUI claimBonusValue;
    [SerializeField]
    private GameObject claimBonusCoin, claimBonusKey, claimBonusMoney;
    public Button backBtn;
    public Button claimBtn;

    [Space]
    [Header("WeklyIcons")]
    [SerializeField]
    private GameObject[] weeklyIcons;
    private TextMeshProUGUI[] weeklyIconsValueTxt;
    private TextMeshProUGUI[] weeklyIconsDayTxt;
    private GameObject[] weeklyIconsLockedImage;
    private GameObject[] weeklyIconsOpenedImage;
    private GameObject[] weeklyIconsBonusImgs;
    private GameObject[] weeklyIconsCoinBonusImgs, weeklyIconsKeyBonusImgs, weeklyIconsMoneyBonusImgs;

    [Space]
    [Header("Sprites")]
    [SerializeField]
    private Sprite CoinBonusSprite;
    [SerializeField]
    private Sprite KeyBonusSprite;
    [SerializeField]
    private Sprite MoneyBonusSprite;

    [Space]
    [Header("Debug")]
    [SerializeField]
    private int addDays = 0;

    void Start()
    {
        //LastClaim = new DateTime();
        InitializedWeeklyIconsValueTxt();
        ButtonSettings();
        UpdateTime();
        CanClaimRewardUpdate();
    }

    private void ButtonSettings()
    {
        backBtn.onClick.AddListener(BackToMenu);
        claimBtn.onClick.AddListener(ClaimReward);
    }

    //последнйи забор наград
    public DateTime LastClaim
    {
        
        get
        {
            DateTime dateTime = new DateTime();
            if (!PlayerPrefs.HasKey("LastClaim"))
            {
                return dateTime;
            }
            else
            {
                return DateTime.Parse(PlayerPrefs.GetString("LastClaim"));
            }
        }
        set
        {
            PlayerPrefs.SetString("LastClaim", value.ToString());
        }
    }

    //сохраненный стрик подярд
    private int Streak
    {
        get
        {
            if (!PlayerPrefs.HasKey("Streak"))
            {
                PlayerPrefs.SetInt("Streak", 1);
            }
            else
            {
                if (PlayerPrefs.GetInt("Streak") < 1 || PlayerPrefs.GetInt("Streak") > maxStreak) 
                {
                    PlayerPrefs.SetInt("Streak", 1);
                }
            }
            return PlayerPrefs.GetInt("Streak");
        }

        set
        {
            if (value >= 1 )
            {
                if (value > maxStreak) 
                {
                    PlayerPrefs.SetInt("Streak", 1);
                }
                else
                {
                    PlayerPrefs.SetInt("Streak", value);
                }
            }
        }
    }

    //сохраненное значение ключей
    private int KeyReward
    {
        get
        {
            if (PlayerPrefs.HasKey("key"))
            {
                return PlayerPrefs.GetInt("key");
            }
            else
            {
                PlayerPrefs.SetInt("key", 0);
                return 0;
            }
        }
        set
        {
            PlayerPrefs.SetInt("key", value);
        }
    }

    //сохраненное значение монет
    private int Coins
    {
        get
        {
            if (PlayerPrefs.HasKey("Coins"))
            {
                return PlayerPrefs.GetInt("Coins");
            }
            else
            {
                PlayerPrefs.SetInt("Coins", 0);
                return 0;
            }
        }
        set
        {
            PlayerPrefs.SetInt("Coins", value);
        }
    }

    //сохраненное значение монет
    private int Money
    {
        get
        {
            if (PlayerPrefs.HasKey("Money"))
            {
                return PlayerPrefs.GetInt("Money");
            }
            else
            {
                PlayerPrefs.SetInt("Money", 0);
                return 0;
            }
        }
        set
        {
            PlayerPrefs.SetInt("Money", value);
        }
    }

    private void FixedUpdate()
    {
        currentStreak = Streak;
        currentBonus = weeklyBonusValue[Streak - 1];

        UpdateTime();
    }

    //обновляем проверку
    private void UpdateTime()
    {
        currentTime = DateTime.Now.AddDays(addDays);
        CanClaimRewardUpdate();
    }

    //можем ли забрать награду
   private void CanClaimRewardUpdate()
    {

        if (LastClaim.Year == currentTime.Year && LastClaim.Month == currentTime.Month)
        {
            if (LastClaim.AddDays(1).Day <= currentTime.Day 
                && LastClaim.AddDays(1).Month == currentTime.Month)
            {
                canClaim = true;

                if (LastClaim.AddDays(1).Day != currentTime.Day 
                    && LastClaim.AddDays(1).Month == currentTime.Month)
                {
                    Streak = 1;
                }
    
            }
            else
            {
                canClaim = false;
            }
        }
        else
        {
            canClaim = true;
            if (LastClaim.AddDays(1).Day != currentTime.Day)
            {
                Streak = 1;
            }
        }

        ViewControllerUpdate();
    }

    //обновляем экраны
    private void ViewControllerUpdate()
    {
        anyWeeklyBonusView.SetActive(false);
        claimBonusView.SetActive(false);

       
        if (canClaim)
        {//вью сбора бонуса
            ClaimBonusViewOpen();
        }
        else
        {//вью недельных бонусов
            claimBonusView.SetActive(false);
            anyWeeklyBonusView.SetActive(true);
            DefindWeeklyIconsLocked();
        }
    }

    //отображение экрана сбора награды
    private void ClaimBonusViewOpen()
    {
        claimBonusCoin.SetActive(false);
        claimBonusMoney.SetActive(false);
        claimBonusKey.SetActive(false);
        if (weeklyBonusItems[Streak-1] == WeeklyBonusItem.CoinBonus)
        {
            claimBonusCoin.GetComponent<Image>().sprite = CoinBonusSprite;
            claimBonusCoin.SetActive(true);
            claimBonusValue.text = "X" + weeklyBonusValue[Streak - 1].ToString();
        }
        else if (weeklyBonusItems[Streak - 1] == WeeklyBonusItem.KeyBonus)
        {
            claimBonusKey.GetComponent<Image>().sprite = KeyBonusSprite;
            claimBonusKey.SetActive(true);
            claimBonusValue.text = "X" + weeklyBonusValue[Streak - 1].ToString();
        }
        else
        {
            claimBonusMoney.GetComponent<Image>().sprite = MoneyBonusSprite;
            claimBonusMoney.SetActive(true);
            claimBonusValue.text = "X" + weeklyBonusValue[Streak - 1].ToString();
        }
        currentClaimDay.text = $"DAY {currentStreak}";
        claimBonusView.SetActive(true);
        anyWeeklyBonusView.SetActive(false);

    }

    //задаем отоброжение компонентов
    private void InitializedWeeklyIconsValueTxt()
    {
        weeklyIconsValueTxt = new TextMeshProUGUI[weeklyIcons.Length];
        weeklyIconsOpenedImage = new GameObject[weeklyIcons.Length];
        weeklyIconsLockedImage = new GameObject[weeklyIcons.Length];
        weeklyIconsDayTxt = new TextMeshProUGUI[weeklyIcons.Length];
        weeklyIconsBonusImgs = new GameObject[weeklyIcons.Length];
        weeklyIconsCoinBonusImgs = new GameObject[weeklyIcons.Length];
        weeklyIconsKeyBonusImgs = new GameObject[weeklyIcons.Length];
        weeklyIconsMoneyBonusImgs = new GameObject[weeklyIcons.Length];

        //инициализация и определение внешнего вида всех эллементов ежедневного бонуса
        if (weeklyIcons.Length > 0)
        {
            for (int i = 0; i < weeklyIcons.Length; i++)
            {
                weeklyIconsOpenedImage[i] = weeklyIcons[i].transform.GetChild(1).gameObject;
                weeklyIconsValueTxt[i] = weeklyIconsOpenedImage[i].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
                weeklyIconsBonusImgs[i] = weeklyIconsOpenedImage[i].transform.GetChild(0).gameObject;

                weeklyIconsCoinBonusImgs[i] = weeklyIconsBonusImgs[i].transform.GetChild(0).gameObject;
                weeklyIconsCoinBonusImgs[i].GetComponent<Image>().sprite = CoinBonusSprite;
                weeklyIconsCoinBonusImgs[i].SetActive(false);

                weeklyIconsKeyBonusImgs[i] = weeklyIconsBonusImgs[i].transform.GetChild(1).gameObject;
                weeklyIconsKeyBonusImgs[i].GetComponent<Image>().sprite = KeyBonusSprite;
                weeklyIconsKeyBonusImgs[i].SetActive(false);

                weeklyIconsMoneyBonusImgs[i] = weeklyIconsBonusImgs[i].transform.GetChild(2).gameObject;
                weeklyIconsMoneyBonusImgs[i].GetComponent<Image>().sprite = MoneyBonusSprite;
                weeklyIconsMoneyBonusImgs[i].SetActive(false);


                weeklyIconsDayTxt[i] = weeklyIcons[i].transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

                weeklyIconsLockedImage[i] = weeklyIcons[i].transform.GetChild(2).gameObject;

                weeklyIconsDayTxt[i].text = $"DAY{i + 1}";

                if (weeklyBonusItems[i] == WeeklyBonusItem.CoinBonus)
                {
                    weeklyIconsValueTxt[i].text = weeklyBonusValue[i].ToString();
                    weeklyIconsCoinBonusImgs[i].SetActive(true);
                }
                else if (weeklyBonusItems[i] == WeeklyBonusItem.KeyBonus)
                {
                    weeklyIconsValueTxt[i].text = weeklyBonusValue[i].ToString();
                    weeklyIconsKeyBonusImgs[i].SetActive(true);
                }
                else
                {
                    weeklyIconsValueTxt[i].text = weeklyBonusValue[i].ToString();
                    weeklyIconsMoneyBonusImgs[i].SetActive(true);
                }
            }
        }

        DefindWeeklyIconsLocked();
    }

    //обновляем иконки наград если закрыты
    private void DefindWeeklyIconsLocked()
    {
        for (int i = 0; i < weeklyIconsLockedImage.Length; i++)
        {
            if (i < Streak - 1)
            {
                //открыта
                weeklyIconsLockedImage[i].SetActive(false);
                weeklyIconsOpenedImage[i].SetActive(true);
            }
            else
            {
                //закрыта
                weeklyIconsLockedImage[i].SetActive(true);
                weeklyIconsOpenedImage[i].SetActive(false);
            }
        }
    }

    //собираем награду
    public void ClaimReward()
    {
        if (canClaim)
        {
            ClaimCurrentBonus();
            Streak += 1;
            LastClaim = currentTime;
            mMainMenu.UpdateCoinTxt();
            CanClaimRewardUpdate();
        }
    }

    private void ClaimCurrentBonus()
    {
        if (weeklyBonusItems[Streak-1] == WeeklyBonusItem.CoinBonus)
        {
            Coins += weeklyBonusValue[Streak - 1];
        }
        else if (weeklyBonusItems[Streak - 1] == WeeklyBonusItem.KeyBonus)
        {
            KeyReward += weeklyBonusValue[Streak - 1];
        }
        else
        {
            Money += weeklyBonusValue[Streak - 1];
        }
    }

    public void BackToMenu()
    {
        gameObject.SetActive(false);
    }

    
}
