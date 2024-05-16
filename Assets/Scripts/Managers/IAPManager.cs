using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPManager : MonoBehaviour
{
    [SerializeField] private CodelessIAPButton[] _IAPButtons;
    [SerializeField] private ShopDollarItemController[] _ShopDollarItemController;

    private void Start()
    {
        if (_IAPButtons.Length < 3)
        {
            return;
        }


        _IAPButtons[0].onPurchaseComplete.AddListener(PurchaseFirstProduct);
        _IAPButtons[0].onPurchaseFailed.AddListener(PurchaseFailed);

        _IAPButtons[1].onPurchaseComplete.AddListener(PurchaseSecondProduct);
        _IAPButtons[1].onPurchaseFailed.AddListener(PurchaseFailed);

        _IAPButtons[2].onPurchaseComplete.AddListener(PurchaseThirdProduct);
        _IAPButtons[2].onPurchaseFailed.AddListener(PurchaseFailed);
    }


    private void PurchaseFirstProduct(Product product)
    {
        _ShopDollarItemController[0].PurchaseComplete();
    }


    private void PurchaseSecondProduct(Product product)
    {
        _ShopDollarItemController[1].PurchaseComplete();
    }

    private void PurchaseThirdProduct(Product product)
    {
        _ShopDollarItemController[2].PurchaseComplete();
    }

    private void PurchaseFailed(Product product, PurchaseFailureDescription arg1)
    {
        throw new NotImplementedException();
    }
}
