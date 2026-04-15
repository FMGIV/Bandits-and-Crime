#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Shadows of Forbidden Gods\ShadowsOfForbiddenGods_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code;

public class Pr_New_Banditry : Pr_Bandity
{
    public Challenge challenge;

    public List<Challenge> challenges = new List<Challenge>();

    public Pr_New_Banditry(Location loc)
        : base(loc)
    {
        challenge = new Ch_CombatBanditry(loc);
        challenges.Add(challenge);
        challenge = new Ch_ArmBandits(loc);
        challenges.Add(challenge);
        challenges.Add(new Ch_SlaughterBandits(loc));
    }

    public override void onTurnStart(Map map)
    {
        foreach (Location loc in map.locations)
        {
            if (loc.settlement is SettlementHuman)
            {
                for (int i = 0; i < loc.properties.Count; i++)
                {
                    if (loc.properties[i] is Pr_Bandity oldBanditry && loc.properties[i] is Pr_New_Banditry == false)
                    {
                        Pr_New_Banditry newBanditry = new Pr_Bandity(loc);
                        newBanditry.charge = oldBanditry.charge;
                        newBanditry.influences = oldBanditry.influences;
                        loc.properties[i] = newBanditry;
                    }
                }
            }
        }
    }

    public override string getName()
    {
        return "Banditry";
    }

    public override void turnTick()
    {   
        if (charge < 75.0)
        {
            influences.Add(new ReasonMsg("Constant Increase", 2.0));
        }
    }

    public override string getDesc()
    {
        return "Bandits have started raiding the lanes and trails near this location, and will require a hero to remove. While they remain, they decrease this location's <b>prosperity</b>.";
    }

    public override bool canTriggerCrisis()
    {
        return false;
    }

    public override double getProsperityInfluence()
    {
        return -0.25;
    }

    public override List<Challenge> getChallenges()
    {
        return challenges;
    }

    public override Sprite getSprite(World world)
    {
        return world.iconStore.banditry;
    }

    public override standardProperties getPropType()
    {
        return standardProperties.OTHER;
    }
}