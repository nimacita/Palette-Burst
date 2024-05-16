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

    //определяем вид кнопки
    private void UpdateItemView()
    {
        itemNameTxt.text = itemName;

        dollarTxt.SetActive(false);
        itemIcon.GetComponent<Image>().sprite = itemImage;

        //устанавливаем отоброжение для покупки за доллары
        gameObject.GetComponent<Button>().interactable = true;
        dollarTxt.GetComponent<TMPro.TMP_Text>().text = $"{dollarPrice.ToString("0.00")}$";
        dollarTxt.SetActive(true);
        itemIcon.SetActive(true); 
    }


    //метод совершенной покупки
    public void PurchaseComplete()
    {
        //прибавляем монетки и ключи
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + itemCoinReward);
        //вызываем вью успешной покупки
        shopManager.PurchaseViewEnable(true);
    }
}
