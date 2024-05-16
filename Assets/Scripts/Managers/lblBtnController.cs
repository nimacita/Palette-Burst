using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class lblBtnController : MonoBehaviour
{
    [SerializeField]
    [Header("Lvl Stats")]
    private int lvlNumber;
    [SerializeField]
    [Tooltip("количесвто монет  и ключей за уровень")]
    private int currentMoneyValue, currentKeyValue;
    [Tooltip("Название сцены уровня")]
    [SerializeField]
    private string sceneName;

    [Header("Debug")]
    [SerializeField]
    private bool locked;

    [Header("Editor")]
    [SerializeField]
    private TMPro.TMP_Text lvlTxt;
    [SerializeField]
    private GameObject lvlTxtObj;
    [SerializeField]
    private GameObject lockedImg;
    public LevelMenuManager levelMenuManager;

    private RewardController rewardController;

    void Start()
    {
        rewardController = RewardController.instanceReward;

        GetComponent<Button>().onClick.AddListener(LvlBtnClick);
        DefineBtnView();
    }

    //определяем открыт ли уровень
    private bool DefineIsOpenLvl()
    {
        if (lvlNumber <= rewardController.CurrentOpenLvl + 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //определяем отображение кнопки
    private void DefineBtnView()
    { 
        lvlTxt.text = lvlNumber.ToString();
        //открыт ли уровень
        if (DefineIsOpenLvl())
        {
            lockedImg.SetActive(false);
            lvlTxtObj.SetActive(true);
            gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            lockedImg.SetActive(true);
            lvlTxtObj.SetActive(false);
            gameObject.GetComponent<Button>().interactable = false;
        }

    }

    //нажатие на кнопку
    private void LvlBtnClick()
    {
        rewardController.ThisLvl = lvlNumber;
        rewardController.SetRewards(currentMoneyValue,currentKeyValue);
        levelMenuManager.LvlBtnClick(sceneName);
    }


}
