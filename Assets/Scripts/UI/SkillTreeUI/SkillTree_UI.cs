using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTree_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int Price;
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

        if (PlayerManager.instance.player.GetComponent<PlayerStats>().HaveEnoughMoney(Price))
        {
            unlocked = true;
            skillImage.color = Color.white;
            Debug.Log("Unlock skill");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowSkillToolTip(skillDescription, skillName);

        Vector2 mousePosition = Input.mousePosition;

        float xOffset = 0;
        float yOffset = 0;

        if (mousePosition.x > Screen.width / 2)
        {
            xOffset = -75;
        }
        else
        {
            xOffset = 75;
        }

        if (mousePosition.y > Screen.height / 2)
        {
            yOffset = -75;
        }
        else
        {
            yOffset = 75;
        }

        ui.skillToolTip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideSkillToolTip();
    }
}
