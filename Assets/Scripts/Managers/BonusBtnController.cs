using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BonusBtnController : MonoBehaviour
{

    [SerializeField]
    [Header("Bonus Settings")]
    private int bonusPrice;
    public Sprite bonusSprite;
    public enum BonusReward
    {
        CoinReward = 1,
        MysteryCoinReward = 2
    }

    [Space]
    [Header("Set Bonus Reward")]
    [SerializeField]
    private BonusReward bonusReward;

    [Space]
    [SerializeField]
    [Header("To Coin Reward")]
    private int coinBonus;

    [Space]
    [SerializeField]
    [Header("To Mystery Reward")]
    private int maxCoinReward;
    [SerializeField]
    private int minCoinReward;

    [Space]
    [SerializeField]
    [Header("To Reward")]
    private Sprite ToClaimPanelImg;

    [Space]
    [SerializeField]
    [Header("To timer")]
    private int hoursToWait;
    private DateTime currentTime;

    [Header("Debug")]
    [SerializeField]
    private int addHours = 0;
    [SerializeField]
    private bool canBuy;
    [SerializeField]
    private bool isWait;
    [SerializeField]
    private bool isUnlock;


    [Space]
    [SerializeField]
    [Header("To Editor")]
    private GameObject unlockBonusBtn;
    public GameObject bonusImg;
    [SerializeField]
    private Sprite unlockBtnSprite, waitBtnSprite, openBtnSprite;
    [SerializeField]
    private GameObject timerTxtTxt;
    [SerializeField]
    private GameObject bonusPricetxt;
    [SerializeField]
    private TMPro.TMP_Text downTimeTxt;


    public BonusMenuManager bonusMenuManager;

    void Start()
    {
        unlockBonusBtn.GetComponent<Button>().onClick.AddListener(BonusBtnClick);
        bonusImg.GetComponent<Image>().sprite = bonusSprite;
        UpdateBonusView();
    }

    private void Update()
    {
        currentTime = DateTime.Now.AddHours(addHours);
        UpdateBonusView();

        isWait = IsWaitBonus();
        isUnlock = IsUnlockClick;
        canBuy = CanUnlock();
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

    //время когда открыли бонус
    private DateTime UnlockBtnTime
    {
        get
        {
            if (!PlayerPrefs.HasKey($"LastGame{bonusPrice}"))
            {
                DateTime dateTime = new DateTime();
                return dateTime;
            }
            else
            {
                return DateTime.Parse(PlayerPrefs.GetString($"LastGame{bonusPrice}"));
            }
        }
        set
        {
            PlayerPrefs.SetString($"LastGame{bonusPrice}", value.ToString());
        }
    }

    //разблокировали ли бонус
    private bool IsUnlockClick
    {
        get
        {
            if (PlayerPrefs.HasKey($"IsUnlockClick{bonusPrice}"))
            {
                if (PlayerPrefs.GetInt($"IsUnlockClick{bonusPrice}") == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                PlayerPrefs.SetInt($"IsUnlockClick{bonusPrice}", 0);
                return false;
            }
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt($"IsUnlockClick{bonusPrice}", 1);
            }
            else
            {
                PlayerPrefs.SetInt($"IsUnlockClick{bonusPrice}", 0);
            }
        }
    }

    //можем ли купить
    private bool CanUnlock()
    {
        if (bonusPrice <= KeyReward)
        {
            return true;
        }
        return false;
    }

    //обновляем отображение кнопки
    private void UpdateBonusView()
    {
        unlockBonusBtn.GetComponent<Image>().sprite = unlockBtnSprite;
        //устанавливаем спрайты на кнопку
        if (IsWaitBonus())
        {
            //если ждем
            unlockBonusBtn.GetComponent<Image>().sprite = waitBtnSprite;
            unlockBonusBtn.GetComponent<Button>().interactable = false;

            bonusPricetxt.SetActive(false);
            timerTxtTxt.GetComponent<TMPro.TMP_Text>().text = TimerTxt();
            timerTxtTxt.SetActive(true);
        }
        else
        {
            timerTxtTxt.SetActive(false);
            bonusPricetxt.GetComponent<TMPro.TMP_Text>().text = $"{KeyReward}/{bonusPrice}";
            bonusPricetxt.SetActive(true);

            //если можем разблокировать
            if (CanUnlock())
            {
                unlockBonusBtn.GetComponent<Image>().sprite = unlockBtnSprite;
                unlockBonusBtn.GetComponent<Button>().interactable = true;
            }
            else
            {
                unlockBonusBtn.GetComponent<Button>().interactable = false;
            }

            //если можем открыть
            if (IsUnlockClick)
            {
                unlockBonusBtn.GetComponent<Image>().sprite = openBtnSprite;
                unlockBonusBtn.GetComponent<Button>().interactable = true;
            }
        }

        downTimeTxt.text = $"{hoursToWait}HOURS";

    }

    //обнволяем таймер ожидания
    private string TimerTxt()
    {
        TimeSpan sub = new TimeSpan(hoursToWait, 0, 0).Subtract(currentTime.Subtract(UnlockBtnTime));
        string txt = $"{sub.Hours:D1}h {sub.Minutes:D2}min";
        return txt;
    }

    //получение денежного бонуса
    private void ClaimCoinBonus(int addCoin)
    {
        bonusMenuManager.OpenClaimPanel(addCoin, ToClaimPanelImg);
        Coins += addCoin;
    }

    //собираем разные бонусы
    private void ClaimBonuses()
    {

        if (bonusReward == BonusReward.CoinReward)
        {
            ClaimCoinBonus(coinBonus);
        }
        if (bonusReward == BonusReward.MysteryCoinReward)
        {
            ClaimCoinBonus(Random.Range(minCoinReward, maxCoinReward + 1));
        }
        UpdateBonusView();
    }

    //ждем ли бонус
    private bool IsWaitBonus()
    {
        if (IsUnlockClick)
        {
            if ((new TimeSpan(hoursToWait, 0, 0).Subtract(currentTime.Subtract(UnlockBtnTime))).Hours <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    //нажатие на кнопку
    public void BonusBtnClick()
    {
        //если не ждем
        if (!IsWaitBonus())
        {
            //если можем открыть
            if (IsUnlockClick)
            {
                ClaimBonuses();
                IsUnlockClick = false;
                return;
            }

            //если можем разблокировать
            if (CanUnlock())
            {
                KeyReward -= bonusPrice;
                UnlockBtnTime = currentTime;
                IsUnlockClick = true;
                return;
            }

        }
    }

}
