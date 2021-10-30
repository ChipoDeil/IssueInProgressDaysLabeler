#!/bin/sh -l

cd /app

dotnet restore
dotnet build
dotnet run --project IssueInProgressDaysLabeler.Model -- \
    --owner "$GITHUB_REPOSITORY_OWNER"\
    --repository-name "$GITHUB_REPOSITORY" \
    --days-mode "$days_mode" \
    --labels "$labels" \
    --github-token "$github_token"
