FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

LABEL "com.github.actions.name"="Issues tracking time in progress"
LABEL "com.github.actions.description"="Visualize time issues spend in assigned state"
LABEL "com.github.actions.icon"="bar-chart-2"
LABEL "com.github.actions.color"="green"

LABEL "repository"="https://github.com/ChipoDeil/IssueInProgressDaysLabeler"

COPY *.sln .
COPY IssueInProgressDaysLabeler.Model/ ./IssueInProgressDaysLabeler.Model/

COPY entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

ENTRYPOINT ["/entrypoint.sh"]
