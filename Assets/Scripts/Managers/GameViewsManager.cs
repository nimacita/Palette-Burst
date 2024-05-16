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
    [Tooltip("количество монет за которое дается ключ")]
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

    //начальные настройки игрвого экрана
    private void StartViewSettings()
    {
        gameView.SetActive(true);
        pauseView.SetActive(false);
        defeatView.SetActive(false);
        victoryView.SetActive(false);
        pause = false;
    }

    //сохраненное значение ключей
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

    //сохраненное значение монет
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

    //сохраненное значение музыки
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

    //сохраненное значение звуков
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

    //сохраненное значение таблицы рекордов
    private void UpdateHightScoreTable(int currScore = 0)
    {
        //обнволяем таблиуц рекордов, если рекорд больше чем значения в таблице

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

    //публичный метод конца игры
    public IEnumerator GameOver()
    {
        //после небольшой задержки
        gameEnd = true;
        pauseView.SetActive(false);
        yield return new WaitForSeconds(1);
        //запускакем сцену конца игры
        if (coinScore <= 0)
        {
            defeatView.SetActive(true);
        }
        else
        {
            victoryView.SetActive(true);
            //сохраняем
            KeyReward += coinScore / keyToCoins;
            CoinsReward += coinScore;
            //выводим
            keyRewardTxt.text = $"X{coinScore / keyToCoins}";
            coinsRewardTxt.text = $"X{coinScore}";
        }
        //обвноялем таблицу рекордов
        UpdateHightScoreTable(score);
    }

    //возвращение в меню
    private IEnumerator BackToCurrScene(string sceneName)
    {
        //после 1 секунды включаем потухающий эффект
        float fadeTime = mainCamera.GetComponent<Fading>().BeginFade(1);
        //после эффекта загружаем меню
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneName);
    }

    //обновляем спрайыт кнопок
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

    //нажатие кнопки выхода
    public void HomeBtnClick()
    {
        Time.timeScale = 1;
        StartCoroutine(BackToCurrScene("main menu"));
    }

    //нажатие кнопки рестарта
    public void RestartBtnClick()
    {
        StartCoroutine(BackToCurrScene("RoundBall"));
    }

    //получение рекорда
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

    //получение денежного рекорда
    public void SetCoinsCore(int value)
    {
        coinScore = value;
    }

    //экран паузы
    private void PauseViewActive()
    {
        UpdateBtnSprite();
        pauseScoreTxt.text = $"YOUR CURRENT SCORE: {score}";
        pauseView.SetActive(true);
        pause = true;
    }

    //пауза
    public void PauseButton()
    {
        //останавливаем игру
        if (!gameEnd) {
            PauseViewActive();
            Time.timeScale = 0;
        }
    }

    //кнопка музыки
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

    //кнопка звуков
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

    //выход с паузы
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
