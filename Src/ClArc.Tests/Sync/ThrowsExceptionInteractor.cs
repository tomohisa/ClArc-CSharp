﻿using System;
using ClArc.Core;

namespace ClArc.Tests.Sync
{
    public class ThrowsExceptionInteractor : IInputPort<InputData, OutputData>
    {
        public OutputData Handle(InputData inputData)
        {
            throw new Exception();
        }
    }
}