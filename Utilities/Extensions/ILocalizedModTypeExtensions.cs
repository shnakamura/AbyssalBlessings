using Terraria.Localization;
using Terraria.ModLoader;

namespace AbyssalBlessings.Utilities.Extensions;

/// <summary>
///     Provides basic <see cref="ILocalizedModType" /> extensions.
/// </summary>
public static class ILocalizedModTypeExtensions
{
    /// <inheritdoc cref="Terraria.ModLoader.ILocalizedModTypeExtensions.GetLocalizedValue" />
    public static string GetLocalizedValue(this ILocalizedModType self, string suffix, params object[] args) {
        return Language.GetTextValue(self.GetLocalizationKey(suffix), args);
    }
}
