pipeline {
  agent none
  stages {
    stage('Initialize') {
      agent {
        node {
          label "${env.AGENT_PREFIX}"
        }

      }
      steps {
        echo "Initialized on node: ${env.NODE_NAME}"
        dir(path: "${env.LOCAL_REPOSITORY}") {
          checkout scm
          script {
            def datas = readYaml file: "${env.CONFIG_FILE}"

            env.PROJECT_PATH = datas.project_path
            env.BUILD_NAME = datas.build_name
            env.PLATFORMS = datas.platforms.join(' ')

            def list = []
            datas.file_extensions.each { p ->
            list += p.name + ':' + p.ext
          }
          env.FILE_EXTENSIONS = list.join(' ')

          env.MAPPING_PROD_BRANCH = datas.mapping.prod.branch
          env.MAPPING_PROD_PRERELEASE = datas.mapping.prod.prerelease
          env.MAPPING_TEST_BRANCH = datas.mapping.test.branch
          env.MAPPING_TEST_PRERELEASE = datas.mapping.test.prerelease
          env.MAPPING_DEV_BRANCH = datas.mapping.dev.branch
          env.MAPPING_DEV_PRERELEASE = datas.mapping.dev.prerelease

          if(env.MAPPING_PROD_BRANCH == env.BRANCH_NAME) {
            env.IS_DEVELOPMENT_BUILD = env.MAPPING_PROD_PRERELEASE
          } else if(env.MAPPING_TEST_BRANCH == env.BRANCH_NAME) {
            env.IS_DEVELOPMENT_BUILD = env.MAPPING_TEST_PRERELEASE
          } else {
            env.IS_DEVELOPMENT_BUILD = env.MAPPING_DEV_PRERELEASE
          }
        }

      }

    }
  }

  stage('Preparing for build') {
    agent {
      node {
        label "${env.AGENT_PREFIX}"
      }

    }
    when {
      beforeAgent true
      anyOf {
        branch env.MAPPING_PROD_BRANCH;
        branch env.MAPPING_TEST_BRANCH;
        branch env.MAPPING_DEV_BRANCH
      }
      expression {
        return env.PLATFORMS.replaceAll("\\s","") != ""
      }

    }
    steps {
      dir(path: "${env.LOCAL_REPOSITORY}") {
        script {
          semantic.init env.MAPPING_PROD_BRANCH, env.MAPPING_TEST_BRANCH, env.MAPPING_DEV_BRANCH, env.MAPPING_PROD_PRERELEASE, env.MAPPING_TEST_PRERELEASE, env.MAPPING_DEV_PRERELEASE
        }

        script {
          env.VERSION = semantic.version "${env.GITHUB_CREDENTIALS_ID}"
        }

        echo "VERSION=${env.VERSION}"
      }

      script {
        unity.init env.UNITY_DOCKER_IMG
      }

      script {
        cloud.login env.GOOGLE_PROJECT, env.JENKINS_CREDENTIALS_ID
      }

    }
  }

  stage('Build') {
    when {
      beforeAgent true
      anyOf {
        branch env.MAPPING_PROD_BRANCH;
        branch env.MAPPING_TEST_BRANCH;
        branch env.MAPPING_DEV_BRANCH
      }
      expression {
        return env.PLATFORMS.replaceAll("\\s","") != ""
      }

    }
    steps {
      script {
        parallelize env.AGENT_PREFIX, env.PLATFORMS.split(' '), {
          PLATFORM ->
          try {
            sh (
              script: 'cp -al "$LOCAL_REPOSITORY/$PROJECT_PATH" .',
              label: 'Hard link project to workspace'
            )
            sh (
              script: 'ls -l "$PROJECT_PATH/Assets"',
              label: 'List project assets'
            )
            sh (
              script: 'cat "$LOCAL_REPOSITORY/$PROJECT_PATH/ProjectSettings/ProjectSettings.asset"',
              label: 'Print project settings'
            )

            cloud.uncache "//${env.CACHE_BUCKET}/${env.JOB_NAME}/${PLATFORM}", "${env.PROJECT_PATH}"

            unity.build env.WORKSPACE, env.UNITY_DOCKER_IMG, env.PROJECT_PATH, PLATFORM, env.FILE_EXTENSIONS, env.BUILD_NAME, env.VERSION, env.IS_DEVELOPMENT_BUILD

            cloud.cache "//${env.CACHE_BUCKET}/${env.JOB_NAME}/${PLATFORM}", "${env.PROJECT_PATH}", 'Library'

            sh 'sudo chown -R jenkins:jenkins bin'

            dir("bin/${PLATFORM}") {
              sh (
                script: "ls ${env.BUILD_NAME}",
                label: 'List artifacts'
              )
              sh (
                script: "mkdir -p ${env.LOCAL_REPOSITORY}/bin",
                label: 'Create repository bin if doesnt exist'
              )
              sh (
                script: "zip -r -m ${env.LOCAL_REPOSITORY}/bin/${env.BUILD_NAME}-${PLATFORM}.zip ${env.BUILD_NAME}",
                label: 'Archive artifacts'
              )
            }
          }
          finally {
            sh (
              script: 'sudo rm -rf ./**',
              label: 'Post workspace cleanup'
            )
          }
        }
      }

    }
  }

  stage('Publish') {
    agent {
      node {
        label "${env.AGENT_PREFIX}"
      }

    }
    when {
      beforeAgent true
      anyOf {
        branch env.MAPPING_PROD_BRANCH;
        branch env.MAPPING_TEST_BRANCH;
        branch env.MAPPING_DEV_BRANCH
      }
      expression {
        return env.PLATFORMS.replaceAll("\\s","") != ""
      }

    }
    steps {
      dir(path: "${env.LOCAL_REPOSITORY}") {
        script {
          semantic.release "${env.GITHUB_CREDENTIALS_ID}"
        }

      }

    }
  }

}
environment {
  GOOGLE_PROJECT = 'unity-firebuild'
  CACHE_BUCKET = 'unity-firebuild-cache'
  UNITY_DOCKER_IMG = 'darkbiker/unity3d:latest'
  AGENT_PREFIX = 'jenkins-agent'
  LOCAL_REPOSITORY = '/tmp/repository'
  JENKINS_CREDENTIALS_ID = 'jenkins-sa'
  GITHUB_CREDENTIALS_ID = 'github-credentials'
  CONFIG_FILE = 'unityci.yml'
  EMAIL_ADDRESS = 'development@naughtybikergames.io'
}
post {
  success {
    emailext(to: "${env.EMAIL_ADDRESS}", subject: "${env.JOB_NAME} - Build #${env.BUILD_NUMBER} - SUCCESS!", body: """
                                                                <html>
                                                                  <header></header>
                                                                  <body>
                                                                    <img src="${env.JENKINS_URL}/static/c5c835c9/images/48x48/blue.png" alt="blue" width="48" height="48" style="float:left" />
                                                                    <h1>BUILD SUCCESS</h1>
                                                                    <p>Project: UnityCI<br>
                                                                    Job: ${env.JOB_NAME}<br>
                                                                    Date of build: ${env.BUILD_TIMESTAMP}<br>
                                                                    Build duration: ${currentBuild.durationString}<br><br>
                                                                    Check console output <a href="${env.BUILD_URL}">here</a> to view the results.</p>
                                                                  </body>
                                                                </html>
                                                                                  """)
  }

  failure {
    emailext(to: "${env.EMAIL_ADDRESS}", subject: "${env.JOB_NAME} - Build #${env.BUILD_NUMBER} - FAILED!", body: """
                                                                <html>
                                                                  <header></header>
                                                                  <body>
                                                                    <img src="${env.JENKINS_URL}/static/c5c835c9/images/48x48/red.png" alt="red" width="48" height="48" style="float:left" />
                                                                    <h1>BUILD FAILED</h1>
                                                                    <p>Project: UnityCI<br>
                                                                    Job: ${env.JOB_NAME}<br>
                                                                    Date of build: ${env.BUILD_TIMESTAMP}<br>
                                                                    Build duration: ${currentBuild.durationString}<br><br>
                                                                    Check console output <a href="${env.BUILD_URL}">here</a> to view the results.</p>
                                                                  </body>
                                                                </html>
                                                                                  """)
  }

  aborted {
    emailext(to: "${env.EMAIL_ADDRESS}", subject: "${env.JOB_NAME} - Build #${env.BUILD_NUMBER} - ABORTED!", body: """
                                                                <html>
                                                                  <header></header>
                                                                  <body>
                                                                    <img src="${env.JENKINS_URL}/static/c5c835c9/images/48x48/aborted.png" alt="aborted" width="48" height="48" style="float:left" />
                                                                    <h1>BUILD ABORTED</h1>
                                                                    <p>Project: UnityCI<br>
                                                                    Job: ${env.JOB_NAME}<br>
                                                                    Date of build: ${env.BUILD_TIMESTAMP}<br>
                                                                    Build duration: ${currentBuild.durationString}<br><br>
                                                                    Check console output <a href="${env.BUILD_URL}">here</a> to view the results.</p>
                                                                  </body>
                                                                </html>
                                                                                  """)
  }

  always {
    node(env.AGENT_PREFIX) {
      sh(script: 'rm -rf $LOCAL_REPOSITORY/bin/**', label: 'Post repository cleanup')
    }

  }

}
options {
  skipDefaultCheckout(true)
  parallelsAlwaysFailFast()
  disableConcurrentBuilds()
}
}
