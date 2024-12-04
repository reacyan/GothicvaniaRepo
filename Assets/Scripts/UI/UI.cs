using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject InGame_UI;
    [SerializeField] private GameObject dieText;
    [SerializeField] private GameObject RestartButton;
    [SerializeField] private UI_FadeScreen fadescreen;

    public UI_SkillToolTip skillToolTip;
    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;

    private void Awake()
    {
        SwitchTo(skillTreeUI);
    }

    private void Start()
    {
        SwitchTo(InGame_UI);

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
        InGame_UI.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchWithKeyTo(characterUI);
        }

        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            SwitchWithKeyTo(skillTreeUI);
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            SwitchWithKeyTo(craftUI);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            SwitchWithKeyTo(optionsUI);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            characterUI.SetActive(false);
            skillTreeUI.SetActive(false);
            craftUI.SetActive(false);
            optionsUI.SetActive(false);
            InGame_UI.SetActive(true);
        }
    }

    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        InGame_UI.SetActive(false);

        if (_menu != null)
        {
            _menu.SetActive(true);
        }
    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            InGame_UI.SetActive(true);
            return;
        }

        SwitchTo(_menu);
    }

    public void DieEffect()
    {
        fadescreen.FadeOut();

        StartCoroutine(DelayShowDied(2.5f));
    }

    IEnumerator DelayShowDied(float _delay)//����Ч��
    {
        yield return new WaitForSeconds(_delay);

        dieText.SetActive(true);

        yield return new WaitForSeconds(_delay);

        RestartButton.SetActive(true);
    }


    public void RestartGame()//�ص������������Ϸ
    {

        SaveManager.instance.SaveGame();

        fadescreen.FadeIn();

        dieText.SetActive(false);

        RestartButton.SetActive(false);

        GameManager.instance.RestartScene();
    }
}
