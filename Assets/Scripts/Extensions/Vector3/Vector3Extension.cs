using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
*	Author  - Ferri de Lange
*	Date	- 17.05.20
*/

namespace FlockingSimulator.Extensions
{

    ///<summary>
    /// Extensions for the Vector3 struct
    ///</summary>
    public static class Vector3Extension
    {
        #region Methods
        #region Public
        /// <summary>
        /// Limits the Vector to a certain magnitude, it can go below the limit, but not above
        /// </summary>
        /// <param name="vector">Extension object</param>
        /// <param name="limit">The maximium magnitude of the vector</param>
        /// <returns>The new limited Vector3</returns>
        public static Vector3 Limit(this Vector3 vector, float limit)
        {
            if(vector.magnitude > limit)
            {
                vector.Normalize();
                vector *= limit;
            }

            return vector;
        }
        #endregion
        #endregion
    }
}
