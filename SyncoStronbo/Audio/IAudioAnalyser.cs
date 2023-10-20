﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncoStronbo.Audio
{
    interface IAudioAnalyser{

        void Init();

        double GetHighLevel();
        double GetMidLevel();
        double GetLowLevel();
    }
}
