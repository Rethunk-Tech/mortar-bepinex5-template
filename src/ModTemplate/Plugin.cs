#if (assets)
using System.IO;
#endif
using BepInEx;
#if (config)
using BepInEx.Configuration;
#endif
using BepInEx.Logging;
#if (assets)
using UnityEngine;
#endif

namespace ModTemplate;

[BepInPlugin(MyPluginInfo.Id, MyPluginInfo.Name, MyPluginInfo.Version)]
[BepInProcess("Lethal Company.exe")]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log { get; private set; } = null!;
#if (config)

    // Settings here are each player's own. Use ConfigSync for ones the whole lobby should share.
    internal static ConfigEntry<bool> Enabled { get; private set; } = null!;
#endif
#if (assets)

    internal static AssetBundle? Assets { get; private set; }
#endif

    private void Awake()
    {
        Log = Logger;
#if (config)
        Enabled = Config.Bind("General", "Enabled", true, "Turns the mod on or off.");
#endif
#if (assets)
        Assets = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Info.Location)!, "modtemplate.bundle"));
        if (Assets == null)
        {
            Log.LogError("modtemplate.bundle is missing beside the plugin");
        }
#endif
#if (netcode)
        ModNetworkSetup.Install();
#endif
#if (config)
        // Put the mod's hooks after this check so Enabled can turn them off.
        if (!Enabled.Value)
        {
            Log.LogInfo($"{MyPluginInfo.Name} is turned off in its config");
            return;
        }
#endif
        Log.LogInfo($"{MyPluginInfo.Name} {MyPluginInfo.Version} loaded");
    }
}
