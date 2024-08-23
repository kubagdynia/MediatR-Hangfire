node('DOTNET8'){
	stage('SCM'){
		checkout([$class: 'GitSCM', branches: [[name: '*/main']], doGenerateSubmoduleConfigurations: false, extensions: [], submoduleCfg: [], userRemoteConfigs: [[url: 'https://github.com/kubagdynia/MediatR-Hangfire']]])
	}
	stage('Restore'){
		sh 'dotnet restore'
	}
	stage('Build'){
		try{
		sh 'dotnet build --no-restore'
		}finally{
			archiveArtifacts artifacts: 'MediatRTest.Api/*.*'
		}
	}
	stage('Test'){
		sh 'dotnet test --no-build --verbosity normal'
	}
	stage('Package'){
		echo 'Zip it up'
	}
	stage('Deploy'){
		echo 'Push to deployment'
	}
	stage('Archive'){
		archiveArtifacts artifacts: 'MediatRTest.Api/*.*'
	}	
}