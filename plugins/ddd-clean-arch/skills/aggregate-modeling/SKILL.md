---
name: aggregate-modeling
version: 0.1.0
description: |
  Derive the tactical-DDD holy grail — aggregates — from a bounded context, using the "aggregate
  game": start with every entity as its own aggregate, split out anything shared, prefer many small
  aggregates, and merge only when an invariant forces strong consistency. Produces an aggregate model
  (roots, local entities/value objects, reference-by-id). Use when asked to "model aggregates",
  "find aggregate boundaries", "do tactical design", or after strategic design. Proactively suggest
  before generating code for a new domain.
allowed-tools:
  - Read
  - Write
  - Edit
  - Glob
  - AskUserQuestion
triggers:
  - model aggregates
  - find aggregate boundaries
  - tactical design
  - aggregate game
---

# aggregate-modeling

The **tactical** phase of DDD: inside one bounded context, decide which entities are **aggregate
roots**, which are **local entities / value objects**, and how aggregates reference each other.
Aggregates are **transactional / consistency boundaries** — a single aggregate is loaded, changed,
and committed as a whole.

This is step 3: [`event-storming`](../event-storming/SKILL.md) →
[`strategic-ddd`](../strategic-ddd/SKILL.md) → `aggregate-modeling`. Its output feeds the code
generators in Family C ([`aggregate-slice-generator`](../aggregate-slice-generator/SKILL.md)).

## The "aggregate game" (how to derive the model)

1. **Lay out every entity** surfaced by process modeling.
2. **Treat each as its own aggregate** to start.
3. **Find entities that appear inside more than one aggregate.** Aggregates must not overlap, so
   such an entity becomes its **own aggregate root**, and every other place that referred to it
   instead holds a **value-object id** (`MenuId`, `DinnerId`, …) pointing at that root.
4. **Prefer as many small aggregates as possible.** A bigger aggregate must be loaded, changed, and
   committed whole — larger boundaries mean more locking and concurrency contention. Only **merge**
   two aggregates when an **invariant** forces strong consistency between them.
5. **Stop when every diagram has exactly one root on top** and that root appears nowhere else except
   as an id reference. Then the domain is consistently modeled.

## Rules of thumb

- **Reference other aggregates by id, never by object reference.** Holding a `MenuId` (a value
  object), not a `Menu`, keeps the boundary crisp and the load small.
- **Invariants drive boundaries.** If two things must always be consistent *atomically*, they
  belong in one aggregate; if eventual consistency is acceptable, split them and connect with a
  domain event (see the `DinnerCreated → add id to Menu` pattern).
- **Local entities** (unique only *within* the aggregate, e.g. a `MenuSection`) and **value
  objects** (no identity, e.g. `Price`, `Location`) live inside their root — they are not roots.
- **Many small aggregates** beats one big one almost always.

## Output: `docs/aggregates/`

A `README.md` describing the model (and the game used to derive it), plus one
`aggregate.<name>.md` per aggregate root: its id type, local entities, value objects, and the
id-references it holds to other aggregates. No code yet — this is the model the code follows.

## Cross-aggregate consistency

When an action in one aggregate must affect another, it is **not** a direct write (that would cross
a transactional boundary). Model it as a **domain event** raised by the source aggregate and handled
to update the target. Note these event flows in the aggregate docs so the implementation knows to
wire them.

## Procedure

1. Read the strategic design; pick the bounded context to model.
2. Play the aggregate game over its entities (steps 1–5 above), asking the user about invariants
   where consistency requirements are unclear (`AskUserQuestion`).
3. For each root, list local entities, value objects, and id-references to other roots.
4. Note cross-aggregate flows as domain events.
5. Write `docs/aggregates/README.md` + one file per root.
6. Optionally record the boundary decisions as an ADR via `adr-keeper`; hand off to
   `aggregate-slice-generator` to implement each root.
