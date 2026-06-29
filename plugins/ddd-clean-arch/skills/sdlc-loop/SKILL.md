---
name: sdlc-loop
version: 0.1.0
description: |
  Orchestrate a single change end-to-end with the discipline that produced 52 consistent ADRs:
  implement → record the ADR → sync the design doc → update the ADR index → build → end-to-end
  verify → unit + architecture tests, stopping at a gate the moment a step fails. Use when asked to
  "do this properly", "full SDLC", "implement and document this", "land this change cleanly", or
  when a change is significant enough to warrant the full ritual. Proactively suggest for any change
  that touches architecture, adds a feature, or alters documented behavior.
allowed-tools:
  - Read
  - Write
  - Edit
  - Glob
  - Grep
  - Bash
  - Skill
  - AskUserQuestion
triggers:
  - full sdlc
  - do this properly
  - implement and document this
  - land this change cleanly
  - run the full ritual
---

# sdlc-loop

The standing ritual for landing one change the way the reference solution was built. It is a
**gated pipeline**: each stage must pass before the next runs. Its job is not to write code fast —
it is to make sure that when a change lands, the **decision is recorded, the docs match reality, and
the build + tests are green**.

This skill orchestrates two others: it calls [`adr-keeper`](../adr-keeper/SKILL.md) and
[`design-doc-keeper`](../design-doc-keeper/SKILL.md) at the right stages.

## Scope check (run first)

Confirm the change is worth the full loop. Trivial fixes (typos, comments, formatting) skip it —
say so and just make the change. The loop is for features, refactors, library swaps, and anything
that creates or changes an architectural decision.

If a decision is involved but ambiguous (which approach, which trade-off), resolve it with
`AskUserQuestion` **before** implementing — not after.

## The pipeline (gated — stop on first failure)

1. **Implement** — the smallest correct change that fully satisfies the requirement. Match the
   surrounding code's idiom, naming, and structure. No drive-by refactors.

2. **ADR** — if the change *makes or changes an architectural decision*, invoke `adr-keeper` to
   write the next-numbered ADR (and flip any superseded ADR). Skip only if there is genuinely no
   decision (rare for a loop-worthy change).

3. **Design doc** — invoke `design-doc-keeper` to update `docs/design.md`: the affected section(s),
   the limitations list, the roadmap row, and the new ADR cross-link.

4. **ADR index** — confirm `docs/adr/README.md` gained the new row and the pending list was updated
   (`adr-keeper` does this; verify it happened).

5. **Build — GATE.** Build the solution; require **0 errors**. Detect the toolchain:
   - .NET: `dotnet build` (e.g. `dotnet build <Solution>.slnx`)
   - node: `npm run build` · rust: `cargo build` · etc.
   If it fails, stop and fix before continuing.

6. **End-to-end verify — GATE.** Exercise the *actual changed path* against real dependencies, not
   just a compile. For a web API: run it (`dotnet run --project <Api>`), hit the changed endpoint(s),
   and confirm the observable behavior — status codes, persisted rows, headers. The reference loop
   always verified against a live LocalDB (register → row with a hashed password; create → `201` +
   `Location`; unauthorized → `401`). Report what you actually observed.

7. **Tests — GATE.** Run the full suite; require **all green**:
   - Unit tests for the new behavior (add them if the change introduced testable logic).
   - **Architecture tests** — if the repo has them (NetArchTest / dependency-rule guards), they must
     stay green, proving the change obeys the structural ADRs. (See `arch-test-generator` to add
     them if missing.)
   - .NET: `dotnet test`.

8. **Report** — a short summary: what changed, ADR number(s), design-doc sections touched, build
   result, what the e2e check observed, test counts.

## Gates, explicitly

- A failed build, a failed e2e check, or a red test **halts the loop**. Do not paper over it, do
  not mark the step done, do not proceed. Fix the cause or surface it to the user.
- Never claim a stage passed without running it. "Tests pass" requires having run the tests in this
  session and seeing them pass.

## Adapting to a non-.NET project

The shape is universal; only the commands differ. Detect build/test commands from the repo
(`package.json` scripts, `Makefile`, `Cargo.toml`, CI config). The doc/ADR stages are
language-agnostic and always apply.

## One-line summary of the discipline

> Implement → ADR → design doc → index → **build** → **e2e** → **tests**, and don't lie about the gates.
