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

    //��������� ����������� � ����
    public void UpdateMenuView()
    {
        currentLocation = CurrentLoc;
        DefineCurrentLocation();
        UpdateCoinTxt();
    }

    //���������� ������
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

    //��������� ����������� ������� � ������
    private void StartViewSettings()
    {
        MenuView.SetActive(true);
        WeeklyBonusView.SetActive(false);
        SettingsView.SetActive(false);
        BonusView.SetActive(false);
        ShopView.SetActive(false);

        UpdateMenuView();
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

    //������� �������
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

    //���������� �������� �������
    public void DefineCurrentLocation()
    {
        int curloc = currentLocation;

        //��������� �������
        if (curloc < AllOpenlocationNumber)
        {
            openLocatinView.SetActive(true);
            lockedLocationView.SetActive(false);
            //���������� �������
            for (int i = 0; i < locationsBgs.Length; i++)
            {
                if (i == curloc)
                {
                    openLocationIcon[i].SetActive(true);
                    //���� ������� �������
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
            //���������� ���
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

    //����������� �������� ������
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
            //���� �� ������� ������� ��������� 8 �������, �� ��������� ���
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

    //��������� ���������� ��������� ������������ �������
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

    //�����������. �� ���� �������
    private void NextLocation()
    {
        currentLocation += 1;
        if (IsOpenLocation(currentLocation))
        {
            CurrentLoc = currentLocation;
        }

        //������ �������
        DefineCurrentLocation();
    }

    //������������ �� ������� �������
    private void PastLocation()
    {
        currentLocation -= 1;
        if (IsOpenLocation(currentLocation))
        {
            CurrentLoc = currentLocation;
        }

        //������ �������
        DefineCurrentLocation();
    }

    //��������� ���� �����
    public void UpdateCoinTxt()
    {
        coinLabel.text = "" + Coins;
        moneyLabel.text = "" + Money;
    }

    //�������� ����� ���������� �������
    private void WeeklyViewOn()
    {
        WeeklyBonusView.SetActive(true);
    }

    //�������� ����� ��������
    private void SettingsViewOn()
    {
        SettingsView.SetActive(true);
    }

    //������ ������� ����
    private void start()
    {
        //������ ����
        levelMenuManager.BgController();
        ViewOpen(LevelView);
    }

    //�������� ���� ����
    private void miniGame()
    {
        StartCoroutine(openScene("MiniGame"));
    }

    //�������� ����� �������
    private void bonusMenu()
    {
        bonusMenuManager.UpdateCounts();
        ViewOpen(BonusView);
    }

    //�������� ����� ��������
    private void shop()
    {
        shopManager.UpdateCoinTxt();
        ViewOpen(ShopView);
    }

    //��������� ������ �����
    private void ViewOpen(GameObject view)
    {
        view.SetActive(true);
        MenuView.SetActive(false);
    }

    //��������� ����� ����� �������� ��� ��������
    IEnumerator openScene(string sceneName)
    {
        float fadeTime = mainCamera.GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneName);
    }

}
