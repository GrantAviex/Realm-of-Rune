using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Requirement : MonoBehaviour 
{
    public Image Border;
    public Image Skill;
    public Image Tier;

    public void SetSprites(LevelReq req)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(true);
        }
        string path = "Sprites/Skill Icons/";
        string skillName = "";
        switch (req.skill)
        {
            case (int)SkillIDs.Melee:
                skillName = "Melee";
                break;
            case (int)SkillIDs.Ranged:
                skillName = "Ranged";
                break;
            case (int)SkillIDs.Magic:
                skillName = "Magic";
                break;
            case (int)SkillIDs.Woodcutting:
                skillName = "Woodcutting";
                break;
            case (int)SkillIDs.Mining:
                skillName = "Mining";
                break;
            case (int)SkillIDs.Fishing:
                skillName = "Fishing";
                break;
            case (int)SkillIDs.Hunting:
                skillName = "Hunting";
                break;
            case (int)SkillIDs.Capturing:
                skillName = "Capturing";
                break;
            case (int)SkillIDs.Shipbuilding:
                skillName = "Shipbuilding";
                break;
            case (int)SkillIDs.Smithing:
                skillName = "Smithing";
                break;
            case (int)SkillIDs.Cooking:
                skillName = "Cooking";
                break;
            case (int)SkillIDs.Crafting:
                skillName = "Crafting";
                break;
            case (int)SkillIDs.Apothecary:
                skillName = "Apothecary";
                break;
            case (int)SkillIDs.Theurgy:
                skillName = "Theurgy";
                break;
            case (int)SkillIDs.Fletching:
                skillName = "Fletching";
                break;
        }

        Sprite skillIcon = Resources.Load<Sprite>(path + skillName);
        Skill.sprite = skillIcon;
        string borderName = "";
        switch ((req.level /5) +1)
        {
            case 1:
                {
                    borderName = "Noob";
                    break;
                }
            case 2:
                {
                    borderName = "Apprentice";
                    break;
                }
            case 3:
                {
                    borderName = "Novice";
                    break;
                }
            case 4:
                {
                    borderName = "Journeyman";
                    break;
                }
            case 5:
                {
                    borderName = "Expert";
                    break;
                }
            case 6:
                {
                    borderName = "Master";
                    break;
                }
            case 7:
                {
                    borderName = "GrandMaster";
                    break;
                }
        }
        Sprite borderIcon = Resources.Load<Sprite>(path + "Badge" + borderName);
        Border.sprite = borderIcon;
    }
}
