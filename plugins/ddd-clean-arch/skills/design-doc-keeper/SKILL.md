---
name: design-doc-keeper
version: 0.1.0
description: |
  Keep a single living design document in sync with reality after a change. Updates the right
  section in place, maintains a "Known limitations" list and a "Roadmap" table, and cross-links the
  ADR that justifies each decision. Use when asked to "update the design doc", "sync design.md",
  "document this in the design", or after a feature/refactor lands. Proactively suggest after a
  change that alters behavior, structure, or a documented limitation.
allowed-tools:
  - Read
  - Write
  - Edit
  - Glob
  - Grep
---

# design-doc-keeper

Maintain one **living** design document (`docs/design.md`) that always describes the system **as it
is now** — not a changelog of how it got here. It is the counterpart to `adr-keeper`: ADRs record
*why a decision was made* (immutable, point-in-time); the design doc records *what the system
currently is* (mutable, always current), with each section linking out to the ADRs behind it.

## The living-doc principle

- Describe the **present**. When behavior changes, rewrite the affected section to match the new
  reality — don't append "previously it was X, now it's Y".
- The **only** backward-looking notes allowed are short, clearly-marked `> Refactor note:` asides
  where the *transition itself* is instructive (e.g. "the AuthenticationService is gone; Register is
  now a command handler"). Use sparingly.
- Every non-trivial claim **links to its ADR**: `([ADR-0046](adr/0046-password-hashing.md))`.
- Keep it honest: a `## Known limitations` section that lists what's *not* done is as valuable as
  the rest.

## Recommended structure

The reference design doc is organized as numbered sections; adapt to the project but keep this spine:

1. **Product** — what it is, in domain terms.
2. **Architectural style** — the layering/dependency rule, with a diagram.
3. **Projects and responsibilities** — a table: project · depends-on · responsibility.
4. **Feature slices** — per-feature request→response flow (the worked example).
5. **Composition / DI** — how the app is wired; the pipeline order.
6. **Conventions** — naming, routing, flow control, validation, mapping, testing.
7. **Build & run** — the exact commands; a smoke test.
8. **Known limitations** — current shortcomings, each linked to a deferred decision.
9. **Roadmap** — a table of next/done items, struck through as they land.

## After a change — what to touch

1. **The relevant section(s)** — edit in place to describe the new behavior/structure.
2. **§8 Known limitations** — remove a limitation the change resolves; add one it introduces.
3. **§9 Roadmap** — move the row to done (`~~strike-through~~` + "**done**") or add a new next item.
4. **Cross-links** — add the `[ADR-00xx]` reference for any decision the change recorded.
5. **The status/"last updated" line** at the top, if the doc has one.

## Procedure

1. Read the current `docs/design.md` (create it from the structure above if absent).
2. Identify which sections the change affects — usually 1–3, plus §8 and §9.
3. Edit those sections in place; keep the prose dense and specific (name the files/types involved).
4. Verify every new claim either links to an ADR or is plainly observable in the code.
5. Report which sections changed.

Pairs with `adr-keeper`; invoked as a step by `sdlc-loop`.
