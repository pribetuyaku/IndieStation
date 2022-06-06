using System;
using System.Collections.Generic;

namespace AppFinal.Models
{
    /// <summary>
    /// Region Enumerator
    /// </summary>
    public enum Region
    {
        EU,
        BR,
        GLOBAL
    }

    /// <summary>
    /// Getter for region
    /// </summary>
    public static class GetRegion
    {
        /// <summary>
        /// Get all available regions as strings
        /// </summary>
        /// <returns>LinkedList of regions</returns>
        public static LinkedList<string> GetRegions()
        {
            LinkedList<string> regions = new LinkedList<string>();
            foreach (var region in (Region[]) Enum.GetValues(typeof(Region)))
            {
                regions.AddLast(GetName(region));
            }

            return regions;
        }

        /// <summary>
        /// Get region name from Region
        /// </summary>
        /// <param name="region">Region</param>
        /// <returns>string name</returns>
        public static string GetName(Region region)
        {
            switch (region)
            {
                case Region.BR:
                    return "BR";
                case Region.EU:
                    return "EU";
                case Region.GLOBAL:
                    return "GLOBAL";
                default:
                    return null;
            }
        }

        /// <summary>
        /// Get Region from region name
        /// </summary>
        /// <param name="region">string name</param>
        /// <returns>Region region</returns>
        public static Region GetRegionEnum(string region)
        {
            switch (region)
            {
                case "BR":
                    return Region.BR;
                case "EU":
                    return Region.EU;
                case "GLOBAL":
                    return Region.GLOBAL;
                default:
                    return Region.GLOBAL;
            }
        }
    }
}