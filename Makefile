.SILENT:
.PHONY: sonar

include .env
export $(shell sed 's/=.*//' .env)

sonar:
	echo "Running sonarqube analysis"
	dotnet sonarscanner begin /k:"${SONAR_PROJECT_NAME}" /d:sonar.host.url="${SONAR_HOST_URL}"  /d:sonar.login="${SONAR_LOGIN}"
	dotnet build
	dotnet sonarscanner end /d:sonar.login="${SONAR_LOGIN}"
