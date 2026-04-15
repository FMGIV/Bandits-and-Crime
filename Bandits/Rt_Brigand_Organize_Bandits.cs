using Assets.Code;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bandits_and_Crime
{
    public class Rt_Brigand_Organize_Bandits : Ritual
    {
        public Pr_New_Banditry bandits;

        public int dangerincrease = 2;

        public Rt_Brigand_Organize_Bandits(Location loc)
            : base(loc)
        {
        }

        public override string getName()
        {
            return "Organize Bandits";
        }

        public override string getCastFlavour()
        {
            return "The Bandits in this location can be organized to be more efficient.";
        }

        public override double getProfile()
        {
            return 5;
        }

        public override double getMenace()
        {
            return 8;
        }

        public override int getCompletionProfile()
        {
            return 5;
        }

        public override int getCompletionMenace()
        {
            return 8;
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
            return 16;
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
            return "Increases the <b>Banditry</b> Modifier in this location by 40% and adds <b>additional danger</b> of the 'Combat Banditry' quest by 2.. At 300% or above Banditry a crisis will occur.";
        }

        public override void complete(UA u)
        {
            foreach (Property property in location.properties)
            {
                bandits = property as Pr_New_Banditry;
                if (bandits != null)
                {
                    bandits.influences.Add(new ReasonMsg("Brigand Organizing Banditry in Location.", 40.0));
                }
            }
            addedDanger = Math.Max(0, addedDanger + 2);

        }

        public override bool valid()
        {
            Pr_New_Banditry isbanditrypresent = null;
            bool checker = false;
            foreach (Property property in location.properties)
            {
                isbanditrypresent = property as Pr_New_Banditry;
                if (isbanditrypresent != null)
                {
                    // This tells the code to exit the loop early. There's no point in continuing to loop over the properties after you've found what you need.
                    checker = true;
                }
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
