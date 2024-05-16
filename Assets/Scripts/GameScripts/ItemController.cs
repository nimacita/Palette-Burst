using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class ItemController : MonoBehaviour
{

    [Header("Item Settings")]
    [SerializeField]
    private Sprite itemSprite;
    [SerializeField]
    private int itemI = -1, itemJ = -1;
    [SerializeField]
    private bool isSelected = false;

    //To Move
    private bool isMove = false;
    private Vector3 targetPos;
    private float itemSpeed;

    [Header("Editor")]
    public GameObject itemSkin;
    public MainGameController mainGameController;
    private Animation itemAnim;


    void Start()
    {
        itemAnim = GetComponent<Animation>();
        GetComponent<Button>().onClick.AddListener(ItemClick);
    }

    private void Update()
    {
        GoMove();
    }

    //нажатие на айтем
    private void ItemClick()
    {
        mainGameController.ItemClick(itemI, itemJ);
    }

    //играем частицы
    public void PlayParticlesAndAnim()
    {
        //particle.Play();
        itemAnim.Play("ItemDeleteAnim");
    }

    //двигаем объект
    public void MoveItem(Vector3 target,float speed)
    {
        targetPos = target;
        itemSpeed = speed;
        isMove = true;
    }

    private void GoMove()
    {
        if (isMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, itemSpeed * Time.deltaTime);
        }
        if (transform.position.y == targetPos.y)
        {
            targetPos = Vector3.zero;
            itemSpeed = 0f;
            isMove = false;
        }
    }

    //возвращаем позицию айтема
    public Vector3 GetItemPosition()
    {
        if (targetPos != Vector3.zero)
        {
            return targetPos;
        }
        else
        {
            return transform.position;
        }
    }

    //устаналиваем текущие координаты
    public void SetItemCoord(int currI,int currJ)
    {
        itemI = currI;
        itemJ = currJ;
    }

    //устанавливаем текущий цвет
    public void SetItemSprite(Sprite currSprite, Sprite skinBg)
    {
        GetComponent<Image>().sprite = skinBg;
        itemSprite = currSprite;
        itemSkin.GetComponent<Image>().sprite = itemSprite;
    }

    public int GetItemI()
    {
        return itemI;
    }

    public int GetItemJ()
    {
        return itemJ;
    }

    //устанавливаем значение выделенности
    public void SetSelected(bool value)
    {
        isSelected = value;
    }

    //возвращаем значение выделенности
    public bool GetSelected()
    {
        return isSelected;
    }

    //возвращаем текущий цвет
    public Sprite GetItemSprite()
    {
        return itemSprite;
    }
}
