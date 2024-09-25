using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    #region Altar

    private Button _skillTree;
    private Button _fastTravelScreen;
    private Button _rest;
    private Button _back;

    //Skill Tree Variables
    private Button _skill1Btn;
    private Button _skill2Btn;
    private Button _skill3Btn;
    private Button _skillClearBtn;

    //Fast Travel Variables
    private Button _fastTravelPoint1;
    private Button _fastTravelPoint2;

    //Altar Page Control
    private bool _skillTreeOpen;
    private bool _fastTravelOpen;

    public Image _keyPrompt;

    public bool keyPressed;
    public bool keySecondPress;

    #endregion

    #region Key Items
    List<Image> _keyItemImageList = new List<Image>();
    #endregion

    #region Boss Information

    private bool _bossInfoOpen;
    private Image _bossBG;
    private Button _bossInfoPage;

    [SerializeField] private GameObject boss1Info;
    [SerializeField] private GameObject boss2Info;

    [SerializeField] private GameObject bossPfp1;
    [SerializeField] private GameObject bossPfp2;
    #endregion

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        GetUIItems();
    }

    private void Start()
    {
        for (int i = 0; i < _keyItemImageList.Count; i++)
        {
            _keyItemImageList[i].sprite = null;
            _keyItemImageList[i].gameObject.SetActive(false);
        }
        OnExitUIAltar();
    }

    #region Altar Functions

    public void OnRest()
    {
        GameManager.instance._playerHealth.HealUnit(GameManager.instance._playerHealth.MaxHealth);
    }
    public void OnKeyPressed()
    {
        keyPressed = true;

        _keyPrompt.gameObject.SetActive(false);

        _rest.gameObject.SetActive(true);
        _skillTree.gameObject.SetActive(true);
        _fastTravelScreen.gameObject.SetActive(true);
        _bossInfoPage.gameObject.SetActive(true);


        _back.gameObject.SetActive(true);
    }

    public void OnExitUIAltar()
    {
        _skillTreeOpen = false;
        _fastTravelOpen = false;

        keyPressed = false;
        keySecondPress = false;
        _keyPrompt.gameObject.SetActive(false);

        _rest.gameObject.SetActive(false);
        _skillTree.gameObject.SetActive(false);
        _fastTravelScreen.gameObject.SetActive(false);
        _bossBG.gameObject.SetActive(false);
        _bossInfoPage.gameObject.SetActive(false);

        _skill1Btn.gameObject.SetActive(false);
        _skill2Btn.gameObject.SetActive(false);
        _skill3Btn.gameObject.SetActive(false);
        _skillClearBtn.gameObject.SetActive(false);

        _fastTravelPoint1.gameObject.SetActive(false);
        _fastTravelPoint2.gameObject.SetActive(false);


        _back.gameObject.SetActive(false);
    }

    public void OnExitUIWithKeyAltar()
    {
        _skillTreeOpen = false;
        _fastTravelOpen = false;

        keyPressed = false;
        keySecondPress = false;
        _keyPrompt.gameObject.SetActive(true);

        _rest.gameObject.SetActive(false);
        _skillTree.gameObject.SetActive(false);
        _fastTravelScreen.gameObject.SetActive(false);
        _bossBG.gameObject.SetActive(false);
        _bossInfoPage.gameObject.SetActive(false);

        _skill1Btn.gameObject.SetActive(false);
        _skill2Btn.gameObject.SetActive(false);
        _skill3Btn.gameObject.SetActive(false);
        _skillClearBtn.gameObject.SetActive(false);

        _fastTravelPoint1.gameObject.SetActive(false);
        _fastTravelPoint2.gameObject.SetActive(false);

        _back.gameObject.SetActive(false);
    }

    public void OnSkillTreePressed()
    {
        _skillTreeOpen = true;

        _rest.gameObject.SetActive(false);
        _skillTree.gameObject.SetActive(false);
        _fastTravelScreen.gameObject.SetActive(false);
        _bossInfoPage.gameObject.SetActive(false);

        _skill1Btn.gameObject.SetActive(true);
        _skill2Btn.gameObject.SetActive(true);
        _skill3Btn.gameObject.SetActive(true);
        _skillClearBtn.gameObject.SetActive(true);

        _back.gameObject.SetActive(true);
    }

    public void OnFastTravelScreen()
    {
        _fastTravelOpen = true;

        _rest.gameObject.SetActive(false);
        _skillTree.gameObject.SetActive(false);
        _fastTravelScreen.gameObject.SetActive(false);
        _bossInfoPage.gameObject.SetActive(false);

        _fastTravelPoint1.gameObject.SetActive(true);
        _fastTravelPoint2.gameObject.SetActive(true);

        _back.gameObject.SetActive(true);
    }

    public void OnBossInfo()
    {
        _bossInfoOpen = true;

        _rest.gameObject.SetActive(false);
        _skillTree.gameObject.SetActive(false);
        _fastTravelScreen.gameObject.SetActive(false);

        _bossBG.gameObject.SetActive(true);

        _back.gameObject.SetActive(true);
    }

    #endregion

    #region Key Item Functions

    public void ShowKeyItems(string areaName)
    {
        for(int i = 0; i < _keyItemImageList.Count; i++)
        {
            _keyItemImageList[i].sprite = null;
            _keyItemImageList[i].gameObject.SetActive(false);
        }

        if(PlayerController.instance != null && PlayerController.instance.keyItems.Count > 0)
        {
            int counter = 0;
            foreach (KeyItem item in PlayerController.instance.keyItems)
            {
                if (areaName == item.whereItBelongs)
                {
                    _keyItemImageList[counter].gameObject.SetActive(true);
                    _keyItemImageList[counter].sprite = item.itemIcon;
                    counter++;
                }

            }
        }
    }

    #endregion

    public void OnBackPressed()
    {
        if (_skillTreeOpen)
        {
            _skillTreeOpen = false;

            _skill1Btn.gameObject.SetActive(false);
            _skill2Btn.gameObject.SetActive(false);
            _skill3Btn.gameObject.SetActive(false);
            _skillClearBtn.gameObject.SetActive(false);

            OnKeyPressed();
        }
        else if (_fastTravelOpen)
        {
            _fastTravelOpen = false;

            _fastTravelPoint1.gameObject.SetActive(false);
            _fastTravelPoint2.gameObject.SetActive(false);

            OnKeyPressed();
        }
        else if(_bossInfoOpen)
        {
            _bossInfoOpen = false;

            _bossBG.gameObject.SetActive(false);

            OnKeyPressed();
        }
    }

    private void GetUIItems()
    {
        _skillTree = GameObject.Find("SkillTree").GetComponent<Button>();

        _fastTravelScreen = GameObject.Find("FastTravel").GetComponent<Button>();
        _fastTravelPoint1 = GameObject.Find("FastTravelButton1").GetComponent<Button>();
        _fastTravelPoint2 = GameObject.Find("FastTravelButton2").GetComponent<Button>();

        _rest = GameObject.Find("Rest").GetComponent<Button>();
        _back = GameObject.Find("Back").GetComponent<Button>();

        _skill1Btn = GameObject.Find("AddSkill1").GetComponent<Button>();
        _skill2Btn = GameObject.Find("AddSkill2").GetComponent<Button>();
        _skill3Btn = GameObject.Find("AddSkill3").GetComponent<Button>();
        _skillClearBtn = GameObject.Find("ClearSkills").GetComponent<Button>();

        _keyPrompt = GameObject.Find("KeyPrompt").GetComponent<Image>();

        for (int i = 1; i <= 8; i++)
        {
            _keyItemImageList.Add(GameObject.Find("KeyItemImage" + i).GetComponent<Image>());
        }

        _bossBG = GameObject.Find("BossInfoBackground").GetComponent<Image>();
        _bossInfoPage = GameObject.Find("Boss Info").GetComponent<Button>();

    }

    

    #region Boss Information Page

    public void Boss1Info()
    {
        bossPfp2.SetActive(false);

        boss1Info.SetActive(true);
    }

    public void Boss2Info()
    {
        bossPfp1.SetActive(false);

        boss2Info.SetActive(true);
    }

    public void OnBossInfoBack()
    {
        bossPfp1.SetActive(true);
        bossPfp2.SetActive(true);

        boss1Info.SetActive(false);
        boss2Info.SetActive(false);
    }

    #endregion
}
