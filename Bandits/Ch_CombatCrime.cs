// Removed unnecessary using. They can be added by accident sometimes, so check when editing.
using Assets.Code;
using System;
using System.Collections.Generic;
using UnityEngine;
//using static Assets.Code.Challenge;

// Namespace has been changed. Change it to your chosen Namespace name. They should all be the same, even for your ModKernel.
namespace Bandits_and_Crime
{
    public class Ch_CombatCrime : Ch_CombatBanditry
    {
        // Variable declarations. These must be `public` in order to be save-load safe.
        public Pr_Crime Crime;

        public double chargeReduction = 100.0;

        public int dangerReduction = 3;

        // The constructor now requires the property that it is made for to be passed into it.
        public Ch_CombatCrime(Pr_Crime Crime, Location loc)
            : base(loc)
        {
            this.Crime = Crime;
        }

        public override string getName()
        {
            return "Combat Crime";
        }

        public override string getDesc()
        {
            // The values for the description are now pulled dynamically from the variables.
            return "Reduces the Crime property by " + (int)chargeReduction + " and reduces danger by " + dangerReduction + ".";
        }

        public override double getProfile()
        {
            // Now uses stored property variable, removing the need to search for it every time.
            if (Crime != null)
            {
                return Crime.charge;
            }

            return 0.0;
        }

        public override double getMenace()
        {
            // Now uses stored property variable, removing the need to search for it every time.
            if (Crime!= null)
            {
                return Crime.charge / 6;
            }

            return 0;
        }

        public override int getCompletionProfile()
        {
            return map.param.ch_combatbanditry_completionProfile;
        }

        public override int getInherentDanger()
        {
            return 2;
        }

        public override bool allowMultipleUsers()
        {
            return true;
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
                // If Might is less than 1, it now properly states that the source of the 1 is a base value.
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
            return map.param.ch_combatbanditry_complexity;
        }

        public override bool validFor(UA ua)
        {
            return true;
        }

        public override Sprite getSprite()
        {
            return map.world.iconStore.rootOutInfiltration;
        }

        public override int isGoodTernary()
        {
            return 1;
        }

        public override void complete(UA u)
        {
            // Since this is the challenge instance that is attached to the property, it can just change it's own value. There's no need to find itself.
            addedDanger = Math.Max(0, addedDanger - dangerReduction);

            if (Crime != null)
            {
                Crime.charge = Math.Max(0.0, Crime.charge - chargeReduction);
            }
        }

        public override bool valid()
        {
            return true;
        }

        public override int[] buildPositiveTags()
        {
            // This function is only run when the challenge is first made. As such, the ruler ID tag will not update as the ruler changes.
            // To solve this, the ruler logic has been moved to `getPositiveTags`
            return new int[3]
            {
                Tags.COMBAT,
                Tags.CRUEL,
                Tags.DANGER
            };
        }

        public override int[] getPositiveTags()
        {
            if (location != null && location.settlement is SettlementHuman settlementHuman && settlementHuman.ruler != null)
            {
                int[] tagsPos = new int[internalTagsPos.Length + 1];
                // Rather than recreating the tags from scratch, this loop appends the current ruler's ID tag to the positive tags.
                // This means that when you change the `buildPositiveTags` function, you don't need to also change this function.
                for (int i = 0; i < internalTagsPos.Length; i++)
                {
                    tagsPos[i] = internalTagsPos[i];
                }
                // This adds the ruler tag after the original tags. The `+1` isn't needed because of the 0-index.
                tagsPos[internalTagsPos.Length] = settlementHuman.ruler.index + 10000;

                return tagsPos;
            }

            return internalTagsPos;
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