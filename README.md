# Svedea Navigation — Frontend Case

A fully responsive navbar built for Svedea in Blazor (.NET 10), plain CSS, and a small amount of vanilla JS for accessibility. No Bootstrap or UI frameworks.

**Live demo:** https://svedea-navigation-hzdjffg4g0etfyf7.swedencentral-01.azurewebsites.net/

---

## Getting started

```bash
# Live reload — rebuilds on file save
dotnet watch

# Single build and run
dotnet run
```

Open `https://localhost:7068` (or the URL printed in the terminal).

---

## My solution

A fully responsive navbar across desktop, tablet and mobile. Nav data is driven from a single C# file (`NavMenuData.cs`), nothing is hardcoded in markup. On desktop, a mega-dropdown shows all insurance categories. On mobile, a slide-in overlay with an accordion pattern handles the same content.

Separate components for `DesktopNav`, `MobileNav`, `MobileCategoryItem`, `InsuranceDropdown`, and `SearchPanel`, orchestrated by a `MainNav` parent that owns shared state.

The original search opened a modal requiring a second interaction to actually search. I replaced it with a dropdown panel that opens inline, shows frequently searched terms and sitemap shortcuts by default, and filters nav data as you type.

---

## Tech stack

| Concern | Choice |
|---|---|
| Language | C# |
| Framework | Blazor Server (.NET 10) |
| Styling | Plain CSS — custom properties, `clamp()`, scoped component files |
| JS | Vanilla — focus trap only (`navFocusTrap.js`) |
| Hosting | Azure App Service (Free F1 tier) |

---

## Design decisions

**Full-width navbar**
The original svedea.se wraps the header inside a max-width container, and it leaves visible background on both sides at wide screens. I let the navbar span the full viewport and constrain only the inner content grid to make better use of available space on the navigation bar.

**Fluid scaling**
Font sizes and padding in the desktop nav scale with `clamp()`, so the bar doesn't feel cramped at ~992 px or oversized at 1600 px. Mobile uses fixed sizes where predictable touch targets matter more.

**Active states**
The three primary links (Försäkringar, Tips & råd, Om oss) have active state styling so users always know where they are. Icon buttons (Logga in, Kontakt, Sök) get a color shift on hover instead of an underline; can be more appropriate for their smaller footprint.

**Mobile navigation**
Hamburger + accordion with progressive disclosure: the three top-level items appear first, Försäkringar expands to reveal categories, which expand to reveal products. Each nesting level has a distinct visual treatment — a teal left border scopes the open section, a darkened background on the category level, subtle separators between sub-items — to reduce cognitive load.

**Mobile information hierarchy**
The original had a phone icon and login icon in the mobile top bar, duplicated again inside the overlay header. Icon-only buttons require the user to recognize a symbol and infer its meaning which can be an unnecessary cognitive load. I removed the duplicate icons from the overlay header and replaced the floating phone icon with a labeled "Kontakt" button in the overlay footer, grouped with the other utility actions ("Anmäl skada", "Logga in"). Utilities grouped with utilities. The top bar became: Login icon (left) + Logo (center) + Hamburger (right) — three clear points, so that they don't compete for space.

**`em`-based breakpoints**
Breakpoints use `em` units instead of `px` so they stay relative to the user's base font size. A user with larger system fonts (e.g. for accessibility) won't hit a layout breakpoint prematurely.

**Focus rings**
The original svedea.se removes all outlines globally with `outline: 0 !important`. I kept native focus outlines intact and built a consistent focus ring system across all interactive elements instead.

**Search panel**
The original design opened a modal that required a second click to actually search. I replaced it with a dropdown that shows frequently searched options and sitemap shortcuts as suggestions, plus a prefill (typeahead) that pulls from nav data client-side.

---

## Design system

Rather than writing component-specific CSS ad hoc, I built a small token-based system in `app.css`:

- **Tokens** — CSS custom properties for the full color palette, a typography scale (`--text-sm` → `--text-8xl`), spacing, and nav-specific sizing.
- **Button system** — composable base + tier + modifier classes. `.btn` is the reset; `.btn--primary / --secondary / --tertiary` set visual weight; modifiers like `.btn--lg`, `.btn--icon`, `.btn--icon--labeled`, `.btn--tertiary--light` handle size and context variations.
- **Nav primitives** — nav link, sub-link, and search input styles follow the same logic: one definition, contextual overrides via combo classes in scoped component files.

The goal: maintainable and modular code. Changing a token cascades everywhere, and adding a new component means reaching for existing classes rather than writing new ones.

---

## Deployment

Hosted on **Azure App Service**. Deployed via the Azure CLI:

```bash
az webapp up \
  --sku F1 \
  --name svedea-nav-demo \
  --runtime "DOTNETCORE:10.0" \
  --os-type linux \
  --location westeurope
```

Note: I use the F1 free tier does not support WebSockets, so Blazor Server's SignalR connection falls back to Long Polling.

---

## What I would improve with more time

- **Keyboard tab order after dropdown** — opening the dropdown via keyboard should move focus directly into it, but Tab currently cycles through the remaining nav buttons first. Exiting the dropdown should then resume focus at the next logical element.
- **Real pages** — stub pages would better demonstrate how the nav behaves with scroll depth and varied content below it.
- **Button hierarchy** — I'd revisit and tighten the button hierarchy further as I'm sure there is room to refine visual weight at edge cases.
- **Localization** — language selection in the navbar (sv/en at minimum).
- **Search index** — search currently filters the nav tree client-side. A real implementation would query a full-site index including article content.
- **Automated tests** — no tests exist. With more time: Playwright end-to-end tests covering keyboard navigation flows and responsive breakpoints, plus assistive technology testing (screen reader pass-throughs) to iterate on accessibility properly.
