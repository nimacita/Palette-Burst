using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

    [Header("Valute Settings")]
    [SerializeField]
    private TMPro.TMP_Text coinValue;
    [SerializeField]
    private TMPro.TMP_Text moneyValue;
    [SerializeField]
    private TMPro.TMP_Text keyValue;

    [Header("Purchase Settings")]
    [SerializeField]
    private GameObject purchaseView;
    [SerializeField]
    private GameObject successPurchase, errorPurchase;

    [Header("Item Purchase")]
    [SerializeField]
    private GameObject itemPurchase;
    public GameObject itemPurchaseIcon;
    public TMPro.TMP_Text itemPurchseName;

    [Header("Button Settings")]
    public Button homeBtn;
    public Button itemPurchseCloseBtn;
    public Button successPurchaseCloseBtn, errorPurchaseCloseBtn;

    [Header("Editor")]
    [SerializeField]
    private GameObject menuView;
    [SerializeField]
    private GameObject shopView;
    [SerializeField]
    private MyMainMenu myMainMenu;

    void Start()
    {
        ButtonSettings();
        purchaseView.SetActive(false);
        UpdateCoinTxt();
    }

    //����������� �������� �����
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

    //����������� �������� �����
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

    //����������� �������� ������
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

    //��������� ������
    private void ButtonSettings()
    {
        homeBtn.onClick.AddListener(MainMenu);
        successPurchaseCloseBtn.onClick.AddListener(PurchaseDisable);
        errorPurchaseCloseBtn.onClick.AddListener(PurchaseDisable);
        itemPurchseCloseBtn.onClick.AddListener(ItemPurchaseDisable);
    }

    //�������� ���� �������
    public void PurchaseViewEnable(bool success)
    {
        purchaseView.SetActive(true);
        UpdateCoinTxt();
        if (success)
        {
            successPurchase.SetActive(true);
            errorPurchase.SetActive(false);
        }
        else
        {
            successPurchase.SetActive(false);
            errorPurchase.SetActive(true);
        }
    }

    //��������� ���� �������
    public void PurchaseDisable()
    {
        purchaseView.SetActive(false);
        UpdateCoinTxt();
    }

    //�������� ���� �������� ��������
    public void ItepPurchaseEnable(Sprite itemImg, string itemName)
    {
        errorPurchase.SetActive(false);
        successPurchase.SetActive(false);
        purchaseView.SetActive(true);
        itemPurchase.SetActive(true);
        itemPurchaseIcon.GetComponent<Image>().sprite = itemImg;
        itemPurchseName.text = itemName;
    }

    //��������� ���� ������� ��������
    private void ItemPurchaseDisable()
    {
        itemPurchase.SetActive(false);
        purchaseView.SetActive(false);
        UpdateCoinTxt();
    }

    //��������� ����� ������
    public void UpdateCoinTxt()
    {
        coinValue.text = $"{Coins}";
        moneyValue.text = $"{Money}";
        keyValue.text = $"{KeyReward}";
    }

    //����� � ����
    public void MainMenu()
    {
        menuView.SetActive(true);
        shopView.SetActive(false);
        myMainMenu.UpdateMenuView();
    }

}
