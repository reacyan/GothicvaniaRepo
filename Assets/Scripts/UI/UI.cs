using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] public GameObject characterUI;
    [SerializeField] public GameObject skillTreeUI;
    [SerializeField] public GameObject craftUI;
    [SerializeField] public GameObject optionsUI;

    [SerializeField] public UI_ItemToolTip itemToolTip;
    [SerializeField] public UI_StatToolTip statToolTip;

    private void Start()
    {
        SwitchTo(null);

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchTo(characterUI);
        }

        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            SwitchTo(skillTreeUI);
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            SwitchTo(craftUI);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            SwitchTo(optionsUI);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            characterUI.SetActive(false);
            skillTreeUI.SetActive(false);
            craftUI.SetActive(false);
            optionsUI.SetActive(false);
        }
    }

    public virtual void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

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
            return;
        }

        SwitchTo(_menu);
    }
}
