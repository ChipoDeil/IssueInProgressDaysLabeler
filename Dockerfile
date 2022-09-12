FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

COPY "src/IssueInProgressDaysLabeler.Model/IssueInProgressDaysLabeler.Model.csproj" \
  "src/IssueInProgressDaysLabeler.Model/IssueInProgressDaysLabeler.Model.csproj"
RUN dotnet restore "src/IssueInProgressDaysLabeler.Model/IssueInProgressDaysLabeler.Model.csproj"

COPY . .
RUN dotnet build "src/IssueInProgressDaysLabeler.Model/IssueInProgressDaysLabeler.Model.csproj" -c Release -o ./app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS release

LABEL "com.github.actions.name"="Issues tracking time in progress"
LABEL "com.github.actions.description"="Visualize time issues spend in assigned state"
LABEL "com.github.actions.icon"="bar-chart-2"
LABEL "com.github.actions.color"="green"

LABEL "repository"="https://github.com/ChipoDeil/IssueInProgressDaysLabeler"

COPY --from=build /app /app
COPY entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

ENTRYPOINT ["/entrypoint.sh"]
