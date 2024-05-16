using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenuManager : MonoBehaviour
{

    [SerializeField]
    private GameObject lvlMenuView;
    [SerializeField]
    private GameObject menuView;
    [SerializeField]
    private GameObject[] levelLocations;
    [SerializeField]
    private TMPro.TMP_Text locationTxt;
    [SerializeField]
    private string[] locationNames;
    [Space]
    [SerializeField]
    private GameObject mainCamera;

    public MyMainMenu mainMenu;

    void Start()
    {
        BgController();
    }

    //нынешн€€ лоаци€
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

    }

    //выбираем фон
    public void BgController()
    {
        for (int i = 0; i < levelLocations.Length; i++)
        {
            if (i == CurrentLoc)
            {
                levelLocations[i].SetActive(true);
            }
            else
            {
                levelLocations[i].SetActive(false);
            }
        }
        if (CurrentLoc > levelLocations.Length)
        {
            levelLocations[0].SetActive(true);
        }
        locationTxt.text = locationNames[CurrentLoc];
    }

    //выход в меню
    public void MainMenu()
    {
        mainMenu.UpdateCoinTxt();
        menuView.SetActive(true);
        lvlMenuView.SetActive(false);
    }

    public void LvlBtnClick(string levelName)
    {
        StartCoroutine(openScene(levelName));
    }

    //открываем сцену после задержки дл€ перехода
    IEnumerator openScene(string sceneName)
    {
        float fadeTime = mainCamera.GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneName);
    }
}
