using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using KnockedDownHeroesInfluencesTroops.Data;
using KnockedDownHeroesInfluencesTroops.Diagnostics;

namespace KnockedDownHeroesInfluencesTroops.Core;

// The only place mod code reads settings from. MCM loaded -> MCM. MCM not loaded -> ModConfiguration.xml.
internal static class ModSettings
{
    private const string McmAssemblyName = "MCMv5";

    private static ModConfiguration _xml = new();
    private static XmlSerializer? _serializer;
    private static bool _useMcm;

    #region Main settings
    internal static bool EnableThisModification => _useMcm ? McmEnableThisModification() : _xml.MainSettings.EnableThisModification;

    internal static bool ShowOnScreenNotifications => _useMcm ? McmShowOnScreenNotifications() : _xml.MainSettings.ShowOnScreenNotifications;

    internal static bool HideUnassignedHeroNotifications => _useMcm ? McmHideUnassignedHeroNotifications() : _xml.MainSettings.HideUnassignedHeroNotifications;

    internal static bool DisableKnockdownSounds => _useMcm ? McmDisableKnockdownSounds() : _xml.MainSettings.DisableKnockdownSounds;

    internal static bool PlayHeroFallReactionVoiceLines => _useMcm ? McmPlayHeroFallReactionVoiceLines() : _xml.MainSettings.PlayHeroFallReactionVoiceLines;
    #endregion

    #region Morale penalties
    internal static int MoraleChangeWhenTroopKillsUnassignedHero => _useMcm ? McmMoraleChangeWhenTroopKillsUnassignedHero() : _xml.MoralePenalties.MoraleChangeWhenTroopKillsUnassignedHero;

    internal static int MoraleChangeWhenTroopKillsCaptainHero => _useMcm ? McmMoraleChangeWhenTroopKillsCaptainHero() : _xml.MoralePenalties.MoraleChangeWhenTroopKillsCaptainHero;

    internal static int MoraleChangeWhenTroopKillsGeneralHero => _useMcm ? McmMoraleChangeWhenTroopKillsGeneralHero() : _xml.MoralePenalties.MoraleChangeWhenTroopKillsGeneralHero;

    internal static int MoraleChangeWhenUnassignedHeroKillsUnassignedHero => _useMcm ? McmMoraleChangeWhenUnassignedHeroKillsUnassignedHero() : _xml.MoralePenalties.MoraleChangeWhenUnassignedHeroKillsUnassignedHero;

    internal static int MoraleChangeWhenUnassignedHeroKillsCaptainHero => _useMcm ? McmMoraleChangeWhenUnassignedHeroKillsCaptainHero() : _xml.MoralePenalties.MoraleChangeWhenUnassignedHeroKillsCaptainHero;

    internal static int MoraleChangeWhenUnassignedHeroKillsGeneralHero => _useMcm ? McmMoraleChangeWhenUnassignedHeroKillsGeneralHero() : _xml.MoralePenalties.MoraleChangeWhenUnassignedHeroKillsGeneralHero;

    internal static int MoraleChangeWhenCaptainHeroKillsUnassignedHero => _useMcm ? McmMoraleChangeWhenCaptainHeroKillsUnassignedHero() : _xml.MoralePenalties.MoraleChangeWhenCaptainHeroKillsUnassignedHero;

    internal static int MoraleChangeWhenCaptainHeroKillsCaptainHero => _useMcm ? McmMoraleChangeWhenCaptainHeroKillsCaptainHero() : _xml.MoralePenalties.MoraleChangeWhenCaptainHeroKillsCaptainHero;

    internal static int MoraleChangeWhenCaptainHeroKillsGeneralHero => _useMcm ? McmMoraleChangeWhenCaptainHeroKillsGeneralHero() : _xml.MoralePenalties.MoraleChangeWhenCaptainHeroKillsGeneralHero;

    internal static int MoraleChangeWhenGeneralHeroKillsUnassignedHero => _useMcm ? McmMoraleChangeWhenGeneralHeroKillsUnassignedHero() : _xml.MoralePenalties.MoraleChangeWhenGeneralHeroKillsUnassignedHero;

    internal static int MoraleChangeWhenGeneralHeroKillsCaptainHero => _useMcm ? McmMoraleChangeWhenGeneralHeroKillsCaptainHero() : _xml.MoralePenalties.MoraleChangeWhenGeneralHeroKillsCaptainHero;

    internal static int MoraleChangeWhenGeneralHeroKillsGeneralHero => _useMcm ? McmMoraleChangeWhenGeneralHeroKillsGeneralHero() : _xml.MoralePenalties.MoraleChangeWhenGeneralHeroKillsGeneralHero;
    #endregion

    #region Morale gains
    internal static int MoraleGainWhenTroopKillsUnassignedHero => _useMcm ? McmMoraleGainWhenTroopKillsUnassignedHero() : _xml.MoraleGains.MoraleGainWhenTroopKillsUnassignedHero;

    internal static int MoraleGainWhenTroopKillsCaptainHero => _useMcm ? McmMoraleGainWhenTroopKillsCaptainHero() : _xml.MoraleGains.MoraleGainWhenTroopKillsCaptainHero;

    internal static int MoraleGainWhenTroopKillsGeneralHero => _useMcm ? McmMoraleGainWhenTroopKillsGeneralHero() : _xml.MoraleGains.MoraleGainWhenTroopKillsGeneralHero;

    internal static int MoraleGainWhenUnassignedHeroKillsUnassignedHero => _useMcm ? McmMoraleGainWhenUnassignedHeroKillsUnassignedHero() : _xml.MoraleGains.MoraleGainWhenUnassignedHeroKillsUnassignedHero;

    internal static int MoraleGainWhenUnassignedHeroKillsCaptainHero => _useMcm ? McmMoraleGainWhenUnassignedHeroKillsCaptainHero() : _xml.MoraleGains.MoraleGainWhenUnassignedHeroKillsCaptainHero;

    internal static int MoraleGainWhenUnassignedHeroKillsGeneralHero => _useMcm ? McmMoraleGainWhenUnassignedHeroKillsGeneralHero() : _xml.MoraleGains.MoraleGainWhenUnassignedHeroKillsGeneralHero;

    internal static int MoraleGainWhenCaptainHeroKillsUnassignedHero => _useMcm ? McmMoraleGainWhenCaptainHeroKillsUnassignedHero() : _xml.MoraleGains.MoraleGainWhenCaptainHeroKillsUnassignedHero;

    internal static int MoraleGainWhenCaptainHeroKillsCaptainHero => _useMcm ? McmMoraleGainWhenCaptainHeroKillsCaptainHero() : _xml.MoraleGains.MoraleGainWhenCaptainHeroKillsCaptainHero;

    internal static int MoraleGainWhenCaptainHeroKillsGeneralHero => _useMcm ? McmMoraleGainWhenCaptainHeroKillsGeneralHero() : _xml.MoraleGains.MoraleGainWhenCaptainHeroKillsGeneralHero;

    internal static int MoraleGainWhenGeneralHeroKillsUnassignedHero => _useMcm ? McmMoraleGainWhenGeneralHeroKillsUnassignedHero() : _xml.MoraleGains.MoraleGainWhenGeneralHeroKillsUnassignedHero;

    internal static int MoraleGainWhenGeneralHeroKillsCaptainHero => _useMcm ? McmMoraleGainWhenGeneralHeroKillsCaptainHero() : _xml.MoraleGains.MoraleGainWhenGeneralHeroKillsCaptainHero;

    internal static int MoraleGainWhenGeneralHeroKillsGeneralHero => _useMcm ? McmMoraleGainWhenGeneralHeroKillsGeneralHero() : _xml.MoraleGains.MoraleGainWhenGeneralHeroKillsGeneralHero;
    #endregion

    #region Notification texts
    internal static string FriendlyGeneralFallenNotification => _useMcm ? McmFriendlyGeneralFallenNotification() : _xml.NotificationTexts.FriendlyGeneralFallenNotification;

    internal static string EnemyGeneralFallenNotification => _useMcm ? McmEnemyGeneralFallenNotification() : _xml.NotificationTexts.EnemyGeneralFallenNotification;

    internal static string FriendlyInfantryCaptainFallenNotification => _useMcm ? McmFriendlyInfantryCaptainFallenNotification() : _xml.NotificationTexts.FriendlyInfantryCaptainFallenNotification;

    internal static string FriendlyRangedCaptainFallenNotification => _useMcm ? McmFriendlyRangedCaptainFallenNotification() : _xml.NotificationTexts.FriendlyRangedCaptainFallenNotification;

    internal static string FriendlyCavalryCaptainFallenNotification => _useMcm ? McmFriendlyCavalryCaptainFallenNotification() : _xml.NotificationTexts.FriendlyCavalryCaptainFallenNotification;

    internal static string FriendlyHorseArchersCaptainFallenNotification => _useMcm ? McmFriendlyHorseArchersCaptainFallenNotification() : _xml.NotificationTexts.FriendlyHorseArchersCaptainFallenNotification;

    internal static string EnemyInfantryCaptainFallenNotification => _useMcm ? McmEnemyInfantryCaptainFallenNotification() : _xml.NotificationTexts.EnemyInfantryCaptainFallenNotification;

    internal static string EnemyRangedCaptainFallenNotification => _useMcm ? McmEnemyRangedCaptainFallenNotification() : _xml.NotificationTexts.EnemyRangedCaptainFallenNotification;

    internal static string EnemyCavalryCaptainFallenNotification => _useMcm ? McmEnemyCavalryCaptainFallenNotification() : _xml.NotificationTexts.EnemyCavalryCaptainFallenNotification;

    internal static string EnemyHorseArchersCaptainFallenNotification => _useMcm ? McmEnemyHorseArchersCaptainFallenNotification() : _xml.NotificationTexts.EnemyHorseArchersCaptainFallenNotification;

    internal static string FriendlyUnassignedHeroFallenNotification => _useMcm ? McmFriendlyUnassignedHeroFallenNotification() : _xml.NotificationTexts.FriendlyUnassignedHeroFallenNotification;

    internal static string EnemyUnassignedHeroFallenNotification => _useMcm ? McmEnemyUnassignedHeroFallenNotification() : _xml.NotificationTexts.EnemyUnassignedHeroFallenNotification;
    #endregion

    #region Technical settings
    internal static int UpdateIntervalInSeconds => _useMcm ? McmUpdateIntervalInSeconds() : _xml.TechnicalSettings.UpdateIntervalInSeconds;

    internal static bool LoggingEnabled => _useMcm ? McmLoggingEnabled() : _xml.TechnicalSettings.LoggingEnabled;
    #endregion

    private static XmlSerializer Serializer => _serializer ??= new XmlSerializer(typeof(ModConfiguration));

    // Called on every game start (campaign or custom battle).
    internal static void Reload()
    {
        _useMcm = false;
        _xml = new ModConfiguration();

        if (IsMcmLoaded())
        {
            if (McmHasSettings())
            {
                _useMcm = true;
                return;
            }

            KdhitLog.Warn("MCM is loaded but returned no settings for this mod. Using " + ModInfo.ConfigurationFileName + " instead.");
        }

        _xml = LoadFromFile();
    }

    private static bool IsMcmLoaded()
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        for (int i = 0; i < assemblies.Length; i++)
        {
            if (assemblies[i].GetName().Name == McmAssemblyName) return true;
        }

        return false;
    }

    // Every method that touches MCMSettings must be NoInlining and may only be called when MCM is loaded.
    // Otherwise the game tries to load MCM's classes and crashes when MCM is not installed.
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmHasSettings() => MCMSettings.Instance != null;

    #region MCM reads
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmEnableThisModification() => MCMSettings.Instance?.EnableThisModification ?? ModConfiguration.EnableThisModificationDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmShowOnScreenNotifications() => MCMSettings.Instance?.ShowOnScreenNotifications ?? ModConfiguration.ShowOnScreenNotificationsDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmHideUnassignedHeroNotifications() => MCMSettings.Instance?.HideUnassignedHeroNotifications ?? ModConfiguration.HideUnassignedHeroNotificationsDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmDisableKnockdownSounds() => MCMSettings.Instance?.DisableKnockdownSounds ?? ModConfiguration.DisableKnockdownSoundsDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmPlayHeroFallReactionVoiceLines() => MCMSettings.Instance?.PlayHeroFallReactionVoiceLines ?? ModConfiguration.PlayHeroFallReactionVoiceLinesDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMoraleChangeWhenTroopKillsUnassignedHero() => MCMSettings.Instance?.MoraleChangeWhenTroopKillsUnassignedHero ?? ModConfiguration.MoraleChangeWhenTroopKillsUnassignedHeroDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMoraleChangeWhenTroopKillsCaptainHero() => MCMSettings.Instance?.MoraleChangeWhenTroopKillsCaptainHero ?? ModConfiguration.MoraleChangeWhenTroopKillsCaptainHeroDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMoraleChangeWhenTroopKillsGeneralHero() => MCMSettings.Instance?.MoraleChangeWhenTroopKillsGeneralHero ?? ModConfiguration.MoraleChangeWhenTroopKillsGeneralHeroDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMoraleChangeWhenUnassignedHeroKillsUnassignedHero() => MCMSettings.Instance?.MoraleChangeWhenUnassignedHeroKillsUnassignedHero ?? ModConfiguration.MoraleChangeWhenUnassignedHeroKillsUnassignedHeroDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMoraleChangeWhenUnassignedHeroKillsCaptainHero() => MCMSettings.Instance?.MoraleChangeWhenUnassignedHeroKillsCaptainHero ?? ModConfiguration.MoraleChangeWhenUnassignedHeroKillsCaptainHeroDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMoraleChangeWhenUnassignedHeroKillsGeneralHero() => MCMSettings.Instance?.MoraleChangeWhenUnassignedHeroKillsGeneralHero ?? ModConfiguration.MoraleChangeWhenUnassignedHeroKillsGeneralHeroDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMoraleChangeWhenCaptainHeroKillsUnassignedHero() => MCMSettings.Instance?.MoraleChangeWhenCaptainHeroKillsUnassignedHero ?? ModConfiguration.MoraleChangeWhenCaptainHeroKillsUnassignedHeroDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMoraleChangeWhenCaptainHeroKillsCaptainHero() => MCMSettings.Instance?.MoraleChangeWhenCaptainHeroKillsCaptainHero ?? ModConfiguration.MoraleChangeWhenCaptainHeroKillsCaptainHeroDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMoraleChangeWhenCaptainHeroKillsGeneralHero() => MCMSettings.Instance?.MoraleChangeWhenCaptainHeroKillsGeneralHero ?? ModConfiguration.MoraleChangeWhenCaptainHeroKillsGeneralHeroDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMoraleChangeWhenGeneralHeroKillsUnassignedHero() => MCMSettings.Instance?.MoraleChangeWhenGeneralHeroKillsUnassignedHero ?? ModConfiguration.MoraleChangeWhenGeneralHeroKillsUnassignedHeroDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMoraleChangeWhenGeneralHeroKillsCaptainHero() => MCMSettings.Instance?.MoraleChangeWhenGeneralHeroKillsCaptainHero ?? ModConfiguration.MoraleChangeWhenGeneralHeroKillsCaptainHeroDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMoraleChangeWhenGeneralHeroKillsGeneralHero() => MCMSettings.Instance?.MoraleChangeWhenGeneralHeroKillsGeneralHero ?? ModConfiguration.MoraleChangeWhenGeneralHeroKillsGeneralHeroDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMoraleGainWhenTroopKillsUnassignedHero() => MCMSettings.Instance?.MoraleGainWhenTroopKillsUnassignedHero ?? ModConfiguration.MoraleGainWhenTroopKillsUnassignedHeroDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMoraleGainWhenTroopKillsCaptainHero() => MCMSettings.Instance?.MoraleGainWhenTroopKillsCaptainHero ?? ModConfiguration.MoraleGainWhenTroopKillsCaptainHeroDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMoraleGainWhenTroopKillsGeneralHero() => MCMSettings.Instance?.MoraleGainWhenTroopKillsGeneralHero ?? ModConfiguration.MoraleGainWhenTroopKillsGeneralHeroDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMoraleGainWhenUnassignedHeroKillsUnassignedHero() => MCMSettings.Instance?.MoraleGainWhenUnassignedHeroKillsUnassignedHero ?? ModConfiguration.MoraleGainWhenUnassignedHeroKillsUnassignedHeroDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMoraleGainWhenUnassignedHeroKillsCaptainHero() => MCMSettings.Instance?.MoraleGainWhenUnassignedHeroKillsCaptainHero ?? ModConfiguration.MoraleGainWhenUnassignedHeroKillsCaptainHeroDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMoraleGainWhenUnassignedHeroKillsGeneralHero() => MCMSettings.Instance?.MoraleGainWhenUnassignedHeroKillsGeneralHero ?? ModConfiguration.MoraleGainWhenUnassignedHeroKillsGeneralHeroDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMoraleGainWhenCaptainHeroKillsUnassignedHero() => MCMSettings.Instance?.MoraleGainWhenCaptainHeroKillsUnassignedHero ?? ModConfiguration.MoraleGainWhenCaptainHeroKillsUnassignedHeroDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMoraleGainWhenCaptainHeroKillsCaptainHero() => MCMSettings.Instance?.MoraleGainWhenCaptainHeroKillsCaptainHero ?? ModConfiguration.MoraleGainWhenCaptainHeroKillsCaptainHeroDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMoraleGainWhenCaptainHeroKillsGeneralHero() => MCMSettings.Instance?.MoraleGainWhenCaptainHeroKillsGeneralHero ?? ModConfiguration.MoraleGainWhenCaptainHeroKillsGeneralHeroDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMoraleGainWhenGeneralHeroKillsUnassignedHero() => MCMSettings.Instance?.MoraleGainWhenGeneralHeroKillsUnassignedHero ?? ModConfiguration.MoraleGainWhenGeneralHeroKillsUnassignedHeroDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMoraleGainWhenGeneralHeroKillsCaptainHero() => MCMSettings.Instance?.MoraleGainWhenGeneralHeroKillsCaptainHero ?? ModConfiguration.MoraleGainWhenGeneralHeroKillsCaptainHeroDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMoraleGainWhenGeneralHeroKillsGeneralHero() => MCMSettings.Instance?.MoraleGainWhenGeneralHeroKillsGeneralHero ?? ModConfiguration.MoraleGainWhenGeneralHeroKillsGeneralHeroDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static string McmFriendlyGeneralFallenNotification() => MCMSettings.Instance?.friendlyGeneralFallenNotification ?? ModConfiguration.FriendlyGeneralFallenNotificationDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static string McmEnemyGeneralFallenNotification() => MCMSettings.Instance?.enemyGeneralFallenNotification ?? ModConfiguration.EnemyGeneralFallenNotificationDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static string McmFriendlyInfantryCaptainFallenNotification() => MCMSettings.Instance?.friendlyInfantryCaptainFallenNotification ?? ModConfiguration.FriendlyInfantryCaptainFallenNotificationDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static string McmFriendlyRangedCaptainFallenNotification() => MCMSettings.Instance?.friendlyRangedCaptainFallenNotification ?? ModConfiguration.FriendlyRangedCaptainFallenNotificationDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static string McmFriendlyCavalryCaptainFallenNotification() => MCMSettings.Instance?.friendlyCavalryCaptainFallenNotification ?? ModConfiguration.FriendlyCavalryCaptainFallenNotificationDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static string McmFriendlyHorseArchersCaptainFallenNotification() => MCMSettings.Instance?.friendlyHorseArchersCaptainFallenNotification ?? ModConfiguration.FriendlyHorseArchersCaptainFallenNotificationDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static string McmEnemyInfantryCaptainFallenNotification() => MCMSettings.Instance?.enemyInfantryCaptainFallenNotification ?? ModConfiguration.EnemyInfantryCaptainFallenNotificationDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static string McmEnemyRangedCaptainFallenNotification() => MCMSettings.Instance?.enemyRangedCaptainFallenNotification ?? ModConfiguration.EnemyRangedCaptainFallenNotificationDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static string McmEnemyCavalryCaptainFallenNotification() => MCMSettings.Instance?.enemyCavalryCaptainFallenNotification ?? ModConfiguration.EnemyCavalryCaptainFallenNotificationDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static string McmEnemyHorseArchersCaptainFallenNotification() => MCMSettings.Instance?.enemyHorseArchersCaptainFallenNotification ?? ModConfiguration.EnemyHorseArchersCaptainFallenNotificationDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static string McmFriendlyUnassignedHeroFallenNotification() => MCMSettings.Instance?.friendlyUnassignedHeroFallenNotification ?? ModConfiguration.FriendlyUnassignedHeroFallenNotificationDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static string McmEnemyUnassignedHeroFallenNotification() => MCMSettings.Instance?.enemyUnassignedHeroFallenNotification ?? ModConfiguration.EnemyUnassignedHeroFallenNotificationDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmUpdateIntervalInSeconds() => MCMSettings.Instance?.UpdateIntervalInSeconds ?? ModConfiguration.UpdateIntervalInSecondsDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmLoggingEnabled() => MCMSettings.Instance?.LoggingEnabled ?? ModConfiguration.LoggingEnabledDefault;
    #endregion

    private static ModConfiguration LoadFromFile()
    {
        string path = Path.Combine(ModInfo.ModuleRoot, ModInfo.ConfigurationFileName);
        ModConfiguration configuration = new();

        if (!File.Exists(path))
        {
            KdhitLog.Warn(ModInfo.ConfigurationFileName + " was missing. Created it with the default settings.");
            TryWrite(path, configuration);
            return configuration;
        }

        try
        {
            configuration = Read(path);
            if (!HasEverySetting(path, configuration))
            {
                KdhitLog.Warn(ModInfo.ConfigurationFileName + " had missing settings. Added them with their default values.");
                TryWrite(path, configuration);
            }
        }
        catch (Exception ex) when (ex is InvalidOperationException or XmlException)
        {
            KdhitLog.Warn(ModInfo.ConfigurationFileName + " was broken and was replaced with the default settings. Reason: " + (ex.InnerException?.Message ?? ex.Message));
            configuration = new ModConfiguration();
            TryWrite(path, configuration);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            KdhitLog.Warn(ModInfo.ConfigurationFileName + " could not be opened. Using the default settings. Reason: " + ex.Message);
            configuration = new ModConfiguration();
        }

        // Out-of-range values are corrected in memory only; the file keeps what the player typed.
        configuration.Clamp();
        return configuration;
    }

    private static ModConfiguration Read(string path)
    {
        using FileStream stream = File.OpenRead(path);
        return Serializer.Deserialize(stream) as ModConfiguration ?? new ModConfiguration();
    }

    private static void TryWrite(string path, ModConfiguration configuration)
    {
        try
        {
            XmlWriterSettings writerSettings = new() { Indent = true, Encoding = Encoding.UTF8 };
            using XmlWriter writer = XmlWriter.Create(path, writerSettings);
            Serializer.Serialize(writer, configuration, NoNamespaces());
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            KdhitLog.Warn(ModInfo.ConfigurationFileName + " could not be written. The settings are kept in memory for this session. Reason: " + ex.Message);
        }
    }

    private static bool HasEverySetting(string path, ModConfiguration configuration)
    {
        XDocument complete = new();
        using (XmlWriter writer = complete.CreateWriter())
        {
            Serializer.Serialize(writer, configuration, NoNamespaces());
        }

        XElement? fileRoot = XDocument.Load(path).Root;
        return fileRoot != null && complete.Root != null && ContainsEveryElement(fileRoot, complete.Root);
    }

    private static bool ContainsEveryElement(XElement actual, XElement expected)
    {
        foreach (XElement expectedChild in expected.Elements())
        {
            XElement? actualChild = actual.Element(expectedChild.Name);
            if (actualChild == null || !ContainsEveryElement(actualChild, expectedChild)) return false;
        }

        return true;
    }

    private static XmlSerializerNamespaces NoNamespaces()
    {
        XmlSerializerNamespaces namespaces = new();
        namespaces.Add(string.Empty, string.Empty);
        return namespaces;
    }
}
