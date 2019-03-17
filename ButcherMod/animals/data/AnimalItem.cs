﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalHusbandryMod.animals.data
{
    public interface AnimalItem 
    {
        string Name { get; set; }
        int MinimalNumberOfMeat { get; set; }
        int MaximumNumberOfMeat { get; set; }
    }
}
