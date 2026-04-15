using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bandits_and_Crime
{
    public class Rt_Lawbreaker_Crime_Organize_Dissent : Ritual
    {
        public Pr_Crime crime = null;

        public Rt_Lawbreaker_Crime_Organize_Dissent(Location loc)
            : base(loc)
        {
        }

        public override string getName()
        {
            return "Crime: Organize Dissent";
        }

        public override string getCastFlavour()
        {
            return "The Criminal Underground in this location can organise dissent against the local ruler.";
        }

        public override double getProfile()
        {
            return map.param.ch_organisedissent_aiProfile;
        }

        public override double getMenace()
        {
            return map.param.ch_organisedissent_aiMenace;
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
            msgs?.Add(new ReasonMsg("Stat: Intrigue", Math.Max(1, unit.getStatIntrigue())));
            return Math.Max(1, unit.getStatIntrigue());
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
            return map.world.iconStore.organisedDissent;
        }

        public override int isGoodTernary()
        {
            return -1;
        }
        public override string getDesc()
        {
            return "Creates an Organized Dissent Modifier in this location at 25% or boosts an existing one by 25%. Reduces crime modifier in location by 60%.";
        }

        public override void complete(UA u)
        {
            if (base.location.person() != null)
            {
                Pr_OrganisedDissent oofouch = null;
                foreach (Property property in base.location.properties)
                {
                    oofouch = property as Pr_OrganisedDissent;
                    if (oofouch != null)
                    {
                        break;
                    }
                }

                if (oofouch == null)
                {
                    oofouch = new Pr_OrganisedDissent(base.location, base.location.person());
                    oofouch.charge = 25.0;
                    base.location.properties.Add(oofouch);
                }
                else
                {
                    oofouch.influences.Add(new ReasonMsg("Criminal Activity", 25.0));
                }
            }
            foreach (Property property in location.properties.ToList())
            {
                crime = property as Pr_Crime;
                if (crime != null)
                {
                    crime.influences.Add(new ReasonMsg("Criminal Casualties", -60.0));
                    break;
                }
            }
        }

        public override string getRestriction()
        {
            return "Requires the Crime modifier to be over 60% in this location.";
        }

        public override bool valid()
        {
            Pr_Crime crime = null;
            foreach (Property property in location.properties)
            {
                crime = property as Pr_Crime;
                if (crime != null)
                {
                    break;
                }
            }
            if (crime == null)
            {
                return false;
            }
            else if (crime.charge >= 40)
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
