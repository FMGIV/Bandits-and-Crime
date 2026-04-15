using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bandits_and_Crime
{
    public class Rt_Brigand_Provoke_Brutal_Crackdown : Ritual
    {
        public Pr_New_Banditry bandits = null;

        public Rt_Brigand_Provoke_Brutal_Crackdown(Location loc)
            : base(loc)
        {
        }

        public override string getName()
        {
            return "Provoke Brutal Crackdown";
        }

        public override string getCastFlavour()
        {
            return "The Bandits can be used to provoke the ruler into cracking down on the people. Some will die in the process, but that's a sacrifice we're willing to make.";
        }

        public override double getProfile()
        {
            return 10;
        }

        public override double getMenace()
        {
            return 10;
        }

        public override int getCompletionProfile()
        {
            return 10;
        }

        public override int getCompletionMenace()
        {
            return 10;
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
            return 25;
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
            return "Creates a Lingering Resentment Modifier in this location at 25%. Reduces banditry modifier in location by 40%. Temporarily reduces ruler intrigue stat by 1 (Can Stack).";
        }

        public override void complete(UA u)
        {
            if (base.location.person() != null)
            {
                Pr_LingeringResentment oofouchies = new Pr_LingeringResentment(base.location);
                oofouchies.charge = 25;
                base.location.properties.Add(oofouchies);
            }
            foreach (Property property in location.properties)
            {
                bandits = property as Pr_New_Banditry;
                if (bandits != null)
                {
                    bandits.influences.Add(new ReasonMsg("Bandit Casualties", -40.0));
                }  
            }
            if((location.settlement is SettlementHuman sh && sh.ruler != null)) {
                T_StatTempIntrigue tempIntrigue = (T_StatTempIntrigue)sh.ruler.traits.FirstOrDefault(t => t is T_StatTempIntrigue);
                if (tempIntrigue == null)
                {
                    tempIntrigue = new T_StatTempIntrigue(25, -1);
                    sh.ruler.receiveTrait(tempIntrigue);
                }
                else
                {
                    tempIntrigue.amount -= 1;
                    tempIntrigue.turnsLeft += 25;
                }
            }
            
        }

        public override string getRestriction()
        {
            return "Requires the banditry modifier to be over 40% in this location.";
        }

        public override bool valid()
        {
            Pr_New_Banditry isbanditrypresent = null;
            foreach (Property property in location.properties)
            {
                isbanditrypresent = property as Pr_New_Banditry;
                if (isbanditrypresent != null)
                {
                    break;
                }
            }
            if (isbanditrypresent == null)
            {
                return false;
            }
            else if (isbanditrypresent.charge >= 40)
            {
                return true;
            }
            else
            {
                return false;
            }
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
