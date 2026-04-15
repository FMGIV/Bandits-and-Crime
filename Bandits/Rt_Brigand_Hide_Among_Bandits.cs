using Assets.Code;
using System.Collections.Generic;
using UnityEngine;

namespace Bandits_and_Crime
{
    public class Rt_Brigand_Hide_Among_Bandits : Ritual
    {
        public Pr_New_Banditry bandits;

        public Rt_Brigand_Hide_Among_Bandits(Location loc)
            : base(loc)
        {
        }

        public override string getName()
        {
            return "Hide Among Bandits";
        }

        public override string getRestriction()
        {
            return "Requires the banditry modifier to be present in location.";
        }

        public override string getDesc()
        {
            return "Causes this agent to begin laying low, where they will lose 2 points of menace and profile each turn.";
        }

        public override string getCastFlavour()
        {
            return "Bandits can be a useful tool in deflecting attention.";
        }

        public override double getProfile()
        {
            return 0;
        }

        public override double getMenace()
        {
            return 0;
        }

        public override int getCompletionProfile()
        {
            return 0;
        }

        public override bool ignoreInterruptionWarning()
        {
            return true;
        }

        public override int getCompletionMenace()
        {
            return 0;
        }

        public override bool isIndefinite()
        {
            return true;
        }

        public override double getUtility(UA ua, List<ReasonMsg> reasons)
        {
            double utility = base.getUtility(ua, reasons);
            double num = ua.inner_profile - ua.inner_profileMin;
            double num2 = ua.inner_menace - ua.inner_menaceMin;
            utility += num;
            utility += num2;
            if (reasons != null)
            {
                reasons.Add(new ReasonMsg("Potential Profile Reduction", num));
                reasons.Add(new ReasonMsg("Potential Menace Reduction", num2));
            }

            return utility;
        }

        public override int getSimplificationLevel()
        {
            return 0;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            return getProgressPerTurnInnerAgnostic(msgs);
        }

        public double getProgressPerTurnInnerAgnostic(List<ReasonMsg> msgs)
        {
            double num = 2.0;
            msgs?.Add(new ReasonMsg("Base", 2));

            return num;
        }


        public override double getComplexity()
        {
            return 0;
        }

        public override bool validFor(UA ua)
        {
            return ua.inner_menace > ua.inner_menaceMin || ua.inner_profile > ua.inner_profileMin;
        }

        public override Sprite getSprite()
        {
            return map.world.iconStore.layLow;
        }

        public override int isGoodTernary()
        {
            return -1;
        }

        public override void complete(UA u)
        {
        }

        public override void turnTick(UA u)
        {
            base.turnTick(u);
            if (!(u.task is Task_PerformChallenge task_PerformChallenge) || task_PerformChallenge.turnsTaken < 1)
            {
                return;
            }

            bool flag = u.inner_profile <= u.inner_profileMin;
            bool flag2 = u.inner_menace <= u.inner_menaceMin;
            u.addProfile(-2.0 * getProgressPerTurn(u, null));
            u.addMenace(-2.0 * getProgressPerTurn(u, null));
            bool flag3 = u.inner_profile <= u.inner_profileMin;
            bool flag4 = u.inner_menace <= u.inner_menaceMin;
            if (flag4 && flag3)
            {
                u.task = null;
                complete(u);
                if (u.isCommandable())
                {
                    map.addMessage(u.getName() + " completes: " + getName(), map.param.ch_laylow_parameterValue5, positive: true, u.location.hex);
                    popCompletionMessage(u);
                }
            }
            else if (flag4 && !flag2)
            {
                if (u.isCommandable())
                {
                    map.addUnifiedMessage(u, null, "Hiding among Bandits", u.getName() + " is laying low and has reached their minimum menace value, but not their minimum profile", UnifiedMessage.messageType.LAY_LOW_PARTIALLY_COMPLETE);
                }
            }
            else if (flag3 && !flag && u.isCommandable())
            {
                map.addUnifiedMessage(u, null, "Hiding Among Bandits", u.getName() + " is laying low and has reached their minimum profile value, but not their minimum menace", UnifiedMessage.messageType.LAY_LOW_PARTIALLY_COMPLETE);
            }
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
            else { return true; }
        }

        public override int[] buildPositiveTags()
        {
            return new int[0];
        }

        public override int[] buildNegativeTags()
        {
            return new int[1] { Tags.DANGER };
        }
    }
}
