#!/bin/sh -l

cd /app

dotnet restore
dotnet build
dotnet run --project IssueInProgressDaysLabeler.Model -- \
    --github-repository "$GITHUB_REPOSITORY" \
    --days-mode "$days_mode" \
    --labels "$labels" \
    --github-token "$github_token" \
    --label-to-increment "$label_to_increment"
