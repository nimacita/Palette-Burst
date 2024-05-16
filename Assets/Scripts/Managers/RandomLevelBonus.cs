using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLevelBonus : MonoBehaviour
{

    [SerializeField]
    [Header("Bonus")]
    private GameObject bonus;
    [SerializeField]
    private int bonusValue;

    [Space]
    [SerializeField]
    [Header("Bonus CoolDown")]
    private int coolDown;
    //0 - брать каждый раз при открытии, 1 - через 1 открытие и так далее

    [Space]
    [SerializeField]
    [Header("Borders")]
    private Transform upBorder;
    [SerializeField]
    private Transform downBorder, leftBorder, rightBorder;

    void Start()
    {
        bonus.SetActive(false);
        SpawnBonus();
    }

    //включаем бонус
    private void SpawnBonus()
    {
        if (IsClaim()) {
            bonus.transform.position = RandomBonusPos();
            bonus.SetActive(true);
        }
    }
    
    //колличество заходов
    private int ClaimCount
    {
        get
        {
            if (PlayerPrefs.HasKey("claimCount"))
            {
                return PlayerPrefs.GetInt("claimCount");
            }
            else
            {
                PlayerPrefs.SetInt("claimCount", coolDown);
                return coolDown;
            }
        }
        set
        {
            PlayerPrefs.SetInt("claimCount", value);
        }
    }

    //можем ли собрать бонус
    private bool IsClaim()
    {
        if (coolDown < 0)
        {
            coolDown = 0;
        }
        if (ClaimCount == coolDown)
        {
            ClaimCount = 0;
            return true;
        }
        else
        {
            ClaimCount+=1;
            return false;
        }
    }

    //рандомные координаты бонуса
    private Vector3 RandomBonusPos()
    {
        float randomX = Random.Range(leftBorder.position.x, rightBorder.position.x);
        float randomY = Random.Range(downBorder.position.y,upBorder.position.y);
        Vector3 RandPos = new Vector3(randomX, randomY, bonus.transform.position.z);
        return RandPos;
    }

    //собирание бонуса
    public void ClaimBonus()
    {
        //собираем бонус
        PlayerPrefs.SetInt("Coins", (PlayerPrefs.GetInt("Coins") + bonusValue));
        bonus.SetActive(false);
    }
    
}
