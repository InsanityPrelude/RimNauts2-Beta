﻿using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimNauts2.Biome.Patch {
    [HarmonyLib.HarmonyPatch(typeof(RimWorld.PawnGroupMakerUtility), "GeneratePawns")]
    class PawnGroupMakerUtility_GeneratePawns {
        public static IEnumerable<Pawn> Postfix(IEnumerable<Pawn> __result, RimWorld.PawnGroupMakerParms parms, bool warnOnZeroResults) {
            RimWorld.BiomeDef biome = Find.WorldGrid[parms.tile].biome;
            bool no_oxygen = Universum.Utilities.Cache.allowed_utility(biome, "universum.vacuum_suffocation");
            bool decompression = Universum.Utilities.Cache.allowed_utility(biome, "universum.vacuum_decompression");
            bool requires_spacesuit = no_oxygen || decompression;
            if (requires_spacesuit) {
                return apply_spacesuits(__result);
            } else return __result;
        }

        public static IEnumerable<Pawn> apply_spacesuits(IEnumerable<Pawn> pawns) {
            if (pawns == null || !pawns.Any()) yield break;
            ThingDef spacesuit_helmet = ThingDef.Named("RimNauts2_Apparel_SpaceSuit_Head");
            ThingDef spacesuit_armor = ThingDef.Named("RimNauts2_Apparel_SpaceSuit_Body");
            foreach (Pawn pawn in pawns) {
                if (pawn != null) {
                    pawn.apparel.Wear((RimWorld.Apparel) ThingMaker.MakeThing(spacesuit_helmet), false);
                    pawn.apparel.Wear((RimWorld.Apparel) ThingMaker.MakeThing(spacesuit_armor), false);
                }
                yield return pawn;
            }
        }
    }
}
