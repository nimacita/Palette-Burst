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
    //0 - ����� ������ ��� ��� ��������, 1 - ����� 1 �������� � ��� �����

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

    //�������� �����
    private void SpawnBonus()
    {
        if (IsClaim()) {
            bonus.transform.position = RandomBonusPos();
            bonus.SetActive(true);
        }
    }
    
    //����������� �������
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

    //����� �� ������� �����
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

    //��������� ���������� ������
    private Vector3 RandomBonusPos()
    {
        float randomX = Random.Range(leftBorder.position.x, rightBorder.position.x);
        float randomY = Random.Range(downBorder.position.y,upBorder.position.y);
        Vector3 RandPos = new Vector3(randomX, randomY, bonus.transform.position.z);
        return RandPos;
    }

    //��������� ������
    public void ClaimBonus()
    {
        //�������� �����
        PlayerPrefs.SetInt("Coins", (PlayerPrefs.GetInt("Coins") + bonusValue));
        bonus.SetActive(false);
    }
    
}
