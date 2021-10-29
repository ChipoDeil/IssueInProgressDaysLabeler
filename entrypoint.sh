#!/bin/sh -l

cd /app

dotnet restore
dotnet build
dotnet run --project IssueInProgressDaysLabeler.Model -- \
    --owner "$owner" \
    --repository-name "$repository-name" \
    --labels "$labels" \
    --github-token "$github-token"
