using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardController : MonoBehaviour
{
    
    public static RewardController instanceReward;

    [SerializeField]
    private int moneyLvlReward;
    [SerializeField]
    private int keyLvlReward;

    [SerializeField]
    private int thisLvl;

    void Awake()
    {
        if (!instanceReward)
            instanceReward = this;
        else
            Destroy(this.gameObject);


        DontDestroyOnLoad(this.gameObject);
    }

    //����������� �������� ������
    public int CurrentOpenLvl
    {
        get
        {
            if (PlayerPrefs.HasKey($"currentOpenLvl{CurrentLoc}"))
            {
                return PlayerPrefs.GetInt($"currentOpenLvl{CurrentLoc}");
            }
            else
            {
                PlayerPrefs.SetInt($"currentOpenLvl{CurrentLoc}", 0);
                return 0;
            }
        }
        set
        {
            PlayerPrefs.SetInt($"currentOpenLvl{CurrentLoc}", value);
        }
    }

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

    //������������� �������
    public void SetRewards(int moneyR, int keyR)
    {
        moneyLvlReward = moneyR;
        keyLvlReward = keyR;
    }

    //������� �������� ������
    public int ThisLvl
    {
        get
        {
            return thisLvl;
        }
        set
        {
            this.thisLvl = value;
        }
    }

    //���������� �������� ������� ������
    public int KeyCurrReward
    {
        get { return keyLvlReward; }
    }

    //���������� �������� ������� �����
    public int MoneyCurrReward
    {
        get { return moneyLvlReward; }
    }


}
