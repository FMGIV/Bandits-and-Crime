#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Shadows of Forbidden Gods\ShadowsOfForbiddenGods_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion
using Assets.Code;
using System;
using UnityEngine;

namespace Bandits_and_Crime
{
    public class Pr_Stolen_Food : Pr_Famine
    {

        public Pr_Stolen_Food(Location loc)
        : base(loc)
        {
        }


        public override string getName()
        {
            return "Stolen Food";
        }

        public override void turnTick()
        {
            influences.Add(new ReasonMsg("Natural Decay", -1.0));
        }

        public override string getDesc()
        {
            return "Bandits have stolen <b>food</b> from this location.";
        }

        public override double foodGenMult()
        {
            return Math.Max(0.1, 1.0 - charge / 200.0);
        }

        public override string getCrisis()
        {
            return "";
        }

        public override bool canTriggerCrisis()
        {
            return false;
        }

        public override Sprite getSprite(World world)
        {
            return world.iconStore.famine;
        }

        public override standardProperties getPropType()
        {
            return standardProperties.OTHER;
        }
    }
}