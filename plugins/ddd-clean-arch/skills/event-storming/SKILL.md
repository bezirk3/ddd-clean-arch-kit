---
name: event-storming
version: 0.1.0
description: |
  Map a domain's problem space with process modeling (a flavor of event storming) BEFORE any code
  or aggregates. Produces a domain-process-model.md: an initial command aimed at a terminal event,
  pivotal events as anchors, gaps filled with command → event → (policy | read model) chains, and
  every open question marked as a hotspot. Use when asked to "event storm", "model the domain",
  "map the process", or at the very start of a new domain. Proactively suggest before aggregate
  design on a greenfield domain.
allowed-tools:
  - Read
  - Write
  - Edit
  - Glob
  - AskUserQuestion
triggers:
  - event storm
  - process model the domain
  - map the domain process
  - explore the problem space
---

# event-storming

Explore the **problem space** of a domain *with a domain expert*, producing a process model that is
the input to strategic and tactical DDD. The cardinal rule: **resist modeling aggregates, methods,
or responsibilities yet.** The goal here is to map the *whole* process and mark every open question
as a **hotspot** — not to design the solution.

This is step 1 of three: `event-storming` → [`strategic-ddd`](../strategic-ddd/SKILL.md) →
[`aggregate-modeling`](../aggregate-modeling/SKILL.md).

## Method (process modeling)

1. Start from an initial **command** and aim at a terminal **event**.
2. Drop a few **pivotal events** in between as anchors — the interesting milestones.
3. Fill the gaps with chains of `command → event → (policy | read model)`.
4. **Do not** decide aggregate boundaries or method ownership. Where something is unknown or risky,
   drop a **hotspot** and move on.

It is done **with a domain expert**, not alone — the model is only as good as the conversation. If
you lack domain answers, use `AskUserQuestion` to interview the user as the stand-in expert; capture
unknowns as hotspots rather than guessing.

## Legend (event-storming stickies)

| Symbol | Colour | Meaning |
| ------ | ------ | ------- |
| `[Actor]` | yellow | a person who issues a command |
| `Command` | blue | an intent / action |
| `«Event»` | orange | something that happened (past tense) |
| `(System)` | — | the consistency boundary a command acts on (a *future* aggregate — don't name it yet) |
| `→ Policy` | purple | reactive rule: *whenever* an event happens, *then* issue a command |
| `{Read model}` | green | data shown to an actor so they can decide what to do next |
| `‹External›` | pink | an external system that performs work (email, payments) |
| `HOTSPOT` | red | an open question / risk to resolve later |

## Output: `docs/domain-process-model.md`

A document that opens with a one-line scope (the **initial command** and **terminal event**, plus
any already-established context that's out of scope), the legend, then the process as ordered
`command → «event» → (policy/read model)` chains with hotspots inline. Keep it prose + the symbol
vocabulary; no code.

See [`reference/example-process-model.md`](reference/example-process-model.md) for the shape (the
Forkfully dinner-hosting flow, condensed).

## Procedure

1. Establish scope with the user: the initial command, the terminal event, and what context is
   already assumed (e.g. "user is registered and approved as a host").
2. Lay down pivotal events as anchors.
3. Walk the flow filling `command → event → (policy | read model)`; involve external systems where
   work leaves the domain.
4. Mark every unknown/risk as a `HOTSPOT` — do not resolve them by guessing.
5. Write `docs/domain-process-model.md`; list the hotspots so they carry forward.
6. Hand off to `strategic-ddd` (contexts) — *not* straight to aggregates.
