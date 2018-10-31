using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2._0.Abilities
{
    interface IAbilityCaster
    {
        IAbility[] Abilities
        {
            get;
            set;
        }
    }
}
