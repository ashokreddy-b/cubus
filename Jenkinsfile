pipeline {
    agent any
    stages {
        stage('Checkout') {
            steps {
              git branch: 'main', url: 'https://github.com/ashokreddy-b/cubus.git'
                sh 'echo "checkout Successgfully"'
            }
        }

        stage('Build') {
            steps {
                // Use the .NET CLI to build the application
                sh "dotnet build"
            }
        }

        stage('Create Docker Image') {
            steps {
                // Build the Docker image using the Dockerfile in the project directory
                sh "docker build -t bapathuashokreddy/cubus:latest ."
            }
        }

        stage('push Image to docker hub')
        {
            steps{
            withCredentials([usernamePassword(credentialsId: 'Dockercredentilas', passwordVariable: 'pwd', usernameVariable: 'user')]) {
                sh "sudo docker login -u ${env.user} -p ${env.pwd}"
                    sh 'sudo docker push bapathuashokreddy/cubus:latest'
                }
                
            }
        }
      stage('Run the container')
      {
        steps{
          sh 'docker run -d -p 9000:80 --name Cubus bapathuashokreddy/cubus:latest'
        }
      }
    }
     post {
        success {
            emailext (
                subject: "Build Status: ${currentBuild.currentResult}",
                body: "The build status is: ${currentBuild.currentResult}",
                recipientProviders: [[$class: 'CulpritsRecipientProvider']],
                to: "recipient@example.com" 
            )
        }
         failure {
            // This stage will always run, regardless of the build result
            emailext (
                subject: "Build Status: ${currentBuild.currentResult}",
                body: "The build status is: ${currentBuild.currentResult}",
                recipientProviders: [[$class: 'CulpritsRecipientProvider']],
                to: "bapathuashokreddy@gmail.com"  // Replace with the recipient's email address
            )
        }
    }
}

