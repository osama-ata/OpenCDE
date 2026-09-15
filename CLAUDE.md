# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Git Auto-Commit Requirement (STRICT)

**Commit finished work as soon as possible — do not wait for the end of the session or for the user to ask.**

As soon as a coherent, working unit of change is done (a bug fix, a completed feature slice, a passing build, a config update), stage and commit it immediately rather than letting it accumulate uncommitted. Do not batch unrelated changes into one commit and do not hold commits back "to see if more changes come." After committing, push to the current branch's remote (`origin`) as soon as possible as well, so work is never left stranded locally.

Still follow standard git hygiene: review `git status`/`git diff` before staging, use clear commit messages, and never force-push or rewrite shared history without explicit user instruction. This auto-commit policy does not authorize destructive operations (`reset --hard`, force-push, history rewrites) or bypassing hooks/signing.

This repo has two remotes:
- `origin` — the working fork; push here.
- `upstream` — `Dangl-IT/Dangl.OpenCDE`, the original project; never push here.
