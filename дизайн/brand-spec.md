# Brand Spec — Book Review & Community Platform

Extracted from user-provided brand specification (2026-08-07).

## Color tokens (OKLch)

```css
:root {
  --bg:      oklch(98.8% 0.008 95);   /* #FDFBF7 Warm Cream */
  --surface: oklch(100% 0 0);         /* #FFFFFF */
  --fg:      oklch(27% 0.03 255);     /* #1E293B Deep Charcoal */
  --muted:   oklch(55% 0.03 250);     /* #64748B Warm Slate */
  --border:  oklch(95.5% 0.004 260);  /* #F3F4F6 */
  --accent:  oklch(76% 0.16 75);      /* #F59E0B Amber Yellow */
  --accent-hover: oklch(65% 0.15 55); /* #D97706 Warm Gold */
}
```

Hex reference: `#F59E0B` / `#D97706` / `#FDFBF7` / `#FFFFFF` / `#1E293B` / `#64748B` / `#F3F4F6`

## Typography

- **Display / headings:** `'Plus Jakarta Sans', 'Playfair Display', Georgia, serif` — Plus Jakarta for UI titles; Playfair Display for editorial section titles
- **Body / UI:** `'Inter', system-ui, -apple-system, 'Segoe UI', sans-serif` — weights 400 / 500 / 600
- **Mono:** not required

## Layout posture (observed / specified)

1. **16:9 desktop** — target 1440px width canvas
2. **8px base grid**, 12-column desktop layout
3. **Radii:** 16px cards/modals · 10px inputs · pill (9999px) primary buttons
4. **Shadows:** soft only — `0 10px 25px -5px rgba(0,0,0,0.05)`; no heavy chrome
5. **Ambient glow:** 30–50px blur behind book covers at 15–20% opacity, matched to cover dominant colors; high contrast text over glow

## Component rules

- Primary CTA: solid amber `#F59E0B` + charcoal text (not white)
- Review cards: mini cover + avatar + stars + snippet + likes/comments
- Sign-up modal: 50/50 split — left visual hero (book stack + glow), right 3-field form
