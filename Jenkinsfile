pipeline {
  agent any
  
  environment {
    // Define the home directory for SonarScanner
    MSBuildScannerHome = tool 'SonarScanner for MSBuild v5'
  }
  
  stages {
    // Begin SonarQube analysis
    stage('Begin SonarQube Analysis') {
      steps {
        scmSkip(deleteBuild: true, skipPattern:'.*\\[CI-SKIP\\].*')
        script {
          withSonarQubeEnv() {
            sh "dotnet ${MSBuildScannerHome}/SonarScanner.MSBuild.dll begin /k:\"panosru_Aviant\""
          }
        }
      }
    }
    
    // Build the project
    stage('Building') {
      steps {
        scmSkip(deleteBuild: true, skipPattern:'.*\\[CI-SKIP\\].*')
        sh 'dotnet restore Aviant.sln'
        sh "dotnet build"
      }
    }
    
    // Complete SonarQube analysis
    stage('Complete SonarQube Analysis') {
      steps {
        scmSkip(deleteBuild: true, skipPattern:'.*\\[CI-SKIP\\].*')
        script {
          withSonarQubeEnv() {
            sh "dotnet ${MSBuildScannerHome}/SonarScanner.MSBuild.dll end"
          }
        }
      }
    }
    
    // Check the quality gate status
    stage('Quality Gate') {
      steps {
        scmSkip(deleteBuild: true, skipPattern:'.*\\[CI-SKIP\\].*')
        echo 'Waiting for quality gate to pass'
        timeout(time: 2, unit: 'MINUTES') {
          waitForQualityGate abortPipeline: true
        }
      }
    }
  }
  
  post {
    cleanup {
      catchError(buildResult: null, stageResult: 'FAILURE', message: 'Cleanup Failure') {
        echo 'Cleaning up workspace...'
        cleanWs()
      }
    }
  }
}
