using Assets.Code;
using System.Collections.Generic;
using UnityEngine;

namespace Bandits_and_Crime
{
    public class Ch_CrimePoliticalGridlock : Challenge
    {
        public Pr_Crime crime;
        public Ch_CrimePoliticalGridlock(Pr_Crime crime, Location loc)
            : base(loc)
        {
            this.crime = crime;
        }

        public override string getName()
        {
            return "Crime: Disturbance";
        }

        public override string getCastFlavour()
        {
            return "The underworld in this location will benefit from the local ruler's distraction.";
        }

        public override string getDesc()
        {
            return "Cancels the current action underway at this location and starts a \"Political Gridlock\" action which wastes 10 turns. If this is the nation's capital the national action is also affected.";
        }

        public override string getRestriction()
        {
            return "Requires Crime to be at least 100% and the location to be partially infiltrated.";
        }

        public override double getProfile()
        {
            return 5;
        }

        public override double getMenace()
        {
            return 10;
        }

        public override int getCompletionProfile()
        {
            return 5;
        }

        public override int getCompletionMenace()
        {
            return 10;
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.INTRIGUE;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            // No longer needs to call Math.Max.
            double progress = unit.getStatIntrigue();
            if (progress < 1)
            {
                progress = 1;
                msgs?.Add(new ReasonMsg("Base", progress));
            }
            else
            {
                // If might is 1 or greater, it states that the value is from might.
                msgs?.Add(new ReasonMsg("Stat: Intrigue", progress));
            }

            // The number was acquired only once, and used five times.
            return progress;
        }

        public override double getComplexity()
        {
            return 20;
        }

        public override bool validFor(UA ua)
        {
            return true;
        }

        public override Sprite getSprite()
        {
            return map.world.iconStore.gridlock;
        }

        public override int isGoodTernary()
        {
            return -1;
        }

        public override void complete(UA u)
        {
            base.location.settlement.actionProgress = 0;
            base.location.settlement.actionUnderway = new Act_Gridlock(base.location);
            if (base.location.soc is Society society && base.location.index == society.capital)
            {
                society.actionProgress = 0;
                society.actionUnderway = new AN_Gridlock(map);
            }
        }

        public override bool valid()
        {
            if (crime.charge >= 100 && base.location.settlement.infiltration > 0)
            {
                return true;
            }
            return false;
        }

        public override int[] buildPositiveTags()
        {
            return new int[1]
            {
                Tags.DISCORD,
            };
        }

        public override int[] buildNegativeTags()
        {
            return new int[0];
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