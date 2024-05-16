using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[System.Serializable]
public class ItemSkin
{
    [Tooltip("���� ������� ���� ������")]
    public Sprite skinBg;
    [Tooltip("����� ������")]
    public Sprite[] itemSprites;
    [Tooltip("���� ������� ��� �������")]
    public AudioClip popSound;
    [Tooltip("���� ������� ��� �������")]
    public AudioClip fallSound;
}

public class MainGameController : MonoBehaviour
{

    [Header("Game Settings")]
    [SerializeField]
    [Tooltip("���������� �����")]
    private int RowCount;
    [SerializeField]
    [Tooltip("���������� �������")]
    private int ColumnCount;
    [SerializeField]
    [Tooltip("�������� ������� �������")]
    private float itemMoveSpeed;

    [Header("Score Settings")]
    [SerializeField]
    [Tooltip("���� ������������ �� ���� �����")]
    private int scoreKoef;
    private int currentScore;


    [Header("Timer Settings")]
    [Tooltip("����� �� ������ �� ������")]
    public bool isTimer;
    [Tooltip("���������� ������ �� �������")]
    public int timerSeconds;
    private float timer;
    private bool goTimer;

    [Header("Skin Settings")]
    [Tooltip("��� ��������� ������� ��� �������")]
    public ItemSkin[] Skins;
    [Tooltip("����� ��� �������")]
    public Sprite defaultLocationSkin;
    public Sprite anotherLocationSkin;

    [Header("Game View Settings")]
    public GameObject itemsPanel;
    public GameObject timerPanel;
    public TMPro.TMP_Text timerTxt;
    public GameObject scorePanel;
    public TMPro.TMP_Text scoreTxt;
    public Button pauseBtn;

    [Header("Victory View")]
    public GameObject victoryView;
    public TMPro.TMP_Text victoryScoreTxt;
    public TMPro.TMP_Text keyRewardTxt;
    public TMPro.TMP_Text moneyRewardTxt;
    public Button victoryNextBtn;

    [Header("Defeat View")]
    public GameObject defeatView;
    public Button defeatRetryBtn;
    public Button defeatSkipBtn;

    [Header("Pause View")]
    public GameObject pauseView;
    public TMPro.TMP_Text pauseScoreTxt;
    public Button pauseNextBtn;
    public Button pauseExitBtn;
    public GameObject musicBtn;
    public GameObject soundBtn;
    [SerializeField]
    private Sprite musicOn, musicOff;
    [SerializeField]
    private Sprite soundOn, soundOff;

    [Header("Sound Settings")]
    public AudioSource winSound;
    public AudioSource defeatSound;
    public AudioSource currentPopSound;
    public AudioSource currentFallSound;

    [Header("Editor")]
    public GameObject mainCamera;
    public GameObject gameBg;

    [Header("Debug")]
    [SerializeField]
    private float clickWaitTime;
    private bool canClick;
    private bool isLose = false;
    private bool isWin = false;

    private GameObject[,] items;
    private ItemController[,] itemControllers;

    private RewardController rewardController;

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
    private bool Music
    {
        get
        {
            if (PlayerPrefs.HasKey("music"))
            {
                if (PlayerPrefs.GetInt("music") == 1)
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
                PlayerPrefs.SetInt("music", 1);
                return true;
            }
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt("music", 1);
            }
            else
            {
                PlayerPrefs.SetInt("music", 0);
            }
        }
    }
    private bool Sound
    {
        get
        {
            if (PlayerPrefs.HasKey("sound"))
            {
                if (PlayerPrefs.GetInt("sound") == 1)
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
                PlayerPrefs.SetInt("sound", 1);
                return true;
            }
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt("sound", 1);
            }
            else
            {
                PlayerPrefs.SetInt("sound", 0);
            }
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

    }

    //��������� �� ���� �� �������
    private bool IsSkinToLocation
    {
        get
        {
            if (PlayerPrefs.HasKey($"IsSkinToLocation{CurrentLoc}"))
            {
                if (PlayerPrefs.GetInt($"IsSkinToLocation{CurrentLoc}") == 0)
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
                PlayerPrefs.SetInt($"IsSkinToLocation{CurrentLoc}", 0);
                return false;
            }
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt($"IsSkinToLocation{CurrentLoc}", 1);
            }
            else
            {
                PlayerPrefs.SetInt($"IsSkinToLocation{CurrentLoc}", 0);
            }
        }
    }

    //������� ���� ���������
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

    void Start()
    {
        rewardController = RewardController.instanceReward;

        canClick = false;
        currentScore = 0;

        StartCoroutine(FirstSettings());
        ViewSettings();
        ButtonSettings();

        PlusScore();
        FirstInitializedItems();
        SetSkinSounds();
    }

    //�������� ������
    private void ButtonSettings()
    {
        victoryNextBtn.onClick.AddListener(GoToMenu);
        defeatSkipBtn.onClick.AddListener(GoToMenu);
        defeatRetryBtn.onClick.AddListener(Restart);
        pauseBtn.onClick.AddListener(PauseOn);
        pauseNextBtn.onClick.AddListener(PauseOff);
        pauseExitBtn.onClick.AddListener(GoToMenu);
        musicBtn.GetComponent<Button>().onClick.AddListener(MusicBtnClick);
        soundBtn.GetComponent<Button>().onClick.AddListener(SoundBtnClick);
    }

    private void Update()
    {
        TimerTick();
    }

    //��������� ���������
    private IEnumerator FirstSettings()
    {
        goTimer = false;
        yield return new WaitForSeconds(0.5f);
        canClick = true;
        if (isTimer) 
        {
            timer = timerSeconds;
            goTimer = true;
        }
    }

    //��������� ���
    private void ViewSettings()
    {
        if (!IsSkinToLocation)
        {
            gameBg.GetComponent<Image>().sprite = defaultLocationSkin;
        }
        else
        {
            gameBg.GetComponent<Image>().sprite = anotherLocationSkin;
        }

        timerPanel.SetActive(isTimer);
        victoryView.SetActive(false);
        defeatView.SetActive(false);
        pauseView.SetActive(false);

        UpdateBtnSprite();
    }

    //�������� ������ ������
    private IEnumerator Victory()
    {
        rewardController.CurrentOpenLvl = rewardController.ThisLvl;

        victoryScoreTxt.text = $"score: {currentScore}";

        //���� �������
        Money += rewardController.MoneyCurrReward;
        KeyReward += rewardController.KeyCurrReward;

        moneyRewardTxt.text = $"x{rewardController.MoneyCurrReward}";
        keyRewardTxt.text = $"x{rewardController.KeyCurrReward}";

        yield return new WaitForSeconds(0.8f);
        PlaySound(winSound);
        victoryView.SetActive(true);
    }

    //�������� ������ ���������
    private IEnumerator Defeat()
    {
        PlaySound(defeatSound);
        yield return new WaitForSeconds(0.8f);
        defeatView.SetActive(true);
    }

    //�������� ������ �����
    private void PauseOn()
    {
        if (!isWin && !isLose) 
        {
            pauseScoreTxt.text = $"{currentScore}";
            pauseView.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    //��������� ������ �����
    private void PauseOff()
    {
        pauseView.SetActive(false);
        Time.timeScale = 1f;
    }

    //��������� ������� ������
    private void UpdateBtnSprite()
    {
        if (Music)
        {
            musicBtn.GetComponent<Image>().sprite = musicOn;
        }
        else
        {
            musicBtn.GetComponent<Image>().sprite = musicOff;
        }

        if (Sound)
        {
            soundBtn.GetComponent<Image>().sprite = soundOn;
        }
        else
        {
            soundBtn.GetComponent<Image>().sprite = soundOff;
        }
    }

    //������ ������
    public void MusicBtnClick()
    {
        if (Music)
        {
            Music = false;
        }
        else
        {
            Music = true;
        }
        UpdateBtnSprite();
    }

    //������ ������
    public void SoundBtnClick()
    {
        if (Sound)
        {
            Sound = false;
        }
        else
        {
            Sound = true;
        }
        UpdateBtnSprite();
    }

    //������� � ����
    private void GoToMenu()
    {
        Time.timeScale = 1f;
        StartCoroutine(openScene("main menu"));
    }

    //�������
    private void Restart()
    {
        Time.timeScale = 1f;
        StartCoroutine(openScene(SceneManager.GetActiveScene().name));
    }

    //�������������� ��� ������� �������
    private void FirstInitializedItems()
    {
        items = new GameObject[RowCount, ColumnCount];
        itemControllers = new ItemController[RowCount, ColumnCount];

        int itemInd = 0;
        for (int i = 0; i < RowCount; i++) 
        {
            for (int j = 0; j < ColumnCount; j++)
            {
                items[i, j] = itemsPanel.transform.GetChild(itemInd).gameObject;
                itemControllers[i,j] = items[i,j].GetComponent<ItemController>();
                itemControllers[i,j].SetItemCoord(i, j);
                itemControllers[i, j].SetItemSprite(RandItemSprite(), Skins[CurrentCharacterSkinInd].skinBg);

                itemInd++;
            }
        }
    }

    //��������� ���� ��� �������
    private Sprite RandItemSprite()
    {
        int rand = Random.Range(0, Skins[CurrentCharacterSkinInd].itemSprites.Length);
        return Skins[CurrentCharacterSkinInd].itemSprites[rand];
    }

    //������������� �����
    private void SetSkinSounds()
    {
        currentPopSound.clip = Skins[CurrentCharacterSkinInd].popSound;
        currentFallSound.clip = Skins[CurrentCharacterSkinInd].fallSound;
    }

    //������� ������
    private void TimerTick()
    {
        if (goTimer && !isLose && !isWin)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer = 0f;
                isLose = true;
                //��� ���������
                StartCoroutine(Defeat());
            }
            timerTxt.text = $"00:{(int)timer:D2}";
        }
    }

    //���������� ������ �� �������� ������
    private void PlusScore(int itemCount = 0)
    {
        currentScore += itemCount * scoreKoef;
        scoreTxt.text = $"{currentScore}";
        IsVictory();
    }

    //�������� ��
    private void IsVictory() 
    {
        if (currentScore >= scoreKoef * RowCount * ColumnCount)
        {
            isWin = true;
            //���� ������
            StartCoroutine(Victory());
        }
    }

    //������� �� ������������ �����
    public void ItemClick(int currI, int currJ)
    {
        if (canClick && !isLose) 
        {
            List<GameObject> matchItems = FindAllMatchItems(currI, currJ);
            StartCoroutine(DisableMatchItems(matchItems));
            PlaySound(currentPopSound);
            PlusScore(matchItems.Count);
        }
    }

    //��������� ��� ���������� �������
    private IEnumerator DisableMatchItems(List<GameObject> matchItems)
    {
        canClick = false;
        for (int i = 0; i < matchItems.Count; i++)
        {
            matchItems[i].GetComponent<ItemController>().PlayParticlesAndAnim();
        }
        yield return new WaitForSeconds(0.12f);
        for (int i = 0; i < matchItems.Count; i++) 
        {
            matchItems[i].SetActive(false);
        }
        FindItemsToMove();
        StartCoroutine(ClickWait());
    }

    //���������� �� ������ ������
    private void FindItemsToMove(int jInd = 0, bool isPlaySound = true)
    {
        for (int j = jInd; j < ColumnCount; j++) 
        {
            GameObject currObj = null;
            for (int i = 0; i < RowCount; i++) 
            {
                //���� ����� ����������
                if (!items[i,j].activeSelf)
                {
                    if (currObj!=null)
                    {
                        //Debug.Log($"������ ������� {currObj.name} � {FindLastEmpty(i,j).name}");
                        MoveSelectedItem(currObj, FindLastEmpty(i, j));
                        currObj = null;

                        if (isPlaySound)
                        {
                            PlaySound(currentFallSound);
                            isPlaySound = false;
                        }

                        FindItemsToMove(j, isPlaySound);
                    }
                }
                else
                {
                    currObj = items[i,j];
                }
            }
            isPlaySound = true;
        }
    }

    //������� ��������� ����� ���� �� ������ ������
    private void MoveSelectedItem(GameObject currObj, GameObject emptyObj)
    {
        ItemController currIC = currObj.GetComponent<ItemController>();
        ItemController emptyObjIC = emptyObj.GetComponent<ItemController>();
        //������� ��������� ������
        Vector3 emptyPos = emptyObj.transform.position;
        //Vector3 emptyPos = emptyObjIC.GetItemPosition();
        //emptyObj.transform.position = currObj.transform.position;
        emptyObj.transform.position = currIC.GetItemPosition();
        currIC.MoveItem(emptyPos, itemMoveSpeed);


        //������ ���������� � �������� ������� �������
        int tI = currIC.GetItemI();
        int tJ = currIC.GetItemJ();
        currIC.SetItemCoord(emptyObjIC.GetItemI(), emptyObjIC.GetItemJ());
        emptyObjIC.SetItemCoord(tI, tJ);

        UpdateItemsArrays();
    }

    //��������� ������ �������
    private void UpdateItemsArrays()
    {
        items = new GameObject[RowCount, ColumnCount];
        itemControllers = new ItemController[RowCount, ColumnCount];

        for (int i = 0; i < RowCount * ColumnCount; i++)
        {
            GameObject item = itemsPanel.transform.GetChild(i).gameObject;
            ItemController itemController = item.GetComponent<ItemController>();
            items[itemController.GetItemI(), itemController.GetItemJ()] = item;
            itemControllers[itemController.GetItemI(), itemController.GetItemJ()] = itemController;
        }
    }

    //������� ������� ������
    private GameObject FindLastEmpty(int currI, int currJ)
    {
        GameObject lastObj = items[currI, currJ];
        for (int i = currI; i < RowCount; i++) 
        {
            if (items[i, currJ].activeSelf)
            {
                return lastObj;
            }
            else
            {
                lastObj = items[i,currJ];
            }
        }
        return lastObj;
    }

    //������� ��� ����������� ������� ������ �����
    private List<GameObject> FindAllMatchItems(int currI, int currJ)
    {
        List<GameObject> allMatchItems = new List<GameObject>();
        allMatchItems.Add(items[currI, currJ]);
        itemControllers[currI, currJ].SetSelected(true);

        allMatchItems.AddRange(CurrentMatchItems(currI, currJ));

        return allMatchItems;
    }

    //����� ��� ������������ ����������� ���� ����������� ��������
    private List<GameObject> CurrentMatchItems(int currI, int currJ)
    {
        List<GameObject> currentMatchItems = new List<GameObject>();
        currentMatchItems = GetMatchAroundItems(currI, currJ);
        if (currentMatchItems.Count != 0)
        {
            for (int i = 0; i < currentMatchItems.Count; i++)
            {
                currentMatchItems.AddRange(CurrentMatchItems(
                    currentMatchItems[i].GetComponent<ItemController>().GetItemI(),
                    currentMatchItems[i].GetComponent<ItemController>().GetItemJ()));
            }
        }
        return currentMatchItems;
    }

    //������� ��� ������� ������ ����� ������ ����������
    private List<GameObject> GetMatchAroundItems(int currI, int currJ)
    {
        List<GameObject> matchItems = new List<GameObject>();
        if (!itemControllers[currI,currJ].GetSelected())
        {
            matchItems.Add(items[currI, currJ]);
            itemControllers[currI, currJ].SetSelected(true);
        }
        //�������
        if (currI > 0)
        {
            //��������� ������������� ������
            int compI = currI - 1;
            int compJ = currJ;
            if (IsMatchColors(compI, compJ, currI, currJ))
            {
                //���� ����� ��������� �� ��������� � ������
                matchItems.Add(items[compI, compJ]);
                itemControllers[compI,compJ].SetSelected(true);
            }
        }
        //������
        if (currI < RowCount - 1)
        {
            //��������� ������������� ������
            int compI = currI + 1;
            int compJ = currJ;
            if (IsMatchColors(compI, compJ, currI, currJ))
            {
                //���� ����� ��������� �� ��������� � ������
                matchItems.Add(items[compI, compJ]);
                itemControllers[compI, compJ].SetSelected(true);
            }
        }
        //������
        if (currJ < ColumnCount - 1)
        {
            //��������� ������������� ������
            int compI = currI;
            int compJ = currJ + 1;
            if (IsMatchColors(compI, compJ, currI, currJ))
            {
                //���� ����� ��������� �� ��������� � ������
                matchItems.Add(items[compI, compJ]);
                itemControllers[compI, compJ].SetSelected(true);
            }
        }
        //�����
        if (currJ > 0)
        {
            //��������� ������������� ������
            int compI = currI;
            int compJ = currJ - 1;
            if (IsMatchColors(compI, compJ, currI, currJ))
            {
                //���� ����� ��������� �� ��������� � ������
                matchItems.Add(items[compI, compJ]);
                itemControllers[compI, compJ].SetSelected(true);
            }
        }
        //���������� ������ ������ � ����� ������
        return matchItems;
    }

    //�������� �� ���������� ����
    private bool IsMatchColors(int compI, int compJ, int mainI, int mainJ)
    {
        //���� ���������� ����� � ��� ��������� ������� � ������������� �������� �� ������� ���
        if (itemControllers[compI,compJ].GetItemSprite() ==
            itemControllers[mainI,mainJ].GetItemSprite() 
            && (items[compI,compJ].activeSelf && items[mainI,mainJ].activeSelf)
            && !itemControllers[compI,compJ].GetSelected())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //������ ��������� ����
    private void PlaySound(AudioSource audio)
    {
        if (Sound) audio.Play();
    }

    //�������� ����� �����
    IEnumerator ClickWait()
    {
        canClick = false;
        yield return new WaitForSeconds(clickWaitTime);
        canClick = true;
    }

    //��������� ����� ����� �������� ��� ��������
    IEnumerator openScene(string sceneName)
    {
        float fadeTime = mainCamera.GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneName);
    }

}
