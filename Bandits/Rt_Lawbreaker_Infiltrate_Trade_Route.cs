using Assets.Code;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bandits_and_Crime
{
    public class Rt_Lawbreaker_Infiltrate_Trade_Route : Ritual
    {
        public Pr_Crime crime;

        public UA Lawbreaker;

        public Rt_Lawbreaker_Infiltrate_Trade_Route(Location loc, UA Lawbreaker)
            : base(loc)
        {
            this.Lawbreaker = Lawbreaker;
        }

        public override string getName()
        {
            return "Infiltrate Trade Network";
        }

        public override string getCastFlavour()
        {
            return "Crime spreads through the trade network, and the underworld feasts on it's properity.";
        }

        public override string getRestriction()
        {
            return "Requires the Crime modifier to be over 100% in this location and for this location to be on a trade route.";
        }

        public override double getProfile()
        {
            return 10;
        }

        public override double getMenace()
        {
            return 16;
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
            return map.world.iconStore.bribe;
        }

        public override int isGoodTernary()
        {
            return -1;
        }
        public override string getDesc()
        {
            return "Starts crime modifiers in cities along this trade route at 60%. If location already has a crime modifier, increases prosperity debuff by 25%. Requires crime above 100%";
        }

        public override void complete(UA u)
        {
            foreach (TradeRoute route in map.tradeManager.routes)
            {
                if (route != null && route.path.Contains(Lawbreaker.location))
                {
                    foreach (Location thing in route.path)
                    {
                        if (thing.settlement is Set_City)
                        {
                            Pr_Crime crime = null;
                            foreach (Property property in thing.properties)
                            {
                                crime = property as Pr_Crime;
                                if (crime != null)
                                {
                                    break;
                                }
                            }

                            if (crime == null)
                            {
                                crime = new Pr_Crime(thing);
                                crime.charge = 60;
                                thing.properties.Add(crime);
                            }
                            else
                            {
                                crime.extraprosperitydebuff -= 0.25;
                            }
                        }
                    }
                }     
            }
        }

        public override bool valid()
        {
            Pr_Crime iscrimepresent = null;
            bool checker = false,
            checker2 = false;
            foreach (Property property in location.properties)
            {
                iscrimepresent = property as Pr_Crime;
                if (iscrimepresent != null && iscrimepresent.charge >= 100)
                {
                    // This tells the code to exit the loop early. There's no point in continuing to loop over the properties after you've found what you need.
                    checker = true;
                }
            }
            if (checker == true)
            {
                foreach (TradeRoute route in map.tradeManager.routes)
                {
                    if (route != null && route.path.Contains(Lawbreaker.location))
                    {
                        checker2 = true;
                    }
                }
            }
            return checker && checker2;
        }

        public override int[] buildPositiveTags()
        {
            return new int[3]
            {
                Tags.DISCORD,
                Tags.GOLD,
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
