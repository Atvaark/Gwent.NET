using System;
using System.Xml.Serialization;

namespace Gwent.NET.Model
{
    [Flags]
    public enum GwintEffect
    {
        [XmlEnum("EFFECT_NONE")]
        EffectNone = 0,

        [XmlEnum("CP_CLEAR_WEATHER")]
        CpClearWeather = 1 << 0,

        [XmlEnum("CP_PICK_FROST_CARD")]
        CpPickFrostCard = 1 << 1,

        [XmlEnum("CP_PICK_FOG_CARD")]
        CpPickFogCard = 1 << 2,

        [XmlEnum("CP_PICK_RAIN_CARD")]
        CpPickRainCard = 1 << 3,

        [XmlEnum("CP_PICK_WEATHER_CARD")]
        CpPickWeatherCard = 1 << 4,

        [XmlEnum("CP_MELEE_HORN")]
        CpMeleeHorn = 1 << 5,

        [XmlEnum("CP_RANGE_HORN")]
        CpRangeHorn = 1 << 6,

        [XmlEnum("CP_SIEGE_HORN")]
        CpSiegeHorn = 1 << 7,

        [XmlEnum("CP_MELEE_SCORCH")]
        CpMeleeScorch = 1 << 8,

        [XmlEnum("CP_SIEGE_SCORCH")]
        CpSiegeScorch = 1 << 9,

        [XmlEnum("CP_RESURECT_CARD")]
        CpResurectCard = 1 << 10,

        [XmlEnum("CP_RESURECT_FROM_ENEMY")]
        CpResurectFromEnemy = 1 << 11,

        [XmlEnum("CP_VIEW_3_ENEMY_CARDS")]
        CpView3EnemyCards = 1 << 12,

        [XmlEnum("CP_COUNTER_KING_ABLILITY")]
        CpCounterKingAblility = 1 << 13,

        [XmlEnum("CP_11TH_CARD")]
        Cp11ThCard = 1 << 14,

        [XmlEnum("CP_BIN2_PICK1")]
        CpBin2Pick1 = 1 << 15,

        [XmlEnum("EFFECT_MELEE")]
        EffectMelee = 1 << 16,

        [XmlEnum("EFFECT_RANGED")]
        EffectRanged = 1 << 17,

        [XmlEnum("EFFECT_SIEGE")]
        EffectSiege = 1 << 18,

        [XmlEnum("EFFECT_SCORCH")]
        EffectScorch = 1 << 19,

        [XmlEnum("EFFECT_HORN")]
        EffectHorn = 1 << 20,

        [XmlEnum("EFFECT_IMPROVE_NEIGHBOURS")]
        EffectImproveNeighbours = 1 << 21,

        [XmlEnum("EFFECT_SUMMON_CLONES")]
        EffectSummonClones = 1 << 22,

        [XmlEnum("EFFECT_NURSE")]
        EffectNurse = 1 << 23,

        [XmlEnum("EFFECT_SAME_TYPE_MORALE")]
        EffectSameTypeMorale = 1 << 24,

        [XmlEnum("EFFECT_DRAW_X2")]
        EffectDrawX2 = 1 << 25,

        [XmlEnum("EFFECT_UNSUMMON_DUMMY")]
        EffectUnsummonDummy = 1 << 26,

        [XmlEnum("EFFECT_CLEAR_SKY")]
        EffectClearSky = 1 << 27,


    }
}