using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopDollarItemController : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField]
    private string itemName;
    [SerializeField]
    private Sprite itemImage;

    [Header("Dollar Item")]
    [SerializeField]
    private float dollarPrice;
    [SerializeField]
    private int itemCoinReward;

    [Space]
    [Header("Editor")]
    [SerializeField]
    private TMPro.TMP_Text itemNameTxt;
    [SerializeField]
    private GameObject itemIcon;
    [SerializeField]
    private GameObject shopBtn;
    [SerializeField]
    private GameObject dollarTxt;
    [SerializeField]
    private ShopManager shopManager;

    void Start()
    {

        UpdateItemView();
    }

    //���������� ��� ������
    private void UpdateItemView()
    {
        itemNameTxt.text = itemName;

        dollarTxt.SetActive(false);
        itemIcon.GetComponent<Image>().sprite = itemImage;

        //������������� ����������� ��� ������� �� �������
        gameObject.GetComponent<Button>().interactable = true;
        dollarTxt.GetComponent<TMPro.TMP_Text>().text = $"{dollarPrice.ToString("0.00")}$";
        dollarTxt.SetActive(true);
        itemIcon.SetActive(true); 
    }


    //����� ����������� �������
    public void PurchaseComplete()
    {
        //���������� ������� � �����
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + itemCoinReward);
        //�������� ��� �������� �������
        shopManager.PurchaseViewEnable(true);
    }
}
