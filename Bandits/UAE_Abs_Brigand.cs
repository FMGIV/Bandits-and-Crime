using Assets.Code;
using UnityEngine;

namespace Bandits_and_Crime
{
    public class UAE_Abs_Brigand : UAE_Abstraction
    {
        public UAE_Abs_Brigand (Map map)
            : base(map, -1)
        {
            base.map = map;
        }

        public override string getName()
        {
            return "The Brigand";
        }

        public override string getDesc()
        {
            return "The Brigand specializes in increasing and using banditry for distraction and occassional destruction. He can start new banditry modifiers in minor settlements, hide among the bandits, or provoke the ruler into cracking down on the people. (Unrest does wonders for driving people to banditry.)";
        }

        public override string getFlavour()
        {
            return "The Brigand was once a soldier, fighting for his country and coin. Then his country lost the war it was fighting, and he was no longer a soldier. Having fallen on hard times, he saw no other option but to turn to banditry to make a living.";
        }

        public override string getRestrictions()
        {
            return "Requires a Minor Settlement";
        }

        public override Sprite getBackground()
        {
            return map.world.iconStore.standardBack;
        }

        public override Sprite getForeground()
        {
            return map.world.textureStore.agent_banditKing;
        }

        public override int getStatMight()
        {
            return 3;
        }

        public override int getStatLore()
        {
            return 1;
        }

        public override int getStatIntrigue()
        {
            return 1;
        }

        public override int getStatCommand()
        {
            return 4;
        }

        public override bool validTarget(Location loc)
        {
            if (World.allowAllAgents)
            {
                return true;
            }

            if (map.overmind.nEnthralled >= map.overmind.getAgentCap())
            {
                return false;
            }

            return loc.settlement is Set_MinorHuman;
        }

        public override void createAgent(Location target)
        {
            if (map.overmind.god is God_Eternity god_Eternity)
            {
                god_Eternity.agentBuffer.Add(this);
            }

            UAE_Brigand Brigand = new UAE_Brigand(target, target.map.soc_dark);
            Brigand.person.stat_might = getStatMight();
            Brigand.person.stat_lore = getStatLore();
            Brigand.person.stat_intrigue = getStatIntrigue();
            Brigand.person.stat_command = getStatCommand();
            map.units.Add(Brigand);
            target.units.Add(Brigand);
            map.overmind.agents.Add(Brigand);
            map.overmind.availableEnthrallments--;
            Brigand.person.shadow = 1.0;
            Brigand.person.skillPoints++;
            Brigand.person.state = Person.personState.enthralled;
            Brigand.person.hates.Clear();
            Brigand.person.extremeHates.Clear();
            Brigand.person.likes.Clear();
            Brigand.person.extremeLikes.Clear();

            if (!map.automatic)
            {
                GraphicalMap.selectedUnit = Brigand;
                map.world.prefabStore.popAgentLevelUp(Brigand);
                GraphicalMap.panTo(target.hex);
            }

            target.map.overmind.agentsUnique.Remove(this);
        }
    }
}
