#!/bin/sh -l

cd /app

dotnet IssueInProgressDaysLabeler.Model.dll -- \
    --github-repository "$GITHUB_REPOSITORY" \
    --days-mode "$days_mode" \
    --labels "$labels" \
    --github-token "$github_token" \
    --label-to-increment "$label_to_increment"
