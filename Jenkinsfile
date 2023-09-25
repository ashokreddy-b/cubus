pipeline {
    agent any

    environment {
        DOTNET_CLI_HOME = tool 'DotNet'
    }

    stages {
        stage('Checkout') {
            steps {
              git branch: 'main', url: 'https://github.com/ashokreddy-b/cubus.git'
            }
        }

        stage('Build') {
            steps {
                // Use the .NET CLI to build the application
                sh "${DOTNET_CLI_HOME} build"
            }
        }

        stage('Create Docker Image') {
            steps {
                // Build the Docker image using the Dockerfile in the project directory
                sh "docker build -t bapathuashokreddy/Cubus:latest ."
            }
        }

        stage('push Image to docker hub')
        {
            steps{
                withCredentials([usernamePassword(credentialsId: 'Docker', passwordVariable: 'pwd', usernameVariable: 'username')]) {
                    sh "sudo docker login -u ${env.username} -p ${env.pwd}"
                    sh 'sudo docker push bapathuashokreddy/Cubus:latest'
                }
                
            }
        }
      stage('Run the container')
      {
        steps{
          sh 'docker run -d -p 9000:80 --name Cubus bapathuashokreddy/Cubus:latest'
        }
      }
}
}

