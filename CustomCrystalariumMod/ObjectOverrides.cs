
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace CustomCrystalariumMod
{
    internal class ObjectOverrides
    {
        public static bool GetMinutesForCrystalarium(ref int whichGem, ref int __result)
        {
            if (DataLoader.CrystalariumData.ContainsKey(whichGem))
            {
                __result = DataLoader.CrystalariumData[whichGem];
                return false;
            }
            else
            {
                var itemCategory = new Object(whichGem, 1, false).Category;
                if (DataLoader.CrystalariumData.ContainsKey(itemCategory))
                {
                    __result = DataLoader.CrystalariumData[itemCategory];
                    return false;
                }
            }
            return true;
        }

        public static bool PerformObjectDropInAction(ref Object __instance, ref Item dropIn, ref bool probe, ref StardewValley.Farmer who, ref bool __result)
        {
            if (dropIn is Object object1)
            {
                if (!(__instance.heldObject != null && !__instance.Name.Equals("Recycling Machine") &&
                      !__instance.Name.Equals("Crystalarium") ||object1 != null && (bool) (object1.bigCraftable)))
                {
                    if (__instance.Name.Equals("Crystalarium"))
                    {
                        if ((__instance.heldObject == null || __instance.heldObject.ParentSheetIndex != object1.ParentSheetIndex))
                        { 
                            int minutesUntilReady;
                            if (DataLoader.CrystalariumData.ContainsKey(object1.ParentSheetIndex))
                            {
                                minutesUntilReady = DataLoader.CrystalariumData[object1.ParentSheetIndex];
                            }
                            else if (DataLoader.CrystalariumData.ContainsKey(object1.Category))
                            {
                                minutesUntilReady = DataLoader.CrystalariumData[object1.Category];
                            }
                            else
                            {
                                return true;
                            }
                            if ((bool)__instance.bigCraftable && !probe &&
                                (object1 != null && __instance.heldObject == null))
                            {
                                __instance.scale.X = 5f;
                            }
                            __instance.heldObject = (Object)object1.getOne();
                            if (!probe)
                            {
                                Game1.playSound("select");
                                __instance.minutesUntilReady = minutesUntilReady;
                            }
                            __result = true;
                            return false;
                        }
                    }

                }
            }

            return true;
        }
    }
}
