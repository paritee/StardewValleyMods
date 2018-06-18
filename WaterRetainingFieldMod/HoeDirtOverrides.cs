using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.TerrainFeatures;

namespace WaterRetainingFieldMod
{
    public class HoeDirtOverrides
    {
        internal static Dictionary<Vector2, int> TileLocationState = new Dictionary<Vector2, int>();

        public static bool DayUpdatePrefix(HoeDirt __instance, ref int __state)
        {
            __state = __instance.state;
            return true;
        }

        public static void DayUpdatePostfix(HoeDirt __instance, ref GameLocation environment, ref Vector2 tileLocation, ref int __state)
        {
            if (environment is Farm farm)
            {
                if (__state == 1 && __instance.fertilizer == 370 || __instance.fertilizer == 371)
                {
                    if (TileLocationState.ContainsKey(tileLocation))
                    {
                        __instance.state = TileLocationState[tileLocation];
                        return;
                    }
                    else
                    {
                        TileLocationState[tileLocation] = __instance.state;
                        AddStateAdjacentFertilizedTiles(farm, tileLocation, __instance.state, __instance.fertilizer);
                    }
                }
            }
        }

        private static void AddStateAdjacentFertilizedTiles(Farm farm, Vector2 tileLocation, int stateValue, int fertilizer)
        {
            Vector2[] adjasent = new Vector2[]
            {
                new Vector2(tileLocation.X, tileLocation.Y + 1)
                , new Vector2(tileLocation.X + 1, tileLocation.Y)
                , new Vector2(tileLocation.X - 1, tileLocation.Y)
                , new Vector2(tileLocation.X, tileLocation.Y - 1)
            };
            foreach (var adjacentTileLocation in adjasent)
            {
                if (!TileLocationState.ContainsKey(adjacentTileLocation) && farm.terrainFeatures.ContainsKey(adjacentTileLocation) && farm.terrainFeatures[adjacentTileLocation] is HoeDirt hoeDirt)
                {
                    if (hoeDirt.state == 1 && hoeDirt.fertilizer == fertilizer)
                    {
                        TileLocationState[adjacentTileLocation] = stateValue;
                        AddStateAdjacentFertilizedTiles(farm, adjacentTileLocation, stateValue, fertilizer);
                    }
                }
            }

        }
    }
}
