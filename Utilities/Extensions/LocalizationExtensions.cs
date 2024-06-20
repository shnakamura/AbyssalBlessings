using Terraria.Localization;
using Terraria.ModLoader;

namespace AbyssalBlessings.Utilities.Extensions;

/// <summary>
///     Provides basic localization extensions.
/// </summary>
public static class LocalizationExtensions
{
    /// <inheritdoc cref="ILocalizedModTypeExtensions.GetLocalizedValue"/>
    /// <param name="args"></param>
    public static string GetLocalizedValue(this ILocalizedModType self, string suffix, params object[] args) {
        return Language.GetTextValue(self.GetLocalizationKey(suffix), args);
    }
}
