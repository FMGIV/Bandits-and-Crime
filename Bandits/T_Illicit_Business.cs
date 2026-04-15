#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Shadows of Forbidden Gods\ShadowsOfForbiddenGods_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using Assets.Code;

namespace Bandits_and_Crime
{
    public class T_Illicit_Business : Trait
    {
        public override string getName()
        {
            return "Illicit Business";
        }

        public override string getDesc()
        {
            return "Increases the Crime Prosperity debuff by 2% each turn (offsetting the normal 1% loss).";
        }

        public override int getMaxLevel()
        {
            return 1;
        }

        public override void turnTick(Person p)
        {
            foreach (Property property in p.unit.location.properties)
            {
                if (property is Pr_Crime Crime)
                {
                    Crime.extraprosperitydebuff -= 0.2;
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