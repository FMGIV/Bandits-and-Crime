using Assets.Code;
using System.Collections.Generic;
using UnityEngine;
using Action = Assets.Code.Action;

namespace Bandits_and_Crime
{
    public class Act_CrimeCrackdown : Action
    {
        public Pr_Crime Crime;
        public Act_CrimeCrackdown(Pr_Crime Crime, Location loc)
            : base(loc)
        {
            this.Crime = Crime;
        }

        public override string getName()
        {
            return "Criminal Supression";
        }

        public override string getShortDesc()
        {
            return "Halves <b>Crime</b> in location at the cost of 33% <b>Defence</b>. (Will not be valid if Defence is under 33%)";
        }

        public override int getTurnsRequired()
        {
            return 8;
        }

        public override Sprite getIconFore()
        {
            return map.world.iconStore.reduceUnrest;
        }

        public override Sprite getIconBack()
        {
            return null;
        }

        public override bool valid(Person ruler, SettlementHuman settlementHuman)
        {
            return Crime.charge > 0.0 && base.location.settlement.defences > (settlementHuman.getMaxDefence() / 3);
            //valid if banditry is over 0% and settlement defense is over 33%
        }

        public override double getUtility(SettlementHuman hum, Person ruler, List<ReasonMsg> reasons)
        {
            double utility = base.getUtility(hum, ruler, reasons);
            double num = Crime.charge * 0.75;
            utility += num;
            reasons?.Add(new ReasonMsg("Level of Crime", num));
            return utility;
        }

        public override int[] getNegativeTags()
        {
            return new int[3]
            {
            Tags.DANGER,
            Tags.DISCORD,
            Tags.CRUEL
            };
        }

        public override int[] getPositiveTags()
        {
            return new int[1]
            {
                Tags.GOLD
            };
        }

        public override void complete()
        {
            double crimereduction = Crime.charge / -2;
            Crime.influences.Add(new ReasonMsg("Ruler cracks down on Crime.", crimereduction));
            base.location.settlement.defences = base.location.settlement.defences - base.location.settlement.defences * .33;
        }
    }
}
