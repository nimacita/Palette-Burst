using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ShopItemController;

public class ShopItemController : MonoBehaviour
{

    [Header("Item Settings")]
    [SerializeField]
    private string itemName;

    [Header("Coin Item")]
    [SerializeField]
    private int coinPrice;
    public enum ItemReward
    {
        CharacterSkinReward = 0,
        LocationSkinReward = 1,
    }
    //награды за покупку за валюту
    [SerializeField]
    private ItemReward itemReward;

    //для покупки скина персонажа
    [Space]
    [Header("Character Skin Reward")]
    [SerializeField]
    private Sprite currentCharacterSkin;
    [SerializeField]
    [Space(5), Tooltip("индекс персонажа скина для покупки")]
    [Range(0, 3)]
    private int playerSkinSelectedInd;

    //для покупки скина локации
    [Space]
    [Header("Location Skin Reward")]
    [SerializeField]
    private Sprite currentLocationSkin;
    [SerializeField]
    [Space(5), Tooltip("индекс локации для покупки скина на нее начиная от 0 до 2 включительно")]
    [Range(0, 2)]
    private int locationSelectedInd;


    [Header("To Item Purchase")]
    public Sprite itemPurchaseIcon;

    [Space]
    [Header("Editor")]
    [SerializeField]
    private ShopManager shopManager;
    [SerializeField]
    private TMPro.TMP_Text itemNameTxt;
    [SerializeField]
    private GameObject itemIcon;
    [SerializeField]
    private GameObject shopBtn;
    [SerializeField]
    private GameObject coinTxt;
    [SerializeField]
    private GameObject equiped;

    void Start()
    {
        itemNameTxt.text = itemName;
        DefineItemImage();
        coinTxt.SetActive(false);
        coinTxt.GetComponent<TMPro.TMP_Text>().text = $"{coinPrice}";
        gameObject.GetComponent<Button>().interactable = true;
        gameObject.GetComponent<Button>().onClick.AddListener(ShopItemBtnClick);

        UpdateItemView();
    }

    private void DefineItemImage()
    {
        if (itemReward == ItemReward.CharacterSkinReward)
        {
            //скина персонажа
            itemIcon.GetComponent<Image>().sprite = currentCharacterSkin;
        }
        else if (itemReward == ItemReward.LocationSkinReward)
        {
            //скина локации
            itemIcon.GetComponent<Image>().sprite = currentLocationSkin;
        }
    }

    private void FixedUpdate()
    {
        UpdateItemView();
    }

    //сохраненное значение куплен ли товар
    private bool IsShopItemPurchased
    {
        get
        {
            if (PlayerPrefs.HasKey($"IsShopItemPurchased{itemName}"))
            {
                if (PlayerPrefs.GetInt($"IsShopItemPurchased{itemName}") == 1)
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
                PlayerPrefs.SetInt($"IsShopItemPurchased{itemName}", 0);
                return false;
            }
        }

        set
        {
            if (value)
            {
                PlayerPrefs.SetInt($"IsShopItemPurchased{itemName}", 1);
            }
            else
            {
                PlayerPrefs.SetInt($"IsShopItemPurchased{itemName}", 0);
            }
        }
    }

    //текущий скин персонажа
    private int CurrentCharacterSkinInd
    {
        get
        {
            if (PlayerPrefs.HasKey($"CurrentCharacterInd"))
            {
                return PlayerPrefs.GetInt($"CurrentCharacterInd");
            }
            else
            {
                PlayerPrefs.SetInt($"CurrentCharacterInd", 0);
                return PlayerPrefs.GetInt($"CurrentCharacterInd");
            }
        }

        set
        {
            PlayerPrefs.SetInt($"CurrentCharacterInd", value);
        }

    }

    //поставлен ли скин на локацию
    private bool IsSkinToLocation
    {
        get
        {
            if (PlayerPrefs.HasKey($"IsSkinToLocation{locationSelectedInd}"))
            {
                if (PlayerPrefs.GetInt($"IsSkinToLocation{locationSelectedInd}") == 0)
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
                PlayerPrefs.SetInt($"IsSkinToLocation{locationSelectedInd}",0);
                return false;
            }
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt($"IsSkinToLocation{locationSelectedInd}", 1);
            }
            else
            {
                PlayerPrefs.SetInt($"IsSkinToLocation{locationSelectedInd}", 0);
            }
        }
    }

    //текущая локация
    private int CurrentLoc
    {
        get
        {
            if (PlayerPrefs.HasKey("currentLocation"))
            {
                return PlayerPrefs.GetInt("currentLocation");
            }
            else
            {
                return 0;
            }
        }
        set
        {
            PlayerPrefs.SetInt("currentLocation", value);
        }

    }

    //определяем вид кнопки
    private void UpdateItemView()
    {
        itemIcon.SetActive(true);

        //проверить куплено ли
        if (!IsShopItemPurchased)
        {
            //если не куплен
            coinTxt.SetActive(true);
            equiped.SetActive(false);
        }
        else
        {
            //если куплен
            coinTxt.SetActive(false);
            equiped.SetActive(IsEquiped());
        }

    }

    //проверяем если скин экипирован
    private bool IsEquiped()
    {
        bool isEquiped = false;
        if (itemReward == ItemReward.CharacterSkinReward)
        {
            //скин персонажа
            //проверяем значение имени спрайта с сохраненным значением
            if(playerSkinSelectedInd == CurrentCharacterSkinInd)
            {
                isEquiped = true;
            }
        }
        else if (itemReward == ItemReward.LocationSkinReward)
        {
            //скин локации
            if (IsSkinToLocation)
            {
                isEquiped = true;
            }
        }
        return isEquiped;
    }

    //можем ли купить, выводим нужные экраны
    private void CanClaim()
    {
        if (!IsShopItemPurchased) {
            //если не куплено - покупаем
            if (PlayerPrefs.GetInt("Coins") < coinPrice)
            {
                //не можем купить
                shopManager.PurchaseViewEnable(false);
            }
            else
            {
                //можем купить
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - coinPrice);
                //shopManager.PurchaseViewEnable(true);
                shopManager.ItepPurchaseEnable(itemPurchaseIcon, itemName);
                IsShopItemPurchased = true;
            }
        }
        else
        {
            //иначе по нажатию - экипируем
            EquipedSelectProduct();
        }
    }

    //экипируем или снимаем
    private void EquipedSelectProduct()
    {
        if (itemReward == ItemReward.CharacterSkinReward)
        {
            //скина персонажа
            if (playerSkinSelectedInd != CurrentCharacterSkinInd) 
            {
                //экипируем 
                CurrentCharacterSkinInd = playerSkinSelectedInd;
            }
            else
            {
                //снимаем 
                CurrentCharacterSkinInd = 0;
            }
        } 
        else if(itemReward == ItemReward.LocationSkinReward)
        {
            //скина локации
            if (!IsSkinToLocation)
            {
                //экипируем
                IsSkinToLocation = true;
            }
            else
            {
                //снимаем
                IsSkinToLocation = false;
            }

        }
    }

    //нажатие на кнопку покупки
    public void ShopItemBtnClick()
    {
        CanClaim();
    }
    
}
