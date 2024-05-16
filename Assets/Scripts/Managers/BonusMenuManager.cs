using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BonusMenuManager : MonoBehaviour
{

    [SerializeField]
    private GameObject bonusView;
    [SerializeField]
    private GameObject menuView;
    [SerializeField]
    private GameObject bonusClaimPanel;
    public TMPro.TMP_Text coinClaimTxt;
    public GameObject bonusClaimImg;

    [Header("Label Settings")]
    public TMPro.TMP_Text keyText;
    public TMPro.TMP_Text coinsText;
    public TMPro.TMP_Text moneyText;

    [Header("Button Settings")]
    public Button homeBtn;
    public Button closeClaimPanelBtn;

    public MyMainMenu mainMenu;


    void Start()
    {
        bonusClaimPanel.SetActive(false);
        ButtonSettings();
        UpdateCounts();
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

    //настройка нопок
    private void ButtonSettings()
    {
        homeBtn.onClick.AddListener(MainMenu);
        closeClaimPanelBtn.onClick.AddListener(CloseBonusClaimPanel);
    }

    private void Update()
    {
        UpdateCounts();
    }

    public void UpdateCounts()
    {
        keyText.text = $"{KeyReward}";
        coinsText.text = $"{Coins}";
        moneyText.text = $"{Money}";
    }

    public void CloseBonusClaimPanel()
    {
        bonusClaimPanel.SetActive(false);
        UpdateCounts();
    }

    public void OpenClaimPanel(int coinsCount, Sprite bonusImg)
    {
        bonusClaimImg.GetComponent<Image>().sprite = bonusImg;
        coinClaimTxt.text = $"X{coinsCount}";
        bonusClaimPanel.SetActive(true);
    }

    //выход в меню
    public void MainMenu()
    {
        mainMenu.UpdateCoinTxt();
        menuView.SetActive(true);
        bonusView.SetActive(false);
    }
}
