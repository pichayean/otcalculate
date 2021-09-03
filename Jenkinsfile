def code  = '';
def notify(message, stickerId) {
    def token = "vev2IewXbmQjYDpzKrFHmDBAEmLseBZqDR5H1kATykA";
    def buildNo = env.BUILD_NUMBER;

    def url = "https://notify-api.line.me/api/notify";
    def lineMessage = "otcalculate-app-${message} ${buildNo} \r\n";
	sh "curl ${url} -H 'Authorization: Bearer ${token}' -F 'message=${lineMessage}' -F 'stickerId=${stickerId}' -F 'stickerPackageId=1'";
}
def getDockerTag(){
    def tag  = sh script: 'git rev-parse HEAD', returnStdout: true
    return tag
}
def getOldDockerTag(){
    def oldtag  = sh script: 'git rev-parse @~', returnStdout: true
    return oldtag
}

pipeline {
    agent any
    environment {
        NEW_VERSION = '1.0.0'
        CI = 'true'
        DOCKER_TAG = getDockerTag()
        DOCKER_OLD_TAG = getOldDockerTag()
    }
    stages {
        stage('Staging') {
            steps {
                sh 'docker build --no-cache -t otcalculate:${DOCKER_TAG} "."';
            }
        }
        stage('Deployment to kube') {
            steps {
                sh "chmod +x sedtag.sh"
                sh "./sedtag.sh ${DOCKER_TAG}"
                sh 'kubectl apply -f app-deployment.yml';
            }
        }
    }
}
