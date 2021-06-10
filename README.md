# raspi-blazor-temperature-app
Simple WebApp which presents temperature data recorded from a raspi

# Run local build docker image on local machine
## Docker-Compose
* open powershell/cmd
* navigate to docker-compose folder in repo
* docker-compose up

## MiniKube and Helm
* open powershell/cmd
* navigate to repo
* minikube start
* minikube image load ettenauer/raspi.temperature.app.server:latest (image name)
* helm install local ./helm 
* helm get manifest dev (check created yaml)
* open new shell, here two options:
** minikube service dev-raspi-temp-app
** kubectl port-forward service/local-raspi-temp-app 80:80