pipeline {
    agent any
    stages {
        stage('Checkout Code from GitHub') {
            steps {
              git branch: 'main', url: 'https://github.com/ashokreddy-b/cubus.git'
                sh 'echo "checkout Successgfully"'
            }
        }

        stage('Build the Application') {
            steps {
                // Use the .NET CLI to build the application
                sh "dotnet build"
                 sh 'echo "Build Successfully"'
            }
        }

        stage('Create Docker Image') {
            steps {
                // Build the Docker image using the Dockerfile in the project directory
                sh "docker build -t bapathuashokreddy/cubus:latest ."
            }
        }

        stage('Docker hub login and push Docker Image to docker hub')
        {
            steps{
                withCredentials([usernamePassword(credentialsId: 'Docker', passwordVariable: 'pwd', usernameVariable: 'user')]) {
                  sh "docker login -u ${env.user} -p ${env.pwd}"
                    sh 'docker push bapathuashokreddy/cubus:latest'
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
                subject: "Cubus Pipeline Status:: ${currentBuild.currentResult}",
                body: "The build status is: ${currentBuild.currentResult}",
                recipientProviders: [[$class: 'CulpritsRecipientProvider']],
                to: "bapathu.ashokreddy@avinsystems.com" 
            )
        }
         failure {
            // This stage will always run, regardless of the build result
            emailext (
                subject: "Cubus Pipeline Status: ${currentBuild.currentResult}",
                body: "The build status is: ${currentBuild.currentResult}",
                recipientProviders: [[$class: 'CulpritsRecipientProvider']],
                to: "bapathu.ashokreddy@avinsystems.com"  // Replace with the recipient's email address
            )
        }
    }
}

