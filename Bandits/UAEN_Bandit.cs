using Assets.Code;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bandits_and_Crime
{
    public class UAEN_Bandit : UAEN
    {
        public Rt_Bandit_Pillage pillagerit;

        public bool inDarkMode;

        public bool Travelling = false;

        List<Rt_Bandit_Robbery> robberyRituals = new List<Rt_Bandit_Robbery>();

        public Location GetDestination()
        {
            bool canArm = person.gold >= map.param.ch_armBanditsGoldCost;
            List<Location> primaryDestinations = new List<Location>();
            List<Location> secondaryDestinations = new List<Location>();
            bool secondaryFound = false;

            List<Location> locations = new List<Location> { location };
            HashSet<Location> locationHashes = new HashSet<Location> { location };
            int steps = 0;

            while (locations.Count > 0 && primaryDestinations.Count == 0 && steps < 256)
            {
                List<Location> newLocations = new List<Location>();
                foreach (Location loc in locations)
                {
                    if (canArm)
                    {
                        foreach (Challenge challenge in loc.GetChallenges())
                        { 
                            if (challenge is Ch_NewArmBandits armBandits && armBandits.valid() && armBandits.validFor(this))
                            {
                                primaryDestinations.Add(loc);
                                continue;
                            }
                        }
                        if (!secondaryFound)
                        {
                            if (loc.settlement is Set_MinorHuman &&
                                 (person.shadow < 0.5
                                || (!loc.soc.isDark() && (!(loc.soc is HolyOrder order)
                                || !order.worshipsThePlayer))))
                            {
                                secondaryDestinations.Add(loc);
                                continue;
                            }
                        }
                    }
                    else
                    {
                        if (loc.settlement is Set_MinorHuman &&
                                     (person.shadow < 0.5
                                    || (!loc.soc.isDark() && (!(loc.soc is HolyOrder order)
                                    || !order.worshipsThePlayer))))
                        {
                            primaryDestinations.Add(loc);
                            continue;
                        }
                    }
                    if(primaryDestinations.Count == 0)
                    {

                    }
                    foreach (Location neighbour in loc.getNeighbours())
                    {
                        if ((loc.map.awarenessOfUnderground >= 0.995 || loc.hex.z == neighbour.hex.z) && locationHashes.Contains(neighbour))
                        {
                            newLocations.Add(neighbour);
                            locationHashes.Add(neighbour);
                        }
                    }
                }
                if(canArm && secondaryDestinations.Count > 0)
                {
                    secondaryFound = true;
                }
                steps++;
            }
            if (primaryDestinations.Count > 0)
            {
                if (primaryDestinations.Count > 1)
                {
                    return primaryDestinations[Eleven.random.Next(primaryDestinations.Count)];
                }

                return primaryDestinations[0];
            }
            if (secondaryDestinations.Count > 0)
            {
                if (secondaryDestinations.Count > 1)
                {
                    return secondaryDestinations[Eleven.random.Next(secondaryDestinations.Count)];
                }

                return secondaryDestinations[0];
            }
            return null;
        }

        public bool TryRestAtBanditCamp()
        {
            Location homeLocationObj = map.locations[homeLocation];
            if (homeLocationObj.settlement is SettlementHuman)
            {
                foreach (Property property in homeLocationObj.properties)
                {
                    if (property is Pr_Bandity)
                    {
                        foreach (Challenge challenge in homeLocationObj.GetChallenges())
                        {
                            if (challenge is Ch_Rest && challenge.valid() && challenge.validFor(this))
                            {
                                if (location == homeLocationObj)
                                {
                                    task = new Task_PerformChallenge(challenge);
                                    return true;
                                }

                                task = new Task_GoToPerformChallenge(challenge);
                                return true;
                            }

                            break;
                        }

                        break;
                    }
                }
            }

            List<Location> targets = new List<Location>();
            List<Location> locations = new List<Location> { location };
            HashSet<Location> searchedLocations = new HashSet <Location> { location };
            int steps = 0;
            while (locations.Count > 0 && targets.Count == 0 && steps < 256)
            {
                List<Location> newLocations = new List<Location>();
                foreach (Location loc in locations)
                {
                    if (loc.settlement is SettlementHuman)
                    {
                        foreach (Property property in loc.properties)
                        {
                            if (property is Pr_Bandity)
                            {
                                foreach (Challenge challenge in loc.GetChallenges())
                                {
                                    if (challenge is Ch_Rest && challenge.valid() && challenge.validFor(this))
                                    {
                                        targets.Add(loc);
                                    }
                                    break;
                                }
                                break;
                            }
                        }
                    }

                    if(targets.Count == 0)
                    {
                        foreach (Location neighborloc in loc.getNeighbours())
                        {
                            if (!(searchedLocations).Contains(neighborloc))
                            {
                                newLocations.Add(neighborloc);
                                searchedLocations.Add(neighborloc);
                            }
                        }
                    } 
                }
                if (targets.Count > 0)
                {
                    break;
                }
                locations = newLocations;
                steps++;
            }

            if (targets.Count > 0)
            {
                Location best = null;
                if (targets.Count > 1) // If there are multiple results, get one at random.
                {
                    best = targets[Eleven.random.Next(targets.Count)];
                }
                else // If there is only one, we can just grab that without getting a random number.
                {
                    best = targets[0];
                }

                foreach (Challenge challenge in best.GetChallenges())
                {
                    if (challenge is Ch_Rest && challenge.valid() && challenge.validFor(this))
                    {
                        if(this.location == best)
                        {
                            task = new Task_PerformChallenge(challenge);
                        }
                        else
                        {
                            task = new Task_GoToPerformChallenge(challenge);
                        }
                    }
                    return true;
                } 
            }
            return false;
        }

        public UAEN_Bandit(Location loc, Society sg, Person p)
            : base(loc, sg, p)
        {
            p.shadow = loc.settlement.shadow;
            pillagerit = new Rt_Bandit_Pillage(loc);
            rituals.Add(pillagerit);
            p.extremeLikes.Add(Tags.DISCORD);
            p.stat_might = 3;
            p.stat_intrigue = 1;
            p.stat_lore = 1;
            p.stat_command = 2;
            p.species = map.species_human;

            if (p.shadow < 0.5)  //lifted from living wilds
            {
                inDarkMode = false;
            }
            else
            {
                inDarkMode = true;
            }
        }

        public override void turnTick(Map map)
        {
            List<Rt_Bandit_Robbery> newRobberyRituals = new List<Rt_Bandit_Robbery>();
            foreach (Unit unit in location.units)
            {
                if (!(unit is UA other))
                {
                    continue;
                }

                Rt_Bandit_Robbery robbery = null;
                foreach (Rt_Bandit_Robbery robberyRitual in robberyRituals)
                {
                    if (robbery.Target == other)
                    {
                        robbery = robberyRitual;
                        break;
                    }
                }

                if (robbery == null)
                {
                    robbery = new Rt_Bandit_Robbery(other.person.getLocation(), other);
                }
                newRobberyRituals.Add(robbery);
            }

            robberyRituals.Clear(); // Get rid of all the robbery rituals we populated last turn in the class variable.

            foreach (Ritual ritual in rituals.ToList())
            {
                if (ritual is Rt_Bandit_Robbery robbery) // We are only adding and removing Robbery Rituals.
                {
                    if (newRobberyRituals.Contains(robbery))
                    {
                        robberyRituals.Add(robbery); // It already exists. Log itback to our class variable.
                        newRobberyRituals.Remove(robbery); // Since it already exists, we don't need to add it in the next loop.
                    }
                    else
                    {
                        rituals.Remove(robbery); // It exists, but we don't need it this turn.
                    }
                }
            }
            rituals.AddRange(newRobberyRituals);
            robberyRituals.AddRange(newRobberyRituals);
        }

        public override void turnTickInner(Map map)
        {
            addMenace(0.5);
            addProfile(0.5);
            base.turnTickInner(map);
        }

        public override void turnTickAI()
        {
            //block for handling resting
            bool injured = hp < maxHp;
            if (!injured)
            {
                for (int i = 0; i < minions.Length; i++)
                {
                    if (minions[i] != null)
                    {
                        if (minions[i].hp < minions[i].getMaxHP())
                        {
                            injured = true;
                            break;
                        }
                    }
                }
            }
            if (injured || challengesSinceRest > 7 || (!Travelling && challengesSinceRest > 3 && Eleven.random.NextDouble() < 0.5))
            {
                Travelling = false;
                if (TryRestAtBanditCamp())
                {
                    return;
                }
            }


            //block for handling arming bandits

            bool canArmBandits = person.gold >= map.param.ch_armBanditsGoldCost;
            if (canArmBandits && location.settlement is SettlementHuman)
            {
                // Find instance of Ch_NewArmBandits
                foreach (Challenge challenge in location.GetChallenges())
                {
                    if (challenge is Ch_NewArmBandits)
                    {
                        // Check if it is valid, and perform if it is.
                        if (challenge.valid() && challenge.validFor(this))
                        {
                            task = new Task_PerformChallenge(challenge);
                            return;
                        }
                        break;
                    }
                }
            }

            //block for handling robbery
            // Iterate over each instance of Rt_Bandit_Robbery, and perform the first valid one.
            int wealth = -1;
            List<Rt_Bandit_Robbery> currentBests = new List<Rt_Bandit_Robbery>();

            foreach (Rt_Bandit_Robbery Robbery in robberyRituals)
            {
                // Check if instance of Rt_Bandit_Robbery is valid, and perform if it is.
                if (Robbery.valid() && Robbery.validFor(this))
                {
                    if (wealth <= Robbery.Target.person.gold) // If the target's wealth is better than or equal to the best result, add them to the list.
                    {
                        if (wealth < Robbery.Target.person.gold) //If the target is richer than the current best, clear the list of previous results.
                        {
                            currentBests.Clear();
                            wealth = Robbery.Target.person.gold;
                        }

                        currentBests.Add(Robbery);
                    }
                }
            }

            if (currentBests.Count > 0)
            {
                Rt_Bandit_Robbery best = null;
                if (currentBests.Count > 1) // If there are multiple best results, get one at random.
                {
                    best = currentBests[Eleven.random.Next(currentBests.Count)];
                }
                else // If there is only one, we can just grab that without getting a random number.
                {
                    best = currentBests[0];
                }

                task = new Task_PerformChallenge(best);
                return;
            }

            //block for handling pillaging
            // Check if Rt_Bandit_Pillage is valid, and perform if it is.
            if (pillagerit.valid() && pillagerit.validFor(this))
            {
                task = new Task_PerformChallenge(pillagerit);
                return;
            }

            if (canArmBandits && map.locations[homeLocation].settlement is SettlementHuman)
            {
                Location destination = GetDestination();
                if (destination != null) 
                {
                    foreach (Challenge challenge in destination.GetChallenges()) 
                    {
                        if (challenge is Ch_NewArmBandits)
                        {
                            // Check if it is valid, and perform if it is.
                            if (challenge.valid() && challenge.validFor(this))
                            {
                                task = new Task_GoToPerformChallenge(challenge);
                                Travelling = true;
                                return;
                            }
                            break;
                        }
                    }
                }
                foreach (Challenge challenge in map.locations[homeLocation].GetChallenges())
                {
                    if (challenge is Ch_NewArmBandits)
                    {
                        // Check if it is valid, and perform if it is.
                        if (challenge.valid() && challenge.validFor(this))
                        {
                            task = new Task_GoToPerformChallenge(challenge);
                            Travelling = true;
                            return;
                        }
                        break;
                    }
                }
            }
        }

        public override bool definesName()
        {
            return true;
        }

        public override string getName()
        {
            return "Bandit" + base.person.firstName; //does the game autogenerate names for em if I do this?
        }

        public override bool isCommandable()
        {
            return false;
        }

        public override Sprite getPortraitBackground()
        {
            return map.world.iconStore.standardBack;
        }

        public override Sprite getPortraitForeground()
        {
            return map.world.iconStore.banditry;
        }
    }
}