pipeline {
  agent any
  stages {
    stage('Restore NuGet Packages') {
      steps {
        sh 'dotnet restore Aviant.DDD.sln'
      }
    }
    stage('SonarQube Analysis') {
      steps {
        script {
          def scannerHome = tool 'SonarScanner for MSBuild v5.3.2'
          withSonarQubeEnv() {
            sh "dotnet ${scannerHome}/SonarScanner.MSBuild.dll begin /k:\"panosru_Aviant.DDD\""
            sh "dotnet build"
            sh "dotnet ${scannerHome}/SonarScanner.MSBuild.dll end"
          }
        }
      }
    }
  }
}
