pipeline {
  agent any
  stages {
    stage('检出') {
      steps {
        checkout([$class: 'GitSCM',
        branches: [[name: GIT_BUILD_REF]],
        userRemoteConfigs: [[
          url: GIT_REPO_URL,
          credentialsId: CREDENTIALS_ID
        ]]])
      }
    }
    stage('构建镜像并推送到 CODING Docker 制品库') {
      steps {
        script {
          docker.withRegistry(
            "${CCI_CURRENT_WEB_PROTOCOL}://${CODING_DOCKER_REG_HOST}",
            "${CODING_ARTIFACTS_CREDENTIALS_ID}"
          ) {        
            def branchName = "${GIT_LOCAL_BRANCH}"
            def dockerImageVersion = branchName.replace('/','-')
            echo dockerImageVersion

            def dockerApiImage = docker.build("${CODING_DOCKER_IMAGE_API_NAME}:${dockerImageVersion}", "-f ${DOCKERFILE_PATH_API} ${DOCKER_BUILD_CONTEXT}")
            dockerApiImage.push()

            def dockerIDS4Image = docker.build("${CODING_DOCKER_IMAGE_IDS4_NAME}:${dockerImageVersion}", "-f ${DOCKERFILE_PATH_IDS4} ${DOCKER_BUILD_CONTEXT}")
            dockerIDS4Image.push()
          }
        }

      }
    }
  }
  environment {
    CODING_DOCKER_REG_HOST = "${CCI_CURRENT_TEAM}-docker.pkg.${CCI_CURRENT_DOMAIN}"
    CODING_DOCKER_IMAGE_API_NAME = "${PROJECT_NAME.toLowerCase()}/${DOCKER_REPO_NAME}/${DOCKER_IMAGE_API_NAME}"
    CODING_DOCKER_IMAGE_IDS4_NAME = "${PROJECT_NAME.toLowerCase()}/${DOCKER_REPO_NAME}/${DOCKER_IMAGE_IDS4_NAME}"
  }
}