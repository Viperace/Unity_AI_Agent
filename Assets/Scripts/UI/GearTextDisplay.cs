using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearTextDisplay
{
    BasicGear basicGear;

    public GearTextDisplay() { }

    public GearTextDisplay(BasicGear basicGear) 
    {
        this.basicGear = basicGear;
    } 

    string ColorToHex(Color color)
    {
        return ColorUtility.ToHtmlStringRGB(color);
    }

    public string ColoredName()
    {
        Color[] palettes = ColorPalette.Instance.ItemTextTheme;
        string legendaryColor = ColorToHex(palettes[0]);
        string commonColor = ColorToHex(palettes[1]);
        string exceptionalColor = ColorToHex(palettes[2]);

        switch (basicGear.rarity)
        {
            case Rarity.EXCEPTIONAL:
                return string.Concat("<color=#", exceptionalColor, ">", basicGear.name, "</color>");
            case Rarity.LEGENDARY:
                return string.Concat("<color=#", legendaryColor, ">", basicGear.name , "</color>");
            default:
                return string.Concat("<color=#", commonColor, ">", basicGear.name, "</color>");
        }
    }

    public string ColoredAttack()
    {
        string bonusText = "";
        if (basicGear.attackBonus != 0)
            bonusText = string.Concat(" (", (basicGear.attackBonus * 100).ToString(), "%)");

        return string.Concat(basicGear.attack, bonusText);        
    }

    public string ColoredDefend()
    {
        string bonusText = "";
        if (basicGear.defendBonus != 0)
            bonusText = string.Concat(" (", (basicGear.defendBonus * 100).ToString(), "%)");

        return string.Concat(basicGear.defend, bonusText);
    }

    // Combining Atk and Def
    public string ColoredStat()
    {
        string attackText = "";
        if (basicGear.attack != 0 | basicGear.attackBonus != 0)
        {
            string bonusAtkText = "";
            if (basicGear.attackBonus != 0)
                bonusAtkText = string.Concat(" (", (basicGear.attackBonus * 100).ToString(), "%)");
            attackText = string.Concat("Atk:", basicGear.attack, bonusAtkText, "\n");
        }

        string defendText = "";
        if (basicGear.defend != 0 | basicGear.defendBonus != 0)
        {
            string bonusDefText = "";
            if (basicGear.defendBonus != 0)
                bonusDefText = string.Concat(" (", (basicGear.defendBonus * 100).ToString(), "%)");
            defendText = string.Concat("Def:", basicGear.defend, bonusDefText);
        }

        return string.Concat(attackText, defendText);
    }
}
