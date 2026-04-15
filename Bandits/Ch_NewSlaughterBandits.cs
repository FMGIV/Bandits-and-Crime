#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Shadows of Forbidden Gods\ShadowsOfForbiddenGods_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion
using Assets.Code;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bandits_and_Crime
{
    public class Ch_NewSlaughterBandits : Ch_SlaughterBandits
    {
        public Pr_New_Banditry banditry;
        public Ch_NewSlaughterBandits(Pr_New_Banditry banditry, Location loc)
            : base(loc)
        {
            this.banditry = banditry;
        }

        public override string getName()
        {
            return "Slaughter Bandits";
        }

        public override string getDesc()
        {
            return "Remove the banditry modifier in this location.";
        }

        public override string getCastFlavour()
        {
            return "There are times when otherwise useful instruments serve no purpose, when what could have been an effective tool finds itself mis-placed, and must be discarded.";
        }

        public override double getProfile()
        {
            foreach (Property property in base.location.properties)
            {
                if (property is Pr_New_Banditry)
                {
                    return property.charge;
                }
            }

            return 0.0;
        }

        public override double getMenace()
        {
            return map.param.ch_slaughterbandits_aiMenace;
        }

        public override int getCompletionProfile()
        {
            return map.param.ch_slaughterbandits_completionProfile;
        }

        public override int getInherentDanger()
        {
            return 2;
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.MIGHT;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            msgs?.Add(new ReasonMsg("Stat: Might", Math.Max(1, unit.getStatMight())));
            return Math.Max(1, unit.getStatMight());
        }

        public override double getComplexity()
        {
            return map.param.ch_slaughterbandits_complexity;
        }

        public override bool validFor(UA ua)
        {
            return true;
        }

        public override Sprite getSprite()
        {
            return map.world.iconStore.banditry;
        }

        public override int isGoodTernary()
        {
            return -1;
        }

        public override void complete(UA u)
        {
            location.properties.Remove(banditry);
        }

        public override bool valid()
        {
            return true;
        }

        public override int[] buildPositiveTags()
        {
            return new int[3]
            {
                Tags.COMBAT,
                Tags.CRUEL,
                Tags.DANGER
            };
        }

        public override int[] buildNegativeTags()
        {
            return new int[0];
        }
    }
}