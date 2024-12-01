using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum SkillType
{
    dash,
    crystal,
    parry,
    mirage,
    sword,
    dodge
}


public class InGame_UI : MonoBehaviour
{

    [SerializeField] private PlayerStats myStats;
    [SerializeField] private Slider slider;

    private float dashCooldown;
    private float crystalCooldown;
    private float parryCooldown;
    private float mirageCooldown;
    private float swordCooldown;
    private float dodgeCooldown;

    [Header("SkillImage")]
    [SerializeField] private Image dashImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image mirageImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image dodgeImage;

    [SerializeField] private TextMeshProUGUI currentSouls;
    private Dictionary<Image, float> beUsedImageDictionary;

    private void Start()
    {
        beUsedImageDictionary = new Dictionary<Image, float>();
        if (myStats != null)
        {
            myStats.onHealthChanged += UpdateHealthUI;
            myStats.onSkillBeUse += SetSkillBeUsedOf;
        }


        UpdateHealthUI();
    }

    private void Update()
    {
        CheckCooldown();

        currentSouls.text = PlayerManager.instance.GetCurrentCurrency().ToString("#,#");
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
    }

    private void SetSkillBeUsedOf(Sprite _skillSprite, SkillType _skillType, float _cooldown)
    {
        switch (_skillType)//根据技能类型分配图片和cd
        {
            case SkillType.dash:
                dashImage.sprite = _skillSprite;
                dashCooldown = _cooldown;

                if (dashImage.fillAmount <= 0)
                {
                    dashImage.fillAmount = 1;
                }
                beUsedImageDictionary.Add(dashImage, dashCooldown);
                break;
            case SkillType.mirage:
                mirageImage.sprite = _skillSprite;
                mirageCooldown = _cooldown;

                if (mirageImage.fillAmount <= 0)
                {
                    mirageImage.fillAmount = 1;
                }
                beUsedImageDictionary.Add(mirageImage, mirageCooldown);
                break;
            case SkillType.parry:
                parryImage.sprite = _skillSprite;
                parryCooldown = _cooldown;

                if (parryImage.fillAmount <= 0)
                {
                    parryImage.fillAmount = 1;
                }
                beUsedImageDictionary.Add(parryImage, parryCooldown);
                break;
            case SkillType.sword:
                swordImage.sprite = _skillSprite;
                swordCooldown = _cooldown;

                if (swordImage.fillAmount <= 0)
                {
                    swordImage.fillAmount = 1;
                }
                beUsedImageDictionary.Add(swordImage, swordCooldown);
                break;
            case SkillType.dodge:
                dodgeImage.sprite = _skillSprite;
                dodgeCooldown = _cooldown;

                if (dodgeImage.fillAmount <= 0)
                {
                    dodgeImage.fillAmount = 1;
                }
                beUsedImageDictionary.Add(dodgeImage, dodgeCooldown);
                break;
        }
    }


    private void CheckCooldown()
    {
        Dictionary<Image,float> CooldownDictionary = new Dictionary<Image,float>();

        foreach (var skill in beUsedImageDictionary)
        {
            if (skill.Key.fillAmount > 0)
            {
                skill.Key.fillAmount -= 1 / skill.Value * Time.deltaTime;
            }
            else
            {
                CooldownDictionary.Add(skill.Key, skill.Value);
            }
        }

        foreach (var overcooldown in CooldownDictionary)
        {
            beUsedImageDictionary.Remove(overcooldown.Key);
        }
    }
}
