using Assets.Code;
using Assets.Code.Modding;

namespace Bandits_and_Crime
{
    public class Bandits_and_Crime : ModKernel
    {

        public override void beforeMapGen(Map map)
        {
            generateNewEvents();
            map.overmind.agentsUnique.Add(new UAE_Abs_Brigand(map));
            map.overmind.agentsUnique.Add(new UAE_Abs_Lawbreaker(map));
        }

        public override void afterLoading(Map map)
        {
            generateNewEvents();
        }

        public void generateNewEvents()
        {
            if (!EventRuntime.properties.ContainsKey("RALLY_BANDITS"))
            {
                EventRuntime.properties.Add("RALLY_BANDITS", new EventRuntime.TypedProperty<string>(delegate (EventContext c, string v)
                {
                    UM_BanditArmy uM_BanditArmy = new UM_BanditArmy(c.location, c.map.soc_dark, 50, null);
                    uM_BanditArmy.location.units.Add(uM_BanditArmy);
                    c.map.units.Add(uM_BanditArmy);
                }));
            }
            if (!EventRuntime.properties.ContainsKey("COMMAND_BANDITS"))
            {
                EventRuntime.properties.Add("COMMAND_BANDITS", new EventRuntime.TypedProperty<string>(delegate (EventContext c, string v)
                {
                    UM_BanditArmy uM_BanditArmy = new UM_BanditArmy(c.location, c.map.soc_dark, 75, null);
                    uM_BanditArmy.location.units.Add(uM_BanditArmy);
                    c.map.units.Add(uM_BanditArmy);
                    // We need to get the brigand again
                    UAE_Brigand brigand = null;
                    foreach (Unit u in c.location.units)
                    {
                        if (u is UAE_Brigand brig)
                        {
                            brigand = brig;
                            break; // There's no need to chek the rest after you found what you need.
                        }
                    }
                    if (brigand != null && !brigand.isDead)
                    {
                        if (GraphicalMap.selectedUnit == brigand)
                        {
                            GraphicalMap.selectedUnit = uM_BanditArmy;
                        }
                        uM_BanditArmy.subsumedUnit = brigand;
                        c.map.overmind.agents.Add(uM_BanditArmy); // Adds the army to the list of units the playr controls
                        uM_BanditArmy.person = brigand.person; // Link the person of the brigand to the army
                        uM_BanditArmy.person.unit = uM_BanditArmy;
                        brigand.isDead = true; // Set him to dead to prevent the game trying to do stuff will him. Do not call `brigand.die`
                        c.location.units.Remove(brigand); // Remove the brigand from the map.
                        c.location.map.units.Remove(brigand);
                        c.location.map.overmind.agents.Remove(brigand);
                        uM_BanditArmy.rituals.Add(new Rt_BanditArmy_AbandonBandits(c.location, uM_BanditArmy));
                    }
                }));
            }
            if (!EventRuntime.properties.ContainsKey("CRIME_STEAL_GOLD"))
            {
                EventRuntime.properties.Add("CRIME_STEAL_GOLD", new EventRuntime.TypedProperty<string>(delegate (EventContext c, string v)
                {
                    {
                        Pr_ItemCache pr_ItemCache = new Pr_ItemCache(c.location);
                        pr_ItemCache.gold = c.person.gold + 100;
                        c.person.gold = 0;
                        c.location.properties.Add(pr_ItemCache);
                    }
                }));
            }
            if (!EventRuntime.properties.ContainsKey("CRIME_REDUCE_SECURITY"))
            {
                EventRuntime.properties.Add("CRIME_REDUCE_SECURITY", new EventRuntime.TypedProperty<string>(delegate (EventContext c, string v)
                {
                    {
                        Pr_Crime Crime = null;
                        foreach (Property property in c.location.properties)
                        {
                            Crime = property as Pr_Crime;
                            if (Crime != null)
                            {
                                Crime.extrasecuritydebuff -= 4;
                            }
                        }
                    }
                }));
            }
            if (!EventRuntime.properties.ContainsKey("CRIME_REDUCE_PROSPERITY"))
            {
                EventRuntime.properties.Add("CRIME_REDUCE_PROSPERITY", new EventRuntime.TypedProperty<string>(delegate (EventContext c, string v)
                {
                    Pr_Crime Crime = null;
                    foreach (Property property in c.location.properties)
                    {
                        Crime = property as Pr_Crime;
                        if (Crime != null)
                        {
                            Crime.extraprosperitydebuff -= 0.35;
                        }
                    }
                }));
            }
            if (!EventRuntime.properties.ContainsKey("BANDITS_REDUCE_FOOD"))
            {
                EventRuntime.properties.Add("BANDITS_REDUCE_FOOD", new EventRuntime.TypedProperty<string>(delegate (EventContext c, string v)
                {
                    Pr_Stolen_Food stolenFood = null;
                    foreach (Property property in c.location.properties)
                    {
                        stolenFood = property as Pr_Stolen_Food;
                        if (stolenFood != null)
                        {
                            // This tells the code to exit the loop early. There's no point in continuing to loop over the properties after you've found what you need.
                            break;
                        }
                    }

                    if (stolenFood == null)
                    {
                        // If there is no instance of Pr_Stolen_Food at that location, the result will remain `null` after the loop goes through all properties, and we need to add a new one.
                        stolenFood = new Pr_Stolen_Food(c.location);
                        stolenFood.charge = 50.0;
                        c.location.properties.Add(stolenFood);
                    }
                    else
                    {
                        // If we did find an instance of stolen food, we need to add an influence to it, so that it shows up in the UI.
                        stolenFood.influences.Add(new ReasonMsg("Bandits Stole Food", 50.0));
                    }
                }));
            }
        }

        public override void onTurnStart(Map map)
        {
            foreach (Location loc in map.locations)
            {
                if (loc.settlement is SettlementHuman)
                {
                    for (int i = 0; i < loc.properties.Count; i++)
                    {
                        Pr_Bandity banditry = loc.properties[i] as Pr_Bandity;
                        if (banditry != null && banditry.GetType() == typeof(Pr_Bandity))
                        {
                            if (loc.settlement is Set_City)
                            {
                                Pr_Crime Crime = new Pr_Crime(loc);
                                Crime.charge = banditry.charge;
                                Crime.influences = banditry.influences;
                                loc.properties[i] = Crime;
                            }
                            else
                            {
                                Pr_New_Banditry newBanditry = new Pr_New_Banditry(loc);
                                newBanditry.charge = banditry.charge;
                                newBanditry.influences = banditry.influences;
                                loc.properties[i] = newBanditry;
                            }
                        }
                    }
                }
            }
        }

        public void spawnBandit(Map map, Location loc)
        {
            Person p = new Person(map.soc_dark);
            UAEN_Bandit UAEN_Bandit = new UAEN_Bandit(loc, map.soc_dark, p);
            UAEN_Bandit.location.units.Add(UAEN_Bandit);
            map.units.Add(UAEN_Bandit);
            map.addUnifiedMessage(UAEN_Bandit, loc, "Bandit", "A Bandit has appeared from the Bandits in " + loc.getName() + ". Driven by greed, they will pillage their surroundings and rob heroes of their items until they inevitably die a violent death.\n\nBandits can spawn randomly from locations with the Banditry Modifier depending on it's charge.", "ABYSSAL PRIEST", force: true);
        } 
    }
}
