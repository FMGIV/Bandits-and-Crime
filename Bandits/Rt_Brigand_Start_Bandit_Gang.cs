using Assets.Code;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bandits_and_Crime
{
    public class Rt_Brigand_Start_Bandit_Gang : Ritual
    {
        public Pr_New_Banditry bandits;

        public Rt_Brigand_Start_Bandit_Gang(Location loc)
            : base(loc)
        {
        }

        public override string getName()
        {
            return "Start Bandit Gang";
        }

        public override string getCastFlavour()
        {
            return "Some of the local populance can be convinced to take up more... direct approaches against their liege.";
        }

        public override double getProfile()
        {
            return 20;
        }

        public override double getMenace()
        {
            return 30;
        }

        public override int getCompletionProfile()
        {
            return 20;
        }

        public override int getCompletionMenace()
        {
            return 30;
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.COMMAND;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            msgs?.Add(new ReasonMsg("Stat: Command", Math.Max(1, unit.getStatCommand())));
            return Math.Max(1, unit.getStatCommand());
        }

        public override double getComplexity()
        {
            return 30;
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
        public override string getDesc()
        {
            return "Creates a Banditry Modifier in this location at 100%";
        }

        public override void complete(UA u)
        {
            Pr_New_Banditry Bandits = null;
            Bandits = new Pr_New_Banditry(location);
            Bandits.charge = 100.0;
            location.properties.Add(Bandits);
        }

        public override string getRestriction()
        {
            return "Requires a minor human settlement without a current Banditry Modifier.";
        }

        public override bool valid()
        {
            Pr_New_Banditry isbanditrypresent = null;
            bool checker = true;
            foreach (Property property in location.properties)
            {
                isbanditrypresent = property as Pr_New_Banditry;
                if (isbanditrypresent != null)
                {
                    // This tells the code to exit the loop early. There's no point in continuing to loop over the properties after you've found what you need.
                    checker = false;
                }
            }
            if (!(location.settlement is Set_MinorHuman) && checker == true)
            {
                checker = false;
            }
            return checker;
        }

        public override int[] buildPositiveTags()
        {
            return new int[3]
            {
                Tags.DISCORD,
                Tags.CRUEL,
                Tags.DANGER
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
