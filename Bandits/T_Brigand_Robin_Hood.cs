#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Shadows of Forbidden Gods\ShadowsOfForbiddenGods_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using Assets.Code;

namespace Bandits_and_Crime
{
    public class T_Brigand_Robin_Hood : Trait
    {
        public override string getName()
        {
            return "Robin Hood";
        }

        public override string getDesc()
        {
            return "Increases any existing Banditry by 2% per turn while in the same location.";
        }

        public override int getMaxLevel()
        {
            return 1;
        }

        public override void turnTick(Person p)
        {
            foreach (Property property in p.unit.location.properties)
            {
                if (property is Pr_New_Banditry Bandits)
                {
                    Bandits.influences.Add(new ReasonMsg("Robin Hood Trait", 2.0));
                    break;
                }
            }
        }

        public override int[] getTags()
        {
            return new int[0];
        }
    }
}