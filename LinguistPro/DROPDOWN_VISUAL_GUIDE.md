# Custom Dropdown - Visual Guide

## How the Dropdown Looks and Works

### 1. **Default State (Closed)**
```
┌─────────────────────────────────────┐
│ 🇩🇪 German                          ▲│
└─────────────────────────────────────┘
  White background
  Shadow: 0 4px 12px rgba(0, 0, 0, 0.08)
  Border: 2px solid white
  Rounded corners: 12px
```

### 2. **Hover State (Closed)**
```
┌─────────────────────────────────────┐
│ 🇩🇪 German                          ▲│
└─────────────────────────────────────┘
  Enhanced shadow: 0 6px 16px rgba(0, 0, 0, 0.12)
  Slight upward transform: translateY(-1px)
  Text color: #333333
```

### 3. **Open State**
```
┌─────────────────────────────────────┐
│ 🇩🇪 German                          ▼│
├─────────────────────────────────────┤
│ 🇩🇪 German                          │  ← Current selection (highlighted)
│ 🇫🇷 French                          │
│ 🇪🇸 Spanish                         │
│ 🇷🇺 Russian                         │
│ 🇰🇷 Korean                          │
└─────────────────────────────────────┘
  Chevron rotates 180° ▼
  Shadow increases
  Options appear with smooth animation
  Border-radius: 12px 12px 0 0 (button only)
  Dropdown border-radius: 0 0 12px 12px
```

### 4. **Option States**

#### Hover Over Option:
```
│ 🇫🇷 French                          │
  Background: #f8f9ff (light blue)
  Left border: 4px solid #667eea (blue)
  Text color: #667eea (blue)
  Padding-left: 18px (shifts right slightly)
```

#### Selected Option:
```
│ 🇩🇪 German                          │
  Background: #e8f0ff (medium blue)
  Left border: 4px solid #667eea (blue)
  Text color: #667eea (blue)
  Font-weight: 600 (bold)
```

#### Regular Option:
```
│ 🇪🇸 Spanish                         │
  Background: white
  Left border: 4px solid transparent
  Text color: #333333 (dark)
  Padding-left: 16px
```

## Color Scheme

| Element | Color | Hex Code | Usage |
|---------|-------|----------|-------|
| Text | Dark Gray | #333333 | Default text |
| Background | White | #ffffff | Main backgrounds |
| Primary Accent | Indigo/Blue | #667eea | Hover & active states |
| Hover Background | Light Blue | #f8f9ff | Option hover |
| Selected Background | Medium Blue | #e8f0ff | Selected option |
| Border | Subtle | #ddd / #f0f0f0 | Separators |
| Shadow Light | Dark | rgba(0,0,0,0.08) | Default shadow |
| Shadow Enhanced | Dark | rgba(0,0,0,0.12) | Hover shadow |

## Animation Timing

| Action | Duration | Easing | Effect |
|--------|----------|--------|--------|
| Button hover | 0.25s | cubic-bezier(0.4, 0, 0.2, 1) | Smooth scale up & shadow |
| Chevron rotate | 0.3s | cubic-bezier(0.4, 0, 0.2, 1) | Smooth rotation |
| Dropdown open | 0.25s | cubic-bezier(0.4, 0, 0.2, 1) | Height & opacity |
| Option hover | 0.2s | ease | Background & text color |

## Interaction Flow

```
1. User sees default dropdown
   └─> Click button
       └─> Dropdown opens with animation
           ├─> Chevron rotates 180°
           ├─> Dropdown expands (max-height: 420px)
           └─> Options become visible & clickable

2. User hovers over option
   └─> Option highlights (blue background)
       └─> Left border appears
           └─> Padding shifts slightly

3. User clicks option
   └─> Option selected & highlighted
       ├─> Dropdown closes
       ├─> Button displays selected option with icon
       ├─> Hidden select element updated
       └─> Form submits (onchange handler)

4. User clicks outside dropdown
   └─> Dropdown closes smoothly
       └─> Button returns to default state
```

## Keyboard Navigation

| Key | Action |
|-----|--------|
| `Tab` | Focus on button |
| `Enter` | Toggle dropdown |
| `Space` | Open dropdown |
| `Arrow Down` | Open dropdown / next option |
| `Arrow Up` | Previous option |
| `Escape` | Close dropdown |
| `Enter` | Select highlighted option |

## Accessibility Features

✅ **ARIA Attributes**:
- `role="listbox"` on dropdown
- `role="option"` on each option
- `aria-haspopup="listbox"` on button
- `aria-expanded` toggles with state
- `aria-selected` on options

✅ **Keyboard Support**: Full keyboard navigation

✅ **Focus Management**: 
- Proper focus states with visible outline
- Focus visible on button and options

✅ **Screen Reader Support**:
- Semantic HTML structure
- ARIA labels for state changes
- Proper role assignments

## Responsive Design

| Screen Size | Behavior |
|-------------|----------|
| Desktop (> 768px) | Full width button, max-height 420px for dropdown |
| Tablet (481px - 768px) | Full width, responsive padding |
| Mobile (< 480px) | Full width button, max-height adapts |

### Mobile Considerations:
- Touch-friendly button size: 48px minimum height
- Adequate padding for finger taps
- Dropdown doesn't exceed screen height
- Scrollable options list if needed

## Testing the Dropdown

### Visual Test:
1. ✅ Button displays correct language with icon
2. ✅ Shadow appears on button
3. ✅ Hover effect works (shadow increases)
4. ✅ Click opens dropdown smoothly
5. ✅ Options display with icons
6. ✅ Hover on option shows blue background
7. ✅ Click option closes dropdown
8. ✅ Button updates with new selection
9. ✅ Click outside closes dropdown

### Functional Test:
1. ✅ Form submits on selection
2. ✅ Selected value is preserved
3. ✅ Page refreshes show correct selection
4. ✅ Multiple dropdowns work independently

### Keyboard Test:
1. ✅ Tab focuses on button
2. ✅ Space/Enter opens dropdown
3. ✅ Arrow keys navigate options
4. ✅ Enter selects option
5. ✅ Escape closes dropdown
6. ✅ Focus is visible on all interactive elements

---

## Reference Image Mapping

The implementation matches the reference image with:

| Image Element | Implementation |
|---------------|-----------------|
| Blue-Cyan gradient background | Page background (not part of dropdown) |
| White rounded button | `.custom-select-button` with white background |
| "Select your option" text | Displays selected language or placeholder |
| Chevron/Arrow icon | `.custom-select-chevron` shows ▲ or ▼ |
| White dropdown card | `.custom-select-dropdown` with white background |
| Option icons (Instagram, LinkedIn, etc.) | Language flags (emoji icons) |
| Option text | Language name displayed next to icon |
| Hover effect | Light blue background on hover |
| Rounded corners | 12px border-radius |
| Shadow effect | Multiple layered box-shadows |

---

**Last Updated**: 2024
**Status**: Production Ready ✅
