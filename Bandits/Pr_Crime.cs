#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Shadows of Forbidden Gods\ShadowsOfForbiddenGods_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion
using Assets.Code;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bandits_and_Crime
{
    public class Pr_Crime : Pr_Bandity
    {
        public int Funded = 0;

        public int SabotageDocksTimer = 0;

        public int extrasecuritydebuff = 0;

        public int CrimeSecurityReductionModifier = 0; //dynamically set later

        public double prosperitydebuff = 0; //dynamically set later

        public double extraprosperitydebuff = 0; //from event

        public Pr_Crime Crime;

        public List<Action> actions = new List<Action>();

        public Pr_Crime(Location loc)
            : base(loc)
        {
            challenges.Clear();
            challenges.Add(new Ch_CombatCrime(this, loc));
            challenges.Add(new Ch_FundCriminals(this, loc));
            challenges.Add(new Ch_SlaughterCriminalUnderworld(this, loc));
            challenges.Add(new Ch_CrimePoliticalGridlock(this, loc));
            //challenges.Add(new Ch_DebugCrime(this, loc)); //for debug purposes
            actions.Add(new Act_CrimeCrackdown(this, loc));
            if (loc.settlement is SettlementHuman settlementhuman)
            {
                foreach (Subsettlement sub in settlementhuman.subs)
                {
                    if (sub is Sub_Temple && !(sub is Sub_HolyOrderCapital))
                    {
                        challenges.Add(new Ch_CrimeDestroyTemple(this, loc));
                    }
                    if (sub is Sub_Sewers)
                    {
                        challenges.Add(new Ch_CrimeAcquireContaminant(this, loc));
                    }
                    if (sub is Sub_Market)
                    {
                        challenges.Add(new Ch_CrimeSellItem(this, loc));
                    }
                    if (sub is Sub_Library)
                    {
                        challenges.Add(new Ch_CrimeStealArcaneTexts(this, loc));
                    }
                    if (sub is Sub_Docks)
                    {
                        challenges.Add(new Ch_CrimeSabotageDocks(this, loc));
                    }
                }
            }
            if (loc.soc is Society society)
            {
                if (!(society.getCapital() == loc))
                {
                    challenges.Add(new Ch_CrimePoliticalAgitation(this, loc));
                }
            }

        }



        public override string getName()
        {
            return "Crime";
        }

        public override void turnTick()
        {
            if (Funded > 0)
            {
                Funded--;
                influences.Add(new ReasonMsg("The Underworld here is being funded by something.", 4.0));
            }
            if (SabotageDocksTimer > 0)
            {
                SabotageDocksTimer--;
                influences.Add(new ReasonMsg("The Underworld here is taking advantage of the sabotaged docks.", 2.0));
            }
            if (extraprosperitydebuff < 0)
            {
                extraprosperitydebuff += 0.01;

            }
            foreach (Property property in base.location.properties)
            {
                if (property is Pr_Unrest && property.charge >= 100)
                {
                    influences.Add(new ReasonMsg("Unrest drives people to crime.", 2.0));
                }
                if (property is Pr_Famine && property.charge >= 100)
                {
                    influences.Add(new ReasonMsg("Hunger and Famine drive people to crime.", 2.0));
                }
                if (property is Pr_LingeringResentment && property.charge >= 1)
                {
                    influences.Add(new ReasonMsg("The people turn to criminality to get revenge on their cruel ruler.", 1.0));
                }
                if (property is Pr_OrganisedDissent && property.charge >= 1)
                {
                    influences.Add(new ReasonMsg("Cooperating with those resisting the local ruler.", 3.0));
                }
            }
            bool foundBanditry = false;
            foreach (Location neighbour in location.getNeighbours())
            {
                foreach (Property property in neighbour.properties)
                {
                    if (property is Pr_Bandity && property.charge >= 100)
                    {
                        if (property is Pr_New_Banditry)
                        {
                            influences.Add(new ReasonMsg("Cooperating with neighbouring bandit groups.", 1.0));
                            foundBanditry = true;
                            break;
                        }
                        else
                        {
                            influences.Add(new ReasonMsg("Cooperating with neighbouring criminal groups.", 1.0));
                            foundBanditry = true;
                            break;
                        }
                    }

                    if (foundBanditry)
                    {
                        break;
                    }
                }
            }
            if (charge >= 100.0 && location.settlement is SettlementHuman sh && sh.ruler != null && sh.ruler.gold > 0)
            {
                if (charge >= 200)
                {
                    sh.ruler.gold = sh.ruler.gold - 2;
                    influences.Add(new ReasonMsg("As crime starts becoming worse, more gold seems to go missing every day.", 1.0));
                    Property.addToProperty("Rampant Crime", Property.standardProperties.UNREST, 2, location);
                }
                else
                {
                    sh.ruler.gold = sh.ruler.gold - 1;
                    influences.Add(new ReasonMsg("Criminals have started robbing this location's treasury.", 1.0));
                }
            }

            if (charge >= 300) {
                if (location.units.Any(u => u is UAE_Lawbreaker Lawbreaker && !Lawbreaker.isDead))
                {
                    EventContext ctx = EventContext.withLocation(map, base.location);
                    ctx.map.world.prefabStore.popEvent(EventManager.events["BanditsandCrime.ToomuchCrimeandLawbreaker"].data, ctx, null, force: true);
                    charge = 200;
                }
                else
                {
                    EventContext ctx = EventContext.withLocation(map, base.location);
                    ctx.map.world.prefabStore.popEvent(EventManager.events["BanditsandCrime.ToomuchCrime"].data, ctx, null, force: true);
                    charge = 200;
                }
            }
            else if (charge >= 200) { CrimeSecurityReductionModifier = -3; } //depending on severity of the crime modifier reduce this locations security
            else if (charge <= 100) { CrimeSecurityReductionModifier = -2; }
            prosperitydebuff = -charge / 1000; //between 0 - 30% debuff 
            if (charge < 0) { charge = 0; } //cap to zero
        }

        public override string getDesc()
        {
            return "Crime has started to become rampant in this location, and will require a hero to remove. While they remain, they decrease this location's <b>Prosperity</b> and may reduce <b>Security</b> based on Current Level. May also start draining this locations treasury at above 100% charge. Increases <b>Unrest</b> at above 200% charge. Boosted from unrest modifiers and neighboring banditry above 100% charge.";
        }

        public override bool hasHexView()
        {
            return true;
        }

        public override List<Action> getActions()
        {
            if (base.location.settlement is SettlementHuman)
            {
                return actions;
            }

            return base.getActions();
        }

        public override Sprite hexViewSprite()
        {
            return EventManager.getImg("BanditsandCrime.Crime_Transparent.png");
        }

        public override bool canTriggerCrisis()
        {
            return true;
        }

        public override double getProsperityInfluence()
        {
            return prosperitydebuff + extraprosperitydebuff;
        }

        public override int getSecurityChange(SettlementHuman hum)
        {
            return CrimeSecurityReductionModifier + extrasecuritydebuff;
        }

        public override List<Challenge> getChallenges()
        {
            return challenges;
        }

        public override Sprite getSprite(World world)
        {
            return world.iconStore.infiltrate;
        }

        public override standardProperties getPropType()
        {
            return standardProperties.OTHER;
        }
    }
}