using Assets.Code;
using System.Collections.Generic;
using UnityEngine;

namespace Bandits_and_Crime
{
    public class UM_BanditArmy : UM
    {

        public UA subsumedUnit;

        public int bonushp = 0;
        public UM_BanditArmy(Location loc, SocialGroup sg, int startingHP, UM_BanditArmy BanditArmy)
            : base(loc, sg)
        {
            maxHp = 100 + bonushp;
            hp = startingHP;
        }

        public override void turnTickInner(Map map)
        {
            base.turnTickInner(map);
            if (subsumedUnit != null)
            {
                bonushp = subsumedUnit.getStatCommand() * 25;
            }
            else
            {
                bonushp = 0;
            }
            maxHp = 100 + bonushp;
            if (hp < maxHp)
            {
                foreach (Property property in base.location.properties)
                {
                    if (property != null && property is Pr_Unrest && property.charge >= 0.0)
                    {
                        property.influences.Add(new ReasonMsg("Unrest drives people to the bandit army.", -5));
                        hp += 2;
                    }
                    if (property != null && property is Pr_LingeringResentment && property.charge >= 0.0)
                    {
                        property.influences.Add(new ReasonMsg("The People have had enough of their ruler's cruel punishments, directly joining up with the Bandit Army in this location.", -5));
                        hp += 3;
                    }
                    if (property != null && property is Pr_OrganisedDissent && property.charge >= 0.0)
                    {
                        property.influences.Add(new ReasonMsg("Some of the citizenry already opposed to the current ruler have joined up with the Bandit Army.", -5));
                        hp += 3;
                    }
                    if (property != null && property is Pr_New_Banditry && property.charge >= 0.0)
                    {
                        property.influences.Add(new ReasonMsg("The Bandits in this location are joining up with the Bandit Army.", -10));
                        hp += 5;
                    }
                    if (property != null && property is Pr_Crime && property.charge >= 0.0)
                    {
                        property.influences.Add(new ReasonMsg("The Criminals in this location are joining up with the Bandit Army.", -5));
                        hp += 4;
                    }
                }

            }
            
            if (hp > maxHp)
             {
                hp = maxHp;
             }
             if (maxHp <= 0)
             {
                maxHp = 1;
             }
        }

        public override void turnTickAI()
        {
            base.turnTickAI();
            if (subsumedUnit != null)
            {
                return;
            }

            if (base.location.settlement is SettlementHuman settlementHuman && settlementHuman.shadow < 0.75)
            {
                Task_RazeLocation task_RazeLocation = new Task_RazeLocation();
                task_RazeLocation.ignorePeace = true;
                task = task_RazeLocation;
            }
            else

            {
                bool banditryPresent = false; // A bool is the proper value type here.
                foreach (Property property in location.properties)
                {
                    if (property != null && property is Pr_New_Banditry && property.charge >= 0.0)
                    {
                        banditryPresent = true;
                        break; // Once you've found what you want, stop the loop from checking the rest. It's pointless and hurts performance.
                    }
                }

                if (banditryPresent)
                {
                    return;
                }

                int stepDistance = -1;
                List<Location> targetLocations = new List<Location>();
                foreach (Location loc in map.locations)
                {
                    if (loc.settlement is SettlementHuman setHuman && setHuman.shadow < 0.75) // Breaking up your if statements can make them easier to read, bug-fix, and maintain.
                    {
                        int dist = map.getStepDist(loc, location); // because we're checking the distance twice, we are gettng it once and storing the result.
                        if (stepDistance == -1 || dist <= stepDistance)
                        {
                            if (dist < stepDistance)
                            {
                                targetLocations.Clear();
                            }

                            stepDistance = dist;
                            targetLocations.Add(loc);
                        }

                    }
                }

                if (targetLocations.Count > 0)
                {
                    Location targetLocation;
                    if (targetLocations.Count > 1)
                    {
                        targetLocation = targetLocations[Eleven.random.Next(targetLocations.Count)]; // If there is more than 1 item in targetLocations, we get a random number from 0 to the final valid index (count - 1), and get the Location at that index. The result is a properly random selection from any number of Locations.
                    }
                    else
                    {
                        targetLocation = targetLocations[0]; // If there's only 1, we get the first.
                    }

                    task = new Task_GoToLocation(targetLocation);
                }
            }
        }

        public override bool isWanderingArmy()
        {
            return true;
        }

        public override bool costsEnthrallmentPoint()
        {
            if (subsumedUnit != null)
            {
                return true;
            }
            return false;
        }

        public override bool checkForDisband(Map map)
        {
            return false;
        }

        public override string getName()
        {
            if (subsumedUnit != null)
            {
                return "Brigand's Army";
            }
            return "Bandit Army";
        }

        public override Sprite getPortraitForeground()
        {
            if (subsumedUnit != null)
            {
                return subsumedUnit.getPortraitForeground();
            }

            return map.world.iconStore.banditry;
        }

        public override Sprite getPortraitForegroundAlt()
        {
            if (subsumedUnit != null)
            {
                return subsumedUnit.getPortraitForegroundAlt();
            }

            return map.world.iconStore.banditry;
        }

        public override bool isCommandable()
        {
            return subsumedUnit != null;
        }

        public override void die(Map map, string v, Person killer = null)
        {
            if (subsumedUnit != null)
            {
                // transfer the person back to the subsumed unit
                person.unit = subsumedUnit;
                person = null;

                // set the required variables on the subsumed unit
                subsumedUnit.isDead = false;
                subsumedUnit.location = location;

                // set the subsumed unit back on the map.
                location.units.Add(subsumedUnit);
                map.units.Add(subsumedUnit);
                map.overmind.agents.Add(subsumedUnit);

                if (GraphicalMap.selectedUnit == this)
                {
                    GraphicalMap.selectedUnit = subsumedUnit;
                }   
            }

            // Continue dieing as normal.
            base.die(map, v, killer);
        }

        public override int[] getPositiveTags()
        {
            return new int[1] { Tags.DISCORD};
        }
    }
}
