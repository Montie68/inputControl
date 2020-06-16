using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Class_Container
{
    #region NeuBool
    public class NeuBool
    {
        int state = 0; // -1 = false, 0 = neutral, 1 = true.

        public void SetTrue()
        {
            state = 1;
        }

        public void SetNeutral()
        {
            state = 0;
        }

        public void SetFalse()
        {
            state = -1;
        }

        public bool IsTrue()
        {
            if (state == 1) return true;
            return false;
        }

        public bool IsNeutral()
        {
            if (state == 0) return true;
            return false;
        }

        public bool IsFalse()
        {
            if (state == -1) return true;
            return false;
        }
    }
    #endregion

    #region 

    #endregion
}
