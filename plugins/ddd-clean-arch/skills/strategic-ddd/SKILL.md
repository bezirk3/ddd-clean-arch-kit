---
name: strategic-ddd
version: 0.1.0
description: |
  Turn a domain process model into the strategic-DDD holy grail — bounded contexts plus a context
  map. Identifies logical boundaries within which a single model applies (so the same word can mean
  different things in different contexts), and defines how the contexts relate. Use when asked to
  "find bounded contexts", "do strategic design", "context map", or after event storming. Proactively
  suggest between a process model and aggregate modeling.
allowed-tools:
  - Read
  - Write
  - Edit
  - Glob
  - AskUserQuestion
triggers:
  - find bounded contexts
  - strategic design
  - context mapping
  - bounded context
---

# strategic-ddd

DDD has two phases, each with a "holy grail":

| Phase | Explores | Holy grail |
| ----- | -------- | ---------- |
| **Strategic** | the *problem* space → the *solution* space | **Bounded contexts** (+ context mapping) |
| **Tactical** | inside one bounded context | **Aggregates** (driven by invariants) |

This skill is the **strategic** phase. It consumes a process model (from
[`event-storming`](../event-storming/SKILL.md)) and produces bounded contexts; tactical modeling
(aggregates) follows in [`aggregate-modeling`](../aggregate-modeling/SKILL.md). The flow is **not
linear** — you move back and forth between problem and solution space as understanding grows.

## Bounded contexts

> A **bounded context** is a *logical boundary within which a domain model applies*.

The key insight: **the same word can mean different things in different contexts — and that's the
point.** A `Host` in a *User Management* context is a profile (account data); a `Host` in a *Dinner
Management* context is a different model (e.g. carries a list of menus). Same word, different
meaning, because they live in different contexts. Forcing one shared model across the whole domain
is the mistake strategic design exists to prevent.

Identify candidate contexts by grouping the process model's commands/events/read-models by **which
mental model they share** — what is each cluster *responsible for*, and what is its *own* model of
the entities it touches.

| Bounded context | Responsible for | Holds (its model of) |
| --------------- | --------------- | -------------------- |
| User Management | accounts, host/guest *profiles* | `User`, `Host` (profile), `Guest` (profile) |
| Dinner Management | creating menus and dinners | `Host`, `Menu`, `MenuReview`, `Dinner` |
| Reservation | reservations, billing | `Dinner`, `Reservation`, `Bill` |

(The table above is the Forkfully example — entities legitimately appear in more than one context
with *different models*.)

## Context mapping

Once contexts exist, define **how they relate** — the integration patterns between them
(e.g. creating a host profile in *User Management* triggers a counterpart in *Dinner Management*).
Name the relationship for each pair of contexts that exchange data.

## Single vs. multiple contexts (a real decision)

A small system often starts as **one** bounded context, with the candidate contexts mapped for a
*future* split. That is a legitimate, documented choice — record it as an ADR (via
[`adr-keeper`](../adr-keeper/SKILL.md)), e.g. "Single bounded context now; candidate contexts mapped
for a future split." Don't split prematurely.

## Output: `docs/strategic-design.md`

The two-phase framing, the candidate bounded-contexts table (context · responsible-for · holds),
the context map (relationships), and the explicit single-vs-multiple decision with a pointer to the
ADR. No code.

## Procedure

1. Read the process model; cluster its commands/events/read-models by shared mental model.
2. Draft candidate bounded contexts; for each, state responsibility + its own model of shared
   entities (highlight where a word means different things across contexts).
3. Define the context map (relationships between contexts).
4. Decide single vs. multiple contexts now; record it via `adr-keeper`.
5. Write `docs/strategic-design.md`; hand off to `aggregate-modeling` for the chosen context(s).
