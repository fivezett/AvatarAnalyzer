using System.Collections.Generic;
using UnityEngine;

namespace AvatarAnalyzer
{
    public interface IFixFunction
    {
        /// <summary>
        /// Fix It!
        /// </summary>
        /// <param name="OIMG">ObjectItem MG </param>
        /// <param name="OI">Pbject Item </param>
        /// <param name="CMP">Source Component</param>
        /// <param name="TARGET_CMP">Target Components</param>
        void Fix(ObjectItemMG OIMG, ObjectItem OI, Component CMP, List<Component> TARGET_CMP);
        /// <summary>
        /// UI Draw by this Function.
        /// </summary>
        /// <returns>Redraw T or F</returns>
        bool Draw(ObjectItemMG OIMG, ObjectItem OI, Component CMP, List<Component> TARGET_CMP);
    }
}