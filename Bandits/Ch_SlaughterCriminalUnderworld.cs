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
    public class Ch_SlaughterCriminalUnderworld : Ch_SlaughterBandits
    {
        public Pr_Crime crime;
        public Ch_SlaughterCriminalUnderworld(Pr_Crime crime, Location loc)
            : base(loc)
        {
            this.crime = crime;
        }

        public override string getName()
        {
            return "Slaughter Criminals";
        }

        public override string getDesc()
        {
            return "Remove the Crime modifier in this location.";
        }

        public override string getCastFlavour()
        {
            return "There are times when otherwise useful instruments serve no purpose, when what could have been an effective tool finds itself mis-placed, and must be discarded.";
        }

        public override double getProfile()
        {
            foreach (Property property in base.location.properties)
            {
                if (property is Pr_Crime)
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
            return challengeStat.INTRIGUE;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            msgs?.Add(new ReasonMsg("Stat: Intrigue", Math.Max(1, unit.getStatIntrigue())));
            return Math.Max(1, unit.getStatIntrigue());
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
            return map.world.iconStore.silentAssassin;
        }

        public override int isGoodTernary()
        {
            return -1;
        }

        public override void complete(UA u)
        {
           location.properties.Remove(crime);
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
            return new int[1]
            {
                Tags.DISCORD
            };
        }
    }
}