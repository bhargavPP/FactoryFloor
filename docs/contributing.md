# Contributing to FactoryFloor

Thanks for your interest in contributing. This document outlines basic guidelines and workflows.

Code style & practices
- Follow C# conventions used across the repo (minimal APIs, record DTOs, small single-responsibility services).
- Keep changes small and well-scoped. Add unit tests where appropriate.

Branching
- Use feature branches off master: feature/<short-description>
- Rebase or merge master before creating pull requests.

Pull requests
- Open PRs against master. Include a short summary of changes and testing steps.
- Add reviewers and link related issues. Use small PRs for reviewability.

Testing
- Run local services and exercise API endpoints described in project READMEs.
- For integration tests, use docker-compose to spin up dependencies.

Security
- Do not commit secrets (JWT secrets, DB passwords). Use environment variables or secret management.

Reporting issues
- Open issues with steps to reproduce, expected behavior, and logs if available.

Maintainers
- Maintainers will review and merge PRs. Ensure CI passes before merging.
