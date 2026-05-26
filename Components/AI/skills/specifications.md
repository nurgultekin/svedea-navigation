# Technical Spec – Svedea Navigation Case (Blazor WebAssembly)

## Overview

A responsive, accessible navigation component built in Blazor WebAssembly, faithfully
reproducing Svedea's existing desktop nav while improving the mobile experience and
fixing identified UX/accessibility issues. Menu data is driven from a typed C# model —
no hardcoded markup.

---

## Technology Stack

| Layer        | Choice                          | Reason                                              |
|--------------|---------------------------------|-----------------------------------------------------|
| Framework    | Blazor WebAssembly (.NET 8)     | Matches Svedea's actual stack; intentional challenge|
| Language     | C# + Razor syntax               | Minimal JS; logic stays in C#                       |
| Styling      | Plain CSS (no framework)        | Full control; matches scope of task                 |
| JS interop   | Minimal — only if needed        | E.g. focus trap, scroll lock on mobile overlay      |
| Data         | Typed C# record classes         | Satisfies "not hardcoded in markup" requirement     |

---

## Design Tokens

Extracted from the live site CSS and screenshots.

### Colors

```css
:root {
  /* Primary brand — sourced directly from compiled stylesheet */
  --color-navy:        #005073;   /* main header bg, h1/h2 primary, p--primary */
  --color-navy-dark:   #003d57;   /* hover state — darkened from #005073 */
  --color-teal:        #91cfc9;   /* h1/h2 secondary, p--secondary, icons */
  --color-teal-light:  #91cfc9;   /* reused at lower opacity for dropdown bg */

  /* Text */
  --color-white:       #ffffff;
  --color-body:        #404040;   /* p--standard, h1/h2 standard, body text */
  --color-link-dark:   #222222;   /* sub-links in dropdown */

  /* UI */
  --color-border:      #d9e6e6;   /* subtle dividers */
  --color-focus-ring:  #91cfc9;   /* keyboard focus outline */

  /* CTA */
  --color-cta-border:  #ffffff;   /* Anmäl skada — outlined pill on dark bg */
}
```

### Typography

```css
:root {
  --font-primary:   'Public Sans', sans-serif;   /* all nav text */
  --font-secondary: 'Barlow', sans-serif;        /* legend / tagline */
}
```

Google Fonts import (add to index.html):
```html
<link rel="preconnect" href="https://fonts.googleapis.com">
<link href="https://fonts.googleapis.com/css2?family=Public+Sans:wght@400;500;700&family=Barlow:wght@400;700&display=swap" rel="stylesheet">
```

### Spacing & Sizing

All values are in `rem` (base: 1rem = 16px browser default). No hard pixel values
in CSS — this ensures spacing and sizing scale with the user's browser font preferences.

```css
:root {
  --nav-height:         5rem;      /* 80px equivalent — sticky header height */
  --nav-height-mobile:  3.75rem;   /* 60px equivalent — mobile header height */
  --nav-padding-x:      1.5rem;    /* 24px — horizontal padding inside header */
  --nav-gap:            2rem;      /* 32px — gap between nav items */
  --nav-font-size:      1rem;      /* 16px — nav link text */
  --nav-small-font:     0.875rem;  /* 14px — sub-links in dropdown */
  --nav-icon-size:      1.25rem;   /* 20px — icon dimensions */
  --radius-pill:        6.25rem;   /* 100px — CTA button pill shape */
  --focus-outline:      0.125rem;  /* 2px — keyboard focus ring width */
  --transition-instant: 0s;        /* CTA hover — no delay (fixes bug #4) */

  /* Active state accent */
  --active-border-width: 0.1875rem; /* 3px — left border on active mobile link */
}
```

> **Why rem over px:** px values are absolute and ignore the user's browser font-size
> preference (an accessibility concern — WCAG 1.4.4). rem values scale proportionally,
> meaning a user who has set their browser to 20px base gets a larger, still-proportional
> nav rather than one that overflows or feels cramped.

---

## Component Architecture

```
SvedeaNav/
├── Models/
│   └── NavData.cs          ← C# records: NavCategory, NavLink
├── Data/
│   └── NavMenuData.cs      ← Static method returning List<NavCategory>
└── Components/
    ├── MainNav.razor        ← Top-level nav shell, state owner
    ├── DesktopNav.razor     ← Desktop layout (hidden on mobile)
    ├── MobileNav.razor      ← Mobile overlay (hidden on desktop)
    ├── InsuranceDropdown.razor  ← Desktop mega-menu panel
    └── MobileCategoryItem.razor ← Single accordion item in mobile menu
```

---

## Data Model

```csharp
// Models/NavData.cs
public record NavLink(string Label, string Url);

public record NavCategory(
    string Name,
    string IconUrl,
    List<NavLink> Links
);
```

```csharp
// Data/NavMenuData.cs
public static class NavMenuData
{
    public static List<NavCategory> GetCategories() => new()
    {
        new("Bil", "images/icons/bil.svg", new()
        {
            new("Bilförsäkring", "/bilforsakring"),
            new("Bilförsäkring för företag", "/foretagsforsakring/bilforsakring-for-foretag")
        }),
        new("Fordon", "images/icons/fordon.svg", new()
        {
            new("Snöskoterförsäkring", "/snoskoterforsakring"),
            new("ATV-försäkring", "/atv-forsakring"),
            new("Släpvagnsförsäkring", "/slapvagnsforsakring"),
            new("Husvagnsförsäkring", "/husvagnsforsakring")
        }),
        new("Motorcykel", "images/icons/mc.svg", new()
        {
            new("Mc-försäkring", "/mc-forsakring"),
            new("Märkesförsäkringar", "/mc-forsakring/markesforsakringar")
        }),
        new("Båt", "images/icons/bat.svg", new()
        {
            new("Båtförsäkring", "/batforsakring"),
            new("Märkesförsäkringar", "/batforsakring/markesforsakringar"),
            new("Vattenskoterförsäkring", "/vattenskoterforsakring"),
            new("Sportfiskarna", "/batforsakring/sportfiskarna")
        }),
        new("Djur", "images/icons/djur.svg", new()
        {
            new("Hundförsäkring", "/hundforsakring"),
            new("Jakthundsförsäkring", "/jakthundsforsakring"),
            new("Kattförsäkring", "/kattforsakring"),
            new("Djurförsäkring", "/djurforsakring")
        }),
        new("Hem & hus", "images/icons/hem.svg", new()
        {
            new("Hemförsäkring", "/hemforsakring"),
            new("Villaförsäkring", "/hemforsakring/villaforsakring"),
            new("Bostadsrättsförsäkring", "/hemforsakring/bostadsrattsforsakring"),
            new("Hyresrättsförsäkring", "/hemforsakring/hyresrattsforsakring"),
            new("Fritidshusförsäkring", "/hemforsakring/fritidshusforsakring")
        }),
        new("Företag", "images/icons/foretag.svg", new()
        {
            new("Företagsförsäkring", "/foretagsforsakring"),
            new("Bilförsäkring för företag", "/foretagsforsakring/bilforsakring-for-foretag"),
            new("Släpvagnsförsäkring", "/slapvagnsforsakring"),
            new("Drönarförsäkring", "/dronarforsakring")
        }),
        new("För förmedlare", "images/icons/grupp.svg", new()
        {
            new("Gruppförsäkringar", "/gruppforsakringar"),
            new("Kommunolycksfall", "/gruppforsakringar/kommunolycksfall"),
            new("Försäkring via förmedlare", "/foretagsforsakring/forsakring-via-formedlare")
        })
    };
}
```

---

## Desktop Nav — Structure & Behaviour

### Layout (left → center → right)

```
[ Försäkringar ↓   Tips & råd   Om oss ]  [ SVEDEA logo + tagline ]  [ Logga in  Kontakt  Sök  | Anmäl skada ]
```

- Header height: `var(--nav-height)` (5rem), `position: sticky; top: 0; z-index: 100`
- Background: `var(--color-navy)`
- All text: `var(--color-white)`

### Left section

- **Försäkringar** — button (`<button>`), triggers mega-menu on click
  - Shows chevron icon that rotates 180° when open
  - Active/open state: underline or subtle highlight
- **Tips & råd** and **Om oss** — plain `<a>` links
  - Hover: underline appears
  - Active page: underline + font-weight 700

### Center — Logo

- SVG logo + tagline "Försäkringar som gör skillnad" below
- Wrapped in `<a href="/">` for home navigation

### Right section

**UX fix #1 — visual balance:**
The original right-side items (Logga in, Kontakt, Sök) render smaller than
the left nav links. Fix: ensure all three share the same `font-size: var(--nav-font-size)` (1rem)
and `font-weight: 500`. Add small SVG icons above each label to match the
screenshot style without making them feel subordinate.

- **Logga in** — link with user icon
- **Kontakt** — link with phone icon
- **Sök** — button that opens search overlay
- **Anmäl skada** — pill CTA button, outlined style (white border, transparent bg)
  - Hover: bg becomes white, text becomes navy — **instant, no transition delay** (fix #4)

### Mega-menu (Försäkringar dropdown)

- Opens below the sticky header, full-width panel
- Background: `var(--color-teal-light)` (`#a8d5d1`)
- 4-column CSS grid of insurance categories
- Each category: icon + bold teal heading + list of plain links below
- Footer link: "Se alla försäkringar →"
- Closes on: clicking Försäkringar again, pressing Escape, clicking outside
- All category accordions **independent** — multiple can be open simultaneously (fix #2, desktop context)

---

## Mobile Nav — Structure & Behaviour

### Closed state (hamburger bar)

```
[ phone icon   user icon ]   [ SVEDEA logo + tagline ]   [ ☰ hamburger ]
```

- Height: `var(--nav-height-mobile)` (3.75rem)
- Background: `var(--color-navy)`
- Hamburger button: `aria-label="Öppna meny"`, `aria-expanded="false"`
- Phone and user icons are tappable shortcuts (quick access without opening full menu)

### Open state (full-screen overlay)

When hamburger is tapped, the nav expands to a **full-screen overlay** sliding
in from the top (or the right — decision: top-down feels more natural here).

```
[ phone icon   user icon ]   [ SVEDEA logo ]   [ ✕ close ]
─────────────────────────────────────────────────────────
[ 🔍 Sök på svedea...                              ]
─────────────────────────────────────────────────────────
  Försäkringar                                    ∧ (open)
  ┌──────────────────────────────────────────────┐
  │  🚗 Bil                              ∨       │
  │  🏍 Motorcykel                       ∨       │
  │  ⛵ Båt                              ∨       │
  │  🐾 Djur                             ∨       │
  │  🏠 Hem & hus                        ∨       │
  │  🏢 Företag                          ∨       │
  │  👥 För förmedlare                   ∨       │
  │                                              │
  │  → Se alla försäkringar                      │
  └──────────────────────────────────────────────┘
  Tips & råd
  Om oss
─────────────────────────────────────────────────────────
  [ Anmäl skada ]      (full-width CTA pill)
  [ Logga in   ]       (ghost/secondary)
```

**UX improvements over original:**

- **Fix #2 — Independent accordions:** All category accordions under Försäkringar
  are independently expandable. Expanding Bil does NOT close Motorcykel. Users can
  compare sub-items across categories simultaneously.
- **Fix #3 — Active state:** Current page URL is matched against nav link hrefs.
  Active link gets: teal color + left border accent (`var(--active-border-width)` 0.1875rem solid teal) + font-weight 700.
- **Fix #4 — CTA hover:** `transition: none` on Anmäl skada button. Instant color swap.
- **WCAG — Skip link:** Visible-on-focus "Gå till sidans innehåll" link renders
  before the nav, allowing keyboard users to bypass the entire menu. This is a WCAG
  2.4.1 (Bypass Blocks) requirement.

### Mobile accordion behaviour

```csharp
// In MobileNav.razor @code block
private HashSet<string> openCategories = new();

private void ToggleCategory(string name)
{
    if (openCategories.Contains(name))
        openCategories.Remove(name);
    else
        openCategories.Add(name);   // Does NOT clear others — fix #2
}

private bool IsCategoryOpen(string name) => openCategories.Contains(name);
```

---

## Accessibility (WCAG 2.1 AA)

| Criterion       | Requirement                          | Implementation                                      |
|-----------------|--------------------------------------|-----------------------------------------------------|
| 1.4.3           | Contrast minimum 4.5:1               | White on navy (#1b3a4b) ≈ 10:1 ✓                   |
| 2.1.1           | Keyboard accessible                  | All buttons/links reachable via Tab; Enter/Space activates |
| 2.4.1           | Bypass blocks                        | Skip-to-content link visually hidden, shown on focus |
| 2.4.7           | Focus visible                        | `outline: var(--focus-outline) solid var(--color-focus-ring)` on all interactive elements |
| 4.1.2           | Name, role, value                    | `aria-expanded`, `aria-controls`, `aria-label` on all toggle buttons |
| 2.1.2           | No keyboard trap                     | Escape closes any open overlay/dropdown; focus returns to trigger |

### Keyboard interactions

| Key         | Context              | Behaviour                                  |
|-------------|----------------------|--------------------------------------------|
| Tab         | Nav                  | Moves through all interactive items        |
| Enter/Space | Försäkringar button  | Opens/closes mega-menu or mobile overlay   |
| Escape      | Menu open            | Closes menu, returns focus to trigger      |
| Enter/Space | Category accordion   | Expands/collapses that category            |

### ARIA pattern for accordion category button

```razor
<button
    aria-expanded="@IsCategoryOpen(category.Name).ToString().ToLower()"
    aria-controls="category-@category.Name.Replace(" ", "-")"
    @onclick="() => ToggleCategory(category.Name)">
    @category.Name
</button>
<ul id="category-@category.Name.Replace(" ", "-")"
    hidden="@(!IsCategoryOpen(category.Name))">
    @foreach (var link in category.Links) { ... }
</ul>
```

---

## Responsive Breakpoints

Breakpoints use `em` to match Svedea's own grid/container breakpoint pattern
found in the compiled stylesheet. `30em ≈ 480px`, `48em ≈ 768px`, `62em ≈ 992px`.

```css
/* Mobile first — default styles target mobile */

@media only screen and (min-width: 30em) {
    /* Small mobile breakpoint — matches their .container rule */
}

@media only screen and (min-width: 48em) {
    /* Tablet — matches their grid-col breakpoint */
}

@media only screen and (min-width: 62em) {
    /* Desktop — show desktop nav, hide mobile hamburger.
       Matches their .container max-width breakpoint. */
    .mobile-nav  { display: none; }
    .desktop-nav { display: flex; }
}
```

---

## Main Content Layout — Responsive Container Fix

The live site's `.www-main-layout` has a fixed or large max-width that leaves
excessive horizontal whitespace on wide screens (1440px+). The content area
does not stretch proportionally — it looks correct on ~1200px screens but
feels marooned on larger displays.

### Root cause

The container uses either a fixed `max-width` with `margin: 0 auto` and no
fluid padding, or a fixed `padding-inline` that doesn't scale with viewport width.
On ultra-wide screens this creates disproportionate dead margins.

### Fix — fluid container with clamped padding

```css
.main-layout {
  width: 100%;
  /* Fluid horizontal padding: never less than 1.5rem, never more than 5%,
     ideally 3vw — scales smoothly with viewport width */
  padding-inline: clamp(1.5rem, 3vw, 5rem);
  box-sizing: border-box;
}

.main-layout .container {
  width: 100%;
  /* Matches their actual compiled CSS: max-width kicks in at 62em (≈992px).
     100rem ceiling = 1600px at default font size. */
  max-width: 100rem;
  margin-inline: auto;
}
```

### Why `clamp()` over a fixed value

`clamp(min, preferred, max)` gives you a single declaration that handles all
three scenarios:
- **Narrow (mobile):** padding never drops below `1.5rem` — content isn't
  flush against the edge
- **Mid (laptop):** `3vw` scales fluidly — feels proportional at any width
- **Wide (large monitor):** padding caps at `5rem` — doesn't become absurdly
  wide on 4K screens

This is preferable to breakpoint-based padding changes because it's continuous
rather than stepped, meaning there's no jarring jump at any specific viewport width.

### Nav alignment

The sticky header should share the same container constraint so the nav content
aligns with the page content below it:

```css
.main-header .header-items {
  max-width: 100rem;
  margin-inline: auto;
  padding-inline: clamp(1.5rem, 3vw, 5rem);
}
```

> **On `em` vs `rem` in media queries:** Their compiled stylesheet uses `em` for
> the core grid/container breakpoints (`30em`, `48em`, `62em`) and raw `px` for
> component-level overrides (`810px`, `1100px`). In your implementation, use `em`
> for layout/container breakpoints to stay consistent with their pattern, and `rem`
> everywhere else (spacing, font sizes, sizing tokens). The practical difference
> between `em` and `rem` in media queries is minimal since both resolve against the
> root font size at that level — the important thing is not using `px`, which ignores
> browser font preferences.

> **Note:** The nav background itself still spans full viewport width (`width: 100%`)
> — only the inner content is constrained. This preserves the full-bleed navy bar
> visual while aligning text/buttons with the rest of the page.

---



## Bugs Fixed (Summary)

| # | Original issue                                | Fix                                                   |
|---|-----------------------------------------------|-------------------------------------------------------|
| 1 | Right nav items visually smaller than left    | Normalise `1rem` font-size + consistent font-weight   |
| 2 | Mobile accordions force single-open           | Use `HashSet<string>` — independent toggle state      |
| 3 | No active page indicator in nav               | Match `NavigationManager.Uri` against link href       |
| 4 | CTA hover has weird animation delay           | `transition: none` on `.btn-cta`                      |
| ✦ | No skip link (WCAG 2.4.1 violation)           | Add visually-hidden skip-to-content link              |

---

## File Structure (Blazor project)

```
SvedeaNav/
├── wwwroot/
│   ├── css/
│   │   └── nav.css
│   └── images/icons/       ← SVG icons copied from svedea.se/media/
├── Models/
│   └── NavData.cs
├── Data/
│   └── NavMenuData.cs
├── Components/
│   ├── MainNav.razor
│   ├── DesktopNav.razor
│   ├── MobileNav.razor
│   ├── InsuranceDropdown.razor
│   └── MobileCategoryItem.razor
├── Pages/
│   └── Index.razor          ← Embeds <MainNav /> to demo
├── App.razor
└── Program.cs
```

---

## What Would Be Improved With More Time

- **Search overlay**: functional search with keyboard trap and results
- **Animation**: smooth height transition on accordion open/close using CSS custom properties
- **Scroll lock**: prevent body scroll when mobile overlay is open (requires JS interop)
- **Testing**: keyboard-only walkthrough + screen reader test (NVDA/VoiceOver)
- **Reduced motion**: respect `prefers-reduced-motion` for any CSS transitions
- **Real routing**: `NavigationManager` active state would work with actual Blazor pages

---

*Spec written for: Svedea Frontend Case — May 2026*
*Author: Nur Gültekin*
