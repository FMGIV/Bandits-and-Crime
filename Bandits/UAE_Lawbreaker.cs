#region Assembly Whisperer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\workshop\content\1741640\3236518418\Whisperer.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using Assets.Code;
using UnityEngine;

namespace Bandits_and_Crime
{
    public class UAE_Lawbreaker : UAE
    {

        public UAE_Lawbreaker(Location loc, Society sg)
            : base(loc, sg)
        {
            base.person.stat_might = 2;
            base.person.stat_command = 2;
            base.person.stat_intrigue = 4;
            base.person.stat_lore = 1;
            base.person.isMale = true;
            rituals.Add(new Rt_Lawbreaker_Organize_Crime(loc));
            rituals.Add(new Rt_Lawbreaker_Start_Crime_Gang(loc));
            rituals.Add(new Rt_Lawbreaker_Infiltrate_Trade_Route(loc, this));
            rituals.Add(new Rt_Lawbreaker_Crime_Organize_Dissent(loc));
            base.person.species = map.species_human;
        }

        public override string getName()
        {
            if (base.person.overrideName != null && base.person.overrideName.Length != 0)
            {
                return base.person.overrideName;
            }

            return "The Lawbreaker";
        }

        public override bool isCommandable()
        {
            return true;
        }

        public override bool hasStartingTraits()
        {
            return true;
        }

        public override void turnTick(Map map)
        {
            base.turnTick(map);
        }

        public override void onMove(Location location, Location loc)
        {
            base.onMove(location, loc);
        }

        public override List<Trait> getStartingTraits()
        {
            List<Trait> list = new List<Trait>();
            list.Add(new T_Godfather());
            list.Add(new T_Underworld_Connections());
            list.Add(new T_Illicit_Business());
            return list;
        }

        public override Sprite getPortraitBackground()
        {
            return map.world.iconStore.standardBack;
        }

        public override Sprite getPortraitForeground()
        {
            return EventManager.getImg("BanditsandCrime.Lawbreaker.png");
        }
    }
#if false // Decompilation log
    '13' items in cache
    ------------------
    Resolve: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
    Found single assembly: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
    Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\mscorlib.dll'
    ------------------
    Resolve: 'Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
    Found single assembly: 'Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
    Load from: 'C:\Program Files (x86)\Steam\steamapps\common\Shadows of Forbidden Gods\ShadowsOfForbiddenGods_Data\Managed\Assembly-CSharp.dll'
    ------------------
    Resolve: 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
    Found single assembly: 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
    Load from: 'C:\Program Files (x86)\Steam\steamapps\common\Shadows of Forbidden Gods\ShadowsOfForbiddenGods_Data\Managed\UnityEngine.CoreModule.dll'
    ------------------
    Resolve: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
    Found single assembly: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
    Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Core.dll'
#endif
}
