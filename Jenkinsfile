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
                withCredentials([usernamePassword(credentialsId: 'Docker', passwordVariable: 'pwd', usernameVariable: 'username')]) {
                    sh "sudo docker login -u ${env.username} -p ${env.pwd}"
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
}

