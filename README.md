# Lens
A pixel-precise screen magnifier with color picker for Windows. Lives in the system tray.

## Keyboard & Mouse Shortcuts

| Input | Condition | Action |
| --- | --- | --- |
| `Ctrl+Shift+Z` | Always | Toggle lens open/closed |
| `Ctrl+Shift+Scroll` | Lens open, any focus | Zoom in/out |
| `Ctrl+=` / `Ctrl+-` | Lens focused | Zoom in/out |
| `[` / `]` | Lens focused | Width narrower/wider |
| `;` / `'` | Lens focused | Height shorter/taller |
| Arrow keys | Lens focused | Nudge cursor 1px |
| `H` / `R` / `X` | Lens focused | Copy color (HSL / RGB / Hex) |
| `ESC` | Lens focused | Close lens |

`Ctrl+Shift+Z` is the primary way to open and close the lens. `ESC` is a fallback that only works when the lens window has focus — the border color indicates this (see below).
