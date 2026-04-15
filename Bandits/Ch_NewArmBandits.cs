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
    public class Ch_NewArmBandits : Ch_ArmBandits
    {
        public Pr_New_Banditry Bandits;

        public int dangerincrease = 3;
        public Ch_NewArmBandits(Pr_New_Banditry Bandits, Location loc)
            : base(loc)
        {
            this.Bandits = Bandits;
        }

        public override string getName()
        {
            return "Arm Bandits";
        }

        public override string getDesc()
        {
            return "Adds <b>additional danger</b> of the 'Combat Banditry' quest by " + (int)dangerincrease + ", at the cost of " + map.param.ch_armBanditsGoldCost + " <b>gold</b>." + " Also boosts the Banditry modifer temporarily.";
        }

        public override bool allowMultipleUsers()
        {
            return true;
        }
        public override double getProfile()
        {
            // Now uses stored property variable, removing the need to search for it every time.
            if (Bandits != null)
            {
                return Bandits.charge;
            }

            return 0.0;
        }

        public override double getMenace()
        {
            return 0;
        }

        public override int getCompletionProfile()
        {
            return map.param.ch_armBanditsProfile;
        }

        public override int getCompletionMenace()
        {
            return map.param.ch_armBanditsMenace;
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.COMMAND;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            // No longer needs to call Math.Max.
            double progress = unit.getStatCommand();
            if (progress < 1)
            {
                progress = 1;
                msgs?.Add(new ReasonMsg("Base", progress));
            }
            else
            {
                // If might is 1 or greater, it states that the value is from might.
                msgs?.Add(new ReasonMsg("Stat: Command", progress));
            }

            // The number was acquired only once, and used five times.
            return progress;
        }

        public override double getComplexity()
        {
            return map.param.ch_armBanditsComplexity;
        }

        public override bool validFor(UA ua)
        {
            return ua.person.gold >= map.param.ch_armBanditsGoldCost;
        }

        public override Sprite getSprite()
        {
            return map.world.iconStore.banditry;
        }

        public override int isGoodTernary()
        {
            return 0;
        }

        public override void complete(UA u)
        {
            addedDanger = Math.Max(0, addedDanger + dangerincrease);
            if (Bandits != null)
            {
                Bandits.FundedBandits += 20;
            }

            u.person.addGold(-map.param.ch_armBanditsGoldCost);
        }

        public override bool valid()
        {
            return true;
        }

        public override int[] buildPositiveTags()
        {
            return new int[4]
            {
                Tags.DISCORD,
                Tags.CRUEL,
                Tags.DANGER,
                Tags.GOLD
            };
        }

        public override int[] buildNegativeTags()
        {
            return new int[1]
            {
                Tags.COOPERATION
            };
        }
        public override int[] getNegativeTags()
        {
            if (location != null && location.settlement is SettlementHuman settlementHuman && settlementHuman.ruler != null)
            {
                int[] tagsNeg = new int[internalTagsNeg.Length + 3];
                // Rather than recreating the tags from scratch, this loop appends the current ruler's ID tag to the positive tags.
                // This means that when you change the `buildPositiveTags` function, you don't need to also change this function.
                for (int i = 0; i < internalTagsNeg.Length; i++)
                {
                    tagsNeg[i] = internalTagsNeg[i];
                }
                // This adds the ruler tag after the original tags. The `+1` isn't needed because of the 0-index.
                tagsNeg[internalTagsNeg.Length] = settlementHuman.ruler.index + 10000;
                tagsNeg[internalTagsNeg.Length + 1] = settlementHuman.ruler.society.index + 20000;
                tagsNeg[internalTagsNeg.Length + 2] = settlementHuman.ruler.society.index + 30000;

                return tagsNeg;
            }

            return internalTagsNeg;
        }
    }
}