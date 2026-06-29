---
name: adr-keeper
version: 0.1.0
description: |
  Author and maintain Architecture Decision Records in MADR format. Writes the next-numbered ADR
  under docs/adr/, updates the index README table, flips superseded ADRs to "Superseded by", and
  keeps the "pending decisions" list current. Use when asked to "write an ADR", "record this
  decision", "document this architectural choice", or right after a significant design decision is
  made. Proactively suggest when a change introduces, reverses, or refines an architectural decision.
allowed-tools:
  - Read
  - Write
  - Edit
  - Glob
  - Grep
  - Bash
triggers:
  - write an adr
  - record this decision
  - document this architectural decision
  - add an adr
  - supersede an adr
---

# adr-keeper

Keep a clean, immutable, cross-linked trail of architecture decisions. This is the discipline
that produced 52 internally-consistent ADRs in the reference solution: every significant
decision is captured once, never silently edited, and superseded (not rewritten) when it changes.

## When to write an ADR

Write one for a decision that is **significant and not obvious from the code**: a layering rule, a
library adoption/removal, a persistence strategy, an error-handling model, an API style. Do **not**
write one for a typo fix, a rename, or a routine bug fix — those live in commits, not ADRs.

One ADR = one decision. If you find yourself writing "and also…", split it.

## Locating the ADR directory

Default location is `docs/adr/`. If it doesn't exist, create it plus an index `README.md` (see
below). Confirm the convention in the repo first:

```bash
ls docs/adr/ 2>/dev/null || ls **/adr/ 2>/dev/null
```

## Numbering and filename

1. Find the highest existing number:
   ```bash
   ls docs/adr/[0-9]*.md | sed 's#.*/##' | sort | tail -1
   ```
2. The new number is that **+ 1**, zero-padded to **4 digits**.
3. Filename: `NNNN-kebab-case-title.md` (e.g. `0046-password-hashing.md`).

## Writing the ADR

Use [`reference/madr-template.md`](reference/madr-template.md). Sections, in order:

- **Title** — `# NNNN — <imperative summary>`.
- **Status / Date / Deciders** metadata.
- **Context** — the forces and constraints; *why a decision is needed now*. Be honest about the
  tension (e.g. "Application gives up persistence ignorance").
- **Decision** — what was chosen, stated actively. Include the concrete mechanism.
- **Consequences** — split **Positive** and **Negative / costs**. Real trade-offs, not just upside.
- **Alternatives considered** — each with a one-line reason it was rejected. This is what makes an
  ADR trustworthy.

Keep it tight. The best ADRs in the reference are ~40–80 lines.

## Immutability and supersession

An **accepted ADR is immutable** — never edit its Decision after the fact. When a later decision
changes it:

1. Write a **new** ADR that references the old one in its Context ("supersedes ADR-00xx because…").
2. In the **old** ADR, change only the `Status` line to `Superseded by [NNNN](NNNN-...md)`.
3. Update both rows in the index (below).

This preserves the *history of thinking*, not just the current state.

## Index maintenance (`docs/adr/README.md`)

The index is a table plus a pending list. After writing/superseding an ADR:

- **Append a row** to the table: `| [NNNN](NNNN-title.md) | Title | Accepted |`.
- For a supersession, change the old row's Status to `Superseded by [NNNN](NNNN-title.md)`.
- Update **"Decisions still pending"**: remove the bullet this ADR resolves; add any new open
  question the decision surfaced.

If the index doesn't exist, create it with: a one-paragraph intro stating ADRs are immutable and
superseded rather than edited, the MADR format link, the table, and a "Decisions still pending"
section.

## Procedure (checklist)

1. Confirm the decision is ADR-worthy (significant, non-obvious). If not, say so and stop.
2. Locate `docs/adr/`; compute the next number.
3. Write `NNNN-title.md` from the template.
4. If it supersedes another, flip that ADR's Status line.
5. Update `docs/adr/README.md` (table row(s) + pending list).
6. Report the new ADR number and what index rows changed.

Pairs with `design-doc-keeper` (sync the living design doc) and is invoked as a step by
`sdlc-loop`.
