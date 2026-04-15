using Assets.Code;
using System.Collections.Generic;
using UnityEngine;

namespace Bandits_and_Crime
{
    public class Ch_CrimeSabotageDocks : Challenge
    {
        public Pr_Crime crime;
        public Ch_CrimeSabotageDocks(Pr_Crime crime, Location loc)
            : base(loc)
        {
            this.crime = crime;
        }

        public override string getName()
        {
            return "Sabotage Docks";
        }

        public override string getCastFlavour()
        {
            return "Port's closed, our connections in the local criminal underworld here have seen to that.";
        }

        public override string getDesc()
        {
            return "Temporarily negates the prosperity buffs from the docks and increases crime charge gain.";
        }

        public override string getRestriction()
        {
            return "Requires Crime to be at least 150% and the location to be at or over 50% infiltration.";
        }

        public override double getProfile()
        {
            return 10;
        }

        public override double getMenace()
        {
            return 15;
        }

        public override int getCompletionProfile()
        {
            return 10;
        }

        public override int getCompletionMenace()
        {
            return 15;
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
            return map.world.iconStore.docks;
        }

        public override int isGoodTernary()
        {
            return -1;
        }

        public override void complete(UA u)
        {
            crime.extraprosperitydebuff -= .2;
            crime.SabotageDocksTimer += 20;
        }

        public override bool valid()
        {
            if (crime.charge >= 150 && base.location.settlement.infiltration >= 0.5)
            {
                return true;
            }
            return false;
        }

        public override int[] buildPositiveTags()
        {
            return new int[2]
            {
                Tags.DISCORD,
                Tags.GOLD
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