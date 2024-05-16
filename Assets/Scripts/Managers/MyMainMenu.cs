using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MyMainMenu : MonoBehaviour
{

    [Space]
    [Header("Main settings")]
    [SerializeField]
    TMPro.TMP_Text coinLabel;
    [SerializeField]
    TMPro.TMP_Text moneyLabel;
    [SerializeField]
    private GameObject MenuView;
    [SerializeField]
    private GameObject WeeklyBonusView;
    [SerializeField]
    private GameObject SettingsView;
    [SerializeField]
    private GameObject BonusView;
    [SerializeField]
    private GameObject ShopView;
    [SerializeField]
    private GameObject LevelView;

    [Space]
    [Header("Buttons Settings")]
    public Button startBtn;
    public Button shopBtn;
    public Button rewardBtn;
    public Button settingsBtn;
    public Button bonusBtn;
    public Button miniGameBtn;
    public Button addMoneyBtn;
    public Button nextArrowBtn, pastArrowBtn;


    [Space]
    [Header("Location Settings")]
    [SerializeField]
    private int AllOpenlocationNumber;
    [SerializeField]
    private int currentLocation;
    [SerializeField]
    private GameObject nextLocationArrow;
    [SerializeField]
    private GameObject pastLocationArrow;
    [SerializeField]
    private GameObject bgPanels;
    [SerializeField]
    private GameObject[] locationsBgs;
    [SerializeField]
    private GameObject[] openLocationIcon;
    [SerializeField]
    private GameObject[] lockedLocationIcon;
    [SerializeField]
    private GameObject openLocatinView, lockedLocationView;

    [Space]
    [SerializeField]
    private GameObject mainCamera;
    [SerializeField]
    private ShopManager shopManager;
    [SerializeField]
    private BonusMenuManager bonusMenuManager;
    [SerializeField]
    private LevelMenuManager levelMenuManager;



    void Start()
    {
        currentLocation = CurrentLoc;
        ButtonSettings();
        DefineCurrentLocation();
        UpdateCoinTxt();
        StartViewSettings();
    }

    //обновляем отображение в меню
    public void UpdateMenuView()
    {
        currentLocation = CurrentLoc;
        DefineCurrentLocation();
        UpdateCoinTxt();
    }

    //настройкка кнопок
    private void ButtonSettings()
    {
        startBtn.onClick.AddListener(start);
        shopBtn.onClick.AddListener(shop);
        rewardBtn.onClick.AddListener(WeeklyViewOn);
        settingsBtn.onClick.AddListener(SettingsViewOn);
        bonusBtn.onClick.AddListener(bonusMenu);
        miniGameBtn.onClick.AddListener(miniGame);
        addMoneyBtn.onClick.AddListener(shop);
        nextArrowBtn.onClick.AddListener(NextLocation);
        pastArrowBtn.onClick.AddListener(PastLocation);
    }

    //настройки отображения экранов в начале
    private void StartViewSettings()
    {
        MenuView.SetActive(true);
        WeeklyBonusView.SetActive(false);
        SettingsView.SetActive(false);
        BonusView.SetActive(false);
        ShopView.SetActive(false);

        UpdateMenuView();
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
            if (value <= AllOpenlocationNumber)
            {
                PlayerPrefs.SetInt("currentLocation", value);
            }
        }

    }

    //определяем открытую локацию
    public void DefineCurrentLocation()
    {
        int curloc = currentLocation;

        //последняя локация
        if (curloc < AllOpenlocationNumber)
        {
            openLocatinView.SetActive(true);
            lockedLocationView.SetActive(false);
            //определяем локацию
            for (int i = 0; i < locationsBgs.Length; i++)
            {
                if (i == curloc)
                {
                    openLocationIcon[i].SetActive(true);
                    //если локацич открыта
                    if (IsOpenLocation(i))
                    {
                        startBtn.interactable = true;
                        lockedLocationIcon[i].SetActive(false);
                        for (int j = 0; j < locationsBgs.Length; j++)
                        {
                            if (j != i)
                            {
                                locationsBgs[j].SetActive(false);
                            }
                        }
                    }
                    else
                    {
                        lockedLocationIcon[i].SetActive(true);
                        startBtn.interactable = false;
                    }
                }
                else
                {
                    openLocationIcon[i].SetActive(false);
                }
            }
            //определяем фон
            for (int i = 0; i < locationsBgs.Length; i++)
            {
                if (curloc == i)
                {
                    locationsBgs[i].SetActive(true);
                }
                else
                {
                    locationsBgs[i].SetActive(false);
                }
            }
        }
        else
        {
            openLocatinView.SetActive(false);
            lockedLocationView.SetActive(true);
        }

        DefineLocationArrow();
    }

    //сохраненное значения уровня
    public int CurrentOpenLvl(int ind)
    {
        if (PlayerPrefs.HasKey($"currentOpenLvl{ind}"))
        {
            return PlayerPrefs.GetInt($"currentOpenLvl{ind}");
        }
        else
        {
            PlayerPrefs.SetInt($"currentOpenLvl{ind}", 0);
            return 0;
        }
    }

    private bool IsOpenLocation(int locateInd)
    {
        if (locateInd == 0)
        {
            return true;
        }
        else
        {
            //если на прошлой локации продйенно 8 уровней, то открываем эту
            if (CurrentOpenLvl(locateInd - 1) >= 8)
            {
                return true;
            }
            else
            {
                return false; 
            }
        }

    }

    //Проверяем активность стрелочек переключения локаций
    private void DefineLocationArrow()
    {
        int curloc = currentLocation;

        if (curloc <= 0)
        {
            pastLocationArrow.SetActive(false);
        }
        else
        {
            pastLocationArrow.SetActive(true);
        }

        if (curloc >= AllOpenlocationNumber)
        {
            nextLocationArrow.SetActive(false);
        }
        else
        {
            nextLocationArrow.SetActive(true);
        }
    }

    //переключени. на след локацию
    private void NextLocation()
    {
        currentLocation += 1;
        if (IsOpenLocation(currentLocation))
        {
            CurrentLoc = currentLocation;
        }

        //меняем локацию
        DefineCurrentLocation();
    }

    //переключение на прошлую локацию
    private void PastLocation()
    {
        currentLocation -= 1;
        if (IsOpenLocation(currentLocation))
        {
            CurrentLoc = currentLocation;
        }

        //меняем локацию
        DefineCurrentLocation();
    }

    //обновляем коин текст
    public void UpdateCoinTxt()
    {
        coinLabel.text = "" + Coins;
        moneyLabel.text = "" + Money;
    }

    //включаем экран ежедневной награды
    private void WeeklyViewOn()
    {
        WeeklyBonusView.SetActive(true);
    }

    //включаем экран настроек
    private void SettingsViewOn()
    {
        SettingsView.SetActive(true);
    }

    //запуск игровых сцен
    private void start()
    {
        //запуск игры
        levelMenuManager.BgController();
        ViewOpen(LevelView);
    }

    //включаем мини игру
    private void miniGame()
    {
        StartCoroutine(openScene("MiniGame"));
    }

    //включаем сцену бонусов
    private void bonusMenu()
    {
        bonusMenuManager.UpdateCounts();
        ViewOpen(BonusView);
    }

    //включаем сцену магазина
    private void shop()
    {
        shopManager.UpdateCoinTxt();
        ViewOpen(ShopView);
    }

    //открываем нужный экран
    private void ViewOpen(GameObject view)
    {
        view.SetActive(true);
        MenuView.SetActive(false);
    }

    //открываем сцену после задержки для перехода
    IEnumerator openScene(string sceneName)
    {
        float fadeTime = mainCamera.GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneName);
    }

}
