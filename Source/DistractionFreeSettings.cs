// -------------------------------------------------------------------------------------
// <copyright file="DistractionFreeSettings.cs">
//   Copyright (c) 2026
//   Licensed under the MIT License. See LICENSE file in the project root.
// </copyright>
// -------------------------------------------------------------------------------------

namespace StrangeLens
{
   /// <summary>Config-file-only overrides applied while distraction-free mode is active
   ///    (Ctrl+Alt+Shift+D, see <see cref="Lens.DistractionFreeActive"/>). No Settings UI
   ///    exposes these -- edit settings.json directly to customize them.</summary>
   public class DistractionFreeSettings
   {
      public ScalingModeOption ScalingMode { get; init; } = ScalingModeOption.HighQualityBicubic;

      public bool ShowCrosshair { get; init; } = false;

      public bool ShowGrid { get; init; } = false;

      public bool ShowInfo { get; init; } = false;
   }
}
