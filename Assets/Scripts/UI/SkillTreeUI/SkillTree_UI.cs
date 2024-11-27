using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTree_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
    }

    public void UnlockSkillSlot()
    {
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (!shouldBeUnlocked[i].unlocked)
            {
                Debug.Log("cannot unlock skill");
                return;
            }
        }

        for (int i = 0; i < shouldBelocked.Length; i++)
        {
            if (shouldBelocked[i].unlocked)
            {
                Debug.Log("cannot unlock skill");
                return;
            }
        }

        if (PlayerManager.instance.player.GetComponent<PlayerStats>().HaveEnoughMoney(skillCost))
        {
            unlocked = true;
            skillImage.color = Color.white;
            Debug.Log("Unlock skill");
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
}
