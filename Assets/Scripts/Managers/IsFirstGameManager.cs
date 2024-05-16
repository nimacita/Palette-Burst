using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsFirstGameManager : MonoBehaviour
{

    [SerializeField]
    private int pageCount;
    private int currPage;
    [SerializeField]
    private TMPro.TMP_Text pageTxt;
    [SerializeField]
    [Space(5), Tooltip("количество подсказок должно соответствовать количеству страниц")]
    private string[] pageTips;
    [SerializeField]
    private GameObject[] pageImgs;
    [SerializeField]
    private TMPro.TMP_Text pageNumberTxt;
    [SerializeField]
    private GameObject firstGameView;
    [SerializeField]
    private Button nextBtn;

    void Start()
    {
        currPage = 0;
        nextBtn.onClick.AddListener(NextBtnClick);
        UpdateIsFirstGame();
    }

    //сохраненное значение самой первой игры
    private bool isFirstgame
    {
        get
        {
            if (PlayerPrefs.HasKey("isFirstGame"))
            {
                if (PlayerPrefs.GetInt("isFirstGame") == 1)
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
                PlayerPrefs.SetInt("isFirstGame", 1);
                return true;
            }
        }

        set
        {
            if (value)
            {
                PlayerPrefs.SetInt("isFirstGame", 1);
            }
            else
            {
                PlayerPrefs.SetInt("isFirstGame", 0);
            }
        }
    }

    //смотрим если первый раз зашли обновляем экраны
    private void UpdateIsFirstGame()
    {
        if (isFirstgame)
        {
            //включаем экран первой игры
            firstGameView.SetActive(true);
            pageTxt.text = pageTips[currPage];
            pageNumberTxt.text = $"{currPage + 1}/{pageCount}";

            for (int i = 0; i < pageImgs.Length; i++) 
            {
                if (i == currPage)
                {
                    pageImgs[i].SetActive(true);
                }
                else
                {
                    pageImgs[i].SetActive(false);
                }
            }
        }
        else
        {
            //отключаем экран первой игры
            firstGameView.SetActive(false);
        }
        
    }

    //нажатие на след кнопку
    public void NextBtnClick()
    {
        if (currPage < pageCount - 1)
        {
            //переключаем страницу
            currPage++;
        }
        else
        {
            //выключаем вью
            isFirstgame = false;
        }
        UpdateIsFirstGame();
    }

}
