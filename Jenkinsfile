pipeline {
  agent any
  
  environment {
    MSBuildScannerHome = tool 'SonarScanner for MSBuild v5'
  }
  
  stages {
    
    stage('Begin SonarQube Analysis') {
      steps {
        script {
          withSonarQubeEnv() {
            sh "dotnet ${MSBuildScannerHome}/SonarScanner.MSBuild.dll begin /k:\"panosru_Aviant.DDD\""
          }
        }
      }
    }
    
    stage('Building') {
      steps {
        sh 'dotnet restore Aviant.DDD.sln'
        sh "dotnet build"
      }
    }
    
    stage('Complete SonarQube Analysis') {
      steps {
        script {
          withSonarQubeEnv() {
            sh "dotnet ${MSBuildScannerHome}/SonarScanner.MSBuild.dll end"
          }
        }
      }
    }
    
    stage('Quality Gate') {
      steps {
        timeout(time: 10, unit: 'MINUTES') {
          script {
            def qg = waitForQualityGate()
            if ('OK' != qg.status) {
              error "Pipeline aborted due to quality gate failure: ${qg.status}"
            }
          }
        }
      }
    }
  }
  
  post {
    cleanup {
      delete '**/SonarQube.xml'
    }
  }
}
