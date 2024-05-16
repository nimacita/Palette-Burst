using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameViewsManager : MonoBehaviour
{

    private int score;
    private int coinScore;

    [Header("Game View Settings")]
    [SerializeField]
    private GameObject gameView;
    [SerializeField]
    private TMPro.TMP_Text hightScoreTxt;
    private int hightScore;
    [SerializeField]
    private GameObject mainCamera;

    [Space]
    [Header("Pause View Settings")]
    [SerializeField]
    private GameObject pauseView;
    [SerializeField]
    private TMPro.TMP_Text pauseScoreTxt;
    [SerializeField]
    private Image musicBtn, soundBtn;
    [SerializeField]
    private Sprite musicOn, musicOff;
    [SerializeField]
    private Sprite soundOn, soundOff;
    private bool pause;

    [Space]
    [Header("End Game View Settings")]
    [SerializeField]
    private GameObject defeatView;
    private bool gameEnd;
    [SerializeField]
    private GameObject victoryView;
    [SerializeField]
    [Tooltip("���������� ����� �� ������� ������ ����")]
    private int keyToCoins;
    [SerializeField]
    private TMPro.TMP_Text keyRewardTxt, coinsRewardTxt;

    void Start()
    {
        pause = false;
        gameEnd = false;
        StartViewSettings();
        UpdateHightScoreTable();
        SetHightScore();
    }

    //��������� ��������� ������� ������
    private void StartViewSettings()
    {
        gameView.SetActive(true);
        pauseView.SetActive(false);
        defeatView.SetActive(false);
        victoryView.SetActive(false);
        pause = false;
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

    //����������� �������� �����
    private int CoinsReward
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

    //����������� �������� ������
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

    //����������� �������� ������
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

    //����������� �������� ������� ��������
    private void UpdateHightScoreTable(int currScore = 0)
    {
        //��������� ������� ��������, ���� ������ ������ ��� �������� � �������

        for (int i = 0; i < 5; i++) 
        {
            if (PlayerPrefs.HasKey($"HightScoreTable{i}"))
            {
                if (currScore > PlayerPrefs.GetInt($"HightScoreTable{i}"))
                {
                    if (PlayerPrefs.GetInt($"HightScoreTable{i}") > 0) 
                    {
                        int lastScore = PlayerPrefs.GetInt($"HightScoreTable{i}");
                        PlayerPrefs.SetInt($"HightScoreTable{i}", currScore);
                        currScore = 0;
                        UpdateHightScoreTable(lastScore);
                        return;
                    }
                    else
                    {
                        PlayerPrefs.SetInt($"HightScoreTable{i}", currScore);
                        currScore = 0;
                        return;
                    }
                }
            }
            else
            {
                PlayerPrefs.SetInt($"HightScoreTable{i}", 0);
            }
        }
    }

    //��������� ����� ����� ����
    public IEnumerator GameOver()
    {
        //����� ��������� ��������
        gameEnd = true;
        pauseView.SetActive(false);
        yield return new WaitForSeconds(1);
        //���������� ����� ����� ����
        if (coinScore <= 0)
        {
            defeatView.SetActive(true);
        }
        else
        {
            victoryView.SetActive(true);
            //���������
            KeyReward += coinScore / keyToCoins;
            CoinsReward += coinScore;
            //�������
            keyRewardTxt.text = $"X{coinScore / keyToCoins}";
            coinsRewardTxt.text = $"X{coinScore}";
        }
        //��������� ������� ��������
        UpdateHightScoreTable(score);
    }

    //����������� � ����
    private IEnumerator BackToCurrScene(string sceneName)
    {
        //����� 1 ������� �������� ���������� ������
        float fadeTime = mainCamera.GetComponent<Fading>().BeginFade(1);
        //����� ������� ��������� ����
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneName);
    }

    //��������� ������� ������
    private void UpdateBtnSprite()
    {
        if (Music)
        {
            musicBtn.sprite = musicOn;
        }
        else
        {
            musicBtn.sprite = musicOff;
        }

        if (Sound)
        {
            soundBtn.sprite = soundOn;
        }
        else
        {
            soundBtn.sprite = soundOff;
        }
    }

    //������� ������ ������
    public void HomeBtnClick()
    {
        Time.timeScale = 1;
        StartCoroutine(BackToCurrScene("main menu"));
    }

    //������� ������ ��������
    public void RestartBtnClick()
    {
        StartCoroutine(BackToCurrScene("RoundBall"));
    }

    //��������� �������
    public void SetScore(int value)
    {
        score = value;
        SetHightScore();
    }

    private void SetHightScore()
    {
        if (score > PlayerPrefs.GetInt($"HightScoreTable{0}"))
        {
            hightScore = score;
        }
        else
        {
            hightScore = PlayerPrefs.GetInt($"HightScoreTable{0}");
        }
        hightScoreTxt.text = $"Best:{hightScore}";
    }

    //��������� ��������� �������
    public void SetCoinsCore(int value)
    {
        coinScore = value;
    }

    //����� �����
    private void PauseViewActive()
    {
        UpdateBtnSprite();
        pauseScoreTxt.text = $"YOUR CURRENT SCORE: {score}";
        pauseView.SetActive(true);
        pause = true;
    }

    //�����
    public void PauseButton()
    {
        //������������� ����
        if (!gameEnd) {
            PauseViewActive();
            Time.timeScale = 0;
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

    //����� � �����
    public void BackToPlay()
    {
        if (pause)
        {
            pauseView.SetActive(false);
            pause = false;
            Time.timeScale = 1;
        }
    }
}
