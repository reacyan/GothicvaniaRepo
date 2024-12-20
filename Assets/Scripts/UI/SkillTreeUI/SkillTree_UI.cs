using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;


public class SkillTree_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,ISaveManager
{
    [SerializeField] private int skillCost;
    [SerializeField] private string skillName;

    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private SkillTree_UI[] shouldBeUnlocked;
    [SerializeField] private SkillTree_UI[] shouldBelocked;
    [SerializeField] private Color lockedSkillColor;

    public bool unlocked;

    private Image skillImage;
    private UI ui;

    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    private void Start()
    {
        ui = GetComponentInParent<UI>();

        skillImage = GetComponent<Image>();

        skillImage.color = lockedSkillColor;

        LoadedSkill();
    }

    public void UnlockSkillSlot()
    {
        foreach (var unlocked in shouldBeUnlocked)
        {
            if (!unlocked.unlocked)
            {
                return;
            }
        }

        foreach (var locked in shouldBelocked)
        {
            if (locked.unlocked)
            {
                return;
            }
        }

        if (PlayerManager.instance.HaveEnoughMoney(skillCost))
        {
            unlocked = true;
            skillImage.color = Color.white;
        }
    }

    private void LoadedSkill()
    {
        if (unlocked ==true)
        {
            skillImage.color = Color.white;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowSkillToolTip(skillDescription, skillName,skillCost);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideSkillToolTip();
    }

    public void LoadData(GameData _data)
    {
        if(_data.skillTree.TryGetValue(skillName, out bool value))
        {
            unlocked = value;
        }
    }

    public void SaveData(ref GameData _data)
    {
        if(_data.skillTree.TryGetValue(skillName, out bool value))
        {
            _data.skillTree.Remove(skillName);
            _data.skillTree.Add(skillName, unlocked);
        }
        else
        {
            _data.skillTree.Add(skillName, unlocked);
        }
    }
}
