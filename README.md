# ddd-clean-arch-kit

A Claude Code plugin marketplace that packages a **repeatable DDD + Clean Architecture
methodology for .NET** — extracted from the *Forkfully* reference solution and its 52
architecture decision records.

It contains one plugin, **`ddd-clean-arch`**, exposing skills across three families:

### A — DDD system design (language-agnostic)
| Skill | Turns… | …into |
| ----- | ------ | ----- |
| `event-storming` | a problem statement | a domain process model (commands → events → policies → read models → hotspots) |
| `strategic-ddd` | a process model | bounded contexts + a context map |
| `aggregate-modeling` | bounded contexts | an aggregate model (roots, local entities/VOs, reference-by-id) |

### B — Method / SDLC discipline (language-agnostic)
| Skill | Does |
| ----- | ---- |
| `adr-keeper` | writes the next-numbered MADR ADR, maintains the index, marks supersessions |
| `design-doc-keeper` | keeps a living design doc (sections + limitations + roadmap) in sync after a change |
| `sdlc-loop` | orchestrates one change end-to-end: implement → ADR → design doc → build → e2e → tests |

### C — .NET scaffolding
| Skill | Does |
| ----- | ---- |
| `clean-arch-template` | scaffolds a new solution from the `dotnet new` Clean Architecture + DDD template |
| `aggregate-slice-generator` | generates a full vertical slice for a new aggregate (domain → EF → CQRS → endpoints → tests) |
| `arch-test-generator` | generates/refreshes NetArchTest rules that encode the structural decisions |

## Install

From a Claude Code session (any project):

```bash
# add this marketplace (local path or a git URL)
claude plugin marketplace add /d/repo/local/ddd-clean-arch-kit
# install the plugin
claude plugin install ddd-clean-arch@ddd-clean-arch-kit
```

The skills then become invocable as `/<skill-name>` (e.g. `/sdlc-loop`).

## Provenance

Every skill embeds the method *and* a worked example lifted from the Forkfully solution
(`d:\repo\local\.poc\ddd-template`). The skills are the generalized, reusable form of the
decisions recorded in that repo's `docs/adr/` and `docs/design.md`.

## License

MIT — see [LICENSE](LICENSE).
"# ddd-clean-arch-kit" 
