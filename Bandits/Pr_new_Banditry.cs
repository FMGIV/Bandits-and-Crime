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
    public class Pr_New_Banditry : Pr_Bandity
    {
        public int FundedBandits = 0;

        public double prosperitydebuff = -0.25;

        public int BanditSpawnedCooldown = 0;

        public int BanditsSpawned = 0;

        public Pr_New_Banditry banditry;

        public List<Action> actions = new List<Action>();

        public Pr_New_Banditry(Location loc)
            : base(loc)
        {
            challenges.Clear();
            challenges.Add(new Ch_NewArmBandits(this, loc));
            challenges.Add(new Ch_NewCombatBanditry(this, loc));
            challenges.Add(new Ch_NewSlaughterBandits(this, loc));
            //challenges.Add(new Ch_DebugBandits(this, loc)); //for debug purposes
            actions.Add(new Act_BanditCrackdown(this, loc));
        }


        public override string getName()
        {
            return "Banditry";
        }

        public override void turnTick()
        {
            if (BanditSpawnedCooldown > 0 && BanditsSpawned < 5) // at most 5 bandits from modifier
            {
                BanditSpawnedCooldown--;
                if (BanditSpawnedCooldown <= 0)
                {
                    if (Eleven.random.NextDouble() < charge / 300.0) //Charge goes from 0.0 to 300.0, while NextDouble goes from 0.0 to 1.0. By dividing charge by 300.0 we convert it into the range of 1.0.
                    {
                        Society society = location.soc as Society;
                        if (society == null)
                        {
                            return; // Assuming no required code exists after the spawning block. Use `if (society != null)` if you can't return here.
                        }
                        Person p;
                        string message;
                        if (Eleven.random.NextDouble() < 0.25) //ex-noble
                        {
                            p = new Person(society);
                            p.embedIntoSociety();
                            message = "An ex-noble, turned bandit has emerged in the ";
                            UAEN_Bandit bandit = new UAEN_Bandit(location, society, p);
                            bandit.location.units.Add(bandit);
                            map.units.Add(bandit);
                            for (int i = 0; i < 3; i++)
                            {
                                if (p.unit != null && p.unit is UA uA && uA.minions[i] == null)
                                {
                                    M_Sellsword sellswords = new M_Sellsword(p.map);
                                    uA.minions[i] = sellswords;
                                }
                            }
                            bandit.location.units.Add(bandit);
                        }
                        else //peasant
                        {
                            House house = new House(map);
                            if (map.landmassID != null)
                            {
                                house.name = TextStore.getLastName(map, map.landmassID[Eleven.random.Next()][Eleven.random.Next()]);
                            }
                            else
                            {
                                house.name = TextStore.getLastName(map, 0);
                            }
                            message = "A jumped up peasant has turned to banditry in the ";
                            house.name = TextStore.getLastName(map, 0);
                            house.background = Eleven.random.Next(World.self.textureStore.houseBackgrounds.Length);
                            house.culture = map.sampleCulture(location);
                            society.houses.Add(house);
                            map.houses.Add(house);
                            p = new Person(society, house);
                            UAEN_Bandit bandit = new UAEN_Bandit(location, society, p);
                            bandit.location.units.Add(bandit);
                            map.units.Add(bandit);
                            for (int i = 0; i < 3; i++)
                            {
                                if (p.unit != null && p.unit is UA uA && uA.minions[i] == null)
                                {
                                    M_Bandit BanditMinions = new M_Bandit(p.map);
                                    uA.minions[i] = BanditMinions;
                                }
                            }
                            bandit.location.units.Add(bandit);
                            if (society is Soc_Elven elves)
                            {
                                p.species = map.species_elf;
                            }
                            /* else if (society is Soc_Dwarves dwarfs)
                            {
                                p.species = map.species_dwarf;
                            } */ //ENABLE FOR DLC
                        }
                        map.addUnifiedMessage(p, location, "Notable Bandit Appears", message + location.getName() + ". Driven by greed and ambition, they will go around funding banditry, pillaging locations and robbing heroes.", "BANDIT", force: true);

                        BanditsSpawned++;
                        BanditSpawnedCooldown += 30;
                    }
                }
            }  
            if (FundedBandits > 0)
            {
                FundedBandits--;
                influences.Add(new ReasonMsg("The Bandits have recieved funding.", 4.0));
            }
            foreach (Property property in base.location.properties)
            {
                if (property is Pr_Unrest && property.charge >= 100)
                {
                    influences.Add(new ReasonMsg("Unrest drives people to banditry.", 2.0));
                }
                if (property is Pr_Famine && property.charge >= 100)
                {
                    influences.Add(new ReasonMsg("Hunger and Famine drives people to banditry.", 2.0));
                }
                if (property is Pr_LingeringResentment && property.charge >= 1)
                {
                    influences.Add(new ReasonMsg("The people turn to banditry to get revenge on their cruel ruler.", 3.0));
                }
                if (property is Pr_OrganisedDissent && property.charge >= 1)
                {
                    influences.Add(new ReasonMsg("Cooperating with those resisting the local ruler.", 1.0));
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
                            influences.Add(new ReasonMsg("Cooperating with neighbouring bandit groups", 1.0));
                            foundBanditry = true;
                            break;
                        }
                        else
                        {
                            influences.Add(new ReasonMsg("Cooperating with neighbouring criminal groups", 1.0));
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
            if (charge >= 100.0)
            {
                if (location.settlement is SettlementHuman sh && sh.ruler != null && sh.ruler.gold > 0)
                {
                    sh.ruler.gold--;
                    influences.Add(new ReasonMsg("Bandits are robbing this location's treasury.", 1.0));
                }

                if (charge >= 200.0)
                {
                    Property.addToProperty("Rampant Banditry", Property.standardProperties.DEVASTATION, 2, location);
                    if (charge >= 300.0)
                    {
                        if (location.units.Any(u => u is UAE_Brigand brigand && !brigand.isDead))
                        {
                            EventContext ctx = EventContext.withLocation(map, base.location);
                            ctx.map.world.prefabStore.popEvent(EventManager.events["BanditsandCrime.ToomuchBanditryandBrigand"].data, ctx, null, force: true);
                            charge = 200;
                        }
                        else
                        {
                            EventContext ctx = EventContext.withLocation(map, base.location);
                            ctx.map.world.prefabStore.popEvent(EventManager.events["BanditsandCrime.ToomuchBanditry"].data, ctx, null, force: true);
                            charge = 200;
                        }
                    }
                }
            }
            else if (charge < 0.0)
            {
                charge = 0.0;
            }

            prosperitydebuff = -charge / 400;
        }

        public override string getDesc()
        {
            return "Bandits have started raiding the lanes and trails near this location, and will require a hero to remove. While they remain, they decrease this location's <b>Prosperity</b> based on its Current Level and decrease <b>Security</b> by 1. May also start draining this location's treasury and increase <b>Devastation</b> at above 200% charge.";
        }

        public override bool hasHexView()
        {
            return true;
        }

        public override Sprite hexViewSprite()
        {
            return EventManager.getImg("BanditsandCrime.Bandit_Transparent.png");
        }

        public override bool canTriggerCrisis()
        {
            return true;
        }

        public override double getProsperityInfluence()
        {
            return prosperitydebuff;
        }

        public override int getSecurityChange(SettlementHuman hum)
        {
            return -1;
        }

        public override List<Challenge> getChallenges()
        {
            return challenges;
        }

        public override List<Action> getActions()
        {
            if (base.location.settlement is SettlementHuman)
            {
                return actions;
            }

            return base.getActions();
        }

        public override Sprite getSprite(World world)
        {
            return world.iconStore.banditry;
        }

        public override standardProperties getPropType()
        {
            return standardProperties.OTHER;
        }
    }
}